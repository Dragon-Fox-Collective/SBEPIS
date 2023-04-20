using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class StateSettingsPageLayout : DequeSettingsPageLayout<StateDeque>
	{
		public SwitchCardAttacher offsetFromEndSwitch;
		
		public SliderCardAttacher offsetSlider;
		public float offsetMin = 0;
		public float offsetMax = 1;
		
		public void ResetOffsetFromEndSwitch() => offsetFromEndSwitch.SwitchValue = Ruleset.offsetFromEnd;
		public void ChangeOffsetFromEnd(bool offsetFromEnd) => Ruleset.offsetFromEnd = offsetFromEnd;
		
		public void ResetOffsetSlider() => offsetSlider.SliderValue = Ruleset.offset.Map(offsetMin, offsetMax, 0, 1);
		public void ChangeOffset(float percent) => Ruleset.offset = percent.Map(0, 1, offsetMin, offsetMax);
	}
}