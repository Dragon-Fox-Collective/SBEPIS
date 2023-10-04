use bevy::prelude::*;
use bevy_xpbd_3d::{prelude::*, PhysicsSchedule, PhysicsStepSet};

fn main()
{
	let mut app = App::new();

	app
		.add_plugins((DefaultPlugins, PhysicsPlugins::default()))
		.insert_resource(Gravity(Vec3::ZERO))
		.insert_resource(FixedTime::new_from_secs(1.0 / 60.0))
		.add_systems(Startup, setup);

	app.get_schedule_mut(PhysicsSchedule)
		.expect("add PhysicsSchedule first")
		.add_systems(gravity.before(PhysicsStepSet::Substeps));

	app.run();
}

#[derive(Component)]
pub struct GravitationalField
{
	gravity: f32,
}

#[derive(Component)]
pub struct AffectedByGravity;


#[derive(Bundle)]
pub struct PlanetBundle
{
	pbr: PbrBundle,
	rigidbody: RigidBody,
	collider: Collider,
	position: Position,
	gravity: GravitationalField,
}

impl PlanetBundle {
	fn new(
		position: Vec3,
		radius: f32,
		meshes: &mut Assets<Mesh>,
		materials: &mut Assets<StandardMaterial>,
	) -> PlanetBundle
	{
		PlanetBundle
		{
			pbr: PbrBundle
			{
				mesh: meshes.add(Mesh::from(shape::UVSphere { radius, sectors: 16, stacks: 16 })),
				material: materials.add(Color::rgb(0.3, 0.5, 0.3).into()),
				..default()
			},
			rigidbody: RigidBody::Static,
			collider: Collider::ball(radius),
			position: Position(position),
			gravity: GravitationalField { gravity: 100.0 },
		}
	}
}


fn setup(
	mut commands: Commands,
	mut meshes: ResMut<Assets<Mesh>>,
	mut materials: ResMut<Assets<StandardMaterial>>,
)
{
	commands.spawn(PlanetBundle::new(Vec3::Y * -2.0, 2.0, &mut meshes, &mut materials));

	commands.spawn((
		PbrBundle {
			mesh: meshes.add(Mesh::from(shape::Cube { size: 1.0 })),
			material: materials.add(Color::rgb(0.8, 0.7, 0.6).into()),
			..default()
		},
		RigidBody::Dynamic,
		Position(Vec3::Y * 4.0),
		AngularVelocity(Vec3::new(2.5, 3.4, 1.6)),
		ExternalForce::new(Vec3::ZERO).with_persistence(false),
		Collider::cuboid(1.0, 1.0, 1.0),
		AffectedByGravity,
	));

	commands.spawn(PointLightBundle {
		point_light: PointLight {
			intensity: 1500.0,
			shadows_enabled: true,
			..default()
		},
		transform: Transform::from_xyz(4.0, 8.0, 4.0),
		..default()
	});

	commands.spawn(Camera3dBundle {
		transform: Transform::from_xyz(-4.0, 6.5, 8.0).looking_at(Vec3::ZERO, Vec3::Y),
		..default()
	});
}

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