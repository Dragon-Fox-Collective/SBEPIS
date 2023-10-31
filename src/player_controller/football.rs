use bevy::prelude::*;
use bevy_xpbd_3d::prelude::*;

#[derive(Component)]
pub struct Football;

pub fn spin_football(
	In(input): In<Vec2>,
	mut football: Query<&mut AngularVelocity, With<Football>>,
)
{
	let mut football_velocity = football.single_mut();
	football_velocity.0 = Vec3::new(-input.y, 0., -input.x) * 10.0;
}