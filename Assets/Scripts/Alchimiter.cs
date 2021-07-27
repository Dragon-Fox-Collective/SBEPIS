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

		public void StartAlchemizing()
		{
			animator.SetBool("Adopted", true);
		}

		public void Alchemize()
		{
			ItemType.itemTypes.TryGetValue(dowelPlacement.item.GetComponent<Dowel>().captchaHash, out ItemType itemType);
			if (!itemType)
				ItemType.itemTypes.TryGetValue(0, out itemType);
			Instantiate(itemType.prefab, spawnPoint.position, spawnPoint.rotation);
		}

		public void AllowOrphan()
		{
			dowelPlacement.AllowOrphan();
		}
	}
}