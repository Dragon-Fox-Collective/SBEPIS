//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours.ObjectPooling
{
	using Arbor.ObjectPooling;

#if ARBOR_DOC_JA
	/// <summary>
	/// 事前にインスタンス化してObjectPoolへ登録。
	/// </summary>
#else
	/// <summary>
	/// Instantiate in advance and register to ObjectPool.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("ObjectPooling/AdvancedPooling")]
	[BuiltInBehaviour]
	public sealed class AdvancedPooling : StateBehaviour
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

#if ARBOR_DOC_JA
		/// <summary>
		/// 事前プールによる準備が完了した際の遷移<br />
		/// 遷移メソッド : OnStateUpdate
		/// </summary>
#else
		/// <summary>
		/// Transition when the advance pool is completed<br />
		/// Transition Method : OnStateUpdate
		/// </summary>
#endif
		[SerializeField]
		private StateLink _Ready = new StateLink();

		// Use this for enter state
		public override void OnStateBegin()
		{
			_PoolingItems.AdvancedPool();
		}

		// OnStateUpdate is called once per frame
		public override void OnStateUpdate()
		{
			if (ObjectPool.isReady)
			{
				Transition(_Ready);
			}
		}
	}
}