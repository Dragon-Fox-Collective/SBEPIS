use bevy::{prelude::*, ecs::schedule::SystemConfigs};
use leafwing_input_manager::{prelude::*, plugin::InputManagerSystem};

pub fn spawn_input_manager<Action: Actionlike>(
	bindings: impl IntoIterator<Item = (impl Into<UserInput>, Action)> + Copy + Send + Sync + 'static,
) -> SystemConfigs
{
	(move |
		mut commands: Commands,
	|
	{
		commands.spawn(InputManagerBundle::<Action> {
			action_state: ActionState::default(),
			input_map: InputMap::new(bindings),
		});
	}).into_configs()
}

pub fn button_event<Action: Actionlike + Copy, EventType: Event>(
	action: Action,
	event_generator: impl Fn() -> EventType + Send + Sync + 'static,
) -> SystemConfigs
{
	(move |
		input: Query<&ActionState<Action>>,
		mut event: EventWriter<EventType>,
	|
	{
		let input = input.single();
		if input.just_pressed(action) { event.send(event_generator()); }
	}).after(InputManagerSystem::ManualControl)
}

pub fn action_event<Action: Actionlike + Copy, EventType: Event>(
	event_generator: impl Fn(Action) -> EventType + Send + Sync + 'static,
) -> SystemConfigs
{
	(move |
		input: Query<&ActionState<Action>>,
		mut event: EventWriter<EventType>,
	|
	{
		let input = input.single();
		for action in input.get_just_pressed() {
			event.send(event_generator(action));
		}
	}).after(InputManagerSystem::ManualControl)
}