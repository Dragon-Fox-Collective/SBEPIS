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
		public Transform cameraParent;

		public TMP_InputField punchPanel;

		private Player editingPlayer;

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
				punchPlacement.item.GetComponent<CaptchalogueCard>().Punch(punchPlacement.item.GetComponent<CaptchalogueCard>().punchedHash | ItemType.hashCaptcha(punchPanel.text));
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

		public void TakeCamera(Player player)
		{
			editingPlayer = player;
			player.SetPlayerMode(Player.PlayerMode.Keyboard, cameraParent);
			punchPanel.Select();
		}

		public void GiveCamera()
		{
			if (editingPlayer)
			{
				editingPlayer.SetPlayerMode(Player.PlayerMode.Normal);
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