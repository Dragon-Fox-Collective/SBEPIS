using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Interaction.Controller
{
	[CreateAssetMenu]
	public class StrengthSettings : ScriptableObject
	{
		public AnimationCurve strengthCurve;
		public float torqueInputFactor = 1;
	}
}
