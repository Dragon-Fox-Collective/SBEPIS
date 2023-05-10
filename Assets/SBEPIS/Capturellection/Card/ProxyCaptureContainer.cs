using System.Collections.Generic;
using System.Linq;

namespace SBEPIS.Capturellection
{
	public class ProxyCaptureContainer : CaptureContainer
	{
		public CaptureContainer realContainer;
		public List<ProxyCaptureContainer> otherProxies = new();
		public CaptureEvent onCapture = new();
		public CaptureEvent onFetch = new();
		
		public override Capturellectable CapturedItem => realContainer.CapturedItem;
		
		private string originalName;
		
		public override void Capture(Capturellectable item)
		{
			if (!item)
				return;
			if (HasCapturedItem)
				Fetch();
			
			CaptureProxy(item);
			foreach (ProxyCaptureContainer otherProxy in otherProxies.Where(proxy => proxy != this))
				otherProxy.CaptureProxy(item);
			
			realContainer.Capture(item);
		}
		
		private void CaptureProxy(Capturellectable item)
		{
			originalName = name;
			name += $" ({item.name})";
			onCapture.Invoke(this, item);
		}
		
		public override Capturellectable Fetch()
		{
			if (!HasCapturedItem)
				return null;
			
			FetchProxy();
			foreach (ProxyCaptureContainer otherProxy in otherProxies.Where(proxy => proxy != this))
				otherProxy.FetchProxy();
			
			return realContainer.Fetch();
		}
		
		private void FetchProxy()
		{
			name += originalName;
			onFetch.Invoke(this, CapturedItem);
		}
	}
}
