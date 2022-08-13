using SBEPIS.Controller;
using SBEPIS.Thaumaturgy;
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
		public List<Func<bool>> storePredicates = new();

		public bool isStored { get; set; }
		public Grabbable grabbable { get; private set; }
		public bool canStore => storePredicates.All(predicate => predicate.Invoke());

		private void Awake()
		{
			grabbable = GetComponent<Grabbable>();
			storePredicates.Add(() => isStoringAllowed);

			// This sucks but it's the best place to put it for now :/
			Capturllectainer container = GetComponent<Capturllectainer>();
			if (container)
				storePredicates.Add(() => container.capturedItem);

			Punchable punchable = GetComponent<Punchable>();
			if (punchable)
				storePredicates.Add(() => punchable.punchedBits == Bits.BitSet.NOTHING);
		}
	}
}
