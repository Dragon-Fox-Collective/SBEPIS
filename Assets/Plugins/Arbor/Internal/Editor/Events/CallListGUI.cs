//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

namespace ArborEditor.Events
{
	internal sealed class CallListGUI : PropertyEditor
	{
		private const int kExtraSpacing = 6;

		private ReorderableListEx _ReorderableList;
		public int lastSelectedIndex;

		private GUIContent _Label;
		private SerializedProperty _CallsProperty;
		private int _LastSelectedIndex;

		private PropertyHeightCache _PropertyHeightCache = new PropertyHeightCache();
		private Dictionary<SerializedPropertyKey, PersistentCallProperty> _CallProperties = new Dictionary<SerializedPropertyKey, PersistentCallProperty>();

		protected override void OnInitialize()
		{
			SerializedProperty callsProperty = property.FindPropertyRelative("_Calls");
			_ReorderableList = new ReorderableListEx(property.serializedObject, callsProperty)
			{
				drawHeaderCallback = DrawEventHeader,
				drawElementCallback = DrawEventListener,
				onSelectCallback = SelectEventListener,
				onReorderCallback = EndDragChild,
				onAddCallback = AddEventListener,
				onRemoveCallback = RemoveButton,
				elementHeightCallback = ElementHeightListener,
			};
		}

		protected override void OnGUI(Rect position, GUIContent label)
		{
			if (_Label == null)
			{
				_Label = new GUIContent(label);
			}
			else
			{
				_Label.text = label.text;
				_Label.image = label.image;
				_Label.tooltip = label.tooltip;
			}

			Restore();

			if (_CallsProperty == null || !_CallsProperty.isArray)
				return;

			if (Event.current.type == EventType.Layout)
			{
				ClearCache();
			}

			if (_ReorderableList != null)
			{
				var oldIndentLevel = EditorGUI.indentLevel;
				EditorGUI.indentLevel = 0;
				_ReorderableList.DoList(position);
				EditorGUI.indentLevel = oldIndentLevel;
			}

			lastSelectedIndex = _LastSelectedIndex;
		}

		protected override float GetHeight(GUIContent label)
		{
			Restore();

			if (Event.current.type == EventType.Layout)
			{
				ClearCache();
			}

			float height = 0f;
			if (_ReorderableList != null)
			{
				height = _ReorderableList.GetHeight();
			}

			return height;
		}

		void Restore()
		{
			_CallsProperty = _ReorderableList.serializedProperty;
			_LastSelectedIndex = lastSelectedIndex;
			_ReorderableList.index = _LastSelectedIndex;
		}

		void ClearCache()
		{
			_CallProperties.Clear();
			_PropertyHeightCache.Clear();
		}

		void DrawEventHeader(Rect headerRect)
		{
			headerRect.height = EditorGUIUtility.singleLineHeight;

			GUI.Label(headerRect, _Label);
		}

		PersistentCallProperty GetProperty(SerializedProperty callProperty)
		{
			SerializedPropertyKey key = new SerializedPropertyKey(callProperty);

			PersistentCallProperty call = null;
			if (_CallProperties.TryGetValue(key, out call))
			{
				if (SerializedPropertyUtility.EqualContents(callProperty, call.property))
				{
					return call;
				}

				_CallProperties.Remove(key);
			}

			call = new PersistentCallProperty(callProperty.Copy());
			_CallProperties.Add(key, call);

			return call;
		}

		float ElementHeightListener(int index)
		{
			SerializedProperty callProperty = _CallsProperty.GetArrayElementAtIndex(index);

			float height = 0f;
			if (!_PropertyHeightCache.TryGetHeight(callProperty, out height))
			{
				height = EditorGUI.GetPropertyHeight(callProperty, GUIContent.none, true) + Mathf.Floor((_ReorderableList.elementHeight - EditorGUIUtility.singleLineHeight) * 0.5f) + kExtraSpacing;

				_PropertyHeightCache.AddHeight(callProperty, height);
			}

			return height;
		}

		void DrawEventListener(Rect rect, int index, bool isactive, bool isfocused)
		{
			SerializedProperty callProperty = _CallsProperty.GetArrayElementAtIndex(index);

			rect.y += Mathf.Floor((_ReorderableList.elementHeight - EditorGUIUtility.singleLineHeight) * 0.5f);

			EditorGUI.PropertyField(rect, callProperty, GUIContent.none, true);
		}

		void RemoveButton(ReorderableList list)
		{
			ReorderableList.defaultBehaviours.DoRemoveButton(list);
			_LastSelectedIndex = list.index;

			ClearCache();
		}

		private void AddEventListener(ReorderableList list)
		{
			ClearCache();

			if (_CallsProperty.hasMultipleDifferentValues)
			{
				//When increasing a multi-selection array using Serialized Property
				//Data can be overwritten if there is mixed values.
				//The Serialization system applies the Serialized data of one object, to all other objects in the selection.
				//We handle this case here, by creating a SerializedObject for each object.
				//Case 639025.
				var targetObjects = _CallsProperty.serializedObject.targetObjects;
				for (int targetObjectIndex = 0; targetObjectIndex < targetObjects.Length; targetObjectIndex++)
				{
					Object targetObject = targetObjects[targetObjectIndex];
					var temSerialziedObject = new SerializedObject(targetObject);
					var listenerArrayProperty = temSerialziedObject.FindProperty(_CallsProperty.propertyPath);
					listenerArrayProperty.arraySize += 1;
					temSerialziedObject.ApplyModifiedProperties();
				}
				_CallsProperty.serializedObject.SetIsDifferentCacheDirty();
				_CallsProperty.serializedObject.Update();
				list.index = list.serializedProperty.arraySize - 1;
			}
			else
			{
				ReorderableList.defaultBehaviours.DoAddButton(list);
			}

			_LastSelectedIndex = list.index;
			SerializedProperty callProperty = _CallsProperty.GetArrayElementAtIndex(list.index);

			PersistentCallProperty call = GetProperty(callProperty);

			call.Clear();
		}

		void SelectEventListener(ReorderableList list)
		{
			_LastSelectedIndex = list.index;
		}

		void EndDragChild(ReorderableList list)
		{
			_LastSelectedIndex = list.index;

			ClearCache();
		}
	}
}