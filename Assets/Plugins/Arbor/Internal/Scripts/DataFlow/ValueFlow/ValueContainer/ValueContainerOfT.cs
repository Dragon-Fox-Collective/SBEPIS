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
	/// 値を格納するクラス。ValueMediatorやListMediatorを仲介することでボックス化を回避したアクセスが可能となる。
	/// </summary>
	/// <typeparam name="T">値の型</typeparam>
#else
	/// <summary>
	/// A class that stores values, which can be accessed without boxing by using ValueMediator or ListMediator as intermediaries.
	/// </summary>
	/// <typeparam name="T">Value type</typeparam>
#endif
	public class ValueContainer<T> : IValueContainer<T>, IValueContainer
	{
		private T _Value;

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を取得する
		/// </summary>
		/// <returns>格納されている値</returns>
#else
		/// <summary>
		/// Get value
		/// </summary>
		/// <returns>Stored value</returns>
#endif
		public T GetValue()
		{
			return _Value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を取得する
		/// </summary>
		/// <returns>格納されている値</returns>
#else
		/// <summary>
		/// Get value
		/// </summary>
		/// <returns>Stored value</returns>
#endif
		public object GetValueObject()
		{
			return GetValue();
		}

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
		public void SetValue(T value)
		{
			_Value = value;
		}

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
		public void SetValueObject(object value)
		{
			SetValue((T)value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を文字列に変換する。
		/// </summary>
		/// <returns>変換した文字列</returns>
#else
		/// <summary>
		/// Convert a value to a string.
		/// </summary>
		/// <returns>Converted string</returns>
#endif
		public override string ToString()
		{
			return _Value.ToString();
		}
	}
}
