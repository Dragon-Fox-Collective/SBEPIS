using System.Collections.Generic;
using System.Linq;

namespace SBEPIS.Capturellection
{
	public class ProxyCaptureContainer : CaptureContainer
	{
		public CaptureEvent onCapture = new();
		public CaptureEvent onFetch = new();
		
		public CaptureContainer RealContainer { get; set; }
		public List<ProxyCaptureContainer> OtherProxies { get; set; } = new();
		
		public override void Capture(Capturellectable item)
		{
			if (!item)
				return;
			if (HasCapturedItem)
				Fetch();
			
			CaptureProxy(item);
			foreach (ProxyCaptureContainer otherProxy in OtherProxies.Where(proxy => proxy != this))
				otherProxy.CaptureProxy(item);
			
			RealContainer.Capture(item);
		}
		
		private void CaptureProxy(Capturellectable item)
		{
			CapturedItem = item;
			onCapture.Invoke(this, item);
		}
		
		public override Capturellectable Fetch()
		{
			if (!HasCapturedItem)
				return null;
			
			FetchProxy();
			foreach (ProxyCaptureContainer otherProxy in OtherProxies.Where(proxy => proxy != this))
				otherProxy.FetchProxy();
			
			return RealContainer.Fetch();
		}
		
		private void FetchProxy()
		{
			Capturellectable item = CapturedItem;
			CapturedItem = null;
			onFetch.Invoke(this, item);
		}
	}
}
