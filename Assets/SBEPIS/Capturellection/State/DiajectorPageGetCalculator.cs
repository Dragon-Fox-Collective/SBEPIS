using System;
using Arbor;
using SBEPIS.Utils.State;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("DiajectorPage/DiajectorPage.Get")]
	[BehaviourTitle("DiajectorPage.Get")]
	public class DiajectorPageGetCalculator : GetCalculator<DiajectorPage, OutputSlotDiajectorPage> { }
	
	[Serializable]
	public class OutputSlotDiajectorPage : OutputSlot<DiajectorPage> { }
	
	[Serializable]
	public class FlexibleDiajectorPage : FlexibleComponent<DiajectorPage> { }
}
