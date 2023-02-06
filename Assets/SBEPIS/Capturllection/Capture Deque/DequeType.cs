using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public abstract class DequeType : ScriptableObject
	{
		public Texture2D cardTexture;
		public Texture2D dequeTexture;

		public abstract void TickDeque(List<CardTarget> targets, float delta);
		public abstract void LayoutTargets(List<CardTarget> targets);
		public abstract bool CanRetrieve(List<CardTarget> targets, CardTarget card);
	}
}
