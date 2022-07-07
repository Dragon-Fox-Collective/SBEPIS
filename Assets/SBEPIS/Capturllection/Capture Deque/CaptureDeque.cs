using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SBEPIS.Interaction;

namespace SBEPIS.Capturllection
{
	public class CaptureDeque : MonoBehaviour
	{
		public BoxCollider mainBox;

		public List<DequeStorable> cards = new();
		public DequeStorable currentCard;

		private void Update()
		{
			SetCardTargets();
		}

		public void SetCardTargets()
		{
			IEnumerable<DequeStorable> cards = this.cards;
			if (currentCard)
				cards = cards.Insert(GetIndexOfUnstoredCard(currentCard), currentCard);

			// Done mostly in local space because easier lol
			Vector3 boxTopPosition = mainBox.center + mainBox.size.y / 2 * Vector3.up;
			float totalStoredWidth = cards.Sum(card => card.boxCollider.size.x);
			float storedCardLeft = -totalStoredWidth / 2;
			foreach (DequeStorable storedCard in cards)
			{
				storedCard.lerper.SetTargets(boxTopPosition + (storedCardLeft + storedCard.boxCollider.size.x / 2) * Vector3.right - storedCard.boxCollider.center, Quaternion.identity); // currentCard's lerper should still be disabled, so this shouldn't affect it
				storedCardLeft += storedCard.boxCollider.size.x;
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody)
				TargetNewCard(other.GetComponent<DequeStorable>());
		}

		private void OnTriggerExit(Collider other)
		{
			if (other.attachedRigidbody)
				StopTargettingCard(other.GetComponent<DequeStorable>());
		}

		public void TargetNewCard(DequeStorable card)
		{
			if (!card)
				return;

			if (currentCard)
				StopTargettingCard();

			currentCard = card;
			card.GetComponent<Grabbable>()?.onDrop.AddListener(AcceptCard);
		}

		public void StopTargettingCard()
		{
			StopTargettingCard(currentCard);
		}

		public void StopTargettingCard(DequeStorable card)
		{
			if (!currentCard || currentCard != card)
				return;

			currentCard = null;
			card.GetComponent<Grabbable>()?.onDrop.RemoveListener(AcceptCard);
		}

		public void AcceptCard(Grabber grabber, Grabbable grabbable) => AcceptCard(grabbable.GetComponent<DequeStorable>());

		public void AcceptCard(DequeStorable card)
		{
			if (!card || cards.Contains(card))
				return;

			StopTargettingCard(card);

			cards.Insert(GetIndexOfUnstoredCard(card), card);
			card.lerper.EnableLerp();
		}

		public int GetIndexOfUnstoredCard(DequeStorable card)
		{
			Vector3 boxTopPosition = mainBox.center + mainBox.size.y / 2 * Vector3.up;
			Vector3 closestPoint = mainBox.transform.InverseTransformPoint(card.boxCollider.ClosestPoint(mainBox.transform.TransformPoint(boxTopPosition)));
			float pointInBox = closestPoint.x;
			float totalStoredWidth = cards.Sum(card => card.boxCollider.size.x) + card.boxCollider.size.x;
			float storedCardLeft = -totalStoredWidth / 2;
			int index = 0;
			foreach (DequeStorable storedCard in cards)
			{
				if (pointInBox < storedCardLeft + storedCard.boxCollider.size.x / 2)
					return index;
				storedCardLeft += storedCard.boxCollider.size.x;
				index++;
			}
			return index;
		}

		public void RejectStoredCard(DequeStorable card)
		{
			if (!card || !cards.Contains(card))
				return;

			cards.Remove(card);
			card.lerper.DisableLerp();
		}
	}
}
