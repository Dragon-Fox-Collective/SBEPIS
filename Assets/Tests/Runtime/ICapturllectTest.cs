using System.Collections;

namespace SBEPIS.Tests
{
	public interface ICapturllectTest
	{
		public IEnumerator CapturllectSummonsDeque();
		public IEnumerator CapturllectDesummonsDeque();
		public IEnumerator CapturllectCapturllectsItem_WhenHoldingItem();
		public IEnumerator CapturllectFetchesItem_WhenHoldingCard();
	}
}
