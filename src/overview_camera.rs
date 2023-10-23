use bevy::prelude::*;
use bevy::input::common_conditions::input_just_pressed;

use crate::player_controller::PlayerCamera;

pub struct OverviewCameraPlugin;

impl Plugin for OverviewCameraPlugin
{
	fn build(&self, app: &mut App) {
		app
			.insert_resource(UsingOverviewCamera(true))
			.add_plugins((
				bevy_panorbit_camera::PanOrbitCameraPlugin,
			))
			.add_systems(Startup, (
				setup,
			))
			.add_systems(Update, (
				toggle_camera.run_if(input_just_pressed(KeyCode::Tab)),
				set_cameras,
			))
			;
	}
}

#[derive(Resource)]
pub struct UsingOverviewCamera(pub bool);

#[derive(Component)]
pub struct OverviewCamera;

fn setup(
	mut commands: Commands,
)
{
	commands.spawn((
		Name::new("Overview Camera"),
		Camera3dBundle {
			transform: Transform::from_xyz(4.0, 6.5, 8.0).looking_at(Vec3::ZERO, Vec3::Y),
			..default()
		},
		bevy_panorbit_camera::PanOrbitCamera {
			button_orbit: MouseButton::Left,
			button_pan: MouseButton::Left,
			modifier_pan: Some(KeyCode::ShiftLeft),
			reversed_zoom: true,
			..default()
		},
		OverviewCamera,
	));
}

pub fn toggle_camera(
	mut using_overview_camera: ResMut<UsingOverviewCamera>,
)
{
	using_overview_camera.0 = !using_overview_camera.0;	
}

pub fn set_cameras(
	mut overview_camera: Query<&mut Camera, (With<OverviewCamera>, Without<PlayerCamera>)>,
	mut player_camera: Query<&mut Camera, (With<PlayerCamera>, Without<OverviewCamera>)>,
	using_overview_camera: Res<UsingOverviewCamera>,
)
{
	overview_camera.single_mut().is_active = using_overview_camera.0;
	player_camera.single_mut().is_active = !using_overview_camera.0;
}