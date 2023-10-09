use bevy::prelude::*;
use bevy_input::common_conditions::input_just_pressed;

pub struct PlayerCommandsPlugin;

impl Plugin for PlayerCommandsPlugin
{
	fn build(&self, app: &mut App) {
		app
			.add_systems(Startup, spawn_staff)
			.add_systems(Update, toggle_staffs.run_if(input_just_pressed(KeyCode::Grave)))
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
)
{
	commands.spawn((
		NodeBundle
		{
			style: Style
			{
				width: Val::Percent(100.0),
				height: Val::Px(100.0),
				display: Display::None,
				..default()
			},
			background_color: Color::hex("#faefb4").unwrap().into(),
			..default()
		},
		CommandStaff::default(),
	));
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