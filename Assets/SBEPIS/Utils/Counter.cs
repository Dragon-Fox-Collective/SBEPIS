using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Utils
{
	public class Counter : MonoBehaviour
	{
		[SerializeField] private bool useMin;
		[SerializeField, ShowIf("useMin")] private int min;
		[SerializeField] private bool useMax;
		[SerializeField, ShowIf("useMin")] private int max;
		
		public UnityEvent<int> onCountChanged = new();
		
		private int count = 0;
		public int Count
		{
			get => count;
			private set
			{
				count = Mathf.Clamp(value, useMin ? min : Int32.MinValue, useMax ? max : Int32.MaxValue);
				onCountChanged.Invoke(count);
			}
		}
		
		public void Increment() => Count++;
		public void Decrement() => Count--;
	}
}
