using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WrightWay.SBEPIS.Modus;
using WrightWay.SBEPIS.Player;

namespace WrightWay.SBEPIS
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
		private Transform insertParent, retrieveParent, modusParent;
		[SerializeField]
		private Renderer[] renderers;
		[SerializeField]
		private Material captchaMaterial;
		[SerializeField]
		private Material colorMaterial;

		public bool isWorking => animator.GetInteger("Insert State") != 0 || animator.GetInteger("Retrieve State") != 0 || animator.GetBool("Syncing Cartridge") || animator.GetInteger("Captchalogue State") != 0;
		private FetchModus modus;
		private Cartridge syncingCart;
		private Color color;

		private void Start()
		{
			modus = new StackModus(null);
			CaptchalogueCard.UpdateMaterials(0, null, color = new Color(255f/255f, 6f/255f, 125f/255f), renderers, captchaMaterial, null, colorMaterial);
		}

		public void StartCaptchaloguing()
		{
			animator.SetInteger("Captchalogue State", 1);
		}

		public void Captchalogue()
		{
			owner.Captchalogue();
		}

		public void Captchalogue(Item item)
		{
			Eject(modus.Insert(item));
			UpdateDisplay();
		}

		public void StopCaptchaloguing()
		{
			animator.SetInteger("Captchalogue State", 0);
		}

		public void StartRetrieving()
		{
			animator.SetInteger("Captchalogue State", -1);
		}

		public void Uncaptchalogue()
		{
			owner.Retrieve();
		}

		private void UpdateDisplay()
		{
			if (modus.cards.Count == 0)
				return;

			foreach (Transform oldDisplay in insertParent)
			{
				oldDisplay.gameObject.SetActive(false);
				oldDisplay.SetParent(transform);
			}

			CaptchalogueCard newDisplay = modus.Display();
			newDisplay.gameObject.SetActive(true);
			newDisplay.transform.SetParent(insertParent);
			newDisplay.transform.localPosition = Vector3.zero;
			newDisplay.transform.localRotation = Quaternion.identity;

			UpdateCaptcha();
		}

		private void UpdateCaptcha()
		{
			CaptchalogueCard card = modus.Display();
			CaptchalogueCard.UpdateMaterials(card && card.heldItem ? card.heldItem.itemkind.captchaHash : 0, null, Color.black, renderers, captchaMaterial, null, null);
		}

		public Item Display()
		{
			CaptchalogueCard card = modus.Display();
			if (card)
				return card.heldItem;
			return null;
		}

		public Item Retrieve()
		{
			Item item = modus.Retrieve();
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
			if (InsertInBack(card))
			{
				card.transform.SetParent(retrieveParent);
				animator.SetInteger("Retrieve State", 1);
			}
			else
			{
				foreach (Transform oldDisplay in insertParent)
				{
					oldDisplay.GetComponent<CaptchalogueCard>().UpdateMaterials(color);
					oldDisplay.SetParent(retrieveParent);
					oldDisplay.localPosition = Vector3.zero;
					oldDisplay.localRotation = Quaternion.identity;
				}

				card.transform.SetParent(insertParent);
				animator.SetInteger("Insert State", 1);
			}

			card.transform.localPosition = Vector3.zero;
			card.transform.localRotation = Quaternion.identity;
			Item.DisableRigidbody(card.rigidbody);
			modus.InsertCard(card);
		}

		private bool InsertInBack(CaptchalogueCard card) => modus.cards.Count != 0 && modus.Display().heldItem && (!card.heldItem || modus.flippedInsert);

		public CaptchalogueCard RetrieveCard()
		{
			if (modus.cards.Count == 0)
				return null;

			CaptchalogueCard card = modus.RetrieveCard();
			card.UpdateMaterials(color);

			if (modus.flippedRetrieve)
			{
				foreach (Transform oldDisplay in insertParent) // Largely affects blank card order
				{
					oldDisplay.gameObject.SetActive(false);
					oldDisplay.transform.SetParent(transform);
				}

				CaptchalogueCard newDisplay = modus.Display();
				if (newDisplay)
				{
					newDisplay.UpdateMaterials(color);
					newDisplay.gameObject.SetActive(true);
					newDisplay.transform.SetParent(retrieveParent);
					newDisplay.transform.localPosition = Vector3.zero;
					newDisplay.transform.localRotation = Quaternion.identity;
				}

				card.gameObject.SetActive(true);
				card.transform.SetParent(insertParent);
				animator.SetInteger("Insert State", -1);
			}
			else
			{
				card.transform.SetParent(retrieveParent);
				animator.SetInteger("Retrieve State", -1);
			}

			card.transform.localPosition = Vector3.zero;
			card.transform.localRotation = Quaternion.identity;
			return card;
		}

		public void StopInsertDepositing()
		{
			animator.SetInteger("Insert State", 0);
			foreach (Transform child in retrieveParent)
			{
				child.gameObject.SetActive(false);
				child.SetParent(transform);
			}
			UpdateCaptcha();
		}

		public void StopInsertWithdrawing()
		{
			animator.SetInteger("Insert State", 0);
			foreach (Transform child in insertParent)
			{
				child.SetParent(null);
				Item.EnableRigidbody(child.GetComponent<CaptchalogueCard>().rigidbody);
			}
			foreach (Transform child in retrieveParent)
			{
				child.SetParent(insertParent, false);
			}
			UpdateCaptcha();
		}

		public void StopRetrieveDepositing()
		{
			animator.SetInteger("Retrieve State", 0);
			foreach (Transform child in retrieveParent)
			{
				child.gameObject.SetActive(false);
				child.SetParent(transform);
			}
		}

		public void StopRetrieveWithdrawing()
		{
			animator.SetInteger("Retrieve State", 0);
			foreach (Transform child in retrieveParent)
			{
				child.SetParent(null);
				Item.EnableRigidbody(child.GetComponent<CaptchalogueCard>().rigidbody);
			}
		}

		public void StartSyncingCartridge(Cartridge syncingCart)
		{
			this.syncingCart = syncingCart;
			syncingCart.transform.SetParent(modusParent);
			syncingCart.transform.localPosition = Vector3.zero;
			syncingCart.transform.localRotation = Quaternion.identity;
			Item.DisableRigidbody(syncingCart.rigidbody);
			animator.SetBool("Syncing Cartridge", true);
		}

		public void SyncCartridge()
		{
			FetchModus.fetchModi.TryGetValue(syncingCart.modus, out Type modusType);
			modus = (FetchModus) Activator.CreateInstance(modusType, modus);
			CaptchalogueCard.UpdateMaterials(0, null, color = syncingCart.color, renderers, null, null, colorMaterial);
		}

		public void StopSyncingCartridge()
		{
			syncingCart.transform.SetParent(null);
			Item.EnableRigidbody(syncingCart.rigidbody);
			syncingCart = null;
			animator.SetBool("Syncing Cartridge", false);
		}
	}
}