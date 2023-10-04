use bevy::prelude::*;
use bevy_xpbd_3d::{prelude::*, PhysicsSchedule, PhysicsStepSet};

pub struct GravityPlugin;

impl Plugin for GravityPlugin
{
	fn build(&self, app: &mut App) {
		app.insert_resource(Gravity(Vec3::ZERO));
		
		app.get_schedule_mut(PhysicsSchedule)
			.expect("add PhysicsSchedule first")
			.add_systems(gravity.before(PhysicsStepSet::Substeps));
	}
}


#[derive(Component)]
pub struct GravitationalField
{
	pub gravity: f32,
}

#[derive(Component)]
pub struct AffectedByGravity;

fn gravity(
	mut rigidbodies: Query<(&Position, &Mass, &mut ExternalForce), With<AffectedByGravity>>,
	gravity_fields: Query<(&Position, &GravitationalField)>,
)
{
	for (position, mass, mut force) in &mut rigidbodies {
		for (planet_position, gravity_field) in &gravity_fields {
			force.apply_force(mass.0 * gravity_field.gravity / planet_position.distance_squared(position.0) * (planet_position.0 - position.0).normalize());
		}
	}
}