using SBEPIS.Captchalogue;
using SBEPIS.Interaction;
using TMPro;
using UnityEngine;

namespace SBEPIS.Alchemy
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
			animator.SetBool("Card Inserted", true);
		}

		/// <summary>
		/// Called when the card is removed, resets the animation
		/// </summary>
		public void StopPunching()
		{
			animator.SetBool("Card Inserted", false);
		}

		/// <summary>
		/// Called during the animation, sets up all the final mathy bits to start lathing
		/// </summary>
		public void Punch()
		{
			if (animator.GetBool("Code Entered"))
			{
				punchPlacement.item.GetComponent<CaptchalogueCard>().Punch(punchPlacement.item.GetComponent<CaptchalogueCard>().punchedHash | CaptchaUtil.HashCaptcha(punchPanel.text));
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
			editingPlayer.SetPlayerMode(PlayerMode.Keyboard, cameraParent);
			punchPanel.Select();
		}

		public void GiveCamera()
		{
			if (editingPlayer)
			{
				editingPlayer.SetPlayerMode(PlayerMode.Normal);
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