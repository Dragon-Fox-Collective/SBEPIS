//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// VariableListのジェネリッククラス。
	/// </summary>
	/// <typeparam name="T">VariableListに格納するパラメータの型</typeparam>
#else
	/// <summary>
	/// A generic class of VariableList.
	/// </summary>
	/// <typeparam name="T">Type of parameter to be stored in VariableList</typeparam>
#endif
	public abstract class VariableList<T> : VariableListBase
	{
		[SerializeField]
		private List<T> _Parameter = new List<T>();

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
				return typeof(IList<T>);
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
				Internal_SetList((IList<T>)value);
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
		public IList<T> value
		{
			get
			{
				return _Parameter;
			}
			set
			{
				Internal_SetList(value);
			}
		}

		private bool Internal_SetList(IList<T> value)
		{
			List<T> list = _Parameter;

			if (value == null || ListUtility.Equals(list, value))
			{
				return false;
			}

			list.Clear();
			for (int i = 0; i < value.Count; i++)
			{
				list.Add(value[i]);
			}

			return true;
		}
	}
}