using UnityEngine;

public class ExistanceDebugger : MonoBehaviour
{
	public MonoBehaviour check;

	private void Awake()
	{
		print("Awake" + check.gameObject.activeSelf + " " + check.gameObject.activeInHierarchy + " " + check.enabled);
		Destroy(this, 0.5f);
	}

	private void Update()
	{
		print(check.gameObject.activeSelf + " " + check.gameObject.activeInHierarchy + " " + check.enabled);
	}
}
