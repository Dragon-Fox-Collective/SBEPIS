using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using SBEPIS.Controller;
using SBEPIS.Physics;
using SBEPIS.UI;
using SBEPIS.Utils;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabbable), typeof(GravitySum), typeof(SplitTextureSetup))]
	[RequireComponent(typeof(CollisionTrigger), typeof(CouplingPlug), typeof(Animator))]
	public class DequeBox : MonoBehaviour
	{
		public LerpTarget lowerTarget;
		public LerpTarget upperTarget;
		
		public DequeLayer definition;
		
		public DequeOwner owner { get; set; }
		
		public bool isDeployed => state.GetBool(IsDeployed);
		
		public Grabbable grabbable { get; private set; }
		public GravitySum gravitySum { get; private set; }
		public SplitTextureSetup split { get; private set; }
		public CollisionTrigger collisionTrigger { get; private set; }
		public CouplingPlug plug { get; private set; }
		public Animator state { get; private set; }
		
		public static readonly int IsGrabbed = Animator.StringToHash("Is Grabbed");
		public static readonly int IsCoupled = Animator.StringToHash("Is Coupled");
		public static readonly int IsBound = Animator.StringToHash("Is Bound");
		public static readonly int IsDiajectorOpen = Animator.StringToHash("Is Diajector Open");
		public static readonly int IsDeployed = Animator.StringToHash("Is Deployed");
		public static readonly int Toss = Animator.StringToHash("On Toss");

		private void Awake()
		{
			grabbable = GetComponent<Grabbable>();
			gravitySum = GetComponent<GravitySum>();
			split = GetComponent<SplitTextureSetup>();
			collisionTrigger = GetComponent<CollisionTrigger>();
			plug = GetComponent<CouplingPlug>();
			state = GetComponent<Animator>();
		}
		
		private void Start()
		{
			split.UpdateTexture(definition.deques.Select(deque => deque.dequeTexture).ToList());
		}
		
		public void AdoptDeque(Grabber grabber, Grabbable grabbable)
		{
			Capturellector capturellector = grabber.GetComponent<Capturellector>();
			if (!capturellector)
				return;

			DequeOwner dequeOwner = capturellector.owner;
			dequeOwner.dequeBox = this;
		}

		public void SetStateGrabbed(bool grabbed)
		{
			state.SetBool(IsGrabbed, grabbed);
		}
	}
}
