mod football;
mod orientation;

use self::football::*;
use self::orientation::*;

use bevy::prelude::*;
use bevy_xpbd_3d::{prelude::*, SubstepSchedule, SubstepSet};

use crate::gravity::GravityRigidbodyBundle;
use crate::gravity::calculate_gravity;
use crate::gridbox_material;

pub struct PlayerControllerPlugin;

impl Plugin for PlayerControllerPlugin
{
	fn build(&self, app: &mut App) {
		app
			.add_systems(Startup, (
				setup,
			))
			.add_systems(Update, (
				compose_axes.pipe(spin_football),
			))
			;
		
		app.get_schedule_mut(SubstepSchedule)
			.expect("add SubstepSchedule first")
			.add_systems((
				orient.after(calculate_gravity),
			).in_set(SubstepSet::SolveUserConstraints));
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
	let position = Vec3::new(5.0, 10.0, 0.0);
	let football_local_position = Vec3::NEG_Y * 1.25;

	let body = commands.spawn((
		Name::new("Player Body"),
		PbrBundle {
			mesh: meshes.add(Mesh::from(shape::Capsule { radius: 0.25, rings: 1, depth: 1.0, latitudes: 8, longitudes: 16, uv_profile: shape::CapsuleUvProfile::Fixed })),
			material: gridbox_material("white", &mut materials, &asset_server),
			..default()
		},
		GravityRigidbodyBundle::default(),
		Position(position),
		Collider::capsule(1.0, 0.25),
		GravityOrientation,
	)).id();

	let football = commands.spawn((
		Name::new("Football"),
		Football,
		PbrBundle {
			mesh: meshes.add(Mesh::try_from(shape::Icosphere { radius: 0.5, subdivisions: 2 }).unwrap()),
			material: gridbox_material("grey4", &mut materials, &asset_server),
			..default()
		},
		GravityRigidbodyBundle::default(),
		Position(position + football_local_position),
		Collider::ball(0.5),
		AngularVelocity::default(),
		Friction::new(100.0).with_combine_rule(CoefficientCombine::Multiply),
	)).id();

	commands.spawn((Name::new("Football Joint"), SphericalJoint::new(body, football).with_local_anchor_1(football_local_position)));

	commands.spawn((
		Name::new("Player Camera"),
		Camera3dBundle {
			camera: Camera {
				..default()
			},
			..default()
		},
		PlayerCamera,
	)).set_parent(body);
}