mod gravity;
mod main_bundles;
mod ui;

use bevy::prelude::*;
use bevy_mod_picking::prelude::RaycastPickCamera;
use bevy_xpbd_3d::prelude::*;
use gravity::*;
use main_bundles::*;
use ui::{UiPlugin, EditorCamera};

fn main()
{
	App::new()
		.add_plugins((
			DefaultPlugins,
			PhysicsPlugins::default(),
			UiPlugin,
			GravityPlugin,
		))
		.insert_resource(FixedTime::new_from_secs(1.0 / 60.0))
		.add_systems(Startup, setup)
		.run();
}

fn setup(
	mut commands: Commands,
	mut meshes: ResMut<Assets<Mesh>>,
	mut materials: ResMut<Assets<StandardMaterial>>,
)
{
	commands.spawn((Name::new("Planet"), PlanetBundle::new(Vec3::Y * -2.0, 2.0, 10.0, &mut meshes, &mut materials)));

	commands.spawn((Name::new("Cube 1"), BoxBundle::new(Vec3::new(0.0, 4.0, 0.0), &mut meshes, &mut materials)));
	commands.spawn((Name::new("Cube 2"), BoxBundle::new(Vec3::new(0.5, 5.5, 0.0), &mut meshes, &mut materials)));
	commands.spawn((Name::new("Cube 3"), BoxBundle::new(Vec3::new(-0.5, 7.0, 0.0), &mut meshes, &mut materials)));

	commands.spawn(PointLightBundle {
		point_light: PointLight {
			intensity: 1500.0,
			shadows_enabled: true,
			..default()
		},
		transform: Transform::from_xyz(4.0, 8.0, 4.0),
		..default()
	});

	commands.spawn((
		Camera3dBundle {
			transform: Transform::from_xyz(-4.0, 6.5, 8.0).looking_at(Vec3::ZERO, Vec3::Y),
			..default()
		},
		EditorCamera,
		RaycastPickCamera::default(),
	));
}