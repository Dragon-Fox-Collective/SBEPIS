using System;
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
		public Diajector diajector { get; private set; }
		
		private UnityEvent onPreparePage = new();

		private readonly Dictionary<LerpTargetAnimator, CardTarget> animators = new();
		private readonly Dictionary<DequeStorable, CardTarget> cardTargets = new();

		private void Awake()
		{
			diajector = GetComponentInParent<Diajector>();
			CreateCards();
		}

		private void CreateCards()
		{
			foreach (CardTarget target in GetComponentsInChildren<CardTarget>())
			{
				DequeStorable card = Instantiate(diajector.cardPrefab);
				card.name += $" ({target.label})";
				target.card = card;
				
				LerpTargetAnimator animator = AddCard(card, target);
				animator.TeleportTo(diajector.deque.lowerTarget);

				Capturellectainer container = card.GetComponent<Capturellectainer>();
				container.isRetrievingAllowed = false;

				Capturllectable capturllectable = card.GetComponent<Capturllectable>();
				capturllectable.canCapturllect = false;

				Grabbable grabbable = card.grabbable;
				grabbable.onGrab.AddListener((grabber, grabbable) => target.onGrab.Invoke());
				grabbable.onDrop.AddListener((grabber, grabbable) => target.onDrop.Invoke());

				target.onCardCreated.Invoke(card);
			}
		}

		public LerpTargetAnimator AddCard(DequeStorable card, CardTarget target)
		{
			card.isStored = true;

			card.grabbable.onGrab.AddListener(DestroyCardJoint);
			card.grabbable.onDrop.AddListener(CreateCardJoint);

			LerpTargetAnimator animator = card.gameObject.AddComponent<LerpTargetAnimator>();
			animator.curve = diajector.curve;

			animators.Add(animator, target);
			cardTargets.Add(card, target);
			
			diajector.deque.definition.UpdateCardTexture(card);

			return animator;
		}

		public void RemoveCard(DequeStorable card)
		{
			card.isStored = false;

			card.grabbable.onGrab.RemoveListener(DestroyCardJoint);
			card.grabbable.onDrop.RemoveListener(CreateCardJoint);

			LerpTargetAnimator anim = card.GetComponent<LerpTargetAnimator>();
			Destroy(anim);

			animators.Remove(anim);
			cardTargets.Remove(card);
		}

		private void CreateCardJoint(Grabber grabber, Grabbable grabbable)
		{
			DequeStorable card = grabbable.GetComponent<DequeStorable>();
			CreateCardJoint(card, cardTargets[card], diajector.staticRigidbody, diajector.cardStrength);
		}

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

		public void DestroyCardJoint(CardTarget target) => DestroyCardJoint(target.card);
		
		private void DestroyCardJoint(DequeStorable card)
		{
			JointTargetter targetter = cardTargets[card].targetter;
			cardTargets[card].targetter = null;
			Destroy(targetter);
		}

		public bool HasAnimator(LerpTargetAnimator animator) => animators.ContainsKey(animator);
		public LerpTarget GetLerpTargetForAnimator(LerpTargetAnimator animator) => animators[animator].lerpTarget;

		public void Refresh()
		{
			onPreparePage.Invoke();
		}
		
		public void StartAssembly()
		{
			gameObject.SetActive(true);
			onPreparePage.Invoke();
			diajector.coroutineOwner.StartCoroutine(SpawnCards());
		}

		private IEnumerator SpawnCards()
		{
			foreach ((LerpTargetAnimator animator, CardTarget target) in animators)
			{
				target.onPrepareCard.Invoke();
				animator.TargetTo(diajector.deque.upperTarget);
				yield return new WaitForSeconds(diajector.cardDelay);
			}
		}

		public void StartDisassembly()
		{
			diajector.coroutineOwner.StartCoroutine(DespawnCards());
			gameObject.SetActive(false);
		}

		private IEnumerator DespawnCards()
		{
			foreach ((LerpTargetAnimator animator, CardTarget target) in animators)
			{
				animator.TargetTo(diajector.upperTarget);
				yield return new WaitForSeconds(diajector.cardDelay);
			}
		}

		public void ForceClose(LerpTarget bottomTarget)
		{
			foreach ((LerpTargetAnimator animator, CardTarget target) in animators)
				animator.TeleportTo(bottomTarget);
			gameObject.SetActive(false);
		}
	}
}
