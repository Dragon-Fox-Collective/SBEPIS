//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Transformの更新タイミング
	/// </summary>
#else
	/// <summary>
	/// Transform update timing
	/// </summary>
#endif
	[System.Flags]
	[Arbor.Internal.Documentable]
	[System.Obsolete("use ExecuteMethodFlags")]
	public enum TransformUpdateTiming
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// ノードに入った時に更新する。
		/// </summary>
#else
		/// <summary>
		/// Update when entering the node.
		/// </summary>
#endif
		Enter = 0x01,

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードが更新されるときに更新する。
		/// </summary>
#else
		/// <summary>
		/// Update when the node is updated.
		/// </summary>
#endif
		Update = 0x02,

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードが更新された後に更新する。
		/// </summary>
#else
		/// <summary>
		/// Update the node is late updated.
		/// </summary>
#endif
		LateUpdate = 0x04,

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードから抜けるときに更新する。
		/// </summary>
#else
		/// <summary>
		/// Update when leaving a node.
		/// </summary>
#endif
		Leave = 0x08,
	}
}