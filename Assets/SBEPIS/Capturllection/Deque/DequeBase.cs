using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class DequeBase : Deque
	{
		public Dequeration dequeration;
		
		public override IEnumerable<Texture2D> GetCardTextures()
		{
			yield return dequeration.cardTexture;
		}
		
		public override IEnumerable<Texture2D> GetBoxTextures()
		{
			yield return dequeration.boxTexture;
		}
		
		
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
		
		public static IEnumerable<(DequeStorable, CardTarget)> InOrder(List<DequeStorable> cards, Dictionary<DequeStorable, CardTarget> targets) => cards.Zip(cards.Select(card => targets[card]));
	}
}
