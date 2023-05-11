using System;
using Arbor;
using UnityEngine;

namespace SBEPIS.Capturellection.CardState
{
	public abstract class GetCalculator<T, TOutputSlot> : Calculator where T : Component where TOutputSlot : OutputSlot<T>, new()
	{
		[SerializeField] private new FlexibleGameObject gameObject = new();
		[SerializeField] private TOutputSlot value = new();
		
		public override void OnCalculate() => value.SetValue(gameObject.value.TryGetComponent(out T component) ? component : null);
	}
}
