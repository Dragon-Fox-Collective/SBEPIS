using System;
using Arbor;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("DiajectorPage/DiajectorPage.Get")]
	[BehaviourTitle("DiajectorPage.Get")]
	public class DiajectorPageGetCalculator : GetCalculator<DiajectorPage, OutputSlotDiajectorPage> { }
	
	[Serializable]
	public class OutputSlotDiajectorPage : OutputSlot<DiajectorPage> { }
	
	[Serializable]
	public class FlexibleDiajectorPage : FlexibleComponent<DiajectorPage> { }
}
