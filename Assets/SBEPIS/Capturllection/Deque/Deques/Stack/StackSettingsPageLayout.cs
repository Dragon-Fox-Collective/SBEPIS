using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	public class StackSettingsPageLayout : DequeSettingsPageLayout<StackDeque>
	{
		public SwitchCardAttacher offsetFromEndSwitch;
		public SliderCardAttacher offsetSlider;
		public float offsetMin = 0;
		public float offsetMax = 1;
		
		public void ResetOffsetFromEndSwitch() => offsetFromEndSwitch.SwitchValue = ruleset.offsetFromEnd;
		public void ChangeOffsetFromEnd(bool offsetFromEnd) => ruleset.offsetFromEnd = offsetFromEnd;
		
		public void ResetOffsetSlider() => offsetSlider.SliderValue = ruleset.offset.Map(offsetMin, offsetMax, 0, 1);
		public void ChangeOffset(float percent) => ruleset.offset = percent.Map(0, 1, offsetMin, offsetMax);
	}
}