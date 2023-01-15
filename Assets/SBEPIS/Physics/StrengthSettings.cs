using UnityEngine;

namespace SBEPIS.Physics
{
	[CreateAssetMenu]
	public class StrengthSettings : ScriptableObject
	{
		public float linearSpeed;
		public float linearMaxForce = Mathf.Infinity;
		public float angularSpeed;
		public float angularMaxForce = Mathf.Infinity;
	}
}
