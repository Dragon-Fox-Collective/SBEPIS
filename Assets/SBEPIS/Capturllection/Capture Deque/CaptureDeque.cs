using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using SBEPIS.Controller;

namespace SBEPIS.Capturllection
{
	[RequireComponent(typeof(Grabbable), typeof(DequeType))]
	public class CaptureDeque : MonoBehaviour
	{
		public Diajector diajector;
		public Transform cardStart;
		public Transform cardTarget;
		// public List<DequeStorable> cards = new(); // apparently all of the cards are just CardTargets in children of DequePage???

		public Grabbable grabbable { get; private set; }
		public DequeType dequeType { get; private set; }

		private void Awake()
		{
			grabbable = GetComponent<Grabbable>();
			dequeType = GetComponent<DequeType>();
		}

		public void ToggleDiajector()
		{
			if (diajector.gameObject.activeSelf)
				DesummonDiajector();
			else
				SummonDiajector();
		}

		public void SummonDiajector()
		{
			if (diajector.gameObject.activeSelf)
				return;

			diajector.StartAssembly(this);
		}

		public void DesummonDiajector()
		{
			if (!diajector.gameObject.activeSelf)
				return;

			diajector.StartDisassembly();
		}

		public void ForceClose()
		{
			diajector.ForceClose();
		}
	}
}
