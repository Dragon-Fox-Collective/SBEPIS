using System;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class Capturllector : MonoBehaviour
	{
		public Transform defaultCapturedItem;

		[NonSerialized]
		public Transform capturedItem;

		private void Awake()
		{
			if (defaultCapturedItem)
				Capturllect(defaultCapturedItem);
		}

		public void Capturllect(Transform item)
		{
			capturedItem = item;
			item.gameObject.SetActive(false);
		}
	}
}
