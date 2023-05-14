//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Variableのジェネリッククラス。
	/// </summary>
	/// <typeparam name="T">Variableに格納するパラメータの型</typeparam>
#else
	/// <summary>
	/// A generic class of Variable.
	/// </summary>
	/// <typeparam name="T">Type of parameter to be stored in Variable</typeparam>
#endif
	public abstract class Variable<T> : VariableBase, IValueGetter<T>
	{
		[SerializeField]
		private T _Parameter = default(T);

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータの値の型
		/// </summary>
#else
		/// <summary>
		/// Value type of parameter
		/// </summary>
#endif
		public override sealed System.Type valueType
		{
			get
			{
				return typeof(T);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値のオブジェクト
		/// </summary>
#else
		/// <summary>
		/// Value object
		/// </summary>
#endif
		public override sealed object valueObject
		{
			get
			{
				return _Parameter;
			}
			set
			{
				_Parameter = (T)value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Variableの値
		/// </summary>
#else
		/// <summary>
		/// Value of Variable
		/// </summary>
#endif
		public T value
		{
			get
			{
				return _Parameter;
			}
			set
			{
				_Parameter = value;
			}
		}

		T IValueGetter<T>.GetValue()
		{
			return value;
		}
	}
}