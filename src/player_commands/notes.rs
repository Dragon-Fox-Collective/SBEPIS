#[derive(Debug)]
pub struct Note
{
	/// In Hz
	pub frequency: f32,

	// Relative to C4 on a major C scale
	pub position: i32,
}

#[allow(dead_code)]
impl Note
{
	pub const C0: Note = Note { frequency: 16.35, position: 0, };
	pub const CS0: Note = Note { frequency: 17.32, position: 0, };
	pub const D0: Note = Note { frequency: 18.35, position: 0, };
	pub const DS0: Note = Note { frequency: 19.45, position: 0, };
	pub const E0: Note = Note { frequency: 20.60, position: 0, };
	pub const F0: Note = Note { frequency: 21.83, position: 0, };
	pub const FS0: Note = Note { frequency: 23.12, position: 0, };
	pub const G0: Note = Note { frequency: 24.50, position: 0, };
	pub const GS0: Note = Note { frequency: 25.96, position: 0, };
	pub const A0: Note = Note { frequency: 27.50, position: 0, };
	pub const AS0: Note = Note { frequency: 29.14, position: 0, };
	pub const B0: Note = Note { frequency: 30.87, position: 0, };
	pub const C1: Note = Note { frequency: 32.70, position: 0, };
	pub const CS1: Note = Note { frequency: 34.65, position: 0, };
	pub const D1: Note = Note { frequency: 36.71, position: 0, };
	pub const DS1: Note = Note { frequency: 38.89, position: 0, };
	pub const E1: Note = Note { frequency: 41.20, position: 0, };
	pub const F1: Note = Note { frequency: 43.65, position: 0, };
	pub const FS1: Note = Note { frequency: 46.25, position: 0, };
	pub const G1: Note = Note { frequency: 49.00, position: 0, };
	pub const GS1: Note = Note { frequency: 51.91, position: 0, };
	pub const A1: Note = Note { frequency: 55.00, position: 0, };
	pub const AS1: Note = Note { frequency: 58.27, position: 0, };
	pub const B1: Note = Note { frequency: 61.74, position: 0, };
	pub const C2: Note = Note { frequency: 65.41, position: 0, };
	pub const CS2: Note = Note { frequency: 69.30, position: 0, };
	pub const D2: Note = Note { frequency: 73.42, position: 0, };
	pub const DS2: Note = Note { frequency: 77.78, position: 0, };
	pub const E2: Note = Note { frequency: 82.41, position: 0, };
	pub const F2: Note = Note { frequency: 87.31, position: 0, };
	pub const FS2: Note = Note { frequency: 92.50, position: 0, };
	pub const G2: Note = Note { frequency: 98.00, position: 0, };
	pub const GS2: Note = Note { frequency: 103.83, position: 0, };
	pub const A2: Note = Note { frequency: 110.00, position: 0, };
	pub const AS2: Note = Note { frequency: 116.54, position: 0, };
	pub const B2: Note = Note { frequency: 123.47, position: 0, };
	pub const C3: Note = Note { frequency: 130.81, position: 0, };
	pub const CS3: Note = Note { frequency: 138.59, position: 0, };
	pub const D3: Note = Note { frequency: 146.83, position: 0, };
	pub const DS3: Note = Note { frequency: 155.56, position: 0, };
	pub const E3: Note = Note { frequency: 164.81, position: 0, };
	pub const F3: Note = Note { frequency: 174.61, position: 0, };
	pub const FS3: Note = Note { frequency: 185.00, position: 0, };
	pub const G3: Note = Note { frequency: 196.00, position: 0, };
	pub const GS3: Note = Note { frequency: 207.65, position: 0, };
	pub const A3: Note = Note { frequency: 220.00, position: 0, };
	pub const AS3: Note = Note { frequency: 233.08, position: 0, };
	pub const B3: Note = Note { frequency: 246.94, position: 0, };
	pub const C4: Note = Note { frequency: 261.63, position: 0, };
	pub const CS4: Note = Note { frequency: 277.18, position: 0, };
	pub const D4: Note = Note { frequency: 293.66, position: 1, };
	pub const DS4: Note = Note { frequency: 311.13, position: 1, };
	pub const E4: Note = Note { frequency: 329.63, position: 2, };
	pub const F4: Note = Note { frequency: 349.23, position: 3, };
	pub const FS4: Note = Note { frequency: 369.99, position: 3, };
	pub const G4: Note = Note { frequency: 392.00, position: 4, };
	pub const GS4: Note = Note { frequency: 415.30, position: 4, };
	pub const A4: Note = Note { frequency: 440.00, position: 5, };
	pub const AS4: Note = Note { frequency: 466.16, position: 5, };
	pub const B4: Note = Note { frequency: 493.88, position: 6, };
	pub const C5: Note = Note { frequency: 523.25, position: 7, };
	pub const CS5: Note = Note { frequency: 554.37, position: 7, };
	pub const D5: Note = Note { frequency: 587.33, position: 8, };
	pub const DS5: Note = Note { frequency: 622.25, position: 8, };
	pub const E5: Note = Note { frequency: 659.25, position: 9, };
	pub const F5: Note = Note { frequency: 698.46, position: 10, };
	pub const FS5: Note = Note { frequency: 739.99, position: 10, };
	pub const G5: Note = Note { frequency: 783.99, position: 11, };
	pub const GS5: Note = Note { frequency: 830.61, position: 11, };
	pub const A5: Note = Note { frequency: 880.00, position: 12, };
	pub const AS5: Note = Note { frequency: 932.33, position: 12, };
	pub const B5: Note = Note { frequency: 987.77, position: 13, };
	pub const C6: Note = Note { frequency: 1046.50, position: 14, };
	pub const CS6: Note = Note { frequency: 1108.73, position: 0, };
	pub const D6: Note = Note { frequency: 1174.66, position: 0, };
	pub const DS6: Note = Note { frequency: 1244.51, position: 0, };
	pub const E6: Note = Note { frequency: 1318.51, position: 0, };
	pub const F6: Note = Note { frequency: 1396.91, position: 0, };
	pub const FS6: Note = Note { frequency: 1479.98, position: 0, };
	pub const G6: Note = Note { frequency: 1567.98, position: 0, };
	pub const GS6: Note = Note { frequency: 1661.22, position: 0, };
	pub const A6: Note = Note { frequency: 1760.00, position: 0, };
	pub const AS6: Note = Note { frequency: 1864.66, position: 0, };
	pub const B6: Note = Note { frequency: 1975.53, position: 0, };
	pub const C7: Note = Note { frequency: 2093.00, position: 0, };
	pub const CS7: Note = Note { frequency: 2217.46, position: 0, };
	pub const D7: Note = Note { frequency: 2349.83, position: 0, };
	pub const DS7: Note = Note { frequency: 2489.02, position: 0, };
	pub const E7: Note = Note { frequency: 2637.02, position: 0, };
	pub const F7: Note = Note { frequency: 2793.83, position: 0, };
	pub const FS7: Note = Note { frequency: 2959.96, position: 0, };
	pub const G7: Note = Note { frequency: 3135.96, position: 0, };
	pub const GS7: Note = Note { frequency: 3322.44, position: 0, };
	pub const A7: Note = Note { frequency: 3520.00, position: 0, };
	pub const AS7: Note = Note { frequency: 3729.31, position: 0, };
	pub const B7: Note = Note { frequency: 3951.07, position: 0, };
	pub const C8: Note = Note { frequency: 4186.01, position: 0, };
	pub const CS8: Note = Note { frequency: 4434.92, position: 0, };
	pub const D8: Note = Note { frequency: 4698.63, position: 0, };
	pub const DS8: Note = Note { frequency: 4978.03, position: 0, };
	pub const E8: Note = Note { frequency: 5274.04, position: 0, };
	pub const F8: Note = Note { frequency: 5587.65, position: 0, };
	pub const FS8: Note = Note { frequency: 5919.91, position: 0, };
	pub const G8: Note = Note { frequency: 6271.93, position: 0, };
	pub const GS8: Note = Note { frequency: 6644.88, position: 0, };
	pub const A8: Note = Note { frequency: 7040.00, position: 0, };
	pub const AS8: Note = Note { frequency: 7458.62, position: 0, };
	pub const B8: Note = Note { frequency: 7902.13, position: 0, };
}