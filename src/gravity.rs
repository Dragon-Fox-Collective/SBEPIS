use bevy::prelude::*;
use bevy_trait_query::{One, queryable, RegisterExt};
use bevy_xpbd_3d::{prelude::*, PhysicsSchedule, PhysicsStepSet};

pub struct GravityPlugin;

impl Plugin for GravityPlugin
{
	fn build(&self, app: &mut App) {
		app
			.insert_resource(Gravity(Vec3::ZERO))
			.register_component_as::<dyn GravitationalField, GravityPoint>()
			;
		
		app.get_schedule_mut(PhysicsSchedule)
			.expect("add PhysicsSchedule first")
			.add_systems(gravity.before(PhysicsStepSet::Substeps));
	}
}

#[queryable]
pub trait GravitationalField
{
	/// How much this acceleration affects an object, but also how much this priority should override lower priorities.
	fn get_priority_factor_at(&self, local_position: Vec3) -> Vec3;
	fn get_acceleration_at(&self, local_position: Vec3) -> Vec3;
}

#[derive(Component)]
pub struct GravityPoint
{
	pub priority: i32,
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

fn gravity(
	mut rigidbodies: Query<(&Position, &Mass, &mut ExternalForce), With<AffectedByGravity>>,
	gravity_fields: Query<(&Position, &Rotation, One<&dyn GravitationalField>)>,
)
{
	for (position, mass, mut force) in &mut rigidbodies {
		for (field_position, field_rotation, gravity_field) in &gravity_fields {
			force.apply_force(mass.0 * gravity_field.get_acceleration_at(global_to_local(position.0, field_position.0, field_rotation.0)));
		}
	}
}

fn global_to_local(position: Vec3, reference_position: Vec3, reference_rotation: Quat) -> Vec3
{
	reference_rotation.mul_vec3(position - reference_position)
}