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
	fn get_priority_factor_at(&self, position: Vec3) -> Vec3;
	fn get_acceleration_at(&self, position: Vec3, field_position: Vec3) -> Vec3;
}

#[derive(Component)]
pub struct GravityPoint
{
	pub priority: i32,
	pub gravity: f32,
}

impl GravitationalField for GravityPoint
{
	fn get_priority_factor_at(&self, _position: Vec3) -> Vec3
	{
		Vec3::ONE
	}

	fn get_acceleration_at(&self, position: Vec3, field_position: Vec3) -> Vec3
	{
		self.gravity / field_position.distance_squared(position) * (field_position - position).normalize()
	}
}

#[derive(Component)]
pub struct AffectedByGravity;

fn gravity(
	mut rigidbodies: Query<(&Position, &Mass, &mut ExternalForce), With<AffectedByGravity>>,
	gravity_fields: Query<(&Position, One<&dyn GravitationalField>)>,
)
{
	for (position, mass, mut force) in &mut rigidbodies {
		for (field_position, gravity_field) in &gravity_fields {
			force.apply_force(mass.0 * gravity_field.get_acceleration_at(position.0, field_position.0));
		}
	}
}