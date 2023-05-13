//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ParameterReferenceクラスで使用する参照タイプ
	/// </summary>
#else
	/// <summary>
	/// Reference type used in ParameterReference class
	/// </summary>
#endif
	public enum ParameterReferenceType
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
		/// データスロット
		/// </summary>
#else
		/// <summary>
		/// Data slot
		/// </summary>
#endif
		DataSlot,
	}
}