//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.Calculators
{
	using Arbor;
	using Arbor.Calculators;
	using Arbor.Events;
	using ArborEditor.Events;

	public class ListElementCalculatorBaseInspector : Editor
	{
		private const string kElementTypePath = "_ElementType";
		private const string kParameterTypePath = "_ParameterType";
		private const string kInputPath = "_Input";
		private const string kParameterListPath = "_ParameterList";

		private ClassTypeReferenceProperty _ElementTypeProperty;
		private SerializedProperty _ParameterTypeProperty;
		private InputSlotTypableProperty _InputProperty;
		private ParameterListProperty _ParameterListProperty;

		ParameterType parameterType
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

		protected virtual void OnEnable()
		{
			_ElementTypeProperty = new ClassTypeReferenceProperty(serializedObject.FindProperty(kElementTypePath));
			_ParameterTypeProperty = serializedObject.FindProperty(kParameterTypePath);
			_InputProperty = new InputSlotTypableProperty(serializedObject.FindProperty(kInputPath));
			_ParameterListProperty = new ParameterListProperty(serializedObject.FindProperty(kParameterListPath));
		}

		void SetParameterType(ParameterType newParameterType)
		{
			if (parameterType == newParameterType)
			{
				return;
			}

			_ParameterListProperty.DisconnectParameterSlots(parameterType);

			SerializedProperty oldParametersProperty = _ParameterListProperty.GetParametersProperty(parameterType);
			if (oldParametersProperty != null)
			{
				oldParametersProperty.ClearArray();
			}

			parameterType = newParameterType;

			SerializedProperty newParametersProperty = _ParameterListProperty.GetParametersProperty(parameterType);
			if (newParametersProperty != null)
			{
				newParametersProperty.arraySize = 1;

				if (parameterType == ParameterType.Slot)
				{
					SerializedProperty valueProperty = newParametersProperty.GetArrayElementAtIndex(0);

					InputSlotTypableProperty slotProperty = new InputSlotTypableProperty(valueProperty);
					slotProperty.type = _ElementTypeProperty.type;
				}
			}
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

		protected virtual void OnOutputGUI()
		{
		}

		public sealed override void OnInspectorGUI()
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
						System.Type listType = ListUtility.GetIListType(newType);
						string listTypeName = TypeUtility.TidyAssemblyTypeName(listType);

						if (_InputProperty.typeProperty.assemblyTypeName.stringValue != listTypeName)
						{
							_InputProperty.Disconnect();

							_InputProperty.type = listType;
						}

						ParameterType newParameterType = ArborEventUtility.GetParameterType(newType, true);

						if (parameterType == newParameterType)
						{
							if (parameterType == ParameterType.Slot)
							{
								SerializedProperty parametersProperty = _ParameterListProperty.GetParametersProperty(parameterType);
								if (parametersProperty != null)
								{
									SerializedProperty valueProperty = parametersProperty.GetArrayElementAtIndex(0);

									InputSlotTypableProperty slotProperty = new InputSlotTypableProperty(valueProperty);
									slotProperty.Disconnect();
									slotProperty.type = newType;
								}
							}
						}
						else
						{
							SetParameterType(newParameterType);
						}

						serializedObject.ApplyModifiedProperties();

						GUIUtility.ExitGUI(); // throw ExitGUIException
					}
				}

				OnOutputGUI();
			}

			bool disableInputField = false;
			bool disabledParameterArgument = false;

			bool hasAssemblyTypeName = !string.IsNullOrEmpty(_ElementTypeProperty.assemblyTypeName.stringValue);

			if (elementType == null)
			{
				if (hasAssemblyTypeName)
				{
					EditorGUILayout.HelpBox(Localization.GetWord("Array.Message.MissingType"), MessageType.Error);
					disabledParameterArgument = true;
				}
				disableInputField = true;
			}

			if (!disabledParameterArgument)
			{
				ParameterType newParameterType = ArborEventUtility.GetParameterType(elementType, true);

				if (parameterType != newParameterType)
				{
					disabledParameterArgument = true;

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

			using (new EditorGUI.DisabledGroupScope(disableInputField))
			{
				EditorGUILayout.PropertyField(_InputProperty.property);
			}

			using (new EditorGUI.DisabledGroupScope(disabledParameterArgument))
			{
				SerializedProperty valueProperty = _ParameterListProperty.GetValueProperty(parameterType, 0);
				if (valueProperty != null)
				{
					EditorGUILayout.PropertyField(valueProperty, GUIContentCaches.Get("Element"), true);
				}
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}