//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.Reflection;

using Arbor;

namespace ArborEditor
{
	using UnityObject = UnityEngine.Object;

	public sealed class Icons
	{
		public static readonly Texture2D logo;
		public static readonly Texture2D logoIcon_LightSkin;
		public static readonly Texture2D logoIcon_DarkSkin;
		public static readonly Texture2D logoIconLarge;

		public static readonly Texture2D startStateIcon;
		public static readonly Texture2D currentStartStateIcon;
		public static readonly Texture2D normalStateIcon;
		public static readonly Texture2D currentNormalStateIcon;
		public static readonly Texture2D residentStateIcon;
		public static readonly Texture2D calculatorNodeIcon;
		public static readonly Texture2D frameCalculateIcon;
		public static readonly Texture2D scopeCalculateIcon;
		public static readonly Texture2D alwaysCalculateIcon;
		public static readonly Texture2D groupNodeIcon;
		public static readonly Texture2D groupNodeIconVertical;
		public static readonly Texture2D groupNodeIconHorizontal;
		public static readonly Texture2D commentNodeIcon;

		public static readonly Texture2D captureIcon;
		public static readonly Texture2D notificationIcon;
		public static readonly Texture2D filterIcon;

		public static readonly Texture2D noneIcon;
		public static readonly Texture2D namespaceIcon;
		public static readonly Texture2D classIcon;
		public static readonly Texture2D classStaticIcon;
		public static readonly Texture2D interfaceIcon;
		public static readonly Texture2D enumIcon;
		public static readonly Texture2D structIcon;
		public static readonly Texture2D delegateIcon;

		public static readonly Texture2D methodIcon;
		public static readonly Texture2D methodStaticIcon;

		public static readonly Texture2D fieldIcon;
		public static readonly Texture2D fieldConstIcon;
		public static readonly Texture2D fieldStaticIcon;

		public static readonly Texture2D propertyIcon;
		public static readonly Texture2D propertyStaticIcon;

		public static readonly Texture2D transitionTimingLateUpdateOverwrite;
		public static readonly Texture2D transitionTimingImmediate;
		public static readonly Texture2D transitionTimingLateUpdateDontOverwrite;
		public static readonly Texture2D transitionTimingNextUpdateOverwrite;
		public static readonly Texture2D transitionTimingNextUpdateDontOverwrite;

		public static readonly Texture2D rootIcon;
		public static readonly Texture2D defaultCompositeIcon;
		public static readonly Texture2D defaultActionIcon;

		public static readonly Texture2D stateLinkPopupIcon;

		public static readonly Texture2D homeIcon;
		public static readonly Texture2D documentationIcon;
		public static readonly Texture2D forumIcon;
		public static readonly Texture2D reviewIcon;
		public static readonly Texture2D tutorialIcon;

		public static readonly Texture2D stateLinkRerouteLargetPinNormal;

		public static readonly Texture2D popupIcon;
		public static readonly Texture2D helpIcon;

		static Texture2D s_DefaultAssetIcon;

		static readonly Dictionary<Type, Texture> s_TypeIcons;

