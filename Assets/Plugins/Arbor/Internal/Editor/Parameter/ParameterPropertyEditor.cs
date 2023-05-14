//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	internal sealed class ParameterPropertyEditor : PropertyEditor
	{
		public ParameterContainerInternalInspector editor;
		public bool isInParametersPanel
		{
			get
			{
				return editor != null ? editor.isInParametersPanel : false;
			}
		}

		private static Dictionary<Parameter.Type, string> s_ParameterTypeNames = new Dictionary<Parameter.Type, string>();

		static string GetParameterTypeName(Parameter.Type parameterType)
		{
			if (s_ParameterTypeNames.TryGetValue(parameterType, out var name))
			{
				return name;
			}

			name = System.Enum.GetName(typeof(Parameter.Type), parameterType);
			s_ParameterTypeNames.Add(parameterType, name);

			return name;
		}

		static class Defaults
		{
			public static readonly GUIContent valueContent;
			public static readonly GUIContent notFoundValueContent;
			public static readonly GUIContent getContent;
			public static readonly GUIContent setContent;
			public static readonly RectOffset layoutMargin;

			static Defaults()
			{
				valueContent = GUIContentCaches.Get("Value");
				notFoundValueContent = GUIContentCaches.Get("Error: not found value");
				getContent = GUIContentCaches.Get("Get");
				setContent = GUIContentCaches.Get("Set");

				layoutMargin = new RectOffset(0, 0, 0, 2);
			}
		}

		private sealed class VariableListEditor : ListParameterEditorBase
		{
			protected override sealed SerializedProperty GetListProperty()
			{
				return property;
			}
		}

		private static readonly string s_InvalidParameterMessage = "Invalid Parameter";
		private static readonly int s_DragParameterHash = "s_DragParameterHash".GetHashCode();
		
		static bool IsTypeObject(Parameter.Type type)
		{
			switch (type)
			{
				case Parameter.Type.GameObject:
				case Parameter.Type.Transform:
				case Parameter.Type.RectTransform:
				case Parameter.Type.Rigidbody:
				case Parameter.Type.Rigidbody2D:
				case Parameter.Type.Component:
				case Parameter.Type.AssetObject:
				case Parameter.Type.Variable:
				case Parameter.Type.VariableList:
					return true;
			}

			return false;
		}

		static System.Type GetObjectType(Parameter.Type type)
		{
			switch (type)
			{
				case Parameter.Type.GameObject:
					return typeof(GameObject);
				case Parameter.Type.Transform:
					return typeof(Transform);
				case Parameter.Type.RectTransform:
					return typeof(RectTransform);
				case Parameter.Type.Rigidbody:
					return typeof(Rigidbody);
				case Parameter.Type.Rigidbody2D:
					return typeof(Rigidbody2D);
				case Parameter.Type.Component:
					return typeof(Component);
				case Parameter.Type.AssetObject:
					return typeof(Object);
				case Parameter.Type.Variable:
					return typeof(VariableBase);
				case Parameter.Type.VariableList:
					return typeof(VariableListBase);
				default:
					throw new System.ArgumentException("Parameter type not an Object type(" + type + ")");
			}
		}

		private LayoutArea _LayoutArea = new LayoutArea();
		private SerializedProperty _NameProperty;
		private SerializedProperty _ParameterIndexProperty;
		private SerializedProperty _TypeProperty;
		private SerializedProperty _ContainerProperty;
		private SerializedProperty _IdProperty;
		private ClassTypeReferenceProperty _ReferenceTypeProperty;
		private SerializedProperty _IsPublicSetProperty;
		private SerializedProperty _IsPublicGetProperty;

		protected override void OnInitialize()
		{
			base.OnInitialize();

			_NameProperty = property.FindPropertyRelative("name");
			_ParameterIndexProperty = property.FindPropertyRelative("_ParameterIndex");
			_TypeProperty = property.FindPropertyRelative("type");
			_ContainerProperty = property.FindPropertyRelative("container");
			_IdProperty = property.FindPropertyRelative("id");
			_ReferenceTypeProperty = new ClassTypeReferenceProperty(property.FindPropertyRelative("referenceType"));
			_IsPublicSetProperty = property.FindPropertyRelative("_IsPublicSet");
			_IsPublicGetProperty = property.FindPropertyRelative("_IsPublicGet");
		}

		void DragParameter(Rect dragRect)
		{
			ParameterContainerInternal container = _ContainerProperty.objectReferenceValue as ParameterContainerInternal;
			Parameter parameter = container.GetParam(_IdProperty.intValue);

			dragRect.xMin = dragRect.xMax - 18f;

			int controlID = GUIUtility.GetControlID(s_DragParameterHash, FocusType.Passive, dragRect);

			Event current = Event.current;

			EventType eventType = current.GetTypeForControl(controlID);
			switch (eventType)
			{
				case EventType.MouseDown:
					if (dragRect.Contains(current.mousePosition))
					{
						DragAndDrop.PrepareStartDrag();

						ParameterDraggingObject draggingObject = ParameterDraggingObject.instance;

						draggingObject.parameter = parameter;

						DragAndDrop.objectReferences = new Object[] { draggingObject };
						DragAndDrop.paths = null;
						DragAndDrop.StartDrag(_NameProperty.stringValue);

						current.Use();
					}
					break;
				case EventType.Repaint:
					{

						Color backgroundColor = GUI.backgroundColor;
						GUI.backgroundColor = EditorGUITools.GetTypeColor(parameter.valueType);

						GUIStyle dragStyle = ArborEditor.Styles.dropField;
						dragStyle.Draw(dragRect, GUIContent.none, controlID);

						GUIStyle dragPinStyle = ArborEditor.Styles.GetDataInPin(parameter.valueType);
						dragPinStyle.Draw(DataSlotGUI.Defaults.dataPinPadding.Remove(dragRect), EditorContents.dragParameterPin, controlID);

						GUI.backgroundColor = backgroundColor;
					}
					break;
			}
		}

		void DoGUI()
		{
			int parameterIndex = _ParameterIndexProperty.intValue;

			Parameter.Type type = EnumUtility.GetValueFromIndex<Parameter.Type>(_TypeProperty.enumValueIndex);
			SerializedProperty parametersProperty = editor.GetParametersProperty(type);

			SerializedProperty valueProperty = null;
			if (parametersProperty != null && 0 <= parameterIndex && parameterIndex < parametersProperty.arraySize)
			{
				valueProperty = parametersProperty.GetArrayElementAtIndex(parameterIndex);
			}

			bool changeLabelWidth = _LayoutArea.rect.width - EditorGUIUtility.labelWidth < EditorGUIUtility.fieldWidth;
			if (changeLabelWidth)
			{
				EditorGUIUtility.labelWidth = _LayoutArea.rect.width - EditorGUIUtility.fieldWidth;
			}

			string label = string.Empty;
			if ((type == Parameter.Type.Variable || type == Parameter.Type.VariableList) && valueProperty != null)
			{
				Object variableObj = valueProperty.objectReferenceValue;
				if (variableObj != null)
				{
					var behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(variableObj);
					label = behaviourInfo.titleContent.text;
				}
				else
				{
					label = "Invalid";
				}
			}
			else
			{
				label = GetParameterTypeName(type);
			}

			float verticalSpacing = Mathf.Floor((editor.parameterList.elementHeight - EditorGUIUtility.singleLineHeight) * 0.5f) - 2f;

			_LayoutArea.Space(verticalSpacing);

			_LayoutArea.BeginHorizontal();

			_LayoutArea.TextField(GUIContentCaches.Get(label), _NameProperty, LayoutArea.Width(_LayoutArea.rect.width - 20f));

			Rect dragRect = _LayoutArea.GetRect(0f, EditorGUIUtility.singleLineHeight);

			if (_LayoutArea.IsDraw())
			{
				DragParameter(dragRect);
			}

			_LayoutArea.EndHorizontal();

			_LayoutArea.Space(verticalSpacing);

			GUIContent valueContent = Defaults.valueContent;

			if (valueProperty != null)
			{
				if (type == Parameter.Type.Enum)
				{
					_ReferenceTypeProperty.SetConstraint(ClassTypeConstraintEditorUtility.enumField);

					_LayoutArea.PropertyField(_ReferenceTypeProperty.property, GUIContent.none, true);

					System.Type enumType = _ReferenceTypeProperty.type;

					if (!EnumFieldUtility.IsEnum(enumType))
					{
						_LayoutArea.PropertyField(valueProperty, valueContent, true);
					}
					else
					{
						object enumValue = System.Enum.ToObject(enumType, valueProperty.intValue);
						if (AttributeHelper.HasAttribute<System.FlagsAttribute>(enumType))
						{
							enumValue = _LayoutArea.EnumMaskField(valueContent, (System.Enum)enumValue);
						}
						else
						{
							enumValue = _LayoutArea.EnumPopup(valueContent, (System.Enum)enumValue);
						}
						valueProperty.intValue = (int)enumValue;
					}
				}
				else if (type == Parameter.Type.Variable)
				{
					VariableBase variable = valueProperty.objectReferenceValue as VariableBase;
					if (variable != null)
					{
						SerializedObject serializedObject = editor.GetVariableSerializedObject(variable);

						serializedObject.Update();

						SerializedProperty parameterProperty = serializedObject.FindProperty("_Parameter");

						if (parameterProperty != null)
						{
							_LayoutArea.PropertyField(parameterProperty, valueContent, true);
						}

						serializedObject.ApplyModifiedProperties();
					}
					else
					{
						_LayoutArea.HelpBox(s_InvalidParameterMessage, MessageType.Error);
					}
				}
				else if (type == Parameter.Type.EnumList)
				{
					_ReferenceTypeProperty.SetConstraint(ClassTypeConstraintEditorUtility.enumField);

					_LayoutArea.PropertyField(_ReferenceTypeProperty.property, GUIContent.none, true);

					System.Type enumType = _ReferenceTypeProperty.type;

					bool isEnum = enumType != null && TypeUtility.IsEnum(enumType);

					if (isEnum)
					{
						valueProperty.SetStateData<System.Type>(enumType);

						_LayoutArea.PropertyField(valueProperty, valueContent, true);
					}
					else
					{
						_LayoutArea.HelpBox(Localization.GetWord("ParameterContainer.SelectReferenceType"), MessageType.Warning);
					}
				}
				else if (type == Parameter.Type.ComponentList)
				{
					_ReferenceTypeProperty.SetConstraint(ClassTypeConstraintEditorUtility.component);

					System.Type objectType = _ReferenceTypeProperty.type ?? typeof(Component);

					EditorGUI.BeginChangeCheck();
					_LayoutArea.PropertyField(_ReferenceTypeProperty.property, GUIContent.none, true);
					if (EditorGUI.EndChangeCheck())
					{
						objectType = _ReferenceTypeProperty.type ?? objectType;

						SerializedProperty listProperty = valueProperty.FindPropertyRelative("list");
						for (int i = 0; i < listProperty.arraySize; i++)
						{
							SerializedProperty elementProperty = listProperty.GetArrayElementAtIndex(i);
							if (elementProperty.objectReferenceValue != null)
							{
								System.Type valueType = elementProperty.objectReferenceValue.GetType();
								if (!objectType.IsAssignableFrom(valueType))
								{
									elementProperty.objectReferenceValue = null;
								}
							}
						}
					}

					valueProperty.SetStateData<System.Type>(objectType);

					_LayoutArea.PropertyField(valueProperty, valueContent, true);
				}
				else if (type == Parameter.Type.AssetObjectList)
				{
					_ReferenceTypeProperty.SetConstraint(ClassTypeConstraintEditorUtility.asset);

					System.Type objectType = _ReferenceTypeProperty.type ?? typeof(Object);

					EditorGUI.BeginChangeCheck();
					_LayoutArea.PropertyField(_ReferenceTypeProperty.property, GUIContent.none, true);
					if (EditorGUI.EndChangeCheck())
					{
						objectType = _ReferenceTypeProperty.type ?? objectType;

						SerializedProperty listProperty = valueProperty.FindPropertyRelative("list");
						for (int i = 0; i < listProperty.arraySize; i++)
						{
							SerializedProperty elementProperty = listProperty.GetArrayElementAtIndex(i);
							if (elementProperty.objectReferenceValue != null)
							{
								System.Type valueType = elementProperty.objectReferenceValue.GetType();
								if (!objectType.IsAssignableFrom(valueType))
								{
									elementProperty.objectReferenceValue = null;
								}
							}
						}
					}

					valueProperty.SetStateData<System.Type>(objectType);

					_LayoutArea.PropertyField(valueProperty, valueContent, true);
				}
				else if (type == Parameter.Type.VariableList)
				{
					VariableListBase variable = valueProperty.objectReferenceValue as VariableListBase;
					if (variable != null)
					{
						SerializedObject serializedObject = editor.GetVariableSerializedObject(variable);

						serializedObject.Update();

						SerializedProperty parameterProperty = serializedObject.FindProperty("_Parameter");

						if (parameterProperty != null)
						{
							System.Type fieldType;
							var fieldInfo = SerializedPropertyUtility.GetFieldInfoFromProperty(parameterProperty, out fieldType);
							PropertyEditor propertyEditor = PropertyEditorUtility<VariableListEditor>.GetPropertyEditor(parameterProperty, fieldInfo);

							if (propertyEditor != null)
							{
								float height = propertyEditor.DoGetHeight(valueContent);
								Rect rect = _LayoutArea.GetRect(0, height);

								if (_LayoutArea.IsDraw())
								{
									propertyEditor.DoOnGUI(rect, valueContent);
								}
							}
							else
							{
								_LayoutArea.PropertyField(parameterProperty, valueContent, true);
							}
						}

						serializedObject.ApplyModifiedProperties();
					}
					else
					{
						_LayoutArea.HelpBox(s_InvalidParameterMessage, MessageType.Error);
					}
				}
				else if (IsTypeObject(type))
				{
					System.Type objectType = GetObjectType(type);
					bool allowSceneObjects = true;

					if (type == Parameter.Type.Component || type == Parameter.Type.AssetObject)
					{
						if (type == Parameter.Type.Component)
						{
							_ReferenceTypeProperty.SetConstraint(ClassTypeConstraintEditorUtility.component);
						}
						else if (type == Parameter.Type.AssetObject)
						{
							_ReferenceTypeProperty.SetConstraint(ClassTypeConstraintEditorUtility.asset);
							allowSceneObjects = false;
						}

						objectType = _ReferenceTypeProperty.type ?? objectType;

						EditorGUI.BeginChangeCheck();
						_LayoutArea.PropertyField(_ReferenceTypeProperty.property, GUIContent.none, true);
						if (EditorGUI.EndChangeCheck())
						{
							objectType = _ReferenceTypeProperty.type ?? objectType;

							if (valueProperty.objectReferenceValue != null)
							{
								System.Type valueType = valueProperty.objectReferenceValue.GetType();
								if (!objectType.IsAssignableFrom(valueType))
								{
									valueProperty.objectReferenceValue = null;
								}
							}
						}
					}

					_LayoutArea.ObjectField(valueContent, valueProperty, objectType, allowSceneObjects);
				}
				else
				{
					_LayoutArea.PropertyField(valueProperty, valueContent, true);
				}
			}
			else
			{
				_LayoutArea.LabelField(valueContent, Defaults.notFoundValueContent);
			}

			if (isInParametersPanel)
			{
				_LayoutArea.BeginHorizontal();

				_LayoutArea.VisibilityToggle(Defaults.setContent, _IsPublicSetProperty, LayoutArea.Width(50f));
				_LayoutArea.VisibilityToggle(Defaults.getContent, _IsPublicGetProperty, LayoutArea.Width(50f));

				_LayoutArea.EndHorizontal();
			}

			if (changeLabelWidth)
			{
				EditorGUIUtility.labelWidth = 0f;
			}
		}

		protected override void OnGUI(Rect position, GUIContent label)
		{
			_LayoutArea.Begin(position, false, Defaults.layoutMargin);

			DoGUI();

			_LayoutArea.End();
		}

		protected override float GetHeight(GUIContent label)
		{
			float height = 0f;

			_LayoutArea.Begin(new Rect(), true, Defaults.layoutMargin);

			DoGUI();

			_LayoutArea.End();

			height += _LayoutArea.rect.height;

			return height;
		}
	}
}