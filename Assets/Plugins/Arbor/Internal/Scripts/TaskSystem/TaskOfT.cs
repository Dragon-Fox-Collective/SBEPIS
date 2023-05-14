//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.TaskSystem
{
	using Arbor.Pool;

#if ARBOR_DOC_JA
	/// <summary>
	/// プールに対応したタスククラス
	/// </summary>
	/// <typeparam name="T">実装する型</typeparam>
#else
	/// <summary>
	/// Task class corresponding to the pool
	/// </summary>
	/// <typeparam name="T">Type to implement</typeparam>
#endif
	public abstract class Task<T> : Task where T : Task<T>, new()
	{
		private static readonly ObjectPool<T> s_Pool = new ObjectPool<T>(() => new T());

		int _RefCount;

#if ARBOR_DOC_JA
		/// <summary>
		/// コンストラクタ
		/// </summary>
#else
		/// <summary>
		/// constructor
		/// </summary>
#endif
		protected Task()
		{
			_RefCount = 0;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 初期化するときに呼ばれるメソッド
		/// </summary>
#else
		/// <summary>
		/// Method called when initializing
		/// </summary>
#endif
		protected override void Init()
		{
			base.Init();

			if (_RefCount != 0)
			{
				Debug.Log("Task improperly released.");
				_RefCount = 0;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プールからタスクインスタンスを取得する
		/// </summary>
		/// <returns>タスクインスタンス</returns>
#else
		/// <summary>
		/// Get a task instance from the pool
		/// </summary>
		/// <returns>Task instance</returns>
#endif
		public static T GetPooled()
		{
			T t = s_Pool.Get();
			t.Init();
			t.pooled = true;
			t.Acquire();

			return t;
		}

		static void ReleasePooled(T task)
		{
			if (task.pooled)
			{
				task.Init();

				s_Pool.Release(task);

				task.pooled = false;
			}
		}

		internal override void Acquire()
		{
			_RefCount++;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 廃棄処理を行う。
		/// </summary>
#else
		/// <summary>
		/// Dispose of it.
		/// </summary>
#endif
		public sealed override void Dispose()
		{
			if (--_RefCount == 0)
			{
				ReleasePooled((T)this);
			}
		}
	}
}