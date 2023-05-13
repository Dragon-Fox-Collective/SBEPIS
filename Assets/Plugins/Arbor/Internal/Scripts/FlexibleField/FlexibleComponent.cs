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
	/// 参照方法が複数ある柔軟なComponent型を扱うクラス。
	/// </summary>
	/// <remarks>
	/// 使用可能な属性 : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// Class to handle a flexible Component type reference method there is more than one.
	/// </summary>
	/// <remarks>
	/// Available Attributes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// </remarks>
#endif
	[System.Serializable]
	public sealed class FlexibleComponent : FlexibleSceneObjectBase, IValueGetter<Component>, IOverrideConstraint, ISerializationCallbackReceiver
	{
		[SerializeField] private Component _Value = null;

		[SerializeField] private ComponentParameterReference _Parameter = new ComponentParameterReference();

		[SerializeField] private InputSlotComponent _Slot = new InputSlotComponent();

		[HideInInspector]
		[SerializeField] private ClassTypeReference _OverrideConstraintType = new ClassTypeReference();

		private NodeGraph _CachedTargetGraph = null;
		private Component _CachedComponent = null;

#if ARBOR_DOC_JA
		/// <summary>
		/// Parameterを返す。TypeがParameter以外の場合はnull。
		/// </summary>
#else
		/// <summary>
		/// It return a Paramter. It is null if Type is other than Parameter.
		/// </summary>
#endif
		public Parameter parameter
		{
			get
			{
				if (_Type == FlexibleSceneObjectType.Parameter)
				{
					return _Parameter.parameter;
				}
				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を返す
		/// </summary>
#else
		/// <summary>
		/// It returns a value
		/// </summary>
#endif
		public Component value
		{
			get
			{
				Component value = null;
				switch (_Type)
				{
					case FlexibleSceneObjectType.Constant:
						value = _Value;
						break;
					case FlexibleSceneObjectType.Parameter:
						value = _Parameter.value;
						break;
					case FlexibleSceneObjectType.DataSlot:
						_Slot.GetValue(ref value);
						break;
					case FlexibleSceneObjectType.Hierarchy:
						{
							if (_CachedTargetGraph == null || _CachedComponent == null)
							{
								_CachedComponent = null;
								_CachedTargetGraph = this.targetGraph;
								if (_CachedTargetGraph != null)
								{
									// connectableBaseType is NodeGraph?
									if (TypeUtility.IsAssignableFrom(connectableBaseType, _CachedTargetGraph.GetType()))
									{
										_CachedComponent = _CachedTargetGraph;
									}

									if (_CachedComponent == null)
									{
										// connectableBaseType is ParameterContainer?
										ParameterContainerInternal parameterContainer = _CachedTargetGraph.parameterContainer;
										if (parameterContainer != null && TypeUtility.IsAssignableFrom(connectableBaseType, parameterContainer.GetType()))
										{
											_CachedComponent = parameterContainer;
										}
									}

									if (_CachedComponent == null)
									{
										_CachedComponent = _CachedTargetGraph.GetComponent(connectableBaseType);
									}
								}
							}
							value = _CachedComponent;
						}
						break;
				}

				return value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 型制約の上書き。<br/>
		/// Typeが<see cref="FlexibleSceneObjectType.Hierarchy"/>の場合にGetComponentを行う際の型に使用される。
		/// </summary>
#else
		/// <summary>
		/// Overriding type constraints.<br/>
		/// Used for GetComponent when Type is <see cref="FlexibleSceneObjectType.Hierarchy"/>.
		/// </summary>
#endif
		public System.Type overrideConstraintType
		{
			get
			{
				return _OverrideConstraintType.type;
			}
			set
			{
				if (_OverrideConstraintType.type != value)
				{
					_OverrideConstraintType.type = value;

					if (value != null)
					{
						if (_OverrideTypeConstraintInfo == null)
						{
							_OverrideTypeConstraintInfo = new ClassConstraintInfo();
						}
						_OverrideTypeConstraintInfo.baseType = value;
					}
					else
					{
						_OverrideTypeConstraintInfo = null;
					}

					SetInternalConstraint(GetConstraint());

					_CachedComponent = null;
				}
			}
		}

		private ClassConstraintInfo _OverrideConstraint;

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
				return _OverrideConstraint;
			}
			set
			{
				if (_OverrideConstraint != value)
				{
					_OverrideConstraint = value;

					SetInternalConstraint(GetConstraint());
				}
			}
		}

		private ClassConstraintInfo _FieldConstraintInfo = null;
		private ClassConstraintInfo _OverrideTypeConstraintInfo = null;

		static ClassConstraintInfo CreateFieldConstraint(System.Reflection.FieldInfo fieldInfo)
		{
			ClassTypeConstraintAttribute constraint = AttributeHelper.GetAttribute<ClassTypeConstraintAttribute>(fieldInfo);
			if (constraint != null && TypeUtility.IsAssignableFrom(typeof(Component), constraint.GetBaseType(fieldInfo)))
			{
				return new ClassConstraintInfo() { constraintAttribute = constraint, constraintFieldInfo = fieldInfo };
			}

			SlotTypeAttribute slotTypeAttribute = AttributeHelper.GetAttribute<SlotTypeAttribute>(fieldInfo);
			if (slotTypeAttribute != null && TypeUtility.IsAssignableFrom(typeof(Component), slotTypeAttribute.connectableType))
			{
				return new ClassConstraintInfo() { slotTypeAttribute = slotTypeAttribute };
			}

			return null;
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
			return _OverrideConstraint ?? _OverrideTypeConstraintInfo ?? _FieldConstraintInfo;
		}

		System.Type connectableBaseType
		{
			get
			{
				ClassConstraintInfo constraintInfo = this.GetConstraint();
				if (constraintInfo != null)
				{
					System.Type connectableType = constraintInfo.GetConstraintBaseType();
					if (connectableType != null && TypeUtility.IsAssignableFrom(typeof(Component), connectableType))
					{
						return connectableType;
					}
				}

				return typeof(Component);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleComponentデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleComponent default constructor
		/// </summary>
#endif
		public FlexibleComponent()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleComponentコンストラクタ
		/// </summary>
		/// <param name="component">Component</param>
#else
		/// <summary>
		/// FlexibleComponent constructor
		/// </summary>
		/// <param name="component">Component</param>
#endif
		public FlexibleComponent(Component component)
		{
			_Type = FlexibleSceneObjectType.Constant;
			_Value = component;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleComponentコンストラクタ
		/// </summary>
		/// <param name="parameter">Parameter</param>
#else
		/// <summary>
		/// FlexibleComponent constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleComponent(ComponentParameterReference parameter)
		{
			_Type = FlexibleSceneObjectType.Parameter;
			_Parameter = parameter;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleComponentコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleComponent constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleComponent(InputSlotComponent slot)
		{
			_Type = FlexibleSceneObjectType.DataSlot;
			_Slot = slot;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleComponentコンストラクタ。
		/// </summary>
		/// <param name="hierarchyType">参照するオブジェクトのヒエラルキータイプ</param>
#else
		/// <summary>
		/// FlexibleComponent constructor.
		/// </summary>
		/// <param name="hierarchyType">Hierarchy type of referenced object</param>
#endif
		public FlexibleComponent(FlexibleHierarchyType hierarchyType)
		{
			_Type = FlexibleSceneObjectType.Hierarchy;
			_HierarchyType = hierarchyType;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleComponentをComponentにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleComponent</param>
		/// <returns>Componentにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleColor to Component.
		/// </summary>
		/// <param name="flexible">FlexibleComponent</param>
		/// <returns>Returns the result of casting to Component.</returns>
#endif
		public static explicit operator Component(FlexibleComponent flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ComponentをFlexibleComponentにキャスト。
		/// </summary>
		/// <param name="value">Component</param>
		/// <returns>FlexibleComponentにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast Component to FlexibleComponent.
		/// </summary>
		/// <param name="value">Component</param>
		/// <returns>Returns the result of casting to FlexibleComponent.</returns>
#endif
		public static explicit operator FlexibleComponent(Component value)
		{
			return new FlexibleComponent(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値をobjectで返す。
		/// </summary>
		/// <returns>値のobject</returns>
#else
		/// <summary>
		/// Return the value as object.
		/// </summary>
		/// <returns>The value object</returns>
#endif
		public override object GetValueObject()
		{
			return value;
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
		public override void Disconnect()
		{
			switch (_Type)
			{
				case FlexibleSceneObjectType.Parameter:
					_Parameter.Disconnect();
					break;
				case FlexibleSceneObjectType.DataSlot:
					_Slot.Disconnect();
					break;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleSceneObjectType.ConstantのObjectを返す。
		/// </summary>
		/// <returns>Constantの時のObject値</returns>
#else
		/// <summary>
		/// Returns an Object of FlexibleSceneObjectType.Constant.
		/// </summary>
		/// <returns>Object value at Constant</returns>
#endif
		public override Object GetConstantObject()
		{
			return _Value;
		}

		internal void SetConstant(Component value)
		{
			_Type = FlexibleSceneObjectType.Constant;
			_Value = value;
		}

		internal void SetSlot(InputSlotBase slot)
		{
			_Type = FlexibleSceneObjectType.DataSlot;
			_Slot.Copy(slot);
		}

		void SetInternalConstraint(ClassConstraintInfo constraint)
		{
			if (_Parameter != null)
			{
				_Parameter.overrideConstraint = constraint;
			}
			if (_Slot != null)
			{
				_Slot.overrideConstraint = constraint;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviour下のフィールドに割り当てられたときに呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when assigned to a field under NodeBehaviour.
		/// </summary>
#endif
		protected override void OnAssignedField()
		{
			_FieldConstraintInfo = CreateFieldConstraint(fieldInfo);
			SetInternalConstraint(GetConstraint());
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			_CachedComponent = null;
			System.Type overrideType = this.overrideConstraintType;
			if (overrideType != null)
			{
				_OverrideTypeConstraintInfo = new ClassConstraintInfo() { baseType = overrideType };
				SetInternalConstraint(GetConstraint());
			}
		}

		Component IValueGetter<Component>.GetValue()
		{
			return value;
		}
	}
}
