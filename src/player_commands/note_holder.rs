use bevy::prelude::*;

use crate::util::MapRange;

use super::{notes::{Note, NotePlayedEvent}, staff::*};

#[derive(Component, Default)]
pub struct NoteNodeHolder
{
	note_entities: Vec<Entity>,
}

impl NoteNodeHolder
{
	pub fn next_note_left(&mut self) -> f32
	{
		QUARTER_NOTE_LEFT_START + (self.note_entities.len() as f32 + 1.0) * QUARTER_NOTE_LEFT_SPACING
	}

	pub fn note_top(&self, note: &Note) -> f32
	{
		(note.position() as f32).map(Note::E4.position() as f32, Note::F5.position() as f32, F5_LINE_TOP + STAFF_HEIGHT - QUARTER_NOTE_WEIRD_SPACING_OFFSET, F5_LINE_TOP) - QUARTER_NOTE_TOP_OFFSET
	}
}

pub fn add_note_to_holder(
	mut commands: Commands,
	mut ev_note_played: EventReader<NotePlayedEvent>,
	mut note_holder: Query<(&mut NoteNodeHolder, Entity)>,
	asset_server: Res<AssetServer>,
)
{
	let (mut note_holder, note_holder_entity) = note_holder.single_mut();

	for ev in &mut ev_note_played
	{
		let note = ev.0;

		println!("{} {} {}", note, note.position(), note_holder.note_top(&note));

		let note_entity = commands.spawn(ImageBundle
		{
			image: asset_server.load("quarter_note.png").into(),
			style: Style
			{
				position_type: PositionType::Absolute,
				left: Val::Px(note_holder.next_note_left()),
				top: Val::Px(note_holder.note_top(&note)),
				height: Val::Px(QUARTER_NOTE_HEIGHT),
				..default()
			},
			..default()
		}).id();
		
		commands.entity(note_holder_entity).add_child(note_entity);

		note_holder.note_entities.push(note_entity);
	}
}

pub fn clear_holder_notes(
	mut commands: Commands,
	mut note_holder: Query<&mut NoteNodeHolder>,
)
{
	let mut note_holder = note_holder.single_mut();
	for note_entity in &mut note_holder.note_entities {
		commands.entity(*note_entity).despawn_recursive();
	}
	note_holder.note_entities.clear();
}