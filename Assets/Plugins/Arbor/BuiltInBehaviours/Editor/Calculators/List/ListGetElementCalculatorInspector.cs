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

	[CustomEditor(typeof(ListGetElementCalculator))]
	internal sealed class ListGetElementCalculatorInspector : Editor
	{
		private const string kElementTypePath = "_ElementType";
		private const string kInputPath = "_Input";
		private const string kIndexPath = "_Index";
		private const string kOutputPath = "_Output";

		private ClassTypeReferenceProperty _ElementTypeProperty;
		private InputSlotTypableProperty _InputProperty;
		private SerializedProperty _IndexProperty;
		private OutputSlotTypableProperty _OutputProperty;

		void OnEnable()
		{
			_ElementTypeProperty = new ClassTypeReferenceProperty(serializedObject.FindProperty(kElementTypePath));
			_InputProperty = new InputSlotTypableProperty(serializedObject.FindProperty(kInputPath));
			_IndexProperty = serializedObject.FindProperty(kIndexPath);
			_OutputProperty = new OutputSlotTypableProperty(serializedObject.FindProperty(kOutputPath));
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

					string typeName = TypeUtility.TidyAssemblyTypeName(newType);

					if (_OutputProperty.typeProperty.assemblyTypeName.stringValue != typeName)
					{
						_OutputProperty.Disconnect();

						_OutputProperty.type = newType;
					}

					serializedObject.ApplyModifiedProperties();

					GUIUtility.ExitGUI(); // throw ExitGUIException
				}

				using (new EditorGUI.DisabledGroupScope(_OutputProperty.type == null))
				{
					EditorGUILayout.PropertyField(_OutputProperty.property, GUIContentCaches.Get(_OutputProperty.property.displayName), true, GUILayout.Width(70f));
				}
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

			EditorGUILayout.PropertyField(_IndexProperty);

			serializedObject.ApplyModifiedProperties();
		}
	}
}