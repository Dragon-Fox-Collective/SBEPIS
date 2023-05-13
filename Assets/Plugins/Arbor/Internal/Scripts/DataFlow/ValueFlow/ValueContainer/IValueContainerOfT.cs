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
	/// 値を格納していることを定義するインターフェイス(ジェネリック版)
	/// </summary>
	/// <typeparam name="T">値の型</typeparam>
#else
	/// <summary>
	/// Interface that defines that the value is stored.(Generic version)
	/// </summary>
	/// <typeparam name="T">Value type</typeparam>
#endif
	public interface IValueContainer<T> : IValueGetter<T>, IValueSetter<T>
	{
	}
}