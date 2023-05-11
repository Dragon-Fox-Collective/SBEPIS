//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

using Arbor.StateMachine.StateBehaviours;

namespace ArborEditor.StateMachine.StateBehaviours
{
	[CustomEditor(typeof(RandomTransition))]
	internal sealed class RandomTransitionInspector : Editor
	{
		private RandomTransition _RandomTransition;
		private SerializedProperty _LinksProperty;
		private ListGUI _LinksList;

		private bool _IsConstantAll;
		private float _TotalWeight;

		private void OnEnable()
		{
			_RandomTransition = target as RandomTransition;

			_LinksProperty = serializedObject.FindProperty("_Links");

			_LinksList = new ListGUI(_LinksProperty);

			_LinksList.drawChild = OnDrawChild;
		}

		void OnDrawChild(SerializedProperty property)
		{
			EditorGUILayout.LabelField(property.displayName);

			FlexibleNumericProperty weightProperty = new FlexibleNumericProperty(property.FindPropertyRelative("_Weight"));

			EditorGUILayout.PropertyField(weightProperty.property);

			if (_IsConstantAll && weightProperty.type == Arbor.FlexiblePrimitiveType.Constant)
			{
				float r = (_TotalWeight != 0.0f) ? weightProperty.valueProperty.floatValue / _TotalWeight : 0.0f;

				EditorGUILayout.LabelField("Probability", string.Format("{0:P1}", r));
			}
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			_IsConstantAll = true;
			for (int i = 0; i < _LinksProperty.arraySize; i++)
			{
				SerializedProperty linkProperty = _LinksProperty.GetArrayElementAtIndex(i);

				FlexibleNumericProperty weightProperty = new FlexibleNumericProperty(linkProperty.FindPropertyRelative("_Weight"));
				if (weightProperty.type != Arbor.FlexiblePrimitiveType.Constant)
				{
					_IsConstantAll = false;
					break;
				}
			}

			if (_IsConstantAll)
			{
				_TotalWeight = _RandomTransition.GetTotalWeight();
			}

			_LinksList.OnGUI();

			serializedObject.ApplyModifiedProperties();
		}
	}
}
