//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using Arbor;
namespace ArborEditor
{
	[CustomPropertyDrawer(typeof(DataSlot), true)]
	internal class DataSlotPropertyDrawer : PropertyDrawer
	{
		private static readonly int s_NotSupportInspectorHash = "s_NotSupportInspectorHash".GetHashCode();

		private sealed class ExternalFields
		{
			public List<string> fieldNames = new List<string>();
		}

		private static Dictionary<System.Type, ExternalFields> s_ExternalFields = new Dictionary<System.Type, ExternalFields>();

		private static ExternalFields GetExternalFields(System.Type fieldType)
		{
			System.Type elementType = Arbor.Serialization.SerializationUtility.ElementType(fieldType);

			ExternalFields externalFields = null;
			if (!s_ExternalFields.TryGetValue(elementType, out externalFields))
			{
				externalFields = new ExternalFields();
				var fields = elementType.GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
				for (int fieldIndex = 0; fieldIndex < fields.Length; fieldIndex++)
				{
					System.Reflection.FieldInfo fieldInfo = fields[fieldIndex];
					if (!Arbor.Serialization.SerializationUtility.IsSerializableField(fieldInfo) ||
						elementType != fieldInfo.DeclaringType ||
						AttributeHelper.HasAttribute<HideSlotFields>(fieldInfo) ||
						AttributeHelper.HasAttribute<HideInInspector>(fieldInfo))
					{
						continue;
					}
					externalFields.fieldNames.Add(fieldInfo.Name);
				}
				s_ExternalFields.Add(elementType, externalFields);

			}
			return externalFields;
		}

		private static bool HasExternalFields(System.Type fieldType)
		{
			ExternalFields externalFields = GetExternalFields(fieldType);
			return (externalFields != null && externalFields.fieldNames.Count > 0);
		}

		private System.Type GetFieldType(SerializedProperty property)
		{
			if (property.propertyType == SerializedPropertyType.ManagedReference)
			{
				System.Type type = property.GetTypeFromManagedReferenceFullTypeName();
				if (type != null)
				{
					return type;
				}
			}
			return fieldInfo.FieldType;
		}

		Rect DoFieldsGUI(Rect position, SerializedProperty property, bool isLayout)
		{
			Rect rect = new Rect(position);

			rect.height = 0f;

			System.Type slotFieldType = GetFieldType(property);
			ExternalFields externalFields = GetExternalFields(slotFieldType);

			for (int i = 0, count = externalFields.fieldNames.Count; i < count; ++i)
			{
				string fieldName = externalFields.fieldNames[i];
				SerializedProperty fieldProperty = property.FindPropertyRelative(fieldName);

				GUIContent label = GUIContentCaches.Get(fieldProperty.displayName);

				rect.height = EditorGUI.GetPropertyHeight(fieldProperty, label, true);

				if (!isLayout)
				{
					EditorGUI.PropertyField(rect, fieldProperty, label, true);
				}

				rect.y += rect.height + EditorGUIUtility.standardVerticalSpacing;
				rect.height = 0f;
			}

			position.yMax = Mathf.Max(rect.yMax, position.yMax);
			return position;
		}

		float GetFieldsHeight(SerializedProperty property)
		{
			Rect position = DoFieldsGUI(new Rect(), property, true);
			return position.height;
		}

		void OnFieldsGUI(Rect position, SerializedProperty property)
		{
			DoFieldsGUI(position, property, false);
		}

		bool IsShowSlotField(SerializedProperty property)
		{
			System.Type slotFieldType = GetFieldType(property);
			if (!HasExternalFields(slotFieldType))
			{
				return false;
			}

			if (AttributeHelper.HasAttribute<HideSlotFields>(fieldInfo))
			{
				return false;
			}

			return true;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			using (new ProfilerScope("DataSlotPropertyDrawer.OnGUI"))
			{
				if (ArborEditorWindow.isInArborEditor)
				{
					if (property.IsInvalidManagedReference())
					{
						EditorGUI.PropertyField(position, property, label);
						return;
					}

					Rect slotPosition = new Rect(position);

					slotPosition.height = EditorGUIUtility.singleLineHeight;

					DataSlot slot = SerializedPropertyUtility.GetPropertyObject<DataSlot>(property);
					if (slot != null)
					{
						switch (slot.slotType)
						{
							case SlotType.Input:
								switch (ArborSettings.dataSlotShowMode)
								{
									case DataSlotShowMode.Outside:
									case DataSlotShowMode.Flexibly:
										{
											int targetInstanceID = property.serializedObject.targetObject.GetInstanceID();
											BehaviourEditorGUI editorGUI = BehaviourEditorGUI.Get(targetInstanceID);
											editorGUI?.SetInputSlotLink(slotPosition, property);
											EditorGUI.LabelField(slotPosition, label);
										}
										break;
									case DataSlotShowMode.Inside:
										{
											int targetInstanceID = property.serializedObject.targetObject.GetInstanceID();
											BehaviourEditorGUI editorGUI = BehaviourEditorGUI.Get(targetInstanceID);
											if (editorGUI != null)
											{
												DataSlotGUI slotGUI = editorGUI.GetDataSlotGUI(property);

												slotGUI?.DoGUI(slotPosition, property, label);
											}
										}
										break;
								}
								break;
							case SlotType.Output:
								{
									int targetInstanceID = property.serializedObject.targetObject.GetInstanceID();
									BehaviourEditorGUI editorGUI = BehaviourEditorGUI.Get(targetInstanceID);
									if (editorGUI != null)
									{
										DataSlotGUI slotGUI = editorGUI.GetDataSlotGUI(property);

										slotGUI?.DoGUI(EditorGUI.IndentedRect(slotPosition), property, label);
									}
								}
								break;
						}
					}

					slotPosition.yMin += slotPosition.height + EditorGUIUtility.standardVerticalSpacing;

					bool isShowSlotField = IsShowSlotField(property);
					if (isShowSlotField)
					{
						int indentLevel = EditorGUI.indentLevel++;
						Rect fieldPosition = new Rect(slotPosition);
						fieldPosition.height = GetFieldsHeight(property);

						OnFieldsGUI(fieldPosition, property);
						EditorGUI.indentLevel = indentLevel;
					}
				}
				else
				{
					int controlID = GUIUtility.GetControlID(s_NotSupportInspectorHash, FocusType.Passive, position);

					Rect helpPosition = new Rect(position);
					if (label != GUIContent.none)
					{
						position.height = EditorGUIUtility.singleLineHeight;
						helpPosition.yMin += position.height;

						EditorGUI.PrefixLabel(position, controlID, label);
					}
					EditorGUI.HelpBox(helpPosition, Localization.GetWord("DataSlot.NotSupportInspector"), MessageType.Error);
				}
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			float height = 0.0f;
			if (ArborEditorWindow.isInArborEditor)
			{
				if (property.IsInvalidManagedReference())
				{
					return EditorGUI.GetPropertyHeight(property, label);
				}

				height += EditorGUIUtility.singleLineHeight;
				if (IsShowSlotField(property))
				{
					int indentLevel = EditorGUI.indentLevel++;
					height += GetFieldsHeight(property);
					EditorGUI.indentLevel = indentLevel;
				}
			}
			else
			{
				string message = Localization.GetWord("DataSlot.NotSupportInspector");

				height += EditorGUIUtility.singleLineHeight;
				height += EditorGUITools.GetHelpBoxHeight(message, MessageType.Error);
			}

			return height;
		}
	}
}
