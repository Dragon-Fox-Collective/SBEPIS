//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 姿勢タイプ
	/// </summary>
#else
	/// <summary>
	/// Posture type
	/// </summary>
#endif
	[Internal.Documentable]
	public enum PostureType
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Transformで指定。
		/// </summary>
#else
		/// <summary>
		/// Specified by Transform.
		/// </summary>
#endif
		Transform,

#if ARBOR_DOC_JA
		/// <summary>
		/// 直接値を指定
		/// </summary>
#else
		/// <summary>
		/// Specify the value directly
		/// </summary>
#endif
		Directly,
	}
}