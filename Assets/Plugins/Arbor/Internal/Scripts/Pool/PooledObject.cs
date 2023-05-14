//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor.Pool
{
#if ARBOR_DOC_JA
	/// <summary>
	/// プールしたインスタンスを管理する型。
	/// </summary>
	/// <typeparam name="T">インスタンスの型</typeparam>
	/// <remarks>usingステートメントに使用することでスコープから出た際にプールへ返却されるようになる。</remarks>
#else
	/// <summary>
	/// A type that manages pooled instances.
	/// </summary>
	/// <typeparam name="T">Instance type</typeparam>
	/// <remarks>By using it in the using statement, it will be returned to the pool when it goes out of scope.</remarks>
#endif
	public struct PooledObject<T> : IDisposable where T : class
	{
#if UNITY_2021_1_OR_NEWER
		private readonly UnityEngine.Pool.PooledObject<T> m_PooledObject;

		internal PooledObject(UnityEngine.Pool.PooledObject<T> pooledObject)
		{
			m_PooledObject = pooledObject;
		}

		static void GenericDispose<TDisposable>(TDisposable value) where TDisposable : IDisposable
		{
			value.Dispose();
		}

		void IDisposable.Dispose()
		{
			GenericDispose(m_PooledObject);
		}
#else
		private readonly T m_ToReturn;
		private readonly IObjectPool<T> m_Pool;

		internal PooledObject(T value, IObjectPool<T> pool)
		{
			m_ToReturn = value;
			m_Pool = pool;
		}

		void IDisposable.Dispose()
		{
			m_Pool.Release(m_ToReturn);
		}
#endif
	}
}