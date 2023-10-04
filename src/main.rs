use bevy::prelude::*;
use bevy_xpbd_3d::{prelude::*, PhysicsSchedule, PhysicsStepSet};

fn main()
{
	let mut app = App::new();

	app
		.add_plugins(DefaultPlugins)
		.add_plugins(PhysicsPlugins::default())
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

impl PlanetBundle
{
	fn new(
		position: Vec3,
		radius: f32,
		meshes: &mut Assets<Mesh>,
		materials: &mut Assets<StandardMaterial>,
	) -> Self
	{
		let mesh = Mesh::from(shape::UVSphere { radius, sectors: 16, stacks: 16 });
		let collider = Collider::trimesh_from_bevy_mesh(&mesh).expect("couldn't make a planet collider");
		PlanetBundle
		{
			pbr: PbrBundle
			{
				mesh: meshes.add(mesh),
				material: materials.add(Color::rgb(0.3, 0.5, 0.3).into()),
				..default()
			},
			rigidbody: RigidBody::Static,
			collider,
			position: Position(position),
			gravity: GravitationalField { gravity: 100.0 },
		}
	}
}


#[derive(Bundle)]
pub struct BoxBundle
{
	pbr: PbrBundle,
	rigidbody: RigidBody,
	position: Position,
	angular_velocity: AngularVelocity,
	force: ExternalForce,
	collider: Collider,
	gravity: AffectedByGravity,
}

impl BoxBundle
{
	fn new(
		position: Vec3,
		meshes: &mut Assets<Mesh>,
		materials: &mut Assets<StandardMaterial>,
	) -> Self
	{
		BoxBundle
		{
			pbr: PbrBundle {
				mesh: meshes.add(Mesh::from(shape::Cube { size: 1.0 })),
				material: materials.add(Color::rgb(0.8, 0.7, 0.6).into()),
				..default()
			},
			rigidbody: RigidBody::Dynamic,
			position: Position(position),
			angular_velocity: AngularVelocity(Vec3::new(2.5, 3.4, 1.6)),
			force: ExternalForce::new(Vec3::ZERO).with_persistence(false),
			collider: Collider::cuboid(1.0, 1.0, 1.0),
			gravity: AffectedByGravity,
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

	commands.spawn(BoxBundle::new(Vec3::new(0.0, 4.0, 0.0), &mut meshes, &mut materials));
	commands.spawn(BoxBundle::new(Vec3::new(0.5, 5.5, 0.0), &mut meshes, &mut materials));
	commands.spawn(BoxBundle::new(Vec3::new(-0.5, 7.0, 0.0), &mut meshes, &mut materials));

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