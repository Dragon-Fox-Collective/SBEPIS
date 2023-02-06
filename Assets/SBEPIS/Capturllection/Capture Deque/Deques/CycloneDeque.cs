using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection.Deques
{
	[CreateAssetMenu(menuName="Deque/"+nameof(CycloneDeque))]
	public class CycloneDeque : DequeType
	{
		public float radius = 0.1f;
		public Quaternion cardRotation = Quaternion.identity;
		public float speed = 20;

		private float time;

		private CardTarget topCard;

		public override void LayoutTargets(List<CardTarget> targets)
		{
			time += Time.fixedDeltaTime * speed;

			float angle = time;
			float deltaAngle = 360f / targets.Count;
			foreach (CardTarget target in targets)
			{
				target.transform.localPosition = Quaternion.Euler(0, 0, angle) * Vector3.up * radius;
				target.transform.localRotation = Quaternion.Euler(0, 0, angle) * cardRotation;

				float modAngle = angle.ModAround(360);
				if (Mathf.Abs(modAngle) < deltaAngle / 2)
					topCard = target;
				else
					target.transform.localRotation *= Quaternion.Euler(0, 180, 0);

				angle += deltaAngle;
			}
		}

		public override bool CanRetrieve(List<CardTarget> targets, CardTarget card) => card == topCard;
	}
}
