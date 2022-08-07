using UnityEngine;
using UnityEngine.Events;

public class UpdateExecutor : MonoBehaviour
{
	public UnityEvent<Transform> OnUpdate = new(), OnFixedUpdate = new();

	private void Update()
	{
		OnUpdate.Invoke(transform);
	}

	private void FixedUpdate()
	{
		OnFixedUpdate.Invoke(transform);
	}
}