		static Icons()
		{
			logo = EditorResources.LoadTexture("ArborLogo");
			logoIcon_LightSkin = EditorResources.LoadTexture("ArborLogoIcon@LightSkin");
			logoIcon_DarkSkin = EditorResources.LoadTexture("ArborLogoIcon@DarkSkin");
			logoIconLarge = EditorResources.LoadTexture("ArborLogoIconLarge");

			startStateIcon = EditorResources.LoadTexture("Icons/StateMachine/start state icon");
			currentStartStateIcon = EditorResources.LoadTexture("Icons/StateMachine/current start state icon");
			normalStateIcon = EditorResources.LoadTexture("Icons/StateMachine/normal state icon");
			currentNormalStateIcon = EditorResources.LoadTexture("Icons/StateMachine/current normal state icon");
			residentStateIcon = EditorResources.LoadTexture("Icons/StateMachine/resident state icon");
			calculatorNodeIcon = EditorResources.LoadTexture("Icons/StateMachine/calculator node icon");
			frameCalculateIcon = EditorResources.LoadTexture("Icons/StateMachine/frame calculate icon");
			scopeCalculateIcon = EditorResources.LoadTexture("Icons/StateMachine/scope calculate icon");
			alwaysCalculateIcon = EditorResources.LoadTexture("Icons/StateMachine/always calculate icon");
			groupNodeIcon = EditorResources.LoadTexture("Icons/StateMachine/group node icon");
			groupNodeIconVertical = EditorResources.LoadTexture("Icons/StateMachine/group node icon vertical");
			groupNodeIconHorizontal = EditorResources.LoadTexture("Icons/StateMachine/group node icon horizontal");
			commentNodeIcon = EditorResources.LoadTexture("Icons/StateMachine/comment node icon");

			captureIcon = EditorResources.LoadTexture("Icons/Toolbar/capture icon");
			notificationIcon = EditorResources.LoadTexture("Icons/Toolbar/notification icon");
			filterIcon = EditorResources.LoadTexture("Icons/Toolbar/filter icon");

			noneIcon = EditorResources.LoadTexture("Icons/Class/none icon");
			namespaceIcon = EditorResources.LoadTexture("Icons/Class/namespace icon");
			classIcon = EditorResources.LoadTexture("Icons/Class/class icon");
			classStaticIcon = EditorResources.LoadTexture("Icons/Class/class static icon");
			interfaceIcon = EditorResources.LoadTexture("Icons/Class/interface icon");
			enumIcon = EditorResources.LoadTexture("Icons/Class/enum icon");
			structIcon = EditorResources.LoadTexture("Icons/Class/struct icon");
			delegateIcon = EditorResources.LoadTexture("Icons/Class/delegate icon");

			methodIcon = EditorResources.LoadTexture("Icons/Class/method icon");
			methodStaticIcon = EditorResources.LoadTexture("Icons/Class/method static icon");

			fieldIcon = EditorResources.LoadTexture("Icons/Class/field icon");
			fieldConstIcon = EditorResources.LoadTexture("Icons/Class/field const icon");
			fieldStaticIcon = EditorResources.LoadTexture("Icons/Class/field static icon");

			propertyIcon = EditorResources.LoadTexture("Icons/Class/property icon");
			propertyStaticIcon = EditorResources.LoadTexture("Icons/Class/property static icon");

			transitionTimingLateUpdateOverwrite = EditorResources.LoadTexture("Icons/TransitionTiming/LateUpdateOverwrite");
			transitionTimingImmediate = EditorResources.LoadTexture("Icons/TransitionTiming/Immediate");
			transitionTimingLateUpdateDontOverwrite = EditorResources.LoadTexture("Icons/TransitionTiming/LateUpdateDontOverwrite");
			transitionTimingNextUpdateDontOverwrite = EditorResources.LoadTexture("Icons/TransitionTiming/NextUpdateDontOverwrite");
			transitionTimingNextUpdateOverwrite = EditorResources.LoadTexture("Icons/TransitionTiming/NextUpdateOverwrite");

			rootIcon = EditorResources.LoadTexture("Icons/BehaviourTree/root icon");
			defaultCompositeIcon = EditorResources.LoadTexture("Icons/BehaviourTree/default composite icon");
			defaultActionIcon = EditorResources.LoadTexture("Icons/BehaviourTree/default action icon");

			stateLinkPopupIcon = EditorResources.LoadTexture("Icons/state link popup");

			homeIcon = EditorResources.LoadTexture("Icons/WelcomeWindow/HomeIcon");
			documentationIcon = EditorResources.LoadTexture("Icons/WelcomeWindow/DocumentationIcon");
			forumIcon = EditorResources.LoadTexture("Icons/WelcomeWindow/ForumIcon");
			reviewIcon = EditorResources.LoadTexture("Icons/WelcomeWindow/ReviewIcon");
			tutorialIcon = EditorResources.LoadTexture("Icons/WelcomeWindow/TutorialIcon");

			stateLinkRerouteLargetPinNormal = EditorResources.LoadTexture("Icons/StateMachine/state link reroute large pin normal");

			popupIcon = FindTexture("_Popup");
			helpIcon = FindTexture("_Help");

			s_DefaultAssetIcon = FindTexture("DefaultAsset Icon");

			s_TypeIcons = new Dictionary<Type, Texture>();

			var scripts = ScriptsUtility.scripts;
			for (int i = 0; i < scripts.Count; i++)
			{
				MonoScript script = scripts[i];
				Type classType = script.GetClass();
				if (classType == null)
				{
					continue;
				}

				Texture icon = AssetDatabase.GetCachedIcon(AssetDatabase.GetAssetPath(script));
				if (icon != null)
				{
					s_TypeIcons[classType] = icon;
				}
			}
		}

