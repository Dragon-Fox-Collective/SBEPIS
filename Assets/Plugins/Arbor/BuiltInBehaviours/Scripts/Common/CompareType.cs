//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 比較タイプ
	/// </summary>
#else
	/// <summary>
	/// Compare type
	/// </summary>
#endif
	[Internal.Documentable]
	public enum CompareType
	{
		/// <summary>
		/// Value1 == Value2
		/// </summary>
		Equals,

		/// <summary>
		/// Value1 != Value2
		/// </summary>
		NotEquals,

		/// <summary>
		/// Value1 &gt; Value2
		/// </summary>
		Greater,

		/// <summary>
		/// Value1 &gt;= Value2
		/// </summary>
		GreaterOrEquals,

		/// <summary>
		/// Value1 &lt; Value2
		/// </summary>
		Less,

		/// <summary>
		/// Value1 &lt;= Value2
		/// </summary>
		LessOrEquals,
	}
}