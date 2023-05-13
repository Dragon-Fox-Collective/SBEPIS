//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なBoundsInt型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible BoundsInt type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleBoundsInt : FlexibleField<BoundsInt>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleBoundsIntデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleBoundsInt default constructor
		/// </summary>
#endif
		public FlexibleBoundsInt()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleBoundsIntコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleBoundsInt constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleBoundsInt(BoundsInt value) : base(value)
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleBoundsIntコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleVector2 constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleBoundsInt(BoundsIntParameterReference parameter) : base(new AnyParameterReference(parameter))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleBoundsIntコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleBoundsInt constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleBoundsInt(InputSlotBoundsInt slot) : base(new InputSlotAny(slot))
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleBoundsIntをBoundsIntにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleBoundsInt</param>
		/// <returns>BoundsIntにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleBoundsInt to BoundsInt.
		/// </summary>
		/// <param name="flexible">FlexibleBoundsInt</param>
		/// <returns>Returns the result of casting to BoundsInt.</returns>
#endif
		public static explicit operator BoundsInt(FlexibleBoundsInt flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntをFlexibleBoundsIntにキャスト。
		/// </summary>
		/// <param name="value">BoundsInt</param>
		/// <returns>FlexibleBoundsIntにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast BoundsInt to FlexibleBoundsInt.
		/// </summary>
		/// <param name="value">BoundsInt</param>
		/// <returns>Returns the result of casting to FlexibleBoundsInt.</returns>
#endif
		public static explicit operator FlexibleBoundsInt(BoundsInt value)
		{
			return new FlexibleBoundsInt(value);
		}
	}
}