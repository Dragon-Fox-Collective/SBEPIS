using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Utils
{
	public class UpdateProxy : MonoBehaviour
	{
		public UnityEvent<Transform> OnUpdate = new(), OnFixedUpdate = new();

		private void Update()
		{
			OnUpdate.Invoke(transform);
		}

		private void FixedUpdate()
		{
			OnFixedUpdate.Invoke(transform);
		}
	}
}
