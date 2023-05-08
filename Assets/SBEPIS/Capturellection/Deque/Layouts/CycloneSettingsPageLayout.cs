using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class CycloneSettingsPageLayout : DequeSettingsPageLayout<CycloneLayout>
	{
		public SliderCardAttacher timePerCardSlider;
		public float timePerCardMin = 0.1f;
		public float timePerCardMax = 5;
		
		public void ResetTimePerCardSlider() => timePerCardSlider.SliderValue = Object.timePerCard.Map(timePerCardMin, timePerCardMax, 0, 1);
		public void ChangeTimePerCard(float percent) => Object.timePerCard = percent.Map(0, 1, timePerCardMin, timePerCardMax);
	}
}