using Arbor;
using SBEPIS.UI;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	[AddBehaviourMenu("ElectricArc/CreateElectricArc")]
	public class CreateElectricArc : StateBehaviour
	{
		[SerializeField] private FlexibleTransform startpoint;
		[SerializeField] private FlexibleTransform endpoint;
		[SerializeField] private ElectricArc electricArcPrefab;
		
		private ElectricArc arc;
		
		public override void OnStateBegin()
		{
			arc = Instantiate(electricArcPrefab, endpoint.value);
			arc.Init(startpoint.value);
		}
		
		public override void OnStateEnd()
		{
			Destroy(arc);
		}
	}
}
