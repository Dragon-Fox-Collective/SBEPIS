using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Interaction
{
	public class PhysicsButton : MonoBehaviour
	{
		public ButtonAxis axis;
		public ButtonDirection direction;
		public float threshold;

		public UnityEvent onPressed, onUnpressed;

		[NonSerialized]
		public bool isPressed;

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