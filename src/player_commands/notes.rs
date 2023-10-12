use bevy::prelude::*;
use bevy_audio::PlaybackMode;
use std::fmt::Display;

use super::staff::CommandStaff;

#[derive(Debug, Clone, Copy, PartialEq)]
pub enum NoteLetter
{
	C, D, E, F, G, A, B
}

#[derive(Debug, Clone, Copy, PartialEq)]
pub struct Note
{
	pub note_letter: NoteLetter,
	pub sharp: bool,
	pub octave: u8,

	/// In Hz
	pub frequency: f32,
}

impl Display for Note
{
	fn fmt(&self, f: &mut std::fmt::Formatter<'_>) -> std::fmt::Result
	{
		write!(f, "{:?}{}{}", self.note_letter, if self.sharp {"#"} else {""}, self.octave)
	}
}

#[allow(dead_code)]
impl Note
{
	pub const C0:  Note = Note { note_letter: NoteLetter::C, sharp: false, octave: 0, frequency:   16.35, };
	pub const CS0: Note = Note { note_letter: NoteLetter::C, sharp: true,  octave: 0, frequency:   17.32, };
	pub const D0:  Note = Note { note_letter: NoteLetter::D, sharp: false, octave: 0, frequency:   18.35, };
	pub const DS0: Note = Note { note_letter: NoteLetter::D, sharp: true,  octave: 0, frequency:   19.45, };
	pub const E0:  Note = Note { note_letter: NoteLetter::E, sharp: false, octave: 0, frequency:   20.60, };
	pub const F0:  Note = Note { note_letter: NoteLetter::F, sharp: false, octave: 0, frequency:   21.83, };
	pub const FS0: Note = Note { note_letter: NoteLetter::F, sharp: true,  octave: 0, frequency:   23.12, };
	pub const G0:  Note = Note { note_letter: NoteLetter::G, sharp: false, octave: 0, frequency:   24.50, };
	pub const GS0: Note = Note { note_letter: NoteLetter::G, sharp: true,  octave: 0, frequency:   25.96, };
	pub const A0:  Note = Note { note_letter: NoteLetter::A, sharp: false, octave: 0, frequency:   27.50, };
	pub const AS0: Note = Note { note_letter: NoteLetter::A, sharp: true,  octave: 0, frequency:   29.14, };
	pub const B0:  Note = Note { note_letter: NoteLetter::B, sharp: false, octave: 0, frequency:   30.87, };
	pub const C1:  Note = Note { note_letter: NoteLetter::C, sharp: false, octave: 1, frequency:   32.70, };
	pub const CS1: Note = Note { note_letter: NoteLetter::C, sharp: true,  octave: 1, frequency:   34.65, };
	pub const D1:  Note = Note { note_letter: NoteLetter::D, sharp: false, octave: 1, frequency:   36.71, };
	pub const DS1: Note = Note { note_letter: NoteLetter::D, sharp: true,  octave: 1, frequency:   38.89, };
	pub const E1:  Note = Note { note_letter: NoteLetter::E, sharp: false, octave: 1, frequency:   41.20, };
	pub const F1:  Note = Note { note_letter: NoteLetter::F, sharp: false, octave: 1, frequency:   43.65, };
	pub const FS1: Note = Note { note_letter: NoteLetter::F, sharp: true,  octave: 1, frequency:   46.25, };
	pub const G1:  Note = Note { note_letter: NoteLetter::G, sharp: false, octave: 1, frequency:   49.00, };
	pub const GS1: Note = Note { note_letter: NoteLetter::G, sharp: true,  octave: 1, frequency:   51.91, };
	pub const A1:  Note = Note { note_letter: NoteLetter::A, sharp: false, octave: 1, frequency:   55.00, };
	pub const AS1: Note = Note { note_letter: NoteLetter::A, sharp: true,  octave: 1, frequency:   58.27, };
	pub const B1:  Note = Note { note_letter: NoteLetter::B, sharp: false, octave: 1, frequency:   61.74, };
	pub const C2:  Note = Note { note_letter: NoteLetter::C, sharp: false, octave: 2, frequency:   65.41, };
	pub const CS2: Note = Note { note_letter: NoteLetter::C, sharp: true,  octave: 2, frequency:   69.30, };
	pub const D2:  Note = Note { note_letter: NoteLetter::D, sharp: false, octave: 2, frequency:   73.42, };
	pub const DS2: Note = Note { note_letter: NoteLetter::D, sharp: true,  octave: 2, frequency:   77.78, };
	pub const E2:  Note = Note { note_letter: NoteLetter::E, sharp: false, octave: 2, frequency:   82.41, };
	pub const F2:  Note = Note { note_letter: NoteLetter::F, sharp: false, octave: 2, frequency:   87.31, };
	pub const FS2: Note = Note { note_letter: NoteLetter::F, sharp: true,  octave: 2, frequency:   92.50, };
	pub const G2:  Note = Note { note_letter: NoteLetter::G, sharp: false, octave: 2, frequency:   98.00, };
	pub const GS2: Note = Note { note_letter: NoteLetter::G, sharp: true,  octave: 2, frequency:  103.83, };
	pub const A2:  Note = Note { note_letter: NoteLetter::A, sharp: false, octave: 2, frequency:  110.00, };
	pub const AS2: Note = Note { note_letter: NoteLetter::A, sharp: true,  octave: 2, frequency:  116.54, };
	pub const B2:  Note = Note { note_letter: NoteLetter::B, sharp: false, octave: 2, frequency:  123.47, };
	pub const C3:  Note = Note { note_letter: NoteLetter::C, sharp: false, octave: 3, frequency:  130.81, };
	pub const CS3: Note = Note { note_letter: NoteLetter::C, sharp: true,  octave: 3, frequency:  138.59, };
	pub const D3:  Note = Note { note_letter: NoteLetter::D, sharp: false, octave: 3, frequency:  146.83, };
	pub const DS3: Note = Note { note_letter: NoteLetter::D, sharp: true,  octave: 3, frequency:  155.56, };
	pub const E3:  Note = Note { note_letter: NoteLetter::E, sharp: false, octave: 3, frequency:  164.81, };
	pub const F3:  Note = Note { note_letter: NoteLetter::F, sharp: false, octave: 3, frequency:  174.61, };
	pub const FS3: Note = Note { note_letter: NoteLetter::F, sharp: true,  octave: 3, frequency:  185.00, };
	pub const G3:  Note = Note { note_letter: NoteLetter::G, sharp: false, octave: 3, frequency:  196.00, };
	pub const GS3: Note = Note { note_letter: NoteLetter::G, sharp: true,  octave: 3, frequency:  207.65, };
	pub const A3:  Note = Note { note_letter: NoteLetter::A, sharp: false, octave: 3, frequency:  220.00, };
	pub const AS3: Note = Note { note_letter: NoteLetter::A, sharp: true,  octave: 3, frequency:  233.08, };
	pub const B3:  Note = Note { note_letter: NoteLetter::B, sharp: false, octave: 3, frequency:  246.94, };
	pub const C4:  Note = Note { note_letter: NoteLetter::C, sharp: false, octave: 4, frequency:  261.63, };
	pub const CS4: Note = Note { note_letter: NoteLetter::C, sharp: true,  octave: 4, frequency:  277.18, };
	pub const D4:  Note = Note { note_letter: NoteLetter::D, sharp: false, octave: 4, frequency:  293.66, };
	pub const DS4: Note = Note { note_letter: NoteLetter::D, sharp: true,  octave: 4, frequency:  311.13, };
	pub const E4:  Note = Note { note_letter: NoteLetter::E, sharp: false, octave: 4, frequency:  329.63, };
	pub const F4:  Note = Note { note_letter: NoteLetter::F, sharp: false, octave: 4, frequency:  349.23, };
	pub const FS4: Note = Note { note_letter: NoteLetter::F, sharp: true,  octave: 4, frequency:  369.99, };
	pub const G4:  Note = Note { note_letter: NoteLetter::G, sharp: false, octave: 4, frequency:  392.00, };
	pub const GS4: Note = Note { note_letter: NoteLetter::G, sharp: true,  octave: 4, frequency:  415.30, };
	pub const A4:  Note = Note { note_letter: NoteLetter::A, sharp: false, octave: 4, frequency:  440.00, };
	pub const AS4: Note = Note { note_letter: NoteLetter::A, sharp: true,  octave: 4, frequency:  466.16, };
	pub const B4:  Note = Note { note_letter: NoteLetter::B, sharp: false, octave: 4, frequency:  493.88, };
	pub const C5:  Note = Note { note_letter: NoteLetter::C, sharp: false, octave: 5, frequency:  523.25, };
	pub const CS5: Note = Note { note_letter: NoteLetter::C, sharp: true,  octave: 5, frequency:  554.37, };
	pub const D5:  Note = Note { note_letter: NoteLetter::D, sharp: false, octave: 5, frequency:  587.33, };
	pub const DS5: Note = Note { note_letter: NoteLetter::D, sharp: true,  octave: 5, frequency:  622.25, };
	pub const E5:  Note = Note { note_letter: NoteLetter::E, sharp: false, octave: 5, frequency:  659.25, };
	pub const F5:  Note = Note { note_letter: NoteLetter::F, sharp: false, octave: 5, frequency:  698.46, };
	pub const FS5: Note = Note { note_letter: NoteLetter::F, sharp: true,  octave: 5, frequency:  739.99, };
	pub const G5:  Note = Note { note_letter: NoteLetter::G, sharp: false, octave: 5, frequency:  783.99, };
	pub const GS5: Note = Note { note_letter: NoteLetter::G, sharp: true,  octave: 5, frequency:  830.61, };
	pub const A5:  Note = Note { note_letter: NoteLetter::A, sharp: false, octave: 5, frequency:  880.00, };
	pub const AS5: Note = Note { note_letter: NoteLetter::A, sharp: true,  octave: 5, frequency:  932.33, };
	pub const B5:  Note = Note { note_letter: NoteLetter::B, sharp: false, octave: 5, frequency:  987.77, };
	pub const C6:  Note = Note { note_letter: NoteLetter::C, sharp: false, octave: 6, frequency: 1046.50, };
	pub const CS6: Note = Note { note_letter: NoteLetter::C, sharp: true,  octave: 6, frequency: 1108.73, };
	pub const D6:  Note = Note { note_letter: NoteLetter::D, sharp: false, octave: 6, frequency: 1174.66, };
	pub const DS6: Note = Note { note_letter: NoteLetter::D, sharp: true,  octave: 6, frequency: 1244.51, };
	pub const E6:  Note = Note { note_letter: NoteLetter::E, sharp: false, octave: 6, frequency: 1318.51, };
	pub const F6:  Note = Note { note_letter: NoteLetter::F, sharp: false, octave: 6, frequency: 1396.91, };
	pub const FS6: Note = Note { note_letter: NoteLetter::F, sharp: true,  octave: 6, frequency: 1479.98, };
	pub const G6:  Note = Note { note_letter: NoteLetter::G, sharp: false, octave: 6, frequency: 1567.98, };
	pub const GS6: Note = Note { note_letter: NoteLetter::G, sharp: true,  octave: 6, frequency: 1661.22, };
	pub const A6:  Note = Note { note_letter: NoteLetter::A, sharp: false, octave: 6, frequency: 1760.00, };
	pub const AS6: Note = Note { note_letter: NoteLetter::A, sharp: true,  octave: 6, frequency: 1864.66, };
	pub const B6:  Note = Note { note_letter: NoteLetter::B, sharp: false, octave: 6, frequency: 1975.53, };
	pub const C7:  Note = Note { note_letter: NoteLetter::C, sharp: false, octave: 7, frequency: 2093.00, };
	pub const CS7: Note = Note { note_letter: NoteLetter::C, sharp: true,  octave: 7, frequency: 2217.46, };
	pub const D7:  Note = Note { note_letter: NoteLetter::D, sharp: false, octave: 7, frequency: 2349.83, };
	pub const DS7: Note = Note { note_letter: NoteLetter::D, sharp: true,  octave: 7, frequency: 2489.02, };
	pub const E7:  Note = Note { note_letter: NoteLetter::E, sharp: false, octave: 7, frequency: 2637.02, };
	pub const F7:  Note = Note { note_letter: NoteLetter::F, sharp: false, octave: 7, frequency: 2793.83, };
	pub const FS7: Note = Note { note_letter: NoteLetter::F, sharp: true,  octave: 7, frequency: 2959.96, };
	pub const G7:  Note = Note { note_letter: NoteLetter::G, sharp: false, octave: 7, frequency: 3135.96, };
	pub const GS7: Note = Note { note_letter: NoteLetter::G, sharp: true,  octave: 7, frequency: 3322.44, };
	pub const A7:  Note = Note { note_letter: NoteLetter::A, sharp: false, octave: 7, frequency: 3520.00, };
	pub const AS7: Note = Note { note_letter: NoteLetter::A, sharp: true,  octave: 7, frequency: 3729.31, };
	pub const B7:  Note = Note { note_letter: NoteLetter::B, sharp: false, octave: 7, frequency: 3951.07, };
	pub const C8:  Note = Note { note_letter: NoteLetter::C, sharp: false, octave: 8, frequency: 4186.01, };
	pub const CS8: Note = Note { note_letter: NoteLetter::C, sharp: true,  octave: 8, frequency: 4434.92, };
	pub const D8:  Note = Note { note_letter: NoteLetter::D, sharp: false, octave: 8, frequency: 4698.63, };
	pub const DS8: Note = Note { note_letter: NoteLetter::D, sharp: true,  octave: 8, frequency: 4978.03, };
	pub const E8:  Note = Note { note_letter: NoteLetter::E, sharp: false, octave: 8, frequency: 5274.04, };
	pub const F8:  Note = Note { note_letter: NoteLetter::F, sharp: false, octave: 8, frequency: 5587.65, };
	pub const FS8: Note = Note { note_letter: NoteLetter::F, sharp: true,  octave: 8, frequency: 5919.91, };
	pub const G8:  Note = Note { note_letter: NoteLetter::G, sharp: false, octave: 8, frequency: 6271.93, };
	pub const GS8: Note = Note { note_letter: NoteLetter::G, sharp: true,  octave: 8, frequency: 6644.88, };
	pub const A8:  Note = Note { note_letter: NoteLetter::A, sharp: false, octave: 8, frequency: 7040.00, };
	pub const AS8: Note = Note { note_letter: NoteLetter::A, sharp: true,  octave: 8, frequency: 7458.62, };
	pub const B8:  Note = Note { note_letter: NoteLetter::B, sharp: false, octave: 8, frequency: 7902.13, };
	
