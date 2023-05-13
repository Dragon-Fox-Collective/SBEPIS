//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace ArborEditor.ObjectPooling
{
	using Arbor.ObjectPooling;

	internal sealed class PoolingItemProperty
	{
		private const string kTypePath = "_Type";
		private const string kOriginalPath = "original";
		private const string kAmountPath = "amount";
		private const string kLifeTimeFlagsPath = "lifeTimeFlags";
		private const string kLifeDurationPath = "lifeDuration";

		public SerializedProperty property
		{
			get;
			private set;
		}

		public ClassTypeReferenceProperty type
		{
			get;
			private set;
		}

		public SerializedProperty original
		{
			get;
			private set;
		}

		public SerializedProperty amount
		{
			get;
			private set;
		}

		public SerializedProperty lifeTimeFlags
		{
			get;
			private set;
		}

		public SerializedProperty lifeDuration
		{
			get;
			private set;
		}

		public PoolingItemProperty(SerializedProperty property)
		{
			this.property = property;

			type = new ClassTypeReferenceProperty(property.FindPropertyRelative(kTypePath));
			original = property.FindPropertyRelative(kOriginalPath);
			amount = property.FindPropertyRelative(kAmountPath);
			lifeTimeFlags = property.FindPropertyRelative(kLifeTimeFlagsPath);
			lifeDuration = property.FindPropertyRelative(kLifeDurationPath);
		}
	}

	internal sealed class PoolingItemListGUI : PropertyEditor
	{
		private const float kSpacing = 5;
		private const int kExtraSpacing = 6;

		private ReorderableList _ItemsList;

		private GUIContent _Label;

		private PropertyHeightCache _PropertyHeights = new PropertyHeightCache();
		private LayoutArea _LayoutArea = new LayoutArea();

		protected override void OnInitialize()
		{
			SerializedProperty itemsProperty = property.FindPropertyRelative("_Items");
			_ItemsList = new ReorderableList(itemsProperty.serializedObject, itemsProperty)
			{
				drawHeaderCallback = DrawHeader,
				drawElementCallback = DrawElement,
				elementHeightCallback = GetElementHeight,
			};
		}

		protected override void OnGUI(Rect position, GUIContent label)
		{
			_Label = new GUIContent(label);

			if (Event.current.type == EventType.Layout)
			{
				_PropertyHeights.Clear();
			}

			if (_ItemsList != null)
			{
				var oldIndentLevel = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
				_ItemsList.DoList(position);
				EditorGUI.indentLevel = oldIndentLevel;
			}
		}

		protected override float GetHeight(GUIContent label)
		{
			float height = 0f;
			if (_ItemsList != null)
			{
				height = _ItemsList.GetHeight();
			}
			return height;
		}

		void DrawHeader(Rect headerRect)
		{
			headerRect.height = EditorGUIUtility.singleLineHeight;

			GUI.Label(headerRect, _Label);
		}

		void DoElementGUI(SerializedProperty property)
		{
			PoolingItemProperty itemProperty = new PoolingItemProperty(property);

			float verticalSpacing = Mathf.Floor((_ItemsList.elementHeight - EditorGUIUtility.singleLineHeight) * 0.5f) - 2f;
			_LayoutArea.Space(verticalSpacing);

			_LayoutArea.BeginHorizontal();

			_LayoutArea.PropertyField(itemProperty.type.property, GUIContent.none, LayoutArea.Width(EditorGUIUtility.labelWidth - kSpacing));

			_LayoutArea.Space(kSpacing);

			System.Type type = itemProperty.type.type ?? typeof(Object);
			_LayoutArea.ObjectField(GUIContent.none, itemProperty.original, type, true);

			_LayoutArea.EndHorizontal();

			_LayoutArea.Space(verticalSpacing);

			_LayoutArea.PropertyField(itemProperty.amount);

			_LayoutArea.PropertyField(itemProperty.lifeTimeFlags);
			_LayoutArea.PropertyField(itemProperty.lifeDuration);

			_LayoutArea.Space(kExtraSpacing);
		}

		static readonly RectOffset s_LayoutMargin = new RectOffset(0, 0, 0, 2);

		void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty property = _ItemsList.serializedProperty.GetArrayElementAtIndex(index);

			_LayoutArea.Begin(rect, false, s_LayoutMargin);

			DoElementGUI(property);

			_LayoutArea.End();
		}

		float GetElementHeight(int index)
		{
			SerializedProperty property = _ItemsList.serializedProperty.GetArrayElementAtIndex(index);

			float height = 0f;
			if (!_PropertyHeights.TryGetHeight(property, out height))
			{
				_LayoutArea.Begin(new Rect(), true, s_LayoutMargin);

				DoElementGUI(property);

				_LayoutArea.End();

				height = _LayoutArea.rect.height;

				_PropertyHeights.AddHeight(property, height);
			}

			return height;
		}
	}

	[CustomPropertyDrawer(typeof(PoolingItemList))]
	internal sealed class PoolingItemListPropertyDrawer : PropertyEditorDrawer<PoolingItemListGUI>
	{
	}
}