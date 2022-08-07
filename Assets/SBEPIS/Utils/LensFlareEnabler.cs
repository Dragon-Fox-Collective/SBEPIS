using UnityEngine;
using UnityEngine.InputSystem;

namespace SBEPIS.Utils
{
	[RequireComponent(typeof(LensFlare))]
	public class LensFlareEnabler : MonoBehaviour
	{
		private LensFlare lensFlare;

		private void Awake()
		{
			lensFlare = GetComponent<LensFlare>();
		}

		public void OnControlsChanged(PlayerInput input)
		{
			switch (input.currentControlScheme)
			{
				case "OpenXR":

					break;

				default:

					break;
			}
		}
	}
}
