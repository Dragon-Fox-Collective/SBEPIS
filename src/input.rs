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

		app
			.add_systems(Update, (
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

#[derive(Event, Default, Debug)]
pub struct MoveForward;

#[derive(Event, Default, Debug)]
pub struct PlayNoteD5;

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
		for input in input.iter() {
			if input.just_pressed(action) { event.send(event_generator()) }
		}
}

fn event_consumer<T: Event + Debug>(
	mut event: EventReader<T>,
)
{
	for ev in event.into_iter() {
		println!("{:?}", ev);
	}
}

fn spawn_manager<Action>(
	bindings: impl IntoIterator<Item = (impl Into<UserInput>, Action)> + Copy,
) -> impl Fn(Commands)
where
	Action: Actionlike
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