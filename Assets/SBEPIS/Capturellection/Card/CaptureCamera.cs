using System.Collections.Generic;
using KBCore.Refs;
using TMPro;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	/// <summary>
	/// The camera that takes pictures of items and captcha codes to paste on captchalogue cards.
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class CaptureCamera : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private new Camera camera;
		
		public TextMeshProUGUI codeBox;
		public Transform objectParent;
		public RectTransform stage;
		
		private static CaptureCamera instance;
		private static CaptureCamera Instance
		{
			get
			{
				if (!instance) instance = FindObjectOfType<CaptureCamera>();
				return instance;
			}
		}
		private readonly Dictionary<string, Texture2D> stringTextures = new();
		
		private Texture2D TakePictureOfObject(GameObject obj)
		{
			Transform oldParent = obj.transform.parent;
			bool wasActive = obj.activeSelf;
			
			obj.transform.SetParent(objectParent, false);
			obj.SetActive(true);
			
			Bounds bounds = new(obj.transform.position, Vector3.zero);
			foreach (Renderer objRenderer in obj.GetComponentsInChildren<Renderer>())
				bounds.Encapsulate(objRenderer.bounds);
			objectParent.position = stage.position + objectParent.position - bounds.center;
			stage.localScale = bounds.size.magnitude > 0
				? Mathf.Min(stage.rect.width / 2f / bounds.size.x, stage.rect.height / bounds.size.y) * Vector3.one
				: Vector3.one;
			
			Texture2D rtn = TakePicture();
			
			stage.localScale = Vector3.one;
			obj.transform.SetParent(oldParent, false);
			obj.SetActive(wasActive);

			return rtn;
		}
		
		private Texture2D TakePictureOfCode(string str)
		{
			codeBox.gameObject.SetActive(true);
			codeBox.text = str;
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
		
		public static Texture2D GetStringTexture(string str)
		{
			if (!Instance.stringTextures.ContainsKey(str))
				Instance.stringTextures.Add(str, Instance.TakePictureOfCode(str));
			
			Instance.stringTextures.TryGetValue(str, out Texture2D texture);
			return texture;
		}
		
		public static Texture2D GetObjectTexture(GameObject obj) => Instance.TakePictureOfObject(obj);
	}
}