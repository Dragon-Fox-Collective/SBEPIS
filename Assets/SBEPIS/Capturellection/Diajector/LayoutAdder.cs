using System.Collections.Generic;
using KBCore.Refs;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(InventoryStorable))]
	public class LayoutAdder : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private InventoryStorable card;
		
		private List<DiajectorCaptureLayout> layouts = new();
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out DiajectorCaptureLayout layout) && !card.DequeElement.IsStored)
				AddLayout(layout);
		}
		
		private void OnTriggerExit(Collider other)
		{
			if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out DiajectorCaptureLayout layout) && layout.HasTemporaryTarget(card))
				RemoveLayout(layout);
		}
		
		private void AddLayout(DiajectorCaptureLayout layout)
		{
			layouts.Add(layout);
			card.DequeElement.IsInLayoutArea = layouts.Count > 0;
			layout.AddTemporaryTarget(card);
		}
		
		private void RemoveLayout(DiajectorCaptureLayout layout)
		{
			layouts.Remove(layout);
			card.DequeElement.IsInLayoutArea = layouts.Count > 0;
			layout.RemoveTemporaryTarget(card);
		}
		
		public DiajectorCaptureLayout PopAllLayouts()
		{
			DiajectorCaptureLayout lastLayout = layouts[^1];
			while (layouts.Count > 0)
				RemoveLayout(layouts[0]);
			return lastLayout;
		}
	}
}