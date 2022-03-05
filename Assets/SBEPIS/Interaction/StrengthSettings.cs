using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Interaction
{
	[CreateAssetMenu]
	public class StrengthSettings : ScriptableObject
	{
		[Tooltip("A grabber's strength factor")]
		public float velocityFactor = 1;
		[Tooltip("A grabber's maximum strength capacity")]
		public float maxVelocity = -1;
		[Tooltip("A grabber's torque strength factor")]
		public float angularVelocityFactor = 1;
		[Tooltip("A grabber's maximum torque strength capacity")]
		public float maxAngularVelocity = -1;
	}
}
