using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Utils
{
	public class CollisionTrigger : MonoBehaviour
	{
		[SerializeField] private float impulseThreshold = 0f;
		
		public UnityEvent<float> onCollide = new();
		
		private void OnCollisionEnter(Collision collision)
		{
			float impulse = collision.impulse.magnitude;
			if (impulse >= impulseThreshold)
				onCollide.Invoke(impulse);
		}
	}
}
