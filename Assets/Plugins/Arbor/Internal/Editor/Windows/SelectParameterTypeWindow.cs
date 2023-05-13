//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArborEditor
{
	using Arbor;
	using UnityEngine.UIElements;

	internal sealed class SelectParameterTypeWindow : SelectWindowBase
	{
		private static SelectParameterTypeWindow _Instance;

		public static SelectParameterTypeWindow instance
		{
			get
			{
				if (_Instance == null)
				{
					SelectParameterTypeWindow[] objects = Resources.FindObjectsOfTypeAll<SelectParameterTypeWindow>();
					if (objects.Length > 0)
					{
						_Instance = objects[0];
					}
				}
				if (_Instance == null)
				{
					_Instance = ScriptableObject.CreateInstance<SelectParameterTypeWindow>();
				}
				return _Instance;
			}
		}

		private sealed class ParameterTypeElement : Element
		{
			public Parameter.Type parameterType;
			public Type variableType;
			public Type dataType;

			public ParameterTypeElement(int level, string name, Parameter.Type parameterType, Type variableType, Type dataType)
			{
				this.level = level;
				this.parameterType = parameterType;
				this.variableType = variableType;
				this.dataType = dataType;

				Texture icon = null;
				this.content = new GUIContent(name, icon);
			}
		}

		protected override string searchWord
		{
			get
			{
				return ArborEditorCache.parameterTypeSearch;
			}

			set
			{
				ArborEditorCache.parameterTypeSearch = value;
			}
		}

		protected override string GetRootElementName()
		{
			return "Parameters";
		}

		protected override void OnCreateTree(TreeBuilder builder)
		{
			for (int i = 0, count = ParameterTypeMenuItem.menuItems.Length; i < count; ++i)
			{
				var menuItem = ParameterTypeMenuItem.menuItems[i];

				if (menuItem.type == Parameter.Type.Variable)
				{
					var variableTypeInfos = VariableEditorUtility.s_VariableTypeInfos;
					for (int variableIndex = 0; variableIndex < variableTypeInfos.Count; variableIndex++)
					{
						var variableInfo = variableTypeInfos[variableIndex];

						string menuName = menuItem.menuName + "/" + variableInfo.menuName;

						if (!variableInfo.isSerializable)
						{
							menuName += " : not serializable";
						}

						builder.AddElement(menuName, (level, name) =>
						{
							var element = new ParameterTypeElement(level, name, Parameter.Type.Variable, variableInfo.variableType, variableInfo.dataType);
							if (!variableInfo.isSerializable)
							{
								element.disabled = true;
							}
							return element;
						});
					}
				}
				else if (menuItem.type == Parameter.Type.VariableList)
				{
					var variableListTypeInfos = VariableEditorUtility.s_VariableListTypeInfos;
					for (int variableIndex = 0; variableIndex < variableListTypeInfos.Count; variableIndex++)
					{
						var variableInfo = variableListTypeInfos[variableIndex];

						string menuName = menuItem.menuName + "/" + variableInfo.menuName;

						if (!variableInfo.isSerializable)
						{
							menuName += " : not serializable";
						}

						builder.AddElement(menuName, (level, name) =>
						{
							var element = new ParameterTypeElement(level, name, Parameter.Type.VariableList, variableInfo.variableType, variableInfo.dataType);
							if (!variableInfo.isSerializable)
							{
								element.disabled = true;
							}
							return element;
						});
					}
				}
				else
				{
					string menuName = menuItem.menuName;

					builder.AddElement(menuName, (level, name) => new ParameterTypeElement(level, name, menuItem.type, null, ParameterUtility.GetValueType(menuItem.type, null)));
				}
			}
		}

		protected sealed override void OnSelect(Element element)
		{
			if (element is ParameterTypeElement parameterTypeElement)
			{
				_OnSelect?.Invoke(parameterTypeElement.parameterType, parameterTypeElement.variableType);
			}
		}

		protected override void OnBindIconElement(Image iconElement, Element item)
		{
			if (item is ParameterTypeElement parameterTypeElement)
			{
				Type dataType = parameterTypeElement.dataType;

				iconElement.image = DataSlotGUIUtility.IsList(dataType)? Defaults.dataArrayPin : Defaults.dataPin;

				iconElement.tintColor = EditorGUITools.GetTypeColor(dataType);
			}
			else
			{
				iconElement.tintColor = Color.white;
			}
		}

		private Action<Parameter.Type, Type> _OnSelect;

		public void Init(Rect buttonRect, Action<Parameter.Type, Type> onSelect)
		{
			_OnSelect = onSelect;

			Open(buttonRect);
		}

		static class Defaults
		{
			public static readonly Texture dataPin;
			public static readonly Texture dataArrayPin;

			static Defaults()
			{
				dataPin = EditorResources.LoadTexture("Textures/node data pin on");
				dataArrayPin = EditorResources.LoadTexture("Textures/node data array pin on");
			}
		}
	}
}