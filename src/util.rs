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