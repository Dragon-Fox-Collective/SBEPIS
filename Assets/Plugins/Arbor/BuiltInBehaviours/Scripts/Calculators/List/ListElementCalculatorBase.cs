//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Arbor.Calculators
{
	using Arbor.Events;
	using Arbor.ValueFlow;

	public abstract class ListElementCalculatorBase : Calculator, INodeBehaviourSerializationCallbackReceiver
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 要素の型
		/// </summary>
#else
		/// <summary>
		/// Element type
		/// </summary>
#endif
		[SerializeField]
		[ClassNotStaticConstraint]
		[TypeFilter((TypeFilterFlags)(-1) & ~(TypeFilterFlags.Static))]
		private ClassTypeReference _ElementType = new ClassTypeReference();

		[SerializeField]
		[Internal.HideInDocument]
		private ParameterType _ParameterType = ParameterType.Unknown;

#if ARBOR_DOC_JA
		/// <summary>
		/// Listの入力スロット
		/// </summary>
#else
		/// <summary>
		/// List input slot
		/// </summary>
#endif
		[SerializeField]
		[HideSlotFields]
		private InputSlotTypable _Input = new InputSlotTypable();

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
		private ParameterList _ParameterList = new ParameterList();

		public sealed override void OnCalculate()
		{
			System.Type elementType = _ElementType.type;

			if (elementType == null)
			{
				if (!string.IsNullOrEmpty(_ElementType.assemblyTypeName))
				{
					Debug.LogError("Type not found. It may have been deleted or renamed : " + this, this);
				}
				else
				{
					Debug.LogError("The element type is not specified : " + this, this);
				}

				return;
			}

			ParameterType parameterType = ArborEventUtility.GetParameterType(elementType, true);

			if (_ParameterType != parameterType)
			{
				Debug.LogError("The parameter type has changed : " + this, this);
				return;
			}

			object array = null;
			if (!_Input.GetValue(ref array))
			{
				return;
			}

			IList list = array as IList;
			if (list == null)
			{
				return;
			}

			IList parameterList = _ParameterList.GetParameterList(_ParameterType);
			IValueGetter valueContainer = null;
			if (parameterList != null && parameterList.Count != 0)
			{
				valueContainer = parameterList[0] as IValueGetter;

				if (valueContainer != null)
				{
					var listElementType = ListMediator.GetElementType(list, valueContainer);
					if (listElementType != null)
					{
						elementType = listElementType;
					}
				}
			}

			ListMediator mediator = ValueMediator.Get(elementType).listMediator;
			OnExecute(mediator, list, valueContainer);
		}

		protected abstract void OnExecute(ListMediator mediator, IList list, IValueGetter valueContainer);

		protected virtual void OnAfterDeserialize()
		{
		}

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			OnAfterDeserialize();

			_ParameterList.SetOverrideConstraint(_ParameterType, _ElementType.type);
		}

		protected virtual void OnBeforeSerialize()
		{
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
			OnBeforeSerialize();
		}
	}
}