using SBEPIS.Capturellection.UI;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class CycloneSettingsPageModule : DequeSettingsPageModule<LayoutSettings<CycloneLayout>>
	{
		[SerializeField] private SliderCardAttacher timePerCardSlider;
		[SerializeField] private float timePerCardMin = 0.1f;
		[SerializeField] private float timePerCardMax = 5;
		
		public void ResetTimePerCardSlider() => timePerCardSlider.SliderValue = Settings.Layout.timePerCard.Map(timePerCardMin, timePerCardMax, 0, 1);
		public void ChangeTimePerCard(float percent) => Settings.Layout.timePerCard = percent.Map(0, 1, timePerCardMin, timePerCardMax);
	}
}