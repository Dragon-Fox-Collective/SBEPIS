mod football;
mod orientation;
mod camera_controls;
mod air_movement;

use self::football::*;
use self::orientation::*;
use self::camera_controls::*;
use self::air_movement::*;

pub use self::camera_controls::{PlayerCamera, PlayerBody, MouseSensitivity};

use bevy::input::common_conditions::input_pressed;
use bevy::prelude::*;
use bevy_xpbd_3d::math::PI;
use bevy_xpbd_3d::{prelude::*, SubstepSchedule, SubstepSet};

use crate::gravity::GravityRigidbodyBundle;
use crate::gravity::apply_gravity;
use crate::gridbox_material;
use crate::util::compose_mouse_delta_axes;
use crate::util::compose_wasd_axes;

pub struct PlayerControllerPlugin;
impl Plugin for PlayerControllerPlugin
{
	fn build(&self, app: &mut App) {
		app
			.insert_resource(MouseSensitivity(0.003))
			.insert_resource(PlayerSpeed { speed: 5.0, sprint_modifier: 2.0, air_acceleration: 20.0 })
			.add_systems(Startup, (
				setup,
			))
			.add_systems(Update, (
				compose_mouse_delta_axes.pipe(rotate_camera_and_body),
			))
			;
		
		app.get_schedule_mut(SubstepSchedule)
			.expect("add SubstepSchedule first")
			.add_systems((
				orient.after(apply_gravity),
				input_pressed(KeyCode::Space).pipe(jump),
				compose_wasd_axes.pipe(axes_to_ground_velocity).pipe(spin_football).after(orient),
				compose_wasd_axes.pipe(axes_to_air_acceleration).pipe(air_strafe).run_if(not(is_football_on_ground)).after(spin_football),
			).in_set(SubstepSet::SolveUserConstraints));
	}
}

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
		PlayerBody,
		Rotation::default(),
		AngularVelocity::default(),
	)).id();

	let football = commands.spawn((
		Name::new("Football"),
		Football { radius: 0.5 },
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

	commands.spawn((
		Name::new("Football Ground Caster"),
		RayCaster::new(football_local_position, Vec3::NEG_Y).with_solidness(false).with_max_time_of_impact(1.0).with_query_filter(SpatialQueryFilter::default().without_entities([body, football])),
		FootballGroundCaster,
	)).set_parent(body);

	commands.spawn((
		Name::new("Football Joint"),
		SphericalJoint::new(body, football).with_local_anchor_1(football_local_position),
		FootballJoint {
			rest_local_position: football_local_position,
			jump_local_position: football_local_position + Vec3::NEG_Y * 0.25,
			jump_speed: 10.0,
		},
	));

	commands.spawn((
		Name::new("Player Camera"),
		Camera3dBundle {
			transform: Transform::from_translation(Vec3::Y * 0.5),
			projection: Projection::Perspective(PerspectiveProjection {
				fov: 70.0 / 180. * PI,
				..default()
			}),
			..default()
		},
		PlayerCamera,
		Pitch(0.0),
	)).set_parent(body);
}