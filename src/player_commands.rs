use bevy::prelude::*;
use bevy_input::common_conditions::input_just_pressed;
use bevy_audio::PlaybackMode;
use super::util::MapRange;

pub struct PlayerCommandsPlugin;

impl Plugin for PlayerCommandsPlugin
{
	fn build(&self, app: &mut App) {
		app
			.add_systems(Startup, spawn_staff)
			.add_systems(Update, (
				toggle_staffs.run_if(input_just_pressed(KeyCode::Grave)),
				play_notes,
			))
			;
	}
}

#[derive(Component, Reflect, Default)]
pub struct CommandStaff
{
	pub is_open: bool,
}

fn spawn_staff(
	mut commands: Commands,
	asset_server: Res<AssetServer>,
)
{
	let treble_clef = asset_server.load("treble_clef.png");
	let quarter_note = asset_server.load("quarter_note.png");
	
	// This should be enough information to map all notes
	let f5_line_top = 15.0;
	let staff_height = 60.0;
	let clef_height = 80.0;

	let quarter_note_top = 60.0;
	let quarter_note_height = 55.0;
	let quarter_note_left_start = 40.0;
	let quarter_note_left_spacing = 20.0;

	// Background
	commands
		.spawn((
			NodeBundle
			{
				style: Style
				{
					width: Val::Percent(100.0),
					height: Val::Px(100.0),
					flex_direction: FlexDirection::Row,
					margin: UiRect::all(Val::Px(10.0)),
					padding: UiRect::axes(Val::Px(100.0), Val::Px(10.0)),
					display: Display::None,
					..default()
				},
				background_color: Color::BEIGE.into(),
				..default()
			},
			CommandStaff::default(),
		))
		.with_children(|parent|
		{
			// Clef
			parent
				.spawn(ImageBundle
				{
					image: treble_clef.into(),
					style: Style
					{
						position_type: PositionType::Absolute,
						height: Val::Px(clef_height),
						..default()
					},
					..default()
				});

			// Notes
			parent
				.spawn(NodeBundle
				{
					style: Style
					{
						flex_direction: FlexDirection::Column,
						height: Val::Px(staff_height),
						padding: UiRect::top(Val::Px(f5_line_top)),
						..default()
					},
					..default()
				})
				.with_children(|parent|
				{
					for i in 0..10
					{
						let note_top = (i as f32 + Note::E4 as u8 as f32).map(Note::E4 as u8 as f32, Note::F5 as u8 as f32 - 1.0, f5_line_top + staff_height, f5_line_top) - quarter_note_top;

						parent
							.spawn(ImageBundle
							{
								image: quarter_note.clone().into(),
								style: Style
								{
									position_type: PositionType::Absolute,
									left: Val::Px(quarter_note_left_start + i as f32 * quarter_note_left_spacing),
									top: Val::Px(note_top),
									height: Val::Px(quarter_note_height),
									..default()
								},
								..default()
							});
					}
				});

			// Staff lines
			parent
				.spawn(NodeBundle
				{
					style: Style
					{
						flex_direction: FlexDirection::Column,
						flex_grow: 1.0,
						padding: UiRect::top(Val::Px(f5_line_top)),
						height: Val::Px(staff_height),
						justify_content: JustifyContent::SpaceBetween,
						..default()
					},
					..default()
				})
				.with_children(|parent|
				{
					for _ in 0..5
					{
						parent.spawn(NodeBundle
						{
							style: Style
							{
								width: Val::Percent(100.0),
								height: Val::Px(2.0),
								..default()
							},
							background_color: Color::BLACK.into(),
							..default()
						});
					}
				});
		});
}

fn toggle_staffs(
	mut staffs: Query<(&mut CommandStaff, &mut Style)>
)
{
	for (mut staff, mut style) in &mut staffs
	{
		if staff.is_open { close_staff(&mut staff, &mut style) }
		else { open_staff(&mut staff, &mut style) }
	}
}

fn open_staff(
	staff: &mut CommandStaff,
	style: &mut Style,
)
{
	staff.is_open = true;
	style.display = Display::Flex;
}

