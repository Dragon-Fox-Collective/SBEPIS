using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WrightWay.SBEPIS.Player;

namespace WrightWay.SBEPIS.Devices
{
	public class PunchDesignix : MonoBehaviour
	{
		public PlacementHelper punchPlacement, keyboardPlacement;
		public Animator animator;
		public Transform cameraParent;

		public TMP_InputField punchPanel;

		private PlayerModeSwapper editingPlayer;

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
			if (animator.GetBool("Code Entered"))
			{
				punchPlacement.item.GetComponent<CaptchalogueCard>().Punch(punchPlacement.item.GetComponent<CaptchalogueCard>().punchedHash | Itemkind.hashCaptcha(punchPanel.text));
				punchPanel.text = "";
			}
		}

		public void PostPunch()
		{
			punchPlacement.AllowOrphan();
		}

		public void KeyboardCardIn()
		{
			keyboardPlacement.AllowOrphan();
		}

		public void TakeCamera(ItemHolder playerItemHolder)
		{
			editingPlayer = playerItemHolder.GetComponent<PlayerModeSwapper>();
			editingPlayer.SetPlayerMode(PlayerModeSwapper.PlayerMode.Keyboard, cameraParent);
			punchPanel.Select();
		}

		public void GiveCamera()
		{
			if (editingPlayer)
			{
				editingPlayer.SetPlayerMode(PlayerModeSwapper.PlayerMode.Normal);
				editingPlayer = null;
			}
		}
		
		public void SetPunchText(string text)
		{
			animator.SetBool("Code Entered", text.Length == 8);

			//while (punchPanel.textComponent.text.Length < 8)
			//	punchPanel.textComponent.text += " ";
		}
	}
}