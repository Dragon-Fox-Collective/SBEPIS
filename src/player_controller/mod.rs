use bevy::prelude::*;
use bevy_xpbd_3d::prelude::*;

use crate::gravity::AffectedByGravity;

pub struct PlayerControllerPlugin;

impl Plugin for PlayerControllerPlugin
{
	fn build(&self, app: &mut App) {
		app
			.add_systems(Startup, (
				setup,
			));
	}
}

fn setup(
	mut commands: Commands,
	mut meshes: ResMut<Assets<Mesh>>,
	mut materials: ResMut<Assets<StandardMaterial>>,
)
{
	let body = commands.spawn((
		Name::new("Player Body"),
		PbrBundle {
			mesh: meshes.add(Mesh::from(shape::Capsule{ radius: 0.5, rings: 1, depth: 1.0, latitudes: 8, longitudes: 16, uv_profile: shape::CapsuleUvProfile::Fixed })),
			material: materials.add(Color::WHITE.into()),
			..default()
		},
		RigidBody::Dynamic,
		LockedAxes::ROTATION_LOCKED,
		Position(Vec3::new(5.0, 10.0, 0.0)),
		Collider::capsule(1.0, 0.5),
		ExternalForce::new(Vec3::ZERO).with_persistence(false),
		AffectedByGravity,
	)).id();

	commands.spawn((
		Name::new("Main Camera"),
		Camera3dBundle {
			camera: Camera {
				is_active: false,
				..default()
			},
			..default()
		},
	)).set_parent(body);
}