//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.BehaviourTree
{
	using Arbor.BehaviourTree;

	internal sealed class DecoratorMenuWindow : SelectScriptWindow<Decorator>
	{
		private static DecoratorMenuWindow _Instance;

		public static DecoratorMenuWindow instance
		{
			get
			{
				if (_Instance == null)
				{
					DecoratorMenuWindow[] objects = Resources.FindObjectsOfTypeAll<DecoratorMenuWindow>();
					if (objects.Length > 0)
					{
						_Instance = objects[0];
					}
				}
				if (_Instance == null)
				{
					_Instance = ScriptableObject.CreateInstance<DecoratorMenuWindow>();
				}
				return _Instance;
			}
		}

		private int _Index = -1;
		private System.Action<int, System.Type> _OnSelect;

		protected override string searchWord
		{
			get
			{
				return ArborEditorCache.decoratorSearch;
			}

			set
			{
				ArborEditorCache.decoratorSearch = value;
			}
		}

		protected override string GetRootElementName()
		{
			return "Decorators";
		}

		protected override void OnSelect(System.Type classType)
		{
			_OnSelect?.Invoke(_Index, classType);
		}

		public void Init(Rect buttonRect, int index, System.Action<int, System.Type> onSelect)
		{
			_Index = index;
			_OnSelect = onSelect;

			Vector2 center = buttonRect.center;
			buttonRect.width = 300f;
			buttonRect.center = center;

			Open(buttonRect);
		}
	}
}