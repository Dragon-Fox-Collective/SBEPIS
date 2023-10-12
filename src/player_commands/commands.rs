use std::marker::PhantomData;

use bevy::prelude::*;
use bevy_audio::PlaybackMode;

use super::notes::{Note, NotePlayedEvent};

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

#[derive(Event, Default)]
pub struct PingCommandEvent;

pub fn check_note_patterns<T: Event + Default>(
	note_holder: Res<NotePatternPlayer>,
	pattern: Res<NotePattern<T>>,
	mut ev_command: EventWriter<T>,
	mut ev_command_sent: EventWriter<CommandSentEvent>,
)
{
	if pattern.pattern == note_holder.current_pattern {
		ev_command.send(T::default());
		ev_command_sent.send(CommandSentEvent);
	}
}

pub fn ping(
	mut commands: Commands,
	asset_server: Res<AssetServer>,
)
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

#[derive(Resource, Default)]
pub struct NotePatternPlayer
{
	pub current_pattern: Vec<Note>,
}

#[derive(Event)]
pub struct CommandSentEvent;

pub fn add_note_to_player(
	mut player: ResMut<NotePatternPlayer>,
	mut ev_note_played: EventReader<NotePlayedEvent>,
)
{
	for ev in ev_note_played.iter()
	{
		player.current_pattern.push(ev.0);
	}
}

pub fn clear_player_notes(
	mut player: ResMut<NotePatternPlayer>,
)
{
	player.current_pattern.clear();
}