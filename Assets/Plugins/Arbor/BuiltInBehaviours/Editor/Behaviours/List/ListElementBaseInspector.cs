//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using Arbor.Events;
	using ArborEditor.Events;

	internal class ListElementBaseInspector : ListBehaviourBaseInspector
	{
		private const string kParameterTypePath = "_ParameterType";
		private const string kParameterListPath = "_ParameterList";

		private SerializedProperty _ParameterTypeProperty;
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

		protected override void OnEnable()
		{
			base.OnEnable();

			_ParameterTypeProperty = serializedObject.FindProperty(kParameterTypePath);
			_ParameterListProperty = new ParameterListProperty(serializedObject.FindProperty(kParameterListPath));
		}

		void UpdateElementType()
		{
			if (parameterType != ParameterType.Slot)
			{
				return;
			}

			SerializedProperty parametersProperty = _ParameterListProperty.GetParametersProperty(parameterType);
			if (parametersProperty == null)
			{
				return;
			}

			System.Type elementType = elementTypeProperty.type;

			for (int i = 0; i < parametersProperty.arraySize; i++)
			{
				SerializedProperty valueProperty = parametersProperty.GetArrayElementAtIndex(i);

				InputSlotTypableProperty slotProperty = new InputSlotTypableProperty(valueProperty);
				slotProperty.type = elementType;
			}
		}

		void SetParameterType(ParameterType newParameterType)
		{
			if (parameterType != newParameterType)
			{
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
				}
			}

			UpdateElementType();
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

		protected override void OnChangeElementType(System.Type currentType, System.Type newType)
		{
			ParameterType newParameterType = ArborEventUtility.GetParameterType(newType, true);

			SetParameterType(newParameterType);
		}

		protected void ElementGUI()
		{
			System.Type elementType = elementTypeProperty.type;

			bool hasAssemblyTypeName = !string.IsNullOrEmpty(elementTypeProperty.assemblyTypeName.stringValue);

			bool disabledParameterArgument = elementType == null && hasAssemblyTypeName;

			bool hasRepairParameter = false;

			if (!disabledParameterArgument)
			{
				ParameterType newParameterType = ArborEventUtility.GetParameterType(elementType, true);

				if (parameterType != newParameterType)
				{
					hasRepairParameter = true;
					disabledParameterArgument = true;
				}
			}

			using (new EditorGUI.DisabledGroupScope(disabledParameterArgument))
			{
				SerializedProperty valueProperty = _ParameterListProperty.GetValueProperty(parameterType, 0);
				if (valueProperty != null)
				{
					EditorGUILayout.PropertyField(valueProperty, GUIContentCaches.Get("Element"), true);
				}
			}

			if (hasRepairParameter)
			{
				ParameterType newParameterType = ArborEventUtility.GetParameterType(elementType, true);

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
	}
}