//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なLayerMask型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible LayerMask type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleLayerMask : FlexibleField<LayerMask>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleLayerMaskデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleLayerMask default constructor
		/// </summary>
#endif
		public FlexibleLayerMask()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleLayerMaskコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleLayerMask constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleLayerMask(LayerMask value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleLayerMaskコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleLayerMask constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleLayerMask(AnyParameterReference parameter) : base(parameter)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleLayerMaskコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleLayerMask constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleLayerMask(InputSlotAny slot) : base(slot)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleLayerMaskをLayerMaskにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleLayerMask</param>
		/// <returns>LayerMaskにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleLayerMask to LayerMask.
		/// </summary>
		/// <param name="flexible">FlexibleLayerMask</param>
		/// <returns>Returns the result of casting to LayerMask.</returns>
#endif
		public static explicit operator LayerMask(FlexibleLayerMask flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// LayerMaskをFlexibleLayerMaskにキャスト。
		/// </summary>
		/// <param name="value">LayerMask</param>
		/// <returns>FlexibleLayerMaskにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast LayerMask to FlexibleLayerMask.
		/// </summary>
		/// <param name="value">LayerMask</param>
		/// <returns>Returns the result of casting to FlexibleLayerMask.</returns>
#endif
		public static explicit operator FlexibleLayerMask(LayerMask value)
		{
			return new FlexibleLayerMask(value);
		}
	}
}