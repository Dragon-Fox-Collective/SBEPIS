using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Item : MonoBehaviour
{
	public string captchaCode;

	public long captchaHash { get; private set; }

	private new Rigidbody rigidbody;

	private void Awake()
	{
		captchaHash = hashCaptcha(captchaCode);
		rigidbody = GetComponent<Rigidbody>();
	}

	private void Start()
	{

	}

	private void Update()
	{

	}

	private void OnMouseDrag()
	{
		float distanceToScreen = Camera.main.WorldToScreenPoint(transform.position).z;
		Vector3 newScreenPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToScreen));
		Vector3 velocity = rigidbody.velocity;
		transform.position = Vector3.SmoothDamp(transform.position, new Vector3(newScreenPosition.x, 2, newScreenPosition.z), ref velocity, 0.3f);
		rigidbody.velocity = velocity;
	}

	private void OnMouseUp()
	{
		
	}

	private static readonly char[] hashCharacters =
	{
		'0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
		'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
		'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
		'?', '!'
	};

	private static long hashCaptcha(string captchaCode)
	{
		if (captchaCode.Length != 8)
			throw new ArgumentException("Captcha code must have 8 characters", nameof(captchaCode));

		long hash = 0;
		for (int i = 0; i < captchaCode.Length; i++)
		{
			if (Array.IndexOf(hashCharacters, captchaCode[i]) == -1)
				throw new ArgumentException("Captcha code contains illegal characters", nameof(captchaCode));

			hash |= (1L << i * 6) * Array.IndexOf(hashCharacters, captchaCode[i]);
		}
		return hash;
	}
}
