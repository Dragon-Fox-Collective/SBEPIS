using SBEPIS.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturllection
{
	public class Capturellectainer : MonoBehaviour
	{
		public Item defaultCapturedItemPrefab;
		[FormerlySerializedAs("isRetrievingAllowed")]
		public bool isFetchingAllowed = true;
		public List<Predicate<Capturellectainer>> fetchPredicates = new();
		
		public CaptureEvent onCapture = new();
		[FormerlySerializedAs("onRetrieve")]
		public CaptureEvent onFetch = new();
		
		public bool canFetch => fetchPredicates.All(predicate => predicate.Invoke(this));
		public Capturllectable capturedItem { get; private set; }
		public bool hasCapturedItem => capturedItem;
		public bool isEmpty => !hasCapturedItem;

		private string originalName;
		
		private void Awake()
		{
			if (defaultCapturedItemPrefab)
			{
				Capturllectable item = Instantiate(defaultCapturedItemPrefab.gameObject).GetComponent<Capturllectable>();
				Capture(item);
			}
			
			fetchPredicates.Add(_ => isFetchingAllowed);
		}

		public void Capture(Capturllectable item)
		{
			if (!item)
				return;
			if (hasCapturedItem)
				Fetch();
			
			capturedItem = item;
			originalName = name;
			name += $" ({item})";
			item.gameObject.SetActive(false);
			item.transform.SetParent(transform);
			onCapture.Invoke(this, item);
		}
		
		public Capturllectable Fetch()
		{
			if (!hasCapturedItem)
				return null;
			
			Capturllectable item = capturedItem;
			capturedItem = null;
			name = originalName;
			item.gameObject.SetActive(true);
			item.transform.SetParent(null);
			onFetch.Invoke(this, item);
			return item;
		}
	}
	
	[Serializable]
	public class CaptureEvent : UnityEvent<Capturellectainer, Capturllectable> { }
}
