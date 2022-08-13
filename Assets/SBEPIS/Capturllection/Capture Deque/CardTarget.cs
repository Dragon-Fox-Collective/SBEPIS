using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class CardTarget : MonoBehaviour
	{
		public DequeStorable card { get; set; }
		public bool isTemporary { get; set; }

		public DequeCardInfo dequeCardInfo;
	}
}
