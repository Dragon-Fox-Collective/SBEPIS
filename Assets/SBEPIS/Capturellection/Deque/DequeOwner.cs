using System;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(Inventory))]
	public class DequeOwner : MonoBehaviour
	{
		[SerializeField]
		private Deque initialDeque;
		public Diajector diajector;
		
		public EventProperty<DequeOwner, Deque, SetDequeEvent, UnsetDequeEvent> dequeEvents = new();
		public Deque Deque
		{
			get => dequeEvents.Get();
			set => dequeEvents.Set(this, value, deque => deque.dequeSlaveEvents);
		}
		
		public EventPropertySlave<DequeStorable, DequeOwner, SetCardOwnerEvent, UnsetCardOwnerEvent> cardOwnerSlaveEvents = new();
		
		public Inventory Inventory { get; private set; }
		
		private void Awake()
		{
			Inventory = GetComponent<Inventory>();
		}
		
		private void Start()
		{
			Deque = initialDeque;
		}
	}
}
