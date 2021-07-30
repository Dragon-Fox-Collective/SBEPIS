using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace WrightWay.SBEPIS
{
	public class PunchDesignix : MonoBehaviour
	{
		public PlacementHelper punchPlacement, keyboardPlacement;
		public Animator animator;

		public TextMeshProUGUI punchPanel;

		/// <summary>
		/// Called when a card is inserted, starts the animation
		/// </summary>
		public void StartPunching()
		{
			animator.SetBool("Adopted", true);
		}

		/// <summary>
		/// Called when the card is removed, resets the animation
		/// </summary>
		public void StopPunching()
		{
			animator.SetBool("Adopted", false);
		}

		/// <summary>
		/// Called during the animation, sets up all the final mathy bits to start lathing
		/// </summary>
		public void Punch()
		{
			if (keyboardPlacement.item && keyboardPlacement.item.GetComponent<CaptchalogueCard>().heldItem)
				punchPlacement.item.GetComponent<CaptchalogueCard>().Punch(punchPlacement.item.GetComponent<CaptchalogueCard>().punchedHash | keyboardPlacement.item.GetComponent<CaptchalogueCard>().heldItem.itemType.captchaHash);
		}

		public void PostPunch()
		{
			punchPlacement.AllowOrphan();
		}

		public void KeyboardCardIn()
		{
			keyboardPlacement.AllowOrphan();
			if (keyboardPlacement.item.GetComponent<CaptchalogueCard>().heldItem)
				punchPanel.text = ItemType.unhashCaptcha(keyboardPlacement.item.GetComponent<CaptchalogueCard>().heldItem.itemType.captchaHash);
			else
				punchPanel.text = "00000000";
			animator.SetBool("Code Entered", true);
		}

		public void KeyboardCardOut()
		{
			punchPanel.text = "";
			animator.SetBool("Code Entered", false);
		}
	}
}