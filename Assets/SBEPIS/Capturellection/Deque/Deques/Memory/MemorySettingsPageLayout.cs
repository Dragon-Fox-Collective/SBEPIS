namespace SBEPIS.Capturellection.Deques
{
	public class MemorySettingsPageLayout : DequeSettingsPageLayout<MemoryDeque>
	{
		public SwitchCardAttacher offsetXFromEndSwitch;
		public SliderCardAttacher offsetXSlider;
		public SwitchCardAttacher offsetYFromEndSwitch;
		public SliderCardAttacher offsetYSlider;
		public float offsetMin = 0;
		public float offsetMax = 1;
		
		public void ResetOffsetXFromEndSwitch() => offsetXFromEndSwitch.SwitchValue = Ruleset.offsetXFromEnd;
		public void ChangeOffsetXFromEnd(bool offsetFromEnd) => Ruleset.offsetXFromEnd = offsetFromEnd;
		
		public void ResetOffsetXSlider() => offsetXSlider.SliderValue = Ruleset.offsetX.Map(offsetMin, offsetMax, 0, 1);
		public void ChangeOffsetX(float percent) => Ruleset.offsetX = percent.Map(0, 1, offsetMin, offsetMax);
		
		public void ResetOffsetYFromEndSwitch() => offsetYFromEndSwitch.SwitchValue = Ruleset.offsetYFromEnd;
		public void ChangeOffsetYFromEnd(bool offsetFromEnd) => Ruleset.offsetYFromEnd = offsetFromEnd;
		
		public void ResetOffsetYSlider() => offsetYSlider.SliderValue = Ruleset.offsetY.Map(offsetMin, offsetMax, 0, 1);
		public void ChangeOffsetY(float percent) => Ruleset.offsetY = percent.Map(0, 1, offsetMin, offsetMax);
	}
}