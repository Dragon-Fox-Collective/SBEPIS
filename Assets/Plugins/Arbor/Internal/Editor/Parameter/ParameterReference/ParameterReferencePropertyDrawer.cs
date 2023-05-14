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
	using ArborEditor.UnityEditorBridge.Extensions;

	public sealed class ParameterReferenceEditorGUI
	{
		public delegate bool DelegateCheckType(Parameter parameter);

		static int GetSelectParameter(int id, ParameterContainerInternal container, List<string> names, List<int> ids, DelegateCheckType checkType)
		{
			int selected = -1;

			int parameterCount = container.parameterCount;

			if (parameterCount > 0)
			{
				for (int paramIndex = 0; paramIndex < parameterCount; paramIndex++)
				{
					Parameter parameter = container.GetParameterFromIndex(paramIndex);

					if (checkType != null && !checkType(parameter))
					{
						continue;
					}

					if (parameter.id == id)
					{
						selected = names.Count;
					}

					names.Add(parameter.name);
					ids.Add(parameter.id);
				}
			}

			return selected;
		}

		private ParameterReferenceProperty _ParameterReferenceProperty = null;

		//[System.NonSerialized]
		//private Parameter _DraggingParameter = null;

		public Parameter GetDraggingParameter()
		{
			var objectReferences = DragAndDrop.objectReferences;
			for (int objectIndex = 0; objectIndex < objectReferences.Length; objectIndex++)
			{
				ParameterDraggingObject draggedObject = objectReferences[objectIndex] as ParameterDraggingObject;
				if (draggedObject != null)
				{
					var parameter = draggedObject.parameter;
					if (parameter != null && _ParameterReferenceProperty.CheckType(parameter))
					{
						return parameter;
					}
				}
			}

			return null;
		}

		public ParameterReferenceEditorGUI(ParameterReferenceProperty parameterReferenceProperty)
		{
			_ParameterReferenceProperty = parameterReferenceProperty;
		}

		public void ParameterField(Rect position)
		{
			ParameterReferenceType parameterReferenceType = _ParameterReferenceProperty.type;
			SerializedProperty containerProperty = _ParameterReferenceProperty.containerProperty;
			switch (parameterReferenceType)
			{
				case ParameterReferenceType.Constant:
					{
						ParameterContainerBase containerBase = containerProperty.objectReferenceValue as ParameterContainerBase;
						ParameterContainerInternal container = null;
						if (containerBase != null)
						{
							container = containerBase.defaultContainer;
						}

						List<string> names = new List<string>();
						List<int> ids = new List<int>();

						int selected = -1;

						if (container != null)
						{
							selected = GetSelectParameter(_ParameterReferenceProperty.id, container, names, ids, _ParameterReferenceProperty.CheckType);
						}

						if (names.Count > 0)
						{
							selected = EditorGUI.Popup(position, "Parameter", selected, names.ToArray());

							if (selected >= 0 && ids[selected] != _ParameterReferenceProperty.id)
							{
								_ParameterReferenceProperty.id = ids[selected];
								_ParameterReferenceProperty.name = names[selected];
							}
						}
						else
						{
							EditorGUI.BeginDisabledGroup(true);

							EditorGUI.LabelField(position, "Parameter", "", EditorStyles.popup);

							EditorGUI.EndDisabledGroup();
						}
					}
					break;
				case ParameterReferenceType.DataSlot:
					{
						EditorGUI.PropertyField(position, _ParameterReferenceProperty.nameProperty, GUIContentCaches.Get("Parameter"));
					}
					break;
			}
		}

		public void ParameterFieldLayout()
		{
			Rect position = GUILayoutUtility.GetRect(0, EditorGUIUtility.singleLineHeight);
			ParameterField(position);
		}

		public float GetDropParameterHeight()
		{
			var draggingParameter = GetDraggingParameter();
			if (draggingParameter == null)
			{
				return 0f;
			}

			return Styles.dropField.CalcSize(EditorContents.dropParameter).y;
		}

		public void DropParameterLayout()
		{
			var draggingParameter = GetDraggingParameter();
			if (draggingParameter == null)
			{
				return;
			}

			GUIContent label = GUIContentCaches.Get(_ParameterReferenceProperty.property.displayName);
			EditorGUILayout.LabelField(label);

			int indentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel++;

			Rect position = GUILayoutUtility.GetRect(EditorContents.dropParameter, Styles.dropField);

			DropParameter(position);

			EditorGUI.indentLevel = indentLevel;
		}

		public void DropParameter(Rect position)
		{
			var draggingParameter = GetDraggingParameter();
			if (draggingParameter == null)
			{
				return;
			}

			position = EditorGUI.IndentedRect(position);

			GUIContent content = EditorContents.dropParameter;

			Event current = Event.current;
			switch (current.type)
			{
				case EventType.DragUpdated:
				case EventType.DragPerform:
					if (position.Contains(current.mousePosition))
					{
						DragAndDrop.visualMode = DragAndDropVisualMode.Link;

						if (current.type == EventType.DragPerform)
						{
							_ParameterReferenceProperty.SetParameter(draggingParameter);
							GUI.changed = true;

							DragAndDrop.AcceptDrag();
							DragAndDrop.activeControlID = 0;
						}

						current.Use();
					}
					break;
				case EventType.Repaint:
					{
						Styles.dropField.Draw(position, content, false, false, position.Contains(current.mousePosition), false);
					}
					break;
			}
		}
	}

	internal sealed class ParameterReferencePropertyEditor : OwnsDataSlotPropertyEditor
	{
		ParameterReferenceProperty _ParameterReferenceProperty = null;
		ParameterReferenceEditorGUI _EditorGUI = null;

		protected override DataSlotProperty slotProperty
		{
			get
			{
				return _ParameterReferenceProperty.slotProperty;
			}
		}

		protected override void OnInitialize()
		{
			base.OnInitialize();

			_ParameterReferenceProperty = new ParameterReferenceProperty(property, fieldInfo);
			_EditorGUI = new ParameterReferenceEditorGUI(_ParameterReferenceProperty);
		}

		private bool _IsInGUI;

		protected override void OnConnectionChanged(bool isConnect)
		{
			if (!property.IsValid())
			{
				return;
			}

			bool isInGUI = _IsInGUI || property.serializedObject.hasModifiedProperties;

			if (!isInGUI)
			{
				property.serializedObject.Update();
			}

			if (isConnect)
			{
				_ParameterReferenceProperty.type = ParameterReferenceType.DataSlot;
			}
			else if (_ParameterReferenceProperty.type == ParameterReferenceType.DataSlot && (ArborSettings.dataSlotShowMode == DataSlotShowMode.Outside || ArborSettings.dataSlotShowMode == DataSlotShowMode.Flexibly))
			{
				_ParameterReferenceProperty.type = ParameterReferenceType.Constant;
			}

			if (!isInGUI)
			{
				property.serializedObject.ApplyModifiedProperties();
			}
		}

		Rect DoGUI(Rect position, GUIContent label, bool isDraw)
		{
			SerializedProperty containerProperty = _ParameterReferenceProperty.containerProperty;

			ParameterReferenceType parameterReferenceType = _ParameterReferenceProperty.type;

			Rect lineRect = new Rect(position);

			lineRect.height = EditorGUIUtility.singleLineHeight;

			if (isDraw)
			{
				EditorGUI.LabelField(lineRect, label);
			}

			lineRect.y += lineRect.height + EditorGUIUtility.standardVerticalSpacing;
			lineRect.height = 0;

			int indentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel++;

			GUIContent containerLabel = GUIContentCaches.Get(containerProperty.displayName);

			switch (parameterReferenceType)
			{
				case ParameterReferenceType.Constant:
					{
						lineRect.height = EditorGUI.GetPropertyHeight(containerProperty, containerLabel);
					}
					break;
				case ParameterReferenceType.DataSlot:
					{
						lineRect.height = EditorGUI.GetPropertyHeight(_ParameterReferenceProperty.slotProperty.property, containerLabel);
					}
					break;
			}

			if (isDraw)
			{
				Rect fieldPosition = EditorGUITools.SubtractDropdownWidth(lineRect);

				int targetInstanceID = property.serializedObject.targetObject.GetInstanceID();
				BehaviourEditorGUI editorGUI = BehaviourEditorGUI.Get(targetInstanceID);
				if (editorGUI != null)
				{
					if (_ParameterReferenceProperty.IsShowOutsideSlot())
					{
						editorGUI.SetInputSlotLink(fieldPosition, _ParameterReferenceProperty.slotProperty.property);
					}
				}

				switch (parameterReferenceType)
				{
					case ParameterReferenceType.Constant:
						{
							EditorGUI.PropertyField(fieldPosition, containerProperty, containerLabel);
						}
						break;
					case ParameterReferenceType.DataSlot:
						{
							EditorGUI.PropertyField(fieldPosition, _ParameterReferenceProperty.slotProperty.property, containerLabel);
						}
						break;
				}
			}

			if (isDraw)
			{
				Rect popupRect = EditorGUITools.GetDropdownRect(lineRect);

				EditorGUI.BeginChangeCheck();
				ParameterReferenceType newParameterReferenceType = EditorGUITools.EnumPopupUnIndent(popupRect, GUIContent.none, parameterReferenceType, BuiltInStyles.shurikenDropDown);
				if (EditorGUI.EndChangeCheck())
				{
					if (parameterReferenceType == ParameterReferenceType.DataSlot)
					{
						_ParameterReferenceProperty.slotProperty.Disconnect();
					}
					_ParameterReferenceProperty.type = newParameterReferenceType;
				}
			}

			lineRect.y += lineRect.height + EditorGUIUtility.standardVerticalSpacing;
			lineRect.height = EditorGUIUtility.singleLineHeight;

			if (isDraw)
			{
				_EditorGUI.ParameterField(lineRect);
			}

			lineRect.y += lineRect.height;
			lineRect.height = 0;

			//bool isDraggingParameter = ParameterReferenceEditorGUI.IsHoverableParameter(_ParameterReferenceProperty);
			var draggingParameter = _EditorGUI.GetDraggingParameter();
			if (draggingParameter != null)
			{
				lineRect.height = _EditorGUI.GetDropParameterHeight();

				if (isDraw)
				{
					_EditorGUI.DropParameter(lineRect);
				}

				lineRect.y += lineRect.height + EditorGUIUtility.standardVerticalSpacing;
				lineRect.height = 0;
			}

			EditorGUI.indentLevel = indentLevel;

			position.yMax = Mathf.Max(position.yMax, lineRect.yMax);

			return position;
		}

		protected override void DoGUI(Rect position, GUIContent label)
		{
			_IsInGUI = true;

			label = EditorGUI.BeginProperty(position, label, property);
			DoGUI(position, label, true);
			EditorGUI.EndProperty();

			_IsInGUI = false;
		}

		protected override float GetHeight(GUIContent label)
		{
			Rect position = DoGUI(new Rect(), label, false);
			return position.height;
		}
	}

	[CustomPropertyDrawer(typeof(ParameterReference), true)]
	internal sealed class ParameterReferencePropertyDrawer : PropertyEditorDrawer<ParameterReferencePropertyEditor>
	{
	}
}
