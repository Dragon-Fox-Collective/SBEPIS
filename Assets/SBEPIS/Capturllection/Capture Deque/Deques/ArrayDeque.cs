using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	public class ArrayDeque : DequeType
	{
		public float cardDistance = 10;
		public Quaternion cardRotation = Quaternion.identity;

		public override void LayoutTargets(List<CardTarget> targets)
		{
			Vector3 position = cardDistance * (targets.Count - 1) / 2 * Vector3.left;
			foreach (CardTarget target in targets)
			{
				target.transform.localPosition = position;
				target.transform.localRotation = cardRotation * Quaternion.Euler(0, 180, 0);
				position += Vector3.right * cardDistance;
			}
		}
	}
}
