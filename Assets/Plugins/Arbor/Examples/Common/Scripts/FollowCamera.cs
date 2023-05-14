//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Examples
{
	/// <summary>
	/// Behavior of the following camera. (General MonoBehaviour script)
	/// </summary>
	[AddComponentMenu("Arbor/Examples/FollowCamera")]
	public sealed class FollowCamera : MonoBehaviour
	{
		/// <summary>
		/// Target Transform
		/// </summary>
		public Transform target;

		/// <summary>
		/// Distance to target
		/// </summary>
		public float distance = 10f;

		/// <summary>
		/// Look offset
		/// </summary>
		public Vector3 lookOffset = Vector3.up * 1.0f;

		/// <summary>
		/// Transform cache
		/// </summary>
		private Transform _Transform;

		/// <summary>
		/// Direction
		/// </summary>
		private Vector3 _Direction;

		private void Start()
		{
			// Cache Transform
			_Transform = transform;

			// Look at Player position + lookOffset
			_Transform.LookAt(target.position + lookOffset, Vector3.up);

			// Calculate direction
			_Direction = (target.position - _Transform.position).normalized;
		}

		// Update is called once per frame
		void LateUpdate()
		{
			// Calculate the position.
			Vector3 targetPosition = target.position;

			_Transform.position = targetPosition - _Direction * distance;

			// Look at Player position + lookOffset
			_Transform.LookAt(target.position + lookOffset, Vector3.up);
		}
	}
}