using SBEPIS.Controller;
using SBEPIS.Thaumaturgy;
using System;
using System.Collections.Generic;
using System.Linq;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabbable), typeof(SplitTextureSetup))]
	public class DequeStorable : MonoBehaviour
	{
		public bool isStoringAllowed = true;
		public readonly List<Func<bool>> storePredicates = new();
		
		public Grabbable grabbable { get; private set; }
		public SplitTextureSetup split { get; private set; }
		
		public bool isStored { get; set; }
		public bool canStore => storePredicates.All(predicate => predicate.Invoke());

		private void Awake()
		{
			grabbable = GetComponent<Grabbable>();
			split = GetComponent<SplitTextureSetup>();
			
			storePredicates.Add(() => isStoringAllowed);

			// This sucks but it's the best place to put it for now :/
			Capturellectainer container = GetComponent<Capturellectainer>();
			if (container)
				storePredicates.Add(() => container.capturedItem);

			Punchable punchable = GetComponent<Punchable>();
			if (punchable)
				storePredicates.Add(() => punchable.punchedBits.isPerfectlyGeneric);
		}
	}
}