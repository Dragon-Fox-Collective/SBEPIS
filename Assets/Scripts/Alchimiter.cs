using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alchimiter : MonoBehaviour
{
	public PlacementHelper placement;
	public Transform spawnPoint;

	public void Alchemize()
	{
		ItemType.itemTypes.TryGetValue(placement.item.GetComponent<Dowel>().captchaHash, out ItemType itemType);
		if (!itemType)
			ItemType.itemTypes.TryGetValue(0, out itemType);
		Instantiate(itemType.prefab, spawnPoint.position, spawnPoint.rotation);
	}
}
