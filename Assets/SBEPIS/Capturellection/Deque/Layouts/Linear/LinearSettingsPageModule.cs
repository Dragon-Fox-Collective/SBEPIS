using SBEPIS.Capturellection.UI;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class LinearSettingsPageModule : DequeSettingsPageModule<LayoutSettings<LinearLayout>>
	{
		[SerializeField] private SwitchCardAttacher offsetFromEndSwitch;
		
		[SerializeField] private SliderCardAttacher offsetSlider;
		[SerializeField] private float offsetMin = -0.25f;
		[SerializeField] private float offsetMax = 0.25f;
		
		public void ResetOffsetFromEndSwitch() => offsetFromEndSwitch.SwitchValue = Settings.Layout.offsetFromEnd;
		public void ChangeOffsetFromEnd(bool offsetFromEnd) => Settings.Layout.offsetFromEnd = offsetFromEnd;
		
		public void ResetOffsetSlider() => offsetSlider.SliderValue = Settings.Layout.offset.Map(offsetMin, offsetMax, 0, 1);
		public void ChangeOffset(float percent) => Settings.Layout.offset = percent.Map(0, 1, offsetMin, offsetMax);
	}
}