using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(Rigidbody), typeof(ConfigurableJoint))]
	public class PhysicsButton : MonoBehaviour
	{
		public ButtonAxis axis;
		public ButtonDirection direction;
		public float threshold = 0.75f;

		public UnityEvent onPressed, onUnpressed;

		[NonSerialized]
		public bool isPressed;

		public new Rigidbody rigidbody { get; private set; }
		public ConfigurableJoint joint { get; private set; }

		private Vector3 initialLocalRotation;

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			joint = GetComponent<ConfigurableJoint>();
		}

		private void Start()
		{
			initialLocalRotation = transform.localRotation.eulerAngles;
		}

		private void Update()
		{
			Evaluate();
		}

		private void Evaluate()
		{
			float progress = GetRelativeProgress();
			if (!isPressed && (direction == ButtonDirection.LessThan ? progress < threshold : progress > threshold))
			{
				isPressed = true;
				onPressed.Invoke();
			}
			else if (isPressed && (direction == ButtonDirection.LessThan ? progress > threshold : progress < threshold))
			{
				isPressed = false;
				onUnpressed.Invoke();
			}
		}

		private float GetRelativeProgress()
		{
			switch (axis)
			{
				case ButtonAxis.XPosition:
				case ButtonAxis.YPosition:
				case ButtonAxis.ZPosition:
					return GetLinearDirectionValue(transform.localPosition).Map(GetLinearDirectionValue(GetWorldLinearStartPoint()), GetLinearDirectionValue(GetWorldLinearEndPoint()), 0, 1);

				case ButtonAxis.XRotation:
				case ButtonAxis.YRotation:
				case ButtonAxis.ZRotation:
					return GetRotationalDirectionValue(transform.localRotation.eulerAngles - initialLocalRotation).Map(GetRotationalDirectionValue(GetLocalRotationalStartPoint()), GetRotationalDirectionValue(GetLocalRotationalEndPoint()), 0, 1);

				default:
					return 0;
			}
		}

		private Vector3 GetWorldLinearStartPoint()
		{
			return transform.TransformPoint(transform.InverseTransformPoint(joint.connectedAnchor) - joint.linearLimit.limit * GetAxis());
		}

		private Vector3 GetWorldLinearEndPoint()
		{
			return transform.TransformPoint(transform.InverseTransformPoint(joint.connectedAnchor) + joint.linearLimit.limit * GetAxis());
		}

		private Vector3 GetLocalRotationalStartPoint()
		{
			return GetLowRotationalLimit() * GetAxis();
		}

		private Vector3 GetLocalRotationalEndPoint()
		{
			return GetHighRotationalLimit() * GetAxis();
		}

		private float GetLowRotationalLimit()
		{
			switch (axis)
			{
				case ButtonAxis.XRotation:
					return joint.lowAngularXLimit.limit;

				case ButtonAxis.YRotation:
					return -joint.angularYLimit.limit;

				case ButtonAxis.ZRotation:
					return -joint.angularZLimit.limit;

				default:
					return 0;
			}
		}

		private float GetHighRotationalLimit()
		{
			switch (axis)
			{
				case ButtonAxis.XRotation:
					return joint.highAngularXLimit.limit;

				case ButtonAxis.YRotation:
					return joint.angularYLimit.limit;

				case ButtonAxis.ZRotation:
					return joint.angularZLimit.limit;

				default:
					return 0;
			}
		}

		private Vector3 GetAxis()
		{
			switch (axis)
			{
				case ButtonAxis.XPosition:
				case ButtonAxis.XRotation:
					return Vector3.right;

				case ButtonAxis.YPosition:
				case ButtonAxis.YRotation:
					return Vector3.up;

				case ButtonAxis.ZPosition:
				case ButtonAxis.ZRotation:
					return Vector3.forward;

				default:
					return Vector3.zero;
			}
		}

		private Vector3 GetRotationalAxis(Transform transform)
		{
			switch (axis)
			{
				case ButtonAxis.XRotation:
					return transform.right;

				case ButtonAxis.YRotation:
					return transform.up;

				case ButtonAxis.ZRotation:
					return transform.forward;

				default:
					return Vector3.zero;
			}
		}

		private float GetLinearDirectionValue(Vector3 vector)
		{
			switch (axis)
			{
				case ButtonAxis.XPosition:
					return vector.x;

				case ButtonAxis.YPosition:
					return vector.y;

				case ButtonAxis.ZPosition:
					return vector.z;

				default:
					return 0;
			}
		}

		private float GetRotationalDirectionValue(Vector3 eulers)
		{
			switch (axis)
			{
				case ButtonAxis.XRotation:
					return eulers.x;

				case ButtonAxis.YRotation:
					return eulers.y;

				case ButtonAxis.ZRotation:
					return eulers.z;

				default:
					return 0;
			}
		}

		public void ForcePress()
		{
			switch (axis)
			{
				case ButtonAxis.XPosition:
				case ButtonAxis.YPosition:
				case ButtonAxis.ZPosition:
					rigidbody.AddRelativeForce(GetAxis() * (direction == ButtonDirection.LessThan ? -1 : 1) * 100);
					break;

				case ButtonAxis.XRotation:
				case ButtonAxis.YRotation:
				case ButtonAxis.ZRotation:
					rigidbody.AddRelativeTorque(GetAxis() * (direction == ButtonDirection.LessThan ? -1 : 1) * 100);
					break;
			}
		}

		public void Yeah()
		{
			print(gameObject + " " + isPressed);
		}

		public enum ButtonAxis
		{
			XPosition, YPosition, ZPosition,
			XRotation, YRotation, ZRotation
		}

		public enum ButtonDirection
		{
			LessThan, GreaterThan
		}
	}
}