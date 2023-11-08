use bevy::{prelude::*, ecs::schedule::SystemConfigs};
use leafwing_input_manager::{prelude::*, plugin::InputManagerSystem};

pub fn spawn_input_manager<Action: Actionlike>(
	input_map: InputMap<Action>,
) -> SystemConfigs
{
	(move |
		mut commands: Commands,
	|
	{
		commands.spawn(InputManagerBundle::<Action> {
			input_map: input_map.clone(),
			..default()
		});
	}).into_configs()
}

pub fn spawn_input_manager_with_bindings<Action: Actionlike>(
	bindings: impl IntoIterator<Item = (impl Into<UserInput>, Action)>,
) -> SystemConfigs
{
	spawn_input_manager(InputMap::new(bindings))
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

pub fn button_input<Action: Actionlike + Copy>(
	action: Action,
) -> impl Fn(Query<&ActionState<Action>>) -> bool
{
	move |
		input: Query<&ActionState<Action>>,
	|
	{
		let input = input.single();
		input.pressed(action)
	}
}

pub fn dual_axes_input<Action: Actionlike + Copy>(
	action: Action,
) -> impl Fn(Query<&ActionState<Action>>) -> Vec2
{
	move |
		input: Query<&ActionState<Action>>,
	|
	{
		let input = input.single();
		input.axis_pair(action).unwrap_or_default().xy()
	}
}

pub fn clamped_dual_axes_input<Action: Actionlike + Copy>(
	action: Action,
) -> impl Fn(Query<&ActionState<Action>>) -> Vec2
{
	move |
		input: Query<&ActionState<Action>>,
	|
	{
		let input = input.single();
		input.clamped_axis_pair(action).unwrap_or_default().xy()
	}
}