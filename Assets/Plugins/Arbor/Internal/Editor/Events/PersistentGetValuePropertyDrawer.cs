//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace ArborEditor.Events
{
	using Arbor;
	using Arbor.Events;

	internal sealed class PersistentGetValueEditor : PropertyEditor
	{
		private const float kSpacing = 5;
		
		private PersistentGetValueProperty _GetValueProperty;

		private LayoutArea _LayoutArea = new LayoutArea();

		protected override void OnInitialize()
		{
			base.OnInitialize();

			_GetValueProperty = new PersistentGetValueProperty(property);
		}

		void DoGUI()
		{
			System.Type targetType = _GetValueProperty.targetTypeProperty.type;
			string targetTypeName = _GetValueProperty.targetTypeProperty.assemblyTypeName.stringValue;
			bool isTargetTypeMissing = targetType == null && !string.IsNullOrEmpty(targetTypeName);

			_LayoutArea.BeginHorizontal();

			EditorGUI.BeginChangeCheck();
			_LayoutArea.PropertyField(_GetValueProperty.targetTypeProperty.property, GUIContent.none, false, LayoutArea.Width(EditorGUIUtility.labelWidth - kSpacing));
			if (EditorGUI.EndChangeCheck())
			{
				System.Type newTargetType = _GetValueProperty.targetTypeProperty.type;
				if (targetType != newTargetType || isTargetTypeMissing)
				{
					targetType = newTargetType;

					_GetValueProperty.ClearType();

					_GetValueProperty.targetMode = PersistentCallProperty.GetTargetMode(targetType);

					_GetValueProperty.property.serializedObject.ApplyModifiedProperties();

					GUIUtility.ExitGUI();
				}
			}

			_LayoutArea.Space(kSpacing);

			EditorGUI.BeginDisabledGroup(targetType == null);

			MemberInfo memberInfo = _GetValueProperty.memberInfo;

			Rect methodNameRect = _LayoutArea.GetRect(0f, EditorGUIUtility.singleLineHeight);

			if (_LayoutArea.IsDraw())
			{
				string memberName = _GetValueProperty.memberNameProperty.stringValue;
				bool isEmpty = string.IsNullOrEmpty(memberName);
				bool isMissing = memberInfo == null && !isEmpty;

				string displayMemberName = _GetValueProperty.GetMemberName();

				if (isMissing)
				{
					displayMemberName = string.Format("<Missing {0} >", displayMemberName);
				}
				else if (string.IsNullOrEmpty(displayMemberName))
				{
					displayMemberName = MemberSelector.kNoFunctionText;
				}

				MemberFilterFlags memberFilterFlags = (MemberFilterFlags)(-1) & ~(MemberFilterFlags.SetProperty | MemberFilterFlags.Method); // ignore Method, SetProperty

				EditorGUI.BeginChangeCheck();
				MemberInfo newMemberInfo = MemberSelector.PopupField(methodNameRect, memberInfo, targetType, true, memberFilterFlags, GUIContentCaches.Get(displayMemberName));
				if (EditorGUI.EndChangeCheck())
				{
					if (newMemberInfo != memberInfo || isMissing)
					{
						System.Type instanceType = ArborEventUtility.IsStatic(newMemberInfo) ? null : targetType;
						TargetMode newTargetMode = PersistentCallProperty.GetTargetMode(instanceType);
						if (_GetValueProperty.targetMode != newTargetMode)
						{
							_GetValueProperty.ClearType();
							_GetValueProperty.targetMode = newTargetMode;
						}

						_GetValueProperty.memberInfo = newMemberInfo;

						_GetValueProperty.property.serializedObject.ApplyModifiedProperties();

						GUIUtility.ExitGUI();
					}
				}
			}

			EditorGUI.EndDisabledGroup();

			_LayoutArea.EndHorizontal();

			System.ObsoleteAttribute obsoleteAttribute = AttributeHelper.GetAttribute<System.ObsoleteAttribute>(memberInfo);
			if (obsoleteAttribute != null)
			{
				string message = string.Format("Obsolete : {0}", obsoleteAttribute.Message);
				MessageType messageType = obsoleteAttribute.IsError ? MessageType.Error : MessageType.Warning;

				_LayoutArea.HelpBox(message, messageType);
			}

			GUIContent targetContent = GUIContentCaches.Get("<Target>");

			switch (_GetValueProperty.targetMode)
			{
				case TargetMode.Component:
					{
						FlexibleComponentProperty targetComponentProperty = _GetValueProperty.targetComponentProperty;

						_LayoutArea.PropertyField(targetComponentProperty.property, targetContent, true);
					}
					break;
				case TargetMode.GameObject:
					{
						FlexibleSceneObjectProperty targetGameObjectProperty = _GetValueProperty.targetGameObjectProperty;

						_LayoutArea.PropertyField(targetGameObjectProperty.property, targetContent, true);
					}
					break;
				case TargetMode.AssetObject:
					{
						FlexibleFieldProperty targetAssetObjectProperty = _GetValueProperty.targetAssetObjectProperty;

						_LayoutArea.PropertyField(targetAssetObjectProperty.property, targetContent, true);
					}
					break;
				case TargetMode.Slot:
					{
						InputSlotBaseProperty targetSlotProperty = _GetValueProperty.targetSlotProperty;

						_LayoutArea.PropertyField(targetSlotProperty.property, targetContent, true);
					}
					break;
				case TargetMode.Static:
					{
						GUIContent staticContent = GUIContentCaches.Get("Static");
						_LayoutArea.LabelField(targetContent, staticContent);
					}
					break;
			}

			if (memberInfo != null)
			{
				_LayoutArea.PropertyField(_GetValueProperty.outputValueProperty.property);
			}

			if (memberInfo != null)
			{
				List<InvalidRepair> invalidRepairs = new List<InvalidRepair>();

				switch (_GetValueProperty.memberType)
				{
					case MemberType.Method:
						{
						}
						break;
					case MemberType.Field:
						{
							FieldInfo fieldInfo = memberInfo as FieldInfo;
							if (fieldInfo == null)
							{
								break;
							}

							System.Type fieldType = fieldInfo.FieldType;

							if (_GetValueProperty.outputValueProperty.type != fieldType)
							{
								invalidRepairs.Add(new OutputValueTypeRepair(_GetValueProperty, fieldType));
							}
						}
						break;
					case MemberType.Property:
						{
							PropertyInfo propertyInfo = memberInfo as PropertyInfo;
							if (propertyInfo == null)
							{
								break;
							}

							System.Type propertyType = propertyInfo.PropertyType;

							if (_GetValueProperty.outputValueProperty.type != propertyType)
							{
								invalidRepairs.Add(new OutputValueTypeRepair(_GetValueProperty, propertyType));
							}
						}
						break;
				}

				if (invalidRepairs.Count > 0)
				{
					StringBuilder invalidMessageBuilder = new StringBuilder();

					for (int repairIndex = 0; repairIndex < invalidRepairs.Count; repairIndex++)
					{
						InvalidRepair repair = invalidRepairs[repairIndex];
						if (invalidMessageBuilder.Length > 0)
						{
							invalidMessageBuilder.AppendLine();
						}
						invalidMessageBuilder.Append(repair.GetMessage());
					}

					string invalidMessage = invalidMessageBuilder.ToString();

					_LayoutArea.HelpBox(invalidMessage, MessageType.Error);

					if (_LayoutArea.Button(EditorContents.repair))
					{
						for (int repairIndex = 0; repairIndex < invalidRepairs.Count; repairIndex++)
						{
							InvalidRepair repair = invalidRepairs[repairIndex];
							repair.OnRepair();
						}
					}
				}
			}
		}

		static readonly RectOffset s_LayoutMargin = new RectOffset(0, 0, 0, 2);

		protected override void OnGUI(Rect position, GUIContent label)
		{
			bool isLabel = (label != null && label != GUIContent.none);

			int indentLevel = EditorGUI.indentLevel;
			if (isLabel)
			{
				EditorGUI.LabelField(position, label);

				EditorGUI.indentLevel++;

				position.yMin += EditorGUIUtility.singleLineHeight;
			}

			_LayoutArea.Begin(position, false, s_LayoutMargin);

			DoGUI();

			_LayoutArea.End();

			if (isLabel)
			{
				EditorGUI.indentLevel = indentLevel;
			}
		}

		protected override float GetHeight(GUIContent label)
		{
			bool isLabel = (label != null && label != GUIContent.none);

			float height = 0f;
			if (isLabel)
			{
				height += EditorGUIUtility.singleLineHeight;
			}

			_LayoutArea.Begin(new Rect(), true, s_LayoutMargin);

			DoGUI();

			_LayoutArea.End();

			height += _LayoutArea.rect.height;

			return height;
		}
	}

	[CustomPropertyDrawer(typeof(PersistentGetValue))]
	internal sealed class PersistentGetValuePropertyDrawer : PropertyEditorDrawer<PersistentGetValueEditor>
	{
	}
}