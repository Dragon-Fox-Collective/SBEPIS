mod notes;
mod commands;

use bevy::prelude::*;
use bevy_input::common_conditions::input_just_pressed;
use bevy_audio::PlaybackMode;

use super::util::MapRange;

use self::notes::*;
use self::commands::*;

pub struct PlayerCommandsPlugin;

impl Plugin for PlayerCommandsPlugin
{
	fn build(&self, app: &mut App) {
		app
			.add_event::<NotePlayedEvent>()
			.add_event::<PingCommandEvent>()
			.insert_resource(NotePattern::<PingCommandEvent>::new(vec![Note::C4, Note::D4, Note::E4]))
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
					spawn_note_icon,
					(
						check_note_patterns::<PingCommandEvent>,
						ping.after(check_note_patterns::<PingCommandEvent>),
					).after(spawn_note_icon),
				).after(play_notes),
			))
			;
	}
}

#[derive(Component, Default)]
pub struct CommandStaff
{
	pub is_open: bool,
}

#[derive(Component, Default)]
pub struct NoteHolder
{
	notes: Vec<Note>,
	note_entities: Vec<Entity>,
	
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
	pub fn add_note(&mut self, note: Note, note_entity: Entity)
	{
		self.notes.push(note);
		self.note_entities.push(note_entity);
	}

	pub fn clear_notes(&mut self, commands: &mut Commands)
	{
		self.notes.clear();
		for note_entity in self.note_entities.iter() {
			commands.entity(*note_entity).despawn_recursive();
		}
		self.note_entities.clear();
	}

	pub fn len(&self) -> usize
	{
		self.notes.len()
	}

	pub fn next_note_left(&mut self) -> f32
	{
		self.quarter_note_left_start + (self.len() as f32 + 1.0) * self.quarter_note_left_spacing
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
						quarter_note,
						f5_line_top,
						staff_height,
						quarter_note_top_offset,
						quarter_note_height,
						quarter_note_left_start,
						quarter_note_left_spacing,
						quarter_note_weird_spacing_offset,
						..default()
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

#[cfg(feature = "spawn_debug_notes_on_staff")]
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

fn toggle_staffs(
	mut commands: Commands,
	mut staffs: Query<(&mut CommandStaff, &mut Style)>,
	mut note_holders: Query<&mut NoteHolder>,
)
{
	let (mut staff, mut style) = staffs.single_mut();

	if staff.is_open { close_staff(&mut commands, &mut staff, &mut style, &mut note_holders.single_mut()) }
	else { open_staff(&mut staff, &mut style) }
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
	commands: &mut Commands,
	staff: &mut CommandStaff,
	style: &mut Style,
	note_holder: &mut NoteHolder,
)
{
	staff.is_open = false;
	style.display = Display::None;
	note_holder.clear_notes(commands);
}

#[derive(Event)]
pub struct NotePlayedEvent(Note);

fn play_notes(
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

pub fn spawn_note_icon(
	mut commands: Commands,
	mut ev_note_played: EventReader<NotePlayedEvent>,
	mut note_holders: Query<(&mut NoteHolder, Entity)>,
)
{
	if ev_note_played.is_empty() {
		return;
	}

	let (mut note_holder, note_holder_entity) = note_holders.single_mut();

	for ev in ev_note_played.iter()
	{
		let note = ev.0;

		println!("{} {} {}", note, note.position(), note_holder.note_top(&note));

		let note_entity = commands.spawn(ImageBundle
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
		}).id();
		
		commands.entity(note_holder_entity).add_child(note_entity);

		note_holder.add_note(note, note_entity);
	}
}