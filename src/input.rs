use core::fmt::Debug;

use bevy::prelude::*;
use leafwing_input_manager::plugin::InputManagerSystem;
use leafwing_input_manager::prelude::*;

pub struct InputPlugin;
impl Plugin for InputPlugin
{
	fn build(&self, app: &mut App) {
		app
			.add_plugins(InputManagerPlugin::<MovementAction>::default())
			.add_plugins(InputManagerPlugin::<NoteAction>::default())
			.add_event::<MoveForward>()
			.add_event::<PlayNoteD5>()
			.add_systems(Startup, (
				spawn_manager([(KeyCode::W, MovementAction::Forward)]),
				spawn_manager([(KeyCode::W, NoteAction::D5)]),
			))
			.add_systems(PreUpdate, (
				button_event(MovementAction::Forward, MoveForward::default).after(InputManagerSystem::ManualControl),
				button_event(NoteAction::D5, PlayNoteD5::default).after(InputManagerSystem::ManualControl),
			))
			.add_systems(Update, (
				event_consumer::<MoveForward>,
				event_consumer::<PlayNoteD5>,
			))
			;
	}
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