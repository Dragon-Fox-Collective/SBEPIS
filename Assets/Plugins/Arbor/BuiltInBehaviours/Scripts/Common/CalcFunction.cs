//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	[Internal.Documentable]
	public enum CalcFunction
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 値を代入する。
		/// </summary>
#else
		/// <summary>
		/// Substitute values.
		/// </summary>
#endif
		Assign,

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を加算する。<br/>
		/// 減算したい場合は負値を指定する。
		/// </summary>
#else
		/// <summary>
		/// Add values.<br/>
		/// To subtract it, specify a negative value.
		/// </summary>
#endif
		Add,
	}
}
