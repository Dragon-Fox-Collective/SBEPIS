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

	[CustomEditor(typeof(ListToArrayListCalculator))]
	internal sealed class ListToArrayListCalculatorInspector : Editor
	{
		private const string kElementTypePath = "_ElementType";
		private const string kInputPath = "_Input";
		private const string kOutputPath = "_Output";
		private const string kOutputTypePath = "_OutputType";

		private ClassTypeReferenceProperty _ElementTypeProperty;
		private InputSlotTypableProperty _InputProperty;
		private OutputSlotTypableProperty _OutputProperty;
		private SerializedProperty _OutputTypeProperty;

		public ArrayListType outputType
		{
			get
			{
				return EnumUtility.GetValueFromIndex<ArrayListType>(_OutputTypeProperty.enumValueIndex);
			}
			set
			{
				_OutputTypeProperty.enumValueIndex = EnumUtility.GetIndexFromValue(value);
			}
		}

		void OnEnable()
		{
			_ElementTypeProperty = new ClassTypeReferenceProperty(serializedObject.FindProperty(kElementTypePath));
			_InputProperty = new InputSlotTypableProperty(serializedObject.FindProperty(kInputPath));
			_OutputProperty = new OutputSlotTypableProperty(serializedObject.FindProperty(kOutputPath));
			_OutputTypeProperty = serializedObject.FindProperty(kOutputTypePath);
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
			System.Type outputSlotType = elementType != null ? GetArrayListType(elementType) : null;
			string outputTypeName = TypeUtility.TidyAssemblyTypeName(outputSlotType);

			if (_OutputProperty.typeProperty.assemblyTypeName.stringValue != outputTypeName)
			{
				_OutputProperty.Disconnect();

				_OutputProperty.type = outputSlotType;
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

					System.Type inputType = ListUtility.GetIListType(newType);
					string inputTypeName = TypeUtility.TidyAssemblyTypeName(inputType);

					if (_InputProperty.typeProperty.assemblyTypeName.stringValue != inputTypeName)
					{
						_InputProperty.Disconnect();

						_InputProperty.type = inputType;
					}

					SetOutputSlotType(newType);

					serializedObject.ApplyModifiedProperties();

					GUIUtility.ExitGUI(); // throw ExitGUIException
				}

				using (new EditorGUI.DisabledGroupScope(_OutputProperty.type == null))
				{
					EditorGUILayout.PropertyField(_OutputProperty.property, GUIContentCaches.Get(_OutputProperty.property.displayName), true, GUILayout.Width(70f));
				}
			}

			using (new EditorGUI.DisabledGroupScope(elementType == null))
			{
				EditorGUI.BeginChangeCheck();
				EditorGUILayout.PropertyField(_OutputTypeProperty);
				if (EditorGUI.EndChangeCheck())
				{
					SetOutputSlotType(_ElementTypeProperty.type);
				}
				EditorGUILayout.PropertyField(_InputProperty.property);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}