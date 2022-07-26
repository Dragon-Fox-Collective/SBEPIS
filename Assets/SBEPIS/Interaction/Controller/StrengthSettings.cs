using UnityEngine;

namespace SBEPIS.Interaction.Controller
{
	[CreateAssetMenu]
	public class StrengthSettings : ScriptableObject
	{
		public float linearSpring;
		public float linearDamper;
		public float linearMaxForce;
		public float angularSpring;
		public float angularDamper;
		public float angularMaxForce;
	}
}
