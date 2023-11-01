use bevy::prelude::*;
use bevy_xpbd_3d::{prelude::{Rotation, AngularVelocity}, math::PI};

#[derive(Component)]
pub struct PlayerCamera;

#[derive(Component)]
pub struct Pitch(pub f32);

#[derive(Component)]
pub struct PlayerBody;

/// Probably in radians per pixel?
#[derive(Resource)]
pub struct MouseSensitivity(pub f32);

pub fn rotate_camera_and_body(
	In(delta): In<Vec2>,
	sensitivity: Res<MouseSensitivity>,
	mut player_camera: Query<(&mut Transform, &mut Pitch, &Camera), With<PlayerCamera>>,
	mut player_body: Query<(&mut Rotation, &mut AngularVelocity), With<PlayerBody>>,
)
{
	{
		let (mut camera_transform, mut camera_pitch, camera) = player_camera.single_mut();
		if !camera.is_active { return; }

		camera_pitch.0 += delta.y * sensitivity.0;
		camera_pitch.0 = camera_pitch.0.clamp(-PI, PI);
		camera_transform.rotation = Quat::from_rotation_x(-camera_pitch.0);
	}

	{
		let (mut body_rotation, mut body_angular_velocity) = player_body.single_mut();

		body_rotation.0 *= Quat::from_rotation_y(-delta.x * sensitivity.0);

		body_angular_velocity.0.y = 0.0; // Football imparts torque on body and LockedAxes doesn't work
	}
}