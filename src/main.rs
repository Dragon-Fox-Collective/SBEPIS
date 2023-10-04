use bevy::prelude::*;
use bevy_xpbd_3d::prelude::*;

fn main()
{
	App::new()
		.add_plugins((DefaultPlugins, PhysicsPlugins::default()))
		.insert_resource(Gravity(Vec3::ZERO))
		.add_systems(Startup, setup)
		.run();
}

#[derive(Component)]
pub struct GravitationalField {
	gravity: f64
}

#[derive(Component)]
pub struct AffectedByGravity { }

fn setup(
	mut commands: Commands,
	mut meshes: ResMut<Assets<Mesh>>,
	mut materials: ResMut<Assets<StandardMaterial>>,
) {
	let mut spawn_planet = |
		position: Vec3,
		radius: f32,
	| {
		commands.spawn((
			PbrBundle {
				mesh: meshes.add(Mesh::from(shape::UVSphere { radius, sectors: 16, stacks: 16 })),
				material: materials.add(Color::rgb(0.3, 0.5, 0.3).into()),
				..default()
			},
			RigidBody::Static,
			Collider::ball(radius),
			Position(position),
		));
	};

	spawn_planet(Vec3::new(0.0, -6.0, 0.0), 6.0);

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