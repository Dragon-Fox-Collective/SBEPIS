using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Interaction
{
	public class Orienter : MonoBehaviour
	{
		public void Orient(Vector3 up)
		{
			transform.up = up;
		}
	}
}
