using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(Rigidbody))]
	public class PhysicsButton : MonoBehaviour
	{
		public ButtonAxis axis;
		public ButtonDirection direction;
		public float threshold;

		public UnityEvent onPressed, onUnpressed;

		[NonSerialized]
		public bool isPressed;

		public new Rigidbody rigidbody { get; private set; }

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void Update()
		{
			switch (axis)
			{
				case ButtonAxis.XPosition:
					Evaluate(transform.localPosition.x);
					break;

				case ButtonAxis.YPosition:
					Evaluate(transform.localPosition.y);
					break;

				case ButtonAxis.ZPosition:
					Evaluate(transform.localPosition.z);
					break;

				case ButtonAxis.XRotation:
					Evaluate(transform.localRotation.eulerAngles.x);
					break;

				case ButtonAxis.YRotation:
					Evaluate(transform.localRotation.eulerAngles.y);
					break;

				case ButtonAxis.ZRotation:
					Evaluate(transform.localRotation.eulerAngles.z);
					break;
			}
		}

		private void Evaluate(float value)
		{
			if (!isPressed && (direction == ButtonDirection.LessThan ? value < threshold : value > threshold))
			{
				isPressed = true;
				onPressed.Invoke();
			}
			else if (isPressed && (direction == ButtonDirection.LessThan ? value > threshold : value < threshold))
			{
				isPressed = false;
				onUnpressed.Invoke();
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
			print(isPressed);
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