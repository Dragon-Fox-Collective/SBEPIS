mod gravity;
mod main_bundles;
mod player_commands;
mod player_controller;
mod util;
mod skybox;
#[cfg(feature = "overview_camera")]
mod overview_camera;

use self::main_bundles::*;

use std::io::Cursor;

use bevy::prelude::*;
use bevy::window::PrimaryWindow;
use bevy::winit::WinitWindows;
use bevy_xpbd_3d::prelude::*;
use winit::window::Icon;

fn main()
{
	App::new()
		.add_plugins((
			DefaultPlugins
				.set(WindowPlugin
				{
					primary_window: Some(Window
					{
						title: "SBEPIS".to_string(),
						..default()
					}),
					..default()
				})
				.set(ImagePlugin {
					default_sampler: bevy::render::render_resource::SamplerDescriptor {
						address_mode_u: bevy::render::render_resource::AddressMode::Repeat,
						address_mode_v: bevy::render::render_resource::AddressMode::Repeat,
						address_mode_w: bevy::render::render_resource::AddressMode::Repeat,
						..default()
					},
				}),
			PhysicsPlugins::default(),
			#[cfg(feature = "inspector")]
			bevy_inspector_egui::quick::WorldInspectorPlugin::new(),
			#[cfg(feature = "overview_camera")]
			self::overview_camera::OverviewCameraPlugin,
			self::gravity::GravityPlugin,
			self::player_commands::PlayerCommandsPlugin,
			self::skybox::SkyboxPlugin,
			self::player_controller::PlayerControllerPlugin,
		))
		.insert_resource(FixedTime::new_from_secs(1.0 / 60.0))
		.add_systems(Startup, (
			set_window_icon,
			setup,
		))
		.run();
}

fn set_window_icon(
	windows: NonSend<WinitWindows>,
	primary_window: Query<Entity, With<PrimaryWindow>>,
)
{
	let icon_buf = Cursor::new(include_bytes!("../assets/house.png"));
	let image = image::load(icon_buf, image::ImageFormat::Png).unwrap();
	let image = image.into_rgba8();
	let (width, height) = image.dimensions();
	let rgba = image.into_raw();
	let icon = Icon::from_rgba(rgba, width, height).unwrap();

	let primary_entity = primary_window.single();
	let primary = windows.get_window(primary_entity).unwrap();

	primary.set_window_icon(Some(icon));
}

fn gridbox_texture(color: &str) -> String
{
	format!("Gridbox Prototype Materials/prototype_512x512_{color}.png")
}

fn gridbox_material(color: &str, materials: &mut Assets<StandardMaterial>, asset_server: &AssetServer) -> Handle<StandardMaterial>
{
	materials.add(StandardMaterial {
		base_color_texture: Some(asset_server.load(gridbox_texture(color))),
		..default()
	})
}

fn setup(
	mut commands: Commands,
	mut meshes: ResMut<Assets<Mesh>>,
	mut materials: ResMut<Assets<StandardMaterial>>,
	asset_server: Res<AssetServer>,
)
{
	let gray_material = gridbox_material("grey2", &mut materials, &asset_server);
	let green_material = gridbox_material("green1", &mut materials, &asset_server);

	commands.spawn((Name::new("Planet"), PlanetBundle::new(Vec3::Y * -100.0, 100.0, 10.0, &mut meshes, gray_material)));

	let cube_mesh = meshes.add(Mesh::from(shape::Cube { size: 1.0 }));
	commands.spawn((Name::new("Cube 1"), BoxBundle::new(Vec3::new(0.0, 4.0, 0.0), cube_mesh.clone(), green_material.clone())));
	commands.spawn((Name::new("Cube 2"), BoxBundle::new(Vec3::new(0.5, 5.5, 0.0), cube_mesh.clone(), green_material.clone())));
	commands.spawn((Name::new("Cube 3"), BoxBundle::new(Vec3::new(-0.5, 7.0, 0.0), cube_mesh.clone(), green_material.clone())));

	commands.spawn((
		Name::new("Sun"),
		DirectionalLightBundle {
			directional_light: DirectionalLight {
				illuminance: 10000.0,
				shadows_enabled: true,
				..default()
			},
			transform: Transform {
				rotation: Quat::from_euler(EulerRot::XYZ, -1.9, 0.8, 0.0),
				..default()
			},
			..default()
		},
	));
}