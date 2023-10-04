mod ui;
mod picking;
mod camera;
mod gizmo;

use bevy::{prelude::*, app::PluginGroupBuilder};

pub struct EditorPlugins;

impl PluginGroup for EditorPlugins
{
	fn build(self) -> PluginGroupBuilder
	{
		PluginGroupBuilder::start::<Self>()
			.add(self::ui::EditorEguiPlugin)
			.add(self::picking::EditorRaycastPlugin)
			.add(self::gizmo::EditorGizmoPlugin)
			.add(self::camera::EditorCameraPlugin)
	}
}