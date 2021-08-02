using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WrightWay.SBEPIS.Devices
{
	public class TotemLathe : MonoBehaviour
	{
		public PlacementHelper card1Placement, card2Placement, dowelPlacement;
		public Transform dowelReciever;
		public SkinnedMeshRenderer chisel;
		public Animator animator;
		public float latheProgress;

		public TextMeshProUGUI dowelPanel;
		public TextMeshProUGUI progressPanel;
		public TextMeshProUGUI unusedPanel;
		public TextMeshProUGUI cardPanel;
		public TextMeshProUGUI captchaPanel;

		private bool isWorking;
		private bool isLathing;
		private long captchaHash;
		private Dowel dowel;
		private float[] initialBlendShapeWeights = new float[8];

		private void Start()
		{
			CardOutFirst();
		}

		private void Update()
		{
			if (isLathing)
			{
				if (dowel)
					for (int i = 0; i < 8; i++)
					{
						long charIndex = (dowel.captchaHash >> 6 * i) & ((1L << 6) - 1);
						float sliceChiselPercent = charIndex / 63f;
						float startLatheProgress = 1f - sliceChiselPercent + initialBlendShapeWeights[i];
						dowel.renderer.SetBlendShapeWeight(i, Math.Max(latheProgress - startLatheProgress, initialBlendShapeWeights[i]) * 100);
					}
				progressPanel.text = (latheProgress * 100).ToString("000.") + "%";
				if (latheProgress == 1)
					isLathing = false;
			}
		}

		/// <summary>
		/// The mathy bits
		/// </summary>
		public void SetCaptchaHash()
		{
			if (!card1Placement.item)
				captchaHash = 0;
			else if (!card2Placement.item)
				captchaHash = card1Placement.item.GetComponent<CaptchalogueCard>().punchedHash;
			else
				captchaHash = card1Placement.item.GetComponent<CaptchalogueCard>().punchedHash & card2Placement.item.GetComponent<CaptchalogueCard>().punchedHash;
		}

		public void PlaceDowel()
		{
			dowel = dowelPlacement.item.GetComponent<Dowel>();
			dowelPlacement.AllowOrphan();
		}

		public void RemoveDowel()
		{
			dowel = null;
		}

		public void HitLever1()
		{
			if (isWorking)
				return;

			isWorking = true;
			animator.SetBool("Lever 1 Pulled", !animator.GetBool("Lever 1 Pulled"));
			if (animator.GetBool("Lever 1 Pulled"))
				if (dowel)
					dowelPlacement.DisallowOrphan();
				else
					dowelPlacement.isAdopting = false;
		}

		public void PostLever1()
		{
			if (dowel)
				dowelPlacement.AllowOrphan();
			else
				dowelPlacement.isAdopting = true;
		}

		public void HitLever2()
		{
			if (isWorking || (dowel && !animator.GetBool("Lever 1 Pulled")))
				return;

			isWorking = true;
			animator.SetBool("Lever 2 Pulled", !animator.GetBool("Lever 2 Pulled"));
			if (!animator.GetBool("Lever 1 Pulled"))
				dowelPlacement.isAdopting = false;
		}

		public void PostLever2()
		{
			if (!animator.GetBool("Lever 1 Pulled"))
				dowelPlacement.isAdopting = true;

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
			progressPanel.text = "000%";
			dowelPanel.text = "Insert dowel";
			UpdateCardPanel();
		}

		public void ResetLevers()
		{
			isWorking = false;
		}

		/// <summary>
		/// Called during the animation, sets up all the final mathy bits to start lathing
		/// </summary>
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
			string captchaCode = Itemkind.unhashCaptcha(captchaHash);
			for (int i = 0; i < 8; i++)
			{
				int charIndex = Array.IndexOf(Itemkind.hashCharacters, captchaCode[i]);
				chisel.SetBlendShapeWeight(i, (1f - charIndex / 63f) * 100);
			}
			isLathing = true;

			if (dowel)
			{
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
		}

		/// <summary>
		/// Starts the first card animation
		/// </summary>
		public void CardGoingInFirst()
		{
			animator.SetInteger("Cards", 1);
		}

		/// <summary>
		/// Enables the second card
		/// </summary>
		public void CardInFirst()
		{
			card1Placement.AllowOrphan();
			card2Placement.collider.enabled = true;
			SetCaptchaPanelText(card1Placement.item.GetComponent<CaptchalogueCard>().punchedHash, -1, -1);
			UpdateCardPanel();
		}

		/// <summary>
		/// Resets the first card animation
		/// </summary>
		public void CardOutFirst()
		{
			card2Placement.collider.enabled = false;
			animator.SetInteger("Cards", 0);
			SetCaptchaPanelText(-1, -1, -1);
			UpdateCardPanel();
		}

		/// <summary>
		/// Starts the second card animation
		/// </summary>
		public void CardGoingInSecond()
		{
			animator.SetInteger("Cards", 2);
		}

		/// <summary>
		/// Does mathy bits ig
		/// </summary>
		public void CardInSecond()
		{
			card1Placement.DisallowOrphan();
			card2Placement.AllowOrphan();
			animator.SetInteger("Cards", 2);
			SetCaptchaHash();
			SetCaptchaPanelText(card1Placement.item.GetComponent<CaptchalogueCard>().punchedHash, card2Placement.item.GetComponent<CaptchalogueCard>().punchedHash, captchaHash);
			UpdateCardPanel();
		}

		/// <summary>
		/// Resets the second card animation
		/// </summary>
		public void CardOutSecond()
		{
			card1Placement.AllowOrphan();
			animator.SetInteger("Cards", 1);
			SetCaptchaPanelText(card1Placement.item.GetComponent<CaptchalogueCard>().punchedHash, -1, -1);
			UpdateCardPanel();
		}

		public void HandOffTotem()
		{
			if (dowel)
				dowel.transform.SetParent(dowelReciever);
		}

		public void TakeBackTotem()
		{
			if (dowel)
				dowel.transform.SetParent(dowelPlacement.itemParent);
		}

		private void SetCaptchaPanelText(long hash1, long hash2, long hashRes)
		{
			string code1 = Itemkind.unhashCaptcha(hash1);
			string code2 = Itemkind.unhashCaptcha(hash2);
			string codeRes = Itemkind.unhashCaptcha(hashRes);
			captchaPanel.text = $"{code1 ?? "00000000"}\n&\n{code2 ?? "!!!!!!!!"}\n=\n{codeRes ?? code1 ?? "00000000"}";
		}

		private void UpdateCardPanel()
		{
			switch (animator.GetInteger("Cards"))
			{
				case 0:
					cardPanel.text = "Insert punched card";
					break;
				case 1:
					cardPanel.text = "Lathe dowel\n||\nInsert another card";
					break;
				case 2:
					cardPanel.text = "Lathe dowel";
					break;
			}
		}
	}
}