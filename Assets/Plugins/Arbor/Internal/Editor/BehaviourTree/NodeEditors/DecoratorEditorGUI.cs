//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

namespace ArborEditor.BehaviourTree
{
	using Arbor.BehaviourTree;
	using ArborEditor.UIElements;
	using ArborEditor.UnityEditorBridge;

	[System.Serializable]
	internal sealed class DecoratorEditorGUI : TreeNodeBehaviourEditorGUI
	{
		protected override bool HasBehaviourEnable()
		{
			return true;
		}

		protected override bool GetBehaviourEnable()
		{
			Decorator decorator = behaviourObj as Decorator;
			if (decorator is object)
			{
				return decorator.behaviourEnabled;
			}
			return false;
		}

		protected override void SetBehaviourEnable(bool enable)
		{
			Decorator decorator = behaviourObj as Decorator;
			decorator.behaviourEnabled = enable;
		}

		static Color GetConditionColor(Decorator.Condition condition)
		{
			switch (condition)
			{
				case Decorator.Condition.None:
					return Color.gray;
				case Decorator.Condition.Success:
					return Color.green;
				case Decorator.Condition.Failure:
					return Color.red;
			}

			return Color.clear;
		}

		void MoveUpContextMenu()
		{
			if (treeBehaviourEditor != null)
			{
				treeBehaviourEditor.MoveDecorator(behaviourIndex, behaviourIndex - 1);
			}
		}

		void MoveDownContextMenu()
		{
			if (treeBehaviourEditor != null)
			{
				treeBehaviourEditor.MoveDecorator(behaviourIndex, behaviourIndex + 1);
			}
		}

		void DeleteContextMenu()
		{
			if (nodeEditor != null)
			{
				TreeBehaviourNodeEditor behaviourNodeEditor = nodeEditor as TreeBehaviourNodeEditor;
				behaviourNodeEditor.DestroyDecoratorAt(behaviourIndex);
			}
		}

		protected override void SetPopupMenu(GenericMenu menu)
		{
			bool editable = nodeEditor.graphEditor.editable;

			DecoratorList decoratorList = treeBehaviourNode.decoratorList;
			int decoratorCount = decoratorList.count;

			if (behaviourIndex >= 1 && editable)
			{
				menu.AddItem(EditorContents.moveUp, false, MoveUpContextMenu);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.moveUp);
			}

			if (behaviourIndex < decoratorCount - 1 && editable)
			{
				menu.AddItem(EditorContents.moveDown, false, MoveDownContextMenu);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.moveDown);
			}

			base.SetPopupMenu(menu);

