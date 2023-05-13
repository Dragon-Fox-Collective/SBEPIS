//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;


namespace ArborEditor
{
	using Arbor;
	using Arbor.Playables;
	using ArborEditor.UIElements;

	[CustomNodeEditor(typeof(State))]
	public sealed class StateEditor : NodeEditor
	{
		internal static class Types
		{
			public static readonly System.Type SetParameterBehaviourType;
			public static readonly System.Type[] SetParameterBehaviourTypes;

			static Types()
			{
				SetParameterBehaviourType = AssemblyHelper.GetTypeByName("Arbor.ParameterBehaviours.SetParameterBehaviour");
				SetParameterBehaviourTypes = new System.Type[] { SetParameterBehaviourType };
			}
		}

		public State state
		{
			get
			{
				return node as State;
			}
		}

		private StateBehaviourEditorList _BehaviourList = new StateBehaviourEditorList();

		protected override void OnInitialize()
		{
			_BehaviourList.nodeEditor = this;
			CreateEditors();
		}

		protected override void OnAttachNode(Node node)
		{
			base.OnAttachNode(node);

			State state = node as State;
			if (state != null)
			{
				StateCallback.RegisterTransitionCountCallback(state, OnChangedTransitionCount);
			}
		}

		protected override void OnDetachNode(Node node)
		{
			base.OnDetachNode(node);

			State state = node as State;
			if (state != null)
			{
				StateCallback.UnregisterTranstiionCountCallback(state, OnChangedTransitionCount);
			}
		}

		void OnChangedTransitionCount()
		{
			if (_TransitionCountElement != null)
			{
				_TransitionCountElement.count = state.transitionCount;
			}
		}

		protected override void RegisterCallbackOnElement()
		{
			base.RegisterCallbackOnElement();

			nodeElement.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			nodeElement.RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
			nodeElement.RegisterCallback<RebuildElementEvent>(OnRebuildElement);
			nodeElement.RegisterCallback<ChangeNodePositionEvent>(OnChangeNodePosition);
			nodeElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
		}

		void OnGeometryChanged(GeometryChangedEvent e)
		{
			var stateMachineGraphEditor = graphEditor as StateMachineGraphEditor;
			stateMachineGraphEditor.UpdateStateLinkTargetPosition(nodeID);

			UpdateTransitionCountPosition();
		}

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

			EditorApplication.pauseStateChanged -= OnPauseStateChanged;
			EditorApplication.pauseStateChanged += OnPauseStateChanged;

			ShowBreakPoint(state.breakPoint);
			UpdateBreakOn();
			ShowTransitionCount(Application.isPlaying);
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

			EditorApplication.pauseStateChanged -= OnPauseStateChanged;
		}

