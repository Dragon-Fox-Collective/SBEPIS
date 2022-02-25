using SBEPIS.Alchemy;
using TMPro;
using UnityEngine;

namespace SBEPIS.Captchalogue
{
	/// <summary>
	/// The camera that takes pictures of items and captcha codes to paste on captchalogue cards.
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class Captcharoid : MonoBehaviour
	{
		public TextMeshProUGUI code;

		private new Camera camera;

		private void Awake()
		{
			camera = GetComponent<Camera>();
		}

		public Texture2D Captcha(Item item)
		{
			Transform oldParent = item.transform.parent;
			Vector3 oldPosition = item.transform.localPosition;
			Quaternion oldRotation = item.transform.localRotation;
			Vector3 oldScale = item.transform.localScale;
			bool wasActive = item.gameObject.activeSelf;

			item.transform.parent = code.transform.parent;
			item.transform.localPosition = Vector3.zero;
			item.transform.localRotation = Quaternion.Euler(0, 180, 0) * item.captchaRotation;
			item.transform.localScale = Vector3.one * item.captchaScale;
			if (!wasActive) item.gameObject.SetActive(true);

			item.preCaptcha.Invoke();
			Texture2D rtn = Captcha();
			item.postCaptcha.Invoke();

			item.transform.parent = oldParent;
			item.transform.localPosition = oldPosition;
			item.transform.localRotation = oldRotation;
			item.transform.localScale = oldScale;
			if (!wasActive) item.gameObject.SetActive(false);

			return rtn;
		}

		/// <summary>
		/// Deprecated or something
		/// </summary>
		public Texture2D Captcha(Itemkind itemType)
		{
			Item instance = Instantiate(itemType.prefab, code.transform.position, code.transform.rotation * Quaternion.Euler(-45, -45, 0));
			Texture2D rtn = Captcha();
			instance.gameObject.SetActive(false);
			Destroy(instance.gameObject);
			return rtn;
		}

		public Texture2D Captcha(long captchaHash)
		{
			code.gameObject.SetActive(true);
			code.text = CaptureCodeUtils.UnhashCaptcha(captchaHash);
			Texture2D rtn = Captcha();
			code.gameObject.SetActive(false);
			return rtn;
		}

		/// <summary>
		/// Takes a picture of a thing
		/// </summary>
		/// <returns>A Texture2D of the thing</returns>
		private Texture2D Captcha()
		{
			Rect texRect = new Rect(0, 0, 256, 256);
			Texture2D captchaTexture = new Texture2D((int)texRect.width, (int)texRect.height, TextureFormat.RGBA32, false);
			RenderTexture renderTexture = new RenderTexture((int)texRect.width, (int)texRect.height, 32);

			camera.targetTexture = renderTexture;
			camera.Render();
			camera.targetTexture = null;

			RenderTexture.active = renderTexture;
			captchaTexture.ReadPixels(texRect, 0, 0);
			captchaTexture.Apply();
			captchaTexture.wrapMode = TextureWrapMode.Clamp;
			RenderTexture.active = null;

			Destroy(renderTexture);

			return captchaTexture;
		}
	}
}