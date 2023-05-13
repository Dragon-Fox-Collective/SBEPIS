//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ArborEditor
{
	using Arbor;

	public sealed class LayoutOption
	{
		internal enum Type
		{
			FixedWidth,
		};

		internal Type type
		{
			get;
			private set;
		}

		internal float value
		{
			get;
			private set;
		}

		internal LayoutOption(Type type, float value)
		{
			this.type = type;
			this.value = value;
		}
	}

	public sealed class LayoutArea
	{
		private sealed class LayoutGroup
		{
			public bool isVertical
			{
				get;
				private set;
			}

			private Vector2 _Position;

			private Rect _Rect = new Rect();
			public Rect rect
			{
				get
				{
					return _Rect;
				}
			}

			private Rect _BeginRect = new Rect();

			public Rect nextRect
			{
				get
				{
					if (isVertical)
					{
						return new Rect(_Rect.x, _Rect.yMax, _BeginRect.width, 0f);
					}
					else
					{
						return new Rect(_Rect.xMax, _Rect.y, _BeginRect.xMax - _Rect.xMax, _BeginRect.height);
					}
				}
			}

			public readonly RectOffset currentMargin;

			public LayoutGroup()
			{
				currentMargin = new RectOffset();
			}

			public void Set(Rect rect, RectOffset margin, bool isVertical)
			{
				_BeginRect = rect;

				if (isVertical)
				{
					rect.height = 0f;
				}
				else
				{
					rect.width = 0f;
				}

				_Rect = rect;

				currentMargin.left = margin.left;
				currentMargin.right = margin.right;
				currentMargin.top = margin.top;
				currentMargin.bottom = margin.bottom;

				_Position = rect.position;

				this.isVertical = isVertical;
			}

			public void GetSize(ref float width, ref float height, params LayoutOption[] options)
			{
				if (width == 0.0f)
				{
					width = _BeginRect.xMax - _Position.x - currentMargin.horizontal;
				}

				for (int optionIndex = 0; optionIndex < options.Length; optionIndex++)
				{
					LayoutOption option = options[optionIndex];
					switch (option.type)
					{
						case LayoutOption.Type.FixedWidth:
							width = option.value;
							break;
					}
				}

				width += currentMargin.horizontal;
				height += currentMargin.vertical;
			}

			public Rect GetRect(float width, float height, params LayoutOption[] options)
			{
				GetSize(ref width, ref height, options);

				Rect rect = new Rect(_Position.x, _Position.y, width, height);
				Rect guiRect = currentMargin.Remove(rect);

				Next(width, height);

				return guiRect;
			}

			public void Next(float width, float height)
			{
				if (isVertical)
				{
					_Rect.yMax += height;
					_Position.y = _Rect.yMax;

					_Rect.xMax = Mathf.Max(_Position.x + width, _Rect.xMax);

					currentMargin.top = 0;
				}
				else
				{
					_Rect.xMax += width;
					_Position.x = _Rect.xMax;

					_Rect.yMax = Mathf.Max(_Position.y + height, _Rect.yMax);

					currentMargin.left = 0;
				}
			}
		}

		private static class Defaults
		{
			public static readonly RectOffset margin = new RectOffset(4, 4, 2, 2);
		}

		public static LayoutOption Width(float width)
		{
			return new LayoutOption(LayoutOption.Type.FixedWidth, width);
		}

		internal const int kDefaultSpacing = 6;

		private Stack<LayoutGroup> _LayoutGroups = new Stack<LayoutGroup>();
		private PropertyHeightCache _PropertyHeight = new PropertyHeightCache();

		public Rect rect
		{
			get;
			private set;
		}

		public bool isLayout
		{
			get;
			private set;
		}

		public Rect lastRect
		{
			get;
			private set;
		}

		private static Arbor.Pool.ObjectPool<LayoutGroup> s_GroupPool = new Arbor.Pool.ObjectPool<LayoutGroup>(
			()=> new LayoutGroup(),
			null,
			null,
			null,
			true, 10, 10000);

		public void Begin(Rect rect, bool isLayout, RectOffset margin)
		{
			if (isLayout)
			{
				_PropertyHeight.Clear();
			}

			this.isLayout = isLayout;

			this.rect = rect;

			LayoutGroup layoutGroup = s_GroupPool.Get();
			layoutGroup.Set(rect, margin, true);

			_LayoutGroups.Push(layoutGroup);
		}

		public void Begin(Rect rect, bool isLayout)
		{
			Begin(rect, isLayout, Defaults.margin);
		}

		Rect PopGroup()
		{
			LayoutGroup layoutGroup = _LayoutGroups.Pop();

			Rect rect = layoutGroup.rect;

			s_GroupPool.Release(layoutGroup);

			return rect;
		}

		public void End()
		{
			Rect groupRect = PopGroup();

			if (isLayout)
			{
				rect = groupRect;
			}
		}

		public void BeginVertical(params LayoutOption[] options)
		{
			LayoutGroup layoutGroup = _LayoutGroups.Peek();

			Rect nextRect = layoutGroup.nextRect;

			float width = nextRect.width;
			float height = nextRect.height;
			layoutGroup.GetSize(ref width, ref height, options);
			nextRect.width = width;
			nextRect.height = height;

			LayoutGroup verticalLayoutGroup = s_GroupPool.Get();
			verticalLayoutGroup.Set(nextRect, layoutGroup.currentMargin, true);

			_LayoutGroups.Push(verticalLayoutGroup);
		}

		public void EndVertical()
		{
			Rect rect = PopGroup();

			LayoutGroup layoutGroup = _LayoutGroups.Peek();

			layoutGroup.Next(rect.width, rect.height);
		}

		public void BeginHorizontal(params LayoutOption[] options)
		{
			LayoutGroup layoutGroup = _LayoutGroups.Peek();

			Rect nextRect = layoutGroup.nextRect;

			float width = nextRect.width;
			float height = nextRect.height;
			layoutGroup.GetSize(ref width, ref height, options);
			nextRect.width = width;
			nextRect.height = height;

			LayoutGroup horizontalLayoutGroup = s_GroupPool.Get();
			horizontalLayoutGroup.Set(nextRect, layoutGroup.currentMargin, false);

			_LayoutGroups.Push(horizontalLayoutGroup);
		}

		public void EndHorizontal()
		{
			Rect rect = PopGroup();

			LayoutGroup layoutGroup = _LayoutGroups.Peek();

			layoutGroup.Next(rect.width, rect.height);
		}

		public Rect GetRect(float width, float height, params LayoutOption[] options)
		{
			LayoutGroup layoutGroup = _LayoutGroups.Peek();
			lastRect = layoutGroup.GetRect(width, height, options);

			return lastRect;
		}

		public void Space(float space)
		{
			LayoutGroup layoutGroup = _LayoutGroups.Peek();
			if (layoutGroup.isVertical)
			{
				layoutGroup.GetRect(0f, space);
			}
			else
			{
				layoutGroup.GetRect(space, 0f);
			}
		}

		public void Space()
		{
			Space(kDefaultSpacing);
		}

		public bool IsDraw()
		{
			if (isLayout)
			{
				return false;
			}

			return true;
		}

		public void PropertyField(SerializedProperty property, params LayoutOption[] options)
		{
			PropertyField(property, GUIContentCaches.Get(property.displayName), false, options);
		}

		public void PropertyField(SerializedProperty property, GUIContent label, params LayoutOption[] options)
		{
			PropertyField(property, label, false, options);
		}

		public void PropertyField(SerializedProperty property, GUIContent label, bool includeChildren, params LayoutOption[] options)
		{
			float height = _PropertyHeight.GetPropertyHeight(property, label, includeChildren);
			Rect rect = GetRect(0f, height, options);

			if (IsDraw())
			{
				EditorGUI.PropertyField(rect, property, label, includeChildren);
			}
		}

		public void LabelField(GUIContent label, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				EditorGUI.LabelField(rect, label);
			}
		}

		public void LabelField(GUIContent label, GUIContent label2, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				EditorGUI.LabelField(rect, label, label2);
			}
		}

		public string TextField(GUIContent label, string text, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				text = EditorGUI.TextField(rect, label, text);
			}

			return text;
		}

		public void TextField(GUIContent label, SerializedProperty property, params LayoutOption[] options)
		{
			if (property.propertyType != SerializedPropertyType.String)
			{
				return;
			}

			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				label = EditorGUI.BeginProperty(rect, label, property);

				EditorGUI.BeginChangeCheck();
				string text = EditorGUI.TextField(rect, label, property.stringValue);
				if (EditorGUI.EndChangeCheck())
				{
					property.stringValue = text;
				}

				EditorGUI.EndProperty();
			}
		}

		public string TextField(string label, string text, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				text = EditorGUI.TextField(rect, label, text);
			}

			return text;
		}

		public bool Foldout(bool foldout, GUIContent label, bool toggleOnLabelClick, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				foldout = EditorGUI.Foldout(rect, foldout, label, toggleOnLabelClick);
			}

			return foldout;
		}

		public bool VisibilityToggle(GUIContent label, bool toggle, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				toggle = EditorGUITools.VisibilityToggle(rect, label, toggle);
			}

			return toggle;
		}

		public void VisibilityToggle(GUIContent label, SerializedProperty property, params LayoutOption[] options)
		{
			if (property.propertyType != SerializedPropertyType.Boolean)
			{
				return;
			}

			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				label = EditorGUI.BeginProperty(rect, label, property);

				EditorGUI.BeginChangeCheck();
				bool toggle = EditorGUITools.VisibilityToggle(rect, label, property.boolValue);
				if (EditorGUI.EndChangeCheck())
				{
					property.boolValue = toggle;
				}

				EditorGUI.EndProperty();
			}
		}

		public bool Button(GUIContent content, GUIStyle style, params LayoutOption[] options)
		{
			bool button = false;

			float height = style.CalcHeight(content, UnityEditorBridge.EditorGUIUtilityBridge.contextWidth);
			Rect rect = GetRect(0f, height, options);

			if (IsDraw())
			{
				button = GUI.Button(rect, content, style);
			}

			return button;
		}

		public bool Button(GUIContent content, params LayoutOption[] options)
		{
			return Button(content, GUI.skin.button, options);
		}

		public bool ButtonMouseDown(GUIContent content, FocusType focusType, GUIStyle style, params LayoutOption[] options)
		{
			bool button = false;

			LayoutGroup layoutGroup = _LayoutGroups.Peek();

			float height = style.CalcHeight(content, layoutGroup.rect.width);
			Rect rect = GetRect(0f, height, options);

			if (IsDraw())
			{
				button = EditorGUI.DropdownButton(rect, content, focusType, style);
			}

			return button;
		}

		public System.Enum EnumPopup(GUIContent label, System.Enum selected, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				selected = EditorGUI.EnumPopup(rect, label, selected);
			}

			return selected;
		}

		public System.Enum EnumPopup(string label, System.Enum selected, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				selected = EditorGUI.EnumPopup(rect, label, selected);
			}

			return selected;
		}

		public System.Enum EnumMaskField(GUIContent label, System.Enum selected, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				selected = EditorGUI.EnumFlagsField(rect, label, selected);
			}

			return selected;
		}

		public Color ColorField(GUIContent label, Color value, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				value = EditorGUI.ColorField(rect, label, value);
			}

			return value;
		}

		public Color ColorField(string label, Color value, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				value = EditorGUI.ColorField(rect, label, value);
			}

			return value;
		}

		public Color ColorField(GUIContent label, Color value, bool showEyedropper, bool showAlpha, bool hdr, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				value = EditorGUI.ColorField(rect, label, value, showEyedropper, showAlpha, hdr);
			}

			return value;
		}

		public Object ObjectField(GUIContent label, Object obj, System.Type objType, bool allowSceneObjects, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				obj = EditorGUI.ObjectField(rect, label, obj, objType, allowSceneObjects);
			}

			return obj;
		}

		public void ObjectField(GUIContent label, SerializedProperty property, System.Type objType, bool allowSceneObjects, params LayoutOption[] options)
		{
			if (property.propertyType != SerializedPropertyType.ObjectReference)
			{
				return;
			}

			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				label = EditorGUI.BeginProperty(rect, label, property);

				EditorGUI.BeginChangeCheck();
				Object obj = EditorGUI.ObjectField(rect, label, property.objectReferenceValue, objType, allowSceneObjects);
				if (EditorGUI.EndChangeCheck())
				{
					property.objectReferenceValue = obj;
				}

				EditorGUI.EndProperty();
			}
		}

		public void HelpBox(string message, MessageType messageType, params LayoutOption[] options)
		{
			LayoutGroup layoutGroup = _LayoutGroups.Peek();
			float height = EditorGUITools.GetHelpBoxHeight(message, messageType, layoutGroup.rect.width);
			Rect rect = GetRect(0f, height, options);

			if (IsDraw())
			{
				EditorGUI.HelpBox(rect, message, messageType);
			}
		}

		public void LanguagePopup(GUIContent label, GUIStyle style, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				LanguageSelector.LanguagePopup(rect, label, style);
			}
		}

		public float Slider(GUIContent label, float value, float leftValue, float rightValue, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				value = EditorGUI.Slider(rect, label, value, leftValue, rightValue);
			}
			return value;
		}

		public bool Toggle(GUIContent label, bool value, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				value = EditorGUI.Toggle(rect, label, value);
			}
			return value;
		}

		public int IntSlider(GUIContent label, int value, int leftValue, int rightValue, params LayoutOption[] options)
		{
			Rect rect = GetRect(0f, EditorGUIUtility.singleLineHeight, options);

			if (IsDraw())
			{
				value = EditorGUI.IntSlider(rect, label, value, leftValue, rightValue);
			}
			return value;
		}
	}
}