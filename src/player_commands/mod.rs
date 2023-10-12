mod notes;

use bevy::prelude::*;
use bevy_input::common_conditions::input_just_pressed;
use bevy_audio::PlaybackMode;

use super::util::MapRange;

use self::notes::*;

pub struct PlayerCommandsPlugin;

impl Plugin for PlayerCommandsPlugin
{
	fn build(&self, app: &mut App) {
		app
			.add_systems(Startup, (
				spawn_staff,
				apply_deferred,
				spawn_debug_notes,
			).chain())
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

#[derive(Component, Reflect, Default)]
pub struct NoteHolder
{
	pub num_notes: u32,
	
	pub quarter_note: Handle<Image>,
	
	pub f5_line_top: f32,
	pub staff_height: f32,

	pub quarter_note_top_offset: f32,
	pub quarter_note_height: f32,
	pub quarter_note_left_start: f32,
	pub quarter_note_left_spacing: f32,
	
	pub quarter_note_weird_spacing_offset: f32,
	
}

impl NoteHolder
{
	pub fn next_note_left(&mut self) -> f32
	{
		let left = self.quarter_note_left_start + self.num_notes as f32 * self.quarter_note_left_spacing;
		self.num_notes += 1;
		left
	}

	pub fn note_top(&self, note: &Note) -> f32
	{
		(note.position() as f32).map(Note::E4.position() as f32, Note::F5.position() as f32, self.f5_line_top + self.staff_height - self.quarter_note_weird_spacing_offset, self.f5_line_top) - self.quarter_note_top_offset
	}
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
	let line_height = 2.0;

	let quarter_note_top_offset = 41.0;
	let quarter_note_height = 55.0;
	let quarter_note_left_start = 40.0;
	let quarter_note_left_spacing = 20.0;
	
	// Does top + height not actually equal bottom???
	let quarter_note_weird_spacing_offset = 18.0;

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

			// Staff lines
			parent
				.spawn((
					NodeBundle
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
					},
					NoteHolder
					{
						num_notes: 0,
						quarter_note,
						f5_line_top,
						staff_height,
						quarter_note_top_offset,
						quarter_note_height,
						quarter_note_left_start,
						quarter_note_left_spacing,
						quarter_note_weird_spacing_offset,
					},
				))
				.with_children(|parent|
				{
					for _ in 0..5
					{
						parent.spawn(NodeBundle
						{
							style: Style
							{
								width: Val::Percent(100.0),
								height: Val::Px(line_height),
								..default()
							},
							background_color: Color::BLACK.into(),
							..default()
						});
					}
				});
		});
}

fn spawn_debug_notes(
	mut commands: Commands,
	mut note_holders: Query<(&mut NoteHolder, Entity)>,
)
{
	let (mut note_holder, note_holder_entity) = note_holders.single_mut();
	
	commands
		.entity(note_holder_entity)
		.with_children(|parent|
		{
			for note in vec![Note::C4, Note::D4, Note::E4, Note::F4, Note::G4, Note::A4, Note::B4, Note::C5, Note::D5, Note::E5, Note::F5, Note::G5, Note::A5].iter()
			{
				parent.spawn(note_bundle(&mut note_holder, note.clone()));
			}
		});
}

fn note_bundle(
	note_holder: &mut NoteHolder,
	note: Note,
) -> impl Bundle
{
	println!("{} {} {}", note, note.position(), note_holder.note_top(&note));

	ImageBundle
	{
		image: note_holder.quarter_note.clone().into(),
		style: Style
		{
			position_type: PositionType::Absolute,
			left: Val::Px(note_holder.next_note_left()),
			top: Val::Px(note_holder.note_top(&note)),
			height: Val::Px(note_holder.quarter_note_height),
			..default()
		},
		..default()
	}
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

fn play_notes(
	mut commands: Commands,
	staffs: Query<&CommandStaff>,
	mut note_holders: Query<(&mut NoteHolder, Entity)>,
	input: Res<Input<KeyCode>>,
	asset_server: Res<AssetServer>,
)
{
	if input.get_just_pressed().next().is_none() {
		return;
	}

	let staff = staffs.single();
	if !staff.is_open {
		return;
	}
	
	let (mut note_holder, note_holder_entity) = note_holders.single_mut();
	
	let sound = asset_server.load("flute.wav");
	
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::Z, Note::C4, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::S, Note::CS4, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::X, Note::D4, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::D, Note::DS4, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::C, Note::E4, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::V, Note::F4, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::G, Note::FS4, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::B, Note::G4, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::H, Note::GS4, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::N, Note::A4, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::J, Note::AS4, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::M, Note::B4, sound.clone());
	
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::Comma, Note::C5, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::L, Note::CS5, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::Period, Note::D5, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::Semicolon, Note::DS5, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::Slash, Note::E5, sound.clone());
	
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::Q, Note::C5, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::Key2, Note::CS5, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::W, Note::D5, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::Key3, Note::DS5, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::E, Note::E5, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::R, Note::F5, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::Key5, Note::FS5, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::T, Note::G5, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::Key6, Note::GS5, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::Y, Note::A5, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::Key7, Note::AS5, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::U, Note::B5, sound.clone());
	
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::I, Note::C6, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::Key9, Note::CS6, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::O, Note::D6, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::Key0, Note::DS6, sound.clone());
	play_note(&mut commands, &mut note_holder, &note_holder_entity, &input, KeyCode::P, Note::E6, sound.clone());
}

fn play_note(
	commands: &mut Commands,
	note_holder: &mut NoteHolder,
	note_holder_entity: &Entity,
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
				speed: note.frequency / Note::C4.frequency,
				..default()
			},
		});

		commands
			.entity(*note_holder_entity)
			.with_children(|parent|
			{
				parent.spawn(note_bundle(note_holder, note));
			});
	}
}