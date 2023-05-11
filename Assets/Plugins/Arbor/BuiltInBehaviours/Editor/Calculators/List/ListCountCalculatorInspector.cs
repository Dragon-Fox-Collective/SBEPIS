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

	[CustomEditor(typeof(ListCountCalculator))]
	internal sealed class ListCountCalculatorInspector : Editor
	{
		private const string kElementTypePath = "_ElementType";
		private const string kInputPath = "_Input";
		private const string kOutputPath = "_Output";

		private ClassTypeReferenceProperty _ElementTypeProperty;
		private InputSlotTypableProperty _InputProperty;
		private OutputSlotBaseProperty _OutputProperty;

		void OnEnable()
		{
			_ElementTypeProperty = new ClassTypeReferenceProperty(serializedObject.FindProperty(kElementTypePath));
			_InputProperty = new InputSlotTypableProperty(serializedObject.FindProperty(kInputPath));
			_OutputProperty = new OutputSlotBaseProperty(serializedObject.FindProperty(kOutputPath));
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

					System.Type listType = ListUtility.GetIListType(newType);
					string listTypeName = TypeUtility.TidyAssemblyTypeName(listType);

					if (_InputProperty.typeProperty.assemblyTypeName.stringValue != listTypeName)
					{
						_InputProperty.Disconnect();

						_InputProperty.type = listType;
					}

					elementType = newType;
				}

				EditorGUILayout.PropertyField(_OutputProperty.property, true, GUILayout.Width(70f));
			}

			bool disableInputField = false;

			bool hasAssemblyTypeName = !string.IsNullOrEmpty(_ElementTypeProperty.assemblyTypeName.stringValue);

			if (elementType == null)
			{
				if (hasAssemblyTypeName)
				{
					EditorGUILayout.HelpBox(Localization.GetWord("Array.Message.MissingType"), MessageType.Error);
				}
				disableInputField = true;
			}

			using (new EditorGUI.DisabledGroupScope(disableInputField))
			{
				EditorGUILayout.PropertyField(_InputProperty.property);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}