using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Utils
{
	public class DistanceTrigger : MonoBehaviour
	{
		public Transform relativeTo;
		public float distance;
		public UnityEvent onExceedDistance;

		private bool exceedingDistance;

		private void Update()
		{
			if (!exceedingDistance)
				if (Vector3.Distance(transform.position, relativeTo.position) > distance)
				{
					exceedingDistance = true;
					onExceedDistance.Invoke();
				}
			else
				if (Vector3.Distance(transform.position, relativeTo.position) < distance)
					exceedingDistance = false;
		}
	}
}
