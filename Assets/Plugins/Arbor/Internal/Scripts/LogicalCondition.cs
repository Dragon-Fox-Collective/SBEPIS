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
	/// コンディションの論理演算
	/// </summary>
#else
	/// <summary>
	/// Logical operation of condition
	/// </summary>
#endif
	[System.Serializable]
	[Internal.Documentable]
	public struct LogicalCondition
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 論理演算
		/// </summary>
#else
		/// <summary>
		/// Logical operation
		/// </summary>
#endif
		public LogicalOperation logicalOperation;

#if ARBOR_DOC_JA
		/// <summary>
		/// NOT演算フラグ
		/// </summary>
#else
		/// <summary>
		/// NOT operation flag
		/// </summary>
#endif
		public bool notOp;
	}
}