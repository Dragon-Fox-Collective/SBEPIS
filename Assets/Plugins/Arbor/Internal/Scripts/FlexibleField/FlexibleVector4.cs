//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なVector4型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible Vector4 type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleVector4 : FlexibleField<Vector4>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector4デフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleVector4 default constructor
		/// </summary>
#endif
		public FlexibleVector4()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector4コンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleVector4 constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleVector4(Vector4 value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector4コンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleVector4 constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleVector4(Vector4ParameterReference parameter) : base(new AnyParameterReference(parameter))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector4コンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleVector4 constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleVector4(InputSlotVector4 slot) : base(new InputSlotAny(slot))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector4をVector4にキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleVector4</param>
		/// <returns>Vector4にキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleVector4 to Vector4.
		/// </summary>
		/// <param name="flexible">FlexibleVector4</param>
		/// <returns>Returns the result of casting to Vector4.</returns>
#endif
		public static explicit operator Vector4(FlexibleVector4 flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4をFlexibleVector4にキャスト。
		/// </summary>
		/// <param name="value">Vector4</param>
		/// <returns>FlexibleVector4にキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast Vector4 to FlexibleVector4.
		/// </summary>
		/// <param name="value">Vector4</param>
		/// <returns>Returns the result of casting to FlexibleVector4.</returns>
#endif
		public static explicit operator FlexibleVector4(Vector4 value)
		{
			return new FlexibleVector4(value);
		}
	}
}