		void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			ShowTransitionCount(Application.isPlaying);
		}

		void OnPauseStateChanged(PauseState pauseState)
		{
			UpdateBreakOn();
		}

		void UpdateBreakOn()
		{
			if (state.breakPoint && _BreakPointElement != null)
			{
				_BreakPointElement.breakOn = Application.isPlaying && EditorApplication.isPaused && state.stateMachine.currentState == state;
			}
		}

		void ShowTransitionCount(bool show)
		{
			if (show)
			{
				if (_TransitionCountElement == null)
				{
					_TransitionCountElement = new CountBadgeElement();
				}

				if (_TransitionCountElement.parent == null)
				{
					nodeElement.overlayLayer.Add(_TransitionCountElement);
					UpdateTransitionCountPosition();
					OnChangedTransitionCount();
				}
			}
			else
			{
				if (_TransitionCountElement != null && _TransitionCountElement.parent != null)
				{
					_TransitionCountElement.RemoveFromHierarchy();
				}
			}
		}

		void OnRebuildElement(RebuildElementEvent e)
		{
			_BehaviourList.RebuildBehaviourEditors();
			UpdateStateLinkEditor();

			ShowBreakPoint(state.breakPoint);
			ShowTransitionCount(Application.isPlaying);
		}

		internal void UpdateStateLinkTargetPosition(int nodeID)
		{
			for (int behaviourIndex = 0, behaviourCount = _BehaviourList.GetCount(); behaviourIndex < behaviourCount; behaviourIndex++)
			{
				var behaviourEditor = _BehaviourList.GetBehaviourEditor(behaviourIndex);
				behaviourEditor.UpdateStateLinkTargetPosition(nodeID);
			}
		}

		void OnChangeNodePosition(ChangeNodePositionEvent e)
		{
			var stateMachineGraphEditor = graphEditor as StateMachineGraphEditor;
			stateMachineGraphEditor.UpdateStateLinkTargetPosition(nodeID);
		}

		void CreateEditors()
		{
			State state = this.state;
			if (state != null)
			{
				_BehaviourList.RebuildBehaviourEditors();
				UpdateStateLinkEditor();
			}
		}

		private BreakPointElement _BreakPointElement = null;
		private CountBadgeElement _TransitionCountElement = null;

		public void ShowBreakPoint(bool show)
		{
			if (show)
			{
				if (_BreakPointElement == null)
				{
					_BreakPointElement = new BreakPointElement();
				}

				if (_BreakPointElement.parent == null)
				{
					nodeElement.overlayLayer.Add(_BreakPointElement);
					UpdateBreakOn();
				}
			}
			else
			{
				if (_BreakPointElement != null && _BreakPointElement.parent != null)
				{
					_BreakPointElement.RemoveFromHierarchy();
				}
			}
		}

		public StateBehaviourEditorGUI GetBehaviourEditor(int behaviourIndex)
		{
			return _BehaviourList.GetBehaviourEditor(behaviourIndex);
		}

		public void MoveBehaviour(int fromIndex, int toIndex)
		{
			ArborFSMInternal stateMachine = state.stateMachine;

			Undo.IncrementCurrentGroup();

			string undoName = (fromIndex > toIndex) ? "MoveUp Behaviour" : "MoveDown Behaviour";

			Undo.RecordObject(stateMachine, undoName);

			state.SwapBehaviour(fromIndex, toIndex);

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			EditorUtility.SetDirty(stateMachine);

			_BehaviourList.MoveBehaviourEditor(fromIndex, toIndex);
		}

		public void RemoveBehaviour(int behaviourIndex)
		{
			RemoveBehaviourEditor(behaviourIndex);

			Undo.IncrementCurrentGroup();
			int undoGruop = Undo.GetCurrentGroup();

			state.DestroyBehaviourAt(behaviourIndex);

			Undo.CollapseUndoOperations(undoGruop);

			graphEditor.RaiseOnChangedNodes();
		}

		internal void RemoveBehaviourEditor(int behaviourIndex)
		{
			_BehaviourList.RemoveBehaviourEditor(behaviourIndex);
		}

		protected override void OnDestroy()
		{
			_BehaviourList.DestroyEditors();

			base.OnDestroy();
		}

		public override void Validate(Node node, bool onInitialize)
		{
			base.Validate(node, onInitialize);

			_BehaviourList.Validate(onInitialize);

			UpdateStateLinkEditor();
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			_BehaviourList.nodeEditor = this;

			isRenamable = true;
			isShowableComment = true;

			_BehaviourList.OnEnable();

			ArborSettings.onChangedStateLinkShowMode += UpdateStateLinkShowMode;
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			_BehaviourList.OnDisable();

			ArborSettings.onChangedStateLinkShowMode -= UpdateStateLinkShowMode;
		}

		public override void ExpandAll(bool expanded)
		{
			int behaviourCount = state.behaviourCount;
			if (behaviourCount > 0)
			{
				for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
				{
					StateBehaviourEditorGUI behaviourEditor = GetBehaviourEditor(behaviourIndex);
					behaviourEditor?.SetExpanded(expanded);
				}
			}
		}

		void ExpandAll()
		{
			ExpandAll(true);
		}

		void FoldAll()
		{
			ExpandAll(false);
		}

		protected override void SetContextMenu(GenericMenu menu, Rect headerPosition, bool editable)
		{
			State state = this.state;
			SerializedObject serializedObject = new SerializedObject(state.stateMachine);

			SerializedProperty startStateIDPropery = serializedObject.FindProperty("_StartStateID");

			if (!state.resident)
			{
				if (startStateIDPropery.intValue != state.nodeID && editable)
				{
					menu.AddItem(EditorContents.setStartState, false, SetStartStateContextMenu);
				}
				else
				{
					menu.AddDisabledItem(EditorContents.setStartState);
				}

				if (editable)
				{
					menu.AddItem(EditorContents.breakPoint, state.breakPoint, FlipStateBreakPoint);
				}
				else
				{
					menu.AddDisabledItem(EditorContents.breakPoint);
				}
			}

			if (menu.GetItemCount() > 0)
			{
				menu.AddSeparator("");
			}

			if (editable)
			{
				menu.AddItem(EditorContents.addBehaviour, false, AddBehaviourToStateContextMenu, GUIUtility.GUIToScreenRect(headerPosition));
			}
			else
			{
				menu.AddDisabledItem(EditorContents.addBehaviour);
			}

			if (Clipboard.CompareBehaviourType(typeof(StateBehaviour), true) && editable)
			{
				menu.AddItem(EditorContents.pasteBehaviour, false, PasteBehaviourToStateContextMenu);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.pasteBehaviour);
			}

			menu.AddSeparator("");

			menu.AddItem(EditorContents.expandAll, false, ExpandAll);

			menu.AddItem(EditorContents.collapseAll, false, FoldAll);
		}

		protected override void SetDebugContextMenu(GenericMenu menu, Rect headerPosition, bool editable)
		{
			State state = this.state;

			if (!state.resident)
			{
				if (menu.GetItemCount() > 0)
				{
					menu.AddSeparator("");
				}
				if (Application.isPlaying && editable)
				{
					menu.AddItem(EditorContents.transition, false, TransitionState);
				}
				else
				{
					menu.AddDisabledItem(EditorContents.transition);
				}
			}
		}

		void SetStartStateContextMenu()
		{
			SerializedObject serializedObject = new SerializedObject(state.stateMachine);

			serializedObject.Update();

			SerializedProperty startStateIDPropery = serializedObject.FindProperty("_StartStateID");

			startStateIDPropery.intValue = state.nodeID;

			serializedObject.ApplyModifiedProperties();

			serializedObject.Dispose();
		}

		void FlipStateBreakPoint()
		{
			ArborFSMInternal stateMachine = state.stateMachine;

			if (state.breakPoint)
			{
				Undo.RecordObject(stateMachine, "State BreakPoint Off");
			}
			else
			{
				Undo.RecordObject(stateMachine, "State BreakPoint On");
			}

			state.breakPoint = !state.breakPoint;

			EditorUtility.SetDirty(stateMachine);

			ShowBreakPoint(state.breakPoint);

			graphEditor.RaiseOnChangedNodes();
		}

		void TransitionState()
		{
			ArborFSMInternal stateMachine = state.stateMachine;

			stateMachine.Transition(state);
		}

		internal static void PasteStateBehaviourAsNew(State state, int index)
		{
			PasteStateBehaviourAsNew(state, index, Clipboard.copyBehaviour);
		}

		internal static StateBehaviour PasteStateBehaviourAsNew(State state, int index, NodeBehaviour destBehaviour)
		{
			using (new Presets.DisableApplyDefaultPresetScope(true))
			{
				StateBehaviour behaviour = NodeBehaviour.CreateNodeBehaviour(state, destBehaviour.GetType(), true) as StateBehaviour;

				if (index != -1)
				{
					state.InsertBehaviour(index, behaviour);
				}
				else
				{
					state.AddBehaviour(behaviour);
				}

				Clipboard.CopyNodeBehaviour(destBehaviour, behaviour, true);

				return behaviour;
			}
		}

		public void PasteBehaviour(int index)
		{
			Undo.IncrementCurrentGroup();

			ArborFSMInternal stateMachine = state.stateMachine;

			Undo.RecordObject(stateMachine, "Paste Behaviour");

			PasteStateBehaviourAsNew(state, index);

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			EditorUtility.SetDirty(stateMachine);

			graphEditor.RaiseOnChangedNodes();
		}

		void PasteBehaviourToStateContextMenu()
		{
			PasteBehaviour(-1);
		}

		internal void InsertBehaviourEditor(int index, Object behaviourObj)
		{
			var editorGUI = _BehaviourList.InsertBehaviourEditor(index, behaviourObj);

			if (editorGUI.behaviourIndex != index)
			{
				Debug.LogWarning("editorGUI.behaviourIndex != index");
				editorGUI.behaviourIndex = index;
			}

			editorGUI.UpdateStateLinkShowMode();

			graphEditor.RaiseOnChangedNodes();
		}

		public StateBehaviour InsertBehaviour(int index, System.Type classType)
		{
			var behaviourObj = state.InsertBehaviour(index, classType);

			InsertBehaviourEditor(index, behaviourObj);

			return behaviourObj;
		}

		public StateBehaviour AddBehaviour(System.Type classType)
		{
			State state = this.state;

			var behaviourObj = state.AddBehaviour(classType);

			int index = state.behaviourCount - 1;
			InsertBehaviourEditor(index, behaviourObj);

			return behaviourObj;
		}

		public void InsertSetParameterBehaviour(int index, Parameter parameter)
		{
			Arbor.ParameterBehaviours.SetParameterBehaviourInternal setParameterBehaviour = InsertBehaviour(index, Types.SetParameterBehaviourType) as Arbor.ParameterBehaviours.SetParameterBehaviourInternal;

			Undo.RecordObject(setParameterBehaviour, "Insert Behaviour");

			setParameterBehaviour.SetParameter(parameter);

			EditorUtility.SetDirty(setParameterBehaviour);
		}

		public void AddSetParameterBehaviour(Parameter parameter)
		{
			Arbor.ParameterBehaviours.SetParameterBehaviourInternal setParameterBehaviour = AddBehaviour(Types.SetParameterBehaviourType) as Arbor.ParameterBehaviours.SetParameterBehaviourInternal;

			Undo.RecordObject(setParameterBehaviour, "Add Behaviour");

			setParameterBehaviour.SetParameter(parameter);

			EditorUtility.SetDirty(setParameterBehaviour);
		}

		void AddBehaviourToStateContextMenu(object obj)
		{
			Rect position = (Rect)obj;

			BehaviourMenuWindow.instance.Init(position, -1, CreateStateBehaviourByType);
		}

		internal void CreateStateBehaviourByType(int index, System.Type type)
		{
			if (index != -1)
			{
				InsertBehaviour(index, type);
			}
			else
			{
				AddBehaviour(type);
			}
		}

		void UpdateTransitionCountPosition()
		{
			if (Application.isPlaying && _TransitionCountElement != null)
			{
				_TransitionCountElement.attachPoint = new Vector2(nodeElement.layout.width, 0f);
			}
		}

		protected override void OnUpdate()
		{
			_BehaviourList.Update();
		}

		protected override void OnRepainted()
		{
			_BehaviourList.OnRepainted();
		}

		private VisualElement _RootContainer = null;
		internal VisualElement _StateLinkListElement;
		private VisualElement _BehavioursGUIElement;

		void UpdateStateLinkShowMode()
		{
			if (_RootContainer == null)
			{
				return;
			}

			if (ArborSettings.stateLinkShowMode == StateLinkShowMode.NodeTop)
			{
				if (_StateLinkListElement.parent == null)
				{
					_RootContainer.Add(_StateLinkListElement);
				}

				_StateLinkListElement.SendToBack();
			}
			else if (ArborSettings.stateLinkShowMode == StateLinkShowMode.NodeBottom)
			{
				if (_StateLinkListElement.parent == null)
				{
					_RootContainer.Add(_StateLinkListElement);
				}

				_StateLinkListElement.BringToFront();
			}
			else
			{
				if (_StateLinkListElement != null && _StateLinkListElement.parent != null)
				{
					_StateLinkListElement.RemoveFromHierarchy();
				}
			}

			UpdateStateLinkEditor();

			IMGUIContainer imguiContainer = _BehavioursGUIElement as IMGUIContainer;
			imguiContainer?.MarkDirtyLayout();
			_BehavioursGUIElement.MarkDirtyRepaint();
		}

		void UpdateStateLinkEditor()
		{
			int behaviourCount = state.behaviourCount;
			if (behaviourCount > 0)
			{
				for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
				{
					StateBehaviourEditorGUI behaviourEditor = GetBehaviourEditor(behaviourIndex);
					if (behaviourEditor != null)
					{
						if (behaviourEditor.behaviourIndex != behaviourIndex)
						{
							Debug.LogWarning("behaviourEditor.behaviourIndex != behaviourIndex");
							behaviourEditor.behaviourIndex = behaviourIndex;
						}

						behaviourEditor.UpdateStateLinkShowMode();
					}
				}
			}
		}

		protected override VisualElement CreateContentElement()
		{
			_RootContainer = new VisualElement();
			_StateLinkListElement = new VisualElement();

			_BehavioursGUIElement = _BehaviourList.CreateElement();
			_RootContainer.Add(_BehavioursGUIElement);

			UpdateStateLinkShowMode();

			return _RootContainer;
		}

		protected override void OnHeaderDragEvent(IDragAndDropEvent e)
		{
			EventBase eventBase = e as EventBase;
			if (eventBase == null)
			{
				return;
			}

			if (eventBase is DragUpdatedEvent || eventBase is DragPerformEvent)
			{
				if (DragHeader(eventBase is DragPerformEvent))
				{
					eventBase.StopPropagation();
				}
			}
		}

		bool DragHeader(bool isPerform)
		{
			bool used = false;
			bool editable = graphEditor.editable;
			bool isAccepted = false;
			var objectReferences = DragAndDrop.objectReferences;
			for (int objIndex = 0; objIndex < objectReferences.Length; objIndex++)
			{
				Object draggedObject = objectReferences[objIndex];
				MonoScript script = draggedObject as MonoScript;
				if (script != null)
				{
					System.Type classType = script.GetClass();

					if (classType != null && classType.IsSubclassOf(typeof(StateBehaviour)))
					{
						if (editable)
						{
							DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

							if (isPerform)
							{
								AddBehaviour(classType);
								isAccepted = true;
							}
						}
						else
						{
							DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
						}

						used = true;
					}
				}
				else
				{
					ParameterDraggingObject parameterDraggingObject = draggedObject as ParameterDraggingObject;
					if (parameterDraggingObject != null)
					{
						if (editable)
						{
							DragAndDrop.visualMode = DragAndDropVisualMode.Link;

							if (isPerform)
							{
								AddSetParameterBehaviour(parameterDraggingObject.parameter);

								isAccepted = true;
							}
						}
						else
						{
							DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
						}

						used = true;
					}
				}
			}

			if (isAccepted)
			{
				DragAndDrop.AcceptDrag();
				DragAndDrop.activeControlID = 0;
			}

			return used;
		}

		public override bool IsDraggingVisible()
		{
			int behaviourCount = state.behaviourCount;

			for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
			{
				StateBehaviourEditorGUI behaviourEditor = GetBehaviourEditor(behaviourIndex);

				if (behaviourEditor == null)
				{
					continue;
				}

				if (behaviourEditor.IsDraggingVisible())
				{
					return true;
				}
			}

			return false;
		}

		private static readonly Styles.Color s_DragColor = Styles.Color.Red;
		private static readonly Styles.Color s_CurrentColor = Styles.Color.Orange;
		private static readonly Styles.Color s_ReservedColor = Styles.Color.Purple;
		private static readonly Styles.Color s_StartColor = Styles.Color.Aqua;
		private static readonly Styles.Color s_ResidentColor = Styles.Color.Green;
		private static readonly Styles.Color s_NormalColor = Styles.Color.Gray;

		public override bool IsActive()
		{
			ArborFSMInternal stateMachine = state.stateMachine;
			return stateMachine.currentState == state;
		}

		public override Styles.Color GetStyleColor()
		{
			ArborFSMInternal stateMachine = state.stateMachine;

			StateMachineGraphEditor stateMachineGraphEditor = graphEditor as StateMachineGraphEditor;
			if (stateMachineGraphEditor != null && stateMachineGraphEditor.IsDragBranchHover(state))
			{
				return s_DragColor;
			}
			else if (stateMachine.currentState == state)
			{
				return s_CurrentColor;
			}
			else if (stateMachine.playState == PlayState.InactivePausing && stateMachine.reservedState == state)
			{
				return s_ReservedColor;
			}
			else if (stateMachine.startStateID == state.nodeID)
			{
				return s_StartColor;
			}
			else if (state.resident)
			{
				return s_ResidentColor;
			}

			return s_NormalColor;
		}

		static Texture2D GetStateIcon(State state)
		{
			ArborFSMInternal stateMachine = state.stateMachine;

			bool isCurrentState = Application.isPlaying && stateMachine.currentState == state;
			if (stateMachine.startStateID == state.nodeID)
			{
				return isCurrentState ? Icons.currentStartStateIcon : Icons.startStateIcon;
			}
			else if (state.resident)
			{
				return Icons.residentStateIcon;
			}

			return isCurrentState ? Icons.currentNormalStateIcon : Icons.normalStateIcon;
		}

		public override Texture2D GetIcon()
		{
			return GetStateIcon(state);
		}

		public override bool IsShowNodeList()
		{
			return true;
		}

		public override void OnBindListElement(VisualElement element)
		{
			var preStatus = element.Q(NodeListElement.itemPreStatusUssClassName);
			if (state.breakPoint)
			{
				NodeListBreakpoint breakPoint = preStatus.Q<NodeListBreakpoint>(NodeListBreakpoint.ussClassName);
				if (breakPoint == null)
				{
					breakPoint = new NodeListBreakpoint()
					{
						name = NodeListBreakpoint.ussClassName,
					};
				}

				breakPoint.nodeEditor = this;
				breakPoint.isStoppingBreakpoint = IsStoppingBreakpoint;

				preStatus.Clear();
				preStatus.Add(breakPoint);
			}
			else
			{
				preStatus.Clear();
			}

			var userContentContainer = element.Q(NodeListElement.itemContentContainerUssClassName);
			if (Application.isPlaying)
			{
				var transitionCount = userContentContainer.Q<StateListTransitionCount>(StateListTransitionCount.ussClassName);
				if (transitionCount == null)
				{
					transitionCount = new StateListTransitionCount()
					{
						name = StateListTransitionCount.ussClassName,
					};
				}

				transitionCount.stateEditor = this;
				userContentContainer.Clear();
				userContentContainer.Add(transitionCount);
			}
			else
			{
				userContentContainer.Clear();
			}
		}

		bool IsStoppingBreakpoint()
		{
			return Application.isPlaying && EditorApplication.isPaused && state.stateMachine.currentState == state;
		}

		sealed class StateListTransitionCount : TextElement
		{
			public static readonly new string ussClassName = "state-list-transition-count";

			private StateEditor _StateEditor;
			public StateEditor stateEditor
			{
				get
				{
					return _StateEditor;
				}
				set
				{
					if (_StateEditor != value)
					{
						if (_StateEditor != null)
						{
							UnregisterCallbackFromStateEditor();
						}

						_StateEditor = value;

						if (_StateEditor != null)
						{
							RegisterCallbackToStateEditor();
						}
					}
				}
			}
			
			public StateListTransitionCount()
			{
				AddToClassList("count-badge");
				AddToClassList(ussClassName);
				
				RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
				RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
			}

			void OnAttachToPanel(AttachToPanelEvent e)
			{
				if (_StateEditor != null)
				{
					RegisterCallbackToStateEditor();

					UpdateTransitionCount();
				}
			}

			void OnDetachFromPanel(DetachFromPanelEvent e)
			{
				if (_StateEditor != null)
				{
					UnregisterCallbackFromStateEditor();
				}
			}

			void RegisterCallbackToStateEditor()
			{
				StateCallback.RegisterTransitionCountCallback(_StateEditor.state, UpdateTransitionCount);
			}

			void UnregisterCallbackFromStateEditor()
			{
				StateCallback.UnregisterTranstiionCountCallback(_StateEditor.state, UpdateTransitionCount);
			}

			void UpdateTransitionCount()
			{
				uint transitionCount = _StateEditor.state.transitionCount;
				text = transitionCount.ToString();
			}
		}
	}
}
