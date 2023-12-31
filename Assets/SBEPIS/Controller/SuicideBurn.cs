using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Controller
{
	[RequireComponent(typeof(Rigidbody))]
	public class SuicideBurn : ValidatedMonoBehaviour
	{
		[SerializeField, Self]
		private new Rigidbody rigidbody;
		
		public float acceleration = 1;

		private const float VelocityGraphMax = 10.0f;
		
		[DebugGUIPrint, DebugGUIGraph(group: 0, min: -VelocityGraphMax, max: VelocityGraphMax, autoScale: false, r: 1.0f, g: 0.3f, b: 0.3f)]
		private float currentVelocity;
		
		private const float DistanceGraphMax = 10.0f;
		
		[DebugGUIPrint, DebugGUIGraph(group: 1, min: -DistanceGraphMax, max: DistanceGraphMax, autoScale: false, r: 1.0f, g: 0.3f, b: 0.3f)]
		private float currentDistance;
		[DebugGUIPrint, DebugGUIGraph(group: 1, min: -DistanceGraphMax, max: DistanceGraphMax, autoScale: false, r: 0.3f, g: 1.0f, b: 0.3f)]
		private float maxPoint;
		[DebugGUIPrint, DebugGUIGraph(group: 1, min: -DistanceGraphMax, max: DistanceGraphMax, autoScale: false, r: 0.3f, g: 0.3f, b: 1.0f)]
		private float criticalPoint;
		[DebugGUIPrint, DebugGUIGraph(group: 1, min: -1.5f, max: 1.5f, autoScale: false, r: 1.0f, g: 1.0f, b: 1.0f)]
		private float accelerationSign;

		private const float TimeGraphMax = 10.0f;
		
		[DebugGUIPrint, DebugGUIGraph(group: 2, min: -TimeGraphMax, max: TimeGraphMax, autoScale: false, r: 0.3f, g: 1.0f, b: 0.3f)]
		private float timeToMax;
		
		[DebugGUIPrint, DebugGUIGraph(group: 3, min: -1.5f, max: 1.5f, autoScale: false, r: 1.0f, g: 1.0f, b: 1.0f)]
		private float velocitySign;
		
		
		public Transform target;
		public float initialVelocity;
		
		private void Awake()
		{
			rigidbody.velocity = (transform.position - target.position).normalized * initialVelocity;
			
			DebugGUI.Log("Bepis");
		}
		
		private void FixedUpdate()
		{
			Vector3 delta = target.position - transform.position;
			
			currentDistance = delta.magnitude;
			
			velocitySign = Vector3.Dot(rigidbody.velocity, delta) > 0 ? -1 : 1;
			currentVelocity = rigidbody.velocity.magnitude * velocitySign;
			
			timeToMax = currentVelocity / acceleration;
			maxPoint = currentDistance - 0.5f * currentVelocity * currentVelocity / -acceleration;
			criticalPoint = 0.5f * maxPoint;
			
			bool decelerate = currentDistance < criticalPoint && currentVelocity < 0;
			accelerationSign = decelerate ? 1 : -1;
			
			float deltaVelocity = accelerationSign * acceleration * Time.fixedDeltaTime;
			rigidbody.velocity += deltaVelocity * -delta.normalized;
			
			if (currentDistance < rigidbody.velocity.magnitude * Time.fixedDeltaTime && rigidbody.velocity.magnitude < 2 * acceleration * Time.fixedDeltaTime)
				rigidbody.velocity = delta / Time.fixedDeltaTime;
		}
	}
}
