//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.ObjectPooling
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なLifeTimeFlags型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible LifeTimeFlags type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleLifeTimeFlags : FlexibleField<LifeTimeFlags>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleLifeTimeFlagsデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleLifeTimeFlags default constructor
		/// </summary>
#endif
		public FlexibleLifeTimeFlags()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleLifeTimeFlagsコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleLifeTimeFlags constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleLifeTimeFlags(LifeTimeFlags value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleLifeTimeFlagsコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleLifeTimeFlags constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleLifeTimeFlags(AnyParameterReference parameter) : base(parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleLifeTimeFlagsコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleLifeTimeFlags constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleLifeTimeFlags(InputSlotAny slot) : base(slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleLifeTimeFlagsをLifeTimeFlagsにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleLifeTimeFlags</param>
		/// <returns>LifeTimeFlagsにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleLifeTimeFlags to LifeTimeFlags.
		/// </summary>
		/// <param name="flexible">FlexibleLifeTimeFlags</param>
		/// <returns>Returns the result of casting to LifeTimeFlags.</returns>
#endif
		public static explicit operator LifeTimeFlags(FlexibleLifeTimeFlags flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// LifeTimeFlagsをFlexibleLifeTimeFlagsにキャスト。
		/// </summary>
		/// <param name="value">LifeTimeFlags</param>
		/// <returns>FlexibleLifeTimeFlagsにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast LifeTimeFlags to FlexibleLifeTimeFlags.
		/// </summary>
		/// <param name="value">LifeTimeFlags</param>
		/// <returns>Returns the result of casting to FlexibleLifeTimeFlags.</returns>
#endif
		public static explicit operator FlexibleLifeTimeFlags(LifeTimeFlags value)
		{
			return new FlexibleLifeTimeFlags(value);
		}
	}
}