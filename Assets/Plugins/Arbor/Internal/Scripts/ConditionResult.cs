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
	/// 条件判定の結果
	/// </summary>
#else
	/// <summary>
	/// Result of condition judgment
	/// </summary>
#endif
	public enum ConditionResult
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// コンディションなし
		/// </summary>
#else
		/// <summary>
		/// No condition
		/// </summary>
#endif
		None,

#if ARBOR_DOC_JA
		/// <summary>
		/// 成功
		/// </summary>
#else
		/// <summary>
		/// Success
		/// </summary>
#endif
		Success,

#if ARBOR_DOC_JA
		/// <summary>
		/// 失敗
		/// </summary>
#else
		/// <summary>
		/// Failure
		/// </summary>
#endif
		Failure,
	}
}