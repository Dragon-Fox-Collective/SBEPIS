using KBCore.Refs;
using SBEPIS.Physics;
using UnityEngine;

namespace SBEPIS.AI
{
	public class FootBallNavigator : Navigator
	{
		[SerializeField, Self] private GravitySum gravitySum;
		
		[SerializeField, Anywhere] private FootBall footBall;
		
		[SerializeField] private float maxSpeed = 1;
		
		protected override void MoveTowardPoint(Vector3 point)
		{
			footBall.Move(gravitySum.UpDirection, (point - transform.position).normalized * maxSpeed);
		}
	}
}
