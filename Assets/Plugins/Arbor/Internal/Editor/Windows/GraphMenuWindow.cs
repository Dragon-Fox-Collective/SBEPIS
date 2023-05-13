//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	internal class GraphMenuWindow : SelectScriptWindow<NodeGraph>
	{
		private static GraphMenuWindow _Instance;

		public static GraphMenuWindow instance
		{
			get
			{
				if (_Instance == null)
				{
					GraphMenuWindow[] objects = Resources.FindObjectsOfTypeAll<GraphMenuWindow>();
					if (objects.Length > 0)
					{
						_Instance = objects[0];
					}
				}
				if (_Instance == null)
				{
					_Instance = ScriptableObject.CreateInstance<GraphMenuWindow>();
				}
				return _Instance;
			}
		}

		protected override string searchWord
		{
			get
			{
				return ArborEditorCache.graphSearch;
			}
			set
			{
				ArborEditorCache.graphSearch = value;
			}
		}

		protected override string GetRootElementName()
		{
			return "Graphs";
		}

		protected override void OnSelect(Type classType)
		{
			_OnSelect?.Invoke(classType);
		}

		private Action<Type> _OnSelect;

		public void Init(Rect buttonRect, Action<Type> onSelect)
		{
			_OnSelect = onSelect;
			Open(buttonRect);
		}
	}
}