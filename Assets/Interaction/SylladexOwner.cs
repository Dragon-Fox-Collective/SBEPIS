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
		private InputActionReference[] sylladexEnabledActions;
		[SerializeField]
		private InputActionReference[] sylladexDisabledActions;
		[SerializeField]
		private InputActionReference[] usingSylladexEnabledActions;
		[SerializeField]
		private InputActionReference[] usingSylladexDisabledActions;

		private bool _isViewing;
		private bool IsViewing
		{
			get => _isViewing;
			set
			{
				_isViewing = value;
				if (_isViewing)
				{
					foreach (InputActionReference action in sylladexEnabledActions)
						action.action.Enable();
					foreach (InputActionReference action in sylladexDisabledActions)
						action.action.Disable();
					sylladexParentRotTarget = Quaternion.identity;
					sylladexParentScaleTarget = Vector3.one;
				}
				else
				{
					foreach (InputActionReference action in sylladexEnabledActions)
						action.action.Disable();
					foreach (InputActionReference action in sylladexDisabledActions)
						action.action.Enable();
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
					foreach (InputActionReference action in usingSylladexEnabledActions)
						action.action.Enable();
					foreach (InputActionReference action in usingSylladexDisabledActions)
						action.action.Disable();
					sylladexPosTarget = new Vector3(0, -0.25f, 1);
					sylladexRotTarget = Quaternion.Euler(-75, 180, 0);
				}
				else
				{
					foreach (InputActionReference action in usingSylladexEnabledActions)
						action.action.Disable();
					foreach (InputActionReference action in usingSylladexDisabledActions)
						action.action.Enable();
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
			if (mode == PlayerMode.Gameplay)
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

		private void OnFetch(InputValue value)
		{
			sylladex.StartFetching();
		}

		public void Fetch()
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