		static Texture2D FindTexture(string name)
		{
			Texture2D tex = EditorGUIUtility.FindTexture(name);
			if (tex != null)
			{
				return tex;
			}

			GUIContent content = EditorGUIUtility.IconContent(name);
			if (content != null)
			{
				return content.image as Texture2D;
			}

			return null;
		}

		static Texture GetScriptIcon(Type type)
		{
			Texture tex = null;
			if (s_TypeIcons.TryGetValue(type, out tex))
			{
				return tex;
			}

			tex = EditorGUITools.FindTexture(type);
			if (tex == null)
			{
				tex = s_DefaultAssetIcon;
			}

			s_TypeIcons[type] = tex;

			return tex;
		}

		public static Texture GetTypeIcon(Type type)
		{
			if (type == null)
			{
				return noneIcon;
			}

			if (typeof(UnityObject).IsAssignableFrom(type))
			{
				Texture tex = GetScriptIcon(type);
				if (tex != null)
				{
					return tex;
				}
			}

			if (type.IsInterface)
			{
				return interfaceIcon;
			}
			if (type.IsClass)
			{
				if (typeof(Delegate).IsAssignableFrom(type))
				{
					return delegateIcon;
				}
				if (type.IsAbstract && type.IsSealed) // static
				{
					return classStaticIcon;
				}
				return classIcon;
			}
			else if (type.IsEnum)
			{
				return enumIcon;
			}
			else if (type.IsValueType)
			{
				return structIcon;
			}

			return null;
		}

		public static Texture GetMethodIcon(MethodInfo methodInfo)
		{
			if (methodInfo == null)
			{
				return noneIcon;
			}

			if (methodInfo.IsStatic)
			{
				return methodStaticIcon;
			}

			return methodIcon;
		}

		public static Texture GetFieldIcon(FieldInfo fieldInfo)
		{
			if (fieldInfo == null)
			{
				return noneIcon;
			}

			if (fieldInfo.IsLiteral)
			{
				return fieldConstIcon;
			}
			else if (fieldInfo.FieldType == typeof(decimal))
			{
				// const decimal is static & readonly
				if (fieldInfo.IsStatic && fieldInfo.IsInitOnly)
				{
					return fieldConstIcon;
				}
			}

			if (fieldInfo.IsStatic)
			{
				return fieldStaticIcon;
			}

			return fieldIcon;
		}

		public static Texture GetPropertyIcon(PropertyInfo propertyInfo)
		{
			if (propertyInfo == null)
			{
				return noneIcon;
			}

			if (propertyInfo.GetAccessors()[0].IsStatic)
			{
				return propertyStaticIcon;
			}


			return propertyIcon;
		}

		public static Texture GetMemberIcon(MemberInfo memberInfo)
		{
			MethodInfo methodInfo = memberInfo as MethodInfo;
			if (methodInfo != null)
			{
				return GetMethodIcon(methodInfo);
			}

			FieldInfo fieldInfo = memberInfo as FieldInfo;
			if (fieldInfo != null)
			{
				return GetFieldIcon(fieldInfo);
			}

			PropertyInfo propertyInfo = memberInfo as PropertyInfo;
			if (propertyInfo != null)
			{
				return GetPropertyIcon(propertyInfo);
			}

			return noneIcon;
		}
	}
}