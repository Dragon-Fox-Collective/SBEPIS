//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;

namespace ArborEditor.BehaviourTree
{
	using Arbor;
	using Arbor.BehaviourTree;
	using ArborEditor.UIElements;
	using ArborEditor.BehaviourTree.UIElements;

	public abstract class TreeBehaviourNodeEditor : TreeNodeBaseEditor
	{
		public TreeBehaviourNode treeBehaviourNode
		{
			get
			{
				return node as TreeBehaviourNode;
			}
		}

		private DecoratorEditorList _DecoratorEditorList = new DecoratorEditorList();
		private ServiceEditorList _ServiceEditorList = new ServiceEditorList();

		private TreeNodeBehaviourEditorGUI _MainEditorGUI = null;

		protected override void OnInitialize()
		{
			_DecoratorEditorList.nodeEditor = this;
			_ServiceEditorList.nodeEditor = this;

			CreateEditors();
		}

		protected override void RegisterCallbackOnElement()
		{
			base.RegisterCallbackOnElement();

			nodeElement.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			nodeElement.RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
			nodeElement.RegisterCallback<RebuildElementEvent>(OnRebuildElement);
		}

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			EditorApplication.pauseStateChanged -= OnPauseStateChanged;
			EditorApplication.pauseStateChanged += OnPauseStateChanged;

			ShowBreakPoint(treeBehaviourNode.breakPoint);
			UpdateBreakOn();
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			EditorApplication.pauseStateChanged -= OnPauseStateChanged;
		}

		void OnPauseStateChanged(PauseState pauseState)
		{
			UpdateBreakOn();
		}

		void OnRebuildElement(RebuildElementEvent e)
		{
			ShowBreakPoint(treeBehaviourNode.breakPoint);
		}

		private BreakPointElement _BreakPointElement = null;

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

		protected override void OnEnable()
		{
			base.OnEnable();

			_DecoratorEditorList.nodeEditor = this;
			_ServiceEditorList.nodeEditor = this;

			_MainEditorGUI?.DoEnable();

			_DecoratorEditorList.OnEnable();
			_ServiceEditorList.OnEnable();
		}

		protected override void OnDisable()
		{
			base.OnDisable();

			_MainEditorGUI?.DoDisable();

			_DecoratorEditorList.OnDisable();
			_ServiceEditorList.OnDisable();
		}

		protected override void OnDestroy()
		{
			DestroyEditors();

			base.OnDestroy();
		}

		protected void ReplaceMainBehaviour(Object behaviourObj)
		{
			_MainEditorGUI?.Repair(behaviourObj);
		}

		public override void Validate(Node node, bool onInitialize)
		{
			base.Validate(node, onInitialize);

			if (_MainEditorGUI != null)
			{
				if (!ComponentUtility.IsValidObject(_MainEditorGUI.behaviourObj))
				{
					Object behaviourObj = treeBehaviourNode.GetBehaviourObject();
					_MainEditorGUI.Repair(behaviourObj);
				}
				else if(onInitialize)
				{
					_MainEditorGUI.RepairEditor();
				}
			}

			_DecoratorEditorList.Validate(onInitialize);
			_ServiceEditorList.Validate(onInitialize);
		}

		void CreateEditors()
		{
			if (treeBehaviourNode != null)
			{
				Object mainBehaviour = treeBehaviourNode.GetBehaviourObject();
				_MainEditorGUI = new TreeNodeBehaviourEditorGUI();
				_MainEditorGUI.Initialize(this, mainBehaviour);

				var behaviourElement = _MainEditorGUI.element;
				if (behaviourElement == null)
				{
					behaviourElement = _MainEditorGUI.CreateElement();
				}

				nodeElement.overlayLayer.Add(_MainEditorGUI.overlayLayer);

				_RootElement.Insert(1, behaviourElement);

				_DecoratorEditorList.RebuildBehaviourEditors();

				_ServiceEditorList.RebuildBehaviourEditors();
			}
		}

