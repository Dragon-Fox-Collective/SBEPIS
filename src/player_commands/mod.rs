mod notes;
mod commands;
mod staff;
mod note_holder;

use bevy::prelude::*;
use bevy_input::common_conditions::input_just_pressed;

use self::note_holder::*;
use self::notes::*;
use self::commands::*;
use self::staff::*;

pub struct PlayerCommandsPlugin;

impl Plugin for PlayerCommandsPlugin
{
	fn build(&self, app: &mut App) {
		app
			.add_event::<NotePlayedEvent>()
			.add_event::<CommandSentEvent>()
			.add_event::<ClearNotesEvent>()

			.add_event::<PingCommandEvent>()
			.insert_resource(NotePattern::<PingCommandEvent>::new(vec![Note::C4, Note::D4, Note::E4]))

			.insert_resource(NotePatternPlayer::default())

			.add_systems(Startup, (
				spawn_staff,
				#[cfg(feature = "spawn_debug_notes_on_staff")]
				(
					apply_deferred,
					spawn_debug_notes,
				).chain().after(spawn_staff),
			))

			.add_systems(Update, (
				toggle_staffs.run_if(input_just_pressed(KeyCode::Grave)),
				play_notes,
				(
					spawn_note_audio,
					add_note_to_holder,
					add_note_to_player,
				).run_if(on_event::<NotePlayedEvent>()),
				(
					check_note_patterns::<PingCommandEvent>,
				),
				(
					clear_notes.run_if(on_event::<CommandSentEvent>()),
					ping.run_if(on_event::<PingCommandEvent>()),
				),
				(
					clear_holder_notes,
					clear_player_notes,
				).run_if(on_event::<ClearNotesEvent>()),
			).chain())

			;
	}
}