mod gravity;
mod main_bundles;
#[cfg(feature = "editor_mode")]
mod editor;
mod player_commands;
mod util;

use self::gravity::*;
use self::main_bundles::*;
use self::player_commands::*;

use std::io::Cursor;

use bevy::core_pipeline::Skybox;
use bevy::prelude::*;
use bevy_asset::LoadState;
use bevy_render::render_resource::Extent3d;
use bevy_render::render_resource::TextureDimension;
use bevy_render::render_resource::TextureViewDescriptor;
use bevy_render::render_resource::TextureViewDimension;
use bevy_window::PrimaryWindow;
use bevy_winit::WinitWindows;
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
				}),
			PhysicsPlugins::default(),
			#[cfg(feature = "editor_mode")]
			editor::EditorPlugins,
			GravityPlugin,
			PlayerCommandsPlugin,
		))
		.insert_resource(FixedTime::new_from_secs(1.0 / 60.0))
		.add_systems(Startup, (
			set_window_icon,
			setup,
		))
		.add_systems(Update, (
			add_skybox,
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

#[derive(Component)]
struct MainCamera;

fn setup(
	mut commands: Commands,
	mut meshes: ResMut<Assets<Mesh>>,
	mut materials: ResMut<Assets<StandardMaterial>>,
)
{
	commands.spawn((Name::new("Planet"), PlanetBundle::new(Vec3::Y * -2.0, 2.0, 10.0, &mut meshes, &mut materials)));

	commands.spawn((Name::new("Cube 1"), BoxBundle::new(Vec3::new(0.0, 4.0, 0.0), &mut meshes, &mut materials)));
	commands.spawn((Name::new("Cube 2"), BoxBundle::new(Vec3::new(0.5, 5.5, 0.0), &mut meshes, &mut materials)));
	commands.spawn((Name::new("Cube 3"), BoxBundle::new(Vec3::new(-0.5, 7.0, 0.0), &mut meshes, &mut materials)));

	commands.spawn(PointLightBundle {
		point_light: PointLight {
			intensity: 1500.0,
			shadows_enabled: true,
			..default()
		},
		transform: Transform::from_xyz(4.0, 8.0, 4.0),
		..default()
	});

	commands.spawn((
		Camera3dBundle {
			transform: Transform::from_xyz(-4.0, 6.5, 8.0).looking_at(Vec3::ZERO, Vec3::Y),
			..default()
		},
		MainCamera,
	));
}

fn add_skybox(
	mut commands: Commands,
	mut images: ResMut<Assets<Image>>,
	asset_server: Res<AssetServer>,
	camera: Query<(Option<&Skybox>, Entity), With<MainCamera>>
)
{
	let (camera_skybox, camera_entity) = camera.single();
	if camera_skybox.is_some() {
		return;
	}

	let skybox = load_skybox(&mut images, &asset_server);
	if skybox.is_none() {
		return;
	}
	let skybox = skybox.unwrap();

	commands.entity(camera_entity).insert(Skybox(skybox));
}

fn load_skybox(
	images: &mut Assets<Image>,
	asset_server: &AssetServer,
) -> Option<Handle<Image>>
{
	let side_handles: Vec<Handle<Image>> = ["left", "right", "bottom", "top", "back", "front"].into_iter()
		.map(|side_name| format!("skybox/{side_name}.png"))
		.map(|texture_name| asset_server.load(texture_name))
		.collect();

	let sides_states: Vec<LoadState> = side_handles.iter().map(|side| asset_server.get_load_state(side)).collect();

	if sides_states.iter().copied().any(|state| match state {
		LoadState::NotLoaded => false,
		LoadState::Loading => false,
		LoadState::Loaded => false,
		LoadState::Failed => true,
		LoadState::Unloaded => true,
	}) {
		panic!("Erroneous skybox load states {:?}", sides_states);
	}
	if sides_states.iter().copied().any(|state| state != LoadState::Loaded) {
		return None;
	}

	let sides: Vec<&Image> = side_handles.iter().map(|side| images.get(side).unwrap()).collect();
	let first_side_image = *sides.first().unwrap();

	let mut skybox = Image::new(
		Extent3d
		{
			width: first_side_image.texture_descriptor.size.width,
			height: first_side_image.texture_descriptor.size.width * 6,
			depth_or_array_layers: 1,
		},
		TextureDimension::D2,
		sides.into_iter().flat_map(|texture| texture.data.as_slice()).copied().collect(),
		first_side_image.texture_descriptor.format
	);
	skybox.reinterpret_stacked_2d_as_array(6);
	skybox.texture_view_descriptor = Some(TextureViewDescriptor
	{
		dimension: Some(TextureViewDimension::Cube),
		..default()
	});

	Some(images.add(skybox))
}