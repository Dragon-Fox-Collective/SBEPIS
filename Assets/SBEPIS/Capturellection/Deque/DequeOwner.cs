using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class DequeOwner : MonoBehaviour
	{
		[SerializeField]
		private Deque initialDeque;
		public Diajector diajector;
		
		[SerializeField]
		private EventProperty<DequeOwner, Deque, SetDequeEvent, UnsetDequeEvent> dequeEvents = new();
		public Deque Deque
		{
			get => dequeEvents.Get();
			set => dequeEvents.Set(this, value, deque => deque.dequeSlaveEvents);
		}
		
		public EventPropertySlave<DequeStorable, DequeOwner, SetCardOwnerEvent, UnsetCardOwnerEvent> cardOwnerSlaveEvents = new();
		
		private void Awake()
		{
			Deque = initialDeque;
		}
		
		private void Start()
		{
			diajector.DequeOwner = this;
		}
		
		public void UnsetDequeOwner(DequeOwner dequeOwner, Deque oldDeque, Deque newDeque) => oldDeque.dequeOwner = null;
		public void SetDequeOwner(DequeOwner dequeOwner, Deque deque) => deque.dequeOwner = this;
	}
}
