//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なForceMode型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible ForceMode type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleForceMode : FlexibleField<ForceMode>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleForceModeデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleForceMode default constructor
		/// </summary>
#endif
		public FlexibleForceMode()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleForceModeコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleForceMode constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleForceMode(ForceMode value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleForceModeコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleForceMode constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleForceMode(AnyParameterReference parameter) : base(parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleForceModeコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleForceMode constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleForceMode(InputSlotAny slot) : base(slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleForceModeをForceModeにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleForceMode</param>
		/// <returns>ForceModeにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleForceMode to ForceMode.
		/// </summary>
		/// <param name="flexible">FlexibleForceMode</param>
		/// <returns>Returns the result of casting to ForceMode.</returns>
#endif
		public static explicit operator ForceMode(FlexibleForceMode flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ForceModeをFlexibleForceModeにキャスト。
		/// </summary>
		/// <param name="value">ForceMode</param>
		/// <returns>FlexibleForceModeにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast ForceMode to FlexibleForceMode.
		/// </summary>
		/// <param name="value">ForceMode</param>
		/// <returns>Returns the result of casting to FlexibleForceMode.</returns>
#endif
		public static explicit operator FlexibleForceMode(ForceMode value)
		{
			return new FlexibleForceMode(value);
		}
	}
}