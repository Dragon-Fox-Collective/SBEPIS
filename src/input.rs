use core::fmt::Debug;

use bevy::prelude::*;
use leafwing_input_manager::plugin::InputManagerSystem;
use leafwing_input_manager::prelude::*;

pub struct InputPlugin;
impl Plugin for InputPlugin
{
	fn build(&self, app: &mut App)
	{
		add_action_set(app, [(KeyCode::W, MovementAction::Forward)]);
		add_button_event(app, MovementAction::Forward, MoveForward::default);
		
		add_action_set(app, [(KeyCode::W, NoteAction::D5)]);
		add_button_event(app, NoteAction::D5, PlayNoteD5::default);

		add_input_block::<NoteAction, MovementAction>(app);

		add_action_set(app, [(KeyCode::Q, ToggleNoteAction::ToggleNote)]);
		add_button_event(app, ToggleNoteAction::ToggleNote, ToggleNote::default);

		app
			.add_systems(Update, (
				toggle_note.run_if(on_event::<ToggleNote>()),
				event_consumer::<MoveForward>,
				event_consumer::<PlayNoteD5>,
			));
	}
}

pub fn add_action_set<Action>(
	app: &mut App,
	bindings: impl IntoIterator<Item = (impl Into<UserInput> + 'static, Action)> + Copy + Send + Sync + 'static,
)
where
	Action: Actionlike + Copy,
{
	app
		.add_plugins(InputManagerPlugin::<Action>::default())
		.add_systems(Startup, spawn_manager(bindings));
}

pub fn add_button_event<Action, EventType>(
	app: &mut App,
	action: Action,  
	event_generator: impl Fn() -> EventType + Send + Sync + 'static,
)
where
	Action: Actionlike + Copy,
	EventType: Event,
{
	app
		.add_event::<EventType>()
		.add_systems(PreUpdate, button_event(action, event_generator).after(InputManagerSystem::ManualControl));
}

#[derive(Actionlike, Clone, Copy, Reflect)]
pub enum MovementAction {
	Forward,
}

#[derive(Actionlike, Clone, Copy, Reflect)]
pub enum NoteAction {
	D5,
}

#[derive(Actionlike, Clone, Copy, Reflect)]
pub enum ToggleNoteAction {
	ToggleNote,
}

#[derive(Event, Default, Debug)]
pub struct MoveForward;

#[derive(Event, Default, Debug)]
pub struct PlayNoteD5;

#[derive(Event, Default, Debug)]
pub struct ToggleNote;

pub fn button_event<Action, EventType>(
	action: Action,
	event_generator: impl Fn() -> EventType,
) -> impl Fn(Query<&ActionState<Action>>, EventWriter<EventType>)
where
	Action: Actionlike + Copy,
	EventType: Event,
{
	move |
		input: Query<&ActionState<Action>>,
		mut event: EventWriter<EventType>,
	|
	{
		let input = input.single();
		if input.just_pressed(action) { event.send(event_generator()); }
	}
}

fn event_consumer<T: Event + Debug>(
	mut event: EventReader<T>,
) {
	for ev in event.into_iter() {
		println!("{:?}", ev);
	}
}

fn spawn_manager<Action: Actionlike>(
	bindings: impl IntoIterator<Item = (impl Into<UserInput>, Action)> + Copy,
) -> impl Fn(Commands)
{
	move |
		mut commands: Commands
	|
	{
		commands.spawn(InputManagerBundle::<Action> {
			action_state: ActionState::default(),
			input_map: InputMap::new(bindings),
		});
	}
}

pub fn add_input_block<Blocker: Actionlike, Blockee: Actionlike>(
	app: &mut App,
) {
	app
		.add_systems(Startup, disable_action::<Blocker>)
		.add_systems(Update, (
			reset_block::<Blockee>,
			input_block::<Blocker, Blockee>.after(reset_block::<Blockee>),
		));
}

fn disable_action<Action: Actionlike>(
	mut action: ResMut<ToggleActions<Action>>,
) {
	action.enabled = false;
}

fn reset_block<Blockee: Actionlike>(
	mut blockee: ResMut<ToggleActions<Blockee>>,
) {
	blockee.enabled = true;
}

fn input_block<Blocker: Actionlike, Blockee: Actionlike>(
	blocker: Res<ToggleActions<Blocker>>,
	mut blockee: ResMut<ToggleActions<Blockee>>,
) {
	if blocker.enabled {
		blockee.enabled = false;
		return;
	}
}

fn toggle_note(
	mut note_action: ResMut<ToggleActions<NoteAction>>,
) {
	note_action.enabled = !note_action.enabled;
	println!("Toggled NoteAction to {}", note_action.enabled);
}