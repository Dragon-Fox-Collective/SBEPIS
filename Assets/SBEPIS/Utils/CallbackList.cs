using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

public class CallbackList<T> : IList<T>
{
	public readonly List<T> BackingList = new();
	
	public readonly UnityEvent<T> OnAddItem = new();
	public readonly UnityEvent<T> OnRemoveItem = new();
	
	public int Count => BackingList.Count;
	bool ICollection<T>.IsReadOnly => false;
	
	public IEnumerator<T> GetEnumerator() => BackingList.GetEnumerator();
	IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	
	public void Add(T item)
	{
		BackingList.Add(item);
		OnAddItem.Invoke(item);
	}
	
	public void AddRange(IEnumerable<T> collection)
	{
		foreach (T item in collection)
		{
			BackingList.Add(item);
			OnAddItem.Invoke(item);
		}
	}
	
	public void Clear()
	{
		List<T> oldItems = this.ToList();
		BackingList.Clear();
		oldItems.ForEach(OnRemoveItem.Invoke);
	}
	
	public bool Contains(T item) => BackingList.Contains(item);
	
	public T Find(Predicate<T> match) => BackingList.Find(match);
	public int FindIndex(Predicate<T> match) => BackingList.FindIndex(match);
	
	public void CopyTo(T[] array, int arrayIndex) => BackingList.CopyTo(array, arrayIndex);
	
	public bool Remove(T item)
	{
		if (!BackingList.Remove(item)) return false;
		OnRemoveItem.Invoke(item);
		return true;
	}
	
	public int IndexOf(T item) => BackingList.IndexOf(item);
	
	public void Insert(int index, T item)
	{
		BackingList.Insert(index, item);
		OnAddItem.Invoke(item);
	}
	
	public void RemoveAt(int index)
	{
		T item = BackingList[index];
		BackingList.RemoveAt(index);
		OnRemoveItem.Invoke(item);
	}
	
	public T this[int index]
	{
		get => BackingList[index];
		set
		{
			T oldItem = BackingList[index];
			BackingList[index] = value;
			OnRemoveItem.Invoke(oldItem);
			OnAddItem.Invoke(value);
		}
	}
}
