using System;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class Capturllector : MonoBehaviour
	{
		public CaptureHashable defaultCapturedItem;

		[NonSerialized]
		public CaptureHashable capturedItem;

		private void Awake()
		{
			if (defaultCapturedItem)
				Capturllect(defaultCapturedItem);
		}

		public void Capturllect(CaptureHashable item)
		{
			capturedItem = item;
			item.gameObject.SetActive(false);
		}
	}
}
