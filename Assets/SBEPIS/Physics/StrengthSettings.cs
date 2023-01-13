using UnityEngine;

namespace SBEPIS.Physics
{
	[CreateAssetMenu]
	public class StrengthSettings : ScriptableObject
	{
		public float linearSpring;
		public float linearDamper;
		public float linearMaxForce = Mathf.Infinity;
		public float angularSpring;
		public float angularDamper;
		public float angularMaxForce = Mathf.Infinity;
	}
}
