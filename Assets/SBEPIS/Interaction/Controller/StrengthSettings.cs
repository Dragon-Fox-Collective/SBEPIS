using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Interaction.Controller
{
	[CreateAssetMenu]
	public class StrengthSettings : ScriptableObject
	{
		public float strength = 10;
		public float maxEffectiveDistance = 1;
		public float torque = 10;
		public float maxEffectiveAngle = 1;
	}
}
