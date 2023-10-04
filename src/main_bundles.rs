use bevy::prelude::*;
use bevy_xpbd_3d::prelude::*;
use super::gravity::*;

#[derive(Bundle)]
pub struct PlanetBundle
{
	pbr: PbrBundle,
	rigidbody: RigidBody,
	collider: Collider,
	position: Position,
	gravity: GravityPoint,
}

impl PlanetBundle
{
	pub fn new(
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
			gravity: GravityPoint { priority:0, gravity: 100.0 },
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
	pub fn new(
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
