using SBEPIS.Items;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public class Capturllectainer : MonoBehaviour
	{
		public Item defaultCapturedItemPrefab;
		public bool canRetrieve = true;

		public CaptureEvent onCapture = new(), onRetrieve = new();

		public Capturllectable capturedItem { get; private set; }

		private void Awake()
		{
			if (defaultCapturedItemPrefab)
			{
				Capturllectable item = Instantiate(defaultCapturedItemPrefab.gameObject).GetComponent<Capturllectable>();
				Capture(item);
			}

			DequeStorable card = GetComponent<DequeStorable>();
			if (card)
				card.storePredicates.Add(() => capturedItem);
		}

		// TODO: Let the player capturllect piles of things somehow
		public void Capture(Capturllectable item)
		{
			if (capturedItem || !item)
				return;

			capturedItem = item;
			item.gameObject.SetActive(false);
			item.transform.parent = transform;
			onCapture.Invoke(this, item);
		}

		public Capturllectable Retrieve()
		{
			if (!capturedItem)
				return null;

			Capturllectable item = capturedItem;
			capturedItem = null;
			item.gameObject.SetActive(true);
			item.transform.parent = null;
			onRetrieve.Invoke(this, item);
			return item;
		}
	}

	[Serializable]
	public class CaptureEvent : UnityEvent<Capturllectainer, Capturllectable> { }
}
