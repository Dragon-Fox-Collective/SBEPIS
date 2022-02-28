using System.Collections;

namespace SBEPIS.Tests
{
	public interface IInteractionTest
	{
		public IEnumerator GrabGrabsGrabbables();
		public IEnumerator GrabLiftsGrabbables();
		public IEnumerator GrabbingSetsLayerToHeldItem();
		public IEnumerator UngrabbingSetsLayerToDefault();
	}
}
