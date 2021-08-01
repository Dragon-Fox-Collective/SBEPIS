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

		/// <summary>
		/// Called when a dowel is inserted, starts the animation
		/// </summary>
		public void StartLathing()
		{
			animator.SetBool("Adopted", true);
			dowel = dowelPlacement.item.GetComponent<Dowel>();
			dowelPanel.text = "Please wait";
			cardPanel.text = "Please wait";
		}

		/// <summary>
		/// Called when the dowel is removed, resets the animation
		/// </summary>
		public void StopLathing()
		{
			animator.SetBool("Adopted", false);
			dowel = null;
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

		/// <summary>
		/// Called during the animation, lets you pick up/deposit cards after lathin
		/// </summary>
		public void PostLathe()
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
			progressPanel.text = "000%";
			dowelPanel.text = "Insert dowel";
			UpdateCardPanel();
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

		/// <summary>
		/// Transfers the dowel from the desk to the grabber
		/// </summary>
		public void HandOffTotem()
		{
			dowel.transform.SetParent(dowelReciever);
		}

		/// <summary>
		/// Transfers the dowel from the grabber to the desk
		/// </summary>
		public void TakeBackTotem()
		{
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