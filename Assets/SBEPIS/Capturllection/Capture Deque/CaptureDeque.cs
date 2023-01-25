using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SBEPIS.Controller;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabbable), typeof(DequeType))]
	public class CaptureDeque : MonoBehaviour
	{
		public Transform cardStart;
		public Transform cardTarget;

		public Grabbable grabbable { get; private set; }
		public DequeType dequeType { get; private set; }

		private void Awake()
		{
			grabbable = GetComponent<Grabbable>();
			dequeType = GetComponent<DequeType>();
		}

		public void ToggleDiajector(Grabber grabber, Grabbable grabbable)
		{
			Capturllector capturllector = grabber.GetComponent<Capturllector>();
			if (!capturllector)
				return;

			DequeOwner dequeOwner = capturllector.dequeOwner;
			if (dequeOwner.deque == this)
				dequeOwner.ToggleDiajector();
			else
			{
				dequeOwner.deque = this;
				if (dequeOwner.diajector.hasPageOpen)
				{
					dequeOwner.ToggleDiajector();
					dequeOwner.ToggleDiajector();
				}
			}
		}
	}
}
