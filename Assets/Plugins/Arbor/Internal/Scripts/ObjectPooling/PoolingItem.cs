//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.ObjectPooling
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 事前にプールするオブジェクトを設定するためのクラス
	/// </summary>
#else
	/// <summary>
	/// Class for setting the object to the pool in advance
	/// </summary>
#endif
	[System.Serializable]
	[Internal.Documentable]
	public sealed class PoolingItem
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトの型
		/// </summary>
#else
		/// <summary>
		/// Object type
		/// </summary>
#endif
		[SerializeField]
		[ClassPoolTarget]
		private ClassTypeReference _Type = new ClassTypeReference(typeof(GameObject));

#if ARBOR_DOC_JA
		/// <summary>
		/// オリジナルのオブジェクト
		/// </summary>
#else
		/// <summary>
		/// Original object
		/// </summary>
#endif
		public Object original = null;

#if ARBOR_DOC_JA
		/// <summary>
		/// プールする個数
		/// </summary>
#else
		/// <summary>
		/// Amount to pool
		/// </summary>
#endif
		public int amount = 0;

#if ARBOR_DOC_JA
		/// <summary>
		/// ObjectPoolの生存時間フラグ
		/// </summary>
#else
		/// <summary>
		/// ObjectPool lifetime flag
		/// </summary>
#endif
		public LifeTimeFlags lifeTimeFlags = LifeTimeFlags.SceneUnloaded;

#if ARBOR_DOC_JA
		/// <summary>
		/// ObjectPoolの生存時間
		/// </summary>
#else
		/// <summary>
		/// ObjectPool lifetime
		/// </summary>
#endif
		public float lifeDuration = 0f;

#if ARBOR_DOC_JA
		/// <summary>
		/// オブジェクトの型
		/// </summary>
#else
		/// <summary>
		/// Object type
		/// </summary>
#endif
		public System.Type type
		{
			get
			{
				return _Type.type;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// デフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// Default constructor
		/// </summary>
#endif
		public PoolingItem()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="original">オリジナルのオブジェクト</param>
		/// <param name="amount">プールする量</param>
#else
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="original">Original object</param>
		/// <param name="amount">Amount to pool</param>
#endif
		public PoolingItem(Object original, int amount)
		{
			this.original = original;
			this.amount = amount;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// コピーコンストラクタ
		/// </summary>
		/// <param name="item">コピーするPoolingItem</param>
#else
		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="item">Copy PoolingItem</param>
#endif
		public PoolingItem(PoolingItem item)
		{
			original = item.original;
			amount = item.amount;
			lifeTimeFlags = item.lifeTimeFlags;
			lifeDuration = item.lifeDuration;
		}
	}
}