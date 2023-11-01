use bevy::prelude::*;
use bevy_xpbd_3d::prelude::*;

use super::PlayerCamera;

#[derive(Component)]
pub struct Football;

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
}

pub fn axes_to_football_velocity(
	In(axes_input): In<Vec2>,
	input: Res<Input<KeyCode>>,
	speed: Res<PlayerSpeed>,
) -> Vec2
{
	axes_input * speed.speed * if input.pressed(KeyCode::ShiftLeft) { speed.sprint_modifier } else { 1.0 }
}

pub fn spin_football(
	In(input_velocity): In<Vec2>,
	mut football: Query<&mut AngularVelocity, With<Football>>,
	player_camera: Query<&GlobalTransform, With<PlayerCamera>>
)
{
	let mut football_velocity = football.single_mut();
	let camera_transform = player_camera.single();
	football_velocity.0 = camera_transform.compute_transform().rotation * Vec3::new(-input_velocity.y, 0., -input_velocity.x);
}

pub fn jump(
	In(is_jumping): In<bool>,
	mut football_joint: Query<(&mut SphericalJoint, &FootballJoint)>,
	delta_time: Res<SubDeltaTime>,
)
{
	let (mut joint, joint_params) = football_joint.single_mut();
	let target = if is_jumping { joint_params.jump_local_position } else { joint_params.rest_local_position };
	joint.local_anchor1 = joint.local_anchor1 + (target - joint.local_anchor1).clamp_length_max(delta_time.0 * joint_params.jump_speed);
}