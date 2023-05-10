namespace SBEPIS.Capturellection.Deques
{
	public class FlippedGridSettingsPageLayout : DequeSettingsPageLayout<FlippedGridLayout>
	{
		public SliderCardAttacher offsetXSlider;
		public SliderCardAttacher offsetYSlider;
		public float offsetMin = -0.25f;
		public float offsetMax = 0.25f;
		
		public void ResetOffsetXSlider() => offsetXSlider.SliderValue = Object.offset.x.Map(offsetMin, offsetMax, 0, 1);
		public void ChangeOffsetX(float percent) => Object.offset.x = percent.Map(0, 1, offsetMin, offsetMax);
		
		public void ResetOffsetYSlider() => offsetYSlider.SliderValue = Object.offset.y.Map(offsetMin, offsetMax, 0, 1);
		public void ChangeOffsetY(float percent) => Object.offset.y = percent.Map(0, 1, offsetMin, offsetMax);
	}
}