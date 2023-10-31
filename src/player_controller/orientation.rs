use bevy::prelude::*;
use bevy_xpbd_3d::prelude::*;

use crate::gravity::AffectedByGravity;

#[derive(Component)]
pub struct GravityOrientation;

pub fn orient(
	mut rigidbodies: Query<(&mut Rotation, &AffectedByGravity), With<GravityOrientation>>,
)
{
	for (mut rotation, gravity) in &mut rigidbodies {
		rotation.0 = Quat::from_rotation_arc(rotation.rotate(Vec3::Y), gravity.up) * rotation.0;
	}
}