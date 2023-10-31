use bevy::{prelude::*, input::mouse::MouseMotion};
use num_traits::Float;
use std::ops::{Add, Sub, Mul, Div};

pub trait MapRange<T>
{
	fn map(self, min_x: T, max_x: T, min_y: T, max_y: T) -> T;
}

impl<T, F> MapRange<T> for F
where
	T: Add<Output = T> + Sub<Output = T> + Div<Output = T> + Mul<Output = T> + Copy,
	F: Float + Sub<T, Output = T>,
{
	fn map(self, min_x: T, max_x: T, min_y: T, max_y: T) -> T
	{
		(self - min_x) / (max_x - min_x) * (max_y - min_y) + min_y
	}
}

pub fn compose_wasd_axes(
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

pub fn compose_mouse_delta_axes(
	mut motion_ev: EventReader<MouseMotion>,
) -> Vec2
{
	motion_ev.into_iter().map(|ev| ev.delta).sum()
}