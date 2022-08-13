using SBEPIS.Items;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public class Capturllectainer : MonoBehaviour
	{
		public Item defaultCapturedItemPrefab;

		public Capturllectable capturedItem { get; private set; }

		public UnityEvent<Capturllectable> onCapture = new(), onRetrieve = new();

		private void Awake()
		{
			if (defaultCapturedItemPrefab)
			{
				Capturllectable item = Instantiate(defaultCapturedItemPrefab.gameObject).GetComponent<Capturllectable>();
				Capture(item);
			}
		}

		// TODO: Let the player capturllect piles of things somehow
		public void Capture(Capturllectable item)
		{
			if (capturedItem || !item)
				return;

			capturedItem = item;
			item.gameObject.SetActive(false);
			item.transform.parent = transform;
			onCapture?.Invoke(item);
		}

		public Capturllectable Retrieve()
		{
			if (!capturedItem)
				return null;

			Capturllectable item = capturedItem;
			capturedItem = null;
			item.gameObject.SetActive(true);
			item.transform.parent = null;
			onRetrieve?.Invoke(item);
			return item;
		}
	}
}
