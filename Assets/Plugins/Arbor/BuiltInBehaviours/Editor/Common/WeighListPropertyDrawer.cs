//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace ArborEditor
{
	using Arbor;

	internal sealed class WeightListProeprtyEditor : PropertyEditor
	{
		internal sealed class Callback
		{
			public delegate void OnPreValueFieldCallbackDelegate(SerializedProperty valueProperty);

			public event OnPreValueFieldCallbackDelegate onPreValueFieldCallback;

			public void OnPreValueField(SerializedProperty valueProperty)
			{
				if (onPreValueFieldCallback != null)
				{
					onPreValueFieldCallback(valueProperty);
				}
			}
		}

		private SerializedProperty _ListProperty;
		private WeightListBase _WeightList;
		private SerializedProperty _ValuesProperty;
		private SerializedProperty _WeightsProperty;
		private int _SelectIndex;

		private float _TotalWeight;
		private bool _IsConstantAll = false;

		public ReorderableList list
		{
			get;
			private set;
		}

		protected override void OnInitialize()
		{
			_ListProperty = property;

			_WeightList = SerializedPropertyUtility.GetPropertyObject<WeightListBase>(_ListProperty);

			_ValuesProperty = property.FindPropertyRelative("_Values");
			_WeightsProperty = property.FindPropertyRelative("_Weights");

			list = new ReorderableList(property.serializedObject, _ValuesProperty)
			{
				drawHeaderCallback = DrawHeader,
				elementHeightCallback = ElementHeight,
				drawElementCallback = DrawElement,
				onAddCallback = OnAdd,
				onRemoveCallback = OnRemove,
				onSelectCallback = OnSelect,
				onReorderCallback = OnReorder,
			};
		}

		private void DrawHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, _ListProperty.displayName);
		}

		private float ElementHeight(int index)
		{
			SerializedProperty valueProperty = _ValuesProperty.GetArrayElementAtIndex(index);
			SerializedProperty weightProperty = _WeightsProperty.GetArrayElementAtIndex(index);

			float height = list.elementHeight + EditorGUIUtility.standardVerticalSpacing;

			Callback callback = _ListProperty.GetStateData<Callback>();
			if (callback != null)
			{
				callback.OnPreValueField(valueProperty);
			}

			height += EditorGUI.GetPropertyHeight(valueProperty, GUIContentCaches.Get("Value"), true) + EditorGUIUtility.standardVerticalSpacing;
			height += EditorGUI.GetPropertyHeight(weightProperty, GUIContentCaches.Get("Weight"), true) + EditorGUIUtility.standardVerticalSpacing;
			if (_IsConstantAll)
			{
				height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; // probabillity
			}

			return height;
		}

		private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			rect.height = list.elementHeight;

			EditorGUI.LabelField(rect, GUIContentCaches.Get("Element " + index));
			rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;

			SerializedProperty valueProperty = _ValuesProperty.GetArrayElementAtIndex(index);

			Callback callback = _ListProperty.GetStateData<Callback>();
			if (callback != null)
			{
				callback.OnPreValueField(valueProperty);
			}

			GUIContent valueContent = GUIContentCaches.Get("Value");
			rect.height = EditorGUI.GetPropertyHeight(valueProperty, valueContent, true);

			EditorGUI.PropertyField(rect, valueProperty, valueContent);
			rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;

			SerializedProperty weightProperty = _WeightsProperty.GetArrayElementAtIndex(index);

			GUIContent weightContent = GUIContentCaches.Get("Weight");
			rect.height = EditorGUI.GetPropertyHeight(weightProperty, weightContent, true);
			EditorGUI.PropertyField(rect, weightProperty, weightContent);
			rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;

			if (_IsConstantAll)
			{
				FlexibleNumericProperty weightFlexibleProperty = new FlexibleNumericProperty(weightProperty);
				if (weightFlexibleProperty.type == FlexiblePrimitiveType.Constant)
				{
					float weight = weightFlexibleProperty.valueProperty.floatValue;

					float probability = (_TotalWeight != 0.0f) ? weight / _TotalWeight : 0.0f;
					EditorGUI.LabelField(rect, "Probability", string.Format("{0:P1}", probability));
					rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
				}
				else
				{
					EditorGUI.LabelField(rect, "Probability", "?");
					rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
				}
			}
		}

		private void OnSelect(ReorderableList list)
		{
			_SelectIndex = list.index;
		}

		private void OnReorder(ReorderableList list)
		{
			_WeightsProperty.MoveArrayElement(_SelectIndex, list.index);
		}

		private void OnAdd(ReorderableList list)
		{
			_ValuesProperty.arraySize++;
			_WeightsProperty.arraySize = _ValuesProperty.arraySize;

			list.index = _ValuesProperty.arraySize - 1;
		}

		private void OnRemove(ReorderableList list)
		{
			int index = list.index;
			SerializedProperty valueProperty = _ValuesProperty.GetArrayElementAtIndex(index);
			if (valueProperty.propertyType == SerializedPropertyType.ObjectReference)
			{
				valueProperty.objectReferenceValue = null;
			}
			_ValuesProperty.DeleteArrayElementAtIndex(index);

			_WeightsProperty.DeleteArrayElementAtIndex(index);
		}

		void Setup()
		{
			_IsConstantAll = true;

			for (int i = 0; i < _ValuesProperty.arraySize; i++)
			{
				FlexibleNumericProperty wp = new FlexibleNumericProperty(_WeightsProperty.GetArrayElementAtIndex(i));
				if (wp.type != FlexiblePrimitiveType.Constant)
				{
					_IsConstantAll = false;
					break;
				}
			}

			if (_IsConstantAll)
			{
				_TotalWeight = _WeightList.GetTotalWeight();
			}
			else
			{
				_TotalWeight = 0.0f;
			}
		}

		protected override void OnGUI(Rect position, GUIContent label)
		{
			Setup();

			list.DoList(position);
		}

		protected override float GetHeight(GUIContent label)
		{
			Setup();

			return list.GetHeight();
		}
	}

	[CustomPropertyDrawer(typeof(WeightListBase), true)]
	internal sealed class WeightListPropertyDrawer : PropertyEditorDrawer<WeightListProeprtyEditor>
	{
	}
}