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

		public override void TickDeque(List<CardTarget> targets, float delta)
		{
			time += delta;
			UpdateTopCard(targets);
		}

		private void UpdateTopCard(List<CardTarget> targets)
		{
			float cardAngle = time * speed;
			float deltaAngle = 360f / targets.Count;
			foreach (CardTarget target in targets)
			{
				float modAngle = cardAngle.ModAround(360);
				if (Mathf.Abs(modAngle) < deltaAngle / 2)
					topCard = target;
				
				cardAngle += deltaAngle;
			}
		}

		public override void LayoutTargets(List<CardTarget> targets)
		{
			float cardAngle = time * speed;
			float deltaAngle = 360f / targets.Count;
			foreach (CardTarget target in targets)
			{
				target.transform.localPosition = Quaternion.Euler(0, 0, cardAngle) * Vector3.up * radius;
				target.transform.localRotation = Quaternion.Euler(0, 0, cardAngle) * cardRotation;

				cardAngle += deltaAngle;
			}
		}

		public override bool CanRetrieve(List<CardTarget> targets, CardTarget card) => card == topCard;
		
		public override int GetIndexToInsertAt(List<CardTarget> targets, CardTarget card) => targets.Count;
	}
}
