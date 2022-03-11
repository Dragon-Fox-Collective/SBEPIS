using UnityEngine;
using UnityEngine.InputSystem;

namespace SBEPIS.Interaction
{
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
}