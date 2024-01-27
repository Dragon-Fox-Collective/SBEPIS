using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SBEPIS.Utils
{
	public class Tree<T> : IEnumerable<T> where T : IComparable<T>, IEquatable<T>
	{
		private Node root;
		
		public int Height => root?.Height ?? 0;
		
		public IEnumerable<IEnumerable<(int, T)>> Layers => root?.Layers ?? Enumerable.Empty<IEnumerable<(int, T)>>();
		
		public void Add(T item, bool balance) => Node.Add(ref root, item, balance);
		
		public IEnumerable<T> Drop(T item, bool balance)
		{
			Node.Drop(item, ref root, balance, out IEnumerable<T> droppedItems);
			return droppedItems ?? Enumerable.Empty<T>();
		}
		
		public IEnumerable<T> DropRoot(bool balance) => Drop(root == null ? default : root.Item, balance);
		
		public IEnumerator<T> GetEnumerator()
		{
			if (root != null)
				foreach (T item in root)
					yield return item;
		}
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
		
		private class Node : IEnumerable<T>
		{
			private T item;
			public T Item => item;
			private Node left, right;
			
			public int Height => Mathf.Max(HeightLeft, HeightRight) + 1;
			private int HeightLeft => left?.Height ?? 0;
			private int HeightRight => right?.Height ?? 0;
			private int BalanceFactor => HeightRight - HeightLeft;
			private int LeftBalanceFactor => left?.BalanceFactor ?? 0;
			private int RightBalanceFactor => right?.BalanceFactor ?? 0;
			
			public IEnumerable<IEnumerable<(int, T)>> Layers
			{
				get
				{
					yield return EnumerableOf.Of((0, item));
					
					foreach ((int depth, (IEnumerable<(int, T)> leftLayer, IEnumerable<(int, T)> rightLayer)) in (left?.Layers ?? Enumerable.Empty<IEnumerable<(int, T)>>()).ZipOrDefault(right?.Layers ?? Enumerable.Empty<IEnumerable<(int, T)>>(), Enumerable.Empty<(int, T)>).Enumerate())
						yield return leftLayer.Concat(rightLayer.Select(tup => (tup.Item1 + (int)Mathf.Pow(2, depth), tup.Item2)));
				}
			}
			
			public Node(T item)
			{
				this.item = item;
			}
			
			public void Add(T item, bool balance)
			{
				if (this.item.CompareTo(item) > 0)
					Add(ref left, item, balance);
				else
					Add(ref right, item, balance);
			}
			
			public bool Drop(T item, bool balance, out IEnumerable<T> droppedItems)
			{
				if (this.item.Equals(item))
				{
					droppedItems = this;
					return true;
				}
				else
				{
					if (this.item.CompareTo(item) > 0)
						Drop(item, ref left, balance, out droppedItems);
					else
						Drop(item, ref right, balance, out droppedItems);
					return false;
				}
			}
			
			public IEnumerator<T> GetEnumerator()
			{
				if (left != null)
					foreach (T leftItem in left)
						yield return leftItem;
				
				yield return item;
				
				if (right != null)
					foreach (T rightItem in right)
						yield return rightItem;
			}
			IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
			
			public override string ToString() => $"Node<{item}>";
			
			public static void Add(ref Node node, T item, bool balance)
			{
				if (node == null)
					node = new Node(item);
				else
					node.Add(item, balance);
				
				if (balance) Balance(ref node);
			}
			
			public static void Drop(T item, ref Node node, bool balance, out IEnumerable<T> droppedItems)
			{
				if (node == null)
					droppedItems = null;
				else if (node.Drop(item, balance, out droppedItems))
					node = null;
				else if (balance)
					while (Mathf.Abs(node.BalanceFactor) > 1)
						Balance(ref node);
			}
			
			private static void RecursiveBalance(ref Node node)
			{
				if (node.left != null) RecursiveBalance(ref node.left);
				if (node.right != null) RecursiveBalance(ref node.right);
				while(Mathf.Abs(node.BalanceFactor) > 1) Balance(ref node);
			}
			
			private static void Balance(ref Node node)
			{
				if (node.BalanceFactor < -1)
				{
					if (node.LeftBalanceFactor > 0)
						node.left = RotateLeft(node.left);
					node = RotateRight(node);
				}
				else if (node.BalanceFactor > 1)
				{
					if (node.RightBalanceFactor < 0)
						node.right = RotateRight(node.right);
					node = RotateLeft(node);
				}
			}
			
			private static Node RotateRight(Node oldTop)
			{
				Node newTop = oldTop.left;
				Node transfer = newTop.right;
				
				newTop.right = oldTop;
				oldTop.left = transfer;
				
				return newTop;
			}
			
			private static Node RotateLeft(Node oldTop)
			{
				Node newTop = oldTop.right;
				Node transfer = newTop.left;
				
				newTop.left = oldTop;
				oldTop.right = transfer;
				
				return newTop;
			}
		}
	}
	
	public class TreeDictionary<TKey, TValue> : IEnumerable<TValue> where TKey : IComparable<TKey>, IEquatable<TKey>
	{
		private Tree<TKey> tree = new();
		private Dictionary<TKey, TValue> dictionary = new();
		
		public IEnumerable<IEnumerable<(int, TValue)>> Layers => tree.Layers.Select(layer => layer.Select(tup => (tup.Item1, dictionary[tup.Item2])));
		
		public void Add(TKey key, TValue value, bool balance)
		{
			dictionary.Add(key, value);
			tree.Add(key, balance);
		}
		
		public bool ContainsKey(TKey key) => dictionary.ContainsKey(key);
		public bool ContainsValue(TValue value) => dictionary.ContainsValue(value);
		
		public TValue Get(TKey key) => dictionary[key];
		
		public IEnumerable<TValue> Drop(TValue value, bool balance)
		{
			if (!ContainsValue(value)) yield break;
			TKey key = dictionary.First(pair => EqualityComparer<TValue>.Default.Equals(pair.Value, value)).Key;
			foreach ((TKey droppedKey, TValue droppedValue) in tree.Drop(key, balance).Select(droppedKey => (droppedKey, dictionary[droppedKey])))
			{
				dictionary.Remove(droppedKey);
				yield return droppedValue;
			}
		}
		
		public IEnumerable<TValue> DropRoot(bool balance)
		{
			IEnumerable<TValue> droppedItems = tree.DropRoot(balance).Select(key => dictionary[key]);
			dictionary.Clear();
			return droppedItems;
		}
		
		public bool TryGetValue(TKey key, out TValue value) => dictionary.TryGetValue(key, out value);
		
		public IEnumerator<TValue> GetEnumerator() => tree.Select(key => dictionary[key]).GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