		void DestroyEditors()
		{
			_MainEditorGUI?.Destroy();
			_MainEditorGUI = null;

			_DecoratorEditorList.DestroyEditors();
			_ServiceEditorList.DestroyEditors();
		}

		protected override VisualElement CreatePreHeaderElement()
		{
			_ParentLinkSlotElement = new ParentLinkSlotElement(this);
			return _ParentLinkSlotElement;
		}

		void UpdateBreakOn()
		{
			if (treeBehaviourNode.breakPoint && _BreakPointElement != null)
			{
				_BreakPointElement.breakOn = Application.isPlaying && EditorApplication.isPaused && treeBehaviourNode.isActive;
			}
		}

		protected override void OnUpdate()
		{
			base.OnUpdate();

			_MainEditorGUI?.Update();

			_DecoratorEditorList.Update();
			_ServiceEditorList.Update();
		}

		protected override void OnRepainted()
		{
			_MainEditorGUI?.OnRepainted();

			_DecoratorEditorList.OnRepainted();
			_ServiceEditorList.OnRepainted();
		}

		private VisualElement _RootElement;

		protected override VisualElement CreateContentElement()
		{
			_RootElement = new VisualElement();

			_RootElement.Add(_DecoratorEditorList.CreateElement());


			if (_MainEditorGUI != null)
			{
				var behaviourElement = _MainEditorGUI.element;
				if (behaviourElement == null)
				{
					behaviourElement = _MainEditorGUI.CreateElement();
				}

				_RootElement.Add(behaviourElement);

				if (_MainEditorGUI.overlayLayer.parent == null)
				{
					nodeElement.overlayLayer.Add(_MainEditorGUI.overlayLayer);
				}
			}

			_RootElement.Add(_ServiceEditorList.CreateElement());

			return _RootElement;
		}

		internal void InsertDecoratorEditor(int index, Object decoratorObj)
		{
			_DecoratorEditorList.InsertBehaviourEditor(index, decoratorObj);

			graphEditor.RaiseOnChangedNodes();
		}

		public void AddDecorator(System.Type classType)
		{
			Object decoratorObj = treeBehaviourNode.AddDecorator(classType);

			DecoratorList decoratorList = treeBehaviourNode.decoratorList;
			int decoratorCount = decoratorList.count;
			int index = decoratorCount - 1;
			InsertDecoratorEditor(index, decoratorObj);
		}

		public void InsertDecorator(int index, System.Type classType)
		{
			Object decoratorObj = treeBehaviourNode.InsertDecorator(index, classType);

			InsertDecoratorEditor(index, decoratorObj);
		}

		public void MoveDecorator(int fromIndex, int toIndex)
		{
			NodeGraph nodeGraph = treeBehaviourNode.nodeGraph;

			Undo.IncrementCurrentGroup();

			Undo.RecordObject(nodeGraph, (fromIndex > toIndex) ? "MoveUp Behaviour" : "MoveDown Behaviour");

			treeBehaviourNode.decoratorList.Swap(fromIndex, toIndex);

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			EditorUtility.SetDirty(nodeGraph);

			_DecoratorEditorList.MoveBehaviourEditor(fromIndex, toIndex);
		}

		void AddDecoratorMenu(object obj)
		{
			Rect position = (Rect)obj;

			DecoratorMenuWindow.instance.Init(position, -1, CreateDecoratorByType);
		}

		internal void CreateDecoratorByType(int index, System.Type type)
		{
			if (index == -1)
			{
				AddDecorator(type);
			}
			else
			{
				InsertDecorator(index, type);
			}
		}

