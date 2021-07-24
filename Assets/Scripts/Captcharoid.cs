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

	public Texture2D Captcha(Item item)
	{
		Vector3 oldPosition = item.transform.position;
		Quaternion oldRotation = item.transform.rotation;
		bool wasActive = item.gameObject.activeSelf;

		item.transform.position = code.transform.position;
		item.transform.rotation = code.transform.rotation;
		if (!wasActive) item.gameObject.SetActive(true);

		Texture2D rtn = Captcha();

		item.transform.position = oldPosition;
		item.transform.rotation = oldRotation;
		if (!wasActive) item.gameObject.SetActive(false);
		
		return rtn;
	}

	public Texture2D Captcha(ItemType itemType)
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
		code.text = ItemType.unhashCaptcha(captchaHash);
		Texture2D rtn = Captcha();
		code.gameObject.SetActive(false);
		return rtn;
	}

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
