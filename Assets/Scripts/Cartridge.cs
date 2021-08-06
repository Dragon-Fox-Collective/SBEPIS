using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using WrightWay.SBEPIS.Modus;

namespace WrightWay.SBEPIS
{
	[RequireComponent(typeof(Rigidbody))]
	public class Cartridge : MonoBehaviour
	{
		public FetchModusType modus;
		[SerializeField]
		private TextMeshProUGUI modusText;
		[SerializeField]
		private TextMeshProUGUI modusName;
		[SerializeField]
		private Renderer[] renderers;
		[SerializeField]
		private Material colorMaterial;
		public Color color;
		[SerializeField]
		private Texture2D icon;

		public new Rigidbody rigidbody { get; set; }

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
		}

		private void Start()
		{
			modusText.color = color;
			modusName.color = color;
			modusName.text = modus.ToString().ToLower();
			CaptchalogueCard.UpdateMaterials(0, icon, color, renderers, null, colorMaterial, colorMaterial);
		}
	}
}