using System;
using System.Linq;
using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(DequeBox), typeof(Grabbable), typeof(CollisionTrigger))]
	public class DequeBoxDiajectorOpener : MonoBehaviour
	{
		private DequeBox dequeBox;
		private Grabbable grabbable;
		private CollisionTrigger collisionTrigger;
		
		private void Awake()
		{
			dequeBox = GetComponent<DequeBox>();
			grabbable = GetComponent<Grabbable>();
			collisionTrigger = GetComponent<CollisionTrigger>();
		}
		
		public void AddListeners(DequeBox dequeBox, DequeOwner dequeOwner)
		{
			collisionTrigger.trigger.AddListener(OpenDiajector);
			grabbable.onUse.AddListener(CloseDiajector);
		}
		
		public void RemoveListeners(DequeBox dequeBox, DequeOwner dequeOwner)
		{
			collisionTrigger.trigger.RemoveListener(OpenDiajector);
			grabbable.onUse.RemoveListener(CloseDiajector);
		}
		
		private void OpenDiajector() => dequeBox.OpenDiajector();
		private void CloseDiajector(Grabber grabber, Grabbable grabbable) => dequeBox.CloseDiajector();
	}
}