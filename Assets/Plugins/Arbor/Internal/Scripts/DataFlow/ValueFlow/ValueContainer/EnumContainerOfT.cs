//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.ValueFlow
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Enum型の値を格納するクラス。ValueMediatorやListMediatorを仲介することでボックス化を回避したアクセスが可能となる。
	/// </summary>
	/// <typeparam name="T">Enumの型</typeparam>
#else
	/// <summary>
	/// A class that stores an Enum type value. By mediating ValueMediator and ListMediator, access that avoids boxing becomes possible.
	/// </summary>
	/// <typeparam name="T">Enum type</typeparam>
#endif
	public class EnumContainer<T> : ValueContainer<T>, IValueContainer<int> where T : System.Enum
	{
		int IValueGetter<int>.GetValue()
		{
			T value = GetValue();
			return EnumFieldUtility.ToInt<T>(value);
		}

		void IValueSetter<int>.SetValue(int value)
		{
			SetValue(EnumFieldUtility.ToEnum<T>(value));
		}
	}
}