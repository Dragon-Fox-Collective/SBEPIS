//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Reflection;

namespace Arbor.Events
{
	using Arbor.DynamicReflection;

#if ARBOR_DOC_JA
	/// <summary>
	/// Reflectionによる永続的なメンバーの値取得を行うクラス。
	/// Arbor内部処理用であるため、メンバーの値取得を行いたい場合はGetValueCalculatorなどの組み込みビヘイビアを使用して下さい。
	/// </summary>
#else
	/// <summary>
	/// A class to get the value of a persistent member by Reflection.
	/// Since it is for Arbor internal processing, if you want to get the member value, use the built-in behavior such as GetValueCalculator.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class PersistentGetValue : ISerializationCallbackReceiver
	{
		[SerializeField]
		private ClassTypeReference _TargetType = new ClassTypeReference(typeof(Component));

		[SerializeField]
		private TargetMode _TargetMode = TargetMode.Component;

		[SerializeField]
		private FlexibleComponent _TargetComponent = new FlexibleComponent();

		[SerializeField]
		private FlexibleGameObject _TargetGameObject = new FlexibleGameObject();

		[SerializeField]
		private FlexibleAssetObject _TargetAssetObject = new FlexibleAssetObject();

		[SerializeField]
		private InputSlotAny _TargetSlot = new InputSlotAny();

		[SerializeField]
		private MemberType _MemberType = MemberType.Field;

		[SerializeField]
		private string _MemberName = "";

		[SerializeField]
		[HideSlotFields]
		private OutputSlotTypable _OutputValue = new OutputSlotTypable();

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

		bool ValidTarget(object target)
		{
			return PersistentCall.ValidTarget(target, _TargetMode, targetType);
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
				MethodInfo methodInfo = dynamicMethod.methodInfo;

				System.Type methodReturnType = methodInfo.ReturnType;
				if (methodReturnType != null && methodReturnType == typeof(void))
				{
					methodReturnType = null;
				}

				object result = dynamicMethod.Invoke(target, null);

				_OutputValue.SetValue(result);
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

			try
			{
				object result = dynamicField.GetValue(target);

				_OutputValue.SetValue(result);
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

		void SetupField()
		{
			System.Type type = targetType;

			System.Type fieldType = _OutputValue.dataType;

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

			System.Type propertyType = _OutputValue.dataType;

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
				dynamicMethod = null;
				dynamicField = null;
				return;
			}

			memberInfo = propertyInfo;

			MethodInfo methodInfo = propertyInfo.GetGetMethod();
			if (methodInfo != null)
			{
				dynamicMethod = DynamicMethod.GetMethod(methodInfo);
			}
			else
			{
				dynamicMethod = null;
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
					}
					break;
				case MemberType.Field:
					{
						FieldInfo fieldInfo = memberInfo as FieldInfo;
						if (fieldInfo != null)
						{
							System.Type valueType = fieldInfo.FieldType;

							if (_OutputValue.dataType != valueType)
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

							if (_OutputValue.dataType != valueType)
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

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
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

			if (type == null)
			{
				dynamicMethod = null;
				return;
			}

			switch (_MemberType)
			{
				case MemberType.Method:
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
		}
	}
}