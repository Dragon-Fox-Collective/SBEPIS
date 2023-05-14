//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Arbor.Events
{
	using Arbor.DynamicReflection;
	using Arbor.Events.Legacy;
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// Reflectionによる永続的なメンバー呼び出しを行うクラス。
	/// Arbor内部処理用であるため、メンバー呼び出しを行いたい場合はArborEventクラスやInvokeMethodなどの組み込みビヘイビアを使用して下さい。
	/// </summary>
#else
	/// <summary>
	/// A class that makes persistent member calls by Reflection.
	/// Since it is for Arbor internal processing, if you want to call a member, use the built-in behavior such as ArborEvent class and InvokeMethod.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class PersistentCall : ISerializationCallbackReceiver, ISerializeVersionCallbackReceiver
	{
		#region Serialize fields

		[SerializeField]
		[FormerlySerializedAs("_ComponentType")]
		private ClassTypeReference _TargetType = new ClassTypeReference(typeof(Component));

		[SerializeField]
		private TargetMode _TargetMode = TargetMode.Component;

		[SerializeField]
		[FormerlySerializedAs("_Target")]
		private FlexibleComponent _TargetComponent = new FlexibleComponent();

		[SerializeField]
		private FlexibleGameObject _TargetGameObject = new FlexibleGameObject();

		[SerializeField]
		private FlexibleAssetObject _TargetAssetObject = new FlexibleAssetObject();

		[SerializeField]
		private InputSlotAny _TargetSlot = new InputSlotAny();

		[SerializeField]
		private MemberType _MemberType = MemberType.Method;

		[SerializeField]
		[FormerlySerializedAs("_MethodName")]
		private string _MemberName = "";

		[SerializeField]
		private List<Argument> _Arguments = new List<Argument>();

		[SerializeField]
		private ClassTypeReference _ReturnType = new ClassTypeReference();

		[SerializeField]
		private int _ReturnOutputSlotIndex = 0;

		[SerializeField]
		private ParameterList _ParameterList = new ParameterList();

		[SerializeField]
		[HideSlotFields]
		private List<OutputSlotTypable> _OutputSlots = new List<OutputSlotTypable>();

		[SerializeField]
		[HideSlotFields]
		private OutputSlotTypable _OutputSlotInstance = new OutputSlotTypable();

		[SerializeField]
		[HideInInspector]
		private SerializeVersion _SerializeVersion = new SerializeVersion();

		#region old

		[SerializeField]
		private List<IntParameter> _IntParameters = new List<IntParameter>();

		[SerializeField]
		private List<LongParameter> _LongParameters = new List<LongParameter>();

		[SerializeField]
		private List<FloatParameter> _FloatParameters = new List<FloatParameter>();

		[SerializeField]
		private List<BoolParameter> _BoolParameters = new List<BoolParameter>();

		[SerializeField]
		private List<StringParameter> _StringParameters = new List<StringParameter>();

		[SerializeField]
		private List<Vector2Parameter> _Vector2Parameters = new List<Vector2Parameter>();

		[SerializeField]
		private List<Vector3Parameter> _Vector3Parameters = new List<Vector3Parameter>();

		[SerializeField]
		private List<QuaternionParameter> _QuaternionParameters = new List<QuaternionParameter>();

		[SerializeField]
		private List<RectParameter> _RectParameters = new List<RectParameter>();

		[SerializeField]
		private List<BoundsParameter> _BoundsParameters = new List<BoundsParameter>();

		[SerializeField]
		private List<ColorParameter> _ColorParameters = new List<ColorParameter>();

		[SerializeField]
		private List<GameObjectParameter> _GameObjectParameters = new List<GameObjectParameter>();

		[SerializeField]
		private List<ComponentParameter> _ComponentParameters = new List<ComponentParameter>();

		[SerializeField]
		private List<EnumParameter> _EnumParameters = new List<EnumParameter>();

		[SerializeField]
		private List<InputSlotParameter> _InputSlotParameters = new List<InputSlotParameter>();

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

#if ARBOR_DOC_JA
		/// <summary>
		/// ターゲットの型
		/// </summary>
#else
		/// <summary>
		/// Target type
		/// </summary>
#endif
		public System.Type targetType
		{
			get
			{
				return _TargetType.type ?? typeof(Component);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ターゲットのインスタンス
		/// </summary>
#else
		/// <summary>
		/// Target instance
		/// </summary>
#endif
		public object targetInstance
		{
			get
			{
				object value = null;
				switch (_TargetMode)
				{
					case TargetMode.Component:
						value = _TargetComponent.value;
						break;
					case TargetMode.GameObject:
						value = _TargetGameObject.value;
						break;
					case TargetMode.AssetObject:
						value = _TargetAssetObject.value;
						break;
					case TargetMode.Slot:
						_TargetSlot.GetValue(ref value);
						value = DynamicUtility.Rebox(value);
						break;
					case TargetMode.Static:
						value = null;
						break;
				}

				return value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// コンポーネントの型
		/// </summary>
#else
		/// <summary>
		/// Component type
		/// </summary>
#endif
		[System.Obsolete("use targetType")]
		public System.Type componentType
		{
			get
			{
				return targetType;
			}
		}

		private MemberInfo memberInfo
		{
			get;
			set;
		}

		private DynamicMethod dynamicMethod
		{
			get;
			set;
		}

		private DynamicField dynamicField
		{
			get;
			set;
		}

		private object[] _Parameters = null;

#if ARBOR_DOC_JA
		/// <summary>
		/// CalculatorConditionLegacyコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// CalculatorConditionLegacy constructor
		/// </summary>
#endif
		public PersistentCall()
		{
			// Initialize when calling from script.
			_SerializeVersion.Initialize(this);
		}

		object GetArgumentValue(Argument argument)
		{
			System.Type argumentType = argument.type;
			if (argumentType.IsByRef)
			{
				argumentType = argumentType.GetElementType();
			}
			IValueGetter valueContainer = argument.valueContainer;

			if (valueContainer != null)
			{
				object value = valueContainer.GetValueObject();
				return DynamicUtility.Cast(value, argumentType);
			}

			return DynamicUtility.GetDefault(argumentType);
		}

		internal static bool ValidTarget(object target, TargetMode targetMode, System.Type targetType)
		{
			if (targetMode == TargetMode.Static)
			{
				return true;
			}

			if (TypeUtility.IsValueType(targetType))
			{
				return true;
			}

			if (TypeUtility.IsAssignableFrom(typeof(Object), targetType))
			{
				return (target as Object) != null;
			}

			return target != null;
		}

		bool ValidTarget(object target)
		{
			return ValidTarget(target, _TargetMode, targetType);
		}

		void InvokeMethod()
		{
			if (dynamicMethod == null)
			{
				//Debug.LogWarning("dynamicMethod == null");
				return;
			}

			object target = targetInstance;
			if (!ValidTarget(target))
			{
				return;
			}

			try
			{
				System.Type returnType = _ReturnType.type;

				MethodInfo methodInfo = dynamicMethod.methodInfo;

				System.Type methodReturnType = methodInfo.ReturnType;
				if (methodReturnType != null && methodReturnType == typeof(void))
				{
					methodReturnType = null;
				}

				ParameterInfo[] parameters = methodInfo.GetParameters();

				int argumentCount = _Arguments.Count;
				for (int i = 0; i < argumentCount; i++)
				{
					ParameterInfo parametrInfo = parameters[i];

					if (parametrInfo.IsOut)
					{
						_Parameters[i] = null;
					}
					else
					{
						Argument argument = _Arguments[i];
						_Parameters[i] = GetArgumentValue(argument);
					}
				}

				object result = dynamicMethod.Invoke(target, _Parameters);

				if (returnType != null && returnType != typeof(void) && returnType == methodReturnType)
				{
					OutputSlotTypable outputSlot = _OutputSlots[_ReturnOutputSlotIndex];
					outputSlot.SetValue(result);
				}

				for (int i = 0; i < argumentCount; i++)
				{
					Argument argument = _Arguments[i];
					OutputSlotTypable outputSlot = argument.outputSlot;
					if (outputSlot != null)
					{
						outputSlot.SetValue(_Parameters[i]);
					}
				}

				System.Type targetType = this.targetType;

				if (targetType != null && TypeUtility.IsValueType(targetType))
				{
					_OutputSlotInstance.SetValue(target);
				}
			}
			catch (TargetInvocationException ex)
			{
				Debug.LogException(ex, target as Object);
			}
			catch (System.Exception ex)
			{
				Debug.LogErrorFormat("{0} : {1} ({2}.{3})", ex.GetType(), ex.Message, targetType, _MemberName);
			}
		}

		void InvokeField()
		{
			if (dynamicField == null)
			{
				//Debug.LogWarning("dynamicField == null");
				return;
			}

			object target = targetInstance;
			if (!ValidTarget(target))
			{
				return;
			}

			Argument argument = _Arguments[0];
			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo.FieldType != argument.type)
			{
				return;
			}

			try
			{
				object value = GetArgumentValue(argument);

				dynamicField.SetValue(target, value);

				System.Type targetType = this.targetType;

				if (targetType != null && TypeUtility.IsValueType(targetType))
				{
					_OutputSlotInstance.SetValue(target);
				}
			}
			catch (TargetInvocationException ex)
			{
				Debug.LogException(ex, target as Object);
			}
			catch (System.Exception ex)
			{
				Debug.LogErrorFormat("{0} : {1} ({2}.{3})", ex.GetType(), ex.Message, targetType, _MemberName);
			}
		}

		void InvokeProperty()
		{
			Argument argument = _Arguments[0];
			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo.PropertyType != argument.type)
			{
				return;
			}

			InvokeMethod();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// メンバーを呼び出す。
		/// </summary>
#else
		/// <summary>
		/// Invoke member.
		/// </summary>
#endif
		public void Invoke()
		{
			switch (_MemberType)
			{
				case MemberType.Method:
					InvokeMethod();
					break;
				case MemberType.Field:
					InvokeField();
					break;
				case MemberType.Property:
					InvokeProperty();
					break;
			}
		}

		static System.Text.StringBuilder s_WarningBulder = new System.Text.StringBuilder();

#if ARBOR_DOC_JA
		/// <summary>
		/// 警告メッセージを取得する。
		/// </summary>
		/// <returns>警告メッセージを返す。</returns>
#else
		/// <summary>
		/// Get the warning message.
		/// </summary>
		/// <returns>Returns the warning message.</returns>
#endif
		public string GetWarningMessage()
		{
			s_WarningBulder.Length = 0;

			if (memberInfo != null)
			{
				System.ObsoleteAttribute obsoleteAttribute = AttributeHelper.GetAttribute<System.ObsoleteAttribute>(memberInfo);
				if (obsoleteAttribute != null)
				{
					if (s_WarningBulder.Length > 0)
					{
						s_WarningBulder.AppendLine();
					}
					s_WarningBulder.AppendFormat("* Obsolete {0} : {1}", obsoleteAttribute.IsError ? "Error" : "Warning", obsoleteAttribute.Message);
				}
			}
			else
			{
				if (!string.IsNullOrEmpty(_MemberName))
				{
					s_WarningBulder.Append("* Missing");
				}
			}

			switch (_MemberType)
			{
				case MemberType.Method:
					{
						MethodInfo methodInfo = memberInfo as MethodInfo;
						if (methodInfo != null)
						{
							System.Type returnType = _ReturnType.type;

							ParameterInfo[] parameters = methodInfo.GetParameters();

							for (int i = 0; i < parameters.Length; i++)
							{
								Argument argument = _Arguments[i];

								ParameterInfo parameterInfo = parameters[i];

								if (parameterInfo.ParameterType.IsByRef)
								{
									if (argument.isOut && !parameterInfo.IsOut)
									{
										// out -> ref
										if (s_WarningBulder.Length > 0)
										{
											s_WarningBulder.AppendLine();
										}
										s_WarningBulder.AppendFormat("* The argument \"{0}\" has been changed to \"ref\". Use the default value.", argument.name);
									}
									else if (!argument.isOut && parameterInfo.IsOut)
									{
										// ref -> out
										if (s_WarningBulder.Length > 0)
										{
											s_WarningBulder.AppendLine();
										}
										s_WarningBulder.AppendFormat("* The argument \"{0}\" has been changed to \"out\". This input parameter is not used.", argument.name);
									}
								}
							}

							System.Type methodReturnType = methodInfo.ReturnType;
							if (methodReturnType != null && methodReturnType == typeof(void))
							{
								methodReturnType = null;
							}

							if (returnType != methodReturnType)
							{
								s_WarningBulder.AppendFormat("* The return type has been changed to \"{0}\". It does not output return value.", TypeUtility.GetTypeName(methodReturnType));
							}
						}
					}
					break;
				case MemberType.Field:
					{
						FieldInfo fieldInfo = memberInfo as FieldInfo;
						if (fieldInfo != null)
						{
							System.Type valueType = fieldInfo.FieldType;

							Argument argument = _Arguments[0];
							if (argument.type != valueType)
							{
								s_WarningBulder.AppendFormat("* The value type has been changed to \"{0}\". It does not set value.", TypeUtility.GetTypeName(valueType));
							}
						}
					}
					break;
				case MemberType.Property:
					{
						PropertyInfo propertyInfo = memberInfo as PropertyInfo;
						if (propertyInfo != null)
						{
							System.Type valueType = propertyInfo.PropertyType;

							Argument argument = _Arguments[0];
							if (argument.type != valueType)
							{
								s_WarningBulder.AppendFormat("* The value type has been changed to \"{0}\". It does not set value.", TypeUtility.GetTypeName(valueType));
							}
						}
					}
					break;
			}

			if (s_WarningBulder.Length > 0)
			{
				return string.Format("{0}.{1} : \n{2}", targetType, _MemberName, s_WarningBulder.ToString());
			}

			return null;
		}

		System.Type[] SetupArguments()
		{
			int argumentCount = _Arguments.Count;
			System.Type[] argumentTypes = new System.Type[argumentCount];
			for (int i = 0; i < argumentCount; i++)
			{
				Argument argument = _Arguments[i];

				System.Type argumentType = argument.type;
				argumentTypes[i] = argumentType;

				argument.valueContainer = null;

				if (!argument.isOut)
				{
					IList parameterList = _ParameterList.GetParameterList(argument.parameterType);
					if (parameterList != null)
					{
						argument.valueContainer = parameterList[argument.parameterIndex] as IValueGetter;
					}

					if (argumentType.IsByRef)
					{
						argument.outputSlot = _OutputSlots[argument.outputSlotIndex];
					}
					else
					{
						argument.outputSlot = null;
					}
				}
				else
				{
					argument.outputSlot = _OutputSlots[argument.outputSlotIndex];
				}
			}

			return argumentTypes;
		}

		void SetupMethod()
		{
			System.Type type = targetType;

			System.Type[] argumentTypes = SetupArguments();

			MethodInfo methodInfo = MemberCache.GetMethodInfo(type, _MemberName, argumentTypes);
			if (methodInfo == null && !string.IsNullOrEmpty(_MemberName))
			{
				MethodInfo renamedMethodInfo = MemberCache.GetMethodInfoRenamedFrom(type, _MemberName, argumentTypes);
				if (renamedMethodInfo != null)
				{
					_MemberName = renamedMethodInfo.Name;
					methodInfo = renamedMethodInfo;
				}
			}

			if (methodInfo != null)
			{
				memberInfo = methodInfo;
				_Parameters = new object[_Arguments.Count];
				dynamicMethod = DynamicMethod.GetMethod(methodInfo);

#if UNITY_EDITOR || ARBOR_DLL
				ParameterInfo[] parameters = methodInfo.GetParameters();
				for (int i = 0; i < parameters.Length; i++)
				{
					_Arguments[i].name = parameters[i].Name;
				}
#endif
			}
			else
			{
				memberInfo = null;
				_Parameters = null;
				dynamicMethod = null;
			}
		}

		void SetupField()
		{
			System.Type type = targetType;

			SetupArguments();

			System.Type fieldType = _Arguments[0].type;

			FieldInfo fieldInfo = MemberCache.GetFieldInfo(type, _MemberName);
			if ((fieldInfo == null || fieldInfo.FieldType != fieldType) && !string.IsNullOrEmpty(_MemberName))
			{
				FieldInfo renamedFieldInfo = MemberCache.GetFieldInfoRenamedFrom(type, _MemberName, fieldType);
				if (renamedFieldInfo != null)
				{
					_MemberName = renamedFieldInfo.Name;
					fieldInfo = renamedFieldInfo;
				}
			}

			if (fieldInfo != null)
			{
				memberInfo = fieldInfo;
				dynamicField = DynamicField.GetField(fieldInfo);
			}
			else
			{
				memberInfo = null;
				dynamicField = null;
			}
		}

		void SetupProperty()
		{
			System.Type type = targetType;

			SetupArguments();

			System.Type propertyType = _Arguments[0].type;

			PropertyInfo propertyInfo = MemberCache.GetPropertyInfo(type, _MemberName);
			if ((propertyInfo == null || propertyInfo.PropertyType != propertyType) && !string.IsNullOrEmpty(_MemberName))
			{
				PropertyInfo renamedPropertyInfo = MemberCache.GetPropertyInfoRenamedFrom(type, _MemberName, propertyType);
				if (renamedPropertyInfo != null)
				{
					_MemberName = renamedPropertyInfo.Name;
					propertyInfo = renamedPropertyInfo;
				}
			}

			if (propertyInfo == null)
			{
				memberInfo = null;
				_Parameters = null;
				dynamicMethod = null;
				return;
			}

			memberInfo = propertyInfo;

			MethodInfo methodInfo = propertyInfo.GetSetMethod();
			if (methodInfo != null)
			{
				_Parameters = new object[_Arguments.Count];
				dynamicMethod = DynamicMethod.GetMethod(methodInfo);
			}
			else
			{
				_Parameters = null;
				dynamicMethod = null;
			}
		}

		void SerializeVer1()
		{
			if (_ParameterList == null)
			{
				_ParameterList = new ParameterList();
			}

			if (_IntParameters != null)
			{
				if (_ParameterList._IntParameters == null)
				{
					_ParameterList._IntParameters = new List<FlexibleInt>();
				}
				for (int i = 0; i < _IntParameters.Count; i++)
				{
					_ParameterList._IntParameters.Add(_IntParameters[i].GetFlexibleField());
				}
				_IntParameters = null;
			}

			if (_LongParameters != null)
			{
				if (_ParameterList._LongParameters == null)
				{
					_ParameterList._LongParameters = new List<FlexibleLong>();
				}
				for (int i = 0; i < _LongParameters.Count; i++)
				{
					_ParameterList._LongParameters.Add(_LongParameters[i].GetFlexibleField());
				}
				_LongParameters = null;
			}

			if (_FloatParameters != null)
			{
				if (_ParameterList._FloatParameters == null)
				{
					_ParameterList._FloatParameters = new List<FlexibleFloat>();
				}
				for (int i = 0; i < _FloatParameters.Count; i++)
				{
					_ParameterList._FloatParameters.Add(_FloatParameters[i].GetFlexibleField());
				}
				_FloatParameters = null;
			}

			if (_BoolParameters != null)
			{
				if (_ParameterList._BoolParameters == null)
				{
					_ParameterList._BoolParameters = new List<FlexibleBool>();
				}
				for (int i = 0; i < _BoolParameters.Count; i++)
				{
					_ParameterList._BoolParameters.Add(_BoolParameters[i].GetFlexibleField());
				}
				_BoolParameters = null;
			}

			if (_StringParameters != null)
			{
				if (_ParameterList._StringParameters == null)
				{
					_ParameterList._StringParameters = new List<FlexibleString>();
				}
				for (int i = 0; i < _StringParameters.Count; i++)
				{
					_ParameterList._StringParameters.Add(_StringParameters[i].GetFlexibleField());
				}
				_StringParameters = null;
			}

			if (_Vector2Parameters != null)
			{
				if (_ParameterList._Vector2Parameters == null)
				{
					_ParameterList._Vector2Parameters = new List<FlexibleVector2>();
				}
				for (int i = 0; i < _Vector2Parameters.Count; i++)
				{
					_ParameterList._Vector2Parameters.Add(_Vector2Parameters[i].GetFlexibleField());
				}
				_Vector2Parameters = null;
			}

			if (_Vector3Parameters != null)
			{
				if (_ParameterList._Vector3Parameters == null)
				{
					_ParameterList._Vector3Parameters = new List<FlexibleVector3>();
				}
				for (int i = 0; i < _Vector3Parameters.Count; i++)
				{
					_ParameterList._Vector3Parameters.Add(_Vector3Parameters[i].GetFlexibleField());
				}
				_Vector3Parameters = null;
			}

			if (_QuaternionParameters != null)
			{
				if (_ParameterList._QuaternionParameters == null)
				{
					_ParameterList._QuaternionParameters = new List<FlexibleQuaternion>();
				}
				for (int i = 0; i < _QuaternionParameters.Count; i++)
				{
					_ParameterList._QuaternionParameters.Add(_QuaternionParameters[i].GetFlexibleField());
				}
				_QuaternionParameters = null;
			}

			if (_RectParameters != null)
			{
				if (_ParameterList._RectParameters == null)
				{
					_ParameterList._RectParameters = new List<FlexibleRect>();
				}
				for (int i = 0; i < _RectParameters.Count; i++)
				{
					_ParameterList._RectParameters.Add(_RectParameters[i].GetFlexibleField());
				}
				_RectParameters = null;
			}

			if (_BoundsParameters != null)
			{
				if (_ParameterList._BoundsParameters == null)
				{
					_ParameterList._BoundsParameters = new List<FlexibleBounds>();
				}
				for (int i = 0; i < _BoundsParameters.Count; i++)
				{
					_ParameterList._BoundsParameters.Add(_BoundsParameters[i].GetFlexibleField());
				}
				_BoundsParameters = null;
			}

			if (_ColorParameters != null)
			{
				if (_ParameterList._ColorParameters == null)
				{
					_ParameterList._ColorParameters = new List<FlexibleColor>();
				}
				for (int i = 0; i < _ColorParameters.Count; i++)
				{
					_ParameterList._ColorParameters.Add(_ColorParameters[i].GetFlexibleField());
				}
				_ColorParameters = null;
			}

			if (_GameObjectParameters != null)
			{
				if (_ParameterList._GameObjectParameters == null)
				{
					_ParameterList._GameObjectParameters = new List<FlexibleGameObject>();
				}
				for (int i = 0; i < _GameObjectParameters.Count; i++)
				{
					_ParameterList._GameObjectParameters.Add(_GameObjectParameters[i].GetFlexibleField());
				}
				_GameObjectParameters = null;
			}

			if (_ComponentParameters != null)
			{
				if (_ParameterList._ComponentParameters == null)
				{
					_ParameterList._ComponentParameters = new List<FlexibleComponent>();
				}
				for (int i = 0; i < _ComponentParameters.Count; i++)
				{
					_ParameterList._ComponentParameters.Add(_ComponentParameters[i].GetFlexibleField());
				}
				_ComponentParameters = null;
			}

			if (_EnumParameters != null)
			{
				if (_ParameterList._EnumParameters == null)
				{
					_ParameterList._EnumParameters = new List<FlexibleEnumAny>();
				}
				for (int i = 0; i < _EnumParameters.Count; i++)
				{
					_ParameterList._EnumParameters.Add(_EnumParameters[i].GetFlexibleField());
				}
				_EnumParameters = null;
			}

			if (_InputSlotParameters != null)
			{
				if (_ParameterList._InputSlotParameters == null)
				{
					_ParameterList._InputSlotParameters = new List<InputSlotTypable>();
				}
				for (int i = 0; i < _InputSlotParameters.Count; i++)
				{
					_ParameterList._InputSlotParameters.Add(_InputSlotParameters[i].GetSlot());
				}
				_InputSlotParameters = null;
			}
		}

		void Serialize()
		{
			while (_SerializeVersion.version != kCurrentSerializeVersion)
			{
				switch (_SerializeVersion.version)
				{
					case 0:
						SerializeVer1();
						_SerializeVersion.version++;
						break;
					default:
						_SerializeVersion.version = kCurrentSerializeVersion;
						break;
				}
			}
		}

		#region ISerializeVersionCallbackReceiver

		int ISerializeVersionCallbackReceiver.newestVersion
		{
			get
			{
				return kCurrentSerializeVersion;
			}
		}

		void ISerializeVersionCallbackReceiver.OnInitialize()
		{
			_SerializeVersion.version = kCurrentSerializeVersion;
		}

		void ISerializeVersionCallbackReceiver.OnVersioning()
		{
		}

		void ISerializeVersionCallbackReceiver.OnSerialize(int version)
		{
			switch (version)
			{
				case 0:
					SerializeVer1();
					break;
			}
		}

		#endregion // ISerializeVersionCallbackReceiver

		#region ISerializationCallbackReceiver

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			_SerializeVersion.AfterDeserialize();

			System.Type type = targetType;

			IOverrideConstraint targetOverrideConstraint = null;
			switch (_TargetMode)
			{
				case TargetMode.Component:
					{
						targetOverrideConstraint = _TargetComponent;
					}
					break;
				case TargetMode.AssetObject:
					{
						targetOverrideConstraint = _TargetAssetObject;
					}
					break;
				case TargetMode.Slot:
					{
						targetOverrideConstraint = _TargetSlot;
					}
					break;
			}

			if (targetOverrideConstraint != null)
			{
				targetOverrideConstraint.overrideConstraint = new ClassConstraintInfo() { baseType = type };
			}

			for (int argumentIndex = 0; argumentIndex < _Arguments.Count; argumentIndex++)
			{
				Argument argument = _Arguments[argumentIndex];
				System.Type argumentType = argument.type;
				ArgumentAttributes attributes = argument.attributes;
				bool isOut = (attributes & ArgumentAttributes.Out) == ArgumentAttributes.Out;
				if (!isOut)
				{
					ParameterType parameterType = argument.parameterType;
					IList parameters = _ParameterList.GetParameterList(parameterType);
					if (parameters != null)
					{
						int parameterIndex = argument.parameterIndex;
						if (parameterIndex < 0 || parameters.Count <= parameterIndex)
						{
							continue;
						}

						object value = parameters[parameterIndex];

						bool isByRef = argumentType.IsByRef;
						System.Type overrideType = isByRef ? argumentType.GetElementType() : argumentType;

						switch (parameterType)
						{
							case ParameterType.Component:
							case ParameterType.Enum:
							case ParameterType.AssetObject:
								{
									IOverrideConstraint flexibleField = value as IOverrideConstraint;
									if (flexibleField != null)
									{
										flexibleField.overrideConstraint = new ClassConstraintInfo() { baseType = overrideType };
									}
								}
								break;
						}
					}
				}
			}

			if (type == null)
			{
				_Parameters = null;
				dynamicMethod = null;
				dynamicField = null;
				return;
			}

			switch (_MemberType)
			{
				case MemberType.Method:
					SetupMethod();
					break;
				case MemberType.Field:
					SetupField();
					break;
				case MemberType.Property:
					SetupProperty();
					break;
			}
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			_SerializeVersion.BeforeDeserialize();
		}

		#endregion // ISerializationCallbackReceiver
	}
}