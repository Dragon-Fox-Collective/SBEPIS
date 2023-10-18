use std::f32::consts::PI;

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
	gravity_priority: GravityPriority,
}

// Stolen from bevy_render
fn planet(sphere: shape::UVSphere) -> Mesh {
	// Largely inspired from http://www.songho.ca/opengl/gl_sphere.html

	let sectors = sphere.sectors as f32;
	let stacks = sphere.stacks as f32;
	let length_inv = 1. / sphere.radius;
	let sector_step = 2. * PI / sectors;
	let stack_step = PI / stacks;
	let uv_correction = sphere.radius * PI;

	let mut vertices: Vec<[f32; 3]> = Vec::with_capacity(sphere.stacks * sphere.sectors);
	let mut normals: Vec<[f32; 3]> = Vec::with_capacity(sphere.stacks * sphere.sectors);
	let mut uvs: Vec<[f32; 2]> = Vec::with_capacity(sphere.stacks * sphere.sectors);
	let mut indices: Vec<u32> = Vec::with_capacity(sphere.stacks * sphere.sectors * 2 * 3);

	for i in 0..sphere.stacks + 1 {
		let stack_angle = PI / 2. - (i as f32) * stack_step;
		let xy = sphere.radius * stack_angle.cos();
		let z = sphere.radius * stack_angle.sin();

		for j in 0..sphere.sectors + 1 {
			let sector_angle = (j as f32) * sector_step;
			let x = xy * sector_angle.cos();
			let y = xy * sector_angle.sin();

			vertices.push([x, y, z]);
			normals.push([x * length_inv, y * length_inv, z * length_inv]);
			uvs.push([(j as f32) / sectors * uv_correction * 2., (i as f32) / stacks * uv_correction]);
		}
	}

	// indices
	//  k1--k1+1
	//  |  / |
	//  | /  |
	//  k2--k2+1
	for i in 0..sphere.stacks {
		let mut k1 = i * (sphere.sectors + 1);
		let mut k2 = k1 + sphere.sectors + 1;
		for _j in 0..sphere.sectors {
			if i != 0 {
				indices.push(k1 as u32);
				indices.push(k2 as u32);
				indices.push((k1 + 1) as u32);
			}
			if i != sphere.stacks - 1 {
				indices.push((k1 + 1) as u32);
				indices.push(k2 as u32);
				indices.push((k2 + 1) as u32);
			}
			k1 += 1;
			k2 += 1;
		}
	}

	let mut mesh = Mesh::new(bevy::render::render_resource::PrimitiveTopology::TriangleList);
	mesh.set_indices(Some(bevy::render::mesh::Indices::U32(indices)));
	mesh.insert_attribute(Mesh::ATTRIBUTE_POSITION, vertices);
	mesh.insert_attribute(Mesh::ATTRIBUTE_NORMAL, normals);
	mesh.insert_attribute(Mesh::ATTRIBUTE_UV_0, uvs);
	mesh
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
		let mesh = planet(shape::UVSphere { radius, sectors: 32, stacks: 18 });
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
