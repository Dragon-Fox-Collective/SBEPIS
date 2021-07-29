using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WrightWay.SBEPIS
{
	public class PunchDesignix : MonoBehaviour
	{
		// TODO: Screens
		/* Screen 1 - 
		 * Screen 2 - Green/red indicator lights
		 * Screen 3 - Keypad
		 */

		public PlacementHelper punchPlacement, keyboardPlacement;
		public Animator animator;

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
			if (keyboardPlacement.item)
				punchPlacement.item.GetComponent<CaptchalogueCard>().Punch(punchPlacement.item.GetComponent<CaptchalogueCard>().punchedHash | keyboardPlacement.item.GetComponent<CaptchalogueCard>().heldItem.itemType.captchaHash);
		}

		public void PostPunch()
		{
			punchPlacement.AllowOrphan();
		}

		public void KeyboardCardIn()
		{
			keyboardPlacement.AllowOrphan();
		}
	}
}