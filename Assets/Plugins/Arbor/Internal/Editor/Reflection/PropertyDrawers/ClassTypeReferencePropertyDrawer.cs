//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System;

using Arbor;

namespace ArborEditor
{
	[CustomPropertyDrawer(typeof(ClassTypeReference))]
	internal sealed class ClassTypeReferencePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			if (property.IsInvalidManagedReference())
			{
				EditorGUI.PropertyField(position, property, label);
				return;
			}

			label = EditorGUI.BeginProperty(position, label, property);

			ClassTypeReferenceProperty typeProperty = new ClassTypeReferenceProperty(property);

			Type type = typeProperty.type;

			ClassTypeConstraintAttribute constraintAttribute = property.GetStateData<ClassTypeConstraintAttribute>();
			if (constraintAttribute == null)
			{
				constraintAttribute = AttributeHelper.GetAttribute<ClassTypeConstraintAttribute>(fieldInfo);
			}

			IDefinableType definableType = null;
			if (constraintAttribute != null)
			{
				definableType = new ClassTypeConstraintFilter(constraintAttribute, fieldInfo);
			}

			TypeFilterAttribute typeFilterAttribute = property.GetStateData<TypeFilterAttribute>();
			if (typeFilterAttribute == null)
			{
				typeFilterAttribute = AttributeHelper.GetAttribute<TypeFilterAttribute>(fieldInfo);
			}

			bool hasFilter = false;
			TypeFilterFlags typeFilterFlags = (TypeFilterFlags)0;

			if (typeFilterAttribute != null)
			{
				hasFilter = true;
				typeFilterFlags = typeFilterAttribute.flags;
			}
			else if (definableType == null)
			{
				hasFilter = true;
				typeFilterFlags = (TypeFilterFlags)(-1);
			}

			string typeName = null;
			if (type != null)
			{
				typeName = TypeUtility.GetTypeName(type);
			}
			else
			{
				string assemblyTypeName = typeProperty.assemblyTypeName.stringValue;
				if (!string.IsNullOrEmpty(assemblyTypeName))
				{
					int index = assemblyTypeName.IndexOf(',');
					if (index >= 0)
					{
						assemblyTypeName = assemblyTypeName.Substring(0, index);
					}
					typeName = string.Format("<Missing {0} >", assemblyTypeName);
				}
				else
				{
					typeName = TypePopupWindow.kNoneText;
				}
			}

			EditorGUI.BeginChangeCheck();
			type = TypeSelector.PopupField(position, type, label, definableType, hasFilter, typeFilterFlags, GUIContentCaches.Get(typeName));
			if (EditorGUI.EndChangeCheck())
			{
				typeProperty.type = type;
			}

			EditorGUI.EndProperty();
		}
	}
}