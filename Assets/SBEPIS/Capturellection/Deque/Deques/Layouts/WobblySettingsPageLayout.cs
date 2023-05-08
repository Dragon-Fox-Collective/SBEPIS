using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class WobblySettingsPageLayout : DequeSettingsPageLayout<WobblyLayout>
	{
		public SwitchCardAttacher offsetFromEndSwitch;
		
		public SliderCardAttacher offsetSlider;
		public float offsetMin = 0;
		public float offsetMax = 1;
		
		public SliderCardAttacher wobbleAmplitudeSlider;
		public float wobbleAmplitudeMin = 0;
		public float wobbleAmplitudeMax = 1;
		
		public SliderCardAttacher wobbleTimeFactorSlider;
		public float wobbleTimeFactorMin = 0;
		public float wobbleTimeFactorMax = 3;
		
		public SliderCardAttacher wobbleSpaceFactorSlider;
		public float wobbleSpaceFactorMin = 0;
		public float wobbleSpaceFactorMax = 10;
		
		public void ResetOffsetFromEndSwitch() => offsetFromEndSwitch.SwitchValue = Object.offsetFromEnd;
		public void ChangeOffsetFromEnd(bool offsetFromEnd) => Object.offsetFromEnd = offsetFromEnd;
		
		public void ResetOffsetSlider() => offsetSlider.SliderValue = Object.offset.Map(offsetMin, offsetMax, 0, 1);
		public void ChangeOffset(float percent) => Object.offset = percent.Map(0, 1, offsetMin, offsetMax);
		
		public void ResetWobbleAmplitudeSlider() => wobbleAmplitudeSlider.SliderValue = Object.wobbleAmplitude.Map(wobbleAmplitudeMin, wobbleAmplitudeMax, 0, 1);
		public void ChangeWobbleAmplitude(float percent) => Object.wobbleAmplitude = percent.Map(0, 1, wobbleAmplitudeMin, wobbleAmplitudeMax);
		
		public void ResetWobbleTimeFactorSlider() => wobbleTimeFactorSlider.SliderValue = Object.wobbleTimeFactor.Map(wobbleTimeFactorMin, wobbleTimeFactorMax, 0, 1);
		public void ChangeWobbleTimeFactor(float percent) => Object.wobbleTimeFactor = percent.Map(0, 1, wobbleTimeFactorMin, wobbleTimeFactorMax);
		
		public void ResetWobbleSpaceFactorSlider() => wobbleSpaceFactorSlider.SliderValue = Object.wobbleSpaceFactor.Map(wobbleSpaceFactorMin, wobbleSpaceFactorMax, 0, 1);
		public void ChangeWobbleSpaceFactor(float percent) => Object.wobbleSpaceFactor = percent.Map(0, 1, wobbleSpaceFactorMin, wobbleSpaceFactorMax);
	}
}