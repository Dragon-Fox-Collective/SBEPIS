using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.UI
{
	public class PhysicsButton : PhysicsSlider
	{
		public ButtonDirection direction;
		public float threshold = 0.75f;
		public float forcePressFactor = 100;
		
		public UnityEvent onPressed = new(), onUnpressed = new();
		
		public bool IsPressed { get; private set; }
		
		protected override void Evaluate()
		{
			base.Evaluate();
			if (!IsPressed && (direction == ButtonDirection.LessThan ? Progress < threshold : Progress > threshold))
			{
				IsPressed = true;
				onPressed.Invoke();
			}
			else if (IsPressed && (direction == ButtonDirection.LessThan ? Progress > threshold : Progress < threshold))
			{
				IsPressed = false;
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
					Rigidbody.AddRelativeForce((direction == ButtonDirection.LessThan ? -1 : 1) * forcePressFactor * Axis, ForceMode.Impulse);
					break;

				case ButtonAxis.XRotation:
				case ButtonAxis.YRotation:
				case ButtonAxis.ZRotation:
					Rigidbody.AddRelativeTorque((direction == ButtonDirection.LessThan ? -1 : 1) * forcePressFactor * Axis, ForceMode.Impulse);
					break;
			}
		}
		
		public void Yeah2()
		{
			print(gameObject + " " + Progress + " " + IsPressed);
		}
		
		public enum ButtonDirection
		{
			LessThan, GreaterThan
		}
	}
}