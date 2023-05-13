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
	/// 型制約を上書きするインターフェイス
	/// </summary>
#else
	/// <summary>
	/// Interface overriding type constraints
	/// </summary>
#endif
	public interface IOverrideConstraint
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 上書きする型制約の情報
		/// </summary>
#else
		/// <summary>
		/// override ClassConstraintInfo
		/// </summary>
#endif
		ClassConstraintInfo overrideConstraint
		{
			get; set;
		}
	}
}