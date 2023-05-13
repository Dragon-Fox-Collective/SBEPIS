//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.Internal
{
#if ARBOR_DOC_JA
	/// <summary>
	/// NodeBehaviourの内部ユーティリティクラス。内部的な呼び出しを直接行いたい場合に使用する。
	/// </summary>
#else
	/// <summary>
	/// NodeBehaviour's internal utility class. Use when you want to make an internal call directly.
	/// </summary>
#endif
	public static class NodeBehaviourInternalUtility
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// グラフを一時停止したイベントを呼び出す。
		/// </summary>
		/// <param name="nodeBehaviour">NodeBehaviour</param>
#else
		/// <summary>
		/// Call the event that paused the graph.
		/// </summary>
		/// <param name="nodeBehaviour">NodeBehaviour</param>
#endif
		public static void CallPauseGraphEvent(NodeBehaviour nodeBehaviour)
		{
			nodeBehaviour.CallPauseEvent();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// グラフの一時停止から再開したイベントを呼び出す。
		/// </summary>
		/// <param name="nodeBehaviour">NodeBehaviour</param>
#else
		/// <summary>
		/// Call the event that resumed from the pause of the graph.
		/// </summary>
		/// <param name="nodeBehaviour">NodeBehaviour</param>
#endif
		public static void CallResumeGraphEvent(NodeBehaviour nodeBehaviour)
		{
			nodeBehaviour.CallResumeEvent();
		}
	}
}