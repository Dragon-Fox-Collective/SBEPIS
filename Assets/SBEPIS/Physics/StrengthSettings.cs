using UnityEngine;

namespace SBEPIS.Physics
{
	[CreateAssetMenu]
	public class StrengthSettings : ScriptableObject
	{
		public float linearDampingTime = 1;
		public float linearMaxSpeed = Mathf.Infinity;
		public float linearMaxForce = Mathf.Infinity;
		public float angularDampingTime = 1;
		public float angularMaxSpeed = Mathf.Infinity;
		public float angularMaxForce = Mathf.Infinity;
	}
}
