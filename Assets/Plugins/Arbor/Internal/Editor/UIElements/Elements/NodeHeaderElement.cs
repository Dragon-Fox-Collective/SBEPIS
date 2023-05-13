//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace ArborEditor.UIElements
{
	internal sealed class NodeHeaderElement : VisualElement
	{
		private Styles.Color _Color;
		public readonly Image iconElement;
		public readonly Label titleElement;
		private MouseDownButton _SettingElement;

		public Styles.Color color
		{
			get
			{
				return _Color;
			}
			set
			{
				if (_Color != value)
				{
					RemoveFromClassList(Styles.GetNodeHeaderClassName(_Color));

					_Color = value;

					AddToClassList(Styles.GetNodeHeaderClassName(_Color));
				}
			}
		}

		public Color backgroundColor
		{
			get
			{
				return resolvedStyle.unityBackgroundImageTintColor;
			}
			set
			{
				if (resolvedStyle.unityBackgroundImageTintColor != value)
				{
					style.unityBackgroundImageTintColor = value;
				}
			}
		}

		public Texture icon
		{
			get
			{
				return iconElement.image;
			}
			set
			{
				iconElement.image = value;
			}
		}

		public string title
		{
			get
			{
				return titleElement.text;
			}
			set
			{
				titleElement.text = value;
			}
		}

		private System.Action<Rect> _OnSettings = null;

		public System.Action<Rect> onSettings
		{
			get
			{
				return _OnSettings;
			}
			set
			{
				if (_OnSettings != value)
				{
					_OnSettings = value;

					if (_OnSettings != null)
					{
						if (_SettingElement == null)
						{
							_SettingElement = new MouseDownButton(() =>
							{
								_OnSettings?.Invoke(_SettingElement.parent.LocalToWorld(_SettingElement.layout));
							})
							{
								style =
								{
									top = -2f,
								}
							};
							_SettingElement.AddToClassList("node-header-popup-button");
							_SettingElement.AddManipulator(new LocalizationManipulator("Settings", LocalizationManipulator.TargetText.Tooltip));
							VisualElement popupButtonImage = new Image()
							{
								image = Icons.popupIcon,
							};
							_SettingElement.Add(popupButtonImage);
						}

						if (_SettingElement.parent == null)
						{
							Add(_SettingElement);
						}
					}
					else
					{
						if (_SettingElement != null && _SettingElement.parent != null)
						{
							_SettingElement.RemoveFromHierarchy();
						}
					}
				}
			}
		}

		public NodeHeaderElement()
		{
			pickingMode = PickingMode.Ignore;

			AddToClassList("node-header");
			AddToClassList(Styles.GetNodeHeaderClassName(_Color));

			iconElement = new Image();
			iconElement.AddToClassList("node-header-icon");
			Add(iconElement);

			titleElement = new Label();
			titleElement.AddToClassList("node-header-title");
			UIElementsUtility.SetBoldFont(titleElement);
			Add(titleElement);
		}
	}
}