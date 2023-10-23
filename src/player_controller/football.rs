use bevy::prelude::*;
use bevy_xpbd_3d::prelude::*;

use super::Football;

pub fn compose_axes(
	input: Res<Input<KeyCode>>,
) -> Vec2
{
	let mut axes = Vec2::ZERO;
	
	if input.pressed(KeyCode::A) { axes += Vec2::NEG_X }
	if input.pressed(KeyCode::D) { axes += Vec2::X }
	if input.pressed(KeyCode::S) { axes += Vec2::NEG_Y }
	if input.pressed(KeyCode::W) { axes += Vec2::Y }

	axes.normalize_or_zero()
}

pub fn spin_football(
	In(input): In<Vec2>,
	mut football: Query<&mut AngularVelocity, With<Football>>,
)
{
	let mut football_velocity = football.single_mut();
	football_velocity.0 = Vec3::new(-input.y, 0., -input.x) * 10.0;
}