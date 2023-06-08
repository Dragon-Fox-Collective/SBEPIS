using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class CallbackList<T> : IList<T>
{
	public readonly List<T> backingList = new();
	
	public readonly UnityEvent<T> onAddItem = new();
	public readonly UnityEvent<T> onRemoveItem = new();
	
	public int Count => backingList.Count;
	bool ICollection<T>.IsReadOnly => false;
	
	public IEnumerator<T> GetEnumerator() => backingList.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	public void Add(T item)
	{
		backingList.Add(item);
		onAddItem.Invoke(item);
	}
	
	public void AddRange(IEnumerable<T> collection)
	{
		foreach (T item in collection)
		{
			backingList.Add(item);
			onAddItem.Invoke(item);
		}
	}
	
	public void Clear()
	{
		List<T> oldItems = this.ToList();
		backingList.Clear();
		oldItems.ForEach(onRemoveItem.Invoke);
	}
	
	public bool Contains(T item) => backingList.Contains(item);
	
	public T Find(Predicate<T> match) => backingList.Find(match);
	public int FindIndex(Predicate<T> match) => backingList.FindIndex(match);
	
	public void CopyTo(T[] array, int arrayIndex) => backingList.CopyTo(array, arrayIndex);
	
	public bool Remove(T item)
	{
		if (!backingList.Remove(item)) return false;
		onRemoveItem.Invoke(item);
		return true;
	}
	
	public int IndexOf(T item) => backingList.IndexOf(item);
	
	public void Insert(int index, T item)
	{
		backingList.Insert(index, item);
		onAddItem.Invoke(item);
	}
	
	public void RemoveAt(int index)
	{
		T item = backingList[index];
		backingList.RemoveAt(index);
		onRemoveItem.Invoke(item);
	}
	
	public T this[int index]
	{
		get => backingList[index];
		set
		{
			T oldItem = backingList[index];
			backingList[index] = value;
			onRemoveItem.Invoke(oldItem);
			onAddItem.Invoke(value);
		}
	}
}
