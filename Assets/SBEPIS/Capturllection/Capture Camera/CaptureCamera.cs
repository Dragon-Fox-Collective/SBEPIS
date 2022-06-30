using SBEPIS.Bits;
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
		public Transform objectParent;
		public RectTransform stage;

		private new Camera camera;

		public static CaptureCamera instance;
		private static readonly Dictionary<BitSet, Texture2D> captureCodeTextures = new();

		private void Awake()
		{
			if (instance && instance != this)
				Destroy(this);
			instance = this;

			camera = GetComponent<Camera>();
		}

		public Texture2D TakePictureOfObject(GameObject obj)
		{
			Transform oldParent = obj.transform.parent;
			bool wasActive = obj.activeSelf;

			obj.transform.SetParent(objectParent, false);
			obj.SetActive(true);

			Bounds bounds = new(obj.transform.position, Vector3.zero);
			foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>())
				bounds.Encapsulate(renderer.bounds);
			objectParent.position = stage.position + objectParent.position - bounds.center;
			stage.localScale = Mathf.Min(stage.rect.width / 2f / bounds.size.x, stage.rect.height / bounds.size.y) * Vector3.one;

			Texture2D rtn = TakePicture();

			stage.localScale = Vector3.one;
			obj.transform.SetParent(oldParent, false);
			obj.SetActive(wasActive);

			return rtn;
		}

		private Texture2D TakePictureOfCode(BitSet bits)
		{
			codeBox.gameObject.SetActive(true);
			codeBox.text = bits.ToCode();
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
			Rect texRect = new(0, 0, 256, 256);
			Texture2D captchaTexture = new((int)texRect.width, (int)texRect.height, TextureFormat.RGBA32, false);
			RenderTexture renderTexture = new((int)texRect.width, (int)texRect.height, 32);

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
		public static Texture2D GetCaptureCodeTexture(BitSet bits)
		{
			if (!captureCodeTextures.ContainsKey(bits))
				captureCodeTextures.Add(bits, instance.TakePictureOfCode(bits));

			captureCodeTextures.TryGetValue(bits, out Texture2D texture);
			return texture;
		}
	}
}