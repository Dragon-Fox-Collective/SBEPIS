//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	internal sealed class CalculateMenuWindow : SelectScriptWindow<Calculator>
	{
		private static CalculateMenuWindow _Instance;

		public static CalculateMenuWindow instance
		{
			get
			{
				if (_Instance == null)
				{
					CalculateMenuWindow[] objects = Resources.FindObjectsOfTypeAll<CalculateMenuWindow>();
					if (objects.Length > 0)
					{
						_Instance = objects[0];
					}
				}
				if (_Instance == null)
				{
					_Instance = ScriptableObject.CreateInstance<CalculateMenuWindow>();
				}
				return _Instance;
			}
		}

		private Vector2 _Position;
		private System.Action<Vector2, System.Type> _OnSelect;

		protected override string searchWord
		{
			get
			{
				return ArborEditorCache.calculatorSearch;
			}

			set
			{
				ArborEditorCache.calculatorSearch = value;
			}
		}

		protected override string GetRootElementName()
		{
			return "Calculators";
		}

		protected override void OnSelect(System.Type classType)
		{
			_OnSelect?.Invoke(_Position, classType);
		}

		public void Init(Vector2 position, Rect buttonRect, System.Action<Vector2, System.Type> onSelect)
		{
			_Position = position;
			_OnSelect = onSelect;

			Open(buttonRect);
		}
	}
}
