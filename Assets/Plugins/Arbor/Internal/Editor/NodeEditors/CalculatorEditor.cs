//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

using Arbor;

namespace ArborEditor
{
	using ArborEditor.UIElements;

	[CustomNodeEditor(typeof(CalculatorNode))]
	public sealed class CalculatorEditor : NodeEditor
	{
		public CalculatorNode calculatorNode
		{
			get
			{
				return node as CalculatorNode;
			}
		}

		CalculatorEditorGUI _BehaviourEditorGUI;

		public Editor editor
		{
			get
			{
				BehaviourEditorGUI behaviourEditor = GetBehaviourEditor();
				if (behaviourEditor != null)
				{
					return behaviourEditor.editor;
				}
				return null;
			}
		}

		public BehaviourEditorGUI GetBehaviourEditor()
		{
			if (node == null)
			{
				return null;
			}

			Object behaviourObj = calculatorNode.GetObject();

			if (behaviourObj is object && _BehaviourEditorGUI != null)
			{
				Editor editor = _BehaviourEditorGUI.editor;
				if (editor == null)
				{
					_BehaviourEditorGUI = null;
				}
				else if (behaviourObj != editor.target)
				{
					_BehaviourEditorGUI.Destroy();

					_BehaviourEditorGUI = null;
				}
			}

			if (_BehaviourEditorGUI == null)
			{
				_BehaviourEditorGUI = new CalculatorEditorGUI();
				_BehaviourEditorGUI.Initialize(this, behaviourObj);
			}
			else if (!ComponentUtility.IsValidObject(_BehaviourEditorGUI.behaviourObj))
			{
				_BehaviourEditorGUI.Repair(behaviourObj);
			}

			return _BehaviourEditorGUI;
		}

		public override void Validate(Node node, bool onInitialize)
		{
			base.Validate(node, onInitialize);

			if (_BehaviourEditorGUI != null)
			{
				if (!ComponentUtility.IsValidObject(_BehaviourEditorGUI.behaviourObj))
				{
					Object behaviourObj = calculatorNode.GetObject();
					_BehaviourEditorGUI.Repair(behaviourObj);
				}
				else if(onInitialize)
				{
					_BehaviourEditorGUI.RepairEditor();
				}
			}
		}

		public override Texture2D GetIcon()
		{
			Texture icon = EditorGUITools.GetThumbnailContent(calculatorNode.GetObject()).image;
			if (icon != null && !DefaultScriptIcon.IsDefaultScriptIcon(icon))
			{
				return icon as Texture2D;
			}
			return Icons.calculatorNodeIcon;
		}

		public override Styles.Color GetStyleColor()
		{
			return Styles.Color.Blue;
		}

		Texture2D GetHeaderOverlayIcon()
		{
			Calculator calculator = calculatorNode.calculator;
			if (calculator != null)
			{
				switch (calculator.recalculateMode)
				{
					case RecalculateMode.Dirty:
						break;
					case RecalculateMode.Frame:
						return Icons.frameCalculateIcon;
					case RecalculateMode.Scope:
						return Icons.scopeCalculateIcon;
					case RecalculateMode.Always:
						return Icons.alwaysCalculateIcon;
				}
			}

			return null;
		}

		private VisualElement _HeaderIconOverlay;
		private Image _HeaderOverlayIcon;

		protected override void OnCreatedHeaderIcon(VisualElement headerIcon)
		{
			_HeaderIconOverlay = new VisualElement();
			_HeaderIconOverlay.AddManipulator(new LocalizationManipulator("Recalculate Mode", LocalizationManipulator.TargetText.Tooltip));
			_HeaderIconOverlay.StretchToParentSize();
			_HeaderIconOverlay.RegisterCallback<MouseDownEvent>(e =>
			{
				GenericMenu menu = new GenericMenu();
				SetRecalculateModeMenu(menu, false);
				menu.DropDown(headerIcon.worldBound);
				e.StopPropagation();
			});
			headerIcon.Add(_HeaderIconOverlay);

			_HeaderOverlayIcon = new Image()
			{
				image = GetHeaderOverlayIcon(),
				style =
				{
					position = Position.Absolute,
					right = 0f,
					bottom = -2f,
				}
			};
			_HeaderIconOverlay.Add(_HeaderOverlayIcon);

			Image dropDownIcon = new Image()
			{
				image = RecalculateModeContents.iconDropDown,
				style =
				{
					position = Position.Absolute,
					left = -2f,
					bottom = -8f,
				}
			};
			_HeaderIconOverlay.Add(dropDownIcon);
		}

		protected override void OnUpdateStyles()
		{
			if (_HeaderOverlayIcon != null)
			{
				_HeaderOverlayIcon.image = GetHeaderOverlayIcon();
			}
		}

		void CreateEditor()
		{
			SetupResizable();
		}

		void DestroyEditor()
		{
			if (_BehaviourEditorGUI != null)
			{
				_BehaviourEditorGUI.Destroy();
				_BehaviourEditorGUI = null;
			}
		}

		protected override void OnInitialize()
		{
			CreateEditor();
		}

