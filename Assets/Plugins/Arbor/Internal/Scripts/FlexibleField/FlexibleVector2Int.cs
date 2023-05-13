//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なVector2Int型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible Vector2Int type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleVector2Int : FlexibleField<Vector2Int>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector2Intデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleVector2Int default constructor
		/// </summary>
#endif
		public FlexibleVector2Int()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector2Intコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleVector2Int constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleVector2Int(Vector2Int value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector2Intコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleVector2 constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleVector2Int(Vector2IntParameterReference parameter) : base(new AnyParameterReference(parameter))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector2Intコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleVector2Int constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleVector2Int(InputSlotVector2Int slot) : base(new InputSlotAny(slot))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector2IntをVector2Intにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleVector2Int</param>
		/// <returns>Vector2Intにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleVector2Int to Vector2Int.
		/// </summary>
		/// <param name="flexible">FlexibleVector2Int</param>
		/// <returns>Returns the result of casting to Vector2Int.</returns>
#endif
		public static explicit operator Vector2Int(FlexibleVector2Int flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2IntをFlexibleVector2Intにキャスト。
		/// </summary>
		/// <param name="value">Vector2Int</param>
		/// <returns>FlexibleVector2Intにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast Vector2Int to FlexibleVector2Int.
		/// </summary>
		/// <param name="value">Vector2Int</param>
		/// <returns>Returns the result of casting to FlexibleVector2Int.</returns>
#endif
		public static explicit operator FlexibleVector2Int(Vector2Int value)
		{
			return new FlexibleVector2Int(value);
		}
	}
}