using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WrightWay.SBEPIS.Devices
{
	public class Alchimiter : MonoBehaviour
	{
		public PlacementHelper dowelPlacement;
		public Transform spawnPoint;
		public Animator animator;
		public TextMeshProUGUI alchemizedCounter;
		public float delayBetweenAlchemizings;

		private long captchaHash;
		private int thingsToAlchemize = 1;
		private bool isAlchemizing;
		private float alchemizingTimer;

		private void Update()
		{
			if (isAlchemizing)
			{
				alchemizingTimer += Time.deltaTime;
				while (alchemizingTimer >= delayBetweenAlchemizings && thingsToAlchemize > 0)
				{
					alchemizingTimer -= delayBetweenAlchemizings;
					thingsToAlchemize--;
					UpdateAlchemizedText();

					Itemkind.itemkind.TryGetValue(captchaHash, out Itemkind itemType);
					if (!itemType)
						Itemkind.itemkind.TryGetValue(0, out itemType);
					Instantiate(itemType.prefab, spawnPoint.position, spawnPoint.rotation);
				}
				if (thingsToAlchemize == 0)
				{
					isAlchemizing = false;
					alchemizingTimer = 0;
					thingsToAlchemize = 1;
					UpdateAlchemizedText();
					animator.SetBool("Alchemizing", false);
					dowelPlacement.isAdopting = true;
				}
			}
		}

		public void TotemInserted()
		{
			dowelPlacement.AllowOrphan();
		}

		public void StartAlchemizing()
		{
			if (animator.GetBool("Alchemizing") || !dowelPlacement.item)
				return;

			animator.SetBool("Alchemizing", true);
			animator.SetTrigger("Alc Button");
			dowelPlacement.DisallowOrphan();
			captchaHash = dowelPlacement.item.GetComponent<Dowel>().captchaHash;
		}

		public void Alchemize()
		{
			isAlchemizing = true;
		}

		public void PostAlchemize()
		{
			dowelPlacement.AllowOrphan();
			dowelPlacement.isAdopting = false;
		}

		public void IncrementAlchemized()
		{
			if (animator.GetBool("Alchemizing"))
				return;
			thingsToAlchemize++;
			if (thingsToAlchemize > 999)
				thingsToAlchemize = 1;
			animator.SetTrigger("Inc Button");
			UpdateAlchemizedText();
		}

		public void DecrementAlchemized()
		{
			if (animator.GetBool("Alchemizing"))
				return;
			thingsToAlchemize--;
			if (thingsToAlchemize < 1)
				thingsToAlchemize = 999;
			animator.SetTrigger("Dec Button");
			UpdateAlchemizedText();
		}

		private void UpdateAlchemizedText()
		{
			alchemizedCounter.text = thingsToAlchemize.ToString("000.");
		}
	}
}