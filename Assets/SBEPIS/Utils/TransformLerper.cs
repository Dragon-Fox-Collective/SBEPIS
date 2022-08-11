using UnityEngine;
using UnityEngine.Events;

public class TransformLerper : MonoBehaviour
{
	public float timeToComplete = 1;
	public AnimationCurve curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
	public UnityEvent onEnd = new();

	public Transform target;

	private Transform start;
	private Vector3 startPosition;
	private Quaternion startRotation;
	private float time = 0;

	private new Rigidbody rigidbody;

	private void Awake()
	{
		enabled = false;
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.Disable();
	}

	private void Update()
	{
		time += Time.deltaTime / timeToComplete;
		transform.SetPositionAndRotation(
			Vector3.Lerp(start ? start.position : startPosition, target.position, curve.Evaluate(time)),
			Quaternion.Lerp(start ? start.rotation : startRotation, target.rotation, curve.Evaluate(time)));

		if (time >= 1)
		{
			Destroy(this);
			rigidbody.Enable();
			onEnd.Invoke();
		}
	}

	public void StartTargetting(Transform start)
	{
		enabled = true;
		this.start = start;
		startPosition = transform.position;
		startRotation = transform.rotation;
		time = 0;
	}

	public TransformLerper Chain(Transform next)
	{
		TransformLerper newLerper = gameObject.AddComponent<TransformLerper>();
		newLerper.target = next;
		newLerper.timeToComplete = timeToComplete;
		onEnd.AddListener(() => newLerper.StartTargetting(target));
		return newLerper;
	}
}
