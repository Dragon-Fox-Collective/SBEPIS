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
	/// 値を格納していることを定義するインターフェイス
	/// </summary>
#else
	/// <summary>
	/// Interface that defines that the value is stored.
	/// </summary>
#endif
	public interface IValueContainer : IValueGetter, IValueSetter
	{
	}
}