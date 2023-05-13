//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// DataLinkの更新タイミング
	/// </summary>
#else
	/// <summary>
	/// DataLink update timing
	/// </summary>
#endif
	[System.Flags]
	public enum DataLinkUpdateTiming
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// ノードに入った時に更新する。
		/// </summary>
#else
		/// <summary>
		/// Update when entering a node.
		/// </summary>
#endif
		Enter = 0x01,

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードが実行される時に更新する。
		/// </summary>
#else
		/// <summary>
		/// Update when the node is executed.
		/// </summary>
#endif
		Execute = 0x02,

#if ARBOR_DOC_JA
		/// <summary>
		/// 手動で更新する。
		/// 更新するには、<see cref="NodeBehaviour.UpdateDataLink()"/>を使用する。
		/// </summary>
#else
		/// <summary>
		/// Manually update.
		/// Use <see cref="NodeBehaviour.UpdateDataLink()"/> to update.
		/// </summary>
#endif
		Manual = 0x04,
	}
}