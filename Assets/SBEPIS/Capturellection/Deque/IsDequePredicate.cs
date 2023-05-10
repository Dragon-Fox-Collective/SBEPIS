using SBEPIS.Predicates;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class IsDequePredicate : GameObjectPredicate
	{
		public override bool IsTrue(GameObject obj) => obj.GetComponent<Deque>();
	}
}
