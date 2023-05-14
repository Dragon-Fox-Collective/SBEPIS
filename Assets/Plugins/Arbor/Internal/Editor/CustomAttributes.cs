//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

using Arbor;

namespace ArborEditor
{
	internal static class CustomAttributes<TAttribute,TEditor> where TAttribute : CustomAttribute
	{
		public sealed class EditorInfo
		{
			public Type objectType;
			public Type editorType;
			public TAttribute attribute;
		}
		private static readonly List<EditorInfo> s_CustomEditors;
		private static bool s_Initialized;

		static CustomAttributes()
		{
			s_CustomEditors = new List<EditorInfo>();
		}

		public static void Rebuild(Assembly assembly)
		{
			var types = TypeUtility.GetTypesFromAssembly(assembly);
			for (int typeIndex = 0; typeIndex < types.Length; typeIndex++)
			{
				Type type = types[typeIndex];
				if (!typeof(TEditor).IsAssignableFrom(type))
				{
					continue;
				}

				var attributes = AttributeHelper.GetAttributes<TAttribute>(type);
				for (int i = 0; i < attributes.Length; i++)
				{
					TAttribute customAttribute = attributes[i];
					if (customAttribute.classType != null)
					{
						EditorInfo editorType = new EditorInfo();
						editorType.objectType = customAttribute.classType;
						editorType.editorType = type;
						editorType.attribute = customAttribute;
						s_CustomEditors.Add(editorType);
					}
					else
					{
						Debug.Log("Can't load custom editor " + type.Name + " because the class type is null.");
					}
				}
			}
		}

		public static EditorInfo FindEditorInfo(System.Type objectType)
		{
			if (!s_Initialized)
			{
				Assembly[] assemblies = UnityEditorBridge.EditorAssembliesBridge.loadedAssemblies;
				for (int i = assemblies.Length - 1; i >= 0; --i)
				{
					Rebuild(assemblies[i]);
				}
				s_Initialized = true;
			}

			for (int i = 0; i < s_CustomEditors.Count; i++)
			{
				EditorInfo info = s_CustomEditors[i];
				if (info.objectType == objectType)
				{
					return info;
				}
			}

			for (int i = 0; i < s_CustomEditors.Count; i++)
			{
				EditorInfo info = s_CustomEditors[i];
				if (objectType.IsSubclassOf(info.objectType))
				{
					return info;
				}
			}

			return null;
		}

		public static Type FindEditorType(System.Type objectType)
		{
			EditorInfo editorInfo = FindEditorInfo(objectType);
			if (editorInfo != null)
			{
				return editorInfo.editorType;
			}

			return null;
		}
	}
}