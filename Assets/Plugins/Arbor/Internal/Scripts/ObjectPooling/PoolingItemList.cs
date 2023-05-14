//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor.ObjectPooling
{
#if ARBOR_DOC_JA
	/// <summary>
	/// プールするオブジェクトのリスト
	/// </summary>
#else
	/// <summary>
	/// List of objects to pool
	/// </summary>
#endif
	[System.Serializable]
	public sealed class PoolingItemList
	{
		[SerializeField]
		private List<PoolingItem> _Items = new List<PoolingItem>();

#if ARBOR_DOC_JA
		/// <summary>
		/// 事前プールする。
		/// </summary>
#else
		/// <summary>
		/// Pool in advance.
		/// </summary>
#endif
		public void AdvancedPool()
		{
			ObjectPool.AdvancedPool(_Items);
		}
	}
}