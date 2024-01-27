using System;
using SBEPIS.Capturellection.UI;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class WobblySettingsPageModule : DequeSettingsPageModule<LayoutSettings<WobblyLayout>>
	{
		[SerializeField] private SwitchCardAttacher offsetFromEndSwitch;
		
		[SerializeField] private SliderCardAttacher offsetSlider;
		[SerializeField] private float offsetMin = 0;
		[SerializeField] private float offsetMax = 1;
		
		[SerializeField] private SliderCardAttacher wobbleAmplitudeSlider;
		[SerializeField] private float wobbleAmplitudeMin = 0;
		[SerializeField] private float wobbleAmplitudeMax = 1;
		
		[SerializeField] private SliderCardAttacher wobbleTimeFactorSlider;
		[SerializeField] private float wobbleTimeFactorMin = 0;
		[SerializeField] private float wobbleTimeFactorMax = 3;
		
		[SerializeField] private SliderCardAttacher wobbleSpaceFactorSlider;
		[SerializeField] private float wobbleSpaceFactorMin = 0;
		[SerializeField] private float wobbleSpaceFactorMax = 10;
		
		public void ResetOffsetFromEndSwitch() => offsetFromEndSwitch.SwitchValue = Settings.Layout.offsetFromEnd;
		public void ChangeOffsetFromEnd(bool offsetFromEnd) => Settings.Layout.offsetFromEnd = offsetFromEnd;
		
		public void ResetOffsetSlider() => offsetSlider.SliderValue = Settings.Layout.offset.Map(offsetMin, offsetMax, 0, 1);
		public void ChangeOffset(float percent) => Settings.Layout.offset = percent.Map(0, 1, offsetMin, offsetMax);
		
		public void ResetWobbleAmplitudeSlider() => wobbleAmplitudeSlider.SliderValue = Settings.Layout.wobbleAmplitude.Map(wobbleAmplitudeMin, wobbleAmplitudeMax, 0, 1);
		public void ChangeWobbleAmplitude(float percent) => Settings.Layout.wobbleAmplitude = percent.Map(0, 1, wobbleAmplitudeMin, wobbleAmplitudeMax);
		
		public void ResetWobbleTimeFactorSlider() => wobbleTimeFactorSlider.SliderValue = Settings.Layout.wobbleTimeFactor.Map(wobbleTimeFactorMin, wobbleTimeFactorMax, 0, 1);
		public void ChangeWobbleTimeFactor(float percent) => Settings.Layout.wobbleTimeFactor = percent.Map(0, 1, wobbleTimeFactorMin, wobbleTimeFactorMax);
		
		public void ResetWobbleSpaceFactorSlider() => wobbleSpaceFactorSlider.SliderValue = Settings.Layout.wobbleSpaceFactor.Map(wobbleSpaceFactorMin, wobbleSpaceFactorMax, 0, 1);
		public void ChangeWobbleSpaceFactor(float percent) => Settings.Layout.wobbleSpaceFactor = percent.Map(0, 1, wobbleSpaceFactorMin, wobbleSpaceFactorMax);
	}
}