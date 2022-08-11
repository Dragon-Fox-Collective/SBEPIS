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
		private Transform[] cards;
		private TransformLerper[] cardLerpers;

		public void StartAssembly(CaptureDeque deque, Diajector diajector, Rigidbody staticRigidbody, StrengthSettings cardStrength)
		{
			cardTargets = GetComponentsInChildren<CardTarget>();
			cardLerpers = cardTargets.Select(target =>
			{
				GameObject card = Instantiate(diajector.cardPrefab);
				card.SetActive(false);

				TransformLerper lerper = card.AddComponent<TransformLerper>();
				lerper.target = deque.cardTarget;
				lerper.timeToComplete = diajector.pageTime;
				
				TransformLerper finalLerper = lerper.Chain(diajector.upperTarget).Chain(target.transform);
				finalLerper.onEnd.AddListener(() =>
				{
					Vector3 position = transform.position;
					Quaternion rotation = transform.rotation;
					CreateCardJoint(card, target.transform, staticRigidbody, cardStrength);
					Grabbable grabbable = card.GetComponent<Grabbable>();
					grabbable.onGrab.AddListener((grabber, grabbable) => DestroyCardJoint(card));
					grabbable.onDrop.AddListener((grabber, grabbable) => CreateCardJoint(card, target.transform, staticRigidbody, cardStrength));
				});

				return lerper;
			}).ToArray();
			cards = cardLerpers.Select(lerper => lerper.transform).ToArray();

			StartCoroutine(SpawnCards(deque.cardStart, diajector.cardDelay));
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

		private IEnumerator SpawnCards(Transform start, float cardDelay)
		{
			foreach (TransformLerper card in cardLerpers)
			{
				card.gameObject.SetActive(true);
				card.StartTargetting(start);
				yield return new WaitForSeconds(cardDelay);
			}
		}

		public void StartDisassembly(CaptureDeque deque, Diajector diajector)
		{
			foreach (Transform card in cards)
				Destroy(card.gameObject);
		}
	}
}
