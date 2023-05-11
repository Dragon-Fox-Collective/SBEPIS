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

#if ARBOR_DOC_JA
	/// <summary>
	/// 配列かListを新規作成する。
	/// </summary>
	/// <remarks>
	/// IL2CPPなどのAOT環境では、List&lt;指定した型&gt;がコード上で使用していないと正常動作しない可能性があります。<br />
	/// 詳しくは、<a href="https://arbor-docs.caitsithware.com/ja/manual/dataflow/list.html#AOTRestrictions">事前コンパイル(AOT)での制限</a>を参照してください。
	/// </remarks>
#else
	/// <summary>
	/// Create a new array or List.
	/// </summary>
	/// <remarks>
	/// In an AOT environment such as IL2CPP, List&lt;specified type&gt; may not work properly unless it is used in the code.<br />
	/// See <a href="https://arbor-docs.caitsithware.com/en/manual/dataflow/list.html#AOTRestrictions">Ahead-of-Time (AOT) Restrictions</a> for more information.
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("List/NewArrayList")]
	[BehaviourTitle("NewArrayList")]
	[BuiltInBehaviour]
	public sealed class NewArrayListCalculator : Calculator, INodeBehaviourSerializationCallbackReceiver
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
		[HideInInspector]
		private ParameterType _ParameterType = ParameterType.Unknown;

#if ARBOR_DOC_JA
		/// <summary>
		/// 新規作成したインスタンスの出力スロット
		/// </summary>
#else
		/// <summary>
		/// Output slot of newly created instance
		/// </summary>
#endif
		[SerializeField]
		[HideSlotFields]
		private OutputSlotTypable _Output = new OutputSlotTypable();

#if ARBOR_DOC_JA
		/// <summary>
		/// 新規作成するインスタンスのタイプ
		/// </summary>
#else
		/// <summary>
		/// Type of newly created instance
		/// </summary>
#endif
		[SerializeField]
		private ArrayListType _OutputType = ArrayListType.Array;

#if ARBOR_DOC_JA
		/// <summary>
		/// 格納する要素のリスト
		/// </summary>
#else
		/// <summary>
		/// List of elements to store
		/// </summary>
#endif
		[SerializeField]
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

			IList parameters = _ParameterList.GetParameterList(_ParameterType);
			if (parameters == null)
			{
				return;
			}

			_Output.SetValue(NewArrayList(elementType, parameters));
		}

		IList CreateInstance(System.Type elementType, int count)
		{
			ListMediator mediator = ValueMediator.Get(elementType).listMediator;

			switch (_OutputType)
			{
				case ArrayListType.Array:
					{
						return mediator.NewArray(count) as IList;
					}
				case ArrayListType.List:
					{
						return mediator.NewList() as IList;
					}
			}

			return null;
		}

		void SetValue(System.Type elementType, IList list, int index, IValueGetter valueContainer)
		{
			var listElementType = ListMediator.GetElementType(list, valueContainer);
			if (listElementType != null)
			{
				elementType = listElementType;
			}

			ListMediator mediator = ValueMediator.Get(elementType).listMediator;

			switch (_OutputType)
			{
				case ArrayListType.Array:
					{
						mediator.SetElement(list, index, valueContainer, ListInstanceType.Keep);
					}
					break;
				case ArrayListType.List:
					{
						mediator.AddElement(list, valueContainer, ListInstanceType.Keep);
					}
					break;
			}
		}

		object NewArrayList(System.Type elementType, IList parameters)
		{
			int count = parameters.Count;
			var list = CreateInstance(elementType, count);
			for (int i = 0; i < count; i++)
			{
				IValueGetter valueContainer = parameters[i] as IValueGetter;
				if (valueContainer == null)
				{
					Debug.LogWarningFormat("parameter[{0}] == null", i.ToString());
					continue;
				}

				SetValue(elementType, list, i, valueContainer);
			}

			return list;
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			_ParameterList.SetOverrideConstraint(_ParameterType, _ElementType.type);
		}
	}
}