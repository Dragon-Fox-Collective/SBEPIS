using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class CaptureDeque : MonoBehaviour
	{
		public int Count => cards.Count;

		private List<Capturllectainer> cards = new List<Capturllectainer>();

		public Capturllectainer this[int i] => cards[i];
	}
}
