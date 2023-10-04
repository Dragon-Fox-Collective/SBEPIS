use bevy::prelude::*;
use bevy_egui::EguiContext;
use bevy_mod_picking::{prelude::*, backends::egui::EguiPointer, PickableBundle};
use bevy_window::PrimaryWindow;

use super::ui::UiState;

pub struct EditorRaycastPlugin;

impl Plugin for EditorRaycastPlugin
{
	fn build(&self, app: &mut App) {
		app
			.add_plugins((
				DefaultPickingPlugins,
			))
			.add_systems(Update, (
				auto_add_raycast_target,
				handle_pick_events,
			));
	}
}

pub fn auto_add_raycast_target(
	mut commands: Commands,
	query: Query<Entity, (Without<RaycastPickTarget>, With<Handle<Mesh>>)>,
) {
	for entity in &query {
		commands
			.entity(entity)
			.insert((
				RaycastPickTarget::default(),
				PickableBundle::default(),
			));
	}
}

pub fn handle_pick_events(
	mut ui_state: ResMut<UiState>,
	mut click_events: EventReader<Pointer<Click>>,
	mut egui: Query<&mut EguiContext, With<PrimaryWindow>>,
	egui_entity: Query<&EguiPointer>,
)
{
	let Ok(mut egui_context) = egui.get_single_mut() else { return; };
	let egui_context = egui_context.get_mut();

	for click in click_events.iter() {
		if egui_entity.get(click.target()).is_ok() {
			continue;
		};

		let modifiers = egui_context.input(|i| i.modifiers);
		let add = modifiers.ctrl || modifiers.shift;

		ui_state
			.selected_entities
			.select_maybe_add(click.target(), add);
	}
}