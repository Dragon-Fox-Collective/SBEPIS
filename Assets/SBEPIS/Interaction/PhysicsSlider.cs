using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Interaction
{
	[RequireComponent(typeof(Rigidbody), typeof(ConfigurableJoint))]
	public class PhysicsSlider : MonoBehaviour
	{
		public ButtonAxis axis;

		public UnityEvent<float> onProgressChanged;

		[NonSerialized]
		public float progress;
		private float lastProgress;

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

		protected virtual void Evaluate()
		{
			progress = GetRelativeProgress();
			if (lastProgress != progress)
				onProgressChanged.Invoke(progress);
			lastProgress = progress;
		}

		protected float GetRelativeProgress()
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

		protected Vector3 GetWorldLinearStartPoint()
		{
			return transform.TransformPoint(transform.InverseTransformPoint(joint.connectedAnchor) - joint.linearLimit.limit * GetAxis());
		}

		protected Vector3 GetWorldLinearEndPoint()
		{
			return transform.TransformPoint(transform.InverseTransformPoint(joint.connectedAnchor) + joint.linearLimit.limit * GetAxis());
		}

		protected Vector3 GetLocalRotationalStartPoint()
		{
			return GetLowRotationalLimit() * GetAxis();
		}

		protected Vector3 GetLocalRotationalEndPoint()
		{
			return GetHighRotationalLimit() * GetAxis();
		}

		protected float GetLowRotationalLimit()
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

		protected float GetHighRotationalLimit()
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

		protected Vector3 GetAxis()
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

		protected Vector3 GetRotationalAxis(Transform transform)
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

		protected float GetLinearDirectionValue(Vector3 vector)
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

		protected float GetRotationalDirectionValue(Vector3 eulers)
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

		public virtual void Yeah()
		{
			print(gameObject + " " + progress);
		}

		public enum ButtonAxis
		{
			XPosition, YPosition, ZPosition,
			XRotation, YRotation, ZRotation
		}
	}
}