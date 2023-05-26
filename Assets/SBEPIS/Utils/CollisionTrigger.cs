using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Utils
{
	public class CollisionTrigger : MonoBehaviour
	{
		[SerializeField] private float impulseThreshold = 0f;
		
		public UnityEvent onCollide = new();
		
		private void OnCollisionEnter(Collision collision)
		{
			if (collision.impulse.magnitude >= impulseThreshold)
				onCollide.Invoke();
		}
	}
}
