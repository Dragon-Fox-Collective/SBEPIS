using UnityEngine;

namespace SBEPIS.Physics
{
	[CreateAssetMenu]
	public class StrengthSettings : ScriptableObject
	{
		public float linearSpring;
		public float linearDamper;
		public float linearMaxForce = 3.402823e+38f;
		public float angularSpring;
		public float angularDamper;
		public float angularMaxForce = 3.402823e+38f;
	}
}
