//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor.Events
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 引数の属性フラグ
	/// </summary>
#else
	/// <summary>
	/// Attribute flag of argument
	/// </summary>
#endif
	[System.Serializable]
	[System.Flags]
	public enum ArgumentAttributes
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// なし
		/// </summary>
#else
		/// <summary>
		/// None
		/// </summary>
#endif
		None = 0,

#if ARBOR_DOC_JA
		/// <summary>
		/// 出力
		/// </summary>
#else
		/// <summary>
		/// Output
		/// </summary>
#endif
		Out = 0x01,
	}
}