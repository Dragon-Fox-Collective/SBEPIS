using UnityEngine;
using System.Linq;

namespace SBEPIS.Physics
{
	public class ConfigurableMassiveBody : MassiveBody
	{
		[SerializeField]
		private AnimationCurve gravityCurve = AnimationCurve.Linear(0, 0, 1, 1);
		[SerializeField]
		private bool useX = true;
		[SerializeField]
		private bool useY = true;
		[SerializeField]
		private bool useZ = true;
		[SerializeField]
		private AnimationCurve priorityLerpX = AnimationCurve.Constant(0, 1, 1);
		[SerializeField]
		private AnimationCurve priorityLerpY = AnimationCurve.Constant(0, 1, 1);
		[SerializeField]
		private AnimationCurve priorityLerpZ = AnimationCurve.Constant(0, 1, 1);

		private bool[] uses;
		private AnimationCurve[] priorityLerps;

		private void Awake()
		{
			uses = new[] { useX, useY, useZ };
			priorityLerps = new[] { priorityLerpX, priorityLerpY, priorityLerpZ };
		}

		public override Vector3 GetPriority(Vector3 centerOfMass)
		{
			return VectorExtensions.SelectVectorIndex(i => priorityLerps[i].Evaluate(centerOfMass[i]));
		}

		public override Vector3 GetGravity(Vector3 centerOfMass)
		{
			Vector3 delta = centerOfMass.AsEnumerable().Zip(uses, (x, use) => use ? -x : 0).ToVector3();
			return gravityCurve.Evaluate(delta.magnitude) * delta.normalized;
		}
	}
}
