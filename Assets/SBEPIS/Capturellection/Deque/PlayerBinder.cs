using SBEPIS.Controller;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturellection
{
	public class PlayerBinder : MonoBehaviour
	{
		public UnityEvent<PlayerReference> onBindToPlayer = new();
		
		public void BindToPlayer(Grabber grabber, Grabbable _)
		{
			if (!grabber.TryGetComponent(out PlayerReference playerReference))
				return;
			
			BindToPlayer(playerReference);
		}
		
		public void BindToPlayer(PlayerReference playerReference) => onBindToPlayer.Invoke(playerReference);
	}
}