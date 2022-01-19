using SBEPIS.Alchemy;
using SBEPIS.Captchalogue;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SBEPIS.Interaction
{
	public class SylladexOwner : MonoBehaviour
	{
		[SerializeField]
		private Sylladex sylladex;
		[SerializeField]
		private Transform sylladexParent;
		[SerializeField]
		private ItemHolder itemHolder;
		[SerializeField]
		private PlayerModeSwapper modeSwapper;
		[SerializeField]
		private PlayerInput playerInput;

		private bool _isViewing;
		private bool IsViewing
		{
			get => _isViewing;
			set
			{
				_isViewing = value;
				if (_isViewing)
				{
					playerInput.actions.FindAction("Pick Up").Disable();
					playerInput.actions.FindAction("Captchalogue Mode").Enable();
					playerInput.actions.FindAction("Toggle Sylladex Panel").Enable();
					sylladexParentRotTarget = Quaternion.identity;
					sylladexParentScaleTarget = Vector3.one;
				}
				else
				{
					playerInput.actions.FindAction("Pick Up").Enable();
					playerInput.actions.FindAction("Captchalogue Mode").Disable();
					playerInput.actions.FindAction("Toggle Sylladex Panel").Disable();
					sylladexParentRotTarget = Quaternion.Euler(0, -90, 0);
					sylladexParentScaleTarget = Vector3.zero;
				}
			}
		}
		private bool _canCaptchalogue;
		private bool CanCaptchalogue
		{
			get => _canCaptchalogue;
			set
			{
				_canCaptchalogue = value;
				if (_canCaptchalogue)
				{
					playerInput.actions.FindAction("Captchalogue").Enable();
					playerInput.actions.FindAction("Captchalogue Use").Enable();
					playerInput.actions.FindAction("Retrieve").Enable();
					sylladexPosTarget = new Vector3(0, -0.25f, 1);
					sylladexRotTarget = Quaternion.Euler(-75, 180, 0);
				}
				else
				{
					playerInput.actions.FindAction("Captchalogue").Disable();
					playerInput.actions.FindAction("Captchalogue Use").Disable();
					playerInput.actions.FindAction("Retrieve").Disable();
					sylladexPosTarget = new Vector3(0, 0, 1);
					sylladexRotTarget = Quaternion.Euler(0, 180, 0);
				}
			}
		}
		private Vector3 sylladexPosTarget;
		private Vector3 sylladexPosTargetVel;
		private Quaternion sylladexRotTarget;
		private Quaternion sylladexRotDeriv;
		private Quaternion sylladexParentRotTarget;
		private Quaternion sylladexParentRotDeriv;
		private Vector3 sylladexParentScaleTarget;
		private Vector3 sylladexParentScaleVel;

		private void Start()
		{
			ResetSylladexControls();
		}

		private void Update()
		{
			sylladex.transform.localPosition = Vector3.SmoothDamp(sylladex.transform.localPosition, sylladexPosTarget, ref sylladexPosTargetVel, 0.1f);
			sylladex.transform.localRotation = QuaternionUtil.SmoothDamp(sylladex.transform.localRotation, sylladexRotTarget, ref sylladexRotDeriv, 0.1f);
			sylladexParent.localRotation = QuaternionUtil.SmoothDamp(sylladexParent.localRotation, sylladexParentRotTarget, ref sylladexParentRotDeriv, 0.2f);
			sylladexParent.localScale = Vector3.SmoothDamp(sylladexParent.localScale, sylladexParentScaleTarget, ref sylladexParentScaleVel, 0.1f);
		}

		public void OnSetPlayerMode(PlayerMode mode)
		{
			if (mode == PlayerMode.Normal)
				ResetSylladexControls();
		}

		private void ResetSylladexControls()
		{
			IsViewing = false;
			CanCaptchalogue = false;
		}

		private void OnViewSylladex()
		{
			if (sylladex.isWorking)
				return;

			IsViewing = !IsViewing;
		}

		private void OnCaptchalogueMode(InputValue value)
		{
			CanCaptchalogue = value.isPressed;
		}

		private void OnCaptchalogue()
		{
			sylladex.StartCaptchaloguing();
		}

		public void Captchalogue()
		{
			Item hitItem;
			if (Physics.Raycast(itemHolder.camera.position, itemHolder.camera.forward, out RaycastHit captchaHit, itemHolder.maxDistance) && captchaHit.rigidbody && (hitItem = captchaHit.rigidbody.GetComponent<Item>()))
				sylladex.Captchalogue(hitItem);
		}

		private void OnRetrieve(InputValue value)
		{
			sylladex.StartRetrieving();
		}

		public void Retrieve()
		{
			Item retrievingItem = sylladex.Retrieve();
			if (retrievingItem)
			{
				retrievingItem.gameObject.SetActive(true);
				retrievingItem.transform.localPosition = Vector3.up;
				retrievingItem.transform.rotation = Quaternion.identity;
				retrievingItem.transform.SetParent(null);
			}
		}

		private void OnCaptchalogueUse()
		{
			if (sylladex.isWorking)
				return;

			if (Physics.Raycast(itemHolder.camera.position, itemHolder.camera.forward, out RaycastHit captchaHit, itemHolder.maxDistance) && captchaHit.rigidbody)
			{
				CaptchalogueCard hitCard = captchaHit.rigidbody.GetComponent<CaptchalogueCard>();
				if (hitCard && hitCard.punchedHash == 0)
				{
					sylladex.InsertCard(hitCard);
					return;
				}

				Cartridge hitCart = captchaHit.rigidbody.GetComponent<Cartridge>();
				if (hitCart)
				{
					sylladex.StartSyncingCartridge(hitCart);
					return;
				}
			}
		}

		private void OnCaptchaloguePrint()
		{
			if (sylladex.isWorking)
				return;

			sylladex.RetrieveCard();
		}

		private void OnFlipCard()
		{
			if (!IsViewing || CanCaptchalogue)
				return;

			if (sylladexRotTarget == Quaternion.identity)
				sylladexRotTarget = Quaternion.Euler(0, 180, 0);
			else
				sylladexRotTarget = Quaternion.identity;
		}

		private void OnToggleSylladexPanel()
		{
			sylladex.StartTogglingPanel();
		}
	}
}