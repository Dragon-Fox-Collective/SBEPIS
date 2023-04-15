using SBEPIS.Controller;
using System.Collections.Generic;
using SBEPIS.Capturellection.CardState;
using SBEPIS.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace SBEPIS.Capturellection
{
	[RequireComponent(typeof(DequeStorable))]
	public class DequeStorableCaptureLayoutAdder : MonoBehaviour
	{
		private DequeStorable card;
		private List<DiajectorCaptureLayout> layouts = new();
		
		private void Awake()
		{
			card = GetComponent<DequeStorable>();
		}
		
		private void OnTriggerEnter(Collider other)
		{
			if (other.attachedRigidbody && other.attachedRigidbody.TryGetComponent(out DiajectorCaptureLayout layout) && !card.IsStored)
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
			if (layouts.Count == 1)
				card.State.IsInLayoutArea = true;
			layout.AddTemporaryTarget(card);
		}
		
		private void RemoveLayout(DiajectorCaptureLayout layout)
		{
			layouts.Add(layout);
			if (layouts.Count == 1)
				card.State.IsInLayoutArea = true;
			layout.AddTemporaryTarget(card);
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