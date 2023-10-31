use bevy::prelude::*;
use bevy_xpbd_3d::prelude::*;

#[derive(Component)]
pub struct Football;

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
)
{
	let mut football_velocity = football.single_mut();
	football_velocity.0 = Vec3::new(-input_velocity.y, 0., -input_velocity.x);
}