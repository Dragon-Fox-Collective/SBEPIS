use bevy::prelude::*;
use bevy_trait_query::{One, queryable, RegisterExt};
use bevy_xpbd_3d::{prelude::*, SubstepSchedule, SubstepSet};
use itertools::Itertools;

use crate::util::{TransformEx, IterElements};

pub struct GravityPlugin;

impl Plugin for GravityPlugin
{
	fn build(&self, app: &mut App)
	{
		app
			.insert_resource(Gravity(Vec3::ZERO))
			.register_type::<GravityPriority>()
			.register_type::<GravityPoint>()
			.register_component_as::<dyn GravitationalField, GravityPoint>()
			;
		
		app.get_schedule_mut(SubstepSchedule)
			.expect("add SubstepSchedule first")
			.add_systems((
				calculate_gravity,
				apply_gravity,
			).chain().in_set(SubstepSet::SolveUserConstraints));
	}
}

#[derive(Component, Reflect)]
pub struct GravityPriority(pub u32);

#[queryable]
pub trait GravitationalField
{
	/// How much this acceleration affects an object, but also how much this priority should override lower priorities.
	fn get_priority_factor_at(&self, local_position: Vec3) -> Vec3;
	fn get_acceleration_at(&self, local_position: Vec3) -> Vec3;
}

#[derive(Component, Reflect)]
pub struct GravityPoint
{
	pub standard_radius: f32,
	pub acceleration_at_radius: f32,
}

impl GravitationalField for GravityPoint
{
	/// Points affect *all* objects, so they will always override lower priorities.
	fn get_priority_factor_at(&self, _local_position: Vec3) -> Vec3
	{
		Vec3::ONE
	}

	fn get_acceleration_at(&self, local_position: Vec3) -> Vec3
	{
		let mass = self.acceleration_at_radius * self.standard_radius * self.standard_radius;
		mass / -local_position.length_squared() * local_position.normalize()
	}
}

#[derive(Component, Default)]
pub struct AffectedByGravity
{
	pub acceleration: Vec3,
	pub up: Vec3,
}

#[derive(Bundle)]
pub struct GravityRigidbodyBundle
{
	pub gravity: AffectedByGravity,
	pub rigidbody: RigidBody,
}

impl Default for GravityRigidbodyBundle
{
	fn default() -> Self {
		GravityRigidbodyBundle
		{
			gravity: AffectedByGravity::default(),
			rigidbody: RigidBody::Dynamic,
		}
	}
}

pub fn calculate_gravity(
	mut rigidbodies: Query<(&Position, &mut AffectedByGravity)>,
	gravity_fields: Query<(&GlobalTransform, &GravityPriority, One<&dyn GravitationalField>)>,
)
{
	let field_groups: Vec<Vec<(&GlobalTransform, &GravityPriority, &dyn GravitationalField)>> = gravity_fields
		.into_iter()
		.sorted_by_cached_key(|(_, priority, _)| priority.0)
		.group_by(|(_, priority, _)| priority.0)
		.into_iter()
		.map(|(_, group)| group.collect())
		.collect();

	for (position, mut gravity) in &mut rigidbodies {
		let acceleration = field_groups.iter().fold(Vec3::ZERO, |lower_priority_acceleration, group: &Vec<(&GlobalTransform, &GravityPriority, &dyn GravitationalField)>| {
				let local_positions: Vec<Vec3> = group.iter().map(|(transform, _, _)| transform.inverse_transform_point(position.0)).collect();
				let priority_factors: Vec<f32> = group.iter().zip(&local_positions).map(|((_, _, field), local_position)| field.get_priority_factor_at(*local_position).iter_elements().product()).collect();
				let accelerations: Vec<Vec3> = group.iter().zip(&local_positions).map(|((transform, _, field), local_position)| transform.transform_vector3(field.get_acceleration_at(*local_position))).collect();
				let accelerations: Vec<Vec3> = accelerations.into_iter().zip(&priority_factors).map(|(acceleration, priority_factor)| acceleration * *priority_factor).collect();
				Vec3::lerp(
					lower_priority_acceleration,
					accelerations.iter().sum(),
					priority_factors.iter().sum())
			});
		
		gravity.acceleration = acceleration;
		gravity.up = -acceleration.normalize();
	}
}

// public static Vector3 GetGravityAt(Vector3 point, IEnumerable<MassiveBody> massiveBodies) => massiveBodies
// 			.Distinct()
// 			.Where(body => body)
// 			.GroupBy(body => body.priority)
// 			.OrderBy(group => group.Key)
// 			.Aggregate(Vector3.zero, (lowerProrityGravity, group) =>
// 			{
// 				List<Vector3> localCentersOfMass = group.Select(body => body.transform.InverseTransformPoint(point)).ToList();
// 				List<float> priorities = group.Zip(localCentersOfMass, (body, localCenterOfMass) => body.GetPriority(localCenterOfMass).Aggregate(1, (product, x) => product * x)).ToList();
// 				return Vector3.Lerp(lowerProrityGravity,
// 					group.Zip(localCentersOfMass, (body, localCenterOfMass) => body.transform.TransformDirection(body.GetGravity(localCenterOfMass)))
// 						.Zip(priorities, (gravity, priority) => Vector3.LerpUnclamped(Vector3.zero, gravity, priority)).Sum(),
// 					priorities.Sum());
// 			});

pub fn apply_gravity(
	mut rigidbodies: Query<(&mut Position, &AffectedByGravity)>,
	delta_time: Res<SubDeltaTime>,
)
{
	for (mut position, gravity) in &mut rigidbodies {
		position.0 += 0.5 * gravity.acceleration * delta_time.0 * delta_time.0;
	}
}