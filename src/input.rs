use core::hash::Hash;
use core::fmt::Debug;

use bevy::prelude::*;
use leafwing_input_manager::prelude::*;

pub struct InputPlugin;
impl Plugin for InputPlugin
{
	fn build(&self, app: &mut App) {
		app
			.add_plugins(InputManagerPlugin)
			.add_event::<MoveForward>()
			.add_event::<PlayNoteD5>()
			.add_systems(Update, (
				(
					button_event(MoveForward::default, KeyCode::W),
					button_event(PlayNoteD5::default, KeyCode::W),
				),
				(
					event_consumer::<MoveForward>,
					event_consumer::<PlayNoteD5>,
				),
			).chain())
			;
	}
}

#[derive(Actionlike)]
enum CameraMovement {
    Pan,
}

#[derive(Event, Default, Debug)]
pub struct MoveForward;

#[derive(Event, Default, Debug)]
pub struct PlayNoteD5;

pub fn button_event<EventType, InputType>(
	event_generator: impl Fn() -> EventType,
	input_type: InputType,
) -> impl Fn(Res<Input<InputType>>, EventWriter<EventType>)
where
	EventType: Event,
	InputType: Copy + Eq + Hash + Send + Sync,
{
	move |
		input: Res<Input<InputType>>,
		mut event: EventWriter<EventType>,
	|
		if input.just_pressed(input_type) { event.send(event_generator()) }
}

fn event_consumer<T: Event + Debug>(
	mut event: EventReader<T>,
)
{
	for ev in event.into_iter() {
		println!("{:?}", ev);
	}
}