using System;
using KBCore.Refs;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.UI
{
	[RequireComponent(typeof(Rigidbody), typeof(ConfigurableJoint))]
	public class PhysicsSlider : MonoBehaviour
	{
		[SerializeField, Self]
		private new Rigidbody rigidbody;
		public Rigidbody Rigidbody => rigidbody;
		
		[SerializeField, Self]
		private ConfigurableJoint joint;
		public ConfigurableJoint Joint => joint;
		
		private void OnValidate() => this.ValidateRefs();
		
		public ButtonAxis axis;
		
		public UnityEvent<float> onProgressChanged = new();
		
		[NonSerialized]
		public float progress;
		private float lastProgress;
		
		private Vector3 initialRelativeConnectedAnchor;
		private Vector3 initialLocalRotation;
		
		private void Start()
		{
			initialLocalRotation = transform.localRotation.eulerAngles;
			joint.autoConfigureConnectedAnchor = false;
			initialRelativeConnectedAnchor = joint.connectedAnchor - (transform.parent ? transform.parent.position : Vector3.zero);
		}
		
		private void Update()
		{
			Evaluate();
		}
		
		protected virtual void Evaluate()
		{
			lastProgress = progress;
			progress = RelativeProgress;
			if (lastProgress != progress)
				onProgressChanged.Invoke(progress);
		}
		
		protected float RelativeProgress
		{
			get
			{
				return axis switch
				{
					ButtonAxis.XPosition => GetDirectionValue(transform.position).Map(StartDirectionValue, EndDirectionValue, 0, 1),
					ButtonAxis.YPosition => GetDirectionValue(transform.position).Map(StartDirectionValue, EndDirectionValue, 0, 1),
					ButtonAxis.ZPosition => GetDirectionValue(transform.position).Map(StartDirectionValue, EndDirectionValue, 0, 1),
					ButtonAxis.XRotation => GetDirectionValue(transform.localRotation.eulerAngles - initialLocalRotation).Map(StartDirectionValue, EndDirectionValue, 0, 1),
					ButtonAxis.YRotation => GetDirectionValue(transform.localRotation.eulerAngles - initialLocalRotation).Map(StartDirectionValue, EndDirectionValue, 0, 1),
					ButtonAxis.ZRotation => GetDirectionValue(transform.localRotation.eulerAngles - initialLocalRotation).Map(StartDirectionValue, EndDirectionValue, 0, 1),
					_ => 0
				};
			}
		}
		
		protected void SetRelativeProgress(float progress)
		{
			switch (axis)
			{
				case ButtonAxis.XPosition:
				case ButtonAxis.YPosition:
				case ButtonAxis.ZPosition:
					Vector3 position = transform.position;
					SetDirectionValue(ref position, progress.Map(0, 1, StartDirectionValue, EndDirectionValue));
					transform.position = position;
					return;
				
				case ButtonAxis.XRotation:
				case ButtonAxis.YRotation:
				case ButtonAxis.ZRotation:
					Vector3 eulers = transform.localRotation.eulerAngles;
					SetDirectionValue(ref eulers, progress.Map(0, 1, StartDirectionValue, EndDirectionValue));
					eulers += initialLocalRotation;
					transform.localRotation = Quaternion.Euler(eulers);
					return;
				
				default:
					return;
			}
		}
		
		protected float StartDirectionValue => GetDirectionValue(StartPoint);
		
		protected float EndDirectionValue => GetDirectionValue(EndPoint);
		
		protected Vector3 StartPoint =>
			axis switch
			{
				ButtonAxis.XPosition => Anchor - joint.linearLimit.limit * GetAxis(transform),
				ButtonAxis.YPosition => Anchor - joint.linearLimit.limit * GetAxis(transform),
				ButtonAxis.ZPosition => Anchor - joint.linearLimit.limit * GetAxis(transform),
				ButtonAxis.XRotation => LowRotationalLimit * Axis,
				ButtonAxis.YRotation => LowRotationalLimit * Axis,
				ButtonAxis.ZRotation => LowRotationalLimit * Axis,
				_ => Vector3.zero
			};
		
		protected Vector3 EndPoint =>
			axis switch
			{
				ButtonAxis.XPosition => Anchor + joint.linearLimit.limit * GetAxis(transform),
				ButtonAxis.YPosition => Anchor + joint.linearLimit.limit * GetAxis(transform),
				ButtonAxis.ZPosition => Anchor + joint.linearLimit.limit * GetAxis(transform),
				ButtonAxis.XRotation => HighRotationalLimit * Axis,
				ButtonAxis.YRotation => HighRotationalLimit * Axis,
				ButtonAxis.ZRotation => HighRotationalLimit * Axis,
				_ => Vector3.zero
			};
		
		protected Vector3 Anchor => joint.connectedBody ? joint.connectedBody.transform.TransformPoint(joint.connectedAnchor) : joint.connectedAnchor;
		
		protected float LowRotationalLimit =>
			axis switch
			{
				ButtonAxis.XRotation => joint.lowAngularXLimit.limit,
				ButtonAxis.YRotation => -joint.angularYLimit.limit,
				ButtonAxis.ZRotation => -joint.angularZLimit.limit,
				_ => 0
			};
		
		protected float HighRotationalLimit =>
			axis switch
			{
				ButtonAxis.XRotation => joint.highAngularXLimit.limit,
				ButtonAxis.YRotation => joint.angularYLimit.limit,
				ButtonAxis.ZRotation => joint.angularZLimit.limit,
				_ => 0
			};
		
		protected Vector3 Axis =>
			axis switch
			{
				ButtonAxis.XPosition => Vector3.right,
				ButtonAxis.XRotation => Vector3.right,
				ButtonAxis.YPosition => Vector3.up,
				ButtonAxis.YRotation => Vector3.up,
				ButtonAxis.ZPosition => Vector3.forward,
				ButtonAxis.ZRotation => Vector3.forward,
				_ => Vector3.zero
			};
		
		protected Vector3 GetAxis(Transform transform) =>
			axis switch
			{
				ButtonAxis.XPosition => transform.right,
				ButtonAxis.XRotation => transform.right,
				ButtonAxis.YPosition => transform.up,
				ButtonAxis.YRotation => transform.up,
				ButtonAxis.ZPosition => transform.forward,
				ButtonAxis.ZRotation => transform.forward,
				_ => Vector3.zero
			};
		
		protected float GetDirectionValue(Vector3 vector) =>
			axis switch
			{
				ButtonAxis.XPosition => vector.x,
				ButtonAxis.XRotation => vector.x,
				ButtonAxis.YPosition => vector.y,
				ButtonAxis.YRotation => vector.y,
				ButtonAxis.ZPosition => vector.z,
				ButtonAxis.ZRotation => vector.z,
				_ => 0
			};
		
		protected void SetDirectionValue(ref Vector3 vector, float value)
		{
			switch (axis)
			{
				case ButtonAxis.XPosition:
				case ButtonAxis.XRotation:
					vector.x = value;
					return;

				case ButtonAxis.YPosition:
				case ButtonAxis.YRotation:
					vector.y = value;
					return;

				case ButtonAxis.ZPosition:
				case ButtonAxis.ZRotation:
					vector.z = value;
					return;

				default:
					return;
			}
		}
		
		public void ResetAnchor(float progress)
		{
			joint.connectedAnchor = initialRelativeConnectedAnchor + (transform.parent ? transform.parent.position : Vector3.zero);
			SetRelativeProgress(progress);
		}
		
		public void Yeah()
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