namespace SBEPIS.Capturellection.Deques
{
	public class FlippedGridSettingsPageLayout : DequeSettingsPageLayout<FlippedGridLayout>
	{
		public SwitchCardAttacher offsetXFromEndSwitch;
		public SliderCardAttacher offsetXSlider;
		public SwitchCardAttacher offsetYFromEndSwitch;
		public SliderCardAttacher offsetYSlider;
		public float offsetMin = 0;
		public float offsetMax = 1;
		
		public void ResetOffsetXFromEndSwitch() => offsetXFromEndSwitch.SwitchValue = Object.offsetXFromEnd;
		public void ChangeOffsetXFromEnd(bool offsetFromEnd) => Object.offsetXFromEnd = offsetFromEnd;
		
		public void ResetOffsetXSlider() => offsetXSlider.SliderValue = Object.offsetX.Map(offsetMin, offsetMax, 0, 1);
		public void ChangeOffsetX(float percent) => Object.offsetX = percent.Map(0, 1, offsetMin, offsetMax);
		
		public void ResetOffsetYFromEndSwitch() => offsetYFromEndSwitch.SwitchValue = Object.offsetYFromEnd;
		public void ChangeOffsetYFromEnd(bool offsetFromEnd) => Object.offsetYFromEnd = offsetFromEnd;
		
		public void ResetOffsetYSlider() => offsetYSlider.SliderValue = Object.offsetY.Map(offsetMin, offsetMax, 0, 1);
		public void ChangeOffsetY(float percent) => Object.offsetY = percent.Map(0, 1, offsetMin, offsetMax);
	}
}