	/// Relative to C0 on a C major scale
	pub fn position(&self) -> i32
	{
		self.octave as i32 * 7 + self.note_letter as i32
	}
}

#[derive(Event)]
pub struct NotePlayedEvent(pub Note);

#[derive(Event)]
pub struct ClearNotesEvent;

pub fn play_notes(
	staffs: Query<&CommandStaff>,
	input: Res<Input<KeyCode>>,
	mut ev_note_played: EventWriter<NotePlayedEvent>,
)
{
	if input.get_just_pressed().next().is_none() {
		return;
	}

	let staff = staffs.single();
	if !staff.is_open {
		return;
	}
	
	let mut play_note_if_pressed = |
		key: KeyCode,
		note: Note,
	|
	{
		if !input.just_pressed(key) {
			return;
		}

		ev_note_played.send(NotePlayedEvent(note));
	};
	
	play_note_if_pressed(KeyCode::Z, Note::C4);
	play_note_if_pressed(KeyCode::S, Note::CS4);
	play_note_if_pressed(KeyCode::X, Note::D4);
	play_note_if_pressed(KeyCode::D, Note::DS4);
	play_note_if_pressed(KeyCode::C, Note::E4);
	play_note_if_pressed(KeyCode::V, Note::F4);
	play_note_if_pressed(KeyCode::G, Note::FS4);
	play_note_if_pressed(KeyCode::B, Note::G4);
	play_note_if_pressed(KeyCode::H, Note::GS4);
	play_note_if_pressed(KeyCode::N, Note::A4);
	play_note_if_pressed(KeyCode::J, Note::AS4);
	play_note_if_pressed(KeyCode::M, Note::B4);
	
	play_note_if_pressed(KeyCode::Comma, Note::C5);
	play_note_if_pressed(KeyCode::L, Note::CS5);
	play_note_if_pressed(KeyCode::Period, Note::D5);
	play_note_if_pressed(KeyCode::Semicolon, Note::DS5);
	play_note_if_pressed(KeyCode::Slash, Note::E5);
	
	play_note_if_pressed(KeyCode::Q, Note::C5);
	play_note_if_pressed(KeyCode::Key2, Note::CS5);
	play_note_if_pressed(KeyCode::W, Note::D5);
	play_note_if_pressed(KeyCode::Key3, Note::DS5);
	play_note_if_pressed(KeyCode::E, Note::E5);
	play_note_if_pressed(KeyCode::R, Note::F5);
	play_note_if_pressed(KeyCode::Key5, Note::FS5);
	play_note_if_pressed(KeyCode::T, Note::G5);
	play_note_if_pressed(KeyCode::Key6, Note::GS5);
	play_note_if_pressed(KeyCode::Y, Note::A5);
	play_note_if_pressed(KeyCode::Key7, Note::AS5);
	play_note_if_pressed(KeyCode::U, Note::B5);
	
	play_note_if_pressed(KeyCode::I, Note::C6);
	play_note_if_pressed(KeyCode::Key9, Note::CS6);
	play_note_if_pressed(KeyCode::O, Note::D6);
	play_note_if_pressed(KeyCode::Key0, Note::DS6);
	play_note_if_pressed(KeyCode::P, Note::E6);
}

pub fn spawn_note_audio(
	mut commands: Commands,
	mut ev_note_played: EventReader<NotePlayedEvent>,
	asset_server: Res<AssetServer>,
)
{
	for ev in ev_note_played.iter()
	{
		let note = ev.0;

		commands.spawn(AudioBundle
		{
			source: asset_server.load("flute.wav").clone(),
			settings: PlaybackSettings
			{
				mode: PlaybackMode::Despawn,
				speed: note.frequency / Note::C4.frequency,
				..default()
			},
		});
	}
}

pub fn clear_notes(
	mut ev_clear_notes: EventWriter<ClearNotesEvent>,
)
{
	ev_clear_notes.send(ClearNotesEvent);
}