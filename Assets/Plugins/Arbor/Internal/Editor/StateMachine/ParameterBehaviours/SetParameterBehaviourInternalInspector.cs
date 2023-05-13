//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.ParameterBehaviours
{
	using Arbor;
	using Arbor.ParameterBehaviours;
	using Arbor.Events;
	using ArborEditor.Events;

	[CustomEditor(typeof(SetParameterBehaviourInternal), true)]
	internal sealed class SetParameterBehaviourInternalInspector : NodeBehaviourEditor
	{
		private ParameterReferenceProperty _ParameterReferenceProperty = null;
		private ParameterReferenceEditorGUI _ParameterReferenceEditorGUI = null;
		private ClassTypeReferenceProperty _ValueTypeProperty;
		private SerializedProperty _ParameterTypeProperty;
		private ParameterListProperty _ParameterListProperty;
		private SerializedProperty _IsInGraphParameterProperty;

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

		private void OnEnable()
		{
			_ParameterReferenceProperty = new ParameterReferenceProperty(serializedObject.FindProperty("_ParameterReference"));
			_ParameterReferenceEditorGUI = new ParameterReferenceEditorGUI(_ParameterReferenceProperty);
			_IsInGraphParameterProperty = serializedObject.FindProperty("_IsInGraphParameter");
			_ValueTypeProperty = new ClassTypeReferenceProperty(serializedObject.FindProperty("_ValueType"));
			_ParameterTypeProperty = serializedObject.FindProperty("_ParameterType");
			_ParameterListProperty = new ParameterListProperty(serializedObject.FindProperty("_ParameterList"));
		}

		bool IsInGraphParameter(ParameterReferenceProperty parameterReferenceProperty)
		{
			ParameterContainerBase parameterContainer = (parameterReferenceProperty.type == ParameterReferenceType.Constant) ? parameterReferenceProperty.container : null;
			return parameterContainer != null && graphEditor != null && graphEditor.nodeGraph.parameterContainer != null && parameterContainer == graphEditor.nodeGraph.parameterContainer && parameterReferenceProperty.GetParameter() != null;
		}

		bool IsInGraphParameter()
		{
			return _IsInGraphParameterProperty.boolValue;
		}

		void UpdateValueType()
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

			System.Type valueType = _ValueTypeProperty.type;

			for (int i = 0; i < parametersProperty.arraySize; i++)
			{
				SerializedProperty valueProperty = parametersProperty.GetArrayElementAtIndex(i);

				InputSlotTypableProperty slotProperty = new InputSlotTypableProperty(valueProperty);
				slotProperty.type = valueType;
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

			UpdateValueType();
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

		void OnChangedValueType(System.Type newType)
		{
			ParameterType newParameterType = ArborEventUtility.GetParameterType(newType, true);

			if (parameterType == newParameterType)
			{
				_ParameterListProperty.DisconnectParameterSlots(parameterType);
			}

			SetParameterType(newParameterType);
		}

		void RepairSlotType(System.Type valueType)
		{
			_ValueTypeProperty.type = valueType;

			OnChangedValueType(valueType);
		}

		void OnChangedParameter(Parameter parameter)
		{
			if (parameter == null)
			{
				return;
			}

			System.Type valueType = _ValueTypeProperty.type;
			System.Type newType = parameter.valueType;

			if (newType != valueType)
			{
				RepairSlotType(newType);
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			EditorGUILayout.PropertyField(serializedObject.FindProperty("_ExecuteMethodFlags"));

			Parameter parameter = _ParameterReferenceProperty.GetParameter();
			System.Type valueType = _ValueTypeProperty.type;

			bool isInGraphParameter = IsInGraphParameter();

			string parameterName = _ParameterReferenceProperty.GetParameterName();

			GUIContent valueContent = GUIContent.none;

			if (isInGraphParameter)
			{
				EditorGUI.BeginChangeCheck();
				if (parameter == null)
				{
					_ParameterReferenceEditorGUI.ParameterFieldLayout();
				}
				else
				{
					_ParameterReferenceEditorGUI.DropParameterLayout();
				}

				if (EditorGUI.EndChangeCheck())
				{
					parameter = _ParameterReferenceProperty.GetParameter();
					if (parameter != null)
					{
						OnChangedParameter(parameter);
					}

					if (_ParameterReferenceProperty.type == ParameterReferenceType.Constant && !IsInGraphParameter(_ParameterReferenceProperty))
					{
						_IsInGraphParameterProperty.boolValue = false;
					}

					serializedObject.ApplyModifiedProperties();

					EditorGUIUtility.ExitGUI();
				}

				valueContent = GUIContentCaches.Get(parameterName);
			}
			else
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(_ParameterReferenceProperty.property);
				if (EditorGUI.EndChangeCheck())
				{
					parameter = _ParameterReferenceProperty.GetParameter();
					OnChangedParameter(parameter);

					serializedObject.ApplyModifiedProperties();

					EditorGUIUtility.ExitGUI();
				}

				if (_ParameterReferenceProperty.type == ParameterReferenceType.DataSlot)
				{
					EditorGUI.BeginChangeCheck();
					EditorGUILayout.PropertyField(_ValueTypeProperty.property);
					if (EditorGUI.EndChangeCheck())
					{
						System.Type newType = _ValueTypeProperty.type;

						OnChangedValueType(newType);

						serializedObject.ApplyModifiedProperties();

						EditorGUIUtility.ExitGUI();
					}
				}

				valueContent = GUIContentCaches.Get("Value");
			}

			bool hasAssemblyTypeName = !string.IsNullOrEmpty(_ValueTypeProperty.assemblyTypeName.stringValue);

			bool disabledParameterArgument = false;

			System.Action onRepair = null;

			System.Text.StringBuilder warningMessage = new System.Text.StringBuilder();

			System.Type parameterValueType = (parameter != null) ? parameter.valueType : null;

			ParameterReferenceType parameterReferenceType = _ParameterReferenceProperty.type;

			if (parameter != null && parameterReferenceType == ParameterReferenceType.Constant && parameterValueType != _ValueTypeProperty.type)
			{
				if (warningMessage.Length > 0)
				{
					warningMessage.AppendLine();
				}
				warningMessage.AppendFormat(Localization.GetWord("ParameterBehaviours.InvalidSlotType"), TypeUtility.GetTypeName(parameterValueType));

				onRepair += new System.Action(() =>
				{
					RepairSlotType(parameterValueType);
				});
				disabledParameterArgument = true;
			}
			else if (valueType == null && hasAssemblyTypeName)
			{
				disabledParameterArgument = true;
			}
			else
			{
				ParameterType newParameterType = ArborEventUtility.GetParameterType(valueType, true);

				if (parameterType != newParameterType)
				{
					if (warningMessage.Length > 0)
					{
						warningMessage.AppendLine();
					}
					warningMessage.AppendFormat(Localization.GetWord("ArborEvent.ChangedParameterType"));

					onRepair += new System.Action(() =>
					{
						MigrationParameterType(newParameterType);

						SetParameterType(newParameterType);
					});

					disabledParameterArgument = true;
				}
			}

			if (parameterType == ParameterType.Unknown || (parameterReferenceType == ParameterReferenceType.Constant && parameter == null))
			{
				if (warningMessage.Length > 0)
				{
					warningMessage.AppendLine();
				}
				warningMessage.Append(Localization.GetWord("ParameterBehaviours.SelectParameter"));

				disabledParameterArgument = true;
			}

			using (new EditorGUI.DisabledGroupScope(disabledParameterArgument))
			{
				SerializedProperty valueProperty = _ParameterListProperty.GetValueProperty(parameterType, 0);
				if (valueProperty != null)
				{
					EditorGUILayout.PropertyField(valueProperty, valueContent, true);
				}
			}

			if (warningMessage.Length > 0)
			{
				EditorGUILayout.HelpBox(warningMessage.ToString(), MessageType.Warning);
			}

			if (onRepair != null && GUILayout.Button(EditorContents.repair))
			{
				onRepair();

				serializedObject.ApplyModifiedProperties();

				GUIUtility.ExitGUI();
			}

			if (isInGraphParameter || parameterReferenceType == ParameterReferenceType.Constant)
			{
				if (parameter == null && !string.IsNullOrEmpty(parameterName) && (isInGraphParameter || _ParameterReferenceProperty.container != null))
				{
					string message = string.Format(Localization.GetWord("ParameterBehaviours.NotFoundParameter"), parameterName);
					EditorGUILayout.HelpBox(message, MessageType.Error);
				}
			}

			if (parameterReferenceType == ParameterReferenceType.Constant)
			{
				bool isCurrentInGraphParameter = IsInGraphParameter(_ParameterReferenceProperty);
				if (!isInGraphParameter && isCurrentInGraphParameter)
				{
					string message = Localization.GetWord("ParameterBehaviours.IsInGraphParameter");
					EditorGUILayout.HelpBox(message, MessageType.Info);
					if (GUILayout.Button(Localization.GetTextContent("ParameterBehaviours.SetInGraphParameterMode")))
					{
						_IsInGraphParameterProperty.boolValue = true;
					}
				}
				else if (isInGraphParameter && !isCurrentInGraphParameter)
				{
					string message = Localization.GetWord("ParameterBehaviours.IsExternalGraphParameter");
					EditorGUILayout.HelpBox(message, MessageType.Info);
					if (GUILayout.Button(Localization.GetTextContent("ParameterBehaviours.SetExternalGraphParameterMode")))
					{
						_IsInGraphParameterProperty.boolValue = false;
					}
				}
			}

#if ARBOR_DEBUG
			EditorGUILayout.LabelField("[Debug]", EditorStyles.boldLabel);
			EditorGUILayout.PropertyField(_ValueTypeProperty.property, true);
			EditorGUILayout.PropertyField(_ParameterTypeProperty, true);
			EditorGUILayout.PropertyField(_ParameterListProperty.property, true);
#endif

			serializedObject.ApplyModifiedProperties();
		}
	}
}