//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Agentの終了フラグ
	/// </summary>
#else
	/// <summary>
	/// Agent finish flag
	/// </summary>
#endif
	[System.Flags]
	[Internal.Documentable]
	public enum AgentFinishFlags
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Agentが移動完了したときにアクションを終了する。
		/// </summary>
#else
		/// <summary>
		/// Finish the action when the Agent completes the move.
		/// </summary>
#endif
		OnDone = 0x01,

#if ARBOR_DOC_JA
		/// <summary>
		/// Agentが移動できなかった時にアクションを終了する。
		/// </summary>
#else
		/// <summary>
		/// Finish the action when the Agent cannot move.
		/// </summary>
#endif
		OnCantMove = 0x02,

#if ARBOR_DOC_JA
		/// <summary>
		/// Agentが移動できなかった時にアクションを成功として返す。このフラグを設定しない場合は失敗として返す。
		/// </summary>
#else
		/// <summary>
		/// Returns the action as successful when the Agent could not be moved. If this flag is not set, it will be returned as a failure.
		/// </summary>
#endif
		ReturnSuccessOnCantMove = 0x04,
	}
}