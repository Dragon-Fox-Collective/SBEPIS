using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public class CaptureCamerable : MonoBehaviour
	{
		public Vector3 location;
		public Quaternion rotation;
		public float scale = 1;

		public UnityEvent prePicture = new(), postPicture = new();
	}
}
