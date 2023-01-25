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
		private Diajector diajector;
		private List<Func<Transform>> baseTargetProviders = new();
		private UnityEvent onPreparePage = new();

		private readonly List<ProceduralAnimation> animations = new();
		private readonly Dictionary<DequeStorable, CardTarget> cardTargets = new();

		private void Awake()
		{
			diajector = GetComponentInParent<Diajector>();
			baseTargetProviders.AddRange(diajector.targetProviders);
			CreateCards();
		}
		
		private void CreateCards()
		{
			foreach (CardTarget target in GetComponentsInChildren<CardTarget>())
			{
				DequeStorable card = Instantiate(diajector.cardPrefab.gameObject).GetComponent<DequeStorable>();
				card.name += $" ({target.label})";
				target.card = card;
				AddCard(card, target);

				Capturllectainer container = card.GetComponent<Capturllectainer>();
				container.isRetrievingAllowed = false;

				Capturllectable capturllectable = card.GetComponent<Capturllectable>();
				capturllectable.canCapturllect = false;

				Grabbable grabbable = card.grabbable;
				grabbable.onGrab.AddListener((grabber, grabbable) => target.onGrab.Invoke());
				grabbable.onDrop.AddListener((grabber, grabbable) => target.onDrop.Invoke());

				target.onCardCreated.Invoke(card);
			}
		}

		public ProceduralAnimation AddCard(DequeStorable card, CardTarget target)
		{
			card.isStored = true;

			card.grabbable.onGrab.AddListener(DestroyCardJoint);
			card.grabbable.onDrop.AddListener(CreateCardJoint);

			ProceduralAnimation anim = card.gameObject.AddComponent<ProceduralAnimation>();
			anim.targetProviders.AddRange(baseTargetProviders);
			anim.targetProviders.Add(() => target.transform);
			anim.curve = diajector.curve;
			anim.onPlay.AddListener(() =>
			{
				card.gameObject.SetActive(true);
			});
			anim.onEnd.AddListener(() =>
			{
				CreateCardJoint(card, target, diajector.staticRigidbody, diajector.cardStrength);
			});
			anim.onReversePlay.AddListener(() =>
			{
				DestroyCardJoint(card);
			});
			anim.onReverseEnd.AddListener(() =>
			{
				card.gameObject.SetActive(false);
			});

			animations.Add(anim);
			cardTargets.Add(card, target);

			return anim;
		}

		public void RemoveCard(DequeStorable card)
		{
			card.isStored = false;

			card.grabbable.onGrab.RemoveListener(DestroyCardJoint);
			card.grabbable.onDrop.RemoveListener(CreateCardJoint);

			ProceduralAnimation anim = card.GetComponent<ProceduralAnimation>();
			Destroy(anim);

			animations.Remove(anim);
			cardTargets.Remove(card);
		}

		private void CreateCardJoint(Grabber grabber, Grabbable grabbable)
		{
			DequeStorable card = grabbable.GetComponent<DequeStorable>();
			CreateCardJoint(card, cardTargets[card], diajector.staticRigidbody, diajector.cardStrength);
		}

		private static void CreateCardJoint(DequeStorable card, CardTarget target, Rigidbody staticRigidbody, StrengthSettings cardStrength)
		{
			JointTargetter targetter = staticRigidbody.gameObject.AddComponent<JointTargetter>();
			targetter.connectedBody = card.grabbable.rigidbody;
			targetter.target = target.transform;
			targetter.strength = cardStrength;
			target.targetter = targetter;
		}

		private void DestroyCardJoint(Grabber grabber, Grabbable grabbable) => DestroyCardJoint(grabbable.GetComponent<DequeStorable>());

		private void DestroyCardJoint(DequeStorable card)
		{
			JointTargetter targetter = cardTargets[card].targetter;
			cardTargets[card].targetter = null;
			Destroy(targetter);
		}

		public void StartAssembly()
		{
			onPreparePage.Invoke();
			diajector.coroutineOwner.StartCoroutine(SpawnCards());
		}

		private IEnumerator SpawnCards()
		{
			foreach ((ProceduralAnimation anim, CardTarget target) in animations.Zip(cardTargets.Values))
			{
				target.onPrepareCard.Invoke();
				anim.PlayForward();
				yield return new WaitForSeconds(diajector.cardDelay);
			}
		}

		public void StartDisassembly()
		{
			diajector.coroutineOwner.StartCoroutine(DespawnCards());
		}

		private IEnumerator DespawnCards()
		{
			foreach (ProceduralAnimation anim in animations)
			{
				anim.PlayReverse();
				yield return new WaitForSeconds(diajector.cardDelay);
			}
		}

		public void ForceClose()
		{
			foreach (ProceduralAnimation card in animations)
			{
				if (card.time >= card.endTime)
					card.onReversePlay.Invoke();
				if (card.time > card.startTime)
					card.onReverseEnd.Invoke();
				card.Stop();
			}
		}
	}
}
