//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Hierarchyから参照するタイプ
	/// </summary>
#else
	/// <summary>
	/// Type referenced from Hierarchy
	/// </summary>
#endif
	public enum FlexibleHierarchyType
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 自身のオブジェクト
		/// </summary>
#else
		/// <summary>
		/// Own object
		/// </summary>
#endif
		Self,

#if ARBOR_DOC_JA
		/// <summary>
		/// ルートグラフのオブジェクト
		/// </summary>
#else
		/// <summary>
		/// Root graph object
		/// </summary>
#endif
		RootGraph,

#if ARBOR_DOC_JA
		/// <summary>
		/// 親グラフのオブジェクト
		/// </summary>
#else
		/// <summary>
		/// Parent graph object
		/// </summary>
#endif
		ParentGraph,
	}
}