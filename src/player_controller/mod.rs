use bevy::{prelude::*, input::common_conditions::input_just_pressed};
use bevy_xpbd_3d::prelude::*;

use crate::{gravity::AffectedByGravity, OverviewCamera, gridbox_material};

pub struct PlayerControllerPlugin;

impl Plugin for PlayerControllerPlugin
{
	fn build(&self, app: &mut App) {
		app
			.add_systems(Startup, (
				setup,
			))
			.add_systems(Update, (
				toggle_camera.run_if(input_just_pressed(KeyCode::Tab)),
			));
	}
}

#[derive(Component)]
pub struct PlayerCamera;

fn setup(
	mut commands: Commands,
	mut meshes: ResMut<Assets<Mesh>>,
	mut materials: ResMut<Assets<StandardMaterial>>,
	asset_server: Res<AssetServer>,
)
{
	let body = commands.spawn((
		Name::new("Player Body"),
		PbrBundle {
			mesh: meshes.add(Mesh::from(shape::Capsule { radius: 0.5, rings: 1, depth: 1.0, latitudes: 8, longitudes: 16, uv_profile: shape::CapsuleUvProfile::Fixed })),
			material: gridbox_material("white", &mut materials, &asset_server),
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
		Name::new("Player Camera"),
		Camera3dBundle {
			camera: Camera {
				is_active: false,
				..default()
			},
			..default()
		},
		PlayerCamera,
	)).set_parent(body);
}

pub fn toggle_camera(
	mut overview_camera: Query<&mut Camera, (With<OverviewCamera>, Without<PlayerCamera>)>,
	mut player_camera: Query<&mut Camera, (With<PlayerCamera>, Without<OverviewCamera>)>,
)
{
	let mut overview_camera = overview_camera.single_mut();
	overview_camera.is_active = !overview_camera.is_active;

	let mut player_camera = player_camera.single_mut();
	player_camera.is_active = !player_camera.is_active;
}