		internal void SetupResizable()
		{
			ICalculatorBehaviourEditor editor = this.editor as ICalculatorBehaviourEditor;
			if (editor != null)
			{
				isResizable = editor.IsResizableNode();
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			isShowableComment = true;
			isRenamable = true;

			if (editor != null)
			{
				SetupResizable();
			}

			_BehaviourEditorGUI?.DoEnable();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			_BehaviourEditorGUI?.DoDisable();
		}

		protected override float GetWidth()
		{
			ICalculatorBehaviourEditor editor = this.editor as ICalculatorBehaviourEditor;
			return (editor != null) ? editor.GetNodeWidth() : base.GetWidth();
		}

		protected override VisualElement CreateContentElement()
		{
			BehaviourEditorGUI behaviourEditor = GetBehaviourEditor();
			if (behaviourEditor != null)
			{
				var behaviourElement = behaviourEditor.element;
				if (behaviourElement == null)
				{
					behaviourElement = behaviourEditor.CreateElement();
				}

				if (behaviourEditor.overlayLayer.parent == null)
				{
					nodeElement.overlayLayer.Add(behaviourEditor.overlayLayer);
				}

				return behaviourElement;
			}

			return null;
		}

		protected override void OnUpdate()
		{
			_BehaviourEditorGUI?.Update();
		}

		protected override void OnRepainted()
		{
			_BehaviourEditorGUI?.OnRepainted();
		}

		protected override void OnDestroy()
		{
			DestroyEditor();

			base.OnDestroy();
		}

		public override bool IsCopyable()
		{
			return calculatorNode.calculator != null;
		}

		protected override void SetContextMenu(GenericMenu menu, Rect headerPosition, bool editable)
		{
			SetRecalculateModeMenu(menu, true);
		}

		public override void ExpandAll(bool expanded)
		{
			BehaviourEditorGUI behaviourEditor = GetBehaviourEditor();
			behaviourEditor?.SetExpanded(expanded);
		}

		void SetRecalculateMode(object value)
		{
			Calculator calculator = calculatorNode.GetObject() as Calculator;
			if (calculator != null)
			{
				var recalculateMode = (RecalculateMode)value;
				Undo.RecordObject(calculator, "Set Recalculate Mode");
				calculator.recalculateMode = recalculateMode;
				EditorUtility.SetDirty(calculator);
			}
		}

		internal bool SetRecalculateModeMenu(GenericMenu menu, bool fullPath)
		{
			Calculator calculator = calculatorNode.GetObject() as Calculator;
			if (calculator == null)
			{
				return false;
			}

			var recalculateMode = calculator.recalculateMode;
			menu.AddItem(RecalculateModeContents.Get(RecalculateMode.Dirty, fullPath), recalculateMode == RecalculateMode.Dirty, SetRecalculateMode, RecalculateMode.Dirty);
			menu.AddItem(RecalculateModeContents.Get(RecalculateMode.Frame, fullPath), recalculateMode == RecalculateMode.Frame, SetRecalculateMode, RecalculateMode.Frame);
			menu.AddItem(RecalculateModeContents.Get(RecalculateMode.Scope, fullPath), recalculateMode == RecalculateMode.Scope, SetRecalculateMode, RecalculateMode.Scope);
			menu.AddItem(RecalculateModeContents.Get(RecalculateMode.Always, fullPath), recalculateMode == RecalculateMode.Always, SetRecalculateMode, RecalculateMode.Always);

			return true;
		}

		static class RecalculateModeContents
		{
			public static GUIContent dirty;
			public static GUIContent frame;
			public static GUIContent scope;
			public static GUIContent always;

			public static GUIContent dirtyFull;
			public static GUIContent frameFull;
			public static GUIContent scopeFull;
			public static GUIContent alwaysFull;

			public static readonly Texture iconDropDown = null;
			public static string buttonTooltip;

			private static SystemLanguage _CurrentLanguage;

			static RecalculateModeContents()
			{
				GUIContent iconDropdownContent = EditorGUIUtility.IconContent("Icon Dropdown");
				iconDropDown = iconDropdownContent?.image;
				UpdateLocalization();

				ArborSettings.onChangedLanguage += OnChangedLanguage;
				LanguageManager.onRebuild += UpdateLocalization;
			}

			static void UpdateLocalization()
			{
				_CurrentLanguage = ArborSettings.currentLanguage;

				dirty = Localization.GetTextContent("Dirty");
				frame = Localization.GetTextContent("Frame");
				scope = Localization.GetTextContent("Scope");
				always = Localization.GetTextContent("Always");

				string recalculateModeText = Localization.GetWord("Recalculate Mode");

				dirtyFull = new GUIContent(dirty);
				dirtyFull.text = recalculateModeText + "/" + dirtyFull.text;

				frameFull = new GUIContent(frame);
				frameFull.text = recalculateModeText + "/" + frameFull.text;

				scopeFull = new GUIContent(scope);
				scopeFull.text = recalculateModeText + "/" + scopeFull.text;

				alwaysFull = new GUIContent(always);
				alwaysFull.text = recalculateModeText + "/" + alwaysFull.text;

				buttonTooltip = recalculateModeText;
			}

			public static GUIContent Get(RecalculateMode mode, bool getFull)
			{
				switch (mode)
				{
					case RecalculateMode.Dirty:
						return getFull ? dirtyFull : dirty;
					case RecalculateMode.Frame:
						return getFull ? frameFull : frame;
					case RecalculateMode.Scope:
						return getFull ? scopeFull : scope;
					case RecalculateMode.Always:
						return getFull ? alwaysFull : always;
				}

				return getFull ? dirtyFull : dirty;
			}

			static void OnChangedLanguage()
			{
				if (_CurrentLanguage != ArborSettings.currentLanguage)
				{
					UpdateLocalization();
				}
			}
		}
	}
}
