using UnityEngine;

namespace SBEPIS.Interaction.Controller
{
	[CreateAssetMenu]
	public class StrengthSettings : ScriptableObject
	{
		public AnimationCurve strength;
		public AnimationCurve torque;
		public AnimationCurve linearDistanceTorqueFactor;
	}
}
