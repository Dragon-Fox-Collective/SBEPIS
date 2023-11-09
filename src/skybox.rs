use std::array::IntoIter;

use bevy::asset::LoadState;
use bevy::core_pipeline::Skybox;
use bevy::prelude::*;
use bevy::render::render_resource::Extent3d;
use bevy::render::render_resource::TextureDimension;
use bevy::render::render_resource::TextureViewDescriptor;
use bevy::render::render_resource::TextureViewDimension;

pub struct SkyboxPlugin;

impl Plugin for SkyboxPlugin
{
	fn build(&self, app: &mut App) {
		app
			.insert_resource(CurrentSkybox::default())
			.add_systems(Startup, (
				start_loading_skybox,
			))
			.add_systems(Update, (
				stitch_skybox.run_if(not(is_skybox_loaded).and_then(is_skybox_parts_loaded)),
				add_skybox.run_if(is_skybox_loaded),
			));
	}
}

#[derive(Resource, Default)]
struct CurrentSkybox
{
	skybox: Option<Handle<Image>>,
	left: Option<Handle<Image>>,
	right: Option<Handle<Image>>,
	top: Option<Handle<Image>>,
	bottom: Option<Handle<Image>>,
	back: Option<Handle<Image>>,
	front: Option<Handle<Image>>,
}
impl CurrentSkybox
{
	pub fn parts(&self) -> IntoIter<Option<Handle<Image>>, 6> {
		[self.left.clone(), self.right.clone(), self.top.clone(), self.bottom.clone(), self.back.clone(), self.front.clone()].into_iter()
	}
}

fn is_skybox_loaded(current_skybox: Res<CurrentSkybox>) -> bool { current_skybox.skybox.is_some() }
fn is_skybox_parts_loaded(
	current_skybox: Res<CurrentSkybox>,
	asset_server: Res<AssetServer>,
) -> bool
{
	current_skybox.parts().all(|image| image.is_some_and(|image| match asset_server.get_load_state(image).unwrap() {
		LoadState::NotLoaded => false,
		LoadState::Loading => false,
		LoadState::Loaded => true,
		LoadState::Failed => panic!("Skybox loading failed"),
	}))
}

fn start_loading_skybox(
	asset_server: Res<AssetServer>,
	mut current_skybox: ResMut<CurrentSkybox>,
)
{
	current_skybox.left = Some(asset_server.load("skybox/left.png"));
	current_skybox.right = Some(asset_server.load("skybox/right.png"));
	current_skybox.top = Some(asset_server.load("skybox/top.png"));
	current_skybox.bottom = Some(asset_server.load("skybox/bottom.png"));
	current_skybox.back = Some(asset_server.load("skybox/back.png"));
	current_skybox.front = Some(asset_server.load("skybox/front.png"));
}

fn stitch_skybox(
	mut images: ResMut<Assets<Image>>,
	mut current_skybox: ResMut<CurrentSkybox>,
)
{
	let sides: Vec<&Image> = current_skybox.parts().map(|side| images.get(side.unwrap()).unwrap()).collect();
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

	current_skybox.skybox = Some(images.add(skybox));
}

fn add_skybox(
	mut commands: Commands,
	camera: Query<Entity, (With<Camera3d>, Without<Skybox>)>,
	current_skybox: Res<CurrentSkybox>,
)
{
	for camera in camera.iter() {
		commands.entity(camera).insert(Skybox(current_skybox.skybox.clone().unwrap()));
	}
}