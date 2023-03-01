using SBEPIS.Capturllection;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Predicates
{
	public class IsOwnedDeque : GameObjectPredicate
	{
		[FormerlySerializedAs("dequeOwner")]
		public DequeOwner owner;
		
		public override bool IsTrue(GameObject obj) => obj == owner.dequeBox.gameObject;
	}
}
