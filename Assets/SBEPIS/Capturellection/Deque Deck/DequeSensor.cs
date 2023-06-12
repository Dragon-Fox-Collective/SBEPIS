using SBEPIS.Capturellection;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Utils
{
	public class DequeSensor : MonoBehaviour
	{
		[SerializeField, Range(0, 1)]
		private int layerIndex;
		public int LayerIndex => layerIndex;
		
		[SerializeField, Range(0, 2)]
		private int groupIndex;
		public int GroupIndex => groupIndex;
		
		public UnityEvent onDequeChanged = new();
		
		public DequeBox DequeBox { get; private set; }
		
		private void OnTriggerEnter(Collider other)
		{
			if (DequeBox != null) return;
			if (!other.attachedRigidbody) return;
			if (!other.attachedRigidbody.TryGetComponent(out DequeBox collisionDequeBox)) return;
			
			DequeBox = collisionDequeBox;
			
			if (DequeBox.TryGetComponent(out DelayedCollisionTrigger collisionTrigger))
				collisionTrigger.CancelPrime();
			
			onDequeChanged.Invoke();
		}
		
		private void OnTriggerExit(Collider other)
		{
			if (DequeBox == null) return;
			if (!other.attachedRigidbody) return;
			if (!other.attachedRigidbody.TryGetComponent(out DequeBox collisionDequeBox)) return;
			
			if (DequeBox == collisionDequeBox)
				DequeBox = null;
			
			onDequeChanged.Invoke();
		}
	}
}
