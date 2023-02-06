using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Predicates
{
	public abstract class GameObjectPredicate : MonoBehaviour
	{
		public abstract bool IsTrue(GameObject obj);
	}

	public static class PredicateExtensionMethods
	{
		public static bool AreTrue(this IEnumerable<GameObjectPredicate> list, GameObject obj) => list.All(predicate => predicate.IsTrue(obj));
	}
}
