use bevy::prelude::*;
use bevy_trait_query::{One, queryable, RegisterExt};
use bevy_xpbd_3d::{prelude::*, SubstepSchedule, SubstepSet};

use crate::util::TransformEx;

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
	// TODO: don't make this n^2
	for (position, mut gravity) in &mut rigidbodies {
		for (field_transform, gravity_priority, gravity_field) in &gravity_fields
		{
			let local_position = field_transform.transform_point(position.0);
			let local_acceleration = gravity_field.get_acceleration_at(local_position);
			let global_acceleration = field_transform.inverse_transform_vector3(local_acceleration);
			gravity.acceleration = global_acceleration;
			gravity.up = -global_acceleration.normalize();
		}
	}
}

pub fn apply_gravity(
	mut rigidbodies: Query<(&mut Position, &AffectedByGravity)>,
	delta_time: Res<SubDeltaTime>,
)
{
	for (mut position, gravity) in &mut rigidbodies {
		position.0 += 0.5 * gravity.acceleration * delta_time.0 * delta_time.0;
	}
}