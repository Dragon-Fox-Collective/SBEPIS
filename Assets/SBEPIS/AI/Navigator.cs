using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.AI
{
	public abstract class Navigator : ValidatedMonoBehaviour
	{
		public Vector3 Target { get; set; }
		
		private void FixedUpdate()
		{
			MoveTowardPoint(Target);
		}
		
		protected abstract void MoveTowardPoint(Vector3 point);
	}
}
