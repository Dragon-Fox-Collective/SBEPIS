use bevy::prelude::*;
use bevy_xpbd_3d::prelude::*;

fn main() {
	App::new()
		.add_plugins((DefaultPlugins, PhysicsPlugins::default()))
		.add_systems(Startup, setup)
		.run();
}

fn setup(
	mut commands: Commands,
	mut meshes: ResMut<Assets<Mesh>>,
	mut materials: ResMut<Assets<StandardMaterial>>,
) {
	// Plane
	commands.spawn((
		PbrBundle {
			mesh: meshes.add(Mesh::from(shape::Plane::from_size(8.0))),
			material: materials.add(Color::rgb(0.3, 0.5, 0.3).into()),
			..default()
		},
		RigidBody::Static,
		Collider::cuboid(8.0, 0.002, 8.0),
	));
	// Cube
	commands.spawn((
		PbrBundle {
			mesh: meshes.add(Mesh::from(shape::Cube { size: 1.0 })),
			material: materials.add(Color::rgb(0.8, 0.7, 0.6).into()),
			..default()
		},
		RigidBody::Dynamic,
		Position(Vec3::Y * 4.0),
		AngularVelocity(Vec3::new(2.5, 3.4, 1.6)),
		Collider::cuboid(1.0, 1.0, 1.0),
	));
	// Light
	commands.spawn(PointLightBundle {
		point_light: PointLight {
			intensity: 1500.0,
			shadows_enabled: true,
			..default()
		},
		transform: Transform::from_xyz(4.0, 8.0, 4.0),
		..default()
	});
	// Camera
	commands.spawn(Camera3dBundle {
		transform: Transform::from_xyz(-4.0, 6.5, 8.0).looking_at(Vec3::ZERO, Vec3::Y),
		..default()
	});
}
