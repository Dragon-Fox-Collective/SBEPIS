using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ProceduralAnimation : MonoBehaviour
{
	public List<Transform> targets = new();
	public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
	public float speed = 1;
	public UnityEvent onPlay = new(), onEnd = new(), onReversePlay = new(), onReverseEnd = new();

	public float startTime => curve[0].time;
	public float endTime => curve[curve.length - 1].time;

	private new Rigidbody rigidbody;
	private float time = 0;

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
		print($"Playing {this} forward");
		Resume();
		speed = Mathf.Abs(speed);
		if (time == startTime)
			onPlay.Invoke();
	}

	public void PlayReverse()
	{
		print($"Playing {this} reverse");
		Resume();
		speed = -Mathf.Abs(speed);
		if (time == endTime)
			onReversePlay.Invoke();
	}

	public void Stop()
	{
		print($"Stopping {this} start");
		Pause();
		SeekStart();
	}

	public void StopEnd()
	{
		print($"Stopping {this} end");
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
		if (targets.Count == 0)
			return;
		else if (targets.Count == 1)
		{
			transform.SetPositionAndRotation(targets[0].position, targets[0].rotation);
			return;
		}

		time += Time.deltaTime * speed;

		float evaluation = curve.Evaluate(time);
		int i = (int)Mathf.Clamp(evaluation, 0, targets.Count - 2);
		evaluation -= i;

		Transform start = targets[i];
		Transform end = targets[i + 1];
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
