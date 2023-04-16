using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class ArraySettingsPageLayout : DequeSettingsPageLayout<ArrayDeque>
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
		
		public void ResetOffsetFromEndSwitch() => offsetFromEndSwitch.SwitchValue = ruleset.offsetFromEnd;
		public void ChangeOffsetFromEnd(bool offsetFromEnd) => ruleset.offsetFromEnd = offsetFromEnd;
		
		public void ResetOffsetSlider() => offsetSlider.SliderValue = ruleset.offset.Map(offsetMin, offsetMax, 0, 1);
		public void ChangeOffset(float percent) => ruleset.offset = percent.Map(0, 1, offsetMin, offsetMax);
		
		public void ResetWobbleAmplitudeSlider() => wobbleAmplitudeSlider.SliderValue = ruleset.wobbleAmplitude.Map(wobbleAmplitudeMin, wobbleAmplitudeMax, 0, 1);
		public void ChangeWobbleAmplitude(float percent) => ruleset.wobbleAmplitude = percent.Map(0, 1, wobbleAmplitudeMin, wobbleAmplitudeMax);
		
		public void ResetWobbleTimeFactorSlider() => wobbleTimeFactorSlider.SliderValue = ruleset.wobbleTimeFactor.Map(wobbleTimeFactorMin, wobbleTimeFactorMax, 0, 1);
		public void ChangeWobbleTimeFactor(float percent) => ruleset.wobbleTimeFactor = percent.Map(0, 1, wobbleTimeFactorMin, wobbleTimeFactorMax);
		
		public void ResetWobbleSpaceFactorSlider() => wobbleSpaceFactorSlider.SliderValue = ruleset.wobbleSpaceFactor.Map(wobbleSpaceFactorMin, wobbleSpaceFactorMax, 0, 1);
		public void ChangeWobbleSpaceFactor(float percent) => ruleset.wobbleSpaceFactor = percent.Map(0, 1, wobbleSpaceFactorMin, wobbleSpaceFactorMax);
	}
}