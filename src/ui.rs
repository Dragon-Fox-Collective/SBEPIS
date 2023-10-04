use bevy::{prelude::*, window::PrimaryWindow, transform::TransformSystem, render::camera::Viewport};
use bevy_egui::{EguiPlugin, EguiContext, EguiSet, EguiSettings};
use bevy_inspector_egui::{DefaultInspectorConfigPlugin, bevy_inspector::hierarchy::{hierarchy_ui, SelectedEntities}};
use egui::{Rect, Context};
use egui_dock::{NodeIndex, DockArea, Style, DockState};

pub struct UiPlugin;

impl Plugin for UiPlugin
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
						.before(TransformSystem::TransformPropagate),
					set_camera_viewport.after(show_ui_system),
			));
	}
}

#[derive(Component)]
pub struct EditorCamera;

fn show_ui_system(world: &mut World)
{
	let Ok(egui_context) = world
		.query_filtered::<&mut EguiContext, With<PrimaryWindow>>()
		.get_single(world)
	else {
		return;
	};
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

#[derive(Resource)]
struct UiState {
	state: DockState<EguiWindow>,
	viewport_rect: Rect,
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
		}
	}

	fn ui(&mut self, world: &mut World, ctx: &mut Context)
	{
		let mut tab_viewer = TabViewer {
			world,
			viewport_rect: &mut self.viewport_rect,
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

				//draw_gizmo(ui, self.world, self.selected_entities, self.gizmo_mode);
			}
			EguiWindow::Hierarchy => {
				let selected = hierarchy_ui(self.world, ui, &mut SelectedEntities::default());
				if selected {
					//*self.selection = InspectorSelection::Entities;
				}
			}
			EguiWindow::Resources => (),//select_resource(ui, &type_registry, self.selection),
			EguiWindow::Assets => (),//select_asset(ui, &type_registry, self.world, self.selection),
			EguiWindow::Inspector => (),/*match *self.selection {
				InspectorSelection::Entities => match self.selected_entities.as_slice() {
					&[entity] => ui_for_entity_with_children(self.world, entity, ui),
					entities => ui_for_entities_shared_components(self.world, entities, ui),
				},
				InspectorSelection::Resource(type_id, ref name) => {
					ui.label(name);
					bevy_inspector::by_type_id::ui_for_resource(
						self.world,
						type_id,
						ui,
						name,
						&type_registry,
					)
				}
				InspectorSelection::Asset(type_id, ref name, handle) => {
					ui.label(name);
					bevy_inspector::by_type_id::ui_for_asset(
						self.world,
						type_id,
						handle,
						ui,
						&type_registry,
					);
				}
			},*/
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
