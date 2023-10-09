use bevy::prelude::*;
use bevy_input::common_conditions::input_just_pressed;

pub struct PlayerCommandsPlugin;

impl Plugin for PlayerCommandsPlugin
{
	fn build(&self, app: &mut App) {
		app
			.add_systems(Update, open_staff.run_if(input_just_pressed(KeyCode::Grave)))
			;
	}
}

fn open_staff(

)
{
	println!("open")
}