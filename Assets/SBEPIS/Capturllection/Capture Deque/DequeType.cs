using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class DequeType : MonoBehaviour
	{
		public abstract void LayoutTargets(List<CardTarget> targets);
		public abstract bool CanRetrieve(List<CardTarget> targets, CardTarget card);
	}

	public abstract class DequeCardInfo { }
}
