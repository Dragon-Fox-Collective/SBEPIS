//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace ArborEditor.UIElements
{
	internal sealed class TabPanel<TTabID> : VisualElement, INotifyValueChanged<TTabID>
	{
		public readonly VisualElement toolbar;
		public readonly VisualElement toolbarTabs;
		private readonly VisualElement _ContentContainer;
		private VisualElement _CurrentPanelElement;

		private TTabID _TabID;

		public override VisualElement contentContainer
		{
			get
			{
				return _ContentContainer;
			}
		}

		public TTabID value
		{
			get
			{
				return _TabID;
			}
			set
			{
				if (!_TabID.Equals(value))
				{
					using (var e = ChangeEvent<TTabID>.GetPooled(_TabID, value))
					{
						SetValueWithoutNotify(value);

						e.target = this;
						SendEvent(e);
					}
				}
			}
		}

		private class Tab
		{
			public ToolbarToggle toggle;
			public VisualElement panel;
		}

		private Dictionary<TTabID, Tab> _Tabs = new Dictionary<TTabID, Tab>();

		public TabPanel()
		{
			style.flexGrow = 1f;

			toolbar = new Toolbar()
			{
				style =
					{
						paddingLeft = 6f,
						paddingRight = 6f,
					}
			};
			hierarchy.Add(toolbar);

			toolbarTabs = new VisualElement()
			{
				style =
					{
						flexDirection = FlexDirection.Row,
					}
			};
			toolbar.Add(toolbarTabs);

			toolbar.Add(new ToolbarSpacer()
			{
				flex = true
			});

			_ContentContainer = new VisualElement()
			{
				style =
					{
						flexGrow = 1f,
					}
			};
			hierarchy.Add(_ContentContainer);
		}

		public void SetValueWithoutNotify(TTabID tabID)
		{
			if (_CurrentPanelElement != null)
			{
				_CurrentPanelElement.RemoveFromHierarchy();
				_CurrentPanelElement = null;
			}

			_TabID = tabID;

			_CurrentPanelElement = GetTab(tabID);
			if (_CurrentPanelElement != null)
			{
				_ContentContainer.Add(_CurrentPanelElement);
			}

			foreach (var pair in _Tabs)
			{
				pair.Value.toggle.SetValueWithoutNotify(pair.Key.Equals(tabID));
			}
		}

		public VisualElement CreateTab(string name, TTabID tabID)
		{
			var tab = new Tab();

			tab.panel = new VisualElement();
			tab.panel.StretchToParentSize();

			tab.toggle = new ToolbarToggle();
			tab.toggle.AddManipulator(new LocalizationManipulator(name, LocalizationManipulator.TargetText.Text));
			tab.toggle.RegisterValueChangedCallback(e =>
			{
				if (e.newValue)
				{
					value = tabID;
				}
				else if (e.previousValue)
				{
					tab.toggle.SetValueWithoutNotify(true);
				}
			});
			toolbarTabs.Add(tab.toggle);

			_Tabs.Add(tabID, tab);

			return tab.panel;
		}

		public VisualElement GetTab(TTabID tabID)
		{
			if (_Tabs.TryGetValue(tabID, out var tab))
			{
				return tab.panel;
			}

			return null;
		}
	}
}