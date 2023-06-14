using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using SBEPIS.Utils.Linq;

namespace SBEPIS.Utils.UniTaskLinq
{
	public static class UniTaskLINQ
	{
		public static async UniTask<TAccumulate> Aggregate<TSource, TAccumulate>(this IEnumerable<TSource> source, TAccumulate seed, Func<TAccumulate, TSource, UniTask<TAccumulate>> func)
		{
			TAccumulate value = seed;
			// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (TSource item in source)
				value = await func(value, item);
			return value;
		}
		
		public static async UniTask ForEach<T>(this IEnumerable<T> source, Func<T, UniTask> func)
		{
			foreach (T t in source)
				await func(t);
		}
		
		public static async UniTask ForEach<T1, T2>(this IEnumerable<(T1, T2)> source, Func<T1, T2, UniTask> func)
		{
			foreach ((T1 t1, T2 t2) in source)
				await func(t1, t2);
		}
		
		public static async UniTask<IEnumerable<TResult>> Select<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, UniTask<TResult>> action)
		{
			IEnumerable<TResult> result = Enumerable.Empty<TResult>();
			// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (TSource item in source)
				result = result.Append(await action(item));
			return result;
		}
		
		public static UniTask<IEnumerable<TResult>> SelectMany<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, UniTask<IEnumerable<TResult>>> action) => source.Select(action).ContinueWith(result => result.Flatten());
		
		public static UniTask<IEnumerable<T>> Where<T>(this UniTask<IEnumerable<T>> source, Func<T, bool> predicate) => source.ContinueWith(result => result.Where(predicate));

	}
}
