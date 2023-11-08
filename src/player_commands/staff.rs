use bevy::prelude::*;
use leafwing_input_manager::prelude::ToggleActions;

use super::{note_holder::NoteNodeHolder, notes::{ClearNotesEvent, PlayNoteAction}};

#[derive(Component, Default)]
pub struct CommandStaff
{
	pub is_open: bool,
}

// This should be enough information to map all notes
pub const F5_LINE_TOP: f32 = 15.0;
pub const STAFF_HEIGHT: f32 = 60.0;
pub const CLEF_HEIGHT: f32 = 80.0;
pub const LINE_HEIGHT: f32 = 2.0;

pub const QUARTER_NOTE_TOP_OFFSET: f32 = 41.0;
pub const QUARTER_NOTE_HEIGHT: f32 = 55.0;
pub const QUARTER_NOTE_LEFT_START: f32 = 40.0;
pub const QUARTER_NOTE_LEFT_SPACING: f32 = 20.0;

// Does top + height not actually equal bottom???
pub const QUARTER_NOTE_WEIRD_SPACING_OFFSET: f32 = 18.0;

pub fn spawn_staff(
	mut commands: Commands,
	asset_server: Res<AssetServer>,
)
{
	let treble_clef = asset_server.load("treble_clef.png");

	// Background
	commands
		.spawn((
			Name::new("Staff"),
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
				.spawn((
					Name::new("Clef"),
					ImageBundle
					{
						image: treble_clef.into(),
						style: Style
						{
							position_type: PositionType::Absolute,
							height: Val::Px(CLEF_HEIGHT),
							..default()
						},
						..default()
					},
				));

			// Staff lines
			parent
				.spawn((
					Name::new("Staff lines"),
					NodeBundle
					{
						style: Style
						{
							flex_direction: FlexDirection::Column,
							flex_grow: 1.0,
							padding: UiRect::top(Val::Px(F5_LINE_TOP)),
							height: Val::Px(STAFF_HEIGHT),
							justify_content: JustifyContent::SpaceBetween,
							..default()
						},
						..default()
					},
					NoteNodeHolder::default(),
				))
				.with_children(|parent|
				{
					for i in 0..5
					{
						parent.spawn((
							Name::new(format!("Line {i}")),
							NodeBundle
							{
								style: Style
								{
									width: Val::Percent(100.0),
									height: Val::Px(LINE_HEIGHT),
									..default()
								},
								background_color: Color::BLACK.into(),
								..default()
							},
						));
					}
				});
		});
}

#[cfg(feature = "spawn_debug_notes_on_staff")]
pub fn spawn_debug_notes(
	mut commands: Commands,
	mut note_holder: Query<(&mut NoteNodeHolder, Entity)>,
)
{
	let (mut note_holder, note_holder_entity) = note_holder.single_mut();
	
	commands
		.entity(note_holder_entity)
		.with_children(|parent|
		{
			for note in [Note::C4, Note::D4, Note::E4, Note::F4, Note::G4, Note::A4, Note::B4, Note::C5, Note::D5, Note::E5, Note::F5, Note::G5, Note::A5]
			{
				parent.spawn(note_bundle(&mut note_holder, note.clone()));
			}
		});
}

#[derive(Event)]
pub struct ToggleStaffEvent;

pub fn send_toggle_staff(
	mut ev_toggle_staff: EventWriter<ToggleStaffEvent>
)
{
	ev_toggle_staff.send(ToggleStaffEvent);
}

pub fn toggle_staff(
	mut staff: Query<(&mut CommandStaff, &mut Style)>,
	mut note_input: ResMut<ToggleActions<PlayNoteAction>>,
	mut ev_clear_notes: EventWriter<ClearNotesEvent>,
)
{
	let (mut staff, mut style) = staff.single_mut();

	if staff.is_open { close_staff(&mut staff, &mut style, &mut note_input, &mut ev_clear_notes) }
	else { open_staff(&mut staff, &mut style, &mut note_input) }
}

fn open_staff(
	staff: &mut CommandStaff,
	style: &mut Style,
	note_input: &mut ToggleActions<PlayNoteAction>,
)
{
	staff.is_open = true;
	style.display = Display::Flex;
	note_input.enabled = true;
}

fn close_staff(
	staff: &mut CommandStaff,
	style: &mut Style,
	note_input: &mut ToggleActions<PlayNoteAction>,
	ev_clear_notes: &mut EventWriter<ClearNotesEvent>,
)
{
	staff.is_open = false;
	style.display = Display::None;
	note_input.enabled = false;
	ev_clear_notes.send(ClearNotesEvent);
}