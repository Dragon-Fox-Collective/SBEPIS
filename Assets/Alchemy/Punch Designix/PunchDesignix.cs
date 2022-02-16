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
		public ToggleButtonFlatscreen alphaButton;
		public ToggleButtonFlatscreen numericButton;

		public SkinnedMeshRenderer keyboardRenderer;
		public Material unlitMaterial;
		public Material litMaterial;
		public int alphaMaterialIndex;
		public int numericMaterialIndex;

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
			editingPlayer.SetPlayerMode(PlayerMode.Typing, cameraParent);
			punchPanel.Select();
		}

		public void GiveCamera()
		{
			if (editingPlayer)
			{
				editingPlayer.SetPlayerMode(PlayerMode.Gameplay);
				editingPlayer = null;
			}
		}
		
		public void OnSetPunchText(string text)
		{
			animator.SetBool("Code Entered", text.Length == 8);
		}

		public void SendKey(char key)
		{
			if (key == '_')
			{
				punchPanel.ProcessEvent(Event.KeyboardEvent("backspace"));
				punchPanel.ForceLabelUpdate();
			}
			else
			{
				Event keyEvent = Event.KeyboardEvent(key.ToString());
				keyEvent.character = key;
				if (char.IsLetter(key) && char.IsUpper(key))
					keyEvent.modifiers |= EventModifiers.Shift;
				punchPanel.ProcessEvent(keyEvent);
				punchPanel.ForceLabelUpdate();
			}
		}

		public void LightNumericKeys()
		{
			Material[] materials = keyboardRenderer.materials;
			materials[alphaMaterialIndex] = unlitMaterial;
			materials[numericMaterialIndex] = litMaterial;
			keyboardRenderer.materials = materials;
		}

		public void UnlightNumericKeys()
		{
			Material[] materials = keyboardRenderer.materials;
			materials[alphaMaterialIndex] = litMaterial;
			materials[numericMaterialIndex] = unlitMaterial;
			keyboardRenderer.materials = materials;
		}
	}
}