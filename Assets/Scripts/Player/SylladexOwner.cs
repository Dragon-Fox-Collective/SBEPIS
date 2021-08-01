using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using WrightWay.SBEPIS.Util;

namespace WrightWay.SBEPIS.Player
{
	public class SylladexOwner : MonoBehaviour
	{
		public Sylladex sylladex;
		public Transform sylladexParent;
		public ItemHolder itemHolder;

		private Item retrievingItem;
		private Quaternion sylladexRotTarget;
		private Quaternion sylladexRotDeriv;
		private Vector3 sylladexScaleTarget;
		private Vector3 sylladexScaleVel;

		private void Update()
		{
			sylladexParent.localRotation = QuaternionUtil.SmoothDamp(sylladexParent.localRotation, sylladexRotTarget, ref sylladexRotDeriv, 0.2f);
			sylladexParent.localScale = Vector3.SmoothDamp(sylladexParent.localScale, sylladexScaleTarget, ref sylladexScaleVel, 0.1f);

			if (retrievingItem)
				itemHolder.UpdateItem(retrievingItem);

		}

		private void OnCaptchalogue()
		{
			if (!itemHolder.heldItem)
				return;

			sylladex.Captchalogue(itemHolder.DropItem(), this);
		}

		private void OnRetrieve(InputValue value)
		{
			if (value.isPressed && (retrievingItem = sylladex.Display()))
			{
				retrievingItem.gameObject.SetActive(true);
				retrievingItem.transform.SetParent(null);
				retrievingItem.onPickUp.AddListener(Retrieve);
			}
			else if (retrievingItem)
			{
				retrievingItem.onPickUp.RemoveListener(Retrieve);
				retrievingItem.gameObject.SetActive(false);
				retrievingItem.transform.SetParent(sylladex.transform, this);
				retrievingItem = null;
			}
		}

		private void Retrieve(ItemHolder holder)
		{
			sylladex.Retrieve();
			retrievingItem.transform.SetParent(null);
			retrievingItem.onPickUp.RemoveListener(Retrieve);
			retrievingItem = null;
		}

		private void OnViewSylladex(InputValue value)
		{
			if (value.isPressed)
			{
				sylladexRotTarget = Quaternion.identity;
				sylladexScaleTarget = Vector3.one;
			}
			else
			{
				sylladexRotTarget = Quaternion.AngleAxis(-90, Vector3.up);
				sylladexScaleTarget = Vector3.zero;
			}
		}
	}
}