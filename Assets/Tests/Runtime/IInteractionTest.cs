using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
