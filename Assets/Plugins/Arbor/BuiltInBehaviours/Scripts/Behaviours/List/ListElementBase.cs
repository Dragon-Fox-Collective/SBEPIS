//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.StateMachine.StateBehaviours
{
	using Arbor.Events;
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Listに要素を設定する挙動の基本クラス
	/// </summary>
#else
	/// <summary>
	/// Base class for behavior of setting elements in List
	/// </summary>
#endif
	public abstract class ListElementBase : ListBehaviourBase, INodeBehaviourSerializationCallbackReceiver
	{
		[SerializeField]
		[HideInInspector]
		private ParameterType _ParameterType = ParameterType.Unknown;

#if ARBOR_DOC_JA
		/// <summary>
		/// 要素
		/// </summary>
#else
		/// <summary>
		/// Element
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentLabel("Element")]
		[Internal.DocumentOrder(1000)]
		private ParameterList _ParameterList = new ParameterList();

		protected sealed override object OnExecute(System.Type elementType, IList list, ListInstanceType outputType)
		{
			ParameterType parameterType = ArborEventUtility.GetParameterType(elementType, true);

			if (_ParameterType != parameterType)
			{
				Debug.LogError("The parameter type has changed : " + this, this);
				return null;
			}

			IList parameterList = _ParameterList.GetParameterList(_ParameterType);
			if (parameterList == null)
			{
				return null;
			}

			int count = parameterList.Count;
			if (count == 0)
			{
				return null;
			}

			IValueGetter valueContainer = parameterList[0] as IValueGetter;

			if (valueContainer == null)
			{
				return null;
			}

			System.Type listElementType = ListMediator.GetElementType(list, valueContainer);

			if (listElementType != null)
			{
				elementType = listElementType;
			}

			ListMediator mediator = ValueMediator.Get(elementType).listMediator;

			return OnExecute(mediator, list, valueContainer, outputType);
		}

		protected abstract object OnExecute(ListMediator mediator, IList list, IValueGetter valueContainer, ListInstanceType outputType);

		protected virtual void OnAfterDeserialize()
		{
		}

		protected virtual void OnBeforeSerialize()
		{
			_ParameterList.SetOverrideConstraint(_ParameterType, elementType);
		}

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			OnAfterDeserialize();
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
			OnBeforeSerialize();
		}
	}
}