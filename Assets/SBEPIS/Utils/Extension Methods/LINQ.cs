using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SBEPIS.Utils.Linq
{
	public static class LINQ
	{
		public static string Join<T>(this string delimiter, IEnumerable<T> source) => string.Join(delimiter, source);
		public static string JoinWith<T>(this IEnumerable<T> source, string delimiter) => string.Join(delimiter, source);
		public static string ToDelimString<T>(this IEnumerable<T> source) => "[" + source.JoinWith(", ") +  "]";
		
		public static T Pop<T>(this IList<T> list)
		{
			T obj = list[0];
			list.RemoveAt(0);
			return obj;
		}
		
		public static IEnumerable<(int index, T item)> Enumerate<T>(this IEnumerable<T> source)
		{
			int i = 0;
			foreach (T item in source)
				yield return (i++, item);
		}
		
		public static bool All(this IEnumerable<bool> source) => source.All(b => b);
		public static bool Any(this IEnumerable<bool> source) => source.Any(b => b);
		
		public static TResult InvokeWith<T1, T2, TResult>(this Func<T1, T2, TResult> func, (T1, T2) args) => func(args.Item1, args.Item2);
		public static void InvokeWith<T1, T2>(this Action<T1, T2> func, (T1, T2) args) => func(args.Item1, args.Item2);
		
		public static IEnumerable<T> Flatten<T>(this IEnumerable<IEnumerable<T>> source) => source.SelectMany(item => item);
        
        public static IEnumerable<T> Of<T>(T item) { yield return item; }
        public static IEnumerable<T> Of<T>(params T[] items) => items;
        
        public static IEnumerable<T> Process<T>(this IEnumerable<T> source, Action<T> action)
        {
        	foreach (T item in source)
        	{
        		action(item);
        		yield return item;
        	}
        }
        
        public static T ProcessOn<T>(this IEnumerable<Action<T>> source, T obj)
        {
	        source.ForEach(action => action(obj));
	        return obj;
        }
        
        public static IEnumerable<T> Pivot<T>(this IEnumerable<T> source, int index)
        {
        	IEnumerable<T> first = Enumerable.Empty<T>();
        	int i = 0;
        	foreach (T item in source)
        	{
        		if (i++ < index)
        			first = first.Append(item);
        		else
        			yield return item;
        	}
        	foreach (T item in first)
        		yield return item;
        }
        
        public static (IEnumerable<T>, IEnumerable<T>) Split<T>(this IEnumerable<T> source, int index)
        {
        	IEnumerable<T> first = Enumerable.Empty<T>();
        	IEnumerable<T> last = Enumerable.Empty<T>();
        	int i = 0;
        	foreach (T item in source)
        	{
        		if (i >= index)
        			first = first.Append(item);
        		else
        			last = last.Append(item);
        		i++;
        	}
        	return (first, last);
        }
        
        public static TValue GetEnsured<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, Func<TValue> newValueFactory)
        {
        	if (!dictionary.ContainsKey(key))
        		dictionary.Add(key, newValueFactory());
        	return dictionary[key];
        }
        
        public static TValue GetEnsured<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key) where TValue : new() => GetEnsured(dictionary, key, () => new TValue());
        
        public static T[] Fill<T>(this T[] array, T item)
        {
	        T[] newArray = new T[array.Length];
	        Array.Fill(newArray, item);
	        return newArray;
        }

        public static T RandomElement<T>(this IList<T> source) => source[UnityEngine.Random.Range(0, source.Count)];
        public static void AddRange<T>(this IList<T> source, int count, Func<T> factory) => Enumerable.Range(0, count).Select(_ => factory()).ForEach(source.Add);
        
        public static IEnumerable<T> Insert<T>(this IEnumerable<T> enumerable, int index, T newItem)
		{
			int i = 0;
			foreach (T item in enumerable)
			{
				if (i++ == index)
					yield return newItem;
				yield return item;
			}
		}
		
		public static T ElementAtOrLast<T>(this IEnumerable<T> enumerable, int index)
		{
			T last = default;
			int i = 0;
			foreach (T item in enumerable)
			{
				last = item;
				if (i == index)
					return last;
				i++;
			}
			return last;
		}
		
		public static IEnumerable<(TFirst, TSecond)> Zip<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second)
		{
			return first.Zip(second, (firstItem, secondItem) => (firstItem, secondItem));
		}
		
		public static IEnumerable<(TFirst, TSecond, TThird)> Zip<TFirst, TSecond, TThird>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third)
		{
			return first.Zip(second).Zip(third, (firstSecond, thirdItem) => (firstSecond.Item1, firstSecond.Item2, thirdItem));
		}
		
		public static IEnumerable<(TFirst, TSecond, TThird, TFourth)> Zip<TFirst, TSecond, TThird, TFourth>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, IEnumerable<TThird> third, IEnumerable<TFourth> fourth)
		{
			return first.Zip(second, third).Zip(fourth, (firstSecondThird, fourthItem) => (firstSecondThird.Item1, firstSecondThird.Item2, firstSecondThird.Item3, fourthItem));
		}
		
		public static (TFirst, TSecond) Zipper<TFirst, TSecond>(TFirst first, TSecond second) => (first, second);
		
		public static IEnumerable<(T, T)> ZipOrDefault<T>(this IEnumerable<T> first, IEnumerable<T> second, Func<T> generator) => ZipOrDefault(first, second, generator, generator, Zipper);
		public static IEnumerable<(TFirst, TSecond)> ZipOrDefault<TFirst, TSecond>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second) => ZipOrDefault(first, second, () => default, () => default, Zipper);
		public static IEnumerable<TResult> ZipOrDefault<TFirst, TSecond, TResult>(this IEnumerable<TFirst> first, IEnumerable<TSecond> second, Func<TFirst> firstGenerator, Func<TSecond> secondGenerator, Func<TFirst, TSecond, TResult> zipper)
		{
			using IEnumerator<TFirst> firstEnumerator = first.GetEnumerator();
			using IEnumerator<TSecond> secondEnumerator = second.GetEnumerator();
			
			bool hasFirst = true;
			bool hasSecond = true;
			
			while (true)
			{
				if (hasFirst) hasFirst = firstEnumerator.MoveNext();
				if (hasSecond) hasSecond = secondEnumerator.MoveNext();
				
				if (!hasFirst && !hasSecond)
					break;
				
				yield return zipper(hasFirst ? firstEnumerator.Current : firstGenerator(), hasSecond ? secondEnumerator.Current : secondGenerator());
			}
		}
		
		public static IEnumerable<TResult> Zip<TSource, TResult>(this IEnumerable<IEnumerable<TSource>> source, Func<IEnumerable<TSource>, TResult> zipper)
		{
			List<IEnumerator<TSource>> enumerators = source.Select(layer => layer.GetEnumerator()).ToList();
			while (true)
			{
				List<IEnumerator<TSource>> currentEnumerators = enumerators.Where(enumerator => enumerator.MoveNext()).ToList();
				if (!currentEnumerators.Any()) yield break;
				yield return zipper(currentEnumerators.Select(enumerator => enumerator.Current));
			}
		}
		
		public static void Shuffle<T>(this IList<T> list)  
		{
			for (int n = list.Count - 1; n > 1; n--)
				list.Swap(n, UnityEngine.Random.Range(0, n + 1));
		}
		
		public static void Swap<T>(this IList<T> list, int i1, int i2) => (list[i1], list[i2]) = (list[i2], list[i1]);
		
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
		{
			foreach (T t in source)
				action(t);
		}
		
		public static void ForEach<T1, T2>(this IEnumerable<(T1, T2)> source, Action<T1, T2> action)
		{
			foreach ((T1 item1, T2 item2) in source)
				action(item1, item2);
		}
		
		public static IEnumerable<IEnumerable<T>> Divide<T>(this IList<T> source, int count)
		{
			for (int i = 0; i * count < source.Count; i++)
				yield return source.Skip(i * count).Take(count);
		}
		
		public static IEnumerable<T> AsEnumerable<T>(this IEnumerator<T> source)
		{
			while (source.MoveNext())
				yield return source.Current;
		}
		public static IEnumerable AsEnumerable(this IEnumerator source)
		{
			while (source.MoveNext())
				yield return source.Current;
		}
		
		public static TSource CompareBy<TSource, TAggregate>(this IEnumerable<TSource> source, Func<TSource, TAggregate> selector, TAggregate seed, Func<TAggregate, TAggregate, bool> useNewValue) => source.Select(item => (item, selector(item))).Aggregate<(TSource, TAggregate), (TSource, TAggregate)>((default, seed), (currentZip, newZip) => useNewValue(currentZip.Item2, newZip.Item2) ? newZip : currentZip).Item1;
		
		public static void AddRange<T>(this HashSet<T> source, IEnumerable<T> items)
		{
			foreach (T item in items)
				source.Add(item);
		}
	}
}
