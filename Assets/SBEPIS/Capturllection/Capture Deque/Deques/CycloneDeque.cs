using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	public class CycloneDeque : DequeType
	{
		public float radius = 20;
		public Quaternion cardRotation = Quaternion.identity;
		public float speed = 1;

		public override void LayoutTargets(List<CardTarget> targets)
		{
			float angle = Time.fixedTime * speed;
			float deltaAngle = 360f / targets.Count;
			foreach (CardTarget target in targets)
			{
				target.transform.localPosition = Quaternion.Euler(0, 0, angle) * Vector3.up * radius;
				target.transform.localRotation = cardRotation * Quaternion.Euler(0, 0, angle);
				angle += deltaAngle;
			}
		}
	}
}
