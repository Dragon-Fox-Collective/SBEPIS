using SBEPIS.Alchemy;
using UnityEngine;

namespace SBEPIS.Captchalogue
{
	[CreateAssetMenu]
	public class Moduskind : Itemkind
	{
		public FetchModusType modusType;
		public Color mainColor, darkColor, textColor;
		public Texture2D icon;
	}
}