namespace SBEPIS.Capturellection.Deques
{
	public class LinearSettingsPageLayout : DequeSettingsPageLayout<LinearLayout>
	{
		public SwitchCardAttacher offsetFromEndSwitch;
		
		public SliderCardAttacher offsetSlider;
		public float offsetMin = 0;
		public float offsetMax = 1;
		
		public void ResetOffsetFromEndSwitch() => offsetFromEndSwitch.SwitchValue = Object.offsetFromEnd;
		public void ChangeOffsetFromEnd(bool offsetFromEnd) => Object.offsetFromEnd = offsetFromEnd;
		
		public void ResetOffsetSlider() => offsetSlider.SliderValue = Object.offset.Map(offsetMin, offsetMax, 0, 1);
		public void ChangeOffset(float percent) => Object.offset = percent.Map(0, 1, offsetMin, offsetMax);
	}
}