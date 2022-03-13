using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Interaction
{
	public class PhysicsButton : PhysicsSlider
	{
		public ButtonDirection direction;
		public float threshold = 0.75f;

		public UnityEvent onPressed, onUnpressed;

		[NonSerialized]
		public bool isPressed;

		protected override void Evaluate()
		{
			base.Evaluate();
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
					rigidbody.AddRelativeTorque(GetAxis() * (direction == ButtonDirection.LessThan ? -1 : 1) * 150);
					break;
			}
		}

		public void Yeah2()
		{
			print(gameObject + " " + progress + " " + isPressed);
		}

		public enum ButtonDirection
		{
			LessThan, GreaterThan
		}
	}
}