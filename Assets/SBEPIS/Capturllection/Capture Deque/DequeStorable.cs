using SBEPIS.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabbable))]
	public class DequeStorable : MonoBehaviour
	{
		public bool isStoringAllowed = true;

		public bool isStored { get; set; }
		public Grabbable grabbable { get; private set; }
		public readonly List<Func<bool>> storePredicates = new();
		public bool canStore => storePredicates.All(predicate => predicate.Invoke());

		private void Awake()
		{
			grabbable = GetComponent<Grabbable>();
			storePredicates.Add(() => isStoringAllowed);
		}
	}
}
