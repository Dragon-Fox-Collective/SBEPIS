using KBCore.Refs;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SBEPIS.Utils
{
	[RequireComponent(typeof(LensFlare))]
	public class LensFlareEnabler : MonoBehaviour
	{
		[SerializeField, Self]
		private LensFlare lensFlare;
		
		private void Awake()
		{
			lensFlare = GetComponent<LensFlare>();
		}
		
		public void OnControlsChanged(PlayerInput input)
		{
			lensFlare.enabled = input.currentControlScheme switch
			{
				"OpenXR" => false,
				_ => true
			};
		}
	}
}
