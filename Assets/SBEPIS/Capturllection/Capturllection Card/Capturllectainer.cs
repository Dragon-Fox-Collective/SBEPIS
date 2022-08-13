using SBEPIS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Capturllection
{
	public class Capturllectainer : MonoBehaviour
	{
		public Item defaultCapturedItemPrefab;
		public bool isRetrievingAllowed = true;
		public List<Predicate<Capturllectainer>> retrievePredicates = new();

		public CaptureEvent onCapture = new(), onRetrieve = new();

		public bool canRetrieve => retrievePredicates.All(predicate => predicate.Invoke(this));
		public Capturllectable capturedItem { get; private set; }

		private void Awake()
		{
			if (defaultCapturedItemPrefab)
			{
				Capturllectable item = Instantiate(defaultCapturedItemPrefab.gameObject).GetComponent<Capturllectable>();
				Capture(item);
			}

			retrievePredicates.Add(container => isRetrievingAllowed);
		}

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
