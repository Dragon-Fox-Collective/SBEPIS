using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(DequeBox))]
	public class DequeBoxOwnerBinder : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private DequeBox dequeBox;
		
		public void BindToPlayer(PlayerReference playerReference)
		{
			playerReference.GetReferencedComponent<DequeBoxOwner>().DequeBox = dequeBox;
		}
	}
}