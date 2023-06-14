using SBEPIS.Capturellection.UI;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class TreeLayoutSettingsPageModule : DequeSettingsPageModule<LayoutSettings<TreeLayout>>
	{
		[SerializeField] private SwitchCardAttacher offsetXFromEndSwitch;
		
		[SerializeField] private SliderCardAttacher offsetXSlider;
		[SerializeField] private float offsetXMin = -0.25f;
		[SerializeField] private float offsetXMax = 0.25f;
		
		[SerializeField] private SwitchCardAttacher offsetYFromEndSwitch;
		
		[SerializeField] private SliderCardAttacher offsetYSlider;
		[SerializeField] private float offsetYMin = -0.25f;
		[SerializeField] private float offsetYMax = 0.25f;
		
		public void ResetOffsetXFromEndSwitch() => offsetXFromEndSwitch.SwitchValue = Settings.Layout.offsetXFromEnd;
		public void ChangeOffsetXFromEnd(bool offsetFromEnd) => Settings.Layout.offsetXFromEnd = offsetFromEnd;
		
		public void ResetOffsetXSlider() => offsetXSlider.SliderValue = Settings.Layout.offsetX.Map(offsetXMin, offsetXMax, 0, 1);
		public void ChangeOffsetX(float percent) => Settings.Layout.offsetX = percent.Map(0, 1, offsetXMin, offsetXMax);
		
		public void ResetOffsetYFromEndSwitch() => offsetYFromEndSwitch.SwitchValue = Settings.Layout.offsetYFromEnd;
		public void ChangeOffsetYFromEnd(bool offsetFromEnd) => Settings.Layout.offsetYFromEnd = offsetFromEnd;
		
		public void ResetOffsetYSlider() => offsetYSlider.SliderValue = Settings.Layout.offsetY.Map(offsetYMin, offsetYMax, 0, 1);
		public void ChangeOffsetY(float percent) => Settings.Layout.offsetY = percent.Map(0, 1, offsetYMin, offsetYMax);
	}
}