//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Text;
using System.Reflection;

namespace ArborEditor.Events
{
	using Arbor;
	using Arbor.Events;

	internal sealed class PersistentGetValueProperty
	{
		// Paths
		private const string kTargetTypePath = "_TargetType";
		private const string kTargetModePath = "_TargetMode";
		private const string kTargetComponentPath = "_TargetComponent";
		private const string kTargetGameObjectPath = "_TargetGameObject";
		private const string kTargetAssetObjectPath = "_TargetAssetObject";
		private const string kTargetSlotPath = "_TargetSlot";
		private const string kMemberTypePath = "_MemberType";
		private const string kMemberNamePath = "_MemberName";
		private const string kOutputValuePath = "_OutputValue";

		private ClassTypeReferenceProperty _TargetType;
		private SerializedProperty _TargetMode;
		private FlexibleComponentProperty _TargetComponent;
		private FlexibleSceneObjectProperty _TargetGameObject;
		private FlexibleFieldProperty _TargetAssetObject;
		private InputSlotBaseProperty _TargetSlot;
		private SerializedProperty _MemberType;
		private SerializedProperty _MemberName;
		private OutputSlotTypableProperty _OutputValue;

		private MemberInfo _MemberInfo;

		private string _MemberNameCache = null;

		public SerializedProperty property
		{
			get;
			private set;
		}

		public ClassTypeReferenceProperty targetTypeProperty
		{
			get
			{
				if (_TargetType == null)
				{
					_TargetType = new ClassTypeReferenceProperty(property.FindPropertyRelative(kTargetTypePath));
				}
				return _TargetType;
			}
		}

		public SerializedProperty targetModeProperty
		{
			get
			{
				if (_TargetMode == null)
				{
					_TargetMode = property.FindPropertyRelative(kTargetModePath);
				}
				return _TargetMode;
			}
		}

		public TargetMode targetMode
		{
			get
			{
				return EnumUtility.GetValueFromIndex<TargetMode>(targetModeProperty.enumValueIndex);
			}
			set
			{
				targetModeProperty.enumValueIndex = EnumUtility.GetIndexFromValue<TargetMode>(value);
			}
		}

		public FlexibleComponentProperty targetComponentProperty
		{
			get
			{
				if (_TargetComponent == null)
				{
					_TargetComponent = new FlexibleComponentProperty(property.FindPropertyRelative(kTargetComponentPath));
				}
				return _TargetComponent;
			}
		}

		public FlexibleSceneObjectProperty targetGameObjectProperty
		{
			get
			{
				if (_TargetGameObject == null)
				{
					_TargetGameObject = new FlexibleSceneObjectProperty(property.FindPropertyRelative(kTargetGameObjectPath));
				}
				return _TargetGameObject;
			}
		}

		public FlexibleFieldProperty targetAssetObjectProperty
		{
			get
			{
				if (_TargetAssetObject == null)
				{
					_TargetAssetObject = new FlexibleFieldProperty(property.FindPropertyRelative(kTargetAssetObjectPath));
				}
				return _TargetAssetObject;
			}
		}

		public InputSlotBaseProperty targetSlotProperty
		{
			get
			{
				if (_TargetSlot == null)
				{
					_TargetSlot = new InputSlotBaseProperty(property.FindPropertyRelative(kTargetSlotPath));
				}
				return _TargetSlot;
			}
		}

		public SerializedProperty memberTypeProperty
		{
			get
			{
				if (_MemberType == null)
				{
					_MemberType = property.FindPropertyRelative(kMemberTypePath);
				}
				return _MemberType;
			}
		}

		public SerializedProperty memberNameProperty
		{
			get
			{
				if (_MemberName == null)
				{
					_MemberName = property.FindPropertyRelative(kMemberNamePath);
				}
				return _MemberName;
			}
		}

		public OutputSlotTypableProperty outputValueProperty
		{
			get
			{
				if (_OutputValue == null)
				{
					_OutputValue = new OutputSlotTypableProperty(property.FindPropertyRelative(kOutputValuePath));
				}
				return _OutputValue;
			}
		}

		public MemberType memberType
		{
			get
			{
				return EnumUtility.GetValueFromIndex<MemberType>(memberTypeProperty.enumValueIndex);
			}
			set
			{
				memberTypeProperty.enumValueIndex = EnumUtility.GetIndexFromValue<MemberType>(value);
			}
		}

		public MemberInfo memberInfo
		{
			get
			{
				if (_MemberInfo == null)
				{
					_MemberInfo = GetMemberInfo();
				}
				return _MemberInfo;
			}
			set
			{
				SetMemberInfo(value);
			}
		}

		public PersistentGetValueProperty(SerializedProperty property)
		{
			this.property = property;
		}

		public MemberInfo GetMemberInfo()
		{
			System.Type type = targetTypeProperty.type;

			if (type == null)
			{
				return null;
			}

			switch (memberType)
			{
				case MemberType.Method:
					{
						return null;
					}
				case MemberType.Field:
					{
						return MemberCache.GetFieldInfo(type, memberNameProperty.stringValue);
					}
				case MemberType.Property:
					{
						return MemberCache.GetPropertyInfo(type, memberNameProperty.stringValue);
					}
			}

			return null;
		}

		private static StringBuilder s_MethodNameBuilder = new StringBuilder();

		string GenerateMemberName()
		{
			string methodName = this.memberNameProperty.stringValue;

			if (string.IsNullOrEmpty(methodName))
			{
				return "";
			}

			StringBuilder sb = s_MethodNameBuilder;
			sb.Length = 0;

			switch (memberType)
			{
				case MemberType.Method:
					{
					}
					break;
				case MemberType.Field:
				case MemberType.Property:
					{
						sb.Append(TypeUtility.GetTypeName(outputValueProperty.type));
						sb.Append(" ");
						sb.Append(methodName);
					}
					break;
			}

			return sb.ToString();
		}

		public string GetMemberName()
		{
			if (_MemberNameCache == null || Event.current.type == EventType.Layout)
			{
				_MemberNameCache = GenerateMemberName();
			}
			return _MemberNameCache;
		}

		public void Clear()
		{
			property.Clear(true);
		}

		public void ClearType()
		{
			targetComponentProperty.Disconnect();
			targetGameObjectProperty.Disconnect();
			targetAssetObjectProperty.Disconnect();

			targetSlotProperty.Disconnect();

			outputValueProperty.Disconnect();

			targetComponentProperty.Clear(false);
			targetGameObjectProperty.Clear(false);
			targetAssetObjectProperty.Clear(false);
			targetSlotProperty.Clear();
			ClearMember();
		}

		public void ClearMember()
		{
			outputValueProperty.Disconnect();

			_MemberInfo = null;
			memberType = MemberType.Field;
			memberNameProperty.stringValue = "";

			outputValueProperty.Clear();

			_MemberNameCache = null;
		}

		void SetFieldInfo(FieldInfo fieldInfo)
		{
			memberType = MemberType.Field;
			_MemberInfo = fieldInfo;

			memberNameProperty.stringValue = fieldInfo.Name;

			outputValueProperty.type = fieldInfo.FieldType;
		}

		void SetPropertyInfo(PropertyInfo propertyInfo)
		{
			memberType = MemberType.Property;
			_MemberInfo = propertyInfo;

			memberNameProperty.stringValue = propertyInfo.Name;

			outputValueProperty.type = propertyInfo.PropertyType;
		}

		void SetMemberInfo(MemberInfo memberInfo)
		{
			ClearMember();

			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				SetFieldInfo(fieldInfo);
				return;
			}

			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				SetPropertyInfo(propertyInfo);
				return;
			}
		}
	}
}