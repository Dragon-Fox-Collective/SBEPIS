using SBEPIS.Capturllection;
using UnityEngine;

namespace SBEPIS.Predicates
{
	public class IsOwnedDeque : GameObjectPredicate
	{
		public DequeOwner dequeOwner;
		
		public override bool IsTrue(GameObject obj) => obj == dequeOwner.dequeBox.gameObject;
	}
}