		public void PasteDecorator(int index)
		{
			Undo.IncrementCurrentGroup();

			TreeBehaviourNode node = treeBehaviourNode;
			NodeGraph nodeGraph = node.nodeGraph;

			Undo.RecordObject(nodeGraph, "Paste Decorator");

			PasteDecoratorAsNew(node, index);

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			EditorUtility.SetDirty(nodeGraph);

			graphEditor.RaiseOnChangedNodes();
		}

		internal static void PasteDecoratorAsNew(TreeBehaviourNode behaviourNode, int index)
		{
			Decorator destDecorator = Clipboard.copyBehaviour as Decorator;
			if (destDecorator == null)
			{
				return;
			}

			PasteDecoratorAsNew(behaviourNode, index, destDecorator);
		}

		internal static Decorator PasteDecoratorAsNew(TreeBehaviourNode behaviourNode, int index, Decorator destDecorator)
		{
			Decorator decorator = NodeBehaviour.CreateNodeBehaviour(behaviourNode, destDecorator.GetType(), true) as Decorator;

			if (index == -1)
			{
				behaviourNode.decoratorList.Add(decorator);
			}
			else
			{
				behaviourNode.decoratorList.Insert(index, decorator);
			}

			Clipboard.CopyNodeBehaviour(destDecorator, decorator, true);

			return decorator;
		}

		void PasteDecoratorAsNewContextMenu()
		{
			PasteDecorator(-1);
		}

		internal void InsertServiceEditor(int index, Object serviceObj)
		{
			_ServiceEditorList.InsertBehaviourEditor(index, serviceObj);

			graphEditor.RaiseOnChangedNodes();
		}

		public void AddService(System.Type classType)
		{
			Object serviceObj = treeBehaviourNode.AddService(classType);

			ServiceList serviceList = treeBehaviourNode.serviceList;
			int serviceCount = serviceList.count;
			int index = serviceCount - 1;
			InsertServiceEditor(index, serviceObj);
		}

		public void InsertService(int index, System.Type classType)
		{
			Object serviceObj = treeBehaviourNode.InsertService(index, classType);

			InsertServiceEditor(index, serviceObj);
		}

		public void MoveService(int fromIndex, int toIndex)
		{
			NodeGraph nodeGraph = treeBehaviourNode.nodeGraph;

			Undo.IncrementCurrentGroup();

			Undo.RecordObject(nodeGraph, (fromIndex > toIndex) ? "MoveUp Behaviour" : "MoveDown Behaviour");

			treeBehaviourNode.serviceList.Swap(fromIndex, toIndex);

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			EditorUtility.SetDirty(nodeGraph);

			_ServiceEditorList.MoveBehaviourEditor(fromIndex, toIndex);
		}

		void AddServiceMenu(object obj)
		{
			Rect position = (Rect)obj;

			ServiceMenuWindow.instance.Init(position, -1, CreateServiceByType);
		}

		internal void CreateServiceByType(int index, System.Type type)
		{
			if (index == -1)
			{
				AddService(type);
			}
			else
			{
				InsertService(index, type);
			}
		}

		public void PasteService(int index)
		{
			Undo.IncrementCurrentGroup();

			TreeBehaviourNode node = treeBehaviourNode;
			NodeGraph nodeGraph = node.nodeGraph;

			Undo.RecordObject(nodeGraph, "Paste Service");

			PasteServiceAsNew(node, index);

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			EditorUtility.SetDirty(nodeGraph);

			graphEditor.RaiseOnChangedNodes();
		}

		internal static void PasteServiceAsNew(TreeBehaviourNode behaviourNode, int index)
		{
			Service destService = Clipboard.copyBehaviour as Service;
			if (destService == null)
			{
				return;
			}

			PasteServiceAsNew(behaviourNode, index, destService);
		}

		internal static Service PasteServiceAsNew(TreeBehaviourNode behaviourNode, int index, Service destService)
		{
			Service service = NodeBehaviour.CreateNodeBehaviour(behaviourNode, destService.GetType(), true) as Service;

			if (index == -1)
			{
				behaviourNode.serviceList.Add(service);
			}
			else
			{
				behaviourNode.serviceList.Insert(index, service);
			}

			Clipboard.CopyNodeBehaviour(destService, service, true);

			return service;
		}

