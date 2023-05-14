//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace ArborEditor
{
	public interface IPropertyEditor
	{
		SerializedProperty property
		{
			get;
		}

		System.Reflection.FieldInfo fieldInfo
		{
			get;
		}

		void OnInitialize(SerializedProperty property, System.Reflection.FieldInfo fieldInfo);
		void OnDestroy();
	}

	public class PropertyEditor : IPropertyEditor
	{
		public SerializedProperty property
		{
			get;
			private set;
		}
		public System.Reflection.FieldInfo fieldInfo
		{
			get;
			private set;
		}

		protected virtual void OnInitialize()
		{
		}

		protected virtual void OnDestroy()
		{
		}

		protected virtual void OnGUI(Rect position, GUIContent label)
		{
		}

		protected virtual float GetHeight(GUIContent label)
		{
			return 0;
		}

		public void DoOnGUI(Rect position, GUIContent label)
		{
			OnGUI(position, label);
		}

		public float DoGetHeight(GUIContent label)
		{
			return GetHeight(label);
		}

		void IPropertyEditor.OnInitialize(SerializedProperty property, System.Reflection.FieldInfo fieldInfo)
		{
			this.property = property;
			this.fieldInfo = fieldInfo;

			OnInitialize();
		}

		void IPropertyEditor.OnDestroy()
		{
			OnDestroy();
		}
	}

	public static class PropertyEditorUtility<T> where T : class, IPropertyEditor, new()
	{
		private static Dictionary<SerializedPropertyKey, T> s_PropertyEditors = null;

		static PropertyEditorUtility()
		{
			EditorApplication.update += Update;
		}

		static void Update()
		{
			if (s_PropertyEditors == null)
			{
				return;
			}

			List<SerializedPropertyKey> removeList = null;
			foreach (var pair in s_PropertyEditors)
			{
				SerializedProperty prop = pair.Key.GetProperty();
				T proeprtyEditor = pair.Value;
				if (prop == null || !SerializedPropertyUtility.EqualContents(prop, proeprtyEditor.property))
				{
					proeprtyEditor.OnDestroy();
					if (removeList == null)
					{
						removeList = new List<SerializedPropertyKey>();
					}
					removeList.Add(pair.Key);
				}
			}

			if (removeList != null)
			{
				foreach (var key in removeList)
				{
					s_PropertyEditors.Remove(key);
				}
			}
		}

		public static T GetPropertyEditor(SerializedProperty property, System.Reflection.FieldInfo fieldInfo)
		{
			if (s_PropertyEditors == null)
			{
				s_PropertyEditors = new Dictionary<SerializedPropertyKey, T>();
			}

			SerializedPropertyKey propertyKey = new SerializedPropertyKey(property);

			T editor = default(T);
			if (s_PropertyEditors.TryGetValue(propertyKey, out editor))
			{
				if (SerializedPropertyUtility.EqualContents(property, editor.property))
				{
					return editor;
				}

				editor.OnDestroy();
				s_PropertyEditors.Remove(propertyKey);
			}

			editor = new T();
			editor.OnInitialize(property.Copy(), fieldInfo);

			s_PropertyEditors.Add(propertyKey, editor);

			return editor as T;
		}
	}

	public class PropertyEditorDrawer<T> : PropertyDrawer where T : PropertyEditor, new()
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			try
			{
				if (property.IsInvalidManagedReference())
				{
					EditorGUI.PropertyField(position, property, label);
					return;
				}

				T editor = PropertyEditorUtility<T>.GetPropertyEditor(property, fieldInfo);

				editor.DoOnGUI(position, label);
			}
			catch (System.Exception ex)
			{
				if (UnityEditorBridge.GUIUtilityBridge.ShouldRethrowException(ex))
				{
					throw;
				}
				else
				{
					Debug.LogException(ex);
				}
			}
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			try
			{
				if (property.IsInvalidManagedReference())
				{
					return EditorGUI.GetPropertyHeight(property, label);
				}

				T editor = PropertyEditorUtility<T>.GetPropertyEditor(property, fieldInfo);

				return editor.DoGetHeight(label);
			}
			catch (System.Exception ex)
			{
				if (UnityEditorBridge.GUIUtilityBridge.ShouldRethrowException(ex))
				{
					throw;
				}
				else
				{
					Debug.LogException(ex);
				}

				return 0f;
			}
		}
	}
}