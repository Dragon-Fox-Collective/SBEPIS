using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WrightWay.SBEPIS
{
	public class TotemLathe : MonoBehaviour
	{
		// TODO: Screens
		/* Screen 1 - Dowel placement alert
		 * Screen 2 - Power gauge
		 * Screen 3 - 
		 * Screen 4 - Card placement alert
		 * Screen 5 - Card captcha combob
		 */

		public PlacementHelper card1Placement, card2Placement, dowelPlacement;
		public Transform dowelReciever;
		public SkinnedMeshRenderer chisel;
		public Animator animator;
		public float latheProgress;

		public Renderer captchaPanel;

		private bool isLathing;
		private long captchaHash;
		private Dowel dowel;
		private float[] initialBlendShapeWeights = new float[8];

		private void Update()
		{
			if (isLathing)
			{
				for (int i = 0; i < 8; i++)
				{
					long charIndex = (dowel.captchaHash >> 6 * i) & ((1L << 6) - 1);
					float sliceChiselPercent = charIndex / 63f;
					float startLatheProgress = 1f - sliceChiselPercent + initialBlendShapeWeights[i];
					dowel.renderer.SetBlendShapeWeight(i, Math.Max(latheProgress - startLatheProgress, initialBlendShapeWeights[i]) * 100);
				}
				if (latheProgress == 1)
					isLathing = false;
			}
		}

		public void SetCaptchaHash()
		{
			if (!card1Placement.item)
				captchaHash = 0;
			else if (!card2Placement.item)
				captchaHash = card1Placement.item.GetComponent<CaptchalogueCard>().punchedHash;
			else
				captchaHash = card1Placement.item.GetComponent<CaptchalogueCard>().punchedHash & card2Placement.item.GetComponent<CaptchalogueCard>().punchedHash;
		}

		public void StartLathing()
		{
			animator.SetBool("Adopted", true);
			dowel = dowelPlacement.item.GetComponent<Dowel>();
		}

		public void StopLathing()
		{
			animator.SetBool("Adopted", false);
			dowel = null;
		}

		public void Lathe()
		{
			if (card2Placement.item)
			{
				card2Placement.DisallowOrphan();
			}
			else if (card1Placement.item)
			{
				card1Placement.DisallowOrphan();
				card2Placement.isAdopting = false;
			}
			else
			{
				card1Placement.isAdopting = false;
			}

			SetCaptchaHash();
			string captchaCode = ItemType.unhashCaptcha(captchaHash);
			for (int i = 0; i < 8; i++)
			{
				int charIndex = Array.IndexOf(ItemType.hashCharacters, captchaCode[i]);
				chisel.SetBlendShapeWeight(i, (1f - charIndex / 63f) * 100);
			}
			isLathing = true;

			long greaterCaptchaHash = 0;
			for (int i = 0; i < 8; i++)
			{
				initialBlendShapeWeights[i] = dowel.renderer.GetBlendShapeWeight(i) / 100f;

				long initialCharIndex = (dowel.captchaHash >> 6 * i) & ((1L << 6) - 1);
				long chiselCharIndex = (captchaHash >> 6 * i) & ((1L << 6) - 1);
				greaterCaptchaHash |= (1L << i * 6) * Math.Max(initialCharIndex, chiselCharIndex);
			}
			dowel.captchaHash = greaterCaptchaHash;
		}

		public void Unlathe()
		{
			dowelPlacement.AllowOrphan();
			if (card2Placement.item)
			{
				card2Placement.AllowOrphan();
			}
			else if (card1Placement.item)
			{
				card1Placement.AllowOrphan();
				card2Placement.isAdopting = true;
			}
			else
			{
				card1Placement.isAdopting = true;
			}
		}

		public void CardGoingInFirst()
		{
			animator.SetInteger("Cards", 1);
		}

		public void CardInFirst()
		{
			card1Placement.AllowOrphan();
			card2Placement.collider.enabled = true;
			captchaPanel.material.SetTexture("Captcha_1", ItemType.GetCaptchaTexture(card1Placement.item.GetComponent<CaptchalogueCard>().punchedHash));
		}

		public void CardOutFirst()
		{
			card2Placement.collider.enabled = false;
			animator.SetInteger("Cards", 0);
			captchaPanel.material.SetTexture("Captcha_1", null);
		}

		public void CardGoingInSecond()
		{
			animator.SetInteger("Cards", 2);
		}

		public void CardInSecond()
		{
			card1Placement.DisallowOrphan();
			card2Placement.AllowOrphan();
			animator.SetInteger("Cards", 2);
			captchaPanel.material.SetTexture("Captcha_2", ItemType.GetCaptchaTexture(card2Placement.item.GetComponent<CaptchalogueCard>().punchedHash));
			SetCaptchaHash();
			captchaPanel.material.SetTexture("Captcha_Result", ItemType.GetCaptchaTexture(captchaHash));
		}

		public void CardOutSecond()
		{
			card1Placement.AllowOrphan();
			animator.SetInteger("Cards", 1);
			captchaPanel.material.SetTexture("Captcha_2", null);
			captchaPanel.material.SetTexture("Captcha_Result", null);
		}

		public void HandOffTotem()
		{
			dowel.transform.SetParent(dowelReciever);
		}

		public void TakeBackTotem()
		{
			dowel.transform.SetParent(dowelPlacement.itemParent);
		}
	}
}