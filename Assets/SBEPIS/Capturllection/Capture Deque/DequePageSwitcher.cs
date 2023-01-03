using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class DequePageSwitcher : MonoBehaviour
	{
		public DequePage dequePage;

		public void SwitchPage(CaptureDeque deque)
		{
			deque.diajector.StartAssembly(deque, dequePage);
		}
	}
}
