using UnityEngine;
using System.Linq;
using SBEPIS.Utils.VectorLinq;

namespace SBEPIS.Physics
{
	public class ConfigurableMassiveBody : MassiveBody
	{
		public AnimationCurve gravityCurve = AnimationCurve.Linear(0, 0, 1, 1);
		public bool useX = true;
		public bool useY = true;
		public bool useZ = true;
		public AnimationCurve priorityLerpX = AnimationCurve.Constant(0, 1, 1);
		public AnimationCurve priorityLerpY = AnimationCurve.Constant(0, 1, 1);
		public AnimationCurve priorityLerpZ = AnimationCurve.Constant(0, 1, 1);

		private bool[] uses;
		private AnimationCurve[] priorityLerps;

		private void Awake()
		{
			uses = new bool[] { useX, useY, useZ };
			priorityLerps = new AnimationCurve[] { priorityLerpX, priorityLerpY, priorityLerpZ };
		}

		public override Vector3 GetPriority(Vector3 centerOfMass)
		{
			return VectorLINQ.SelectVectorIndex(i => priorityLerps[i].Evaluate(centerOfMass[i]));
		}

		public override Vector3 GetGravity(Vector3 centerOfMass)
		{
			Vector3 delta = centerOfMass.AsEnumerable().Zip(uses, (x, use) => use ? -x : 0).ToVector3();
			return gravityCurve.Evaluate(delta.magnitude) * delta.normalized;
		}
	}
}
