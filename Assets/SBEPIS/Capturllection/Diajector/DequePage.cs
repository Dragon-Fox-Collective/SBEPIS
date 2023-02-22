using SBEPIS.Controller;
using SBEPIS.Physics;
using System.Collections;
using System.Collections.Generic;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public class DequePage : MonoBehaviour
	{
		public UnityEvent onPreparePage = new();
		
		public Diajector diajector { get; private set; }

		private readonly Dictionary<DequeStorable, CardTarget> cardTargets = new();

		private void Awake()
		{
			diajector = GetComponentInParent<Diajector>();
		}

		private void CreateCards(IEnumerable<CardTarget> targets)
		{
			foreach (CardTarget target in targets)
			{
				DequeStorable card = Instantiate(diajector.cardPrefab);
				card.name += $" ({target.label})";
				card.owner = diajector.owner.dequeBox.owner;
				card.state.SetBool(DequeStorable.IsBound, true);
				target.card = card;
				
				LerpTargetAnimator animator = AddCard(card, target);
				animator.TeleportTo(diajector.owner.dequeBox.lowerTarget);

				Capturellectainer container = card.GetComponent<Capturellectainer>();
				container.isRetrievingAllowed = false

				Capturllectable capturllectable = card.GetComponent<Capturllectable>();
				capturllectable.canCapturllect = false;
			}
		}

		public LerpTargetAnimator AddCard(DequeStorable card, CardTarget target)
		{
			card.grabbable.onGrab.AddListener(DestroyCardJoint);
			card.grabbable.onDrop.AddListener(CreateCardJoint);
			card.grabbable.onGrab.AddListener(InvokeCardTargetOnGrab);
			card.grabbable.onDrop.AddListener(InvokeCardTargetOnDrop);

			cardTargets.Add(card, target);
			
			diajector.owner.dequeBox.definition.UpdateCardTexture(card);
			
			target.onCardCreated.Invoke(card);
		}

		public void RemoveCard(DequeStorable card)
		{
			card.grabbable.onGrab.RemoveListener(DestroyCardJoint);
			card.grabbable.onDrop.RemoveListener(CreateCardJoint);
			card.grabbable.onGrab.RemoveListener(InvokeCardTargetOnGrab);
			card.grabbable.onDrop.RemoveListener(InvokeCardTargetOnDrop);

			if (cardTargets[card].targetter)
				DestroyCardJoint(card);
			
			cardTargets.Remove(card);
		}

		private void CreateCardJoint(Grabber grabber, Grabbable grabbable) => CreateCardJoint(grabbable.GetComponent<DequeStorable>());
		private void CreateCardJoint(DequeStorable target) => CreateCardJoint(cardTargets[target]);
		public void CreateCardJoint(CardTarget target) => CreateCardJoint(target.card, target, diajector.staticRigidbody, diajector.cardStrength);
		private static void CreateCardJoint(DequeStorable card, CardTarget target, Rigidbody staticRigidbody, StrengthSettings cardStrength)
		{
			JointTargetter targetter = staticRigidbody.gameObject.AddComponent<JointTargetter>();
			targetter.connectedBody = card.grabbable.rigidbody;
			targetter.target = target.transform;
			targetter.strength = cardStrength;
			target.targetter = targetter;
		}

		private void DestroyCardJoint(Grabber grabber, Grabbable grabbable) => DestroyCardJoint(grabbable.GetComponent<DequeStorable>());
		private void DestroyCardJoint(DequeStorable card) => DestroyCardJoint(cardTargets[card]);
		public void DestroyCardJoint(CardTarget target)
		{
			JointTargetter targetter = target.targetter;
			target.targetter = null;
			Destroy(targetter);
		}

		private void InvokeCardTargetOnGrab(Grabber grabber, Grabbable grabbable)
		{
			cardTargets[grabbable.GetComponent<DequeStorable>()].onGrab.Invoke();
		}
		
		private void InvokeCardTargetOnDrop(Grabber grabber, Grabbable grabbable)
		{
			cardTargets[grabbable.GetComponent<DequeStorable>()].onDrop.Invoke();
		}

		public bool HasCard(DequeStorable card) => cardTargets.ContainsKey(card);
		public LerpTarget GetLerpTarget(DequeStorable card) => cardTargets[card].lerpTarget;

		public void Refresh()
		{
			onPreparePage.Invoke();
		}
		
		public void StartAssembly()
		{
			gameObject.SetActive(true);
			if (cardTargets.Count == 0)
				CreateCards(GetComponentsInChildren<CardTarget>());
			foreach ((DequeStorable card, CardTarget target) in cardTargets)
				card.state.SetBool(DequeStorable.IsPageOpen, true);
			onPreparePage.Invoke();
			diajector.coroutineOwner.StartCoroutine(SpawnCards());
		}

		private IEnumerator SpawnCards()
		{
			foreach ((DequeStorable card, CardTarget target) in cardTargets)
			{
				target.onPrepareCard.Invoke();
				card.state.SetBool(DequeStorable.IsAssembling, true);
				card.state.SetBool(DequeStorable.IsDisassembling, false);
				yield return new WaitForSeconds(diajector.cardDelay);
			}
		}

		public void StartDisassembly()
		{
			foreach ((DequeStorable card, CardTarget target) in cardTargets)
				card.state.SetBool(DequeStorable.IsPageOpen, false);
			diajector.coroutineOwner.StartCoroutine(DespawnCards());
			gameObject.SetActive(false);
		}

		private IEnumerator DespawnCards()
		{
			foreach ((DequeStorable card, CardTarget target) in cardTargets)
			{
				card.state.SetBool(DequeStorable.IsAssembling, false);
				card.state.SetBool(DequeStorable.IsDisassembling, true);
				yield return new WaitForSeconds(diajector.cardDelay);
			}
		}

		public void ForceClose(LerpTarget bottomTarget)
		{
			foreach ((DequeStorable card, CardTarget target) in cardTargets)
			{
				card.state.SetBool(DequeStorable.IsAssembling, false);
				card.state.SetBool(DequeStorable.IsDisassembling, false);
				card.state.SetTrigger(DequeStorable.ForceClose);
			}
			
			gameObject.SetActive(false);
		}
	}
}
