using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class DequeBase : DequeRuleset
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
		
		public static int OrBeforeAllCards<T>(List<T> items) => 0;
		public static int OrAfterAllCards<T>(List<T> items) => items.Count;
		public static int OrFirstCard<T>(List<T> items) => 0;
		public static int OrLastCard<T>(List<T> items) => items.Count - 1;
		
		public static int GetFirstIndexWhere<T>(List<T> items, Predicate<T> predicate, Func<List<T>, int> defaultIndexFunc)
		{
			int index = items.FindIndex(predicate);
			return index != -1 ? index : defaultIndexFunc.Invoke(items);
		}
		
		public static IEnumerable<(DequeStorable, CardTarget)> InOrder(List<DequeStorable> cards, Dictionary<DequeStorable, CardTarget> targets) => cards.Zip(cards.Select(card => targets[card]));
	}
}
