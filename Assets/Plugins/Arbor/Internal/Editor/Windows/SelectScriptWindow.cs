//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

using Arbor;

namespace ArborEditor
{
	public abstract class SelectScriptWindow<T> : SelectWindowBase where T : Object
	{
		private sealed class ScriptElement : Element
		{
			public System.Type classType;

			public ScriptElement(int level, string name, System.Type classType, Texture icon)
			{
				this.level = level;
				this.classType = classType;

				this.content = new GUIContent(name, icon);
			}
		}

		private sealed class ScriptMenuItem
		{
			public string menuName;
			public Texture icon;
			public System.Type classType;
		};

		private static class ScriptMenus
		{
			private static List<ScriptMenuItem> s_ScriptMenuItems = new List<ScriptMenuItem>();

			static ScriptMenus()
			{
				Build();

				ArborSettings.onChangedLanguage += Build;
			}

			static void Build()
			{
				s_ScriptMenuItems.Clear();

				var scriptTypes = ScriptsUtility.scriptTypes;
				for (int typeIndex = 0; typeIndex < scriptTypes.Count; typeIndex++)
				{
					System.Type classType = scriptTypes[typeIndex];

					if (classType.IsAbstract || AttributeHelper.HasAttribute<HideBehaviour>(classType))
					{
						continue;
					}

					string menuName = BehaviourInfo.GetBehaviourMenu(classType);
					if (string.IsNullOrEmpty(menuName))
					{
						continue;
					}

					ScriptMenuItem menuItem = new ScriptMenuItem();
					menuItem.classType = classType;
					menuItem.icon = Icons.GetTypeIcon(classType);
					menuItem.menuName = menuName;

					s_ScriptMenuItems.Add(menuItem);
				}

				s_ScriptMenuItems.Sort((a, b) =>
				{
					string[] aNames = a.menuName.Split('/');
					string[] bNames = b.menuName.Split('/');
					int i = 0;

					while (i < aNames.Length && i < bNames.Length)
					{
						int compare = 0;
						if (i + 1 >= aNames.Length || i + 1 >= bNames.Length)
						{
							compare = bNames.Length - aNames.Length;
						}
						if (compare == 0)
						{
							compare = aNames[i].CompareTo(bNames[i]);
						}
						if (compare != 0)
						{
							return compare;
						}
						i++;
					}
					return bNames.Length - aNames.Length;
				});
			}

			public static List<ScriptMenuItem> GetItems(System.Type targetClassType)
			{
				List<ScriptMenuItem> items = new List<ScriptMenuItem>();
				var scriptMenuItems = s_ScriptMenuItems;
				for (int scriptIndex = 0; scriptIndex < scriptMenuItems.Count; scriptIndex++)
				{
					ScriptMenuItem scriptMenuItem = scriptMenuItems[scriptIndex];
					if (scriptMenuItem == null || !scriptMenuItem.classType.IsSubclassOf(targetClassType))
					{
						continue;
					}

					items.Add(scriptMenuItem);
				}

				return items;
			}
		}

		protected override void OnCreateTree(TreeBuilder builder)
		{
			System.Type targetClassType = typeof(T);

			List<ScriptMenuItem> items = ScriptMenus.GetItems(targetClassType);

			int itemCount = items.Count;
			for (int itemIndex = 0; itemIndex < itemCount; itemIndex++)
			{
				ScriptMenuItem item = items[itemIndex];

				string menuName = item.menuName;
				builder.AddElement(menuName, (level, name) => new ScriptElement(level, name, item.classType, item.icon));
			}
		}

		protected abstract void OnSelect(System.Type classType);

		protected sealed override void OnSelect(Element element)
		{
			ScriptElement scriptElement = element as ScriptElement;
			if (scriptElement != null)
			{
				OnSelect(scriptElement.classType);
			}
		}

		protected sealed override bool OnBindHelpElement(VisualElement helpElement, Element item)
		{
			var scriptElement = item as ScriptElement;
			if (scriptElement != null)
			{
				BehaviourInfo behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(scriptElement.classType);
				string url = behaviourInfo.helpUrl;

				if (!string.IsNullOrEmpty(url))
				{
					helpElement.userData = url;
					helpElement.tooltip = behaviourInfo.helpTooltip;
					return true;
				}
			}

			return false;
		}
	}
}