//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なVector3型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible Vector3 type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleVector3 : FlexibleField<Vector3>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector3デフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleVector3 default constructor
		/// </summary>
#endif
		public FlexibleVector3()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector3コンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleVector3 constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleVector3(Vector3 value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector3コンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleVector3 constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleVector3(Vector3ParameterReference parameter) : base(new AnyParameterReference(parameter))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector3コンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleVector3 constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleVector3(InputSlotVector3 slot) : base(new InputSlotAny(slot))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleVector3をVector3にキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleVector3</param>
		/// <returns>Vector3にキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleVector3 to Vector3.
		/// </summary>
		/// <param name="flexible">FlexibleVector3</param>
		/// <returns>Returns the result of casting to Vector3.</returns>
#endif
		public static explicit operator Vector3(FlexibleVector3 flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3をFlexibleVector3にキャスト。
		/// </summary>
		/// <param name="value">Vector3</param>
		/// <returns>FlexibleVector3にキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast Vector3 to FlexibleVector3.
		/// </summary>
		/// <param name="value">Vector3</param>
		/// <returns>Returns the result of casting to FlexibleVector3.</returns>
#endif
		public static explicit operator FlexibleVector3(Vector3 value)
		{
			return new FlexibleVector3(value);
		}
	}
}
