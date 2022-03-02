using SBEPIS.Alchemy;
using SBEPIS.Interaction;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SBEPIS.Captchalogue
{
	public class Sylladex : MonoBehaviour
	{
		[SerializeField]
		private SylladexOwner owner;
		[SerializeField]
		private Animator animator;
		[SerializeField]
		private float ejectionSpeed = 6f;
		[SerializeField]
		private int maxCards = 16;
		[SerializeField]
		private Transform frontCardParent, backCardParent, modusParent;
		[SerializeField]
		private Renderer[] renderers;
		[SerializeField]
		private Material captchaMaterial;
		[SerializeField]
		private Material colorMaterial;
		[SerializeField]
		private Material panelMaterial;
		[SerializeField]
		private Material glassMaterial;
		[SerializeField]
		private int[] panelMaterialIndecies;
		[SerializeField]
		private Transform cardIconParent;
		[SerializeField]
		private RawImage cardIconPrefab;
		[SerializeField]
		private RawImage darkBar;
		[SerializeField]
		private TextMeshProUGUI title;
		[SerializeField]
		private Moduskind defaultModus;

		public bool isWorking => animator.GetInteger("Front Card State") != 0 || animator.GetInteger("Back Card State") != 0 || animator.GetBool("Syncing Cartridge") || animator.GetInteger("Captchalogue State") != 0;
		private FetchModus modus;
		private Moduskind moduskind;
		private Cartridge syncingCart;
		private Color color;

		private void Start()
		{
			SetModus(defaultModus);
			SetPanelMaterial(0);
		}

		public void StartCaptchaloguing()
		{
			animator.SetInteger("Captchalogue State", 1);
		}

		public void OnCaptchalogue()
		{
			owner.CastCaptchalogue();
		}

		public void Captchalogue(Item item)
		{
			Eject(modus.Insert(item));
			UpdateDisplay();
		}

		public void OnFinishCaptchalogueingAndFetching()
		{
			animator.SetInteger("Captchalogue State", 0);
		}

		public void StartFetching()
		{
			animator.SetInteger("Captchalogue State", -1);
		}

		public void OnFetch()
		{
			owner.Fetch();
		}

		private void UpdateDisplay()
		{
			if (modus.cards.Count == 0)
				return;

			foreach (Transform oldDisplay in frontCardParent)
			{
				oldDisplay.gameObject.SetActive(false);
				oldDisplay.SetParent(transform);
			}

			CaptchalogueCard newDisplay = modus.Display();
			newDisplay.gameObject.SetActive(true);
			newDisplay.transform.SetParent(frontCardParent);
			newDisplay.transform.localPosition = Vector3.zero;
			newDisplay.transform.localRotation = Quaternion.identity;

			UpdateCaptcha();
			UpdateCardsDisplay();
		}

		private void UpdateCaptcha()
		{
			CaptchalogueCard displayCard = modus.Display();
			CaptchalogueCard.UpdateMaterials(displayCard && displayCard.heldItem ? displayCard.heldItem.itemkind.captchaHash : 0, null, Color.black, renderers, captchaMaterial, null, null);
		}

		private void UpdateCardsDisplay()
		{
			foreach (Transform child in cardIconParent.transform)
			{
				foreach (Transform child2 in child)
					Destroy(child2.GetComponent<RawImage>().texture);
				Destroy(child.gameObject);
			}

			int i = 0;
			foreach (CaptchalogueCard card in modus.cards)
			{
				RawImage cardIcon = Instantiate(cardIconPrefab, cardIconParent.transform);
				cardIcon.transform.localPosition = Vector3.left * 0.1f + Vector3.right * 0.07f * (i % 4) + Vector3.up * 0.11f + Vector3.down * 0.09f * (i / 4);
				foreach (Transform child in cardIcon.transform)
				{
					RawImage childImage = child.GetComponent<RawImage>();
					if (card.heldItem)
					{
						//childImage.texture = GameManager.instance.captcharoid.Captcha(card.heldItem);
						childImage.color = Color.white;
					}
				}
				i++;
			}
		}

		public Item Display()
		{
			CaptchalogueCard card = modus.Display();
			if (card)
				return card.heldItem;
			return null;
		}

		public Item Fetch()
		{
			Item item = modus.Fetch();
			UpdateDisplay();
			return item;
		}

		private void Eject(Item item)
		{
			if (!item)
				return;

			item.gameObject.SetActive(true);
			item.transform.SetParent(null);
			Quaternion ejectionAngle = Quaternion.AngleAxis(UnityEngine.Random.value * 360, owner.transform.up);
			item.transform.position = owner.transform.position + ejectionAngle * Vector3.forward;
			item.transform.rotation = ejectionAngle;
			item.rigidbody.velocity = ejectionAngle * Vector3.forward * ejectionSpeed;
			item.rigidbody.angularVelocity = Vector3.zero;
		}
		
		public void InsertCard(CaptchalogueCard card)
		{
			if (modus.cards.Count >= maxCards)
				return;

			if (InsertInBack(card))
			{
				card.transform.SetParent(backCardParent);
				animator.SetInteger("Back Card State", 1);
			}
			else
			{
				foreach (Transform oldDisplay in frontCardParent)
				{
					oldDisplay.GetComponent<CaptchalogueCard>().UpdateMaterials(moduskind);
					oldDisplay.SetParent(backCardParent);
					oldDisplay.localPosition = Vector3.zero;
					oldDisplay.localRotation = Quaternion.identity;
				}

				card.transform.SetParent(frontCardParent);
				animator.SetInteger("Front Card State", 1);
			}

			card.transform.localPosition = Vector3.zero;
			card.transform.localRotation = Quaternion.identity;
			card.rigidbody.Disable();
			modus.InsertCard(card);
		}

		private bool InsertInBack(CaptchalogueCard card) => modus.cards.Count != 0 && modus.Display().heldItem && (!card.heldItem || modus.flippedInsert);

		public CaptchalogueCard RetrieveCard()
		{
			if (modus.cards.Count == 0)
				return null;

			CaptchalogueCard card = modus.RetrieveCard();
			card.UpdateMaterials(moduskind);

			if (modus.flippedRetrieve)
			{
				foreach (Transform oldDisplay in frontCardParent) // Largely affects blank card order
				{
					oldDisplay.gameObject.SetActive(false);
					oldDisplay.transform.SetParent(transform);
				}

				CaptchalogueCard newDisplay = modus.Display();
				if (newDisplay)
				{
					newDisplay.UpdateMaterials(moduskind);
					newDisplay.gameObject.SetActive(true);
					newDisplay.transform.SetParent(backCardParent);
					newDisplay.transform.localPosition = Vector3.zero;
					newDisplay.transform.localRotation = Quaternion.identity;
				}

				card.gameObject.SetActive(true);
				card.transform.SetParent(frontCardParent);
				animator.SetInteger("Front Card State", -1);
			}
			else
			{
				card.transform.SetParent(backCardParent);
				animator.SetInteger("Back Card State", -1);
			}

			card.transform.localPosition = Vector3.zero;
			card.transform.localRotation = Quaternion.identity;
			return card;
		}

		public void OnFinishDepositingFrontCard()
		{
			animator.SetInteger("Front Card State", 0);
			foreach (Transform child in backCardParent)
			{
				child.gameObject.SetActive(false);
				child.SetParent(transform);
			}
			UpdateDisplay();
		}

		public void OnFinishWithdrawingFrontCard()
		{
			animator.SetInteger("Front Card State", 0);
			foreach (Transform child in frontCardParent)
			{
				child.SetParent(null);
				child.GetComponent<CaptchalogueCard>().rigidbody.Enable();
			}
			foreach (Transform child in backCardParent)
			{
				child.SetParent(frontCardParent, false);
			}
			UpdateDisplay();
		}

		public void OnFinishDepositingBackCard()
		{
			animator.SetInteger("Back Card State", 0);
			foreach (Transform child in backCardParent)
			{
				child.gameObject.SetActive(false);
				child.SetParent(transform);
			}
		}

		public void OnFinishWithdrawingBackCard()
		{
			animator.SetInteger("Back Card State", 0);
			foreach (Transform child in backCardParent)
			{
				child.SetParent(null);
				child.GetComponent<CaptchalogueCard>().rigidbody.Enable();
			}
		}

		public void StartSyncingCartridge(Cartridge syncingCart)
		{
			this.syncingCart = syncingCart;
			syncingCart.transform.SetParent(modusParent);
			syncingCart.transform.localPosition = Vector3.zero;
			syncingCart.transform.localRotation = Quaternion.identity;
			syncingCart.rigidbody.Disable();
			animator.SetBool("Syncing Cartridge", true);
		}

		public void OnSyncCartridge()
		{
			SetModus(syncingCart.modus);
		}

		private void SetModus(Moduskind modus)
		{
			this.moduskind = modus;
			FetchModus.fetchModi.TryGetValue(modus.modusType, out Type modusType);
			this.modus = (FetchModus) Activator.CreateInstance(modusType, this.modus);
			UpdateMaterials();
			UpdateDisplay();
		}

		private void UpdateMaterials()
		{
			CaptchalogueCard.UpdateMaterials(0, null, color = moduskind.mainColor, renderers, null, null, colorMaterial);
			CaptchalogueCard.UpdateMaterials(0, null, color, renderers, null, null, panelMaterial);
			darkBar.color = moduskind.darkColor;
			title.color = moduskind.textColor;
		}

		public void OnFinishSyncingCartridge()
		{
			syncingCart.transform.SetParent(null);
			syncingCart.rigidbody.Enable();
			syncingCart = null;
			animator.SetBool("Syncing Cartridge", false);
		}

		public void StartTogglingPanel()
		{
			animator.SetBool("Litty", !animator.GetBool("Litty"));
		}

		public void SetPanelMaterial(int litty)
		{
			foreach (int mati in panelMaterialIndecies)
			{
				Material[] materials = renderers[0].materials;
				materials[mati] = litty > 0 ? panelMaterial : glassMaterial;
				renderers[0].materials = materials;
			}
			if (litty > 0)
				CaptchalogueCard.UpdateMaterials(0, null, color, renderers, null, null, panelMaterial);
		}
	}
}