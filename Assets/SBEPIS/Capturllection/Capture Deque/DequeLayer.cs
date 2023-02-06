using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[Serializable]
	public class DequeLayer
	{
		public List<DequeType> deques;

		private List<Texture2D> cardTextures;

		public void TickDeque(List<CardTarget> targets, float delta)
		{
			deques.Do(deque => deque.TickDeque(targets, delta));
		}

		public void LayoutTargets(List<CardTarget> targets)
		{
			deques[0].LayoutTargets(targets);
		}
		
		public bool CanFetch(List<CardTarget> targets, CardTarget card)
		{
			return deques.AsEnumerable().Reverse().Any(deque => deque.CanRetrieve(targets, card));
		}

		public void UpdateCardTexture(DequeStorable card)
		{
			cardTextures ??= deques.Select(deque => deque.cardTexture).ToList();
			card.split.UpdateTexture(cardTextures);
		}
	}
}
