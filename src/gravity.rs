use bevy::prelude::*;
use bevy_xpbd_3d::{prelude::*, PhysicsSchedule, PhysicsStepSet};

pub struct GravityPlugin;

impl Plugin for GravityPlugin
{
	fn build(&self, app: &mut App) {
		app
			.insert_resource(Gravity(Vec3::ZERO))
			.register_type::<GravityPriority>()
			.register_type::<GravityPoint>()
			;
		
		app.get_schedule_mut(PhysicsSchedule)
			.expect("add PhysicsSchedule first")
			.add_systems((
				gravity::<GravityPoint>,
			).chain().before(PhysicsStepSet::Substeps));
	}
}

#[derive(Component, Reflect)]
pub struct GravityPriority(pub u32);

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

#[derive(Component)]
pub struct AffectedByGravity;

#[derive(Bundle)]
pub struct GravityRigidbodyBundle
{
	pub gravity: AffectedByGravity,
	pub rigidbody: RigidBody,
	pub external_force: ExternalForce,
}

impl Default for GravityRigidbodyBundle
{
	fn default() -> Self {
		GravityRigidbodyBundle
		{
			gravity: AffectedByGravity,
			rigidbody: RigidBody::Dynamic,
			external_force: ExternalForce::new(Vec3::ZERO).with_persistence(false),
		}
	}
}

fn gravity<T>(
	mut rigidbodies: Query<(&Position, &Mass, &mut ExternalForce), With<AffectedByGravity>>,
	gravity_fields: Query<(&Position, &Rotation, &GravityPriority, &T)>,
)
	where T : Component + GravitationalField
{
	for (position, mass, mut force) in &mut rigidbodies {
		for (field_position, field_rotation, gravity_priority, gravity_field) in &gravity_fields
		{
			let local_position = global_position_to_local(position.0, field_position.0, field_rotation.0);
			let local_acceleration = gravity_field.get_acceleration_at(local_position);
			let global_acceleration = local_direction_to_global(local_acceleration, field_rotation.0);
			force.apply_force(mass.0 * global_acceleration);
		}
	}
}

fn global_position_to_local(position: Vec3, reference_position: Vec3, reference_rotation: Quat) -> Vec3
{
	reference_rotation.mul_vec3(position - reference_position)
}

fn local_direction_to_global(direction: Vec3, reference_rotation: Quat) -> Vec3
{
	reference_rotation.inverse().mul_vec3(direction)
}