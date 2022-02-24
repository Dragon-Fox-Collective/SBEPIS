using SBEPIS.Captchalogue;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS
{
	public class GameManager : MonoBehaviour
	{
		public static GameManager instance { get; private set; }

		public Captcharoid captcharoid;

		private void Awake()
		{
			instance = this;
		}


		private static readonly Dictionary<long, Texture2D> captchaTextures = new Dictionary<long, Texture2D>();

		/// <summary>
		/// Look up or generate a Texture2D of a captcha code string
		/// </summary>
		public static Texture2D GetCaptchaTexture(long captchaHash)
		{
			if (!captchaTextures.ContainsKey(captchaHash))
				captchaTextures.Add(captchaHash, instance.captcharoid.Captcha(captchaHash));

			captchaTextures.TryGetValue(captchaHash, out Texture2D texture);
			return texture;
		}
	}
}