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

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			joint = GetComponent<ConfigurableJoint>();
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
			}
			return 0;
		}

		private Vector3 GetWorldLinearStartPoint()
		{
			return transform.TransformPoint(transform.InverseTransformPoint(joint.connectedAnchor) - joint.linearLimit.limit * GetLinearAxis());
		}

		private Vector3 GetWorldLinearEndPoint()
		{
			return transform.TransformPoint(transform.InverseTransformPoint(joint.connectedAnchor) + joint.linearLimit.limit * GetLinearAxis());
		}

		private Vector3 GetLinearAxis()
		{
			switch (axis)
			{
				case ButtonAxis.XPosition:
					return Vector3.right;

				case ButtonAxis.YPosition:
					return Vector3.up;

				case ButtonAxis.ZPosition:
					return Vector3.forward;

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

		public void ForcePress()
		{
			rigidbody.AddRelativeForce(new Vector3(
				axis == ButtonAxis.XPosition ? 1 : 0,
				axis == ButtonAxis.YPosition ? 1 : 0,
				axis == ButtonAxis.ZPosition ? 1 : 0
			) * (direction == ButtonDirection.LessThan ? -1 : 1) * 100);
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