using System;
using System.Collections.Generic;
using UnityEngine;
using WrightWay.SBEPIS.Modus;

namespace WrightWay.SBEPIS
{
	[CreateAssetMenu]
	public class Moduskind : Itemkind
	{
		public FetchModusType modusType;
		public Color mainColor, darkColor, textColor;
		public Texture2D icon;
	}
}