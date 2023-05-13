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

	[CustomEditor(typeof(GetParameterCalculatorInternal), true)]
	internal sealed class GetParameterCalculatorInternalInspector : NodeBehaviourEditor, ICalculatorBehaviourEditor
	{
		private ParameterReferenceProperty _ParameterReferenceProperty = null;
		private ParameterReferenceEditorGUI _ParameterReferenceEditorGUI = null;
		private OutputSlotTypableProperty _OutputProperty;
		private SerializedProperty _IsInGraphParameterProperty;

		private SerializedProperty isInGraphParameterProperty
		{
			get
			{
				if (_IsInGraphParameterProperty == null)
				{
					_IsInGraphParameterProperty = serializedObject.FindProperty("_IsInGraphParameter");
				}

				return _IsInGraphParameterProperty;
			}
		}

		private void OnEnable()
		{
			_OutputProperty = new OutputSlotTypableProperty(serializedObject.FindProperty("_Output"));
			_ParameterReferenceProperty = new ParameterReferenceProperty(serializedObject.FindProperty("_ParameterReference"));
			_ParameterReferenceEditorGUI = new ParameterReferenceEditorGUI(_ParameterReferenceProperty);
			_IsInGraphParameterProperty = serializedObject.FindProperty("_IsInGraphParameter");
		}

		bool IsInGraphParameter(ParameterReferenceProperty parameterReferenceProperty)
		{
			ParameterContainerBase parameterContainer = (parameterReferenceProperty.type == ParameterReferenceType.Constant) ? parameterReferenceProperty.container : null;
			return parameterContainer != null && graphEditor != null && graphEditor.nodeGraph.parameterContainer != null && parameterContainer == graphEditor.nodeGraph.parameterContainer && parameterReferenceProperty.GetParameter() != null;
		}

		bool IsInGraphParameter()
		{
			return isInGraphParameterProperty.boolValue;
		}

		public bool IsResizableNode()
		{
			serializedObject.Update();

			if (IsInGraphParameter())
			{
				return false;
			}

			return true;
		}

		public float GetNodeWidth()
		{
			serializedObject.Update();

			if (IsInGraphParameter())
			{
				return 200f;
			}

			return Node.defaultWidth;
		}

		void RepairSlotType(System.Type valueType)
		{
			if (!valueType.IsAssignableFrom(_OutputProperty.type))
			{
				_OutputProperty.Disconnect();
			}

			_OutputProperty.type = valueType;
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			bool isInGraphParameter = IsInGraphParameter();

			if (!isInGraphParameter)
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(_ParameterReferenceProperty.property);
				if (EditorGUI.EndChangeCheck())
				{
					Parameter parameter = _ParameterReferenceProperty.GetParameter();
					if (parameter != null)
					{
						System.Type valueType = parameter.valueType;
						if (_OutputProperty.type != valueType)
						{
							RepairSlotType(valueType);
						}
					}
				}
			}

			string parameterName = _ParameterReferenceProperty.GetParameterName();

			if (isInGraphParameter)
			{
				Parameter parameter = _ParameterReferenceProperty.GetParameter();

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
						System.Type valueType = parameter.valueType;
						if (_OutputProperty.type != valueType)
						{
							RepairSlotType(valueType);
						}
					}

					if (_ParameterReferenceProperty.type == ParameterReferenceType.Constant && !IsInGraphParameter(_ParameterReferenceProperty))
					{
						isInGraphParameterProperty.boolValue = false;

						CalculatorEditor calculatorEditor = nodeEditor as CalculatorEditor;
						if (calculatorEditor != null)
						{
							calculatorEditor.isResizable = true;
						}
					}
				}
				EditorGUILayout.PropertyField(_OutputProperty.property, GUIContentCaches.Get(parameterName));
			}
			else
			{
				if (_ParameterReferenceProperty.type == ParameterReferenceType.DataSlot)
				{
					TypeFilterFlags filterFlags = (TypeFilterFlags)(-1) & ~(TypeFilterFlags.Static);
					_OutputProperty.typeProperty.SetTypeFilter(new TypeFilterAttribute(filterFlags));
					EditorGUILayout.PropertyField(_OutputProperty.typeProperty.property);
				}
				EditorGUILayout.PropertyField(_OutputProperty.property);
			}

			if (isInGraphParameter || _ParameterReferenceProperty.type == ParameterReferenceType.Constant)
			{
				bool isCurrentInGraphParameter = IsInGraphParameter(_ParameterReferenceProperty);
				if (!isInGraphParameter && isCurrentInGraphParameter)
				{
					string message = Localization.GetWord("ParameterBehaviours.IsInGraphParameter");
					EditorGUILayout.HelpBox(message, MessageType.Info);
					if (GUILayout.Button(Localization.GetTextContent("ParameterBehaviours.SetInGraphParameterMode")))
					{
						isInGraphParameterProperty.boolValue = true;

						CalculatorEditor calculatorEditor = nodeEditor as CalculatorEditor;
						if (calculatorEditor != null)
						{
							calculatorEditor.isResizable = false;
						}
					}
				}
				else if (isInGraphParameter && !isCurrentInGraphParameter)
				{
					string message = Localization.GetWord("ParameterBehaviours.IsExternalGraphParameter");
					EditorGUILayout.HelpBox(message, MessageType.Info);
					if (GUILayout.Button(Localization.GetTextContent("ParameterBehaviours.SetExternalGraphParameterMode")))
					{
						isInGraphParameterProperty.boolValue = false;

						CalculatorEditor calculatorEditor = nodeEditor as CalculatorEditor;
						if (calculatorEditor != null)
						{
							calculatorEditor.isResizable = true;
						}
					}
				}

				Parameter parameter = _ParameterReferenceProperty.GetParameter();
				if (parameter != null)
				{
					System.Type valueType = parameter.valueType;

					if (_OutputProperty.type != valueType)
					{
						string message = string.Format(Localization.GetWord("ParameterBehaviours.InvalidSlotType"), TypeUtility.GetTypeName(valueType));
						EditorGUILayout.HelpBox(message, MessageType.Warning);
						if (GUILayout.Button(EditorContents.repair))
						{
							RepairSlotType(valueType);
						}
					}
				}
				else if ((isInGraphParameter || _ParameterReferenceProperty.container != null) && !string.IsNullOrEmpty(parameterName))
				{
					string message = string.Format(Localization.GetWord("ParameterBehaviours.NotFoundParameter"), parameterName);
					EditorGUILayout.HelpBox(message, MessageType.Error);
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

		public override void OnPresetApplied()
		{
			serializedObject.Update();

			bool isInGraphParameter = isInGraphParameterProperty.boolValue;
			bool newIsInGraphParameter = IsInGraphParameter(_ParameterReferenceProperty);

			if (isInGraphParameter != newIsInGraphParameter)
			{
				isInGraphParameterProperty.boolValue = newIsInGraphParameter;
			}

			CalculatorEditor calculatorEditor = nodeEditor as CalculatorEditor;
			if (calculatorEditor != null)
			{
				calculatorEditor.isResizable = !newIsInGraphParameter;
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}