//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor.Pool
{
#if ARBOR_DOC_JA
	/// <summary>
	/// クラス型インスタンスをプールするインターフェイス
	/// </summary>
	/// <typeparam name="T">プールする型</typeparam>
#else
	/// <summary>
	/// Interface for pooling class type instances
	/// </summary>
	/// <typeparam name="T">Pool type</typeparam>
#endif
	public interface IObjectPool<T> where T : class
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// このプールに格納されている未使用のインスタンスの数
		/// </summary>
#else
		/// <summary>
		/// Number of unused instances stored in this pool
		/// </summary>
#endif
		int CountInactive
		{
			get;
		}

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
		T Get();

#if ARBOR_DOC_JA
		/// <summary>
		/// プールからインスタンスを取り出し、<see cref="PooledObject{T}"/>を返す。
		/// </summary>
		/// <param name="v">取り出したインスタンス。</param>
		/// <returns><see cref="PooledObject{T}"/></returns>
		/// <remarks>usingステートメントに<see cref="PooledObject{T}"/>を使用することでスコープから出た際にプールへ返却されるようになる。</remarks>
#else
		/// <summary>
		/// Fetch an instance from the pool and return a <see cref="PooledObject{T}"/>.
		/// </summary>
		/// <param name="v">The retrieved instance.</param>
		/// <returns><see cref="PooledObject{T}"/></returns>
		/// <remarks>By using <see cref = "PooledObject {T}" /> in the using statement, it will be returned to the pool when it goes out of scope.</remarks>
#endif
		PooledObject<T> Get(out T v);

#if ARBOR_DOC_JA
		/// <summary>
		/// プールへインスタンスを返却する。
		/// </summary>
		/// <param name="element">返却するインスタンス</param>
#else
		/// <summary>
		/// Return the instance to the pool.
		/// </summary>
		/// <param name="element">Instance to return</param>
#endif
		void Release(T element);

#if ARBOR_DOC_JA
		/// <summary>
		/// プールしているインスタンスを全てクリアする。
		/// </summary>
#else
		/// <summary>
		/// Clear all pooled instances.
		/// </summary>
#endif
		void Clear();
	}
}