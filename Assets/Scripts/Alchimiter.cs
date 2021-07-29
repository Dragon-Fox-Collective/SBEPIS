using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WrightWay.SBEPIS
{
	public class Alchimiter : MonoBehaviour
	{
		public PlacementHelper dowelPlacement;
		public Transform spawnPoint;
		public Animator animator;

		private long captchaHash;

		/// <summary>
		/// Called when a dowel is placed in the slot, starts the animation
		/// </summary>
		public void StartAlchemizing()
		{
			animator.SetBool("Adopted", true);
			captchaHash = dowelPlacement.item.GetComponent<Dowel>().captchaHash;
		}

		public void StopAlchemizing()
		{
			animator.SetBool("Adopted", false);
			dowelPlacement.collider.enabled = true;
		}

		public void Alchemize()
		{
			ItemType.itemTypes.TryGetValue(captchaHash, out ItemType itemType);
			if (!itemType)
				ItemType.itemTypes.TryGetValue(0, out itemType);
			Instantiate(itemType.prefab, spawnPoint.position, spawnPoint.rotation);
		}

		/// <summary>
		/// Called during the animation, lets you pick the dowel back up again
		/// </summary>
		public void PostAlchemize()
		{
			dowelPlacement.AllowOrphan();
			dowelPlacement.collider.enabled = false;
		}
	}
}