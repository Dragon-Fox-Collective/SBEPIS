using System;
using UnityEngine;
using UnityEngine.Events;

namespace SBEPIS.Utils
{
	public class Counter : MonoBehaviour
	{
		[SerializeField] private int count = 0;
		public int Count
		{
			get => count;
			private set
			{
				count = Mathf.Clamp(value, useMin ? min : int.MinValue, useMax ? max : int.MaxValue);
				onCountChanged.Invoke(count);
			}
		}
		[SerializeField] private bool useMin;
		[SerializeField] private int min;
		[SerializeField] private bool useMax;
		[SerializeField] private int max;
		
		public UnityEvent<int> onCountChanged = new();
		
		public void Increment() => Count++;
		public void Decrement() => Count--;
	}
}
