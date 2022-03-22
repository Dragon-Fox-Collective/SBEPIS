using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Interaction
{
	[CreateAssetMenu]
	public class StrengthSettings : ScriptableObject
	{
		public float velocity = 120;
		public float maxAcceleration = 20;
		public float angularVelocity = 1;
		public float maxAngularAcceleration = 10;
	}
}
