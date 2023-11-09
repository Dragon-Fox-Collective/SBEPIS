use bevy::prelude::*;
use bevy_xpbd_3d::prelude::*;

use super::PlayerBody;

#[derive(Component)]
pub struct Football
{
	pub radius: f32,
}

#[derive(Component)]
pub struct FootballJoint
{
	pub rest_local_position: Vec3,
	pub jump_local_position: Vec3,
	pub jump_speed: f32,
}

#[derive(Resource)]
pub struct PlayerSpeed
{
	pub speed: f32,
	pub sprint_modifier: f32,
	pub air_acceleration: f32,
}

pub fn axes_to_ground_velocity(
	In(axes_input): In<Vec2>,
	input: Res<Input<KeyCode>>,
	speed: Res<PlayerSpeed>,
) -> Vec2
{
	axes_input * speed.speed * if input.pressed(KeyCode::ShiftLeft) { speed.sprint_modifier } else { 1.0 }
}

pub fn spin_football(
	In(input_velocity): In<Vec2>,
	mut football: Query<(&mut Rotation, &PreviousRotation, &Football), Without<PlayerBody>>,
	player_body: Query<&Rotation, With<PlayerBody>>,
	time: Res<Time>,
)
{
	let (mut rotation, prev_rotation, football) = football.single_mut();
	let body_rotation = player_body.single();
	let delta = body_rotation.0 * Vec3::new(-input_velocity.y, 0., -input_velocity.x) / football.radius * time.delta_seconds();
	rotation.0 = Quat::from_scaled_axis(delta) * prev_rotation.0.0;
}

pub fn jump(
	In(is_jumping): In<bool>,
	mut football_joint: Query<(&mut SphericalJoint, &FootballJoint)>,
	time: Res<Time>,
)
{
	let (mut joint, joint_params) = football_joint.single_mut();
	let target = if is_jumping { joint_params.jump_local_position } else { joint_params.rest_local_position };
	joint.local_anchor1 = joint.local_anchor1 + (target - joint.local_anchor1).clamp_length_max(time.delta_seconds() * joint_params.jump_speed);
}