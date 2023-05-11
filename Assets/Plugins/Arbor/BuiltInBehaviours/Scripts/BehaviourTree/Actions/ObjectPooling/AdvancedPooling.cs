//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

using Arbor.ObjectPooling;

namespace Arbor.BehaviourTree.Actions.ObjectPooling
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 事前にインスタンス化してObjectPoolへ登録。
	/// </summary>
	/// <remarks>
	/// 完了したら成功を返す。
	/// </remarks>
#else
	/// <summary>
	/// Instantiate in advance and register to ObjectPool.
	/// </summary>
	/// <remarks>
	/// Returning success is completed.
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("ObjectPooling/AdvancedPooling")]
	[BuiltInBehaviour]
	public sealed class AdvancedPooling : ActionBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// プールするオブジェクトリスト。
		/// </summary>
#else
		/// <summary>
		/// List of objects to pool.
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(PoolingItem))]
		private PoolingItemList _PoolingItems = new PoolingItemList();

		protected override void OnStart()
		{
			_PoolingItems.AdvancedPool();
		}

		protected override void OnExecute()
		{
			if (ObjectPool.isReady)
			{
				FinishExecute(true);
			}
		}
	}
}