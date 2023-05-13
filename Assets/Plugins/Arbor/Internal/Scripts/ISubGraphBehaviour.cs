//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// NodeBehaviourがNodeGraphの入れ物である場合に使用するインターフェイス
	/// </summary>
#else
	/// <summary>
	/// Interface to use if NodeBehaviour is a NodeGraph container
	/// </summary>
#endif
	public interface ISubGraphBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 外部グラフを参照しているかを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns whether an external graph is referenced.
		/// </summary>
#endif
		bool isExternal
		{
			get;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照しているNodeGraphを取得する。
		/// </summary>
		/// <returns>NodeGraph</returns>
#else
		/// <summary>
		/// Get the referenced NodeGraph.
		/// </summary>
		/// <returns>NodeGraph</returns>
#endif
		NodeGraph GetSubGraph();
	}
}