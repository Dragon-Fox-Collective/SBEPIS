//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace ArborEditor
{
	using Arbor;

	internal sealed class ParameterConditionListEditor : PropertyEditor
	{
		ReorderableList _ConditionsList;

		SerializedProperty _IntParameters;
		SerializedProperty _LongParameters;
		SerializedProperty _FloatParameters;
		SerializedProperty _BoolParameters;
		SerializedProperty _StringParameters;
		SerializedProperty _EnumParameters;
		SerializedProperty _Vector2Parameters;
		SerializedProperty _Vector3Parameters;
		SerializedProperty _QuaternionParameters;
		SerializedProperty _RectParameters;
		SerializedProperty _BoundsParameters;
		SerializedProperty _ColorParameters;
		SerializedProperty _Vector4Parameters;
		SerializedProperty _Vector2IntParameters;
		SerializedProperty _Vector3IntParameters;
		SerializedProperty _RectIntParameters;
		SerializedProperty _BoundsIntParameters;
		SerializedProperty _GameObjectParameters;
		SerializedProperty _ComponentParameters;
		SerializedProperty _AssetObjectParameters;

		private LayoutArea _LayoutArea = new LayoutArea();

		private PropertyHeightCache _PropertyHeights = new PropertyHeightCache();

		protected override void OnInitialize()
		{
			_ConditionsList = new ReorderableList(property.serializedObject, property.FindPropertyRelative("_Conditions"))
			{
				drawHeaderCallback = DrawHeader,
				onAddCallback = OnAdd,
				onRemoveCallback = OnRemove,
				drawElementCallback = DrawElement,
				elementHeightCallback = GetElementHeight,
			};

			_IntParameters = property.FindPropertyRelative("_IntParameters");
			_LongParameters = property.FindPropertyRelative("_LongParameters");
			_FloatParameters = property.FindPropertyRelative("_FloatParameters");
			_BoolParameters = property.FindPropertyRelative("_BoolParameters");
			_EnumParameters = property.FindPropertyRelative("_EnumParameters");
			_StringParameters = property.FindPropertyRelative("_StringParameters");
			_Vector2Parameters = property.FindPropertyRelative("_Vector2Parameters");
			_Vector3Parameters = property.FindPropertyRelative("_Vector3Parameters");
			_QuaternionParameters = property.FindPropertyRelative("_QuaternionParameters");
			_RectParameters = property.FindPropertyRelative("_RectParameters");
			_BoundsParameters = property.FindPropertyRelative("_BoundsParameters");
			_ColorParameters = property.FindPropertyRelative("_ColorParameters");
			_Vector4Parameters = property.FindPropertyRelative("_Vector4Parameters");
			_Vector2IntParameters = property.FindPropertyRelative("_Vector2IntParameters");
			_Vector3IntParameters = property.FindPropertyRelative("_Vector3IntParameters");
			_RectIntParameters = property.FindPropertyRelative("_RectIntParameters");
			_BoundsIntParameters = property.FindPropertyRelative("_BoundsIntParameters");
			_GameObjectParameters = property.FindPropertyRelative("_GameObjectParameters");
			_ComponentParameters = property.FindPropertyRelative("_ComponentParameters");
			_AssetObjectParameters = property.FindPropertyRelative("_AssetObjectParameters");
		}

		void DrawHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, property.displayName);
		}

		void AddParameter(ParameterConditionProperty conditionProperty, Parameter.Type parameterType)
		{
			conditionProperty.parameterType = parameterType;

			SerializedProperty parametersProperty = GetParametersProperty(parameterType);

			if (parametersProperty != null)
			{
				parametersProperty.arraySize++;
				int parameterIndex = parametersProperty.arraySize - 1;

				SerializedProperty valueProperty = parametersProperty.GetArrayElementAtIndex(parameterIndex);

				FlexibleFieldPropertyBase fieldProperty = null;
				switch (parameterType)
				{
					case Parameter.Type.Int:
					case Parameter.Type.Float:
					case Parameter.Type.Long:
						fieldProperty = new FlexibleNumericProperty(valueProperty);
						break;
					case Parameter.Type.Bool:
						fieldProperty = new FlexibleBoolProperty(valueProperty);
						break;
					default:
						fieldProperty = new FlexibleFieldProperty(valueProperty);
						break;
				}

				fieldProperty.ClearSlot();
				fieldProperty.Clear(true);

				conditionProperty.parameterIndex = parameterIndex;
			}
		}

		void AddParameter(ParameterConditionProperty conditionProperty)
		{
			AddParameter(conditionProperty, conditionProperty.parameterType);
		}

		private void OnAdd(ReorderableList list)
		{
			ReorderableList.defaultBehaviours.DoAddButton(list);
			SerializedProperty elementProperty = list.serializedProperty.GetArrayElementAtIndex(list.index);
			ParameterConditionProperty conditionProperty = new ParameterConditionProperty(elementProperty);

			conditionProperty.referenceProperty.Clear();
			conditionProperty.parameterTypeProperty.Clear();
			conditionProperty.logicalConditionProperty.Clear();
			conditionProperty.compareTypeProperty.Clear();

			AddParameter(conditionProperty);
		}

		private void OnRemove(ReorderableList list)
		{
			SerializedProperty elementProperty = list.serializedProperty.GetArrayElementAtIndex(list.index);
			ParameterConditionProperty conditionProperty = new ParameterConditionProperty(elementProperty);
			DeleteParameter(conditionProperty, conditionProperty.parameterType, true);

			ParameterCondition condition = SerializedPropertyUtility.GetPropertyObject<ParameterCondition>(elementProperty);
			if (condition != null)
			{
				condition.Destroy();
			}

			ReorderableList.defaultBehaviours.DoRemoveButton(list);
		}

		void DoGUI(SerializedProperty elementProperty)
		{
			ParameterConditionProperty conditionProperty = new ParameterConditionProperty(elementProperty);

			Rect prevRect = _LayoutArea.rect;

			_LayoutArea.PropertyField(conditionProperty.logicalConditionProperty);

			Parameter.Type oldParameterType = conditionProperty.parameterType;

			EditorGUI.BeginChangeCheck();
			_LayoutArea.PropertyField(conditionProperty.referenceProperty.property);
			if (EditorGUI.EndChangeCheck())
			{
				Parameter.Type parameterType = conditionProperty.GetParameterType();
				if (parameterType != oldParameterType)
				{
					OnChangeParameterType(conditionProperty, oldParameterType, parameterType);

					elementProperty.serializedObject.ApplyModifiedProperties();

					GUIUtility.ExitGUI();
				}
			}

			ParameterReferenceType parameterReferenceType = conditionProperty.referenceProperty.type;
			if (parameterReferenceType == ParameterReferenceType.DataSlot)
			{
				EditorGUI.BeginChangeCheck();
				_LayoutArea.PropertyField(conditionProperty.parameterTypeProperty);
				if (EditorGUI.EndChangeCheck())
				{
					Parameter.Type parameterType = conditionProperty.parameterType;
					if (parameterType != oldParameterType)
					{
						OnChangeParameterType(conditionProperty, oldParameterType, parameterType);

						conditionProperty.referenceType = null;

						elementProperty.serializedObject.ApplyModifiedProperties();

						GUIUtility.ExitGUI();
					}
				}

				if (ClassTypeConstraintEditorUtility.GetParameterTypeConstraint(oldParameterType, out var constraint))
				{
					System.Type oldReferenceType = conditionProperty.referenceType;

					if (constraint != null)
					{
						conditionProperty.referenceTypeProperty.SetConstraint(constraint);
					}

					EditorGUI.BeginChangeCheck();
					_LayoutArea.PropertyField(conditionProperty.referenceTypeProperty.property);
					if (EditorGUI.EndChangeCheck())
					{
						System.Type referenceType = conditionProperty.referenceType;
						if (referenceType != oldReferenceType)
						{
							DeleteParameter(conditionProperty, oldParameterType, false);

							elementProperty.serializedObject.ApplyModifiedProperties();

							GUIUtility.ExitGUI();
						}
					}
				}
			}

			if (parameterReferenceType == ParameterReferenceType.DataSlot || conditionProperty.referenceProperty.container != null)
			{
				ConditionGUI(conditionProperty);
			}

			if (Event.current.type == EventType.Repaint && Application.isPlaying)
			{
				ParameterCondition condition = SerializedPropertyUtility.GetPropertyObject<ParameterCondition>(elementProperty);
				if (condition != null)
				{
					var currentRect = _LayoutArea.rect;

					var conditionRect = new Rect(prevRect);
					conditionRect.x -= ReorderableList.Defaults.dragHandleWidth;
					conditionRect.yMax = currentRect.yMax;
					conditionRect.width = 5f;

					conditionRect = new RectOffset(1, 1, 1, 1).Remove(conditionRect);

					EditorGUI.DrawRect(conditionRect, EditorGUITools.GetConditionColor(condition.conditionResult));
				}
			}
		}

		private void ConditionGUI(ParameterConditionProperty conditionProperty)
		{
			Parameter.Type parameterType = conditionProperty.parameterType;

			int parameterIndex = conditionProperty.parameterIndex;

			SerializedProperty compareTypeProperty = conditionProperty.compareTypeProperty;
			SerializedProperty parametersProperty = GetParametersProperty(parameterType);
			if (parametersProperty != null && (parameterIndex < 0 || parametersProperty.arraySize <= parameterIndex))
			{
				_LayoutArea.HelpBox(Localization.GetWord("ParameterCondition.NoParameterField"), MessageType.Warning);

				if (_LayoutArea.Button(Localization.GetTextContent("Repair")))
				{
					AddParameter(conditionProperty);
				}

				return;
			}

			SerializedProperty valueProperty = parametersProperty != null ? parametersProperty.GetArrayElementAtIndex(parameterIndex) : null;

			switch (parameterType)
			{
				case Parameter.Type.Int:
					{
						_LayoutArea.PropertyField(compareTypeProperty);
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Int Value"));
					}
					break;
				case Parameter.Type.Long:
					{
						_LayoutArea.PropertyField(compareTypeProperty);
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Long Value"));
					}
					break;
				case Parameter.Type.Float:
					{
						_LayoutArea.PropertyField(compareTypeProperty);
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Float Value"));
					}
					break;
				case Parameter.Type.Bool:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Bool Value"));
					}
					break;
				case Parameter.Type.String:
					{
						_LayoutArea.PropertyField(compareTypeProperty);
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("String Value"));
					}
					break;
				case Parameter.Type.GameObject:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("GameObject Value"));
					}
					break;
				case Parameter.Type.Vector2:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Vector2 Value"));
					}
					break;
				case Parameter.Type.Vector3:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Vector3 Value"));
					}
					break;
				case Parameter.Type.Quaternion:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Quaternion Value"));
					}
					break;
				case Parameter.Type.Rect:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Rect Value"));
					}
					break;
				case Parameter.Type.Bounds:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Bounds Value"));
					}
					break;
				case Parameter.Type.Color:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Color Value"));
					}
					break;
				case Parameter.Type.Vector4:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Vector4 Value"));
					}
					break;
				case Parameter.Type.Vector2Int:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Vector2Int Value"));
					}
					break;
				case Parameter.Type.Vector3Int:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Vector3Int Value"));
					}
					break;
				case Parameter.Type.RectInt:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("RectInt Value"));
					}
					break;
				case Parameter.Type.BoundsInt:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("BoundsInt Value"));
					}
					break;
				case Parameter.Type.Transform:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Transform Value"));
					}
					break;
				case Parameter.Type.RectTransform:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("RectTransform Value"));
					}
					break;
				case Parameter.Type.Rigidbody:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Rigidbody Value"));
					}
					break;
				case Parameter.Type.Rigidbody2D:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Rigidbody2D Value"));
					}
					break;
				case Parameter.Type.Component:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Component Value"));
					}
					break;
				case Parameter.Type.AssetObject:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("AssetObject Value"));
					}
					break;
				case Parameter.Type.Enum:
					{
						_LayoutArea.PropertyField(valueProperty, GUIContentCaches.Get("Enum Value"));
					}
					break;
				case Parameter.Type.Variable:
					{
						Parameter parameter = conditionProperty.referenceProperty.GetParameter();
						string valueTypeName = (parameter != null && parameter.valueType != null) ? parameter.valueType.ToString() : "Variable";
						string message = string.Format(Localization.GetWord("ParameterCondition.NotSupportVariable"), valueTypeName);

						_LayoutArea.HelpBox(message, MessageType.Warning);
					}
					break;
				case Parameter.Type.VariableList:
					{
						Parameter parameter = conditionProperty.referenceProperty.GetParameter();
						string valueTypeName = (parameter != null && parameter.valueType != null) ? parameter.valueType.ToString() : "VariableList";
						string message = string.Format(Localization.GetWord("ParameterCondition.NotSupportVariableList"), valueTypeName);

						_LayoutArea.HelpBox(message, MessageType.Warning);
					}
					break;
			}
		}

		static readonly RectOffset s_LayoutMargin = new RectOffset(0, 0, 2, 2);

		void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty property = _ConditionsList.serializedProperty.GetArrayElementAtIndex(index);

			_LayoutArea.Begin(rect, false, s_LayoutMargin);

			DoGUI(property);

			_LayoutArea.End();
		}

		float GetElementHeight(int index)
		{
			SerializedProperty property = _ConditionsList.serializedProperty.GetArrayElementAtIndex(index);

			float height = 0f;
			if (!_PropertyHeights.TryGetHeight(property, out height))
			{
				_LayoutArea.Begin(new Rect(), true, s_LayoutMargin);

				DoGUI(property);

				_LayoutArea.End();

				height = _LayoutArea.rect.height;

				_PropertyHeights.AddHeight(property, height);
			}

			return height;
		}

		SerializedProperty GetParametersProperty(Parameter.Type parameterType)
		{
			switch (parameterType)
			{
				case Parameter.Type.Int:
					return _IntParameters;
				case Parameter.Type.Long:
					return _LongParameters;
				case Parameter.Type.Float:
					return _FloatParameters;
				case Parameter.Type.Bool:
					return _BoolParameters;
				case Parameter.Type.String:
					return _StringParameters;
				case Parameter.Type.Enum:
					return _EnumParameters;
				case Parameter.Type.GameObject:
					return _GameObjectParameters;
				case Parameter.Type.Vector2:
					return _Vector2Parameters;
				case Parameter.Type.Vector3:
					return _Vector3Parameters;
				case Parameter.Type.Quaternion:
					return _QuaternionParameters;
				case Parameter.Type.Rect:
					return _RectParameters;
				case Parameter.Type.Bounds:
					return _BoundsParameters;
				case Parameter.Type.Color:
					return _ColorParameters;
				case Parameter.Type.Vector4:
					return _Vector4Parameters;
				case Parameter.Type.Vector2Int:
					return _Vector2IntParameters;
				case Parameter.Type.Vector3Int:
					return _Vector3IntParameters;
				case Parameter.Type.RectInt:
					return _RectIntParameters;
				case Parameter.Type.BoundsInt:
					return _BoundsIntParameters;
				case Parameter.Type.Transform:
				case Parameter.Type.RectTransform:
				case Parameter.Type.Rigidbody:
				case Parameter.Type.Rigidbody2D:
				case Parameter.Type.Component:
					return _ComponentParameters;
				case Parameter.Type.AssetObject:
					return _AssetObjectParameters;
				case Parameter.Type.Variable:
				case Parameter.Type.VariableList:
					return null;
			}

			return null;
		}

		void DeleteParameter(ParameterConditionProperty conditionProperty, Parameter.Type parameterType, bool deleteParameter)
		{
			SerializedProperty parametersProperty = GetParametersProperty(parameterType);
			if (parametersProperty == null)
			{
				return;
			}

			int parameterIndex = conditionProperty.parameterIndex;
			if (parameterIndex < 0 || parametersProperty.arraySize <= parameterIndex)
			{
				return;
			}

			SerializedProperty valueProperty = parametersProperty.GetArrayElementAtIndex(parameterIndex);
			if (valueProperty == null)
			{
				return;
			}

			switch (FlexibleUtility.GetPropertyType(parameterType))
			{
				case FlexiblePropertyType.Primitive:
					{
						FlexiblePrimitiveProperty flexibleProperty = new FlexiblePrimitiveProperty(valueProperty);
						flexibleProperty.Disconnect();
					}
					break;
				case FlexiblePropertyType.Field:
					{
						FlexibleFieldProperty flexibleProperty = new FlexibleFieldProperty(valueProperty);
						flexibleProperty.Disconnect();
					}
					break;
				case FlexiblePropertyType.SceneObject:
					{
						FlexibleSceneObjectProperty flexibleProperty = new FlexibleSceneObjectProperty(valueProperty);
						flexibleProperty.Disconnect();
					}
					break;
			}

			if (deleteParameter)
			{
				parametersProperty.DeleteArrayElementAtIndex(parameterIndex);

				for (int i = 0, count = _ConditionsList.serializedProperty.arraySize; i < count; i++)
				{
					SerializedProperty p = _ConditionsList.serializedProperty.GetArrayElementAtIndex(i);
					ParameterConditionProperty cp = new ParameterConditionProperty(p);
					Parameter.Type t = cp.GetParameterType();
					SerializedProperty parameters = GetParametersProperty(t);
					if (!SerializedPropertyUtility.EqualContents(parameters, parametersProperty))
					{
						continue;
					}
					if (cp.parameterIndex > parameterIndex)
					{
						cp.parameterIndex--;
					}
				}
			}
		}

		void OnChangeParameterType(ParameterConditionProperty conditionProperty, Parameter.Type oldType, Parameter.Type newType)
		{
			DeleteParameter(conditionProperty, oldType, true);

			AddParameter(conditionProperty, newType);
		}

		void ClearCache()
		{
			_PropertyHeights.Clear();
		}

		protected override void OnGUI(Rect position, GUIContent label)
		{
			if (Event.current.type == EventType.Layout)
			{
				ClearCache();
			}

			if (_ConditionsList != null)
			{
				var oldIndentLevel = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
				_ConditionsList.DoList(position);
				EditorGUI.indentLevel = oldIndentLevel;
			}
		}

		protected override float GetHeight(GUIContent label)
		{
			if (Event.current.type == EventType.Layout)
			{
				ClearCache();
			}

			float height = 0f;
			if (_ConditionsList != null)
			{
				var oldIndentLevel = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
				height = _ConditionsList.GetHeight();
				EditorGUI.indentLevel = oldIndentLevel;
			}

			return height;
		}
	}

	[CustomPropertyDrawer(typeof(ParameterConditionList))]
	internal sealed class ParameterConditionListPropertyDrawer : PropertyEditorDrawer<ParameterConditionListEditor>
	{
	}
}