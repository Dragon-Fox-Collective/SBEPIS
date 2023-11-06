mod notes;
mod commands;
mod staff;
mod note_holder;

use bevy::input::common_conditions::input_just_pressed;
use bevy::prelude::*;

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
			.add_event::<ToggleStaffEvent>()

			.add_event::<PingCommandEvent>()
			.add_event::<KillCommandEvent>()

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
				// input, an example of a thing that needs to be disabled if something else is enabled
				send_toggle_staff.run_if(input_just_pressed(KeyCode::Grave)),

				toggle_staff.run_if(on_event::<ToggleStaffEvent>()),

				play_notes, // also input
				(
					spawn_note_audio,
					add_note_to_holder,
					add_note_to_player,
				).run_if(on_event::<NotePlayedEvent>()),
				(
					check_note_patterns::<PingCommandEvent>,
					check_note_patterns::<KillCommandEvent>,
				),
				(
					clear_notes.run_if(on_event::<CommandSentEvent>()),
					ping.run_if(on_event::<PingCommandEvent>()),
					kill.run_if(on_event::<KillCommandEvent>()),
				),
				(
					clear_holder_notes,
					clear_player_notes,
				).run_if(on_event::<ClearNotesEvent>()),
			).chain())

			;
	}
}