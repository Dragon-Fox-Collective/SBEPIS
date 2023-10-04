mod tab_viewer;

use std::any::TypeId;

use bevy::prelude::*;
use bevy_asset::HandleId;
use bevy_egui::{EguiPlugin, EguiContext, EguiSet};
use bevy_inspector_egui::{DefaultInspectorConfigPlugin, bevy_inspector::hierarchy::SelectedEntities};
use bevy_window::PrimaryWindow;
use egui::{Rect, Context};
use egui_dock::{NodeIndex, DockArea, Style, DockState};
use egui_gizmo::GizmoMode;

use self::tab_viewer::TabViewer;

pub struct EditorEguiPlugin;

impl Plugin for EditorEguiPlugin
{
	fn build(&self, app: &mut App) {
		app
			.add_plugins((
				DefaultInspectorConfigPlugin,
				EguiPlugin,
			))
			.insert_resource(UiState::new())
			.add_systems(
				PostUpdate, (
					show_ui_system
						.before(EguiSet::ProcessOutput)
						.before(bevy::transform::TransformSystem::TransformPropagate),
			));
	}
}

#[derive(Debug)]
pub enum EguiWindow {
	GameView,
	Hierarchy,
	Resources,
	Assets,
	Inspector,
}

#[derive(Eq, PartialEq)]
pub enum InspectorSelection {
	Entities,
	Resource(TypeId, String),
	Asset(TypeId, String, HandleId),
}

#[derive(Resource)]
pub struct UiState {
	pub state: DockState<EguiWindow>,
	pub viewport_rect: Rect,
	pub selected_entities: SelectedEntities,
	pub selection: InspectorSelection,
	pub gizmo_mode: GizmoMode,
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

pub fn show_ui_system(
	world: &mut World,
)
{
	let Ok(egui_context) = world.query_filtered::<&mut EguiContext, With<PrimaryWindow>>().get_single(world)
	else { return; };
	let mut egui_context = egui_context.clone();

	world.resource_scope::<UiState, _>(|world, mut ui_state| {
		ui_state.ui(world, egui_context.get_mut())
	});
}
