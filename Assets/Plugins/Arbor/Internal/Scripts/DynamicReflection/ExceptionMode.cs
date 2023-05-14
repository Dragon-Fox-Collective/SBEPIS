//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.DynamicReflection
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 発生した例外の対処方法を指定する
	/// </summary>
#else
	/// <summary>
	/// Specify what to do with the exception that occurred
	/// </summary>
#endif
	public enum ExceptionMode
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 発生した例外を再び投げる
		/// </summary>
#else
		/// <summary>
		/// Throw the exception that occurred again
		/// </summary>
#endif
		Throw,

#if ARBOR_DOC_JA
		/// <summary>
		/// 発生した例外のログを出力するのみ
		/// </summary>
#else
		/// <summary>
		/// Only output a log of the exception that occurred
		/// </summary>
#endif
		Log,

#if ARBOR_DOC_JA
		/// <summary>
		/// 発生した例外を無視する
		/// </summary>
#else
		/// <summary>
		/// Ignore the exception that occurred
		/// </summary>
#endif
		Ignore,
	}
}