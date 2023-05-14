//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なVector3Int型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible Vector3Int type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleVector3Int : FlexibleField<Vector3Int>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector3Intデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleVector3Int default constructor
		/// </summary>
#endif
		public FlexibleVector3Int()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector3Intコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleVector3Int constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleVector3Int(Vector3Int value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector3Intコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleVector2 constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleVector3Int(Vector3IntParameterReference parameter) : base(new AnyParameterReference(parameter))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector3Intコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleVector3Int constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleVector3Int(InputSlotVector3Int slot) : base(new InputSlotAny(slot))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector3IntをVector3Intにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleVector3Int</param>
		/// <returns>Vector3Intにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleVector3Int to Vector3Int.
		/// </summary>
		/// <param name="flexible">FlexibleVector3Int</param>
		/// <returns>Returns the result of casting to Vector3Int.</returns>
#endif
		public static explicit operator Vector3Int(FlexibleVector3Int flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3IntをFlexibleVector3Intにキャスト。
		/// </summary>
		/// <param name="value">Vector3Int</param>
		/// <returns>FlexibleVector3Intにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast Vector3Int to FlexibleVector3Int.
		/// </summary>
		/// <param name="value">Vector3Int</param>
		/// <returns>Returns the result of casting to FlexibleVector3Int.</returns>
#endif
		public static explicit operator FlexibleVector3Int(Vector3Int value)
		{
			return new FlexibleVector3Int(value);
		}
	}
}