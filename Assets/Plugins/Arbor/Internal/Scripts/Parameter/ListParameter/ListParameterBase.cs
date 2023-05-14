//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Listを扱うパラメータの基本クラス
	/// </summary>
#else
	/// <summary>
	/// Base class for parameters that handle List
	/// </summary>
#endif
	[System.Serializable]
	public abstract class ListParameterBase
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Listを設定する。
		/// </summary>
		/// <param name="value">設定するList</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Set the List.
		/// </summary>
		/// <param name="value">The List to set</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public abstract bool SetList(IList value);

#if ARBOR_DOC_JA
		/// <summary>
		/// Listのインスタンスオブジェクト
		/// </summary>
#else
		/// <summary>
		/// List instance object
		/// </summary>
#endif
		public abstract object listObject
		{
			get;
		}
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// Listを扱うパラメータの基本クラス
	/// </summary>
	/// <typeparam name="T">Listの要素の型</typeparam>
#else
	/// <summary>
	/// Base class for parameters that handle List
	/// </summary>
	/// <typeparam name="T">List element type</typeparam>
#endif
	[System.Serializable]
	public abstract class ListParameterBaseInternal<T> : ListParameterBase, IValueGetter, IValueGetter<IList<T>>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Listのインスタンス
		/// </summary>
#else
		/// <summary>
		/// List instance
		/// </summary>
#endif
		protected abstract List<T> listInstance
		{
			get;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Listのインスタンスオブジェクト
		/// </summary>
#else
		/// <summary>
		/// List instance object
		/// </summary>
#endif
		public override object listObject
		{
			get
			{
				return listInstance;
			}
		}

		private bool Internal_SetList(IList<T> value)
		{
			List<T> list = listInstance;

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

#if ARBOR_DOC_JA
		/// <summary>
		/// Listを設定する。
		/// </summary>
		/// <param name="value">設定するList</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Set the List.
		/// </summary>
		/// <param name="value">The List to set</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetList(IList<T> value)
		{
			return Internal_SetList(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Listを設定する。
		/// </summary>
		/// <param name="value">設定するList</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Set the List.
		/// </summary>
		/// <param name="value">The List to set</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public override bool SetList(IList value)
		{
			return Internal_SetList((IList<T>)value);
		}

		object IValueGetter.GetValueObject()
		{
			return listInstance;
		}

		IList<T> IValueGetter<IList<T>>.GetValue()
		{
			return listInstance;
		}
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// Listを扱うパラメータの基本クラス
	/// </summary>
	/// <typeparam name="T">Listの要素の型</typeparam>
#else
	/// <summary>
	/// Base class for parameters that handle List
	/// </summary>
	/// <typeparam name="T">List element type</typeparam>
#endif
	[System.Serializable]
	public class ListParameterBase<T> : ListParameterBaseInternal<T>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Listのインスタンス
		/// </summary>
#else
		/// <summary>
		/// List instance
		/// </summary>
#endif
		public List<T> list = new List<T>();

#if ARBOR_DOC_JA
		/// <summary>
		/// Listのインスタンス
		/// </summary>
#else
		/// <summary>
		/// List instance
		/// </summary>
#endif
		protected override sealed List<T> listInstance
		{
			get
			{
				return list;
			}
		}
	}
}
