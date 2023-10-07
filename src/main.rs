mod gravity;
mod main_bundles;
#[cfg(feature = "editor_mode")]
mod editor;

use std::io::Cursor;

use bevy::prelude::*;
use bevy_window::PrimaryWindow;
use bevy_winit::WinitWindows;
use bevy_xpbd_3d::prelude::*;
use winit::window::Icon;

use self::gravity::*;
use self::main_bundles::*;

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
	));
}