using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class CaptureDeque : MonoBehaviour
	{
		public int Count => cards.Count;

		private List<Capturllector> cards = new List<Capturllector>();

		public Capturllector this[int i] => cards[i];
	}
}
