using System.Linq;
using UnityEngine;
using SBEPIS.Controller;
using SBEPIS.Physics;
using SBEPIS.Utils;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabbable), typeof(GravitySum), typeof(SplitTextureSetup))]
	[RequireComponent(typeof(CollisionTrigger), typeof(CouplingPlug))]
	public class DequeBox : MonoBehaviour
	{
		public LerpTarget lowerTarget;
		public LerpTarget upperTarget;
		
		public DequeLayer definition;
		
		public DequeOwner owner { get; set; }
		
		public bool isDeployed => !plug.isCoupled;
		
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
			Capturellector capturellector = grabber.GetComponent<Capturellector>();
			if (!capturellector)
				return;

			DequeOwner dequeOwner = capturellector.dequeOwner;
			dequeOwner.dequeBox = this;
		}
		
		public void AddHeldTemporaryTarget(DequeStorable card, Grabbable cardGrabbable)
		{
			definition.UpdateCardTexture(card);
			cardGrabbable.onDrop.AddListener(AddTemporaryTarget);
			// Add arc
		}
		
		private void AddTemporaryTarget(Grabber grabber, Grabbable grabbable)
		{
			grabbable.onDrop.RemoveListener(AddTemporaryTarget);
			AddTemporaryTarget(grabbable.GetComponent<DequeStorable>());
			// Remove arc
		}
		public void AddTemporaryTarget(DequeStorable card)
		{
			definition.UpdateCardTexture(card);
			
			LerpTargetAnimator animator = card.gameObject.AddComponent<LerpTargetAnimator>();
			animator.curve = owner.diajector.curve;
			animator.AddListenerOnMoveTo(lowerTarget, CleanUpTemporaryTarget);
			animator.TargetTo(upperTarget);
		}
		
		public void CleanUpTemporaryTarget(LerpTargetAnimator animator)
		{
			animator.RemoveListenerOnMoveTo(lowerTarget, CleanUpTemporaryTarget);
			Destroy(animator);
		}
	}
}
