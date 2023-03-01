using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	[CreateAssetMenu(menuName="Deque/"+nameof(QueueDeque))]
	public class QueueDeque : DequeType
	{
		public float separatingDistance = 0.1f;
		public Quaternion cardRotation = Quaternion.identity;
		
		public override void Tick(List<DequeStorable> cards, float delta) { }
		
		public override void LayoutTargets(List<DequeStorable> cards, Dictionary<DequeStorable, CardTarget> targets)
		{
			Vector3 right = separatingDistance * (targets.Count - 1) / 2 * Vector3.left;
			foreach ((DequeStorable card, CardTarget target) in InOrder(cards, targets))
			{
				target.transform.localPosition = right;
				target.transform.localRotation = cardRotation;
				right += Vector3.right * separatingDistance;
			}
		}
		
		public override bool CanFetch(List<DequeStorable> cards, DequeStorable card) => cards[^1] == card;
		
		public override int GetIndexToStoreInto(List<DequeStorable> cards) => GetFirstIndexWhere(cards, HasEmptyContainer, OrLastCard);
		
		public override int GetIndexToInsertCardBetween(List<DequeStorable> cards, DequeStorable card) => GetFirstIndexWhere(cards, DoesntHaveEmptyContainer, OrBeforeAllCards);
	}
}
