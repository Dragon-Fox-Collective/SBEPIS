use bevy::audio::PlaybackMode;
use bevy::prelude::*;
use leafwing_input_manager::prelude::*;
use std::fmt::Display;

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

#[derive(Actionlike, Clone, Copy, Reflect)]
pub enum PlayNoteAction {
	C0, CS0, D0, DS0, E0, F0, FS0, G0, GS0, A0, AS0, B0,
	C1, CS1, D1, DS1, E1, F1, FS1, G1, GS1, A1, AS1, B1,
	C2, CS2, D2, DS2, E2, F2, FS2, G2, GS2, A2, AS2, B2,
	C3, CS3, D3, DS3, E3, F3, FS3, G3, GS3, A3, AS3, B3,
	C4, CS4, D4, DS4, E4, F4, FS4, G4, GS4, A4, AS4, B4,
	C5, CS5, D5, DS5, E5, F5, FS5, G5, GS5, A5, AS5, B5,
	C6, CS6, D6, DS6, E6, F6, FS6, G6, GS6, A6, AS6, B6,
	C7, CS7, D7, DS7, E7, F7, FS7, G7, GS7, A7, AS7, B7,
	C8, CS8, D8, DS8, E8, F8, FS8, G8, GS8, A8, AS8, B8,
}

impl PlayNoteAction {
	pub fn note(&self) -> Note
	{
		match self {
			PlayNoteAction::C0 => Note::C0,
			PlayNoteAction::CS0 => Note::CS0,
			PlayNoteAction::D0 => Note::D0,
			PlayNoteAction::DS0 => Note::DS0,
			PlayNoteAction::E0 => Note::E0,
			PlayNoteAction::F0 => Note::F0,
			PlayNoteAction::FS0 => Note::FS0,
			PlayNoteAction::G0 => Note::G0,
			PlayNoteAction::GS0 => Note::GS0,
			PlayNoteAction::A0 => Note::A0,
			PlayNoteAction::AS0 => Note::AS0,
			PlayNoteAction::B0 => Note::B0,
			PlayNoteAction::C1 => Note::C1,
			PlayNoteAction::CS1 => Note::CS1,
			PlayNoteAction::D1 => Note::D1,
			PlayNoteAction::DS1 => Note::DS1,
			PlayNoteAction::E1 => Note::E1,
			PlayNoteAction::F1 => Note::F1,
			PlayNoteAction::FS1 => Note::FS1,
			PlayNoteAction::G1 => Note::G1,
			PlayNoteAction::GS1 => Note::GS1,
			PlayNoteAction::A1 => Note::A1,
			PlayNoteAction::AS1 => Note::AS1,
			PlayNoteAction::B1 => Note::B1,
			PlayNoteAction::C2 => Note::C2,
			PlayNoteAction::CS2 => Note::CS2,
			PlayNoteAction::D2 => Note::D2,
			PlayNoteAction::DS2 => Note::DS2,
			PlayNoteAction::E2 => Note::E2,
			PlayNoteAction::F2 => Note::F2,
			PlayNoteAction::FS2 => Note::FS2,
			PlayNoteAction::G2 => Note::G2,
			PlayNoteAction::GS2 => Note::GS2,
			PlayNoteAction::A2 => Note::A2,
			PlayNoteAction::AS2 => Note::AS2,
			PlayNoteAction::B2 => Note::B2,
			PlayNoteAction::C3 => Note::C3,
			PlayNoteAction::CS3 => Note::CS3,
			PlayNoteAction::D3 => Note::D3,
			PlayNoteAction::DS3 => Note::DS3,
			PlayNoteAction::E3 => Note::E3,
			PlayNoteAction::F3 => Note::F3,
			PlayNoteAction::FS3 => Note::FS3,
			PlayNoteAction::G3 => Note::G3,
			PlayNoteAction::GS3 => Note::GS3,
			PlayNoteAction::A3 => Note::A3,
			PlayNoteAction::AS3 => Note::AS3,
			PlayNoteAction::B3 => Note::B3,
			PlayNoteAction::C4 => Note::C4,
			PlayNoteAction::CS4 => Note::CS4,
			PlayNoteAction::D4 => Note::D4,
			PlayNoteAction::DS4 => Note::DS4,
			PlayNoteAction::E4 => Note::E4,
			PlayNoteAction::F4 => Note::F4,
			PlayNoteAction::FS4 => Note::FS4,
			PlayNoteAction::G4 => Note::G4,
			PlayNoteAction::GS4 => Note::GS4,
			PlayNoteAction::A4 => Note::A4,
			PlayNoteAction::AS4 => Note::AS4,
			PlayNoteAction::B4 => Note::B4,
			PlayNoteAction::C5 => Note::C5,
			PlayNoteAction::CS5 => Note::CS5,
			PlayNoteAction::D5 => Note::D5,
			PlayNoteAction::DS5 => Note::DS5,
			PlayNoteAction::E5 => Note::E5,
			PlayNoteAction::F5 => Note::F5,
			PlayNoteAction::FS5 => Note::FS5,
			PlayNoteAction::G5 => Note::G5,
			PlayNoteAction::GS5 => Note::GS5,
			PlayNoteAction::A5 => Note::A5,
			PlayNoteAction::AS5 => Note::AS5,
			PlayNoteAction::B5 => Note::B5,
			PlayNoteAction::C6 => Note::C6,
			PlayNoteAction::CS6 => Note::CS6,
			PlayNoteAction::D6 => Note::D6,
			PlayNoteAction::DS6 => Note::DS6,
			PlayNoteAction::E6 => Note::E6,
			PlayNoteAction::F6 => Note::F6,
			PlayNoteAction::FS6 => Note::FS6,
			PlayNoteAction::G6 => Note::G6,
			PlayNoteAction::GS6 => Note::GS6,
			PlayNoteAction::A6 => Note::A6,
			PlayNoteAction::AS6 => Note::AS6,
			PlayNoteAction::B6 => Note::B6,
			PlayNoteAction::C7 => Note::C7,
			PlayNoteAction::CS7 => Note::CS7,
			PlayNoteAction::D7 => Note::D7,
			PlayNoteAction::DS7 => Note::DS7,
			PlayNoteAction::E7 => Note::E7,
			PlayNoteAction::F7 => Note::F7,
			PlayNoteAction::FS7 => Note::FS7,
			PlayNoteAction::G7 => Note::G7,
			PlayNoteAction::GS7 => Note::GS7,
			PlayNoteAction::A7 => Note::A7,
			PlayNoteAction::AS7 => Note::AS7,
			PlayNoteAction::B7 => Note::B7,
			PlayNoteAction::C8 => Note::C8,
			PlayNoteAction::CS8 => Note::CS8,
			PlayNoteAction::D8 => Note::D8,
			PlayNoteAction::DS8 => Note::DS8,
			PlayNoteAction::E8 => Note::E8,
			PlayNoteAction::F8 => Note::F8,
			PlayNoteAction::FS8 => Note::FS8,
			PlayNoteAction::G8 => Note::G8,
			PlayNoteAction::GS8 => Note::GS8,
			PlayNoteAction::A8 => Note::A8,
			PlayNoteAction::AS8 => Note::AS8,
			PlayNoteAction::B8 => Note::B8,
		}
	}
}

pub fn spawn_note_audio(
	mut commands: Commands,
	mut ev_note_played: EventReader<NotePlayedEvent>,
	asset_server: Res<AssetServer>,
)
{
	for ev in ev_note_played.read()
	{
		let note = ev.0;

		commands.spawn(AudioBundle
		{
			source: asset_server.load("flute.wav"),
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