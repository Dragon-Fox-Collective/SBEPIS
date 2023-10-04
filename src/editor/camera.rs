use bevy::prelude::*;
use bevy_egui::EguiSettings;
use bevy_mod_picking::prelude::*;
use bevy_panorbit_camera::{PanOrbitCamera, PanOrbitCameraPlugin};
use bevy_render::camera::Viewport;
use bevy_window::PrimaryWindow;

use super::ui::{show_ui_system, UiState};

pub struct EditorCameraPlugin;

impl Plugin for EditorCameraPlugin
{
	fn build(&self, app: &mut App)
	{
		app
			.add_plugins((
				PanOrbitCameraPlugin,
			))
			.add_systems(Startup, (
				spawn_editor_camera,
			))
			.add_systems(PostStartup, (
				disable_other_cameras,
			))
			.add_systems(PostUpdate, (
				set_camera_viewport.after(show_ui_system),
			));
	}
}

#[derive(Component)]
pub struct EditorCamera;

/// Make camera only render to view not obstructed by UI
pub fn set_camera_viewport(
	ui_state: Res<UiState>,
	primary_window: Query<&mut Window, With<PrimaryWindow>>,
	egui_settings: Res<EguiSettings>,
	mut cameras: Query<&mut Camera, With<EditorCamera>>,
)
{
	let mut cam = cameras.single_mut();

	let Ok(window) = primary_window.get_single() else {
		return;
	};

	let scale_factor = window.scale_factor() * egui_settings.scale_factor;

	let viewport_pos = ui_state.viewport_rect.left_top().to_vec2() * scale_factor as f32;
	let viewport_size = ui_state.viewport_rect.size() * scale_factor as f32;

	cam.viewport = Some(Viewport {
		physical_position: UVec2::new(viewport_pos.x as u32, viewport_pos.y as u32),
		physical_size: UVec2::new(viewport_size.x as u32, viewport_size.y as u32),
		depth: 0.0..1.0,
	});
}

pub fn spawn_editor_camera(
	mut commands: Commands,
)
{
	commands.spawn((
		Camera3dBundle {
			transform: Transform::from_xyz(4.0, 6.5, 8.0).looking_at(Vec3::ZERO, Vec3::Y),
			..default()
		},
		EditorCamera,
		RaycastPickCamera::default(),
		PanOrbitCamera {
			button_orbit: MouseButton::Left,
			button_pan: MouseButton::Left,
			modifier_pan: Some(KeyCode::ShiftLeft),
			reversed_zoom: true,
			..default()
		},
	));
}

pub fn disable_other_cameras(
	mut cameras: Query<&mut Camera, Without<EditorCamera>>
)
{
	for mut camera in &mut cameras
	{
		camera.is_active = false;
	}
}