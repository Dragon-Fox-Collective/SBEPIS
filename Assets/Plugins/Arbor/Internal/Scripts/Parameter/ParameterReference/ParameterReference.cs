//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using System.Reflection;

namespace Arbor
{
	using Arbor.ValueFlow;
	using Arbor.Internal;

#if ARBOR_DOC_JA
	/// <summary>
	/// パラメータの参照。
	/// </summary>
#else
	/// <summary>
	/// Reference parameters.
	/// </summary>
#endif
	[System.Serializable]
	[Internal.DocumentManual("/manual/scripting/parameterreference/parameterreference.md")]
	public class ParameterReference : IValueContainer, IAssignFieldReceiver, IOverrideConstraint
	{
		[SerializeField]
		private ParameterReferenceType _Type;

		[SerializeField]
		[FormerlySerializedAs("container")]
		private ParameterContainerBase _Container;

		[SerializeField]
		[ClassExtends(typeof(ParameterContainerBase))]
		private InputSlotComponent _Slot = new InputSlotComponent();

#if ARBOR_DOC_JA
		/// <summary>
		/// ID。
		/// </summary>
#else
		/// <summary>
		/// ID.
		/// </summary>
#endif
		public int id;

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータ名。
		/// </summary>
#else
		/// <summary>
		/// Paramenter name.
		/// </summary>
#endif
		public string name;

#if ARBOR_DOC_JA
		/// <summary>
		/// ParameterContainerの参照タイプ
		/// </summary>
#else
		/// <summary>
		/// Reference type of ParameterContainer
		/// </summary>
#endif
		public ParameterReferenceType type
		{
			get
			{
				return _Type;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 格納しているコンテナ。
		/// </summary>
#else
		/// <summary>
		/// Is stored to that container.
		/// </summary>
#endif
		public ParameterContainerBase container
		{
			get
			{
				switch (_Type)
				{
					case ParameterReferenceType.Constant:
						return _Container;
					case ParameterReferenceType.DataSlot:
						return _Slot.GetValue<ParameterContainerBase>();
				}

				return null;
			}
			set
			{
				_Container = value;
				if (_Container != null && _Type == ParameterReferenceType.DataSlot)
				{
					ParameterContainerInternal parameterContainer = _Container.container;
					if (parameterContainer != null)
					{
						id = parameterContainer.GetParamID(name);
					}
				}
				_Type = ParameterReferenceType.Constant;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 参照する<see cref="Parameter.Type"/>を返す。
		/// </summary>
#else
		/// <summary>
		/// Returns the <see cref="Parameter.Type"/> to be referenced.
		/// </summary>
#endif
		[System.Obsolete("use Arbor.Internal.ParameterTypeAttribute.")]
		public virtual Parameter.Type? referenceType
		{
			get
			{
				Internal.ParameterTypeAttribute parameterTypeAttribute = AttributeHelper.GetAttribute<Internal.ParameterTypeAttribute>(GetType());
				if (parameterTypeAttribute != null)
				{
					return parameterTypeAttribute.parameterType;
				}
				return null;
			}
		}

		internal event System.Action<Parameter> onGetParameter;

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータを取得する。存在しない場合はnull。
		/// </summary>
#else
		/// <summary>
		/// Get the parameters. null if it does not exist.
		/// </summary>
#endif
		public Parameter parameter
		{
			get
			{
				ParameterContainerBase containerBase = container;
				if (containerBase == null)
				{
					return null;
				}
				ParameterContainerInternal parameterContainer = containerBase.container;

				if (parameterContainer == null)
				{
					return null;
				}

				var parameter = (_Type == ParameterReferenceType.Constant) ? parameterContainer.GetParam(id) : parameterContainer.GetParam(name);

				if (parameter != null)
				{
					onGetParameter?.Invoke(parameter);
				}

				return parameter;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 定数指定しているコンテナ
		/// </summary>
#else
		/// <summary>
		/// Container specifying a constant
		/// </summary>
#endif
		public ParameterContainerBase constantContainer
		{
			get
			{
				return _Container;
			}
			set
			{
				_Container = value;
			}
		}

		internal void Copy(ParameterReference parameterReference)
		{
			_Type = parameterReference._Type;
			_Container = parameterReference._Container;
			_Slot = parameterReference._Slot;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// データスロットの接続を切断する。
		/// </summary>
#else
		/// <summary>
		/// Disconnect the data slot.
		/// </summary>
#endif
		public void Disconnect()
		{
			switch (_Type)
			{
				case ParameterReferenceType.DataSlot:
					_Slot.Disconnect();
					break;
			}
		}

		object IValueGetter.GetValueObject()
		{
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				return parameter.value;
			}

			return null;
		}

		void IValueSetter.SetValueObject(object value)
		{
			Parameter parameter = this.parameter;
			if (parameter != null)
			{
				parameter.value = value;
			}
		}

		private ClassConstraintInfo _FieldConstraintInfo;
		private ClassConstraintInfo _OverrideConstraintInfo;
		private ParameterConstraintAttributeBase[] _ParameterConstraintAttributes;

#if ARBOR_DOC_JA
		/// <summary>
		/// 上書きする型制約の情報
		/// </summary>
#else
		/// <summary>
		/// override ClassConstraintInfo
		/// </summary>
#endif
		public ClassConstraintInfo overrideConstraint
		{
			get
			{
				return _OverrideConstraintInfo;
			}
			set
			{
				if (_OverrideConstraintInfo != value)
				{
					_OverrideConstraintInfo = value;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメーターのタイプを制約する属性を取得する。
		/// </summary>
		/// <returns>パラメーターのタイプを制約する属性の配列。</returns>
#else
		/// <summary>
		/// Gets the attribute that constrains the type of parameter.
		/// </summary>
		/// <returns>An array of attributes that constrain the type of parameter.</returns>
#endif
		public ParameterConstraintAttributeBase[] GetConstraintAttributes()
		{
			return _ParameterConstraintAttributes;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 型制約の情報を返す。
		/// </summary>
		/// <returns>型制約の情報</returns>
#else
		/// <summary>
		/// Return information on type constraints.
		/// </summary>
		/// <returns>Type constraint information</returns>
#endif
		public ClassConstraintInfo GetConstraint()
		{
			return _OverrideConstraintInfo ?? _FieldConstraintInfo;
		}

		void IAssignFieldReceiver.OnAssignField(Object ownerObject, FieldInfo fieldInfo)
		{
			System.Type elementType = Arbor.Serialization.SerializationUtility.ElementType(fieldInfo.FieldType);

			_ParameterConstraintAttributes = AttributeHelper.GetAttributes<ParameterConstraintAttributeBase>(elementType);

			for (int attrIndex = 0; attrIndex < _ParameterConstraintAttributes.Length; attrIndex++)
			{
				ParameterConstraintAttributeBase attr = _ParameterConstraintAttributes[attrIndex];
				IConstraintableAttribute constraintableAttribute = attr as IConstraintableAttribute;
				if (constraintableAttribute != null)
				{
					ClassTypeConstraintAttribute classTypeConstraintAttribute = AttributeHelper.GetAttribute<ClassTypeConstraintAttribute>(fieldInfo);
					if (classTypeConstraintAttribute != null)
					{
						_FieldConstraintInfo = new ClassConstraintInfo() { constraintAttribute = classTypeConstraintAttribute, constraintFieldInfo = fieldInfo };
					}
					else
					{
						SlotTypeAttribute slotTypeAttribute = AttributeHelper.GetAttribute<SlotTypeAttribute>(fieldInfo);
						if (slotTypeAttribute != null && constraintableAttribute.IsConstraintSatisfied(slotTypeAttribute.connectableType))
						{
							_FieldConstraintInfo = new ClassConstraintInfo() { slotTypeAttribute = slotTypeAttribute };
						}
					}
					break;
				}
			}
		}
	}
}
