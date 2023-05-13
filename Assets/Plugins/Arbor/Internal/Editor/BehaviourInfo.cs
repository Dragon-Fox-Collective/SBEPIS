//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	public sealed class BehaviourInfo
	{
		public readonly System.ObsoleteAttribute obsolete;
		public readonly GUIContent titleContent;
		public readonly string helpUrl;
		public readonly string helpTooltip;

		static string ToHelpTooltip(string topic)
		{
			return string.Format(Localization.GetWord("OpenReference"), topic);
		}

		public BehaviourInfo(System.Type type)
		{
			if (type == null)
			{
				titleContent = GUIContentCaches.Get("Missing");
				return;
			}

			obsolete = AttributeHelper.GetAttribute<System.ObsoleteAttribute>(type);

			string title = GetObjectTypeName(type);

			if (obsolete != null)
			{
				title += " (Deprecated)";
			}

			titleContent = GUIContentCaches.Get(title);

			string classTypeName = type.Name;

			helpUrl = string.Empty;
			helpTooltip = "Open Arbor Document";

			BehaviourHelp behaviourHelp = AttributeHelper.GetAttribute<BehaviourHelp>(type);
			if (behaviourHelp != null)
			{
				helpUrl = behaviourHelp.url;
				helpTooltip = ToHelpTooltip(classTypeName);
			}
			else if (AttributeHelper.HasAttribute<BuiltInBehaviour>(type) || AttributeHelper.HasAttribute<BuiltInComponent>(type))
			{
				helpUrl = PathUtility.Combine(ArborReferenceUtility.docUrl, GetBuiltinReferenceDirectory(type), GetBuiltinReferenceFileName(type) + ".html");
				helpTooltip = ToHelpTooltip(classTypeName);
			}
			else if (type.IsSubclassOf(typeof(Calculator)))
			{
#pragma warning disable 0618
				CalculatorHelp calculatorHelp = AttributeHelper.GetAttribute<CalculatorHelp>(type);
				if (calculatorHelp != null)
				{
					helpUrl = calculatorHelp.url;
					helpTooltip = ToHelpTooltip(classTypeName);
				}
				else  if (AttributeHelper.HasAttribute<BuiltInCalculator>(type))
				{
					helpUrl = PathUtility.Combine(ArborReferenceUtility.docUrl, GetBuiltinReferenceDirectory(type), GetBuiltinReferenceFileName(type) + ".html");
					helpTooltip = ToHelpTooltip(classTypeName);
				}
#pragma warning restore 0618
			}
		}

		public bool HasHelp(Object obj)
		{
			return !string.IsNullOrEmpty(helpUrl) || Help.HasHelpForObject(obj);
		}

		public string GetHelpUrl(Object obj)
		{
			if (!string.IsNullOrEmpty(helpUrl))
			{
				return helpUrl;
			}

			if (Help.HasHelpForObject(obj))
			{
				return Help.GetHelpURLForObject(obj);
			}

			return null;
		}

		public string GetHelpTooltip(Object obj)
		{
			string tooltip = helpTooltip;

			if (string.IsNullOrEmpty(helpUrl) && Help.HasHelpForObject(obj))
			{
				string helpTopic = UnityEditorBridge.HelpBridge.GetNiceHelpNameForObject(obj);
				tooltip = ToHelpTooltip(helpTopic);
			}

			return tooltip;
		}

		static string GetObjectTypeName(System.Type type)
		{
			string titleName = type.Name;

			bool useNicifyName = true;

			BehaviourTitle behaviourTitle = AttributeHelper.GetAttribute<BehaviourTitle>(type);
			if (behaviourTitle != null)
			{
				if (behaviourTitle.localization)
				{
					titleName = Localization.GetWord(behaviourTitle.titleName);
				}
				else
				{
					titleName = behaviourTitle.titleName;
				}

				useNicifyName = behaviourTitle.useNicifyName;
			}
			else
			{
				if (type.IsSubclassOf(typeof(Calculator)))
				{
#pragma warning disable 0618
					CalculatorTitle calculatorTitle = AttributeHelper.GetAttribute<CalculatorTitle>(type);
					if (calculatorTitle != null)
					{
						titleName = calculatorTitle.titleName;
					}
#pragma warning restore 0618
				}
				else if (type.IsSubclassOf(typeof(VariableBase)))
				{
					titleName = VariableEditorUtility.GetVariableName(type);
				}
			}

			return useNicifyName ? ObjectNames.NicifyVariableName(titleName) : titleName;
		}

		public static string GetBehaviourMenu(System.Type type)
		{
			return GetBehaviourMenu(type, ArborSettings.currentLanguage);
		}

		private static string GetBehaviourMenu(System.Type type, SystemLanguage language)
		{
			AddBehaviourMenu behaviourMenu = AttributeHelper.GetAttribute<AddBehaviourMenu>(type);
			if (behaviourMenu != null)
			{
				if (behaviourMenu.localization)
				{
					return LanguageManager.GetWord(language, behaviourMenu.menuName);
				}

				return behaviourMenu.menuName;
			}

			if (type.IsSubclassOf(typeof(Calculator)))
			{
#pragma warning disable 0618
				AddCalculatorMenu calculatorMenu = AttributeHelper.GetAttribute<AddCalculatorMenu>(type);
				if (calculatorMenu != null)
				{
					return calculatorMenu.menuName;
				}
#pragma warning restore 0618
			}

			return "Scripts/" + type.Name;
		}

		public static string GetBuiltinReferenceFileName(System.Type type)
		{
			return type.Name.ToLower();
		}

		public static string GetBehaviourMenuDirectory(System.Type type)
		{
			if (type.IsSubclassOf(typeof(NodeBehaviour)))
			{
				return GetBehaviourMenu(type, SystemLanguage.English);
			}
			else if (type.IsSubclassOf(typeof(MonoBehaviour)))
			{
				AddComponentMenu componentMenu = AttributeHelper.GetAttribute<AddComponentMenu>(type);
				if (componentMenu != null)
				{
					return componentMenu.componentMenu;
				}
			}

			return "Scripts/" + type.Name;
		}

		public static readonly string ClassesDirecotryName = "Classes";

		public static string GetBuiltinReferenceDirectory(System.Type type)
		{
			string menuName = GetBehaviourMenuDirectory(type);

			string directory = PathUtility.GetDirectoryName(menuName);
			if (string.IsNullOrEmpty(directory))
			{
				directory = ClassesDirecotryName;
			}

			return PathUtility.Combine(ArborReferenceUtility.inspectorDirectoryName, BuiltinPathUtility.GetBuiltinPath(type), directory);
		}
	}
}