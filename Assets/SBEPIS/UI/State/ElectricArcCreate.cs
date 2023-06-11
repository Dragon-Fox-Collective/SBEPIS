using Arbor;
using SBEPIS.UI;
using UnityEngine;

namespace SBEPIS.Capturellection.State
{
	[AddBehaviourMenu("ElectricArc/Create")]
	[BehaviourTitle("ElectricArc.Create")]
	public class ElectricArcCreate : StateBehaviour
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
			Destroy(arc.gameObject);
		}
	}
}
