using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Commands
{
	public class NotePlayer : ValidatedMonoBehaviour
	{
		[SerializeField, Anywhere]
		private AudioSource notePrefab;
		
		[SerializeField, Anywhere]
		private CommandStaff staff;
		
		[SerializeField]
		private float rootFrequency = 261.63f;
		
		public void PlayC0() => PlayNote(Notes.C0);
		public void PlayCS0() => PlayNote(Notes.CS0);
		public void PlayD0() => PlayNote(Notes.D0);
		public void PlayDS0() => PlayNote(Notes.DS0);
		public void PlayE0() => PlayNote(Notes.E0);
		public void PlayF0() => PlayNote(Notes.F0);
		public void PlayFS0() => PlayNote(Notes.FS0);
		public void PlayG0() => PlayNote(Notes.G0);
		public void PlayGS0() => PlayNote(Notes.GS0);
		public void PlayA0() => PlayNote(Notes.A0);
		public void PlayAS0() => PlayNote(Notes.AS0);
		public void PlayB0() => PlayNote(Notes.B0);
		public void PlayC1() => PlayNote(Notes.C1);
		public void PlayCS1() => PlayNote(Notes.CS1);
		public void PlayD1() => PlayNote(Notes.D1);
		public void PlayDS1() => PlayNote(Notes.DS1);
		public void PlayE1() => PlayNote(Notes.E1);
		public void PlayF1() => PlayNote(Notes.F1);
		public void PlayFS1() => PlayNote(Notes.FS1);
		public void PlayG1() => PlayNote(Notes.G1);
		public void PlayGS1() => PlayNote(Notes.GS1);
		public void PlayA1() => PlayNote(Notes.A1);
		public void PlayAS1() => PlayNote(Notes.AS1);
		public void PlayB1() => PlayNote(Notes.B1);
		public void PlayC2() => PlayNote(Notes.C2);
		public void PlayCS2() => PlayNote(Notes.CS2);
		public void PlayD2() => PlayNote(Notes.D2);
		public void PlayDS2() => PlayNote(Notes.DS2);
		public void PlayE2() => PlayNote(Notes.E2);
		public void PlayF2() => PlayNote(Notes.F2);
		public void PlayFS2() => PlayNote(Notes.FS2);
		public void PlayG2() => PlayNote(Notes.G2);
		public void PlayGS2() => PlayNote(Notes.GS2);
		public void PlayA2() => PlayNote(Notes.A2);
		public void PlayAS2() => PlayNote(Notes.AS2);
		public void PlayB2() => PlayNote(Notes.B2);
		public void PlayC3() => PlayNote(Notes.C3);
		public void PlayCS3() => PlayNote(Notes.CS3);
		public void PlayD3() => PlayNote(Notes.D3);
		public void PlayDS3() => PlayNote(Notes.DS3);
		public void PlayE3() => PlayNote(Notes.E3);
		public void PlayF3() => PlayNote(Notes.F3);
		public void PlayFS3() => PlayNote(Notes.FS3);
		public void PlayG3() => PlayNote(Notes.G3);
		public void PlayGS3() => PlayNote(Notes.GS3);
		public void PlayA3() => PlayNote(Notes.A3);
		public void PlayAS3() => PlayNote(Notes.AS3);
		public void PlayB3() => PlayNote(Notes.B3);
		public void PlayC4() => PlayNote(Notes.C4);
		public void PlayCS4() => PlayNote(Notes.CS4);
		public void PlayD4() => PlayNote(Notes.D4);
		public void PlayDS4() => PlayNote(Notes.DS4);
		public void PlayE4() => PlayNote(Notes.E4);
		public void PlayF4() => PlayNote(Notes.F4);
		public void PlayFS4() => PlayNote(Notes.FS4);
		public void PlayG4() => PlayNote(Notes.G4);
		public void PlayGS4() => PlayNote(Notes.GS4);
		public void PlayA4() => PlayNote(Notes.A4);
		public void PlayAS4() => PlayNote(Notes.AS4);
		public void PlayB4() => PlayNote(Notes.B4);
		public void PlayC5() => PlayNote(Notes.C5);
		public void PlayCS5() => PlayNote(Notes.CS5);
		public void PlayD5() => PlayNote(Notes.D5);
		public void PlayDS5() => PlayNote(Notes.DS5);
		public void PlayE5() => PlayNote(Notes.E5);
		public void PlayF5() => PlayNote(Notes.F5);
		public void PlayFS5() => PlayNote(Notes.FS5);
		public void PlayG5() => PlayNote(Notes.G5);
		public void PlayGS5() => PlayNote(Notes.GS5);
		public void PlayA5() => PlayNote(Notes.A5);
		public void PlayAS5() => PlayNote(Notes.AS5);
		public void PlayB5() => PlayNote(Notes.B5);
		public void PlayC6() => PlayNote(Notes.C6);
		public void PlayCS6() => PlayNote(Notes.CS6);
		public void PlayD6() => PlayNote(Notes.D6);
		public void PlayDS6() => PlayNote(Notes.DS6);
		public void PlayE6() => PlayNote(Notes.E6);
		public void PlayF6() => PlayNote(Notes.F6);
		public void PlayFS6() => PlayNote(Notes.FS6);
		public void PlayG6() => PlayNote(Notes.G6);
		public void PlayGS6() => PlayNote(Notes.GS6);
		public void PlayA6() => PlayNote(Notes.A6);
		public void PlayAS6() => PlayNote(Notes.AS6);
		public void PlayB6() => PlayNote(Notes.B6);
		public void PlayC7() => PlayNote(Notes.C7);
		public void PlayCS7() => PlayNote(Notes.CS7);
		public void PlayD7() => PlayNote(Notes.D7);
		public void PlayDS7() => PlayNote(Notes.DS7);
		public void PlayE7() => PlayNote(Notes.E7);
		public void PlayF7() => PlayNote(Notes.F7);
		public void PlayFS7() => PlayNote(Notes.FS7);
		public void PlayG7() => PlayNote(Notes.G7);
		public void PlayGS7() => PlayNote(Notes.GS7);
		public void PlayA7() => PlayNote(Notes.A7);
		public void PlayAS7() => PlayNote(Notes.AS7);
		public void PlayB7() => PlayNote(Notes.B7);
		public void PlayC8() => PlayNote(Notes.C8);
		public void PlayCS8() => PlayNote(Notes.CS8);
		public void PlayD8() => PlayNote(Notes.D8);
		public void PlayDS8() => PlayNote(Notes.DS8);
		public void PlayE8() => PlayNote(Notes.E8);
		public void PlayF8() => PlayNote(Notes.F8);
		public void PlayFS8() => PlayNote(Notes.FS8);
		public void PlayG8() => PlayNote(Notes.G8);
		public void PlayGS8() => PlayNote(Notes.GS8);
		public void PlayA8() => PlayNote(Notes.A8);
		public void PlayAS8() => PlayNote(Notes.AS8);
		public void PlayB8() => PlayNote(Notes.B8);
		
		public void PlayNote(Note note)
		{
			AudioSource noteSource = Instantiate(notePrefab);
			noteSource.pitch = note.Frequency / rootFrequency;
			noteSource.Play();
			Destroy(noteSource.gameObject, noteSource.clip.length * noteSource.pitch);
			
			staff.AddNote(note);
		}
	}
}
