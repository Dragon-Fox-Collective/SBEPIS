using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Camera))]
public class Captcharoid : MonoBehaviour
{
	public TextMeshProUGUI code;

	private new Camera camera;

	private void Awake()
	{
		camera = GetComponent<Camera>();
	}

	public Texture2D Captcha(long captchaHash)
	{
		Rect texRect = new Rect(0, 0, 256, 256);
		Texture2D captchaTexture = new Texture2D((int) texRect.width, (int) texRect.height, TextureFormat.RGBA32, false);
		RenderTexture renderTexture = new RenderTexture((int) texRect.width, (int) texRect.height, 32);

		code.text = ItemType.unhashCaptcha(captchaHash);

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
