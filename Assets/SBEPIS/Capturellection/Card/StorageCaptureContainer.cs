namespace SBEPIS.Capturellection
{
	public class StorageCaptureContainer : CaptureContainer
	{
		public CaptureEvent onCapture = new();
		public CaptureEvent onFetch = new();

		private Capturellectable capturedItem;
		public override Capturellectable CapturedItem => capturedItem;
		
		private string originalName;
		
		public override void Capture(Capturellectable item)
		{
			if (!item)
				return;
			if (HasCapturedItem)
				Fetch();
			
			capturedItem = item;
			originalName = name;
			name += $" ({item.name})";
			item.gameObject.SetActive(false);
			item.transform.SetParent(transform);
			onCapture.Invoke(this, item);
			item.onCapture.Invoke(this, item);
		}
		
		public override Capturellectable Fetch()
		{
			if (!HasCapturedItem)
				return null;
			
			Capturellectable item = CapturedItem;
			capturedItem = null;
			name = originalName;
			item.gameObject.SetActive(true);
			item.transform.SetParent(null);
			onFetch.Invoke(this, item);
			item.onFetch.Invoke(this, item);
			return item;
		}
	}
}
