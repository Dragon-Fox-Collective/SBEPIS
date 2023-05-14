//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なVector2型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible Vector2 type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleVector2 : FlexibleField<Vector2>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector2デフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleVector2 default constructor
		/// </summary>
#endif
		public FlexibleVector2()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector2コンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleVector2 constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleVector2(Vector2 value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector2コンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleVector2 constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleVector2(Vector2ParameterReference parameter) : base(new AnyParameterReference(parameter))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector2コンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleVector2 constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleVector2(InputSlotVector2 slot) : base(new InputSlotAny(slot))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector2をVector2にキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleVector2</param>
		/// <returns>Vector2にキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleVector2 to Vector2.
		/// </summary>
		/// <param name="flexible">FlexibleVector2</param>
		/// <returns>Returns the result of casting to Vector2.</returns>
#endif
		public static explicit operator Vector2(FlexibleVector2 flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2をFlexibleVector2にキャスト。
		/// </summary>
		/// <param name="value">Vector2</param>
		/// <returns>FlexibleVector2にキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast Vector2 to FlexibleVector2.
		/// </summary>
		/// <param name="value">Vector2</param>
		/// <returns>Returns the result of casting to FlexibleVector2.</returns>
#endif
		public static explicit operator FlexibleVector2(Vector2 value)
		{
			return new FlexibleVector2(value);
		}
	}
}
