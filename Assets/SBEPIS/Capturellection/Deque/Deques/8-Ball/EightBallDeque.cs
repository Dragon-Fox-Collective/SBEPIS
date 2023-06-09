using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SBEPIS.Capturellection.Storage;
using UnityEngine;

namespace SBEPIS.Capturellection.Deques
{
	public class EightBallDeque : LaidOutDeque<LinearSettings, LinearLayout, EightBallState>
	{
		[SerializeField] private GameObject menuEightBallPrefab;
		[SerializeField] private GameObject fetchedEightBallPrefab;
		
		public override UniTask<FetchResult> FetchItemHook(EightBallState state, InventoryStorable card, FetchResult oldResult)
		{
			EightBallFetched eightBall = Instantiate(fetchedEightBallPrefab).GetComponentInChildren<EightBallFetched>();
			eightBall.Container.Capture(oldResult.fetchedItem);
			oldResult.fetchedItem = eightBall.Capturellectable;
			return UniTask.FromResult(oldResult);
		}
		
		public override IEnumerable<InventoryStorable> LoadCardHook(EightBallState state, InventoryStorable card)
		{
			EightBall eightBall = Instantiate(menuEightBallPrefab).GetComponentInChildren<EightBall>();
			eightBall.Card.DequeElement.SetParent(card.DequeElement.Parent);
			eightBall.Card.Inventory = card.Inventory;
			eightBall.OriginalCard = card;
			state.eightBalls[card] = eightBall;
			yield return eightBall.Card;
		}
		
		public override IEnumerable<InventoryStorable> SaveCardHook(EightBallState state, InventoryStorable slot)
		{
			state.eightBalls.Remove(slot, out EightBall eightBall);
			InventoryStorable card = eightBall.OriginalCard;
			Destroy(eightBall.Root.gameObject);
			yield return card;
		}
	}
	
	public class EightBallState : InventoryState, DirectionState
	{
		public CallbackList<Storable> Inventory { get; set; } = new();
		public Vector3 Direction { get; set; }
		public readonly Dictionary<InventoryStorable, EightBall> eightBalls = new();
	}
}
