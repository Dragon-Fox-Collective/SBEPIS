using UnityEngine;
using System.Linq;

namespace SBEPIS.Interaction.Physics
{
	public class ConfigurableMassiveBody : MassiveBody
	{
		public AnimationCurve gravityCurve;
		public bool useX;
		public bool useY;
		public bool useZ;
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
			return ExtensionMethods.SelectVectorIndex(i => priorityLerps[i].Evaluate(centerOfMass[i]));
		}

		public override Vector3 GetGravity(Vector3 centerOfMass)
		{
			Vector3 delta = centerOfMass.AsEnumerable().Zip(uses, (x, use) => use ? -x : 0).AsVector3();
			return gravityCurve.Evaluate(delta.magnitude) * delta.normalized;
		}
	}
}
