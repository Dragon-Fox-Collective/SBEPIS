using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalTransformLerper : MonoBehaviour
{
	public float smoothTime = 1;

	public bool lerpPosition;
	public Vector3 positionTarget;
	private Vector3 velocity;
	public bool lerpRotation;
	public Quaternion rotationTarget;
	private Quaternion deriv;

	private void Update()
	{
		if (lerpPosition)
			transform.localPosition = Vector3.SmoothDamp(transform.localPosition, positionTarget, ref velocity, smoothTime);
		if (lerpRotation)
			transform.localRotation = QuaternionUtil.SmoothDamp(transform.localRotation, rotationTarget, ref deriv, smoothTime);
	}

	public void EnableLerp()
	{
		lerpPosition = true;
		lerpRotation = true;
		ResetVelocities();
	}

	public void DisableLerp()
	{
		lerpPosition = false;
		lerpRotation = false;
		ResetVelocities();
	}

	public void ResetVelocities()
	{
		velocity = Vector3.zero;
		deriv = Quaternion.identity;
	}

	public void SetTargets(Vector3 positionTarget, Quaternion rotationTarget)
	{
		this.positionTarget = positionTarget;
		this.rotationTarget = rotationTarget;
	}
}
