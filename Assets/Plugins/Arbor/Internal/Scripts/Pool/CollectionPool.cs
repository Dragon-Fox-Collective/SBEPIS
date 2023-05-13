//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

namespace Arbor.Pool
{
#if ARBOR_DOC_JA
	/// <summary>
	/// <see cref="ICollection{T}"/>型のプール。
	/// </summary>
	/// <typeparam name="TCollection">Collectionの型</typeparam>
	/// <typeparam name="TItem">要素の型</typeparam>
#else
	/// <summary>
	/// <see cref="ICollection{T}"/> type pool.
	/// </summary>
	/// <typeparam name="TCollection">Collection type</typeparam>
	/// <typeparam name="TItem">Element type</typeparam>
#endif
	public class CollectionPool<TCollection, TItem> where TCollection : class, ICollection<TItem>, new()
	{
		internal static readonly ObjectPool<TCollection> s_Pool = new ObjectPool<TCollection>(
			() => new TCollection(),
			null,
			l => l.Clear(),
			null,
			true, 10, 10000);

#if ARBOR_DOC_JA
		/// <summary>
		/// プールからインスタンスを取り出す。
		/// </summary>
		/// <returns>取り出したインスタンス。</returns>
#else
		/// <summary>
		/// Fetch an instance from the pool.
		/// </summary>
		/// <returns>The retrieved instance.</returns>
#endif
		public static TCollection Get()
		{
			return s_Pool.Get();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プールからインスタンスを取り出し、<see cref="PooledObject{T}"/>を返す。
		/// </summary>
		/// <param name="value">取り出したインスタンス。</param>
		/// <returns><see cref="PooledObject{T}"/></returns>
		/// <remarks>usingステートメントに<see cref="PooledObject{T}"/>を使用することでスコープから出た際にプールへ返却されるようになる。</remarks>
#else
		/// <summary>
		/// Fetch an instance from the pool and return a <see cref="PooledObject{T}"/>.
		/// </summary>
		/// <param name="value">The retrieved instance.</param>
		/// <returns><see cref="PooledObject{T}"/></returns>
		/// <remarks>By using <see cref = "PooledObject {T}" /> in the using statement, it will be returned to the pool when it goes out of scope.</remarks>
#endif
		public static PooledObject<TCollection> Get(out TCollection value)
		{
			return s_Pool.Get(out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プールへインスタンスを返却する。
		/// </summary>
		/// <param name="toRelease">返却するインスタンス</param>
#else
		/// <summary>
		/// Return the instance to the pool.
		/// </summary>
		/// <param name="toRelease">Instance to return</param>
#endif
		public static void Release(TCollection toRelease)
		{
			s_Pool.Release(toRelease);
		}
	}
}