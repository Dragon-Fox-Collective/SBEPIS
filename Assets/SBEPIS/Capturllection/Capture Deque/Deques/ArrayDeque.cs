using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	public class ArrayDeque : DequeType
	{
		public float cardDistance = 10;
		public Quaternion cardRotation = Quaternion.identity;
		public float wobbleHeight = 10;
		public float wobbleTimeOffset = 1;

		public override void LayoutTargets(IReadOnlyCollection<CardTarget> targets)
		{
			int i = 0;
			Vector3 right = cardDistance * (targets.Count - 1) / 2 * Vector3.left;
			foreach (CardTarget target in targets.OrderBy(target => target.lifetime))
			{
				Vector3 up = Mathf.Sin(Time.fixedTime + i * wobbleTimeOffset) * wobbleHeight * Vector3.up;
				target.transform.localPosition = right + up;
				target.transform.localRotation = cardRotation;
				right += Vector3.right * cardDistance;
				i++;
			}
		}
	}
}
