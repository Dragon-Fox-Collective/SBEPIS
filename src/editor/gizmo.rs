use bevy::prelude::*;
use bevy_inspector_egui::bevy_inspector::hierarchy::SelectedEntities;
use bevy_render::camera::CameraProjection;
use egui_gizmo::{Gizmo, GizmoMode, GizmoOrientation};

use super::ui::UiState;
use super::camera::EditorCamera;

pub struct EditorGizmoPlugin;

impl Plugin for EditorGizmoPlugin
{
	fn build(&self, app: &mut App) {
		app
			.add_systems(Update, (
				set_gizmo_mode,
			));
	}
}

pub fn set_gizmo_mode(input: Res<Input<KeyCode>>, mut ui_state: ResMut<UiState>) {
	for (key, mode) in [
		(KeyCode::R, GizmoMode::Rotate),
		(KeyCode::T, GizmoMode::Translate),
		(KeyCode::S, GizmoMode::Scale),
	] {
		if input.just_pressed(key) {
			ui_state.gizmo_mode = mode;
		}
	}
}

pub fn draw_gizmo(
	ui: &mut egui::Ui,
	world: &mut World,
	selected_entities: &SelectedEntities,
	gizmo_mode: GizmoMode,
) {
	let (cam_transform, projection) = world
		.query_filtered::<(&GlobalTransform, &Projection), With<EditorCamera>>()
		.single(world);
	let view_matrix = Mat4::from(cam_transform.affine().inverse());
	let projection_matrix = projection.get_projection_matrix();

	if selected_entities.len() != 1 {
		return;
	}

	for selected in &selected_entities {
		let Some(transform) = world.get::<Transform>(selected) else {
			continue;
		};
		let model_matrix =transform.compute_matrix();

		let Some(result) = Gizmo::new(selected)
			.model_matrix(model_matrix.to_cols_array_2d())
			.view_matrix(view_matrix.to_cols_array_2d())
			.projection_matrix(projection_matrix.to_cols_array_2d())
			.orientation(GizmoOrientation::Local)
			.mode(gizmo_mode)
			.interact(ui)
		else {
			continue;
		};

		let mut transform = world.get_mut::<Transform>(selected).unwrap();
		*transform = Transform {
			translation: Vec3::from(<[f32; 3]>::from(result.translation)),
			rotation: Quat::from_array(<[f32; 4]>::from(result.rotation)),
			scale: Vec3::from(<[f32; 3]>::from(result.scale)),
		};
	}
}