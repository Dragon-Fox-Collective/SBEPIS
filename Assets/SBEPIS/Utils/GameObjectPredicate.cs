using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Predicates
{
	public class GameObjectPredicate : MonoBehaviour
	{
		public virtual bool IsTrue(GameObject obj) => true;
	}

	public static class PredicateExtensionMethods
	{
		public static bool AreTrue(this IEnumerable<GameObjectPredicate> list, GameObject obj) => list.All(predicate => predicate.IsTrue(obj));
	}
}
