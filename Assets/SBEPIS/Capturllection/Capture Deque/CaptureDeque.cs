using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SBEPIS.Controller;
using SBEPIS.Physics;
using SBEPIS.Thaumaturgy;
using SBEPIS.Utils;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabbable), typeof(GravitySum), typeof(SplitTextureSetup))]
	[RequireComponent(typeof(CollisionTrigger), typeof(CouplingPlug))]
	public class CaptureDeque : MonoBehaviour
	{
		public Transform cardStart;
		public Transform cardTarget;
		
		public DequeLayer definition;

		public Grabbable grabbable { get; private set; }
		public GravitySum gravitySum { get; private set; }
		public SplitTextureSetup split { get; private set; }
		public CollisionTrigger collisionTrigger { get; private set; }
		public CouplingPlug plug { get; private set; }

		private void Awake()
		{
			grabbable = GetComponent<Grabbable>();
			gravitySum = GetComponent<GravitySum>();
			split = GetComponent<SplitTextureSetup>();
			collisionTrigger = GetComponent<CollisionTrigger>();
			plug = GetComponent<CouplingPlug>();
		}

		private void Start()
		{
			split.UpdateTexture(definition.deques.Select(deque => deque.dequeTexture).ToList());
		}

		public void AdoptDeque(Grabber grabber, Grabbable grabbable)
		{
			Capturllector capturllector = grabber.GetComponent<Capturllector>();
			if (!capturllector)
				return;

			DequeOwner dequeOwner = capturllector.dequeOwner;
			dequeOwner.deque = this;
		}
	}
}
