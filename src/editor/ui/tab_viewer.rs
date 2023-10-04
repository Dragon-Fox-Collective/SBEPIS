use bevy::prelude::*;
use bevy_asset::ReflectAsset;
use bevy_inspector_egui::bevy_inspector::{hierarchy::{hierarchy_ui, SelectedEntities}, ui_for_entity_with_children, ui_for_entities_shared_components, by_type_id::{ui_for_resource, ui_for_asset}};
use bevy_reflect::TypeRegistry;
use egui::Rect;
use egui_gizmo::GizmoMode;

use super::{InspectorSelection, EguiWindow};
use super::super::gizmo::draw_gizmo;

pub struct TabViewer<'a> {
	pub world: &'a mut World,
	pub viewport_rect: &'a mut Rect,
	pub selected_entities: &'a mut SelectedEntities,
	pub selection: &'a mut InspectorSelection,
	pub gizmo_mode: GizmoMode,
}

impl egui_dock::TabViewer for TabViewer<'_> {
	type Tab = EguiWindow;

	fn ui(&mut self, ui: &mut egui_dock::egui::Ui, window: &mut Self::Tab)
	{
		let type_registry = self.world.resource::<AppTypeRegistry>().0.clone();
		let type_registry = type_registry.read();

		match window {
			EguiWindow::GameView => {
				*self.viewport_rect = ui.clip_rect();

				draw_gizmo(ui, self.world, self.selected_entities, self.gizmo_mode);
			}
			EguiWindow::Hierarchy => {
				let selected = hierarchy_ui(self.world, ui, self.selected_entities);
				if selected {
					*self.selection = InspectorSelection::Entities;
				}
			}
			EguiWindow::Resources => select_resource(ui, &type_registry, self.selection),
			EguiWindow::Assets => select_asset(ui, &type_registry, self.world, self.selection),
			EguiWindow::Inspector => match *self.selection {
				InspectorSelection::Entities => match self.selected_entities.as_slice() {
					&[entity] => ui_for_entity_with_children(self.world, entity, ui),
					entities => ui_for_entities_shared_components(self.world, entities, ui),
				},
				InspectorSelection::Resource(type_id, ref name) => {
					ui.label(name);
					ui_for_resource(
						self.world,
						type_id,
						ui,
						name,
						&type_registry,
					)
				}
				InspectorSelection::Asset(type_id, ref name, handle) => {
					ui.label(name);
					ui_for_asset(
						self.world,
						type_id,
						handle,
						ui,
						&type_registry,
					);
				}
			},
		}
	}

	fn title(&mut self, window: &mut Self::Tab) -> egui_dock::egui::WidgetText
	{
		format!("{window:?}").into()
	}

	fn clear_background(&self, window: &Self::Tab) -> bool
	{
		!matches!(window, EguiWindow::GameView)
	}
}

fn select_resource(
	ui: &mut egui::Ui,
	type_registry: &TypeRegistry,
	selection: &mut InspectorSelection,
) {
	let mut resources: Vec<_> = type_registry
		.iter()
		.filter(|registration| registration.data::<ReflectResource>().is_some())
		.map(|registration| (registration.short_name().to_owned(), registration.type_id()))
		.collect();
	resources.sort_by(|(name_a, _), (name_b, _)| name_a.cmp(name_b));

	for (resource_name, type_id) in resources {
		let selected = match *selection {
			InspectorSelection::Resource(selected, _) => selected == type_id,
			_ => false,
		};

		if ui.selectable_label(selected, &resource_name).clicked() {
			*selection = InspectorSelection::Resource(type_id, resource_name);
		}
	}
}

fn select_asset(
	ui: &mut egui::Ui,
	type_registry: &TypeRegistry,
	world: &World,
	selection: &mut InspectorSelection,
) {
	let mut assets: Vec<_> = type_registry
		.iter()
		.filter_map(|registration| {
			let reflect_asset = registration.data::<ReflectAsset>()?;
			Some((
				registration.short_name().to_owned(),
				registration.type_id(),
				reflect_asset,
			))
		})
		.collect();
	assets.sort_by(|(name_a, ..), (name_b, ..)| name_a.cmp(name_b));

	for (asset_name, asset_type_id, reflect_asset) in assets {
		let mut handles: Vec<_> = reflect_asset.ids(world).collect();
		handles.sort();

		ui.collapsing(format!("{asset_name} ({})", handles.len()), |ui| {
			for handle in handles {
				let selected = match *selection {
					InspectorSelection::Asset(_, _, selected_id) => selected_id == handle,
					_ => false,
				};

				if ui
					.selectable_label(selected, format!("{:?}", handle))
					.clicked()
				{
					*selection =
						InspectorSelection::Asset(asset_type_id, asset_name.clone(), handle);
				}
			}
		});
	}
}