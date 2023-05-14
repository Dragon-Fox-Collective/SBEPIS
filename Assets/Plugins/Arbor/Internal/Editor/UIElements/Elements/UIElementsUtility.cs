//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.UIElements
{
	using Arbor.DynamicReflection;

	internal static class UIElementsUtility
	{
#if ARBOR_DLL
		private static readonly PropertyInfo s_TransformOriginProperty;
		private static readonly System.Type s_TransformOriginType;

		private static Object s_BoldFont;
		private static MethodInfo s_FromSDFFontMethod;
		private static PropertyInfo s_UnityFontDefinitionProperty;

		private static readonly PropertyInfo s_TextOverflowProperty;
		private static readonly object s_TextOverflowEllipsis;

#elif UNITY_2021_2_OR_NEWER
		static UnityEngine.TextCore.Text.FontAsset s_BoldFont;
#endif

		static UIElementsUtility()
		{
#if ARBOR_DLL
			var styleType = typeof(IStyle);
			s_TransformOriginProperty = styleType.GetProperty("transformOrigin", BindingFlags.Instance | BindingFlags.Public);
			s_TransformOriginType = AssemblyHelper.GetTypeByName("UnityEngine.UIElements.TransformOrigin");
			
			var fontDefinitionType = AssemblyHelper.GetTypeByName("UnityEngine.UIElements.FontDefinition");
			s_FromSDFFontMethod = fontDefinitionType?.GetMethod("FromSDFFont", BindingFlags.Static | BindingFlags.Public);
			s_UnityFontDefinitionProperty = styleType.GetProperty("unityFontDefinition");

			s_TextOverflowProperty = styleType.GetProperty("textOverflow", BindingFlags.Instance | BindingFlags.Public);
			if (s_TextOverflowProperty != null)
			{
				var textOverflowType = AssemblyHelper.GetTypeByName("UnityEngine.UIElements.TextOverflow");
				var textOverflowEllipsis = System.Enum.ToObject(textOverflowType, 1);
				s_TextOverflowEllipsis = System.Activator.CreateInstance(s_TextOverflowProperty.PropertyType, textOverflowEllipsis);
			}
#endif
		}

		public static bool IsLayoutEvent(EventBase evt)
		{
			long eventTypeId = evt.eventTypeId;

			if (evt.propagationPhase == PropagationPhase.DefaultAction && eventTypeId == GeometryChangedEvent.TypeId())
			{
				return true;
			}

			return false;
		}

		public static bool IsVisible(VisualElement target)
		{
			while (target != null)
			{
				if (target.resolvedStyle.display == DisplayStyle.None)
				{
					return false;
				}

				target = target.parent;
			}

			return true;
		}

		public static VisualElement GetFirstAncestorWithClass(VisualElement element, string className)
		{
			if (element == null)
				return null;

			if (element.ClassListContains(className))
				return element;

			return GetFirstAncestorWithClass(element.parent, className);
		}

		public static void SetTransformOrigin(VisualElement element, float x, float y, float z)
		{
#if ARBOR_DLL
			if (s_TransformOriginProperty != null && s_TransformOriginType != null)
			{
				var transformOrigin = System.Activator.CreateInstance(s_TransformOriginType, new Length(x), new Length(y), z);
				var styleTransformOrigin = System.Activator.CreateInstance(s_TransformOriginProperty.PropertyType, transformOrigin);
				s_TransformOriginProperty.SetValue(element.style, styleTransformOrigin);
			}
#elif UNITY_2021_2_OR_NEWER
			element.style.transformOrigin = new TransformOrigin(x, y, z);
#endif
		}

		public static void SetEllipsis(VisualElement element)
		{
#if ARBOR_DLL
			if (s_TextOverflowProperty != null)
			{
				s_TextOverflowProperty.SetValue(element.style, s_TextOverflowEllipsis);
			}
#elif UNITY_2020_1_OR_NEWER
			element.style.textOverflow = TextOverflow.Ellipsis;
#endif
		}

		public static void SetBoldFont(VisualElement element)
		{
#if ARBOR_DLL
			if (s_UnityFontDefinitionProperty != null && s_FromSDFFontMethod != null)
			{
				if (s_BoldFont == null)
				{
					s_BoldFont = EditorGUIUtility.Load("UIPackageResources/Fonts/Inter/Inter-SemiBold SDF.asset");
				}
				var fontDefiniation = s_FromSDFFontMethod.Invoke(null, new object[] { s_BoldFont });
				var styleFontDefinition = System.Activator.CreateInstance(s_UnityFontDefinitionProperty.PropertyType, fontDefiniation);
				s_UnityFontDefinitionProperty.SetValue(element.style, styleFontDefinition);
			}
#elif UNITY_2021_2_OR_NEWER
			if (s_BoldFont == null)
			{
				s_BoldFont = EditorGUIUtility.Load("UIPackageResources/Fonts/Inter/Inter-SemiBold SDF.asset") as UnityEngine.TextCore.Text.FontAsset;
			}

			element.style.unityFontDefinition = FontDefinition.FromSDFFont(s_BoldFont);
#endif
		}
	}
}