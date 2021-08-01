using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WrightWay.SBEPIS.Modus
{
	public abstract class FetchModus
	{
		protected abstract int DepositIndex(List<Item> cards);

		protected abstract int EjectIndex(List<Item> cards);

		protected abstract int RetrieveIndex(List<Item> cards);

		/// <returns>An item to eject</returns>
		public virtual Item Deposit(Item item, List<Item> cards)
		{
			int ejectIndex = EjectIndex(cards);
			Item rtn = cards[ejectIndex];
			cards.RemoveAt(ejectIndex);

			cards.Insert(DepositIndex(cards), item);

			return rtn;
		}

		public virtual Item Display(List<Item> cards)
		{
			return cards[RetrieveIndex(cards)];
		}

		public virtual Item Retrieve(List<Item> cards, bool asCard)
		{
			int retrieveIndex = RetrieveIndex(cards);
			Item rtn = cards[retrieveIndex];
			cards.RemoveAt(retrieveIndex);

			if (!asCard)
				cards.Insert(EjectIndex(cards), null);

			return rtn;
		}

		public virtual void Clean(List<Item> cards, bool asCard) { }
	}

	public class StackModus : FetchModus
	{
		protected override int DepositIndex(List<Item> cards)
		{
			return 0;
		}

		protected override int EjectIndex(List<Item> cards)
		{
			return cards.Count - 1;
		}

		protected override int RetrieveIndex(List<Item> cards)
		{
			return 0;
		}

		public override void Clean(List<Item> cards, bool asCard)
		{
			int insertIndex = 0;
			for (int i = 0; i < cards.Count; i++)
				if (cards[i])
				{
					cards.Insert(insertIndex++, cards[i]);
					cards.RemoveAt(i + 1);
				}
		}
	}

	public class QueueModus : FetchModus
	{
		protected override int DepositIndex(List<Item> cards)
		{
			return cards.Count - 1;
		}

		protected override int EjectIndex(List<Item> cards)
		{
			return 0;
		}

		protected override int RetrieveIndex(List<Item> cards)
		{
			return cards.FindIndex(item => item != null);
		}

		public override void Clean(List<Item> cards, bool asCard)
		{
			int insertIndex = cards.Count - 1;
			for (int i = cards.Count - 1; i >= 0; i--)
				if (cards[i])
				{
					cards.Insert(insertIndex--, cards[i]);
					cards.RemoveAt(i);
				}
		}
	}
}