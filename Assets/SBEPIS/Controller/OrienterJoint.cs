using UnityEngine;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	public class OrienterJoint : MonoBehaviour
	{
		public float acceleration = 1;


		const float VELOCITY_GRAPH_MAX = 6.0f;

		[DebugGUIPrint, DebugGUIGraph(group: 0, min: -VELOCITY_GRAPH_MAX, max: VELOCITY_GRAPH_MAX, autoScale: false, r: 1.0f, g: 0.3f, b: 0.3f)]
		private float currentVelocity;

		const float DISTANCE_GRAPH_MAX = 3.0f;

		[DebugGUIPrint, DebugGUIGraph(group: 1, min: -DISTANCE_GRAPH_MAX, max: DISTANCE_GRAPH_MAX, autoScale: false, r: 1.0f, g: 0.3f, b: 0.3f)]
		private float currentDistance;
		[DebugGUIPrint, DebugGUIGraph(group: 1, min: -DISTANCE_GRAPH_MAX, max: DISTANCE_GRAPH_MAX, autoScale: false, r: 0.3f, g: 1.0f, b: 0.3f)]
		private float maxPoint;
		[DebugGUIPrint, DebugGUIGraph(group: 1, min: -DISTANCE_GRAPH_MAX, max: DISTANCE_GRAPH_MAX, autoScale: false, r: 0.3f, g: 0.3f, b: 1.0f)]
		private float criticalPoint;
		[DebugGUIPrint, DebugGUIGraph(group: 1, min: -1.5f, max: 1.5f, autoScale: false, r: 1.0f, g: 1.0f, b: 1.0f)]
		private float accelerationSign;

		const float TIME_GRAPH_MAX = 2.0f;

		[DebugGUIPrint, DebugGUIGraph(group: 2, min: -TIME_GRAPH_MAX, max: TIME_GRAPH_MAX, autoScale: false, r: 0.3f, g: 1.0f, b: 0.3f)]
		private float timeToMax;

		[DebugGUIPrint, DebugGUIGraph(group: 3, min: -1.5f, max: 1.5f, autoScale: false, r: 1.0f, g: 1.0f, b: 1.0f)]
		private float velocitySign;


		private Vector3 up = Vector3.up;
		private bool isStanding = false;
		private new Rigidbody rigidbody;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();

			DebugGUI.Log("Bepis");
		}
		
		public void Orient(Vector3 up)
		{
			isStanding = up != Vector3.zero;
			if (isStanding)
				this.up = up.normalized;
		}

		private void FixedUpdate()
		{
			if (!isStanding)
				return;

			Quaternion delta = Quaternion.FromToRotation(transform.up, up);
			if (delta.w < 0) delta = delta.Select(x => -x);
			delta.ToAngleAxis(out currentDistance, out Vector3 axis);
			currentDistance *= Mathf.Deg2Rad;

			velocitySign = Vector3.Dot(rigidbody.angularVelocity, axis) > 0 ? -1 : 1;
			currentVelocity = rigidbody.angularVelocity.magnitude * velocitySign;

			timeToMax = currentVelocity / acceleration;
			maxPoint = currentDistance - 0.5f * currentVelocity * currentVelocity / -acceleration;
			criticalPoint = 0.5f * maxPoint;

			bool decelerate = currentDistance < criticalPoint && currentVelocity < 0;
			accelerationSign = decelerate ? 1 : -1;

			float deltaVelocity = accelerationSign * acceleration * Time.fixedDeltaTime;
			rigidbody.angularVelocity += deltaVelocity * -axis;

			if (currentDistance < rigidbody.angularVelocity.magnitude * Time.fixedDeltaTime && rigidbody.angularVelocity.magnitude < 2 * acceleration * Time.fixedDeltaTime)
				rigidbody.angularVelocity = currentDistance * axis / Time.fixedDeltaTime;
		}
	}
}
