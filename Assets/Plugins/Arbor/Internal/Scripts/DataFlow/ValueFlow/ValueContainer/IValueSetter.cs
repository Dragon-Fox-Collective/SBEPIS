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
#else
	/// <summary>
	/// Interface to indicate that the value can set
	/// </summary>
#endif
	public interface IValueSetter
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
		void SetValueObject(object value);
	}
}