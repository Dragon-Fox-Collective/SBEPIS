use bevy::prelude::*;
use bevy_xpbd_3d::prelude::*;

use super::{PlayerBody, football::PlayerSpeed};

pub fn air_strafe(
	In(velocity): In<Vec2>,
	mut player_body: Query<(&mut Position, &Rotation), With<PlayerBody>>,
	delta_time: Res<SubDeltaTime>,
)
{
	let (mut position, rotation) = player_body.single_mut();
	let delta = rotation.0 * Vec3::new(velocity.x, 0., -velocity.y) * 0.5 * delta_time.0 * delta_time.0;
	println!("{} {} {}", velocity, delta_time.0, delta);
	position.0 += delta;
}

pub fn is_football_on_ground(
	football_caster: Query<&RayHits, With<FootballGroundCaster>>,
) -> bool
{
	!football_caster.single().is_empty()
}

pub fn axes_to_air_acceleration(
	In(axes_input): In<Vec2>,
	input: Res<Input<KeyCode>>,
	speed: Res<PlayerSpeed>,
) -> Vec2
{
	axes_input * speed.air_acceleration * if input.pressed(KeyCode::ShiftLeft) { speed.sprint_modifier } else { 1.0 }
}

#[derive(Component)]
pub struct FootballGroundCaster;