using SBEPIS.Controller;
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
		public UnityEvent onAssembled = new(), onDisassembled = new();

		private readonly List<ProceduralAnimation> cards = new();
		private Coroutine currentCoroutine;

		public void CreateCards(DequeStorable cardPrefab)
		{
			foreach (CardTarget target in GetComponentsInChildren<CardTarget>())
				AddCard(Instantiate(cardPrefab.gameObject).GetComponent<DequeStorable>(), target);
		}

		public ProceduralAnimation AddCard(DequeStorable card, CardTarget target)
		{
			card.isStored = true;

			card.grabbable.onGrab.AddListener((grabber, grabbable) => DestroyCardJoint(card));
			card.grabbable.onDrop.AddListener((grabber, grabbable) => CreateCardJoint(card, target.transform, staticRigidbody, cardStrength));

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
				CreateCardJoint(card, target.transform, staticRigidbody, cardStrength);
			});
			animation.onReversePlay.AddListener(() =>
			{
				DestroyCardJoint(card);
			});
			animation.onReverseEnd.AddListener(() =>
			{
				card.gameObject.SetActive(false);
			});

			cards.Add(animation);

			return animation;
		}

		private void CreateCardJoint(DequeStorable card, Transform target, Rigidbody staticRigidbody, StrengthSettings cardStrength)
		{
			JointTargetter targetter = card.gameObject.AddComponent<JointTargetter>();
			targetter.connectedRigidbody = staticRigidbody;
			targetter.target = target;
			targetter.strength = cardStrength;
			targetter.accountForTargetMovement = false;
		}

		private void DestroyCardJoint(DequeStorable card)
		{
			JointTargetter targetter = card.GetComponent<JointTargetter>();
			Destroy(targetter);
		}

		public void StartAssembly(MonoBehaviour coroutineOwner)
		{
			coroutineOwner.StartCoroutine(SpawnCards());
		}

		private IEnumerator SpawnCards()
		{
			foreach (ProceduralAnimation card in cards)
			{
				card.PlayForward();
				yield return new WaitForSeconds(cardDelay);
			}
		}

		public void StartDisassembly(MonoBehaviour coroutineOwner)
		{
			coroutineOwner.StartCoroutine(DespawnCards());
		}

		private IEnumerator DespawnCards()
		{
			foreach (ProceduralAnimation card in cards)
			{
				card.PlayReverse();
				yield return new WaitForSeconds(cardDelay);
			}
		}

		public void ForceClose()
		{
			foreach (ProceduralAnimation card in cards)
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
