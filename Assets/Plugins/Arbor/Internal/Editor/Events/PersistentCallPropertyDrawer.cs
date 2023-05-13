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

	internal sealed class PersistentCallEditor : PropertyEditor
	{
		private const float kSpacing = 5;

		private static List<InvalidRepair> s_InvalidRepairs = new List<InvalidRepair>();

		private static class Default
		{
			public static readonly GUIContent targetContent;

			static Default()
			{
				targetContent = GUIContentCaches.Get("<Target>");
			}
		}

		private PersistentCallProperty _CallProperty;

		private LayoutArea _LayoutArea = new LayoutArea();

		protected override void OnInitialize()
		{
			base.OnInitialize();

			_CallProperty = new PersistentCallProperty(property);
		}

		void DoGUI()
		{
			using (new ProfilerScope("PersistentCallEditor.DoGUI"))
			{
				PersistentCallProperty call = _CallProperty;

				System.Type targetType = call.targetTypeProperty.type;
				string targetTypeName = call.targetTypeProperty.assemblyTypeName.stringValue;
				bool isTargetTypeMissing = targetType == null && !string.IsNullOrEmpty(targetTypeName);

				_LayoutArea.BeginHorizontal();

				EditorGUI.BeginChangeCheck();
				_LayoutArea.PropertyField(call.targetTypeProperty.property, GUIContent.none, false, LayoutArea.Width(EditorGUIUtility.labelWidth - kSpacing));
				if (EditorGUI.EndChangeCheck())
				{
					System.Type newTargetType = call.targetTypeProperty.type;
					if (targetType != newTargetType || isTargetTypeMissing)
					{
						targetType = newTargetType;

						call.ClearType();

						call.targetMode = PersistentCallProperty.GetTargetMode(targetType);

						if (targetType != null && targetType.IsValueType)
						{
							call.outputSlotInstanceProperty.type = targetType;
						}

						call.property.serializedObject.ApplyModifiedProperties();

						GUIUtility.ExitGUI();
					}
				}

				_LayoutArea.Space(kSpacing);

				EditorGUI.BeginDisabledGroup(targetType == null);

				MemberInfo memberInfo = call.memberInfo;

				Rect methodNameRect = _LayoutArea.GetRect(0f, EditorGUIUtility.singleLineHeight);

				if (_LayoutArea.IsDraw())
				{
					string memberName = call.memberNameProperty.stringValue;
					bool isEmpty = string.IsNullOrEmpty(memberName);
					bool isMissing = memberInfo == null && !isEmpty;

					string displayMemberName = call.GetMemberName();

					if (isMissing)
					{
						displayMemberName = string.Format("<Missing {0} >", displayMemberName);
					}
					else if (string.IsNullOrEmpty(displayMemberName))
					{
						displayMemberName = MemberSelector.kNoFunctionText;
					}

					MemberFilterFlags memberFilterFlags = (MemberFilterFlags)(-1) & ~(MemberFilterFlags.GetProperty | MemberFilterFlags.ReadOnlyField); // ignore GetProperty, ReadOnlyField

					EditorGUI.BeginChangeCheck();
					MemberInfo newMemberInfo = MemberSelector.PopupField(methodNameRect, memberInfo, targetType, true, memberFilterFlags, GUIContentCaches.Get(displayMemberName));
					if (EditorGUI.EndChangeCheck())
					{
						if (newMemberInfo != memberInfo || isMissing)
						{
							System.Type instanceType = ArborEventUtility.IsStatic(newMemberInfo) ? null : targetType;
							TargetMode newTargetMode = PersistentCallProperty.GetTargetMode(instanceType);
							if (call.targetMode != newTargetMode)
							{
								call.ClearType();
								call.targetMode = newTargetMode;
							}

							call.memberInfo = newMemberInfo;

							call.property.serializedObject.ApplyModifiedProperties();

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

				GUIContent targetContent = Default.targetContent;

				switch (call.targetMode)
				{
					case TargetMode.Component:
						{
							FlexibleComponentProperty targetComponentProperty = call.targetComponentProperty;
							
							_LayoutArea.PropertyField(targetComponentProperty.property, targetContent, true);
						}
						break;
					case TargetMode.GameObject:
						{
							FlexibleSceneObjectProperty targetGameObjectProperty = call.targetGameObjectProperty;

							_LayoutArea.PropertyField(targetGameObjectProperty.property, targetContent, true);
						}
						break;
					case TargetMode.AssetObject:
						{
							FlexibleFieldProperty targetAssetObjectProperty = call.targetAssetObjectProperty;
							
							_LayoutArea.PropertyField(targetAssetObjectProperty.property, targetContent, true);
						}
						break;
					case TargetMode.Slot:
						{
							InputSlotBaseProperty targetSlotProperty = call.targetSlotProperty;

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

				List<ArgumentProperty> argumentProperties = call.argumentProperties;
				for (int argumentIndex = 0; argumentIndex < argumentProperties.Count; argumentIndex++)
				{
					ArgumentProperty argumentProperty = argumentProperties[argumentIndex];

					System.Type argumentType = argumentProperty.type;

					string name = ObjectNames.NicifyVariableName(argumentProperty.name);
					GUIContent nameLabel = GUIContentCaches.Get(name);

					ArgumentAttributes attributes = argumentProperty.attributes;
					bool isOut = (attributes & ArgumentAttributes.Out) == ArgumentAttributes.Out;

					if (!isOut)
					{
						ParameterType parameterType = argumentProperty.parameterType;
						SerializedProperty parameterList = call.GetParametersProperty(parameterType);
						if (parameterList != null)
						{
							int parameterIndex = argumentProperty.parameterIndex;

							if (parameterIndex < 0 || parameterList.arraySize <= parameterIndex)
							{
								string message = string.Format("{0} : Unknown", name);

								_LayoutArea.HelpBox(message, MessageType.Error);

								continue;
							}

							SerializedProperty valueProperty = parameterList.GetArrayElementAtIndex(parameterIndex);

							bool isByRef = argumentType.IsByRef;
							SerializedProperty outputSlotProperty = isByRef ? call.outputSlotsProperty.GetArrayElementAtIndex(argumentProperty.outputSlotIndex) : null;

							float width = _LayoutArea.rect.width;

							_LayoutArea.BeginHorizontal();

							if (outputSlotProperty != null)
							{
								width -= 18;
							}

							_LayoutArea.PropertyField(valueProperty, nameLabel, true, LayoutArea.Width(width));

							if (outputSlotProperty != null)
							{
								_LayoutArea.PropertyField(outputSlotProperty, GUIContent.none, true);
							}

							_LayoutArea.EndHorizontal();
						}
					}
					else
					{
						SerializedProperty outputSlotProperty = call.outputSlotsProperty.GetArrayElementAtIndex(argumentProperty.outputSlotIndex);

						_LayoutArea.PropertyField(outputSlotProperty, nameLabel, true);
					}
				}

				System.Type returnType = call.returnTypeProperty.type;
				if (returnType != null)
				{
					int outputSlotIndex = call.returnOutputSlotIndexProperty.intValue;
					SerializedProperty outputSlotProperty = call.outputSlotsProperty.GetArrayElementAtIndex(outputSlotIndex);

					GUIContent returnLabel = GUIContentCaches.Get("Return");

					_LayoutArea.PropertyField(outputSlotProperty, returnLabel, true);
				}

				var outputSlotInstanceProperty = call.outputSlotInstanceProperty;

				if (targetType != null && targetType.IsValueType)
				{
					if (outputSlotInstanceProperty.type != targetType)
					{
						outputSlotInstanceProperty.type = targetType;

						outputSlotInstanceProperty.Disconnect();
					}

					GUIContent outputInstanceLabel = GUIContentCaches.Get("<Output Instance>");
					_LayoutArea.PropertyField(outputSlotInstanceProperty.property, outputInstanceLabel, true);
				}
				else if(outputSlotInstanceProperty.IsConnected())
				{
					outputSlotInstanceProperty.type = null;
					outputSlotInstanceProperty.Disconnect();
				}

				if (memberInfo != null)
				{
					s_InvalidRepairs.Clear();

					switch (call.memberType)
					{
						case MemberType.Method:
							{
								MethodInfo methodInfo = memberInfo as MethodInfo;
								if (methodInfo == null)
								{
									break;
								}

								ParameterInfo[] parameters = methodInfo.GetParameters();

								if (parameters != null)
								{
									for (int argumentIndex = 0; argumentIndex < parameters.Length; argumentIndex++)
									{
										ArgumentProperty argumentProperty = argumentProperties[argumentIndex];

										ParameterInfo parameterInfo = parameters[argumentIndex];

										ArgumentAttributes attributes = argumentProperty.attributes;
										bool isOut = (attributes & ArgumentAttributes.Out) == ArgumentAttributes.Out;

										if (parameterInfo.ParameterType.IsByRef && isOut != parameterInfo.IsOut)
										{
											if (parameterInfo.IsOut && !isOut)
											{
												s_InvalidRepairs.Add(new ArgumentToOutRepair(call, argumentIndex));
											}
											else if (!parameterInfo.IsOut && isOut)
											{
												s_InvalidRepairs.Add(new ArgumentToRefRepair(call, argumentIndex));
											}
										}
									}
								}

								System.Type methodReturnType = methodInfo.ReturnType;
								if (methodReturnType != null && methodReturnType == typeof(void))
								{
									methodReturnType = null;
								}

								if (methodReturnType != returnType)
								{
									s_InvalidRepairs.Add(new ReturnTypeRepair(call));
								}
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

								if (argumentProperties[0].type != fieldType)
								{
									s_InvalidRepairs.Add(new ValueTypeRepair(call, fieldType));
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

								if (argumentProperties[0].type != propertyType)
								{
									s_InvalidRepairs.Add(new ValueTypeRepair(call, propertyType));
								}
							}
							break;
					}

					if (s_InvalidRepairs.Count > 0)
					{
						StringBuilder invalidMessageBuilder = new StringBuilder();

						for (int repairIndex = 0; repairIndex < s_InvalidRepairs.Count; repairIndex++)
						{
							InvalidRepair repair = s_InvalidRepairs[repairIndex];
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
							for (int repairIndex = 0; repairIndex < s_InvalidRepairs.Count; repairIndex++)
							{
								InvalidRepair repair = s_InvalidRepairs[repairIndex];
								repair.OnRepair();
							}
						}
					}
				}
			}
		}

		static readonly RectOffset s_LayoutMargin = new RectOffset(0, 0, 0, 2);

		protected override void OnGUI(Rect position, GUIContent label)
		{
			_LayoutArea.Begin(position, false, s_LayoutMargin);

			DoGUI();

			_LayoutArea.End();
		}

		protected override float GetHeight(GUIContent label)
		{
			float height = 0f;

			_LayoutArea.Begin(new Rect(), true, s_LayoutMargin);

			DoGUI();

			_LayoutArea.End();

			height += _LayoutArea.rect.height;

			return height;
		}
	}

	[CustomPropertyDrawer(typeof(PersistentCall))]
	internal sealed class PersistentCallPropertyDrawer : PropertyEditorDrawer<PersistentCallEditor>
	{
	}
}