		void PasteServiceAsNewContextMenu()
		{
			PasteService(-1);
		}

		public override void ExpandAll(bool expanded)
		{
			BehaviourEditorGUI mainEditor = GetMainEditor();
			mainEditor?.SetExpanded(expanded);

			DecoratorList decoratorList = treeBehaviourNode.decoratorList;
			int decoratorCount = decoratorList.count;
			for (int decoratorIndex = 0; decoratorIndex < decoratorCount; decoratorIndex++)
			{
				BehaviourEditorGUI behaviourEditor = GetDecoratorEditor(decoratorIndex);
				behaviourEditor?.SetExpanded(expanded);
			}

			ServiceList serviceList = treeBehaviourNode.serviceList;
			int serviceCount = serviceList.count;
			for (int serviceIndex = 0; serviceIndex < serviceCount; serviceIndex++)
			{
				BehaviourEditorGUI behaviourEditor = GetServiceEditor(serviceIndex);
				behaviourEditor?.SetExpanded(expanded);
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

		void FlipBreakPoint()
		{
			NodeGraph nodeGraph = treeBehaviourNode.nodeGraph;

			if (treeBehaviourNode.breakPoint)
			{
				Undo.RecordObject(nodeGraph, "Node BreakPoint Off");
			}
			else
			{
				Undo.RecordObject(nodeGraph, "Node BreakPoint On");
			}

			treeBehaviourNode.breakPoint = !treeBehaviourNode.breakPoint;

			EditorUtility.SetDirty(nodeGraph);

			ShowBreakPoint(treeBehaviourNode.breakPoint);

			graphEditor.RaiseOnChangedNodes();
		}

		protected virtual void SetReplaceBehaviourMenu(GenericMenu menu, Rect headerPosition, bool editable)
		{
		}

		protected override void SetContextMenu(GenericMenu menu, Rect headerPosition, bool editable)
		{
			if (editable)
			{
				menu.AddItem(EditorContents.breakPoint, treeBehaviourNode.breakPoint, FlipBreakPoint);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.breakPoint);
			}

			menu.AddSeparator("");

			int itemCount = menu.GetItemCount();

			SetReplaceBehaviourMenu(menu, headerPosition, editable);

			if (menu.GetItemCount() > itemCount)
			{
				menu.AddSeparator("");
			}

			if (editable)
			{
				menu.AddItem(EditorContents.addDecorator, false, AddDecoratorMenu, GUIUtility.GUIToScreenRect(headerPosition));
				menu.AddItem(EditorContents.addService, false, AddServiceMenu, GUIUtility.GUIToScreenRect(headerPosition));
			}
			else
			{
				menu.AddDisabledItem(EditorContents.addDecorator);
				menu.AddDisabledItem(EditorContents.addService);
			}

			if (Clipboard.CompareBehaviourType(typeof(Decorator), true) && editable)
			{
				menu.AddItem(EditorContents.pasteDecoratorAsNew, false, PasteDecoratorAsNewContextMenu);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.pasteDecoratorAsNew);
			}

			if (Clipboard.CompareBehaviourType(typeof(Service), true) && editable)
			{
				menu.AddItem(EditorContents.pasteServiceAsNew, false, PasteServiceAsNewContextMenu);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.pasteServiceAsNew);
			}

			menu.AddSeparator("");

			menu.AddItem(EditorContents.expandAll, false, ExpandAll);

