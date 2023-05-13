//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

namespace Arbor.Pool
{
#if ARBOR_DOC_JA
	/// <summary>
	/// <see cref="List{T}"/>型のプール
	/// </summary>
	/// <typeparam name="T">要素の型</typeparam>
#else
	/// <summary>
	/// <see cref="List{T}" /> type pool
	/// </summary>
	/// <typeparam name="T">Element type</typeparam>
#endif
	public class ListPool<T>
#if !UNITY_2021_1_OR_NEWER
		: CollectionPool<List<T>, T>
#endif
	{
#if UNITY_2021_1_OR_NEWER
		public static List<T> Get()
		{
			return UnityEngine.Pool.ListPool<T>.Get();
		}

		public static PooledObject<List<T>> Get(out List<T> value)
		{
			var pooledObject = UnityEngine.Pool.ListPool<T>.Get(out value);
			return new PooledObject<List<T>>(pooledObject);
		}

		public static void Release(List<T> toRelease)
		{
			UnityEngine.Pool.ListPool<T>.Release(toRelease);
		}
#endif
	}
}