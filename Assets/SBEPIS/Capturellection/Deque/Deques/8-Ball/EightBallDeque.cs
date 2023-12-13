using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class EightBallDeque : LaidOutRuleset<LinearSettings, LinearLayout, EightBallState>
	{
		[SerializeField] private GameObject menuEightBallPrefab;
		[SerializeField] private GameObject fetchedEightBallPrefab;
		
		protected override UniTask<FetchResult> FetchItemHook(EightBallState state, InventoryStorable card, FetchResult oldResult)
		{
			EightBallFetched eightBall = Instantiate(fetchedEightBallPrefab).GetComponentInChildren<EightBallFetched>();
			eightBall.Container.Capture(oldResult.FetchedItem);
			oldResult.FetchedItem = eightBall.Capturellectable;
			return UniTask.FromResult(oldResult);
		}
		
		protected override IEnumerable<InventoryStorable> LoadCardHook(EightBallState state, InventoryStorable card)
		{
			EightBall eightBall = Instantiate(menuEightBallPrefab).GetComponentInChildren<EightBall>();
			eightBall.Card.DequeElement.SetParent(card.DequeElement.Parent);
			eightBall.Card.Inventory = card.Inventory;
			eightBall.OriginalCard = card;
			state.EightBalls[card] = eightBall;
			yield return eightBall.Card;
		}
		
		protected override IEnumerable<InventoryStorable> SaveCardHook(EightBallState state, InventoryStorable slot)
		{
			state.EightBalls.Remove(slot, out EightBall eightBall);
			InventoryStorable card = eightBall.OriginalCard;
			Destroy(eightBall.Root.gameObject);
			yield return card;
		}
	}
	
	public class EightBallState : InventoryState, DirectionState
	{
		public CallbackList<Storable> Inventory { get; set; } = new();
		public Vector3 Direction { get; set; }
		public readonly Dictionary<InventoryStorable, EightBall> EightBalls = new();
	}
}
