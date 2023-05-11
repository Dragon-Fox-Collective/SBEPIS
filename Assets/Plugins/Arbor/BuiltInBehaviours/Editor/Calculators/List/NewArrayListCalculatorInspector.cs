//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace ArborEditor.Calculators
{
	using Arbor;
	using Arbor.Calculators;
	using Arbor.Events;
	using ArborEditor.Events;

	[CustomEditor(typeof(NewArrayListCalculator))]
	internal sealed class NewArrayListCalculatorInspector : Editor
	{
		private const string kElementTypePath = "_ElementType";
		private const string kParameterTypePath = "_ParameterType";
		private const string kOutputPath = "_Output";
		private const string kOutputTypePath = "_OutputType";
		private const string kParameterListPath = "_ParameterList";

		private ClassTypeReferenceProperty _ElementTypeProperty;
		private SerializedProperty _ParameterTypeProperty;
		private OutputSlotTypableProperty _OutputProperty;
		private SerializedProperty _OutputTypeProperty;
		private ParameterListProperty _ParameterListProperty;
		private ReorderableListEx _ReorderableList;

		public ParameterType parameterType
		{
			get
			{
				return EnumUtility.GetValueFromIndex<ParameterType>(_ParameterTypeProperty.enumValueIndex);
			}
			set
			{
				_ParameterTypeProperty.enumValueIndex = EnumUtility.GetIndexFromValue<ParameterType>(value);
			}
		}

		public ArrayListType outputType
		{
			get
			{
				return EnumUtility.GetValueFromIndex<ArrayListType>(_OutputTypeProperty.enumValueIndex);
			}
			set
			{
				_OutputTypeProperty.enumValueIndex = EnumUtility.GetIndexFromValue<ArrayListType>(value);
			}
		}

		void OnEnable()
		{
			_ElementTypeProperty = new ClassTypeReferenceProperty(serializedObject.FindProperty(kElementTypePath));
			_ParameterTypeProperty = serializedObject.FindProperty(kParameterTypePath);
			_OutputProperty = new OutputSlotTypableProperty(serializedObject.FindProperty(kOutputPath));
			_OutputTypeProperty = serializedObject.FindProperty(kOutputTypePath);
			_ParameterListProperty = new ParameterListProperty(serializedObject.FindProperty(kParameterListPath));

			_ReorderableList = new ReorderableListEx(serializedObject, _ParameterListProperty.GetParametersProperty(ParameterType.Int), true, false, true, true)
			{
				onAddCallback = AddElement,
				headerHeight = 0f,
				drawElementCallback = DrawElement,
				elementHeightCallback = GetElementHeight,
			};
		}

		void AddElement(ReorderableList list)
		{
			list.serializedProperty.arraySize++;
			SerializedProperty valueProperty = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
			valueProperty.Clear(true);

			if (parameterType == ParameterType.Slot)
			{
				InputSlotTypableProperty slotProperty = new InputSlotTypableProperty(valueProperty);
				slotProperty.type = _ElementTypeProperty.type;
			}
		}

		void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			float verticalSpacing = Mathf.Floor((_ReorderableList.elementHeight - EditorGUIUtility.singleLineHeight) * 0.5f) - 2f;

			rect.yMin += verticalSpacing;

			SerializedProperty valueProperty = _ReorderableList.serializedProperty.GetArrayElementAtIndex(index);

			EditorGUI.PropertyField(rect, valueProperty, GUIContentCaches.Get(valueProperty.displayName), true);
		}

		float GetElementHeight(int index)
		{
			float height = 0.0f;

			float verticalSpacing = Mathf.Floor((_ReorderableList.elementHeight - EditorGUIUtility.singleLineHeight) * 0.5f) - 2f;
			height += verticalSpacing;

			SerializedProperty valueProperty = _ReorderableList.serializedProperty.GetArrayElementAtIndex(index);

			float propertyHeight = EditorGUI.GetPropertyHeight(valueProperty, GUIContentCaches.Get(valueProperty.displayName), true) + EditorGUIUtility.standardVerticalSpacing;
			height += propertyHeight;

			return height;
		}

		void SetParameterType(ParameterType newParameterType)
		{
			if (parameterType == newParameterType)
			{
				return;
			}

			_ParameterListProperty.DisconnectParameterSlots(parameterType);

			SerializedProperty parametersProperty = _ParameterListProperty.GetParametersProperty(parameterType);
			if (parametersProperty != null)
			{
				parametersProperty.ClearArray();
			}

			parameterType = newParameterType;
		}

		void MigrationParameterType(ParameterType newParameterType)
		{
			if (newParameterType == ParameterType.Unknown)
			{
				return;
			}

			SerializedProperty parametersProperty = _ParameterListProperty.GetParametersProperty(parameterType);
			SerializedProperty newParametersProperty = _ParameterListProperty.GetParametersProperty(newParameterType);

			newParametersProperty.arraySize = parametersProperty.arraySize;

			for (int i = 0; i < newParametersProperty.arraySize; i++)
			{
				SerializedProperty newProperty = newParametersProperty.GetArrayElementAtIndex(i);
				SerializedProperty oldProperty = parametersProperty.GetArrayElementAtIndex(i);

				if (parameterType == ParameterType.Slot)
				{
					InputSlotTypableProperty oldValueProperty = new InputSlotTypableProperty(oldProperty);

					if (oldValueProperty.branchID != 0)
					{
						SerializedProperty newValueProperty = newProperty;

						FlexibleFieldPropertyBase flexiblePropertyBase = ParameterListProperty.GetFlexibleFieldProperty(newParameterType, newValueProperty);

						if (flexiblePropertyBase != null)
						{
							flexiblePropertyBase.SetSlotType();

							flexiblePropertyBase.slotProperty.nodeGraph = oldValueProperty.nodeGraph;
							flexiblePropertyBase.slotProperty.branchID = oldValueProperty.branchID;
						}
					}
				}
				else
				{
					InputSlotTypableProperty newValueProperty = new InputSlotTypableProperty(newProperty);

					SerializedProperty oldValueProperty = oldProperty;

					FlexibleFieldPropertyBase flexiblePropertyBase = ParameterListProperty.GetFlexibleFieldProperty(parameterType, oldValueProperty);

					if (flexiblePropertyBase != null && flexiblePropertyBase.IsSlotType())
					{
						newValueProperty.nodeGraph = flexiblePropertyBase.slotProperty.nodeGraph;
						newValueProperty.branchID = flexiblePropertyBase.slotProperty.branchID;
					}
				}
			}
		}

		System.Type GetArrayListType(System.Type elementType)
		{
			switch (outputType)
			{
				case ArrayListType.Array:
					return elementType.MakeArrayType();
				case ArrayListType.List:
					return ListUtility.GetListType(elementType);
			}

			return null;
		}

		void SetOutputSlotType(System.Type elementType)
		{
			System.Type listType = elementType != null ? GetArrayListType(elementType) : null;
			string listTypeName = TypeUtility.TidyAssemblyTypeName(listType);

			if (_OutputProperty.typeProperty.assemblyTypeName.stringValue != listTypeName)
			{
				_OutputProperty.Disconnect();

				_OutputProperty.type = listType;
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			System.Type elementType = _ElementTypeProperty.type;

			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(_ElementTypeProperty.property, GUIContent.none, true);
				if (EditorGUI.EndChangeCheck())
				{
					System.Type newType = _ElementTypeProperty.type;

					if (elementType != newType)
					{
						ParameterType newParameterType = ArborEventUtility.GetParameterType(newType, true);

						if (parameterType == newParameterType)
						{
							_ParameterListProperty.DisconnectParameterSlots(parameterType);

							SerializedProperty parametersProperty = _ParameterListProperty.GetParametersProperty(parameterType);
							if (parametersProperty != null)
							{
								parametersProperty.ClearArray();
							}
						}
						else
						{
							SetParameterType(newParameterType);
						}

						SetOutputSlotType(newType);

						serializedObject.ApplyModifiedProperties();

						GUIUtility.ExitGUI(); // throw ExitGUIException
					}
				}

				using (new EditorGUI.DisabledGroupScope(_OutputProperty.type == null))
				{
					EditorGUILayout.PropertyField(_OutputProperty.property, GUIContentCaches.Get(_OutputProperty.property.displayName), true, GUILayout.Width(70f));
				}
			}

			bool disabledParameterList = false;

			if (elementType == null && !string.IsNullOrEmpty(_ElementTypeProperty.assemblyTypeName.stringValue))
			{
				EditorGUILayout.HelpBox(Localization.GetWord("Array.Message.MissingType"), MessageType.Error);
				disabledParameterList = true;
			}
			else
			{
				ParameterType newParameterType = ArborEventUtility.GetParameterType(elementType, true);

				if (parameterType != newParameterType)
				{
					disabledParameterList = true;

					SerializedProperty parametersProperty = _ParameterListProperty.GetParametersProperty(parameterType);

					if (parametersProperty.arraySize > 0)
					{
						EditorGUILayout.HelpBox(Localization.GetWord("ArborEvent.ChangedParameterType"), MessageType.Warning);
						if (GUILayout.Button(EditorContents.repair))
						{
							MigrationParameterType(newParameterType);

							SetParameterType(newParameterType);

							serializedObject.ApplyModifiedProperties();

							EditorGUIUtility.ExitGUI();
						}
					}
					else
					{
						SetParameterType(newParameterType);
					}
				}
			}

			using (new EditorGUI.DisabledGroupScope(disabledParameterList))
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(_OutputTypeProperty);
				if (EditorGUI.EndChangeCheck())
				{
					SetOutputSlotType(_ElementTypeProperty.type);
				}

				if (parameterType != ParameterType.Unknown)
				{
					SerializedProperty parametersProperty = _ParameterListProperty.GetParametersProperty(parameterType);

					if (parametersProperty != null)
					{
						_ReorderableList.serializedProperty = parametersProperty;
						_ReorderableList.DoLayoutList();

						GUILayout.Space(EditorGUIUtility.standardVerticalSpacing);
					}
				}
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}