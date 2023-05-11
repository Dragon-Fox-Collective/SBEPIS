//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 更新メソッドのタイプ
	/// </summary>
#else
	/// <summary>
	/// Update method type
	/// </summary>
#endif
	[Internal.Documentable]
	public enum UpdateMethodType
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Updateメソッド
		/// </summary>
#else
		/// <summary>
		/// Update method
		/// </summary>
#endif
		Update,

#if ARBOR_DOC_JA
		/// <summary>
		/// OnStateUpdateメソッド
		/// </summary>
#else
		/// <summary>
		/// OnStateUpdate method
		/// </summary>
#endif
		StateUpdate,
	}
}