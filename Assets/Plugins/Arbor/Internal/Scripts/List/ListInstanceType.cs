//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Listインスタンスをどのように変更するかを指定する。
	/// </summary>
#else
	/// <summary>
	/// Specify how to modify the List instance.
	/// </summary>
#endif
	[Internal.Documentable]
	public enum ListInstanceType
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 指定したインスタンスをそのまま変更する。
		/// </summary>
#else
		/// <summary>
		/// Change the specified instance as it is.
		/// </summary>
#endif
		Keep,

#if ARBOR_DOC_JA
		/// <summary>
		/// 配列を新規作成して変更する。
		/// </summary>
#else
		/// <summary>
		/// Create a new array and change it.
		/// </summary>
#endif
		NewArray,

#if ARBOR_DOC_JA
		/// <summary>
		/// Listを新規作成して変更する。
		/// </summary>
#else
		/// <summary>
		/// Create a new List and change it.
		/// </summary>
#endif
		NewList,
	}
}