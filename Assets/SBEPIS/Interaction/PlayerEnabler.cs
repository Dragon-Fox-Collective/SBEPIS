using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerEnabler : MonoBehaviour
{
	public int ticks = 10;

	private void Update()
	{
		if (ticks > 0)
			ticks--;
		else
		{
			GetComponent<PlayerInput>().enabled = true;
			Destroy(this);
		}
	}
}
