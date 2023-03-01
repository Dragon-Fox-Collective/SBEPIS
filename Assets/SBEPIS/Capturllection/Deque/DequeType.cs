using System;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class DequeType : ScriptableObject
	{
		public Texture2D cardTexture;
		public Texture2D dequeTexture;
		
		public abstract void Tick(List<DequeStorable> cards, float delta);
		public abstract void LayoutTargets(List<DequeStorable> cards, Dictionary<DequeStorable, CardTarget> targets);
		
		public abstract bool CanFetch(List<DequeStorable> cards, DequeStorable card);
		public abstract int GetIndexToStoreInto(List<DequeStorable> cards);
		public abstract int GetIndexToInsertCardBetween(List<DequeStorable> cards, DequeStorable card);

		public static bool HasEmptyContainer(DequeStorable card) => card.TryGetComponent(out Capturellectainer container) && !container.hasCapturedItem;
		public static bool DoesntHaveEmptyContainer(DequeStorable card) => !HasEmptyContainer(card);
		
		public static int OrBeforeAllCards(List<DequeStorable> cards) => 0;
		public static int OrAfterAllCards(List<DequeStorable> cards) => cards.Count;
		public static int OrFirstCard(List<DequeStorable> cards) => 0;
		public static int OrLastCard(List<DequeStorable> cards) => cards.Count - 1;
		
		public static int GetFirstIndexWhere(List<DequeStorable> cards, Predicate<DequeStorable> predicate, Func<List<DequeStorable>, int> defaultIndexFunc)
		{
			int index = cards.FindIndex(predicate);
			return index != -1 ? index : defaultIndexFunc.Invoke(cards);
		}
	}
}
