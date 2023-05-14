//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 中止フラグ
	/// </summary>
#else
	/// <summary>
	/// Abort flag
	/// </summary>
#endif
	[Internal.Documentable]
	[System.Flags]
	public enum AbortFlags
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// このノード自体、および配下で動くサブツリーを中止。
		/// </summary>
#else
		/// <summary>
		/// Abort self, and any sub-trees running under this node.
		/// </summary>
#endif
		Self = 0x01,

#if ARBOR_DOC_JA
		/// <summary>
		/// このノードよりも優先度が低いノードをすべて中止。
		/// </summary>
#else
		/// <summary>
		/// Abort any nodes with lower priority than this node.
		/// </summary>
#endif
		LowerPriority = 0x02,
	}
}