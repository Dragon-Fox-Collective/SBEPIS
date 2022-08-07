using UnityEngine;

namespace SBEPIS.Physics
{
	[RequireComponent(typeof(Rigidbody))]
	public class MovementLerp : MonoBehaviour
	{
		public Transform endPoint;
		public float speed = 1;

		private new Rigidbody rigidbody;
		private Vector3 startPos, endPos;
		private bool resetting;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			startPos = transform.position;
			endPos = endPoint.position;
		}

		private void FixedUpdate()
		{
			Vector3 target = resetting ? startPos : endPos;
			Vector3 delta = target - transform.position;
			Vector3 velocity = speed * Time.fixedDeltaTime * delta.normalized;

			if (delta.sqrMagnitude < velocity.sqrMagnitude)
			{
				rigidbody.MovePosition(target);
				resetting = !resetting;
			}
			else
				rigidbody.MovePosition(transform.position + velocity);
		}
	}
}
