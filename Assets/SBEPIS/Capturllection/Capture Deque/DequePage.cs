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
		public UnityEvent onAssembled = new(), onDisassembled = new();

		private CardTarget[] cardTargets;
		private GameObject[] cards;
		private ProceduralAnimation[] cardAnimations;

		public void CreateCards(GameObject cardPrefab, IEnumerable<Transform> targets, AnimationCurve curve, StrengthSettings cardStrength, Rigidbody staticRigidbody)
		{
			cardTargets = GetComponentsInChildren<CardTarget>();

			cards = cardTargets.Select(target =>
			{
				GameObject card = Instantiate(cardPrefab);

				Grabbable grabbable = card.GetComponent<Grabbable>();
				grabbable.onGrab.AddListener((grabber, grabbable) => DestroyCardJoint(card));
				grabbable.onDrop.AddListener((grabber, grabbable) => CreateCardJoint(card, target.transform, staticRigidbody, cardStrength));

				return card;
			}).ToArray();

			cardAnimations = cards.Zip(cardTargets, (card, target) =>
			{
				ProceduralAnimation animation = card.AddComponent<ProceduralAnimation>();
				animation.targets.AddRange(targets);
				animation.targets.Add(target.transform);
				animation.curve = curve;
				animation.onPlay.AddListener(() =>
				{
					card.SetActive(true);
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
					card.SetActive(false);
				});
				return animation;
			}).ToArray();
		}

		private void CreateCardJoint(GameObject card, Transform target, Rigidbody staticRigidbody, StrengthSettings cardStrength)
		{
			JointTargetter targetter = card.AddComponent<JointTargetter>();
			targetter.connectedRigidbody = staticRigidbody;
			targetter.target = target;
			targetter.strength = cardStrength;
		}

		private void DestroyCardJoint(GameObject card)
		{
			JointTargetter targetter = card.GetComponent<JointTargetter>();
			Destroy(targetter);
		}

		public void StartAssembly(MonoBehaviour coroutineOwner, float cardDelay)
		{
			coroutineOwner.StartCoroutine(SpawnCards(cardDelay));
		}

		private IEnumerator SpawnCards(float cardDelay)
		{
			print($"Starting to spawn {cardAnimations.ToDelimString()} {cardDelay}");
			foreach (ProceduralAnimation cardAnimation in cardAnimations)
			{
				print($"Spawning {cardAnimation}");
				cardAnimation.PlayForward();
				yield return new WaitForSeconds(cardDelay);
			}
		}

		public void StartDisassembly(MonoBehaviour coroutineOwner, float cardDelay)
		{
			coroutineOwner.StartCoroutine(DespawnCards(cardDelay));
		}

		private IEnumerator DespawnCards(float cardDelay)
		{
			print($"Starting to despawn {cardAnimations.ToDelimString()} {cardDelay}");
			foreach (ProceduralAnimation cardAnimation in cardAnimations)
			{
				print($"Despawning {cardAnimation}");
				cardAnimation.PlayReverse();
				print($"Despawninged {cardAnimation}");
				yield return new WaitForSeconds(cardDelay);
				print($"Despawningeded {cardAnimation}");
			}
			print($"Despawningededed {cardAnimations.ToDelimString()}");
		}
	}
}
