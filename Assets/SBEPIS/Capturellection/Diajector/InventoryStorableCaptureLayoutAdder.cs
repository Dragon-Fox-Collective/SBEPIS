using System.Collections.Generic;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(InventoryStorable))]
	public class InventoryStorableCaptureLayoutAdder : MonoBehaviour
	{
		public InventoryStorable Card { get; private set; }
		private List<DiajectorCaptureLayout> layouts = new();
		
		private void Awake()
		{
			Card = GetComponent<InventoryStorable>();
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out DiajectorCaptureLayout layout) && !Card.DequeElement.IsStored)
				AddLayout(layout);
		}
		
		private void OnTriggerExit(Collider other)
		{
			if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out DiajectorCaptureLayout layout) && layout.HasTemporaryTarget(Card))
				RemoveLayout(layout);
		}
		
		private void AddLayout(DiajectorCaptureLayout layout)
		{
			layouts.Add(layout);
			if (layouts.Count == 1)
				Card.DequeElement.State.IsInLayoutArea = true;
			layout.AddTemporaryTarget(Card);
		}
		
		private void RemoveLayout(DiajectorCaptureLayout layout)
		{
			layouts.Add(layout);
			if (layouts.Count == 1)
				Card.DequeElement.State.IsInLayoutArea = true;
			layout.AddTemporaryTarget(Card);
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