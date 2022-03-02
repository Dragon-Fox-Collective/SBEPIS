using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	/// <summary>
	/// The camera that takes pictures of items and captcha codes to paste on captchalogue cards.
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class CaptureCamera : MonoBehaviour
	{
		public TextMeshProUGUI codeBox;

		private new Camera camera;

		public static CaptureCamera instance;
		private static readonly Dictionary<long, Texture2D> captureCodeTextures = new Dictionary<long, Texture2D>();

		private void Awake()
		{
			if (instance && instance != this)
				Destroy(gameObject);
			else
				instance = this;

			camera = GetComponent<Camera>();
		}

		public Texture2D TakePictureOfObject(CaptureCamerable camerable)
		{
			Transform oldParent = camerable.transform.parent;
			Vector3 oldPosition = camerable.transform.localPosition;
			Quaternion oldRotation = camerable.transform.localRotation;
			Vector3 oldScale = camerable.transform.localScale;
			bool wasActive = camerable.gameObject.activeSelf;

			camerable.transform.parent = codeBox.transform.parent;
			camerable.transform.localPosition = camerable.location;
			camerable.transform.localRotation = Quaternion.Euler(0, 180, 0) * camerable.rotation;
			camerable.transform.localScale = Vector3.one * camerable.scale;
			if (!wasActive) camerable.gameObject.SetActive(true);

			camerable.prePicture.Invoke();
			Texture2D rtn = TakePicture();
			camerable.postPicture.Invoke();

			camerable.transform.parent = oldParent;
			camerable.transform.localPosition = oldPosition;
			camerable.transform.localRotation = oldRotation;
			camerable.transform.localScale = oldScale;
			if (!wasActive) camerable.gameObject.SetActive(false);

			return rtn;
		}

		private Texture2D TakePictureOfCode(long captchaHash)
		{
			codeBox.gameObject.SetActive(true);
			codeBox.text = CaptureCodeUtils.UnhashCaptureHash(captchaHash);
			Texture2D rtn = TakePicture();
			codeBox.gameObject.SetActive(false);
			return rtn;
		}

		/// <summary>
		/// Takes a picture of a thing
		/// </summary>
		/// <returns>A Texture2D of the thing</returns>
		private Texture2D TakePicture()
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

		/// <summary>
		/// Look up or generate a Texture2D of a captcha code string
		/// </summary>
		public static Texture2D GetCaptureCodeTexture(long captureHash)
		{
			if (!captureCodeTextures.ContainsKey(captureHash))
				captureCodeTextures.Add(captureHash, instance.TakePictureOfCode(captureHash));

			captureCodeTextures.TryGetValue(captureHash, out Texture2D texture);
			return texture;
		}
	}
}