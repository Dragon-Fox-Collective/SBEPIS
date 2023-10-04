use std::any::TypeId;

use bevy::prelude::*;
use bevy_asset::{HandleId, ReflectAsset};
use bevy_egui::{EguiPlugin, EguiContext, EguiSet, EguiSettings};
use bevy_inspector_egui::{DefaultInspectorConfigPlugin, bevy_inspector::{hierarchy::{hierarchy_ui, SelectedEntities}, ui_for_entity_with_children, ui_for_entities_shared_components, by_type_id::{ui_for_resource, ui_for_asset}}};
use bevy_mod_picking::{prelude::*, backends::egui::EguiPointer, PickableBundle};
use bevy_reflect::TypeRegistry;
use bevy_render::camera::{Viewport, CameraProjection};
use bevy_window::PrimaryWindow;
use egui::{Rect, Context};
use egui_dock::{NodeIndex, DockArea, Style, DockState};
use egui_gizmo::{Gizmo, GizmoMode, GizmoOrientation};

pub struct UiPlugin;

impl Plugin for UiPlugin
{
	fn build(&self, app: &mut App) {
		app
			.add_plugins((
				DefaultInspectorConfigPlugin,
				EguiPlugin,
				DefaultPickingPlugins,
			))
			.insert_resource(UiState::new())
			.add_systems(
				PostUpdate, (
					show_ui_system
						.before(EguiSet::ProcessOutput)
						.before(bevy::transform::TransformSystem::TransformPropagate),
					set_camera_viewport.after(show_ui_system),
			))
			.add_systems(Update, (
				set_gizmo_mode,
				auto_add_raycast_target,
				handle_pick_events,
			));
	}
}

fn auto_add_raycast_target(
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

fn handle_pick_events(
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

#[derive(Component)]
pub struct EditorCamera;

fn show_ui_system(
	world: &mut World,
)
{
	let Ok(egui_context) = world.query_filtered::<&mut EguiContext, With<PrimaryWindow>>().get_single(world) else { return; };
	let mut egui_context = egui_context.clone();

	world.resource_scope::<UiState, _>(|world, mut ui_state| {
		ui_state.ui(world, egui_context.get_mut())
	});
}

/// Make camera only render to view not obstructed by UI
fn set_camera_viewport(
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

fn set_gizmo_mode(input: Res<Input<KeyCode>>, mut ui_state: ResMut<UiState>) {
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

#[derive(Eq, PartialEq)]
enum InspectorSelection {
	Entities,
	Resource(TypeId, String),
	Asset(TypeId, String, HandleId),
}

#[derive(Resource)]
struct UiState {
	state: DockState<EguiWindow>,
	viewport_rect: Rect,
	selected_entities: SelectedEntities,
	selection: InspectorSelection,
	gizmo_mode: GizmoMode,
}

impl UiState {
	pub fn new() -> Self
	{
		let mut state = DockState::new(vec![EguiWindow::GameView]);
		let surface = state.main_surface_mut();
		let [game, _inspector] =
			surface.split_right(NodeIndex::root(), 0.75, vec![EguiWindow::Inspector]);
		let [game, _hierarchy] = surface.split_left(game, 0.2, vec![EguiWindow::Hierarchy]);
		let [_game, _bottom] =
			surface.split_below(game, 0.8, vec![EguiWindow::Resources, EguiWindow::Assets]);

		Self {
			state,
			viewport_rect: Rect::NOTHING,
			selected_entities: SelectedEntities::default(),
			selection: InspectorSelection::Entities,
			gizmo_mode: GizmoMode::Translate,
		}
	}

	fn ui(&mut self, world: &mut World, ctx: &mut Context)
	{
		let mut tab_viewer = TabViewer {
			world,
			viewport_rect: &mut self.viewport_rect,
			selected_entities: &mut self.selected_entities,
			selection: &mut self.selection,
			gizmo_mode: self.gizmo_mode,
		};
		DockArea::new(&mut self.state)
			.style(Style::from_egui(ctx.style().as_ref()))
			.show(ctx, &mut tab_viewer);
	}
}

#[derive(Debug)]
enum EguiWindow {
	GameView,
	Hierarchy,
	Resources,
	Assets,
	Inspector,
}

struct TabViewer<'a> {
	world: &'a mut World,
	viewport_rect: &'a mut Rect,
	selected_entities: &'a mut SelectedEntities,
	selection: &'a mut InspectorSelection,
	gizmo_mode: GizmoMode,
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

fn draw_gizmo(
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

	for selected in selected_entities.iter() {
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