			menu.AddItem(EditorContents.collapseAll, false, FoldAll);
		}

		public enum BehaviourType
		{
			Main,
			Decorator,
			Service,
		}

		public static BehaviourType GetBehaviourType(Object behaviourObj)
		{
			if (behaviourObj is Decorator)
			{
				return BehaviourType.Decorator;
			}
			else if (behaviourObj is Service)
			{
				return BehaviourType.Service;
			}

			return BehaviourType.Main;
		}

		public BehaviourEditorGUI GetBehaviourEditor(Object behaviourObj)
		{
			return GetBehaviourEditor(behaviourObj, GetBehaviourType(behaviourObj));
		}

		public BehaviourEditorGUI GetBehaviourEditor(Object behaviourObj, BehaviourType behaviourType)
		{
			switch (behaviourType)
			{
				case BehaviourType.Main:
					return GetMainEditor();
				case BehaviourType.Decorator:
					int decoratorIndex = treeBehaviourNode.decoratorList.IndexOf(behaviourObj);
					return GetDecoratorEditor(decoratorIndex);
				case BehaviourType.Service:
					int serviceIndex = treeBehaviourNode.serviceList.IndexOf(behaviourObj);
					return GetServiceEditor(serviceIndex);
			}

			return null;
		}

		internal void RemoveDecoratorEditor(int decoratorIndex)
		{
			_DecoratorEditorList.RemoveBehaviourEditor(decoratorIndex);
		}

		public void DestroyDecoratorAt(int decoratorIndex)
		{
			RemoveDecoratorEditor(decoratorIndex);

			Undo.IncrementCurrentGroup();
			int undoGruop = Undo.GetCurrentGroup();

			treeBehaviourNode.decoratorList.Destroy(treeBehaviourNode, decoratorIndex);

			Undo.CollapseUndoOperations(undoGruop);

			graphEditor.RaiseOnChangedNodes();
		}

		internal void RemoveServiceEditor(int serviceIndex)
		{
			_ServiceEditorList.RemoveBehaviourEditor(serviceIndex);
		}

		public void DestroyServiceAt(int serviceIndex)
		{
			RemoveServiceEditor(serviceIndex);

			Undo.IncrementCurrentGroup();
			int undoGruop = Undo.GetCurrentGroup();

			treeBehaviourNode.serviceList.Destroy(treeBehaviourNode, serviceIndex);

			Undo.CollapseUndoOperations(undoGruop);

			graphEditor.RaiseOnChangedNodes();
		}

		public BehaviourEditorGUI GetMainEditor()
		{
			BehaviourEditorGUI behaviourEditor = _MainEditorGUI;

			if (!ComponentUtility.IsValidObject(behaviourEditor.behaviourObj))
			{
				Object behaviourObj = treeBehaviourNode.GetBehaviourObject();
				behaviourEditor.Repair(behaviourObj);
			}

			return behaviourEditor;
		}

		internal DecoratorEditorGUI GetDecoratorEditor(int decoratorIndex)
		{
			return _DecoratorEditorList.GetBehaviourEditor(decoratorIndex);
		}

		internal ServiceEditorGUI GetServiceEditor(int serviceIndex)
		{
			return _ServiceEditorList.GetBehaviourEditor(serviceIndex);
		}

		protected virtual Texture2D GetDefaultIcon()
		{
			return null;
		}

		public override Texture2D GetIcon()
		{
			Texture icon = EditorGUITools.GetThumbnailContent(treeBehaviourNode.behaviour).image;
			if (icon != null && !DefaultScriptIcon.IsDefaultScriptIcon(icon))
			{
				return icon as Texture2D;
			}
			return GetDefaultIcon();
		}

		bool IsStoppingBreakpoint()
		{
			return Application.isPlaying && EditorApplication.isPaused && treeBehaviourNode.isActive;
		}

		public override void OnBindListElement(VisualElement element)
		{
			base.OnBindListElement(element);

			var preStatus = element.Q(NodeListElement.itemPreStatusUssClassName);
			if (treeBehaviourNode.breakPoint)
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
		}

		public override bool IsCopyable()
		{
			return treeBehaviourNode.behaviour != null;
		}
	}
}