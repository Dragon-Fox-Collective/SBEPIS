using SBEPIS.Alchemy;
using SBEPIS.Bits;
using TMPro;
using UnityEngine;

namespace SBEPIS.Captchalogue
{
	[RequireComponent(typeof(Rigidbody))]
	[RequireComponent(typeof(Item))]
	public class Cartridge : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI modusText;
		[SerializeField]
		private TextMeshProUGUI modusName;
		[SerializeField]
		private Renderer[] renderers;
		[SerializeField]
		private Material colorMaterial;

		public new Rigidbody rigidbody { get; private set; }
		public Moduskind modus { get; private set; }

		private void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			modus = (Moduskind) GetComponent<Item>().itemkind;
		}

		private void Start()
		{
			modusText.color = modus.mainColor;
			modusName.color = modus.mainColor;
			modusName.text = modus.itemName.ToLower();
			CaptchalogueCard.UpdateMaterials(BitSet.Nothing, modus.icon, modus.mainColor, renderers, null, colorMaterial, colorMaterial);
		}
	}
}