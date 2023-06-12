using Cysharp.Threading.Tasks;
using KBCore.Refs;
using SBEPIS.Controller;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(DequeBox))]
	public class DequeBoxOwnerBinder : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private DequeBox dequeBox;
		
		public void BindToPlayer(PlayerReference playerReference)
		{
			DequeBoxOwner dequeBoxOwner = playerReference.GetReferencedComponent<DequeBoxOwner>();
			dequeBoxOwner.DequeBox = dequeBox;
			Retrieve(dequeBoxOwner).Forget();
		}
		
		private async UniTask Retrieve(DequeBoxOwner dequeBoxOwner)
		{
			await UniTask.NextFrame();
			if (!dequeBox) return;
			if (!(dequeBox.TryGetComponent(out Grabbable grabbable) && grabbable.IsBeingHeld))
				dequeBox.Retrieve(dequeBoxOwner);
		}
	}
}