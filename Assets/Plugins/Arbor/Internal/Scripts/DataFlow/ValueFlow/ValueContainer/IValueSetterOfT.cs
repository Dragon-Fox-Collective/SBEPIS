//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;

namespace Arbor.ValueFlow
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 値が格納できることを示すインターフェイス
	/// </summary>
	/// <typeparam name="T">値の型</typeparam>
#else
	/// <summary>
	/// Interface to indicate that the value can set
	/// </summary>
	/// <typeparam name="T">Value type</typeparam>
#endif
	public interface IValueSetter<T>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 値を格納する
		/// </summary>
		/// <param name="value">設定する値</param>
#else
		/// <summary>
		/// Set value
		/// </summary>
		/// <param name="value">Value to set</param>
#endif
		void SetValue(T value);
	}
}