fn close_staff(
	staff: &mut CommandStaff,
	style: &mut Style,
)
{
	staff.is_open = false;
	style.display = Display::None;
}

#[derive(Debug)]
#[allow(dead_code)]
enum Note
{
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

impl Note
{
	/// In Hz
	fn frequency(&self) -> f32
	{
		match self
		{
			Note::C0 => 16.35,
			Note::CS0 => 17.32,
			Note::D0 => 18.35,
			Note::DS0 => 19.45,
			Note::E0 => 20.60,
			Note::F0 => 21.83,
			Note::FS0 => 23.12,
			Note::G0 => 24.50,
			Note::GS0 => 25.96,
			Note::A0 => 27.50,
			Note::AS0 => 29.14,
			Note::B0 => 30.87,
			Note::C1 => 32.70,
			Note::CS1 => 34.65,
			Note::D1 => 36.71,
			Note::DS1 => 38.89,
			Note::E1 => 41.20,
			Note::F1 => 43.65,
			Note::FS1 => 46.25,
			Note::G1 => 49.00,
			Note::GS1 => 51.91,
			Note::A1 => 55.00,
			Note::AS1 => 58.27,
			Note::B1 => 61.74,
			Note::C2 => 65.41,
			Note::CS2 => 69.30,
			Note::D2 => 73.42,
			Note::DS2 => 77.78,
			Note::E2 => 82.41,
			Note::F2 => 87.31,
			Note::FS2 => 92.50,
			Note::G2 => 98.00,
			Note::GS2 => 103.83,
			Note::A2 => 110.00,
			Note::AS2 => 116.54,
			Note::B2 => 123.47,
			Note::C3 => 130.81,
			Note::CS3 => 138.59,
			Note::D3 => 146.83,
			Note::DS3 => 155.56,
			Note::E3 => 164.81,
			Note::F3 => 174.61,
			Note::FS3 => 185.00,
			Note::G3 => 196.00,
			Note::GS3 => 207.65,
			Note::A3 => 220.00,
			Note::AS3 => 233.08,
			Note::B3 => 246.94,
			Note::C4 => 261.63,
			Note::CS4 => 277.18,
			Note::D4 => 293.66,
			Note::DS4 => 311.13,
			Note::E4 => 329.63,
			Note::F4 => 349.23,
			Note::FS4 => 369.99,
			Note::G4 => 392.00,
			Note::GS4 => 415.30,
			Note::A4 => 440.00,
			Note::AS4 => 466.16,
			Note::B4 => 493.88,
			Note::C5 => 523.25,
			Note::CS5 => 554.37,
			Note::D5 => 587.33,
			Note::DS5 => 622.25,
			Note::E5 => 659.25,
			Note::F5 => 698.46,
			Note::FS5 => 739.99,
			Note::G5 => 783.99,
			Note::GS5 => 830.61,
			Note::A5 => 880.00,
			Note::AS5 => 932.33,
			Note::B5 => 987.77,
			Note::C6 => 1046.50,
			Note::CS6 => 1108.73,
			Note::D6 => 1174.66,
			Note::DS6 => 1244.51,
			Note::E6 => 1318.51,
			Note::F6 => 1396.91,
			Note::FS6 => 1479.98,
			Note::G6 => 1567.98,
			Note::GS6 => 1661.22,
			Note::A6 => 1760.00,
			Note::AS6 => 1864.66,
			Note::B6 => 1975.53,
			Note::C7 => 2093.00,
			Note::CS7 => 2217.46,
			Note::D7 => 2349.83,
			Note::DS7 => 2489.02,
			Note::E7 => 2637.02,
			Note::F7 => 2793.83,
			Note::FS7 => 2959.96,
			Note::G7 => 3135.96,
			Note::GS7 => 3322.44,
			Note::A7 => 3520.00,
			Note::AS7 => 3729.31,
			Note::B7 => 3951.07,
			Note::C8 => 4186.01,
			Note::CS8 => 4434.92,
			Note::D8 => 4698.63,
			Note::DS8 => 4978.03,
			Note::E8 => 5274.04,
			Note::F8 => 5587.65,
			Note::FS8 => 5919.91,
			Note::G8 => 6271.93,
			Note::GS8 => 6644.88,
			Note::A8 => 7040.00,
			Note::AS8 => 7458.62,
			Note::B8 => 7902.13,
		}
	}
}

fn play_notes(
	mut commands: Commands,
	staffs: Query<&CommandStaff>,
	input: Res<Input<KeyCode>>,
	asset_server: Res<AssetServer>,
)
{
	if input.get_just_pressed().next().is_none() {
		return;
	}

	for staff in &staffs
	{
		if !staff.is_open {
			continue;
		}
		
		let sound = asset_server.load("flute.wav");
		
		play_note(&mut commands, &input, KeyCode::Z, Note::C4, sound.clone());
		play_note(&mut commands, &input, KeyCode::S, Note::CS4, sound.clone());
		play_note(&mut commands, &input, KeyCode::X, Note::D4, sound.clone());
		play_note(&mut commands, &input, KeyCode::D, Note::DS4, sound.clone());
		play_note(&mut commands, &input, KeyCode::C, Note::E4, sound.clone());
		play_note(&mut commands, &input, KeyCode::V, Note::F4, sound.clone());
		play_note(&mut commands, &input, KeyCode::G, Note::FS4, sound.clone());
		play_note(&mut commands, &input, KeyCode::B, Note::G4, sound.clone());
		play_note(&mut commands, &input, KeyCode::H, Note::GS4, sound.clone());
		play_note(&mut commands, &input, KeyCode::N, Note::A4, sound.clone());
		play_note(&mut commands, &input, KeyCode::J, Note::AS4, sound.clone());
		play_note(&mut commands, &input, KeyCode::M, Note::B4, sound.clone());
		
		play_note(&mut commands, &input, KeyCode::Comma, Note::C5, sound.clone());
		play_note(&mut commands, &input, KeyCode::L, Note::CS5, sound.clone());
		play_note(&mut commands, &input, KeyCode::Period, Note::D5, sound.clone());
		play_note(&mut commands, &input, KeyCode::Semicolon, Note::DS5, sound.clone());
		play_note(&mut commands, &input, KeyCode::Slash, Note::E5, sound.clone());
		
		play_note(&mut commands, &input, KeyCode::Q, Note::C5, sound.clone());
		play_note(&mut commands, &input, KeyCode::Key2, Note::CS5, sound.clone());
		play_note(&mut commands, &input, KeyCode::W, Note::D5, sound.clone());
		play_note(&mut commands, &input, KeyCode::Key3, Note::DS5, sound.clone());
		play_note(&mut commands, &input, KeyCode::E, Note::E5, sound.clone());
		play_note(&mut commands, &input, KeyCode::R, Note::F5, sound.clone());
		play_note(&mut commands, &input, KeyCode::Key5, Note::FS5, sound.clone());
		play_note(&mut commands, &input, KeyCode::T, Note::G5, sound.clone());
		play_note(&mut commands, &input, KeyCode::Key6, Note::GS5, sound.clone());
		play_note(&mut commands, &input, KeyCode::Y, Note::A5, sound.clone());
		play_note(&mut commands, &input, KeyCode::Key7, Note::AS5, sound.clone());
		play_note(&mut commands, &input, KeyCode::U, Note::B5, sound.clone());
		
		play_note(&mut commands, &input, KeyCode::I, Note::C6, sound.clone());
		play_note(&mut commands, &input, KeyCode::Key9, Note::CS6, sound.clone());
		play_note(&mut commands, &input, KeyCode::O, Note::D6, sound.clone());
		play_note(&mut commands, &input, KeyCode::Key0, Note::DS6, sound.clone());
		play_note(&mut commands, &input, KeyCode::P, Note::E6, sound.clone());
	}
}

fn play_note(
	commands: &mut Commands,
	input: &Input<KeyCode>,
	key: KeyCode,
	note: Note,
	sound: Handle<AudioSource>,
)
{
	if input.just_pressed(key)
	{
		commands.spawn(AudioBundle
		{
			source: sound,
			settings: PlaybackSettings
			{
				mode: PlaybackMode::Despawn,
				speed: note.frequency() / Note::C4.frequency(),
				..default()
			},
		});
	}
}