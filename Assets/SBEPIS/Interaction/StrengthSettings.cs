using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Interaction
{
	[CreateAssetMenu]
	public class StrengthSettings : ScriptableObject
	{
		[Header("New system")]
		public float velocityTarget = 120;
		public float maxAcceleration = 20;
		public float angularVelocityTarget = 1;
		public float maxAngularAcceleration = 10;

		[Header("Old system")]
		public float strength = 10;
		public float maxEffectiveDistance = 1;
		public float torque = 10;
		public float maxEffectiveAngle = 1;
	}
}
