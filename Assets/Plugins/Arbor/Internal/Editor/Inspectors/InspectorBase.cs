//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArborEditor.Inspectors
{
	internal interface IInspectorElement
	{
		void OnGUI();
	}

	internal sealed class SpaceElement : IInspectorElement
	{
		void IInspectorElement.OnGUI()
		{
			EditorGUILayout.Space();
		}
	}

	internal sealed class PropertyElement : IInspectorElement
	{
		private SerializedProperty _Property;
		private GUIContent _Label;
		private bool _IncludeChildren;

		public PropertyElement(SerializedProperty property, GUIContent label, bool includeChildren)
		{
			_Property = property;
			_Label = label;
			_IncludeChildren = includeChildren;
		}

		void IInspectorElement.OnGUI()
		{
			EditorGUILayout.PropertyField(_Property, _Label, _IncludeChildren);
		}
	}

	internal sealed class IMGUIElement : IInspectorElement
	{
		private System.Action _OnGUIHandler;

		public IMGUIElement(System.Action onGUIHandler)
		{
			_OnGUIHandler = onGUIHandler;
		}

		void IInspectorElement.OnGUI()
		{
			_OnGUIHandler?.Invoke();
		}
	}

	public abstract class InspectorBase : Editor
	{
		private List<IInspectorElement> _InspectorElements = new List<IInspectorElement>();

		protected abstract void OnRegisterElements();

		private void RegisterElement(IInspectorElement element)
		{
			if (element == null)
			{
				throw new System.ArgumentNullException(nameof(element));
			}

			_InspectorElements.Add(element);
		}

		protected void RegisterSpace()
		{
			RegisterElement(new SpaceElement());
		}

		protected void RegisterIMGUI(System.Action onGUIHandler)
		{
			RegisterElement(new IMGUIElement(onGUIHandler));
		}

		void DoRegisterProperty(SerializedProperty property, string label, bool includeChildren)
		{
			GUIContent content = null;
			if (label != null)
			{
				if (label == "")
				{
					content = GUIContent.none;
				}
				else
				{
					content = GUIContentCaches.Get(label);
				}
			}
			RegisterElement(new PropertyElement(property, content, includeChildren));
		}

		void DoRegisterProperty(SerializedProperty property, string label)
		{
			DoRegisterProperty(property, label, IsChildrenIncluded(property));
		}

		protected void RegisterProperty(SerializedProperty property)
		{
			RegisterProperty(property, null);
		}

		protected void RegisterProperty(SerializedProperty property, bool includeChildren)
		{
			RegisterProperty(property, null, includeChildren);
		}

		protected void RegisterProperty(SerializedProperty property, string label)
		{
			if (property == null)
			{
				throw new System.ArgumentNullException(nameof(property));
			}

			DoRegisterProperty(property, label);
		}

		protected void RegisterProperty(SerializedProperty property, string label, bool includeChildren)
		{
			if (property == null)
			{
				throw new System.ArgumentNullException(nameof(property));
			}

			DoRegisterProperty(property, label, includeChildren);
		}

		protected void RegisterProperty(string name)
		{
			RegisterProperty(name, null);
		}

		protected void RegisterProperty(string name, bool includeChildren)
		{
			RegisterProperty(name, null, includeChildren);
		}

		protected void RegisterProperty(string name, string label)
		{
			var property = serializedObject.FindProperty(name);
			if (property == null)
			{
				throw new System.ArgumentException($"not found {name} property", nameof(name));
			}

			DoRegisterProperty(property, label);
		}

		protected void RegisterProperty(string name, string label, bool includeChildren)
		{
			var property = serializedObject.FindProperty(name);
			if (property == null)
			{
				throw new System.ArgumentException($"not found {name} property", nameof(name));
			}

			DoRegisterProperty(property, label, includeChildren);
		}

		private static bool IsChildrenIncluded(SerializedProperty prop)
		{
			switch (prop.propertyType)
			{
				case SerializedPropertyType.Generic:
				case SerializedPropertyType.Vector4:
					return true;
				default:
					return false;
			}
		}

		protected virtual void OnEnable()
		{
			_InspectorElements.Clear();

			OnRegisterElements();
		}

		public sealed override void OnInspectorGUI()
		{
			serializedObject.Update();

			for (int i = 0, count = _InspectorElements.Count; i < count; i++)
			{
				var inspectorGUI = _InspectorElements[i];
				inspectorGUI?.OnGUI();
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}