use bevy::prelude::*;
use num_traits::Float;
use std::{ops::{Add, Sub, Mul, Div}, array::IntoIter};

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

pub trait TransformEx
{
	fn transform_vector3(&self, vector: Vec3) -> Vec3;
	fn inverse_transform_point(&self, point: Vec3) -> Vec3;
	fn inverse_transform_vector3(&self, vector: Vec3) -> Vec3;
}
impl TransformEx for GlobalTransform
{
	fn transform_vector3(&self, vector: Vec3) -> Vec3 {
		self.affine().transform_vector3(vector)
	}
	
	fn inverse_transform_point(&self, point: Vec3) -> Vec3 {
		self.affine().inverse().transform_point3(point)
	}
	
	fn inverse_transform_vector3(&self, vector: Vec3) -> Vec3 {
		self.affine().inverse().transform_vector3(vector)
	}
}

pub trait IterElements<T, const N: usize>
{
	fn iter_elements(&self) -> IntoIter<T, N>;
}
impl IterElements<f32, 3> for Vec3
{
	fn iter_elements(&self) -> IntoIter<f32, 3> {
		[self.x, self.y, self.z].into_iter()
	}
}