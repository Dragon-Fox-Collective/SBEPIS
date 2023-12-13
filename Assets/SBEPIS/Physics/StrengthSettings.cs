using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Physics
{
	[CreateAssetMenu]
	public class StrengthSettings : ScriptableObject
	{
		[FormerlySerializedAs("linearSpeed")]
		public float LinearSpeed;
		[FormerlySerializedAs("linearMaxForce")]
		public float LinearMaxForce = Mathf.Infinity;
		[FormerlySerializedAs("angularSpeed")]
		public float AngularSpeed;
		[FormerlySerializedAs("angularMaxForce")]
		public float AngularMaxForce = Mathf.Infinity;
	}
}
