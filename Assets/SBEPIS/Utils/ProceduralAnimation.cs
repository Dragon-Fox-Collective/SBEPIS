using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Utils
{
	
public class ProceduralAnimation : MonoBehaviour
{
	public List<Func<Transform>> targetProviders = new();
	public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
	public float speed = 1;
	public UnityEvent onPlay = new();
	public UnityEvent onEnd = new();
	public UnityEvent onReversePlay = new();
	public UnityEvent onReverseEnd = new();

	public float startTime => curve[0].time;
	public float endTime => curve[curve.length - 1].time;
	public float time { get; private set; } = 0;

	private new Rigidbody rigidbody;

	private void Awake()
	{
		rigidbody = GetComponent<Rigidbody>();

		Stop();
	}

	public void Pause()
	{
		enabled = false;
	}

	public void Resume()
	{
		enabled = true;
	}

	public void SeekStart()
	{
		time = startTime;
	}

	public void SeekEnd()
	{
		time = endTime;
	}

	public void PlayForward()
	{
		Resume();
		speed = Mathf.Abs(speed);
		if (time <= startTime)
			onPlay.Invoke();
	}

	public void PlayReverse()
	{
		Resume();
		speed = -Mathf.Abs(speed);
		if (time >= endTime)
			onReversePlay.Invoke();
	}

	public void Stop()
	{
		Pause();
		SeekStart();
	}

	public void StopEnd()
	{
		Pause();
		SeekEnd();
	}

	private void OnEnable()
	{
		if (rigidbody)
			rigidbody.Disable();
	}

	private void OnDisable()
	{
		if (rigidbody)
			rigidbody.Enable();
	}

	private void Update()
	{
		if (targetProviders.Count == 0)
			return;
		else if (targetProviders.Count == 1)
		{
			Transform target = targetProviders[0].Invoke();
			transform.SetPositionAndRotation(target.position, target.rotation);
			return;
		}

		time += Time.deltaTime * speed;

		float evaluation = curve.Evaluate(time);
		int i = (int)Mathf.Clamp(evaluation, 0, targetProviders.Count - 2);
		evaluation -= i;

		Transform start = targetProviders[i].Invoke();
		Transform end = targetProviders[i + 1].Invoke();
		transform.SetPositionAndRotation(
				Vector3.Lerp(start.position, end.position, evaluation),
				Quaternion.Lerp(start.rotation, end.rotation, evaluation));

		if (time > endTime)
		{
			StopEnd();
			onEnd.Invoke();
		}
		else if (time < startTime)
		{
			Stop();
			onReverseEnd.Invoke();
		}
	}
}

}