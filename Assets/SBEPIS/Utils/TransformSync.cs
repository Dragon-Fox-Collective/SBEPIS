using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Utils
{
	public class TransformSync : MonoBehaviour
	{
		public void SyncPosition(Transform other)
		{
			transform.position = other.position;
		}

		public void SyncRotation(Transform other)
		{
			transform.rotation = other.rotation;
		}
	}
}
