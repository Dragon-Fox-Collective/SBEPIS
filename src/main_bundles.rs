use std::f32::consts::{PI, TAU};

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
	rotation: Rotation,
	gravity: GravityPoint,
	gravity_priority: GravityPriority,
}

impl PlanetBundle
{
	pub fn new(
		position: Vec3,
		radius: f32,
		gravity: f32,
		meshes: &mut Assets<Mesh>,
		material: Handle<StandardMaterial>,
	) -> Self
	{
		let mut mesh = Mesh::try_from(shape::Icosphere { radius, subdivisions: 6 }).unwrap();
		let uvs = mesh.attribute_mut(Mesh::ATTRIBUTE_UV_0).unwrap();
		match uvs {
			bevy::render::mesh::VertexAttributeValues::Float32x2(values) =>
			{
				for uv in values
				{
					uv[0] *= radius * TAU;
					uv[1] *= radius * PI;
				}
			},
			_ => panic!()
		}

		let collider = Collider::trimesh_from_bevy_mesh(&mesh).expect("couldn't make a planet collider");
		PlanetBundle
		{
			pbr: PbrBundle
			{
				mesh: meshes.add(mesh),
				material,
				..default()
			},
			rigidbody: RigidBody::Static,
			collider,
			position: Position(position),
			rotation: Rotation(Quat::from_axis_angle(Vec3::X, PI/2.)),
			gravity: GravityPoint { standard_radius: radius, acceleration_at_radius: gravity },
			gravity_priority: GravityPriority(0),
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
		mesh: Handle<Mesh>,
		material: Handle<StandardMaterial>,
	) -> Self
	{
		BoxBundle
		{
			pbr: PbrBundle {
				mesh,
				material,
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
