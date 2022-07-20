using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Interaction.Controller
{
	public class GravityNormalizer : MonoBehaviour
	{
		public Vector3 upDirection = -UnityEngine.Physics.gravity.normalized;
		public float gravity = UnityEngine.Physics.gravity.magnitude;
	}
}
