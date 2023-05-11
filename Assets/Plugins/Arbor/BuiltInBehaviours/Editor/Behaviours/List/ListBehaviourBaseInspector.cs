//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.StateMachine.StateBehaviours
{
	using Arbor;

	internal class ListBehaviourBaseInspector : Editor
	{
		private const string kElementTypePath = "_ElementType";
		private const string kInputPath = "_Input";
		private const string kOutputPath = "_Output";
		private const string kOutputTypePath = "_OutputType";

		private InputSlotTypableProperty _InputProperty;
		private OutputSlotTypableProperty _OutputProperty;
		private SerializedProperty _OutputTypeProperty;

		protected ClassTypeReferenceProperty elementTypeProperty
		{
			get;
			private set;
		}

		protected virtual void OnEnable()
		{
			elementTypeProperty = new ClassTypeReferenceProperty(serializedObject.FindProperty(kElementTypePath));
			_InputProperty = new InputSlotTypableProperty(serializedObject.FindProperty(kInputPath));
			_OutputProperty = new OutputSlotTypableProperty(serializedObject.FindProperty(kOutputPath));
			_OutputTypeProperty = serializedObject.FindProperty(kOutputTypePath);
		}

		protected virtual void OnChangeElementType(System.Type currentType, System.Type newType)
		{
		}

		protected virtual void OnGUI()
		{
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			System.Type elementType = elementTypeProperty.type;

			using (new EditorGUILayout.HorizontalScope())
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(elementTypeProperty.property, GUIContent.none, true);
				if (EditorGUI.EndChangeCheck())
				{
					System.Type newType = elementTypeProperty.type;

					System.Type listType = ListUtility.GetIListType(newType);
					string listTypeName = TypeUtility.TidyAssemblyTypeName(listType);

					if (_InputProperty.typeProperty.assemblyTypeName.stringValue != listTypeName)
					{
						_InputProperty.Disconnect();

						_InputProperty.type = listType;
					}

					if (_OutputProperty.typeProperty.assemblyTypeName.stringValue != listTypeName)
					{
						_OutputProperty.Disconnect();

						_OutputProperty.type = listType;
					}

					OnChangeElementType(elementType, newType);

					serializedObject.ApplyModifiedProperties();

					GUIUtility.ExitGUI(); // throw ExitGUIException
				}

				using (new EditorGUI.DisabledGroupScope(_OutputProperty.type == null))
				{
					EditorGUILayout.PropertyField(_OutputProperty.property, GUIContentCaches.Get(_OutputProperty.property.displayName), true, GUILayout.Width(70f));
				}
			}

			bool hasAssemblyTypeName = !string.IsNullOrEmpty(elementTypeProperty.assemblyTypeName.stringValue);
			if (elementType == null && hasAssemblyTypeName)
			{
				EditorGUILayout.HelpBox(Localization.GetWord("Array.Message.MissingType"), MessageType.Error);
			}

			EditorGUILayout.PropertyField(_OutputTypeProperty, true);

			using (new EditorGUI.DisabledGroupScope(elementType == null))
			{
				EditorGUILayout.PropertyField(_InputProperty.property, true);
			}

			OnGUI();

			serializedObject.ApplyModifiedProperties();
		}
	}
}