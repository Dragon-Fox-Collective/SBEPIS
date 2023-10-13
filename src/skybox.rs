use bevy::core_pipeline::Skybox;
use bevy::prelude::*;
use bevy_asset::LoadState;
use bevy_render::render_resource::Extent3d;
use bevy_render::render_resource::TextureDimension;
use bevy_render::render_resource::TextureViewDescriptor;
use bevy_render::render_resource::TextureViewDimension;

pub struct SkyboxPlugin;

impl Plugin for SkyboxPlugin
{
	fn build(&self, app: &mut App) {
		app
			.insert_resource(CurrentSkybox::default())
			.add_systems(Update, (
				load_skybox.run_if(is_skybox_not_loaded),
				add_skybox.run_if(is_skybox_loaded),
			));
	}
}

#[derive(Resource, Default)]
struct CurrentSkybox(Option<Handle<Image>>);

fn is_skybox_loaded(current_skybox: Res<CurrentSkybox>) -> bool { current_skybox.0.is_some() }
fn is_skybox_not_loaded(current_skybox: Res<CurrentSkybox>) -> bool { current_skybox.0.is_none() }

fn add_skybox(
	mut commands: Commands,
	camera: Query<Entity, (With<Camera3d>, Without<Skybox>)>,
	current_skybox: Res<CurrentSkybox>,
)
{
	for camera in camera.iter() {
		commands.entity(camera).insert(Skybox(current_skybox.0.clone().unwrap()));
	}
}

fn load_skybox(
	mut images: ResMut<Assets<Image>>,
	asset_server: Res<AssetServer>,
	mut current_skybox: ResMut<CurrentSkybox>,
)
{
	let side_handles: Vec<Handle<Image>> = ["left", "right", "top", "bottom", "back", "front"].into_iter()
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
		return;
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

	current_skybox.0 = Some(images.add(skybox));
}