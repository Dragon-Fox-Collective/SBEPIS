using System;
using SBEPIS.Capturellection.UI;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class FlippedGridSettingsPageModule : DequeSettingsPageModule<LayoutSettings<FlippedGridLayout>>
	{
		[SerializeField] private SliderCardAttacher offsetXSlider;
		[SerializeField] private SliderCardAttacher offsetYSlider;
		[SerializeField] private float offsetMin = -0.25f;
		[SerializeField] private float offsetMax = 0.25f;
		
		public void ResetOffsetXSlider() => offsetXSlider.SliderValue = Settings.Layout.offset.x.Map(offsetMin, offsetMax, 0, 1);
		public void ChangeOffsetX(float percent) => Settings.Layout.offset.x = percent.Map(0, 1, offsetMin, offsetMax);
		
		public void ResetOffsetYSlider() => offsetYSlider.SliderValue = Settings.Layout.offset.y.Map(offsetMin, offsetMax, 0, 1);
		public void ChangeOffsetY(float percent) => Settings.Layout.offset.y = percent.Map(0, 1, offsetMin, offsetMax);
	}
}