using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	[Serializable]
	public class DequeLayer
	{
		public List<DequeType> deques;

		public void LayoutTargets(List<CardTarget> targets)
		{
			deques[0].LayoutTargets(targets);
		}
		
		public bool CanRetrieve(List<CardTarget> targets, CardTarget card)
		{
			return deques.AsEnumerable().Reverse().Any(deque => deque.CanRetrieve(targets, card));
		}
	}
}