			if (editable)
			{
				menu.AddItem(EditorContents.delete, false, DeleteContextMenu);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.delete);
			}
		}

		private VisualElement _TopElement;
		private VisualElement _ConditionSettingsElement;
		private VisualElement _ConditionStateElement;

		protected override VisualElement CreateTopElement()
		{
			_TopElement = new VisualElement();

			_ConditionSettingsElement = new VisualElement()
			{
				style =
				{
					flexDirection = FlexDirection.Row,
				}
			};
			_TopElement.Add(_ConditionSettingsElement);

			var abortField = new EnumFlagsField()
			{
				style =
				{
					width = 100f,
				}
			};
			abortField.tooltip = "Abort Flags";
			abortField.bindingPath = "_AbortFlags";
			_ConditionSettingsElement.Add(abortField);

#if ARBOR_DLL
			var unityEngineAssembly = System.Reflection.Assembly.Load("UnityEngine.dll");
			var enumFieldType = unityEngineAssembly.GetType("UnityEngine.UIElements.EnumField", false);
			if (enumFieldType == null)
			{
				var unityEditorAssembly = System.Reflection.Assembly.Load("UnityEditor.dll");
				enumFieldType = unityEditorAssembly.GetType("UnityEditor.UIElements.EnumField", false);
			}

			BindableElement logicalOperation = System.Activator.CreateInstance(enumFieldType) as BindableElement;
			logicalOperation.style.marginRight = 0f;
			logicalOperation.style.width = 50f;
			var inputUssClassNameField = enumFieldType.GetField("inputUssClassName");
			var inputUssClassName = (string)inputUssClassNameField.GetValue(null);
			var logicalInput = logicalOperation.Q(className: inputUssClassName);
#else
			BindableElement logicalOperation = new EnumField()
			{
				style =
				{
					marginRight = 0f,
					width = 50f,
				}
			};
			var logicalInput = logicalOperation.Q(className: EnumField.inputUssClassName);
#endif
			logicalInput.AddToClassList("button-left");
			logicalOperation.tooltip = "Logical Operation";
			logicalOperation.bindingPath = "_LogicalCondition.logicalOperation";
			_ConditionSettingsElement.Add(logicalOperation);

			var notToggle = new Toggle()
			{
				style =
				{
					marginLeft = 0f,
				}
			};
			notToggle.text = "Not";
			notToggle.RemoveFromClassList(Toggle.ussClassName);
			notToggle.AddToClassList(Button.ussClassName);
			notToggle.AddToClassList("button-right");
			notToggle.AddToClassList("toggle-button");
			notToggle.bindingPath = "_LogicalCondition.notOp";
			_ConditionSettingsElement.Add(notToggle);

			SetupConditionSettings();

			return _TopElement;
		}

		void SetupConditionSettings()
		{
			if (_ConditionSettingsElement == null)
			{
				return;
			}

			_ConditionSettingsElement.Unbind();

			var serializedObject = editor != null? editor.serializedObject : null;
			if (serializedObject != null)
			{
				_ConditionSettingsElement.Bind(serializedObject);
				_ConditionSettingsElement.EnableInClassList(InspectorElement.uIEInspectorVariantUssClassName, editor.UseDefaultMargins());
			}

			Decorator decorator = behaviourObj as Decorator;

			bool show = editor != null && decorator != null && GetExpanded() && decorator.HasConditionCheck();

			if (show)
			{
				if (_ConditionSettingsElement.parent == null)
				{
					_TopElement.Add(_ConditionSettingsElement);
				}
			}
			else
			{
				if (_ConditionSettingsElement.parent != null)
				{
					_ConditionSettingsElement.RemoveFromHierarchy();
				}
			}
		}

		protected override void OnChangedObject()
		{
			SetDecorator(behaviourObj as Decorator);

			SetupConditionSettings();
		}

		protected override void OnChangedExpanded()
		{
			SetupConditionSettings();
		}

		protected override VisualElement CreateUnderlayElement()
		{
			_ConditionStateElement = new VisualElement()
			{
				pickingMode = PickingMode.Ignore,
				style =
				{
					position = Position.Absolute,
					left = 0f,
					width = 5f,
					top = 0f,
					bottom = 0f,
					display = DisplayStyle.None,
				}
			};
			return _ConditionStateElement;
		}

		protected override void OnCreatedElement()
		{
			base.OnCreatedElement();

			var element = this.element;

			element.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			element.RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

			ShowConditionState(IsDrawConditionState());
		}

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

			SetDecorator(null);
		}

		void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			ShowConditionState(IsDrawConditionState());
		}

		void ShowConditionState(bool show)
		{
			if (show)
			{
				_ConditionStateElement.style.display = DisplayStyle.Flex;

				UpdateCondition();
			}
			else
			{
				_ConditionStateElement.style.display = DisplayStyle.None;
			}
		}

		Decorator _Decorator;

		void SetDecorator(Decorator decorator)
		{
			if (!ReferenceEquals(_Decorator, decorator))
			{
				if (_Decorator is object)
				{
					_Decorator.onConditionChanged -= OnConditionChanged;
				}

				_Decorator = decorator;

				if (_Decorator is object)
				{
					_Decorator.onConditionChanged += OnConditionChanged;
				}
			}
		}

		void OnConditionChanged()
		{
			UpdateCondition();
		}

		void UpdateCondition()
		{
			if (_ConditionStateElement != null)
			{
				Decorator decorator = behaviourObj as Decorator;

				Decorator.Condition condition = decorator.currentCondition;
				Color conditionColor = GetConditionColor(condition);

				_ConditionStateElement.style.backgroundColor = conditionColor;
			}		
		}

		bool IsDrawConditionState()
		{
			if (!Application.isPlaying)
			{
				return false;
			}

			Decorator decorator = behaviourObj as Decorator;
			if (!decorator.HasConditionCheck())
			{
				return false;
			}

			return true;
		}
	}
}