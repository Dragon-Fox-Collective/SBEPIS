using UnityEngine;
using SBEPIS.Controller;
using SBEPIS.Physics;
using SBEPIS.Utils;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabbable), typeof(DequeType), typeof(GravitySum))]
	[RequireComponent(typeof(CollisionTrigger))]
	public class CaptureDeque : MonoBehaviour
	{
		public Transform cardStart;
		public Transform cardTarget;

		public Grabbable grabbable { get; private set; }
		public DequeType dequeType { get; private set; }
		public GravitySum gravitySum { get; private set; }
		public CollisionTrigger collisionTrigger { get; private set; }

		private void Awake()
		{
			grabbable = GetComponent<Grabbable>();
			dequeType = GetComponent<DequeType>();
			gravitySum = GetComponent<GravitySum>();
			collisionTrigger = GetComponent<CollisionTrigger>();
		}

		public void AdoptDeque(Grabber grabber, Grabbable grabbable)
		{
			Capturllector capturllector = grabber.GetComponent<Capturllector>();
			if (!capturllector)
				return;

			DequeOwner dequeOwner = capturllector.dequeOwner;
			if (dequeOwner.deque != this)
			{
				dequeOwner.deque = this;
				if (dequeOwner.diajector.isOpen)
				{
					dequeOwner.ToggleDiajector();
					dequeOwner.ToggleDiajector();
				}
			}
		}
	}
}
