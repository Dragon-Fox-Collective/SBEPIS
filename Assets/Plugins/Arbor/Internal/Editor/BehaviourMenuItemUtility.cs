//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace ArborEditor
{
	using Arbor;

	internal static class BehaviourMenuItemUtility
	{
		private delegate bool DelegateValidContextEditorMenu(MenuCommand command);
		private delegate void DelegateContextEditorMenu(MenuCommand command);

		static List<Element> _Elements = new List<Element>();

		static BehaviourMenuItemUtility()
		{
			foreach (var methodInfo in TypeCache.GetMethodsWithAttribute<BehaviourMenuItem>())
			{
				if (!ValidateMethodForMenuCommand(methodInfo))
				{
					continue;
				}
				foreach (var attribute in methodInfo.GetCustomAttributes(typeof(BehaviourMenuItem), false))
				{
					var menuItem = attribute as BehaviourMenuItem;

					Element element = new Element();
					element.menuItem = menuItem;
					element.method = methodInfo;
					_Elements.Add(element);
				}
			}
		}

		public static void AddContextMenu(GenericMenu menu, Object obj)
		{
			if (obj == null)
			{
				return;
			}

			ContextEditorMenuElement[] editorContextMenus = ExtractEditorMenuItem(obj.GetType());
			ContextMenuElement[] contextMenus = ExtractContextMenu(obj);

			if (editorContextMenus.Length > 0 || contextMenus.Length > 0)
			{
				menu.AddSeparator("");
				if (editorContextMenus.Length > 0)
				{
					MenuCommand command = new MenuCommand(obj);
					for (int elementIndex = 0; elementIndex < editorContextMenus.Length; elementIndex++)
					{
						ContextEditorMenuElement element = editorContextMenus[elementIndex];
						if (element.method == null)
						{
							continue;
						}

						bool enable = true;
						if (element.validateMethod != null)
						{
							enable = element.validateMethod(command);
						}
						if (enable)
						{
							menu.AddItem(GUIContentCaches.Get(element.menuItem), false, ExecuteEditorContextMenu, new KeyValuePair<MenuCommand, ContextEditorMenuElement>(command, element));
						}
						else
						{
							menu.AddDisabledItem(GUIContentCaches.Get(element.menuItem));
						}
					}
				}
				if (contextMenus.Length > 0)
				{
					for (int menuIndex = 0; menuIndex < contextMenus.Length; menuIndex++)
					{
						ContextMenuElement element = contextMenus[menuIndex];
						if (element.method != null)
						{
							menu.AddItem(GUIContentCaches.Get(element.menuItem), false, ExecuteContextMenu, element);
						}
					}
				}
			}
		}

		private static bool ValidateMethodForMenuCommand(MethodInfo methodInfo)
		{
			if (methodInfo.DeclaringType.IsGenericType)
			{
				Debug.LogWarningFormat("Method {0}.{1} cannot be used for menu commands because class {0} is an open generic type.", methodInfo.DeclaringType, methodInfo.Name);
				return false;
			}
			// Skip non-static methods for regular menus
			if (!methodInfo.IsStatic)
			{
				Debug.LogWarningFormat("Method {0}.{1} is not static and cannot be used for menu commands.", methodInfo.DeclaringType, methodInfo.Name);
				return false;
			}
			// Skip generic methods
			if (methodInfo.IsGenericMethod)
			{
				Debug.LogWarningFormat("Method {0}.{1} is generic and cannot be used for menu commands.", methodInfo.DeclaringType, methodInfo.Name);
				return false;
			}
			// Skip invalid methods
			ParameterInfo[] parameters = methodInfo.GetParameters();
			if (parameters.Length > 1 || parameters.Length == 0)
			{
				Debug.LogWarningFormat("Method {0}.{1} has invalid parameters. MenuCommand is the only optional supported parameter.", methodInfo.DeclaringType, methodInfo.Name);
				return false;
			}
			if (parameters.Length == 1)
			{
				if (parameters[0].ParameterType != typeof(MenuCommand))
				{
					Debug.LogWarningFormat("Method {0}.{1} has invalid parameters. MenuCommand is the only optional supported parameter.", methodInfo.DeclaringType, methodInfo.Name);
					return false;
				}
			}
			return true;
		}

		static ContextEditorMenuElement[] ExtractEditorMenuItem(System.Type behaviourType)
		{
			Dictionary<string, ContextEditorMenuElement> dic = new Dictionary<string, ContextEditorMenuElement>();

			var menuElements = _Elements;
			for (int elementIndex = 0; elementIndex < menuElements.Count; elementIndex++)
			{
				Element element = menuElements[elementIndex];
				if (element.menuItem.type != behaviourType && !behaviourType.IsSubclassOf(element.menuItem.type))
				{
					continue;
				}

				ContextEditorMenuElement menuEelement = dic.ContainsKey(element.menuItem.menuItem) ? dic[element.menuItem.menuItem] : new ContextEditorMenuElement();
				if (element.menuItem.localization)
				{
					menuEelement.menuItem = Localization.GetWord(element.menuItem.menuItem);
				}
				else
				{
					menuEelement.menuItem = element.menuItem.menuItem;
				}
				if (element.menuItem.validate)
				{
					menuEelement.validateMethod = (DelegateValidContextEditorMenu)System.Delegate.CreateDelegate(typeof(DelegateValidContextEditorMenu), element.method, false);
				}
				else
				{
					menuEelement.method = (DelegateContextEditorMenu)System.Delegate.CreateDelegate(typeof(DelegateContextEditorMenu), element.method, false);
					menuEelement.index = elementIndex;
					menuEelement.priority = element.menuItem.priority;
				}

				dic[element.menuItem.menuItem] = menuEelement;
			}

			ContextEditorMenuElement[] elements = dic.Values.ToArray();
			System.Array.Sort(elements, new CompareEditorMenuIndex());

			return elements;
		}

		static ContextMenuElement[] ExtractContextMenu(Object obj)
		{
			System.Type type = obj.GetType();

			Dictionary<string, ContextMenuElement> dic = new Dictionary<string, ContextMenuElement>();

			MethodInfo[] methods = type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
			for (int index = 0; index < methods.Length; ++index)
			{
				MethodInfo method = methods[index];
				var contextMenus = AttributeHelper.GetAttributes<ContextMenu>(method);
				for (int i = 0; i < contextMenus.Length; i++)
				{
					ContextMenu contextMenu = contextMenus[i];
					ContextMenuElement element = dic.ContainsKey(contextMenu.menuItem) ? dic[contextMenu.menuItem] : new ContextMenuElement();
					element.menuItem = contextMenu.menuItem;
					element.method = (System.Action)System.Delegate.CreateDelegate(typeof(System.Action), obj, method, false);
					element.index = 0;
					element.priority = 0;
					dic[contextMenu.menuItem] = element;
				}
			}

			return dic.Values.ToArray();
		}

		static void ExecuteContextMenu(object obj)
		{
			ContextMenuElement contextMenu = (ContextMenuElement)obj;

			contextMenu.method?.Invoke();
		}

		static void ExecuteEditorContextMenu(object obj)
		{
			KeyValuePair<MenuCommand, ContextEditorMenuElement> pair = (KeyValuePair<MenuCommand, ContextEditorMenuElement>)obj;
			MenuCommand command = pair.Key;
			ContextEditorMenuElement contextMenu = pair.Value;

			contextMenu.method?.Invoke(command);
		}

		private sealed class Element
		{
			public BehaviourMenuItem menuItem;
			public MethodInfo method;
		}

		struct ContextMenuElement
		{
			public string menuItem;
			public System.Action method;
			public int index;
			public int priority;
		}

		struct ContextEditorMenuElement
		{
			public string menuItem;
			public DelegateContextEditorMenu method;
			public DelegateValidContextEditorMenu validateMethod;
			public int index;
			public int priority;
		}

		private sealed class CompareEditorMenuIndex : IComparer<ContextEditorMenuElement>
		{
			public int Compare(ContextEditorMenuElement element1, ContextEditorMenuElement element2)
			{
				if (element1.priority != element2.priority)
					return element1.priority.CompareTo(element2.priority);
				return element1.index.CompareTo(element2.index);
			}
		}
	}
}
