//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.UIElements
{
	internal sealed class BehaviourTitlebarElement : VisualElement
	{
		static readonly CustomStyleProperty<Color> s_ColorsBackgroundProperty = new CustomStyleProperty<Color>("--colors-behaviour_titlebar-background");
		static readonly CustomStyleProperty<Color> s_ColorsBackgroundHoverProperty = new CustomStyleProperty<Color>("--colors-behaviour_titlebar-background-hover");
		static readonly CustomStyleProperty<Color> s_ColorsBorderTopProperty = new CustomStyleProperty<Color>("--colors-behaviour_titlebar-border_top");
		static readonly CustomStyleProperty<Color> s_ColorsBorderBottomProperty = new CustomStyleProperty<Color>("--colors-behaviour_titlebar-border_bottom");

		private readonly BehaviourEditorGUI _BehaviourEditorGUI;

		private Foldout _Foldout;
		private Image _Icon;
		private Toggle _EnableToggle;
		private Label _Label;

		private Button _HelpButton;
		private MouseDownButton _PresetButton;
		private MouseDownButton _PopupButton;

		private ClickManipulator _ClickManipulator;

		private Color? _BackgroundColor = null;

		public Color? backgroundColor
		{
			get
			{
				return _BackgroundColor;
			}
			set
			{
				if (_BackgroundColor != value)
				{
					_BackgroundColor = value;

					UpdateBackgroundColor();
				}
			}
		}

		static class Defaults
		{
			public static readonly Texture presetIcon = EditorGUIUtility.FindTexture("Preset.Context");
		}

		public BehaviourTitlebarElement(BehaviourEditorGUI behaviourEditorGUI)
		{
			_BehaviourEditorGUI = behaviourEditorGUI;

			focusable = true;

			AddToClassList("behaviour-titlebar");

			_Foldout = new Foldout();
			SetFoldout(behaviourEditorGUI.GetExpanded());
			_Foldout.RegisterValueChangedCallback(OnChangedFoldout);
			Add(_Foldout);

			_Icon = new Image();
			_Icon.scaleMode = ScaleMode.ScaleToFit;
			_Icon.image = EditorGUITools.GetThumbnailContent(_BehaviourEditorGUI.behaviourObj).image;
			Add(_Icon);

			_EnableToggle = new Toggle();
			bool hasBehaviourEnable = _BehaviourEditorGUI.HasBehaviourEnable_Internal();
			_EnableToggle.visible = hasBehaviourEnable;
			if (hasBehaviourEnable)
			{
				_EnableToggle.SetValueWithoutNotify(_BehaviourEditorGUI.GetBehaviourEnable_Internal());
				_EnableToggle.RegisterValueChangedCallback(OnChangedEnableToggle);
			}

			Add(_EnableToggle);

			_Label = new Label();
			UIElementsUtility.SetBoldFont(_Label);
			UIElementsUtility.SetEllipsis(_Label);
			Add(_Label);

			var behaviourObj = _BehaviourEditorGUI.behaviourObj;
			_HelpButton = new Button(BrowseHelp);
			_HelpButton.RemoveFromClassList("unity-button");
			_HelpButton.AddToClassList("icon-button");
			_HelpButton.Add(new Image() { image = Icons.helpIcon });
			Add(_HelpButton);

			BehaviourInfo behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(behaviourObj);
			if (!behaviourInfo.HasHelp(behaviourObj))
			{
				_HelpButton.style.display = DisplayStyle.None;
			}

			_PresetButton = new MouseDownButton(OnClickPreset);
			_PresetButton.RemoveFromClassList("unity-button");
			_PresetButton.AddToClassList("icon-button");
			_PresetButton.Add(new Image() { image = Defaults.presetIcon });
			Add(_PresetButton);

			if (!Presets.PresetContextMenu.HasPresetButton(behaviourObj))
			{
				_PresetButton.style.display = DisplayStyle.None;
			}

			_PopupButton = new MouseDownButton(ShowContextMenu);
			_PopupButton.AddManipulator(new LocalizationManipulator("Settings", LocalizationManipulator.TargetText.Tooltip));
			_PopupButton.RemoveFromClassList("unity-button");
			_PopupButton.AddToClassList("icon-button");
			_PopupButton.Add(new Image() { image = Icons.popupIcon });
			Add(_PopupButton);

			this.AddManipulator(new ContextClickManipulator(OnContextClick));

			_ClickManipulator = new ClickManipulator(OnClick);
			_ClickManipulator.onChangedActive += isActive =>
			{
				EnableInClassList("behaviour-titlebar-active", isActive);
			};
			this.AddManipulator(_ClickManipulator);

			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

			RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResoleved);
			RegisterCallback<MouseEnterEvent>(OnMouseEnter);
			RegisterCallback<MouseLeaveEvent>(OnMouseLeave);
			RegisterCallback<KeyDownEvent>(OnKeyDown);

			SetupBehaviourInfo();
		}

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			var nodeEditor = _BehaviourEditorGUI.nodeEditor;
			var nodeElement = nodeEditor.nodeElement;

			nodeElement.RegisterCallback<UndoRedoPerformedEvent>(OnUndoRedoPerformed);

			BehaviourInfoUtility.onChanged -= SetupBehaviourInfo;
			BehaviourInfoUtility.onChanged += SetupBehaviourInfo;
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			var nodeEditor = _BehaviourEditorGUI.nodeEditor;
			var nodeElement = nodeEditor.nodeElement;

			nodeElement.UnregisterCallback<UndoRedoPerformedEvent>(OnUndoRedoPerformed);

			BehaviourInfoUtility.onChanged -= SetupBehaviourInfo;
		}

		void SetupBehaviourInfo()
		{
			var behaviourObj = _BehaviourEditorGUI.behaviourObj;
			BehaviourInfo behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(behaviourObj);
			_Label.text = behaviourInfo.titleContent.text;
			
			if (behaviourInfo.HasHelp(behaviourObj))
			{
				_HelpButton.style.display = StyleKeyword.Null;
				_HelpButton.tooltip = behaviourInfo.GetHelpTooltip(behaviourObj);
			}
			else
			{
				_HelpButton.style.display = DisplayStyle.None;
			}
		}

		void SetupBehaviour()
		{
			bool hasBehaviourEnable = _BehaviourEditorGUI.HasBehaviourEnable_Internal();
			if (hasBehaviourEnable)
			{
				_EnableToggle.SetValueWithoutNotify(_BehaviourEditorGUI.GetBehaviourEnable_Internal());
			}
		}

		void OnUndoRedoPerformed(UndoRedoPerformedEvent e)
		{
			SetupBehaviour();
		}

		void OnChangedFoldout(ChangeEvent<bool> e)
		{
			_BehaviourEditorGUI.SetExpandedInternal(e.newValue);
		}

		void OnChangedEnableToggle(ChangeEvent<bool> e)
		{
			bool behaviourEnabled = e.newValue;
			Undo.RecordObject(_BehaviourEditorGUI.behaviourObj, (!behaviourEnabled ? "Disable" : "Enable") + " Behaviour");
			_BehaviourEditorGUI.SetBehaviourEnable_Internal(behaviourEnabled);
			EditorUtility.SetDirty(_BehaviourEditorGUI.behaviourObj);
		}

		void OnCustomStyleResoleved(CustomStyleResolvedEvent e)
		{
			UpdateBackgroundColor();
		}

		internal void SetFoldout(bool expanded)
		{
			_Foldout.SetValueWithoutNotify(expanded);
		}

		void SetExpanded(bool expanded)
		{
			_BehaviourEditorGUI.SetExpanded(expanded);
		}

		void OnClick()
		{
			bool expanded = _BehaviourEditorGUI.GetExpanded();
			SetExpanded(!expanded);
		}

		void OnClickPreset()
		{
			Presets.PresetContextMenu.CreateAndShow(_BehaviourEditorGUI.behaviourObj, OnPresetChanged);
		}

		void OnPresetChanged()
		{
			IPresetAppliedReceiver editor = _BehaviourEditorGUI.editor as IPresetAppliedReceiver;
			editor?.OnPresetApplied();

			_BehaviourEditorGUI.nodeEditor.graphEditor.Repaint();
		}

		internal void OnRepair()
		{
			SetupBehaviourInfo();
		}

		internal void OnUpdate()
		{
			bool isEditorEnabled = _BehaviourEditorGUI.IsEditorEnabled();
			_Icon.SetEnabled(isEditorEnabled);
			_EnableToggle.SetEnabled(isEditorEnabled);
			
			SetupBehaviour();

			var behaviourObj = _BehaviourEditorGUI.behaviourObj;

			if (Presets.PresetContextMenu.HasPresetButton(behaviourObj))
			{
				_PresetButton.style.display = StyleKeyword.Null;
			}
			else
			{
				_PresetButton.style.display = DisplayStyle.None;
			}
		}

		private bool _IsHover = false;

		void OnMouseEnter(MouseEnterEvent e)
		{
			_IsHover = true;

			UpdateBackgroundColor();
		}

		void OnMouseLeave(MouseLeaveEvent e)
		{
			_IsHover = false;

			UpdateBackgroundColor();
		}

		void OnKeyDown(KeyDownEvent e)
		{
			switch (e.keyCode)
			{
				case KeyCode.LeftArrow:
					{
						SetExpanded(false);
						e.StopPropagation();
					}
					break;
				case KeyCode.RightArrow:
					{
						SetExpanded(true);
						e.StopPropagation();
					}
					break;
			}
		}

		void UpdateBackgroundColor()
		{
			if (_BackgroundColor.HasValue)
			{
				var backgroundColor = _BackgroundColor.Value;

				var styleBackgroundColor = backgroundColor;
				if (customStyle.TryGetValue((_IsHover && !_ClickManipulator.isActive) ? s_ColorsBackgroundHoverProperty : s_ColorsBackgroundProperty, out var customBackgroundColor))
				{
					styleBackgroundColor *= customBackgroundColor;
				}
				style.backgroundColor = styleBackgroundColor;

				var styleBorderTopColor = backgroundColor;
				if (customStyle.TryGetValue(s_ColorsBorderTopProperty, out var customBorderColor))
				{
					styleBorderTopColor *= customBorderColor;
				}
				style.borderTopColor = styleBorderTopColor;

				var styleBorderBottomColor = backgroundColor;
				if (customStyle.TryGetValue(s_ColorsBorderBottomProperty, out var customBorderBottomColor))
				{
					styleBorderBottomColor *= customBorderBottomColor;
				}
				style.borderBottomColor = styleBorderBottomColor;
			}
			else
			{
				style.borderTopColor = StyleKeyword.Null;
				style.borderBottomColor = StyleKeyword.Null;
				style.backgroundColor = StyleKeyword.Null;
			}
		}

		void ShowContextMenu()
		{
			GenericMenu menu = new GenericMenu();
			_BehaviourEditorGUI.SetContextMenu(menu);

			menu.DropDown(_PopupButton.worldBound);
		}

		void OnContextClick(ContextClickEvent e)
		{
			ShowContextMenu();

			e.StopPropagation();
		}

		void BrowseHelp()
		{
			var obj = _BehaviourEditorGUI.behaviourObj;

			BehaviourInfo behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(obj);

			string url = behaviourInfo.GetHelpUrl(obj);
			if (!string.IsNullOrEmpty(url))
			{
				Help.BrowseURL(url);
			}
		}

		sealed class ClickManipulator : DragManipulator
		{
			private System.Action _Handler;
			public ClickManipulator(System.Action handler)
			{
				_Handler = handler;
				activators.Add(new ManipulatorActivationFilter() { button = MouseButton.LeftMouse });
			}

			private Vector2 _BeginMousePosition;
			static readonly float kDragRange = 6f;

			protected override void OnMouseDown(MouseDownEvent e)
			{
				BehaviourTitlebarElement titlebarElement = target as BehaviourTitlebarElement;
				if (titlebarElement == null)
				{
					return;
				}

				_BeginMousePosition = e.localMousePosition;
				e.StopPropagation();
			}

			protected override void OnMouseMove(MouseMoveEvent e)
			{
				BehaviourTitlebarElement titlebarElement = target as BehaviourTitlebarElement;
				if (titlebarElement == null)
				{
					return;
				}

				if (Vector2.Distance(_BeginMousePosition, e.localMousePosition) > kDragRange)
				{
					// Unity 2019.4.34f1: DragAndDrop.Start Drag error countermeasures
					Event oldEvent = Event.current;
					if (e.imguiEvent != null && Event.current != e.imguiEvent)
					{
						Event.current = e.imguiEvent;
					}

					EndDrag();
					BehaviourDragInfo.BeginDragBehaviour(titlebarElement._BehaviourEditorGUI, 0);

					if (Event.current != oldEvent)
					{
						Event.current = oldEvent;
					}
				}
				e.StopPropagation();
			}

			protected override void OnMouseUp(MouseUpEvent e)
			{
				BehaviourTitlebarElement titlebarElement = target as BehaviourTitlebarElement;
				if (titlebarElement == null)
				{
					return;
				}

				Vector2 localPoint = (e.currentTarget as VisualElement).ChangeCoordinatesTo(titlebarElement, e.localMousePosition);
				if (titlebarElement.ContainsPoint(localPoint))
				{
					_Handler();
				}
			}
		}
	}
}