//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Flexibleクラスで使用する参照タイプ
	/// </summary>
#else
	/// <summary>
	/// Reference type used in Flexible class
	/// </summary>
#endif
	public enum FlexibleType
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 定数
		/// </summary>
#else
		/// <summary>
		/// Constant
		/// </summary>
#endif
		Constant,

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータ
		/// </summary>
#else
		/// <summary>
		/// Parameter
		/// </summary>
#endif
		Parameter,

#if ARBOR_DOC_JA
		/// <summary>
		/// データスロット
		/// </summary>
#else
		/// <summary>
		/// Data slot
		/// </summary>
#endif
		DataSlot,

#if ARBOR_DOC_JA
		/// <summary>
		/// データスロット
		/// </summary>
#else
		/// <summary>
		/// Data slot
		/// </summary>
#endif
		[System.Obsolete("use FlexibleType.DataSlot")]
		Calculator = DataSlot,
	}
}