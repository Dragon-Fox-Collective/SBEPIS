//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Listを扱うパラメータのAOT対応クラス
	/// </summary>
#else
	/// <summary>
	/// AOT supported class for parameters that handle List
	/// </summary>
#endif
	public class ListParameterAOT : ListParameterBase, IValueGetter
	{
		private System.Type _ElementType;
		private ListMediator _Mediator;
		private IList _ListInstance;

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
				return _ListInstance;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ListParameterAOTコンストラクタ
		/// </summary>
		/// <param name="elementType">要素の型</param>
#else
		/// <summary>
		/// ListParameterAOT constructor
		/// </summary>
		/// <param name="elementType">Element type</param>
#endif
		public ListParameterAOT(System.Type elementType)
		{
			_ElementType = elementType;
			_Mediator = ValueMediator.Get(elementType).listMediator;
			_ListInstance = _Mediator.NewList();
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
			if (value == null)
			{
				return false;
			}

			var valueType = value.GetType();
			if (!TypeUtility.IsAssignableFrom(ListUtility.GetIListType(_ElementType), valueType))
			{
				return false;
			}

			if (_Mediator.EqualsList(_ListInstance, value))
			{
				return false;
			}

			_Mediator.Clear(_ListInstance, ListInstanceType.Keep);
			_Mediator.AddRange(_ListInstance, value);

			return true;
		}

		object IValueGetter.GetValueObject()
		{
			return _ListInstance;
		}
	}
}