use std::marker::PhantomData;

use bevy::prelude::*;
use bevy_audio::PlaybackMode;

use super::{notes::Note, NoteHolder};

#[derive(Resource)]
pub struct NotePattern<T: Event>
{
	pub pattern: Vec<Note>,
	event_type: PhantomData<T>,
}

impl<T: Event> NotePattern<T>
{
	pub fn new(pattern: Vec<Note>) -> Self { Self{ pattern, event_type: PhantomData } }
}

impl<T: Event + Default> NotePattern<T>
{
	pub fn new_event(&self) -> T { T::default() }
}

#[derive(Event, Default)]
pub struct PingCommandEvent;

pub fn check_note_patterns<T: Event + Default>(
	mut commands: Commands,
	mut note_holders: Query<&mut NoteHolder>,
	pattern: Res<NotePattern<T>>,
	mut event_writer: EventWriter<T>,
)
{
	let mut note_holder = note_holders.single_mut();
	
	if pattern.pattern == note_holder.notes {
		event_writer.send(pattern.new_event());
		note_holder.clear_notes(&mut commands);
	}
}

pub fn ping(
	mut commands: Commands,
	mut event_reader: EventReader<PingCommandEvent>,
	asset_server: Res<AssetServer>,
)
{
	for _ in event_reader.iter()
	{
		commands.spawn(AudioBundle
		{
			source: asset_server.load("pester_notif.mp3").clone(),
			settings: PlaybackSettings
			{
				mode: PlaybackMode::Despawn,
				..default()
			},
		});
}
}