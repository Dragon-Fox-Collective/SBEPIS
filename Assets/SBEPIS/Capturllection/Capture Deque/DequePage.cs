using SBEPIS.Controller;
using SBEPIS.Physics;
using SBEPIS.Items;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public class DequePage : MonoBehaviour
	{
		public List<Transform> baseTargets = new();
		public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 3, 3);
		public float cardDelay = 0.5f;
		public Rigidbody staticRigidbody;
		public StrengthSettings cardStrength;
		public UnityEvent onPreparePage = new();

		private readonly List<ProceduralAnimation> animations = new();
		private readonly Dictionary<DequeStorable, CardTarget> cardTargets = new();

		public bool cardsCreated { get; private set; }

		public void CreateCards(CaptureDeque deque, DequeStorable cardPrefab)
		{
			if (cardsCreated)
				return;

			cardsCreated = true;

			foreach (CardTarget target in GetComponentsInChildren<CardTarget>())
			{
				DequeStorable card = Instantiate(cardPrefab.gameObject).GetComponent<DequeStorable>();
				target.card = card;
				AddCard(card, target);

				Capturllectainer container = card.GetComponent<Capturllectainer>();
				container.isRetrievingAllowed = false;

				Capturllectable capturllectable = card.GetComponent<Capturllectable>();
				capturllectable.canCapturllect = false;

				Grabbable grabbable = card.grabbable;
				grabbable.onGrab.AddListener((grabber, grabbable) => target.onGrab.Invoke(deque));
				grabbable.onDrop.AddListener((grabber, grabbable) => target.onDrop.Invoke(deque));

				target.onCardCreated.Invoke(card);
			}
		}

		public ProceduralAnimation AddCard(DequeStorable card, CardTarget target)
		{
			card.isStored = true;

			card.grabbable.onGrab.AddListener(DestroyCardJoint);
			card.grabbable.onDrop.AddListener(CreateCardJoint);

			ProceduralAnimation animation = card.gameObject.AddComponent<ProceduralAnimation>();
			animation.targets.AddRange(baseTargets);
			animation.targets.Add(target.transform);
			animation.curve = curve;
			animation.onPlay.AddListener(() =>
			{
				card.gameObject.SetActive(true);
			});
			animation.onEnd.AddListener(() =>
			{
				CreateCardJoint(card, target, staticRigidbody, cardStrength);
			});
			animation.onReversePlay.AddListener(() =>
			{
				DestroyCardJoint(card);
			});
			animation.onReverseEnd.AddListener(() =>
			{
				card.gameObject.SetActive(false);
			});

			animations.Add(animation);
			cardTargets.Add(card, target);

			return animation;
		}

		public void RemoveCard(DequeStorable card)
		{
			card.isStored = false;

			card.grabbable.onGrab.RemoveListener(DestroyCardJoint);
			card.grabbable.onDrop.RemoveListener(CreateCardJoint);

			ProceduralAnimation animation = card.GetComponent<ProceduralAnimation>();
			Destroy(animation);

			animations.Remove(animation);
			cardTargets.Remove(card);
		}

		private void CreateCardJoint(Grabber grabber, Grabbable grabbable)
		{
			DequeStorable card = grabbable.GetComponent<DequeStorable>();
			CreateCardJoint(card, cardTargets[card], staticRigidbody, cardStrength);
		}

		private void CreateCardJoint(DequeStorable card, CardTarget target, Rigidbody staticRigidbody, StrengthSettings cardStrength)
		{
			JointTargetter targetter = card.gameObject.AddComponent<JointTargetter>();
			targetter.connectedRigidbody = staticRigidbody;
			targetter.target = target.transform;
			targetter.strength = cardStrength;
			targetter.accountForTargetMovement = false;
			target.targetter = targetter;
		}

		private void DestroyCardJoint(Grabber grabber, Grabbable grabbable) => DestroyCardJoint(grabbable.GetComponent<DequeStorable>());

		private void DestroyCardJoint(DequeStorable card)
		{
			JointTargetter targetter = cardTargets[card].targetter;
			cardTargets[card].targetter = null;
			Destroy(targetter);
		}

		public void StartAssembly(MonoBehaviour coroutineOwner)
		{
			onPreparePage.Invoke();
			coroutineOwner.StartCoroutine(SpawnCards());
		}

		private IEnumerator SpawnCards()
		{
			foreach ((ProceduralAnimation animation, CardTarget target) in animations.Zip(cardTargets.Values))
			{
				target.onPrepareCard.Invoke();
				animation.PlayForward();
				yield return new WaitForSeconds(cardDelay);
			}
		}

		public void StartDisassembly(MonoBehaviour coroutineOwner)
		{
			coroutineOwner.StartCoroutine(DespawnCards());
		}

		private IEnumerator DespawnCards()
		{
			foreach (ProceduralAnimation animation in animations)
			{
				animation.PlayReverse();
				yield return new WaitForSeconds(cardDelay);
			}
		}

		public void ForceClose()
		{
			foreach (ProceduralAnimation card in animations)
			{
				if (card.time == card.endTime)
					card.onReversePlay.Invoke();
				if (card.time != card.startTime)
					card.onReverseEnd.Invoke();
				card.Stop();
			}
		}
	}
}
