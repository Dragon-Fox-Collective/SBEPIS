using TMPro;
using UnityEngine;

public class InfoPedistal : MonoBehaviour
{
	public Dowel dowel;
	public TextMeshProUGUI itemName;
	public TextMeshProUGUI captchaCode;

	private void OnCollisionEnter(Collision collision)
	{
		CaptchalogueCard collideCard = collision.gameObject.GetComponent<CaptchalogueCard>();
		if (collideCard)
		{
			dowel.captchaHash = collideCard.itemHash;

			ItemType.itemTypes.TryGetValue(collideCard.itemHash, out ItemType itemType);
			if (itemType)
			{
				itemName.text = itemType.itemName;
				captchaCode.text = itemType.captchaCode;
			}
			else
			{
				itemName.text = "Unexpected item in bagging area";
				captchaCode.text = "JDbSuprm";
			}
		}
	}
}
