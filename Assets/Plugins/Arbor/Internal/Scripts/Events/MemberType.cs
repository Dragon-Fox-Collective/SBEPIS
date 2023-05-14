//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor.Events
{
#if ARBOR_DOC_JA
	/// <summary>
	/// メンバーのタイプ
	/// </summary>
#else
	/// <summary>
	/// Member type
	/// </summary>
#endif
	public enum MemberType
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// メソッド
		/// </summary>
#else
		/// <summary>
		/// Method
		/// </summary>
#endif
		Method,

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールド
		/// </summary>
#else
		/// <summary>
		/// Field
		/// </summary>
#endif
		Field,

#if ARBOR_DOC_JA
		/// <summary>
		/// プロパティ
		/// </summary>
#else
		/// <summary>
		/// Property
		/// </summary>
#endif
		Property,
	}
}