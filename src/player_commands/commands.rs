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

pub fn check_note_patterns<T: Event + NotePatternEvent>(
	note_holder: Res<NotePatternPlayer>,
	mut ev_command: EventWriter<T>,
	mut ev_command_sent: EventWriter<CommandSentEvent>,
)
{
	let event = T::compare_notes(note_holder.current_pattern.as_slice());
	if let Some(event) = event {
		ev_command.send(event);
		ev_command_sent.send(CommandSentEvent);
	}
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
	for ev in &mut ev_note_played
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

pub trait NotePatternEvent
{
	fn compare_notes(notes: &[Note]) -> Option<Self> where Self: Sized;
}

pub trait NoteSequence
{
	fn eat(self, notes: &[Note]) -> Option<Self> where Self: Sized;
}

impl NoteSequence for &[Note]
{
	fn eat(self, notes: &[Note]) -> Option<Self>
	{
		if self.starts_with(notes) {
			Some(&self[notes.len()..])
		} else {
			None
		}
	}
}

pub trait NoteSequenceTyped<T>
{
	fn eat_type(self) -> Option<(T, Self)> where Self: Sized;
}

impl NoteSequenceTyped<bool> for &[Note]
{
	fn eat_type(self) -> Option<(bool, Self)> where Self: Sized
	{
		if self.starts_with(&[Note::A4]) {
			Some((true, &self[1..]))
		} else if self.starts_with(&[Note::C5]) {
			Some((false, &self[1..]))
		} else {
			None
		}
	}
}


#[derive(Event)]
pub struct PingCommandEvent;

impl PingCommandEvent
{
	const PATTERN: &[Note] = &[Note::C4, Note::D4, Note::E4];
}

impl NotePatternEvent for PingCommandEvent
{
	fn compare_notes(notes: &[Note]) -> Option<Self> where Self: Sized
	{
		let _notes = notes.eat(PingCommandEvent::PATTERN)?;
		Some(PingCommandEvent)
	}
}

pub fn ping(
	mut commands: Commands,
	asset_server: Res<AssetServer>,
)
{
	commands.spawn(AudioBundle
	{
		source: asset_server.load("pester_notif.mp3"),
		settings: PlaybackSettings
		{
			mode: PlaybackMode::Despawn,
			..default()
		},
	});
}


#[derive(Event)]
pub struct KillCommandEvent(pub bool);

impl KillCommandEvent
{
	const PATTERN: &[Note] = &[Note::D4, Note::D4, Note::D5];
}

impl NotePatternEvent for KillCommandEvent
{
	fn compare_notes(notes: &[Note]) -> Option<Self> where Self: Sized
	{
		let notes = notes.eat(KillCommandEvent::PATTERN)?;
		let (actually_kill, _notes) = notes.eat_type()?;
		Some(KillCommandEvent(actually_kill))
	}
}

pub fn kill(
	mut ev_kill: EventReader<KillCommandEvent>,
	mut ev_quit: EventWriter<bevy::app::AppExit>,
)
{
	for ev in &mut ev_kill {
		println!("Tried to kill {}", ev.0);
		if ev.0 {
			ev_quit.send(bevy::app::AppExit);
		}
	}
}