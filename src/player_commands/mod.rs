mod notes;
mod commands;
mod staff;
mod note_holder;

use bevy::prelude::*;
use leafwing_input_manager::prelude::*;

use crate::input::action_event;
use crate::input::button_event;
use crate::input::spawn_input_manager_with_bindings;

use self::note_holder::*;
use self::notes::*;
use self::commands::*;
use self::staff::*;

pub struct PlayerCommandsPlugin;

impl Plugin for PlayerCommandsPlugin
{
	fn build(&self, app: &mut App) {
		app
			.add_plugins(InputManagerPlugin::<ToggleStaffAction>::default())
			.add_plugins(InputManagerPlugin::<PlayNoteAction>::default())
			.insert_resource(ToggleActions::<PlayNoteAction>::DISABLED)
			
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
				spawn_input_manager_with_bindings([
					(KeyCode::Z, PlayNoteAction::C4),
					(KeyCode::S, PlayNoteAction::CS4),
					(KeyCode::X, PlayNoteAction::D4),
					(KeyCode::D, PlayNoteAction::DS4),
					(KeyCode::C, PlayNoteAction::E4),
					(KeyCode::V, PlayNoteAction::F4),
					(KeyCode::G, PlayNoteAction::FS4),
					(KeyCode::B, PlayNoteAction::G4),
					(KeyCode::H, PlayNoteAction::GS4),
					(KeyCode::N, PlayNoteAction::A4),
					(KeyCode::J, PlayNoteAction::AS4),
					(KeyCode::M, PlayNoteAction::B4),
					
					(KeyCode::Comma, PlayNoteAction::C5),
					(KeyCode::L, PlayNoteAction::CS5),
					(KeyCode::Period, PlayNoteAction::D5),
					(KeyCode::Semicolon, PlayNoteAction::DS5),
					(KeyCode::Slash, PlayNoteAction::E5),
					
					(KeyCode::Q, PlayNoteAction::C5),
					(KeyCode::Key2, PlayNoteAction::CS5),
					(KeyCode::W, PlayNoteAction::D5),
					(KeyCode::Key3, PlayNoteAction::DS5),
					(KeyCode::E, PlayNoteAction::E5),
					(KeyCode::R, PlayNoteAction::F5),
					(KeyCode::Key5, PlayNoteAction::FS5),
					(KeyCode::T, PlayNoteAction::G5),
					(KeyCode::Key6, PlayNoteAction::GS5),
					(KeyCode::Y, PlayNoteAction::A5),
					(KeyCode::Key7, PlayNoteAction::AS5),
					(KeyCode::U, PlayNoteAction::B5),
					
					(KeyCode::I, PlayNoteAction::C6),
					(KeyCode::Key9, PlayNoteAction::CS6),
					(KeyCode::O, PlayNoteAction::D6),
					(KeyCode::Key0, PlayNoteAction::DS6),
					(KeyCode::P, PlayNoteAction::E6),
				]),
				spawn_input_manager_with_bindings([
					(KeyCode::Grave, ToggleStaffAction::ToggleStaff),
				])
			))

			.add_systems(PreUpdate, (
				action_event(|action: PlayNoteAction| NotePlayedEvent(action.note())),
				button_event(ToggleStaffAction::ToggleStaff, ToggleStaffEvent::default)
			))

			.add_systems(Update, toggle_staff.run_if(on_event::<ToggleStaffEvent>()))

			.add_systems(Update, (
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