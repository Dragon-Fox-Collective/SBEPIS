using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace SBEPIS.Capturllection
{
	public abstract class DequeType : ScriptableObject
	{
		public Texture2D dequeTexture;
		
		public abstract void LayoutTargets(List<CardTarget> targets);
		public abstract bool CanRetrieve(List<CardTarget> targets, CardTarget card);
	}
}
