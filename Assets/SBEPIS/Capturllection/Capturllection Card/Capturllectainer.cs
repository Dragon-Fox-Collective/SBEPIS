using SBEPIS.Items;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public class Capturllectainer : MonoBehaviour
	{
		public Item defaultCapturedItem;

		public Item capturedItem { get; private set; }

		public UnityEvent<Item> onCapture = new(), onRetrieve = new();

		private void Awake()
		{
			if (defaultCapturedItem)
				Capture(defaultCapturedItem);
		}

		// TODO: Let the player capturllect piles of things somehow
		public void Capture(Item item)
		{
			if (capturedItem || !item)
				return;

			capturedItem = item;
			item.gameObject.SetActive(false);
			item.transform.parent = transform;
			onCapture?.Invoke(item);
		}

		public Item Retrieve()
		{
			if (!capturedItem)
				return null;

			Item item = capturedItem;
			capturedItem = null;
			item.gameObject.SetActive(true);
			item.transform.parent = null;
			onRetrieve?.Invoke(item);
			return item;
		}
	}
}
