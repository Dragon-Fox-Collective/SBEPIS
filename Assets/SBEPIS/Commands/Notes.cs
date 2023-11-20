using System;

namespace SBEPIS.Commands
{
	[Serializable]
	public enum NoteLetter
	{
		C, D, E, F, G, A, B,
	}
    
	[Serializable]
	public class Note
	{
		public NoteLetter NoteLetter;
		public bool Sharp;
		public int Octave;
		public float Frequency;
		
		public int Position => Octave * 7 + (int)NoteLetter;
		
		public override string ToString() => $"{NoteLetter}{(Sharp ? "#" : "")}{Octave}";
	}
	
	public static class Notes
	{
		public static readonly Note C0  = new(){ NoteLetter = NoteLetter.C, Sharp = false, Octave = 0, Frequency =   16.35f };
		public static readonly Note CS0 = new(){ NoteLetter = NoteLetter.C, Sharp = true,  Octave = 0, Frequency =   17.32f };
		public static readonly Note D0  = new(){ NoteLetter = NoteLetter.D, Sharp = false, Octave = 0, Frequency =   18.35f };
		public static readonly Note DS0 = new(){ NoteLetter = NoteLetter.D, Sharp = true,  Octave = 0, Frequency =   19.45f };
		public static readonly Note E0  = new(){ NoteLetter = NoteLetter.E, Sharp = false, Octave = 0, Frequency =   20.60f };
		public static readonly Note F0  = new(){ NoteLetter = NoteLetter.F, Sharp = false, Octave = 0, Frequency =   21.83f };
		public static readonly Note FS0 = new(){ NoteLetter = NoteLetter.F, Sharp = true,  Octave = 0, Frequency =   23.12f };
		public static readonly Note G0  = new(){ NoteLetter = NoteLetter.G, Sharp = false, Octave = 0, Frequency =   24.50f };
		public static readonly Note GS0 = new(){ NoteLetter = NoteLetter.G, Sharp = true,  Octave = 0, Frequency =   25.96f };
		public static readonly Note A0  = new(){ NoteLetter = NoteLetter.A, Sharp = false, Octave = 0, Frequency =   27.50f };
		public static readonly Note AS0 = new(){ NoteLetter = NoteLetter.A, Sharp = true,  Octave = 0, Frequency =   29.14f };
		public static readonly Note B0  = new(){ NoteLetter = NoteLetter.B, Sharp = false, Octave = 0, Frequency =   30.87f };
		public static readonly Note C1  = new(){ NoteLetter = NoteLetter.C, Sharp = false, Octave = 1, Frequency =   32.70f };
		public static readonly Note CS1 = new(){ NoteLetter = NoteLetter.C, Sharp = true,  Octave = 1, Frequency =   34.65f };
		public static readonly Note D1  = new(){ NoteLetter = NoteLetter.D, Sharp = false, Octave = 1, Frequency =   36.71f };
		public static readonly Note DS1 = new(){ NoteLetter = NoteLetter.D, Sharp = true,  Octave = 1, Frequency =   38.89f };
		public static readonly Note E1  = new(){ NoteLetter = NoteLetter.E, Sharp = false, Octave = 1, Frequency =   41.20f };
		public static readonly Note F1  = new(){ NoteLetter = NoteLetter.F, Sharp = false, Octave = 1, Frequency =   43.65f };
		public static readonly Note FS1 = new(){ NoteLetter = NoteLetter.F, Sharp = true,  Octave = 1, Frequency =   46.25f };
		public static readonly Note G1  = new(){ NoteLetter = NoteLetter.G, Sharp = false, Octave = 1, Frequency =   49.00f };
		public static readonly Note GS1 = new(){ NoteLetter = NoteLetter.G, Sharp = true,  Octave = 1, Frequency =   51.91f };
		public static readonly Note A1  = new(){ NoteLetter = NoteLetter.A, Sharp = false, Octave = 1, Frequency =   55.00f };
		public static readonly Note AS1 = new(){ NoteLetter = NoteLetter.A, Sharp = true,  Octave = 1, Frequency =   58.27f };
		public static readonly Note B1  = new(){ NoteLetter = NoteLetter.B, Sharp = false, Octave = 1, Frequency =   61.74f };
		public static readonly Note C2  = new(){ NoteLetter = NoteLetter.C, Sharp = false, Octave = 2, Frequency =   65.41f };
		public static readonly Note CS2 = new(){ NoteLetter = NoteLetter.C, Sharp = true,  Octave = 2, Frequency =   69.30f };
		public static readonly Note D2  = new(){ NoteLetter = NoteLetter.D, Sharp = false, Octave = 2, Frequency =   73.42f };
		public static readonly Note DS2 = new(){ NoteLetter = NoteLetter.D, Sharp = true,  Octave = 2, Frequency =   77.78f };
		public static readonly Note E2  = new(){ NoteLetter = NoteLetter.E, Sharp = false, Octave = 2, Frequency =   82.41f };
		public static readonly Note F2  = new(){ NoteLetter = NoteLetter.F, Sharp = false, Octave = 2, Frequency =   87.31f };
		public static readonly Note FS2 = new(){ NoteLetter = NoteLetter.F, Sharp = true,  Octave = 2, Frequency =   92.50f };
		public static readonly Note G2  = new(){ NoteLetter = NoteLetter.G, Sharp = false, Octave = 2, Frequency =   98.00f };
		public static readonly Note GS2 = new(){ NoteLetter = NoteLetter.G, Sharp = true,  Octave = 2, Frequency =  103.83f };
		public static readonly Note A2  = new(){ NoteLetter = NoteLetter.A, Sharp = false, Octave = 2, Frequency =  110.00f };
		public static readonly Note AS2 = new(){ NoteLetter = NoteLetter.A, Sharp = true,  Octave = 2, Frequency =  116.54f };
		public static readonly Note B2  = new(){ NoteLetter = NoteLetter.B, Sharp = false, Octave = 2, Frequency =  123.47f };
		public static readonly Note C3  = new(){ NoteLetter = NoteLetter.C, Sharp = false, Octave = 3, Frequency =  130.81f };
		public static readonly Note CS3 = new(){ NoteLetter = NoteLetter.C, Sharp = true,  Octave = 3, Frequency =  138.59f };
		public static readonly Note D3  = new(){ NoteLetter = NoteLetter.D, Sharp = false, Octave = 3, Frequency =  146.83f };
		public static readonly Note DS3 = new(){ NoteLetter = NoteLetter.D, Sharp = true,  Octave = 3, Frequency =  155.56f };
		public static readonly Note E3  = new(){ NoteLetter = NoteLetter.E, Sharp = false, Octave = 3, Frequency =  164.81f };
		public static readonly Note F3  = new(){ NoteLetter = NoteLetter.F, Sharp = false, Octave = 3, Frequency =  174.61f };
		public static readonly Note FS3 = new(){ NoteLetter = NoteLetter.F, Sharp = true,  Octave = 3, Frequency =  185.00f };
		public static readonly Note G3  = new(){ NoteLetter = NoteLetter.G, Sharp = false, Octave = 3, Frequency =  196.00f };
		public static readonly Note GS3 = new(){ NoteLetter = NoteLetter.G, Sharp = true,  Octave = 3, Frequency =  207.65f };
		public static readonly Note A3  = new(){ NoteLetter = NoteLetter.A, Sharp = false, Octave = 3, Frequency =  220.00f };
		public static readonly Note AS3 = new(){ NoteLetter = NoteLetter.A, Sharp = true,  Octave = 3, Frequency =  233.08f };
		public static readonly Note B3  = new(){ NoteLetter = NoteLetter.B, Sharp = false, Octave = 3, Frequency =  246.94f };
		public static readonly Note C4  = new(){ NoteLetter = NoteLetter.C, Sharp = false, Octave = 4, Frequency =  261.63f };
		public static readonly Note CS4 = new(){ NoteLetter = NoteLetter.C, Sharp = true,  Octave = 4, Frequency =  277.18f };
		public static readonly Note D4  = new(){ NoteLetter = NoteLetter.D, Sharp = false, Octave = 4, Frequency =  293.66f };
		public static readonly Note DS4 = new(){ NoteLetter = NoteLetter.D, Sharp = true,  Octave = 4, Frequency =  311.13f };
		public static readonly Note E4  = new(){ NoteLetter = NoteLetter.E, Sharp = false, Octave = 4, Frequency =  329.63f };
		public static readonly Note F4  = new(){ NoteLetter = NoteLetter.F, Sharp = false, Octave = 4, Frequency =  349.23f };
		public static readonly Note FS4 = new(){ NoteLetter = NoteLetter.F, Sharp = true,  Octave = 4, Frequency =  369.99f };
		public static readonly Note G4  = new(){ NoteLetter = NoteLetter.G, Sharp = false, Octave = 4, Frequency =  392.00f };
		public static readonly Note GS4 = new(){ NoteLetter = NoteLetter.G, Sharp = true,  Octave = 4, Frequency =  415.30f };
		public static readonly Note A4  = new(){ NoteLetter = NoteLetter.A, Sharp = false, Octave = 4, Frequency =  440.00f };
		public static readonly Note AS4 = new(){ NoteLetter = NoteLetter.A, Sharp = true,  Octave = 4, Frequency =  466.16f };
		public static readonly Note B4  = new(){ NoteLetter = NoteLetter.B, Sharp = false, Octave = 4, Frequency =  493.88f };
		public static readonly Note C5  = new(){ NoteLetter = NoteLetter.C, Sharp = false, Octave = 5, Frequency =  523.25f };
		public static readonly Note CS5 = new(){ NoteLetter = NoteLetter.C, Sharp = true,  Octave = 5, Frequency =  554.37f };
		public static readonly Note D5  = new(){ NoteLetter = NoteLetter.D, Sharp = false, Octave = 5, Frequency =  587.33f };
		public static readonly Note DS5 = new(){ NoteLetter = NoteLetter.D, Sharp = true,  Octave = 5, Frequency =  622.25f };
		public static readonly Note E5  = new(){ NoteLetter = NoteLetter.E, Sharp = false, Octave = 5, Frequency =  659.25f };
		public static readonly Note F5  = new(){ NoteLetter = NoteLetter.F, Sharp = false, Octave = 5, Frequency =  698.46f };
		public static readonly Note FS5 = new(){ NoteLetter = NoteLetter.F, Sharp = true,  Octave = 5, Frequency =  739.99f };
		public static readonly Note G5  = new(){ NoteLetter = NoteLetter.G, Sharp = false, Octave = 5, Frequency =  783.99f };
		public static readonly Note GS5 = new(){ NoteLetter = NoteLetter.G, Sharp = true,  Octave = 5, Frequency =  830.61f };
		public static readonly Note A5  = new(){ NoteLetter = NoteLetter.A, Sharp = false, Octave = 5, Frequency =  880.00f };
		public static readonly Note AS5 = new(){ NoteLetter = NoteLetter.A, Sharp = true,  Octave = 5, Frequency =  932.33f };
		public static readonly Note B5  = new(){ NoteLetter = NoteLetter.B, Sharp = false, Octave = 5, Frequency =  987.77f };
		public static readonly Note C6  = new(){ NoteLetter = NoteLetter.C, Sharp = false, Octave = 6, Frequency = 1046.50f };
		public static readonly Note CS6 = new(){ NoteLetter = NoteLetter.C, Sharp = true,  Octave = 6, Frequency = 1108.73f };
		public static readonly Note D6  = new(){ NoteLetter = NoteLetter.D, Sharp = false, Octave = 6, Frequency = 1174.66f };
		public static readonly Note DS6 = new(){ NoteLetter = NoteLetter.D, Sharp = true,  Octave = 6, Frequency = 1244.51f };
		public static readonly Note E6  = new(){ NoteLetter = NoteLetter.E, Sharp = false, Octave = 6, Frequency = 1318.51f };
		public static readonly Note F6  = new(){ NoteLetter = NoteLetter.F, Sharp = false, Octave = 6, Frequency = 1396.91f };
		public static readonly Note FS6 = new(){ NoteLetter = NoteLetter.F, Sharp = true,  Octave = 6, Frequency = 1479.98f };
		public static readonly Note G6  = new(){ NoteLetter = NoteLetter.G, Sharp = false, Octave = 6, Frequency = 1567.98f };
		public static readonly Note GS6 = new(){ NoteLetter = NoteLetter.G, Sharp = true,  Octave = 6, Frequency = 1661.22f };
		public static readonly Note A6  = new(){ NoteLetter = NoteLetter.A, Sharp = false, Octave = 6, Frequency = 1760.00f };
		public static readonly Note AS6 = new(){ NoteLetter = NoteLetter.A, Sharp = true,  Octave = 6, Frequency = 1864.66f };
		public static readonly Note B6  = new(){ NoteLetter = NoteLetter.B, Sharp = false, Octave = 6, Frequency = 1975.53f };
		public static readonly Note C7  = new(){ NoteLetter = NoteLetter.C, Sharp = false, Octave = 7, Frequency = 2093.00f };
		public static readonly Note CS7 = new(){ NoteLetter = NoteLetter.C, Sharp = true,  Octave = 7, Frequency = 2217.46f };
		public static readonly Note D7  = new(){ NoteLetter = NoteLetter.D, Sharp = false, Octave = 7, Frequency = 2349.83f };
		public static readonly Note DS7 = new(){ NoteLetter = NoteLetter.D, Sharp = true,  Octave = 7, Frequency = 2489.02f };
		public static readonly Note E7  = new(){ NoteLetter = NoteLetter.E, Sharp = false, Octave = 7, Frequency = 2637.02f };
		public static readonly Note F7  = new(){ NoteLetter = NoteLetter.F, Sharp = false, Octave = 7, Frequency = 2793.83f };
		public static readonly Note FS7 = new(){ NoteLetter = NoteLetter.F, Sharp = true,  Octave = 7, Frequency = 2959.96f };
		public static readonly Note G7  = new(){ NoteLetter = NoteLetter.G, Sharp = false, Octave = 7, Frequency = 3135.96f };
		public static readonly Note GS7 = new(){ NoteLetter = NoteLetter.G, Sharp = true,  Octave = 7, Frequency = 3322.44f };
		public static readonly Note A7  = new(){ NoteLetter = NoteLetter.A, Sharp = false, Octave = 7, Frequency = 3520.00f };
		public static readonly Note AS7 = new(){ NoteLetter = NoteLetter.A, Sharp = true,  Octave = 7, Frequency = 3729.31f };
		public static readonly Note B7  = new(){ NoteLetter = NoteLetter.B, Sharp = false, Octave = 7, Frequency = 3951.07f };
		public static readonly Note C8  = new(){ NoteLetter = NoteLetter.C, Sharp = false, Octave = 8, Frequency = 4186.01f };
		public static readonly Note CS8 = new(){ NoteLetter = NoteLetter.C, Sharp = true,  Octave = 8, Frequency = 4434.92f };
		public static readonly Note D8  = new(){ NoteLetter = NoteLetter.D, Sharp = false, Octave = 8, Frequency = 4698.63f };
		public static readonly Note DS8 = new(){ NoteLetter = NoteLetter.D, Sharp = true,  Octave = 8, Frequency = 4978.03f };
		public static readonly Note E8  = new(){ NoteLetter = NoteLetter.E, Sharp = false, Octave = 8, Frequency = 5274.04f };
		public static readonly Note F8  = new(){ NoteLetter = NoteLetter.F, Sharp = false, Octave = 8, Frequency = 5587.65f };
		public static readonly Note FS8 = new(){ NoteLetter = NoteLetter.F, Sharp = true,  Octave = 8, Frequency = 5919.91f };
		public static readonly Note G8  = new(){ NoteLetter = NoteLetter.G, Sharp = false, Octave = 8, Frequency = 6271.93f };
		public static readonly Note GS8 = new(){ NoteLetter = NoteLetter.G, Sharp = true,  Octave = 8, Frequency = 6644.88f };
		public static readonly Note A8  = new(){ NoteLetter = NoteLetter.A, Sharp = false, Octave = 8, Frequency = 7040.00f };
		public static readonly Note AS8 = new(){ NoteLetter = NoteLetter.A, Sharp = true,  Octave = 8, Frequency = 7458.62f };
		public static readonly Note B8  = new(){ NoteLetter = NoteLetter.B, Sharp = false, Octave = 8, Frequency = 7902.13f };
	}
}
