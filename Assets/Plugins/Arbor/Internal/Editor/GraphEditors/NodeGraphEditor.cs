//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ArborEditor
{
	using Arbor;
	using Arbor.ValueFlow;
	using Arbor.Playables;
	using ArborEditor.UIElements;
	using ArborEditor.UnityEditorBridge;
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	public class NodeGraphEditor : ScriptableObject
	{
		private static class Types
		{
			public static readonly System.Type ParameterContainerType;
			public static readonly System.Type GetParameterCalculatorType;

			static Types()
			{
				ParameterContainerType = AssemblyHelper.GetTypeByName("Arbor.ParameterContainer");
				GetParameterCalculatorType = AssemblyHelper.GetTypeByName("Arbor.ParameterBehaviours.GetParameterCalculator");
			}
		}

		public static bool HasEditor(NodeGraph nodeGraph)
		{
			if (nodeGraph == null)
			{
				return false;
			}

			System.Type classType = nodeGraph.GetType();
			System.Type editorType = CustomAttributes<CustomNodeGraphEditor, NodeGraphEditor>.FindEditorType(classType);

			return editorType != null;
		}

		public static NodeGraphEditor CreateEditor(ArborEditorWindow window, NodeGraph nodeGraph, bool isExternalGraph)
		{
			if (nodeGraph == null)
			{
				return null;
			}

			System.Type classType = nodeGraph.GetType();
			System.Type editorType = CustomAttributes<CustomNodeGraphEditor, NodeGraphEditor>.FindEditorType(classType);

			if (editorType == null)
			{
				editorType = typeof(NodeGraphEditor);
			}

			NodeGraphEditor nodeGraphEditor = CreateInstance(editorType) as NodeGraphEditor;
			nodeGraphEditor.hideFlags = HideFlags.HideAndDontSave;
			nodeGraphEditor.hostWindow = window;
			nodeGraphEditor.nodeGraph = nodeGraph;
			nodeGraphEditor.isExternalGraph = isExternalGraph;

			nodeGraphEditor.InitNodeEditors();
			nodeGraphEditor.SetupElements();

			return nodeGraphEditor;
		}

		private NodeListElement _NodeListElement;

		[SerializeField]
		private NodeListElement.SearchState _NodeListSearchState = new NodeListElement.SearchState();

		private VisualElement _ParametersElement;
		private VisualElement _ParametersEditorElement;
		private VisualElement _CreateParameterContainerButton;

		internal void SetupElements()
		{
			_NodeListElement = new NodeListElement()
			{
				selectionType = SelectionType.Multiple,
				sortComparisonCallback = NodeListSortComparison,
				onSelectionChange = OnSelectionChange,
				searchState = _NodeListSearchState,
			};
			var nodeListTab = hostWindow.sidePanel.GetTab(SidePanelTab.NodeList);
			if (nodeListTab != null)
			{
				nodeListTab.Add(_NodeListElement);
			}

			_NodeListElement.graphEditor = this;

			_ParametersElement = new VisualElement()
			{
				style =
				{
					flexGrow = 1f,
				}
			};
			_ParametersElement.RegisterCallback<MouseDownEvent>(e =>
			{
				GUIUtility.keyboardControl = 0;
				e.StopPropagation();
			});
			var parametersTab = hostWindow.sidePanel.GetTab(SidePanelTab.Parameters);
			if (parametersTab != null)
			{
				parametersTab.Add(_ParametersElement);
			}

			UpdateParameterContainerEditor();
		}

		[SerializeField]
		private ArborEditorWindow _HostWindow;

		[SerializeField]
		private NodeGraph _NodeGraph;

		[SerializeField]
		private int _NodeGraphInstanceID = 0;

		[SerializeField]
		private List<int> _Selection = new List<int>();

		public GraphView graphView
		{
			get
			{
				return _HostWindow.graphView;
			}
		}

		public MinimapView minimapView
		{
			get
			{
				return _HostWindow.minimapView;
			}
		}

		private List<NodeEditor> _NodeEditors = new List<NodeEditor>();
		private Dictionary<int, NodeEditor> _DicNodeEditors = new Dictionary<int, NodeEditor>();

		private PopupButtonElement _PopupButton = null;

		private RenameOverlayElement _RenameOverlay;
		private bool _RenameCreateNode = false;
		private int _BeginRenameUndoGroup;

		private List<int> _OldSelection;

		private DottedBezierElement _DragDataBranchElement;
		private bool _DragDataBranchEnable = false;
		private int _DragDataBranchNodeID = 0;

		private Dictionary<Node, Rect> _DragNodePositions = new Dictionary<Node, Rect>();
		private Dictionary<Node, Rect> _SaveNodePositions = new Dictionary<Node, Rect>();
		private Vector2 _BeginMousePosition;
		private Vector2 _DragNodeDistance;

		public delegate void DropNodesCallback(Node[] nodes);

		private sealed class DropNodesElement
		{
			public Rect position;
			public DropNodesCallback callback;
		}

		private List<DropNodesElement> _DropNodesElements = new List<DropNodesElement>();

		public void AddDropNodesListener(Rect position, DropNodesCallback callback)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}

			DropNodesElement element = new DropNodesElement();
			element.position = GUIUtility.GUIToScreenRect(position);
			element.callback = callback;

			_DropNodesElements.Add(element);
		}


		public bool isDragNodes
		{
			get
			{
				return _DragNodePositions.Count > 0;
			}
		}

		public ArborEditorWindow hostWindow
		{
			get
			{
				return _HostWindow;
			}
			set
			{
				_HostWindow = value;
			}
		}

		public NodeGraph nodeGraph
		{
			get
			{
				return _NodeGraph;
			}
			set
			{
				if (_NodeGraph != value)
				{
					_NodeGraph = value;
					if (_NodeGraph != null)
					{
						_NodeGraphInstanceID = _NodeGraph.GetInstanceID();
					}
					else
					{
						_NodeGraphInstanceID = 0;
					}

					UpdateParameterContainerEditor();

					onChangedGraph?.Invoke();
				}
			}
		}

		public int nodeEditorCount
		{
			get
			{
				return _NodeEditors.Count;
			}
		}

		public int selectionCount
		{
			get
			{
				return _Selection.Count;
			}
		}

		public Node GetSelectionNode(int index)
		{
			int nodeId = _Selection[index];

			return nodeGraph.GetNodeFromID(nodeId);
		}

		public NodeEditor GetSelectionNodeEditor(int index)
		{
			int nodeId = _Selection[index];

			return GetNodeEditorFromID(nodeId);
		}

		public Node[] GetSelectionNodes()
		{
			List<Node> nodes = new List<Node>();

			int selectionCount = this.selectionCount;
			for (int selectionIndex = 0; selectionIndex < selectionCount; selectionIndex++)
			{
				Node node = GetSelectionNode(selectionIndex);
				if (node != null)
				{
					nodes.Add(node);
				}
			}

			return nodes.ToArray();
		}

		private bool _IsExternalGraph = false;

		public bool isExternalGraph
		{
			get
			{
				return _IsExternalGraph;
			}
			internal set
			{
				if (_IsExternalGraph != value)
				{
					_IsExternalGraph = value;
					UpdateEditable();
				}
			}
		}

		private bool _Editable = true;

		public event System.Action<bool> onChangedEditable;

		void UpdateEditable()
		{
			bool editable = (nodeGraph.hideFlags & HideFlags.NotEditable) != HideFlags.NotEditable && !_IsExternalGraph;
			if (_Editable != editable)
			{
				_Editable = editable;
				onChangedEditable?.Invoke(_Editable);
			}
		}

		public bool editable
		{
			get
			{
				return _Editable;
			}
		}

		public event System.Action onChangedGraph;
		public event System.Action onChangedNodes;

		public bool RepairReferences()
		{
			if (_NodeGraph == null && _NodeGraphInstanceID != 0)
			{
#if ARBOR_DEBUG
				Debug.Log("Reatach");
#endif
				_NodeGraph = EditorUtility.InstanceIDToObject(_NodeGraphInstanceID) as NodeGraph;

				UpdateParameterContainerEditor();

				onChangedGraph?.Invoke();

				return true;
			}
			return false;
		}

		public void Repaint()
		{
			_HostWindow.DoRepaint();
		}

		private void CreateNodeEditor(Node node)
		{
			if (!NodeEditor.HasEditor(node))
			{
				return;
			}

			NodeEditor editor = NodeEditor.CreateEditors(this, node);

			if (editor == null)
			{
				return;
			}

			_NodeEditors.Add(editor);
			_DicNodeEditors.Add(node.nodeID, editor);
		}

		internal void DirtyGraphExtents()
		{
			if (graphView.DirtyGraphExtents())
			{
				minimapView.UpdateMinimapTransfrom();
			}
		}

		public NodeEditor GetNodeEditor(int index)
		{
			return _NodeEditors[index];
		}

		public NodeEditor GetNodeEditorFromID(int nodeID)
		{
			NodeEditor result;
			if (_DicNodeEditors.TryGetValue(nodeID, out result))
			{
				return result;
			}

			for (int i = 0, count = _NodeEditors.Count; i < count; i++)
			{
				NodeEditor nodeEditor = _NodeEditors[i];
				if (nodeEditor.nodeID == nodeID)
				{
					_DicNodeEditors.Add(nodeID, nodeEditor);
					return nodeEditor;
				}
			}

			return null;
		}

		public NodeEditor GetNodeEditor(Node node)
		{
			return GetNodeEditorFromID(node.nodeID);
		}

		void DeleteNodeEditor(Node node)
		{
			NodeEditor nodeEditor = GetNodeEditor(node);

			if (nodeEditor != null)
			{
				_NodeEditors.Remove(nodeEditor);
				_DicNodeEditors.Remove(node.nodeID);

				Object.DestroyImmediate(nodeEditor);
			}
		}

		void InitNodeEditors()
		{
			if (nodeGraph == null)
			{
				return;
			}

			int nodeCount = nodeGraph.nodeCount;
			if (nodeCount == 0)
			{
				return;
			}

			for (int nodeIndex = 0; nodeIndex < nodeCount; nodeIndex++)
			{
				Node node = nodeGraph.GetNodeFromIndex(nodeIndex);

				CreateNodeEditor(node);
			}

			RaiseOnChangedNodes();
		}

		void FinalizeNodeEditors()
		{
			for (int i = 0, count = _NodeEditors.Count; i < count; i++)
			{
				NodeEditor nodeEditor = _NodeEditors[i];
				if (nodeEditor != null)
				{
					Object.DestroyImmediate(nodeEditor);
				}
			}

			_NodeEditors.Clear();
			_DicNodeEditors.Clear();
		}

		internal void ValidateNodes()
		{
			int nodeCount = nodeGraph.nodeCount;
			for (int nodeIndex = 0; nodeIndex < nodeCount; nodeIndex++)
			{
				Node node = nodeGraph.GetNodeFromIndex(nodeIndex);

				NodeEditor nodeEditor = GetNodeEditor(node);
				if (nodeEditor != null)
				{
					nodeEditor.Validate(node, true);
				}
			}
		}

		public void RebuildIfNecessary()
		{
			bool removed = false;
			bool created = false;

			for (int count = _NodeEditors.Count, i = count - 1; i >= 0; i--)
			{
				NodeEditor nodeEditor = _NodeEditors[i];
				if (nodeEditor == null)
				{
					_NodeEditors.RemoveAt(i);

					removed = true;
				}
				else
				{
					Node node = nodeGraph != null ? nodeGraph.GetNodeFromID(nodeEditor.nodeID) : null;

					if (!nodeEditor.IsValidNode(node))
					{
						_NodeEditors.RemoveAt(i);
						Object.DestroyImmediate(nodeEditor);

						removed = true;
					}
				}
			}

			if (removed)
			{
				_DicNodeEditors.Clear();
			}

			if (nodeGraph != null)
			{
				int nodeCount = nodeGraph.nodeCount;
				for (int nodeIndex = 0; nodeIndex < nodeCount; nodeIndex++)
				{
					Node node = nodeGraph.GetNodeFromIndex(nodeIndex);

					NodeEditor nodeEditor = GetNodeEditor(node);
					if (nodeEditor == null)
					{
						CreateNodeEditor(node);

						created = true;
					}
					else
					{
						nodeEditor.Validate(node, false);
					}
				}
			}

			if (removed && !created)
			{
				DirtyGraphExtents();
			}

			if (removed || created)
			{
				RaiseOnChangedNodes();

				UnityEditorBridge.ScriptAttributeUtilityBridge.ClearGlobalCache();
			}
		}

		void OnValidate()
		{
			if (graphView == null)
			{
				// Ignore if called before ArborEditorWindow.OnEnable()
				return;
			}
			_DicNodeEditors.Clear();

			RebuildIfNecessary();
		}

		public bool OverlapsViewArea(Rect position)
		{
			return graphView.graphViewport.Overlaps(position);
		}

		public virtual GUIContent GetGraphLabel()
		{
			return null;
		}

		public virtual bool HasPlayState()
		{
			return false;
		}

		public virtual PlayState GetPlayState()
		{
			return PlayState.Stopping;
		}

		public void UpdateLayout()
		{
			for (int i = 0, count = nodeEditorCount; i < count; i++)
			{
				GroupNodeEditor groupNodeEditor = GetNodeEditor(i) as GroupNodeEditor;
				if (groupNodeEditor != null)
				{
					groupNodeEditor.AutoLayout();
				}
			}

			DirtyGraphExtents();
		}

		protected virtual void OnCreateSetParameter(Vector2 position, Parameter parameter)
		{
		}

		void OnCreateGetParameter(Vector2 position, Parameter parameter)
		{
			Undo.IncrementCurrentGroup();

			CalculatorNode calculatorNode = CreateCalculatorInternal(position, Types.GetParameterCalculatorType);
			Arbor.ParameterBehaviours.GetParameterCalculatorInternal getParameterCalculator = calculatorNode.calculator as Arbor.ParameterBehaviours.GetParameterCalculatorInternal;

			Undo.RecordObject(getParameterCalculator, "Created Calculator");

			getParameterCalculator.SetParameter(parameter);

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			EditorUtility.SetDirty(getParameterCalculator);

			CalculatorEditor calculatorEditor = GetNodeEditor(calculatorNode) as CalculatorEditor;
			if (calculatorEditor != null)
			{
				calculatorEditor.SetupResizable();
			}
		}

		private bool _IsDragObject = false;

		static bool Internal_IsDragObject()
		{
			var objectReferences = DragAndDrop.objectReferences;
			var paths = DragAndDrop.paths;
			return (objectReferences != null && objectReferences.Length >= 1) || (paths != null && paths.Length >= 1);
		}

		public virtual void OnDragEnter()
		{
		}

		Parameter GetDragginParameter()
		{
			var objectReferences = DragAndDrop.objectReferences;
			for (int objIndex = 0; objIndex < objectReferences.Length; objIndex++)
			{
				Object obj = objectReferences[objIndex];
				if (obj is ParameterDraggingObject draggingObject)
				{
					return draggingObject.parameter;
				}
			}

			return null;
		}

		public virtual void OnDragUpdated()
		{
			var draggingParameter = GetDragginParameter();
			if (draggingParameter != null)
			{
				if (editable)
				{
					DragAndDrop.visualMode = DragAndDropVisualMode.Link;
				}
				else
				{
					DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
				}
			}

			var isDragObject = Internal_IsDragObject() || draggingParameter != null;
			if (_IsDragObject != isDragObject)
			{
				_IsDragObject = isDragObject;
				ClearInvsibleNodes();
				Repaint();
			}

			BehaviourDragInfo behaviourDragInfo = BehaviourDragInfo.GetBehaviourDragInfo();
			if (behaviourDragInfo != null)
			{
				behaviourDragInfo.dragging = true;
			}

			graphView.autoScroll = _IsDragObject || behaviourDragInfo != null;
		}

		public virtual void OnDragPerform(Vector2 graphMousePosition)
		{
			if (ContainsNodes(graphMousePosition))
			{
				return;
			}

			var draggingParameter = GetDragginParameter();

			if (draggingParameter != null)
			{
				if (editable)
				{
					MousePosition mousePosition = new MousePosition(graphMousePosition);

					GenericMenu menu = new GenericMenu();

					menu.AddItem(EditorContents.get, false, () =>
					{
						OnCreateGetParameter(mousePosition.guiPoint, draggingParameter);
					});

					menu.AddItem(EditorContents.set, false, () =>
					{
						OnCreateSetParameter(mousePosition.guiPoint, draggingParameter);
					});

					menu.ShowAsContext();

					DragAndDrop.AcceptDrag();
					DragAndDrop.activeControlID = 0;
				}
				else
				{
					DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
				}
			}
		}

		void ExitDrag()
		{
			_IsDragObject = false;
			ClearInvsibleNodes();
			Repaint();

			BehaviourDragInfo behaviourDragInfo = BehaviourDragInfo.GetBehaviourDragInfo();
			if (behaviourDragInfo != null)
			{
				behaviourDragInfo.dragging = false;
			}

			graphView.autoScroll = false;
		}

		public virtual void OnDragLeave()
		{
			ExitDrag();
		}

		public virtual void OnDragExited()
		{
			ExitDrag();
		}

		public VisualElement ShowPopupButtonControl(Vector2 attachPoint, GUIContent content, int activeControlID, GUIStyle style, System.Action<Rect> onClick)
		{
			if (_PopupButton == null)
			{
				_PopupButton = new PopupButtonElement(graphView)
				{
					style =
					{
						position = Position.Absolute,
					}
				};

				_PopupButton.RegisterCallback<DetachFromPanelEvent>((evt) => {
					_PopupButton = null;
				});

				graphView.popupLayer.Add(_PopupButton);
			}

			_PopupButton.attachPoint = attachPoint;
			_PopupButton.content = content;
			_PopupButton.buttonStyle = style;
			_PopupButton.activeControlID = activeControlID;
			_PopupButton.onClick = onClick;

			Repaint();

			return _PopupButton;
		}

		public void CloseAllPopupButtonControl()
		{
			FinalizePopupLayer();

			Repaint();
		}

		public int GetPopupButtonActiveControlID()
		{
			return _PopupButton != null ? _PopupButton.activeControlID : 0;
		}

		void FinalizePopupLayer()
		{
			if (_PopupButton != null)
			{
				_PopupButton.RemoveFromHierarchy();
				_PopupButton = null;
			}
		}

		public static bool IsShowNodeComment(Node node)
		{
			switch (ArborSettings.nodeCommentViewMode)
			{
				case NodeCommentViewMode.Normal:
					return NodeEditorUtility.GetShowComment(node);
				case NodeCommentViewMode.ShowAll:
					return true;
				case NodeCommentViewMode.ShowCommentedOnly:
					return !string.IsNullOrEmpty(node.nodeComment);
				case NodeCommentViewMode.HideAll:
					return false;
			}

			return false;
		}

		public bool TryGetBehaviourPosition(Object obj, out Rect position)
		{
			if (obj == null)
			{
				position = default;
				return false;
			}
			var editorGUI = BehaviourEditorGUI.Get(obj.GetInstanceID());
			if (editorGUI == null)
			{
				position = default;
				return false;
			}

			position = editorGUI.GetHeaderPosition();
			return true;
		}

		private sealed class EnumInfo
		{
			public readonly int[] values;
			public readonly string[] names;

			public EnumInfo(System.Type enumType)
			{
				if (!enumType.IsEnum)
				{
					throw new System.ArgumentException("The type `" + enumType.Name + "' must be convertible to `enum' in order to use it as parameter `enumType'", "enumType");
				}

				List<System.Reflection.FieldInfo> enumFields = EnumUtility.GetFields(enumType);

				values = enumFields.Select(f => (int)System.Enum.Parse(enumType, f.Name)).ToArray();
				names = enumFields.Select(f => f.Name).ToArray();
			}
		}

		private static Dictionary<System.Type, EnumInfo> s_EnumInfos = new Dictionary<System.Type, EnumInfo>();
		private static System.Text.StringBuilder s_DataValueStringBuilder = new System.Text.StringBuilder();

		static string ToEnumFlagsString(System.Type valueType, int currentValue)
		{
			EnumInfo enumInfo = null;
			if (!s_EnumInfos.TryGetValue(valueType, out enumInfo))
			{
				enumInfo = new EnumInfo(valueType);
				s_EnumInfos.Add(valueType, enumInfo);
			}

			s_DataValueStringBuilder.Length = 0;
			for (int i = 0, count = enumInfo.values.Length; i < count; i++)
			{
				int intValue = enumInfo.values[i];
				if ((currentValue & intValue) == intValue)
				{
					if (s_DataValueStringBuilder.Length > 0)
					{
						s_DataValueStringBuilder.Append(", ");
					}
					s_DataValueStringBuilder.Append(enumInfo.names[i]);
				}
			}

			return s_DataValueStringBuilder.Length == 0 ? "<Nothing>" : s_DataValueStringBuilder.ToString();
		}

		static string ToListString(System.Type valueType, IList list)
		{
			s_DataValueStringBuilder.Length = 0;
			s_DataValueStringBuilder.AppendFormat("{0} : Length {1}", TypeUtility.GetSlotTypeName(valueType), list.Count);

			return s_DataValueStringBuilder.ToString();
		}

		internal static string ToDataValueString(OutputSlotBase valueSlot, System.Type valueType)
		{
			if (EnumFieldUtility.IsEnumFlags(valueType))
			{
				object value = valueSlot.GetValue();
				if (value == null)
				{
					return "null";
				}
				return ToEnumFlagsString(valueType, (int)value);
			}
			else if (typeof(IList).IsAssignableFrom(valueType) || TypeUtility.IsGeneric(valueType, typeof(IList<>)))
			{
				IList list = valueSlot.GetValue() as IList;
				if (list == null)
				{
					return "null";
				}
				return ToListString(valueType, list);
			}

			return valueSlot.GetValueString();
		}

		internal static string ToDataValueDetailsString(OutputSlotBase valueSlot, System.Type valueType)
		{
			if (!(typeof(IList).IsAssignableFrom(valueType) || TypeUtility.IsGeneric(valueType, typeof(IList<>))))
			{
				return null;
			}

			IList list = valueSlot.GetValue() as IList;
			if (list == null)
			{
				return null;
			}
			
			s_DataValueStringBuilder.Length = 0;

			var listType = list.GetType();
			var elementType = ListUtility.GetElementType(listType);
			if (elementType != null)
			{
				ListMediator mediator = ValueMediator.Get(elementType).listMediator;
				int count = list.Count;
				for (int i = 0; i < count; i++)
				{
					if (i != 0)
					{
						s_DataValueStringBuilder.AppendLine();
					}
					s_DataValueStringBuilder.AppendFormat("\t[{0}] {1}", i, mediator.GetElementString(list, i));
				}
			}
			else
			{
				for (int i = 0; i < list.Count; i++)
				{
					object element = list[i];
					if (i != 0)
					{
						s_DataValueStringBuilder.AppendLine();
					}
					s_DataValueStringBuilder.AppendFormat("\t[{0}] {1}", i, (element == null) ? "null" : element.ToString());
				}
			}

			return s_DataValueStringBuilder.ToString();
		}

		internal static string GetDataValueLogString(string valueString, string detailsString)
		{
			s_DataValueStringBuilder.Length = 0;

			s_DataValueStringBuilder.Append(valueString);
			if (!string.IsNullOrEmpty(detailsString))
			{
				s_DataValueStringBuilder.AppendLine();
				s_DataValueStringBuilder.Append(detailsString);
			}

			return s_DataValueStringBuilder.ToString();
		}


		protected virtual void OnUpdate()
		{
		}

		internal DataBranch ConnectDataBranch(int inputNodeID, Object inputObj, DataSlot inputSlot, int outputNodeID, Object outputObj, DataSlot outputSlot)
		{
			var branch = nodeGraph.ConnectDataBranch(inputNodeID, inputObj, inputSlot, outputNodeID, outputObj, outputSlot);

			CreateDataBranchElement(branch);
			CreateMinimapDataBranchElement(branch);

			return branch;
		}

		internal void DeleteDataBranch(DataBranch branch)
		{
			RemoveDataBranchElement(branch);
			RemoveMinimapDataBranchElement(branch);

			nodeGraph.DeleteDataBranch(branch);
		}

		internal void UpdateDataBranchBezier(DataBranch branch)
		{
			DataSlot inputSlot = branch.inputSlot;
			DataSlot outputSlot = branch.outputSlot;

			if (inputSlot == null || outputSlot == null)
			{
				return;
			}

			NodeGraph nodeGraph = this.nodeGraph;

			if (!inputSlot.isVisible)
			{
				Rect inRect = new Rect();
				if (TryGetBehaviourPosition(branch.inBehaviour, out inRect))
				{
					Vector2 endPosition = new Vector2(inRect.x, inRect.center.y);
					branch.lineBezier.endPosition = endPosition;
					branch.lineBezier.endControl = endPosition - EditorGUITools.kBezierTangentOffset;
				}
				else
				{
					DataBranchRerouteNodeEditor rerouteNodeEditor = GetNodeEditorFromID(branch.inNodeID) as DataBranchRerouteNodeEditor;
					if (rerouteNodeEditor != null)
					{
						Vector2 endPosition = rerouteNodeEditor.rect.center;
						branch.lineBezier.endPosition = endPosition;
						branch.lineBezier.endControl = endPosition - EditorGUITools.kBezierTangentOffset;
					}
				}
			}

			if (!outputSlot.isVisible)
			{
				Rect outRect = new Rect();
				if (TryGetBehaviourPosition(branch.outBehaviour, out outRect))
				{
					Vector2 startPosition = new Vector2(outRect.xMax, outRect.center.y);
					branch.lineBezier.startPosition = startPosition;
					branch.lineBezier.startControl = startPosition + EditorGUITools.kBezierTangentOffset;
				}
				else
				{
					DataBranchRerouteNodeEditor rerouteNodeEditor = GetNodeEditorFromID(branch.outNodeID) as DataBranchRerouteNodeEditor;
					if (rerouteNodeEditor != null)
					{
						Vector2 startPosition = rerouteNodeEditor.rect.center;
						branch.lineBezier.startPosition = startPosition;
						branch.lineBezier.startControl = startPosition + EditorGUITools.kBezierTangentOffset;
					}
				}
			}
		}

		private Dictionary<int, DataBranchElement> _DataBranchElements = new Dictionary<int, DataBranchElement>();

		DataBranchElement CreateDataBranchElement(DataBranch branch)
		{
			var branchElement = new DataBranchElement(this);
			graphView.dataBranchUnderlayLayer.Add(branchElement);
			_DataBranchElements.Add(branch.branchID, branchElement);

			branchElement.branch = branch;
			branchElement.Update();
			return branchElement;
		}

		void RemoveDataBranchElement(DataBranch branch)
		{
			if (_DataBranchElements.TryGetValue(branch.branchID, out var branchElement))
			{
				branchElement.RemoveFromHierarchy();
				_DataBranchElements.Remove(branch.branchID);
			}
		}

		void UpdateBranchies()
		{
			for (int i = graphView.dataBranchUnderlayLayer.childCount - 1; i >= 0; i--)
			{
				DataBranchElement branchElement = graphView.dataBranchUnderlayLayer[i] as DataBranchElement;
				if (branchElement != null)
				{
					var branch = branchElement.branch;
					if (branch == null || !_DataBranchElements.ContainsKey(branch.branchID) && nodeGraph.GetDataBranchFromID(branch.branchID) != branch)
					{
						branchElement.RemoveFromHierarchy();
					}
				}
			}

			for (int count = nodeGraph.dataBranchCount, i = count - 1; i >= 0; i--)
			{
				DataBranch branch = nodeGraph.GetDataBranchFromIndex(i);

				if (_DataBranchElements.TryGetValue(branch.branchID, out var branchElement))
				{
					branchElement.branch = branch;
					branchElement.Update();
				}
				else
				{
					CreateDataBranchElement(branch);
				}
			}

			using (Arbor.Pool.ListPool<int>.Get(out var removeList))
			{
				foreach (var pair in _DataBranchElements)
				{
					int branchID = pair.Key;
					DataBranchElement branchElement = pair.Value;
					if (nodeGraph.GetDataBranchFromID(pair.Key) == null)
					{
						removeList.Add(branchID);

						branchElement.RemoveFromHierarchy();
					}
				}

				foreach (var branchID in removeList)
				{
					_DataBranchElements.Remove(branchID);
				}
			}
		}

		private Dictionary<int, DottedBezierElement> _MinimapDataBranchElements = new Dictionary<int, DottedBezierElement>();

		DottedBezierElement CreateMinimapDataBranchElement(DataBranch branch)
		{
			var branchElement = new DottedBezierElement(minimapView.contentContainer)
			{
				pickingMode = PickingMode.Ignore,
				shadow = false,
				edgeWidth = 8f,
				space = 5f,
				tex = EditorContents.dataConnectionTexture,
			};
			branchElement.userData = branch;
			branchElement.AddManipulator(new MinimapTransformManipulator(minimapView, () =>
			{
				UpdateMinimapBranch(branchElement, branchElement.userData as DataBranch);
			}));
			minimapView.dataBranchLayer.Add(branchElement);
			_MinimapDataBranchElements.Add(branch.branchID, branchElement);

			UpdateMinimapBranch(branchElement, branch);

			return branchElement;
		}

		void RemoveMinimapDataBranchElement(DataBranch branch)
		{
			if (_MinimapDataBranchElements.TryGetValue(branch.branchID, out var branchElement))
			{
				branchElement.RemoveFromHierarchy();
				_MinimapDataBranchElements.Remove(branch.branchID);
			}
		}

		void UpdateMinimapBranchies()
		{
			for (int i = minimapView.dataBranchLayer.childCount - 1; i >= 0; i--)
			{
				DottedBezierElement branchElement = minimapView.dataBranchLayer[i] as DottedBezierElement;
				if (branchElement != null)
				{
					var branch = branchElement.userData as DataBranch;
					if (branch == null || !_MinimapDataBranchElements.ContainsKey(branch.branchID) && nodeGraph.GetDataBranchFromID(branch.branchID) != branch)
					{
						branchElement.RemoveFromHierarchy();
					}
				}
			}

			for (int count = nodeGraph.dataBranchCount, i = count - 1; i >= 0; i--)
			{
				DataBranch branch = nodeGraph.GetDataBranchFromIndex(i);

				DottedBezierElement branchElement;
				if (_MinimapDataBranchElements.TryGetValue(branch.branchID, out branchElement))
				{
					branchElement.userData = branch;
					UpdateMinimapBranch(branchElement, branch);					
				}
				else
				{
					CreateMinimapDataBranchElement(branch);
				}

			}

			using (Arbor.Pool.ListPool<int>.Get(out var removeList))
			{
				foreach (var pair in _MinimapDataBranchElements)
				{
					int branchID = pair.Key;
					DottedBezierElement branchElement = pair.Value;
					if (nodeGraph.GetDataBranchFromID(pair.Key) == null)
					{
						removeList.Add(branchID);

						branchElement.RemoveFromHierarchy();
					}
				}

				foreach (var branchID in removeList)
				{
					_MinimapDataBranchElements.Remove(branchID);
				}
			}
		}

		void UpdateMinimapBranch(DottedBezierElement branchElement, DataBranch branch)
		{
			if (branch == null || !minimapView.isSettedTransform)
			{
				return;
			}

			DataSlot inputSlot = branch.inputSlot;
			DataSlot outputSlot = branch.outputSlot;

			if (inputSlot == null || outputSlot == null)
			{
				return;
			}

			UpdateDataBranchBezier(branch);

			Color outputSlotColor = EditorGUITools.GetTypeColor(outputSlot.connectableType);
			Color inputSlotColor = EditorGUITools.GetTypeColor(inputSlot.connectableType);

			float alpha = 1.0f;
			if (!branch.enabled)
			{
				alpha = 0.1f;
			}

			outputSlotColor.a = alpha;
			inputSlotColor.a = alpha;

			if (Application.isPlaying && !branch.isUsed)
			{
				outputSlotColor *= Color.gray;
				inputSlotColor *= Color.gray;
			}

			outputSlotColor = EditorGUITools.GetColorOnGUI(outputSlotColor);
			inputSlotColor = EditorGUITools.GetColorOnGUI(inputSlotColor);

			branchElement.startPosition = minimapView.GraphToMinimap(branch.lineBezier.startPosition);
			branchElement.startControl = minimapView.GraphToMinimap(branch.lineBezier.startControl);
			branchElement.startColor = outputSlotColor;
			branchElement.endPosition = minimapView.GraphToMinimap(branch.lineBezier.endPosition);
			branchElement.endControl = minimapView.GraphToMinimap(branch.lineBezier.endControl);
			branchElement.endColor = inputSlotColor;
			
			branchElement.UpdateLayout();
		}

		internal bool wantsRepaint = false;
		internal double _LastRenderedTime;

		internal void Update()
		{
			UpdateEditable();

			_DropNodesElements.Clear();

			UpdateRenameOverlayGUI();

			wantsRepaint = false;

			foreach (NodeEditor nodeEditor in _NodeEditors)
			{
				if (nodeEditor != null)
				{
					nodeEditor.Update();
				}
			}

			if (_CreateParameterContainerButton != null)
			{
				_CreateParameterContainerButton.SetEnabled(editable);
			}

			UpdateBranchies();
			UpdateMinimapBranchies();

			OnUpdate();

			if (wantsRepaint && _LastRenderedTime + 0.033f < EditorApplication.timeSinceStartup)
			{
				_LastRenderedTime = EditorApplication.timeSinceStartup;
				Repaint();
				wantsRepaint = false;
			}
		}

		internal void OnRepainted()
		{
			foreach (NodeEditor nodeEditor in _NodeEditors)
			{
				if (nodeEditor != null)
				{
					nodeEditor.DoRepaintedEvent();
				}
			}
		}

		void FinalizeDataBranches()
		{
			foreach (var pair in _DataBranchElements)
			{
				pair.Value.RemoveFromHierarchy();
			}
			_DataBranchElements.Clear();

			foreach (var pair in _MinimapDataBranchElements)
			{
				pair.Value.RemoveFromHierarchy();
			}
			_MinimapDataBranchElements.Clear();
		}

		void FinalizeParameterContainerEditor()
		{
			if (_ParameterContainerEditor != null)
			{
				Object.DestroyImmediate(_ParameterContainerEditor);
				_ParameterContainerEditor = null;
				
				UpdateParametersElement();
			}
		}

		public void OnUndoRedoPerformed()
		{
			RebuildIfNecessary();

			UpdateParameterContainerEditor();

			InitializeVisibleNodes();

			for (int i = 0; i < _NodeEditors.Count; i++)
			{
				var nodeEditor = _NodeEditors[i];
				nodeEditor.DoUndoRedoPerformed();
			}
		}

		protected virtual void OnFinalizeGraph()
		{
		}

		public void RaiseOnChangedNodes()
		{
			onChangedNodes?.Invoke();
		}

		public Rect GetHeaderContentRect(Node node)
		{
			NodeEditor nodeEditor = GetNodeEditor(node);
			if (nodeEditor != null)
			{
				return nodeEditor.GetNameRect();
			}

			return node.position;
		}

		private RenameOverlayElement GetRenameOverlay()
		{
			if (_RenameOverlay == null)
			{
				_RenameOverlay = new RenameOverlayElement(OnRenameEnded);
			}
			return _RenameOverlay;
		}

		public void BeginRename(int nodeID, string name, bool createNode = false)
		{
			_BeginRenameUndoGroup = Undo.GetCurrentGroup();

			hostWindow.Focus();

			var renameOverlay = GetRenameOverlay();
			if (renameOverlay.parent == null)
			{
				graphView.contentContainer.Add(renameOverlay);
			}
			renameOverlay.BeginRename(name, nodeID, 0.0f);

			_RenameCreateNode = createNode;
		}

		void OnRenameEnded(string name, int userData)
		{
			int nodeID = userData;
			Node node = nodeGraph.GetNodeFromID(nodeID);
			if (node == null)
			{
				return;
			}

			NodeEditor nodeEditor = GetNodeEditor(node);
			if (nodeEditor == null)
			{
				return;
			}

			nodeEditor.OnRename(name);

			if (_RenameCreateNode)
			{
				for (int group = _BeginRenameUndoGroup; group <= Undo.GetCurrentGroup(); group++)
				{
					Undo.CollapseUndoOperations(group);
				}
				_RenameCreateNode = false;
			}

			RaiseOnChangedNodes();
		}

		void UpdateRenameOverlayGUI()
		{
			var renameOverlay = GetRenameOverlay();
			if (renameOverlay.IsRenaming() && renameOverlay.parent != null)
			{
				int nodeID = renameOverlay.renameUserData;
				Node node = nodeGraph.GetNodeFromID(nodeID);
				if (node != null)
				{
					Rect renamePosition = GetHeaderContentRect(node);

					renamePosition = graphView.GraphToElement(renameOverlay.parent, renamePosition);
					renameOverlay.SetLayout(renamePosition);
				}
			}
		}

		public bool IsRenaming()
		{
			var renameOverlay = GetRenameOverlay();
			if (renameOverlay.IsRenaming())
				return !renameOverlay.isWaitingForDelay;
			return false;
		}

		public bool IsRenaming(int instanceID)
		{
			var renameOverlay = GetRenameOverlay();
			if (renameOverlay.IsRenaming() && renameOverlay.renameUserData == instanceID)
				return !renameOverlay.isWaitingForDelay;
			return false;
		}

		DottedBezierElement GetDragDataBranchElement()
		{
			if (_DragDataBranchElement == null)
			{
				_DragDataBranchElement = new DottedBezierElement(graphView.contentContainer)
				{
					startColor = dragBezierColor,
					endColor = dragBezierColor,
					shadow = true,
					shadowColor = bezierShadowColor,
					edgeWidth = 16.0f,
					space = 10.0f,
					tex = EditorContents.dataConnectionTexture,
				};
			}
			return _DragDataBranchElement;
		}

		public void BeginDragDataBranch(int nodeID)
		{
			var dragDataBranchElement = GetDragDataBranchElement();
			graphView.dataBranchOverlayLayer.Add(dragDataBranchElement);
			_DragDataBranchEnable = true;
			_DragDataBranchNodeID = nodeID;

			graphView.autoScroll = true;
		}

		public void EndDragDataBranch()
		{
			if (_DragDataBranchElement != null)
			{
				_DragDataBranchElement.RemoveFromHierarchy();
			}
			_DragDataBranchEnable = false;
			_DragDataBranchNodeID = 0;

			graphView.autoScroll = false;

			hostWindow.Repaint();
		}

		public void DragDataBranchBezier(Vector2 start, Vector2 startTangent, Vector2 end, Vector2 endTangent)
		{
			var dragDataBranchElement = GetDragDataBranchElement();
			dragDataBranchElement.startPosition = start;
			dragDataBranchElement.startControl = startTangent;
			dragDataBranchElement.endPosition = end;
			dragDataBranchElement.endControl = endTangent;
		}

		public static readonly Color dragBezierColor = new Color(1.0f, 0.8f, 0.8f, 1.0f);
		public static readonly Color bezierShadowColor = new Color(0, 0, 0, 1.0f);

		public virtual bool IsDraggingBranch(Node node)
		{
			return _DragDataBranchEnable && _DragDataBranchNodeID == node.nodeID;
		}

		public virtual bool IsDragBranch()
		{
			return _DragDataBranchEnable;
		}

		public void SelectAll()
		{
			Undo.IncrementCurrentGroup();

			Undo.RecordObject(this, "Selection State");

			ClearSelection();

			for (int i = 0, count = nodeGraph.nodeCount; i < count; i++)
			{
				Node node = nodeGraph.GetNodeFromIndex(i);
				AddSelectionNodeInternal(node.nodeID);
			}

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			EditorUtility.SetDirty(this);
		}

		public bool HasSelection()
		{
			return _Selection != null && _Selection.Count > 0;
		}

		public bool IsSelection(Node node)
		{
			return _Selection != null && _Selection.Contains(node.nodeID);
		}

		void ClearSelectionInternal(bool applyNodeList = true)
		{
			foreach (int oldNodeID in _Selection)
			{
				NodeEditor nodeEditor = GetNodeEditorFromID(oldNodeID);
				if (nodeEditor != null)
				{
					nodeEditor.SetSelection(false);
				}
			}

			_Selection.Clear();

			if (applyNodeList)
			{
				_NodeListElement.ClearSelection();
			}
		}

		void AddSelectionNodeInternal(int nodeID, bool applyNodeList = true)
		{
			if (!_Selection.Contains(nodeID))
			{
				_Selection.Add(nodeID);

				NodeEditor nodeEditor = GetNodeEditorFromID(nodeID);
				if (nodeEditor != null)
				{
					if (applyNodeList)
					{
						_NodeListElement.AddToSelection(nodeEditor);
					}
					nodeEditor.SetSelection(true);
				}
			}
		}

		void RemoveSelectionNodeInternal(int nodeID)
		{
			if (_Selection.Remove(nodeID))
			{
				NodeEditor nodeEditor = GetNodeEditorFromID(nodeID);
				if (nodeEditor != null)
				{
					_NodeListElement.RemoveFromSelection(nodeEditor);
					nodeEditor.SetSelection(false);
				}
			}
		}

		public void SetSelectNode(Node node)
		{
			int nodeID = node.nodeID;

			Undo.RecordObject(this, "Selection Node");

			ClearSelectionInternal();
			AddSelectionNodeInternal(nodeID);

			EditorUtility.SetDirty(this);
		}

		public void AddSelectNode(Node node)
		{
			int nodeID = node.nodeID;

			Undo.RecordObject(this, "Selection Node");

			if (_Selection.Contains(nodeID))
			{
				RemoveSelectionNodeInternal(nodeID);
			}
			else
			{
				AddSelectionNodeInternal(nodeID);
			}

			EditorUtility.SetDirty(this);
		}

		public void ChangeSelectNode(Node node, bool add)
		{
			int nodeID = node.nodeID;

			if (add)
			{
				Undo.IncrementCurrentGroup();

				AddSelectNode(node);

				Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
			}
			else
			{
				if (!_Selection.Contains(nodeID))
				{
					Undo.IncrementCurrentGroup();

					SetSelectNode(node);

					Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
				}
				HandleUtility.Repaint();
			}
		}

		public bool ContainsNodes(Vector2 point)
		{
			for (int i = 0, count = _NodeEditors.Count; i < count; i++)
			{
				NodeEditor nodeEditor = _NodeEditors[i];

				if (nodeEditor.IsSelectPoint(point))
				{
					return true;
				}
			}

			return false;
		}

		public List<NodeEditor> GetNodeEditorsInRect(Rect rect)
		{
			List<NodeEditor> nodeEditors = new List<NodeEditor>();

			for (int i = 0, count = _NodeEditors.Count; i < count; i++)
			{
				NodeEditor nodeEditor = _NodeEditors[i];

				if (nodeEditor.IsSelectRect(rect))
				{
					nodeEditors.Add(nodeEditor);
				}
			}

			return nodeEditors;
		}

		public void SelectNodesInRect(Rect rect, bool actionKey, bool shiftKey)
		{
			Undo.RecordObject(this, "Selection State");

			List<int> newSelection = new List<int>();

			for (int i = 0, count = _NodeEditors.Count; i < count; i++)
			{
				NodeEditor nodeEditor = _NodeEditors[i];

				if (nodeEditor.IsSelectRect(rect))
				{
					newSelection.Add(nodeEditor.nodeID);
				}
			}

			if (!actionKey && !shiftKey)
			{
				for (int i = _Selection.Count - 1; i >= 0; i--)
				{
					int nodeID = _Selection[i];
					if (!newSelection.Contains(nodeID))
					{
						_Selection.RemoveAt(i);

						NodeEditor nodeEditor = GetNodeEditorFromID(nodeID);
						if (nodeEditor != null)
						{
							_NodeListElement.RemoveFromSelection(nodeEditor);
							nodeEditor.SetSelection(false);
						}
					}
				}
			}

			foreach (var selectable in newSelection)
			{
				if (actionKey)
				{
					RemoveSelectionNodeInternal(selectable);
				}
				else
				{
					AddSelectionNodeInternal(selectable);
				}
			}

			EditorUtility.SetDirty(this);
		}

		internal void ClearSelection()
		{
			Undo.RecordObject(this, "Selection State");

			ClearSelectionInternal();

			EditorUtility.SetDirty(this);
		}

		internal void BeginSelection(bool clear)
		{
			_OldSelection = new List<int>(_Selection);
			if (clear)
			{
				ClearSelection();
			}

			graphView.autoScroll = true;
		}

		internal void EndSelection()
		{
			if (_OldSelection != null)
			{
				_OldSelection.Clear();
			}

			graphView.autoScroll = false;
		}

		internal void CancelSelection()
		{
			Undo.RecordObject(this, "Selection State");

			ClearSelectionInternal();

			if (_OldSelection != null)
			{
				_Selection = _OldSelection;
			}

			foreach (int nodeID in _Selection)
			{
				NodeEditor nodeEditor = GetNodeEditorFromID(nodeID);
				if (nodeEditor != null)
				{
					nodeEditor.SetSelection(true);
				}
			}

			if (_OldSelection != null)
			{
				_OldSelection.Clear();
			}

			graphView.autoScroll = false;

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			EditorUtility.SetDirty(this);
		}

		public void SelectNodes(Node[] nodes)
		{
			ClearSelectionInternal();

			for (int nodeIndex = 0; nodeIndex < nodes.Length; nodeIndex++)
			{
				Node node = nodes[nodeIndex];

				AddSelectionNodeInternal(node.nodeID);

				INodeBehaviourContainer behaviours = node as INodeBehaviourContainer;
				if (behaviours != null)
				{
					int behaviourCount = behaviours.GetNodeBehaviourCount();
					for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
					{
						NodeBehaviour behaviour = behaviours.GetNodeBehaviour<NodeBehaviour>(behaviourIndex);
						EditorUtility.SetDirty(behaviour);
					}
				}
			}
		}

		public void OnAutoLayoutNode(Node node)
		{
			if (_DragNodePositions.Count == 0 ||
				_DragNodePositions.ContainsKey(node) ||
				_SaveNodePositions.ContainsKey(node))
			{
				return;
			}

			_SaveNodePositions[node] = node.position;
		}

		public void RegisterDragNode(Node node)
		{
			_SaveNodePositions[node] = _DragNodePositions[node] = node.position;
			graphView.autoScroll = true;
		}

		public void OnBeginDragNodes(Node selectTargetNode, Vector2 graphMousePosition, bool actionKey, bool shiftKey, bool altKey)
		{
			ChangeSelectNode(selectTargetNode, actionKey || shiftKey);

			Undo.IncrementCurrentGroup();

			_BeginMousePosition = graphMousePosition;

			_DragNodeDistance = Vector2.zero;
			for (int i = 0; i < selectionCount; i++)
			{
				NodeEditor nodeEditor = GetSelectionNodeEditor(i);

				if (nodeEditor != null)
				{
					nodeEditor.OnBeginDrag(altKey);
				}
			}
		}

		public void OnEndDragNodes(Vector2 screenMousePosition)
		{
			if (_DropNodesElements.Count > 0)
			{
				if (editable)
				{
					Vector2 mousePosition = screenMousePosition;

					for (int elementIndex = 0; elementIndex < _DropNodesElements.Count; elementIndex++)
					{
						DropNodesElement element = _DropNodesElements[elementIndex];
						if (element.position.Contains(mousePosition))
						{
							List<Node> nodes = new List<Node>(_DragNodePositions.Keys);

							OnEscapeDragNodes();

							element.callback(nodes.ToArray());
							break;
						}
					}
				}

				_DropNodesElements.Clear();
			}
			_DragNodePositions.Clear();
			_SaveNodePositions.Clear();

			graphView.autoScroll = false;
		}

		public void OnEndDragNodes()
		{
			_DropNodesElements.Clear();
			_DragNodePositions.Clear();
			_SaveNodePositions.Clear();

			graphView.autoScroll = false;
		}

		protected virtual void OnDragNodes()
		{
		}

		internal void DoMoveNode(Node node, Rect position)
		{
			var nodeEditor = GetNodeEditor(node);
			if (nodeEditor != null)
			{
				Undo.RecordObject(nodeGraph, "Move Node");

				nodeEditor.rect = position;

				Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

				EditorUtility.SetDirty(nodeGraph);

				nodeEditor.nodeElement.UpdatePosition();
			}
		}

		public void OnDragNodes(Vector2 graphMousePosition)
		{
			if (!editable)
			{
				return;
			}

			_DragNodeDistance = graphMousePosition - _BeginMousePosition;

			bool isMove = false;

			foreach (KeyValuePair<Node, Rect> pair in _DragNodePositions)
			{
				Node node = pair.Key;
				Rect rect = pair.Value;

				Rect position = node.position;
				position.x = rect.x + _DragNodeDistance.x;
				position.y = rect.y + _DragNodeDistance.y;

				position = EditorGUITools.SnapPositionToGrid(position);

				if ((position.x != node.position.x || position.y != node.position.y))
				{
					isMove = true;

					DoMoveNode(node, position);
				}
			}

			if (isMove)
			{
				OnDragNodes();
				UpdateLayout();
			}
		}

		public void OnEscapeDragNodes()
		{
			if (editable)
			{
				bool isMove = false;

				foreach (KeyValuePair<Node, Rect> pair in _SaveNodePositions)
				{
					Node node = pair.Key;
					Rect position = pair.Value;

					position = EditorGUITools.SnapPositionToGrid(position);

					if ((position.x != node.position.x || position.y != node.position.y))
					{
						isMove = true;

						DoMoveNode(node, position);
					}
				}

				if (isMove)
				{
					OnDragNodes();
					UpdateLayout();
				}
			}

			_DropNodesElements.Clear();
			_DragNodePositions.Clear();
			_SaveNodePositions.Clear();

			graphView.autoScroll = false;
		}

		public bool IsDraggingNode(Node node)
		{
			return _DragNodePositions.ContainsKey(node);
		}

		private HashSet<int> _InVisibleNodes = new HashSet<int>();

		void InitializeVisibleNodes()
		{
			if (nodeGraph == null)
			{
				return;
			}

			ClearInvsibleNodes();
		}

		public void ClearInvsibleNodes()
		{
			foreach (var nodeID in _InVisibleNodes)
			{
				NodeEditor nodeEditor = GetNodeEditorFromID(nodeID);
				if (nodeEditor == null)
				{
					continue;
				}

				nodeEditor.nodeElement.visible = true;
			}

			_InVisibleNodes.Clear();
		}

		bool PredicateInvisibleNodes(int nodeID)
		{
			NodeEditor nodeEditor = GetNodeEditorFromID(nodeID);
			if (nodeEditor == null)
			{
				return true;
			}

			bool isVisible = !nodeEditor.isLayouted || nodeEditor.IsDraggingVisible();
			if (!isVisible)
			{
				Rect nodePosition = nodeEditor.nodeElement.boundingBox;

				isVisible = OverlapsViewArea(nodePosition);
			}

			if (isVisible)
			{
				nodeEditor.nodeElement.visible = true;
			}

			return isVisible;
		}

		private System.Predicate<int> _PredicateInvisibleNodes;

		internal void UpdateVisibleNodes()
		{
			if (_PredicateInvisibleNodes == null)
			{
				_PredicateInvisibleNodes = PredicateInvisibleNodes;
			}
			_InVisibleNodes.RemoveWhere(_PredicateInvisibleNodes);

			int nodeCount = nodeGraph.nodeCount;
			for (int i = 0; i < nodeCount; i++)
			{
				Node node = nodeGraph.GetNodeFromIndex(i);
				if (IsDraggingNode(node) || IsDraggingBranch(node))
				{
					continue;
				}

				NodeEditor nodeEditor = GetNodeEditor(node);
				if (nodeEditor == null || !(nodeEditor.isLayouted && nodeEditor.isRepainted) || nodeEditor.IsDraggingVisible())
				{
					continue;
				}

				Rect nodePosition = nodeEditor.nodeElement.boundingBox;
				
				bool overlapsViewArea = OverlapsViewArea(nodePosition);				
				if (!overlapsViewArea)
				{
					AddInvisibleNode(node.nodeID);
				}
			}
		}

		void AddInvisibleNode(int nodeID)
		{
			if (_InVisibleNodes.Add(nodeID))
			{
				NodeEditor nodeEditor = GetNodeEditorFromID(nodeID);
				if (nodeEditor == null)
				{
					return;
				}

				nodeEditor.isRepainted = false;
				nodeEditor.nodeElement.visible = false;
			}
		}

		void RemoveInvisibleNode(int nodeID)
		{
			if (_InVisibleNodes.Remove(nodeID))
			{
				NodeEditor nodeEditor = GetNodeEditorFromID(nodeID);
				if (nodeEditor == null)
				{
					return;
				}

				nodeEditor.nodeElement.visible = true;
				nodeEditor.nodeElement.MarkDirtyRepaint();
			}
		}

		public void VisibleNode(Node node)
		{
			VisibleNode(node.nodeID);
		}

		public void VisibleNode(int nodeID)
		{
			RemoveInvisibleNode(nodeID);
		}

		public bool IsVisibleNode(int nodeID)
		{
			return !_InVisibleNodes.Contains(nodeID);
		}

		public void BeginFrameSelected(Vector2 frameSelectTarget)
		{
			graphView.FrameSelected(frameSelectTarget);
		}

		public void BeginFrameSelected()
		{
			if (!HasSelection())
			{
				return;
			}

			int selectionCount = this.selectionCount;

			Vector2 frameSelectTarget = Vector2.zero;
			for (int i = 0; i < selectionCount; i++)
			{
				NodeEditor nodeEditor = GetSelectionNodeEditor(i);
				frameSelectTarget += nodeEditor.rect.center;
			}
			frameSelectTarget /= (float)selectionCount;

			BeginFrameSelected(frameSelectTarget);
		}

		public void BeginFrameSelected(Node node, bool select = true)
		{
			if (select)
			{
				SetSelectNode(node);
			}

			NodeEditor nodeEditor = GetNodeEditor(node);
			BeginFrameSelected(nodeEditor.rect.center);
		}

		protected void OnCreatedNode(Node node, bool beginRename = true)
		{
			CreateNodeEditor(node);
			RaiseOnChangedNodes();
			SetSelectNode(node);
			if (beginRename)
			{
				BeginRename(node.nodeID, node.name, true);
			}
		}

		CalculatorNode CreateCalculatorInternal(Vector2 position, System.Type calculatorType)
		{
			CalculatorNode calculator = nodeGraph.CreateCalculator(calculatorType);

			if (calculator != null)
			{
				Undo.RecordObject(nodeGraph, "Created Calculator");

				BehaviourInfo behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(calculatorType);
				calculator.name = behaviourInfo.titleContent.text;
				calculator.position = EditorGUITools.SnapPositionToGrid(new Rect(position.x, position.y, Node.defaultWidth, 100));

				EditorUtility.SetDirty(nodeGraph);

				OnCreatedNode(calculator);
			}

			Repaint();

			return calculator;
		}

		public void CreateCalculator(Vector2 position, System.Type calculatorType)
		{
			Undo.IncrementCurrentGroup();

			CreateCalculatorInternal(position, calculatorType);

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
		}

		void CreateComment(Vector2 position)
		{
			Undo.IncrementCurrentGroup();

			CommentNode comment = nodeGraph.CreateComment();

			if (comment != null)
			{
				Undo.RecordObject(nodeGraph, "Created Comment");

				comment.position = EditorGUITools.SnapPositionToGrid(new Rect(position.x, position.y, Node.defaultWidth, 100));

				EditorUtility.SetDirty(nodeGraph);

				OnCreatedNode(comment);
			}

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			Repaint();
		}

		public void CreateGroup(Vector2 position)
		{
			Undo.IncrementCurrentGroup();

			GroupNode group = nodeGraph.CreateGroup();

			if (group != null)
			{
				Undo.RecordObject(nodeGraph, "Created Group");

				group.position = EditorGUITools.SnapPositionToGrid(new Rect(position.x, position.y, Node.defaultWidth, 100));

				EditorUtility.SetDirty(nodeGraph);

				OnCreatedNode(group);
			}

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			Repaint();
		}

		public DataBranchRerouteNode CreateDataBranchRerouteNode(Vector2 position, System.Type dataType, Vector2 direction)
		{
			var rerouteNode = nodeGraph.CreateDataBranchRerouteNode(EditorGUITools.SnapToGrid(position), dataType, direction);
			if (rerouteNode != null)
			{
				OnCreatedNode(rerouteNode, false);
			}

			Repaint();

			return rerouteNode;
		}

		public DataBranchRerouteNode CreateDataBranchRerouteNode(Vector2 position, System.Type dataType)
		{
			return CreateDataBranchRerouteNode(position, dataType, DataBranchRerouteNode.kDefaultDirection);
		}

		void CreateCalculator(object obj)
		{
			MousePosition mousePosition = (MousePosition)obj;

			Rect buttonRect = new Rect(mousePosition.screenPoint, Vector2.zero);

			CalculateMenuWindow.instance.Init(mousePosition.guiPoint, buttonRect, CreateCalculator);
		}

		void CreateComment(object obj)
		{
			Vector2 position = (Vector2)obj;

			CreateComment(position);
		}

		void CreateGroup(object obj)
		{
			Vector2 position = (Vector2)obj;

			CreateGroup(position);
		}

		void CopyNodes()
		{
			Clipboard.CopyNodes(GetSelectionNodes());
		}

		void CutNodes()
		{
			Clipboard.CopyNodes(GetSelectionNodes());
			DeleteNodes();
		}

		public void DuplicateNodes(Vector2 position, Node[] sourceNodes, string undoName = "Duplicate Nodes")
		{
			Undo.IncrementCurrentGroup();

			Undo.RegisterCompleteObjectUndo(nodeGraph, undoName);

			Vector2 minPosition = new Vector2(float.MaxValue, float.MaxValue);

			for (int sourceNodeIndex = 0; sourceNodeIndex < sourceNodes.Length; sourceNodeIndex++)
			{
				Node sourceNode = sourceNodes[sourceNodeIndex];
				minPosition.x = Mathf.Min(sourceNode.position.x, minPosition.x);
				minPosition.y = Mathf.Min(sourceNode.position.y, minPosition.y);
			}

			position -= minPosition;

			Node[] duplicateNodes = Clipboard.DuplicateNodes(position, sourceNodes, nodeGraph, false);

			if (duplicateNodes != null && duplicateNodes.Length > 0)
			{
				EditorUtility.SetDirty(nodeGraph);

				foreach (var node in duplicateNodes)
				{
					CreateNodeEditor(node);
				}

				RaiseOnChangedNodes();

				Undo.RecordObject(this, undoName);

				SelectNodes(duplicateNodes);

				EditorUtility.SetDirty(this);
			}

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			_HostWindow.Repaint();
		}

		void DuplicateNodes(object obj)
		{
			Vector2 position = (Vector2)obj;

			DuplicateNodes(position, GetSelectionNodes());
		}

		void PasteNodes(object obj)
		{
			Vector2 position = (Vector2)obj;

			DuplicateNodes(position, Clipboard.GetClippedNodes(), "Paste Nodes");
		}

		internal string GetNodeTitle(Node node)
		{
			NodeEditor nodeEditor = GetNodeEditor(node);
			if (nodeEditor != null)
			{
				return nodeEditor.GetTitle();
			}

			return "Node";
		}

		public void DeleteNodes(Node[] deleteNodes)
		{
			Undo.IncrementCurrentGroup();
			int undoGroup = Undo.GetCurrentGroup();

			List<int> deleteNodeIDs = new List<int>();

			for (int deleteNodeIndex = 0; deleteNodeIndex < deleteNodes.Length; deleteNodeIndex++)
			{
				Node deleteNode = deleteNodes[deleteNodeIndex];
				if (deleteNode.IsDeletable())
				{
					if (nodeGraph.DeleteNode(deleteNode))
					{
						deleteNodeIDs.Add(deleteNode.nodeID);
						NodeEditorUtility.DeleteShowComment(deleteNode);
						DeleteNodeEditor(deleteNode);
					}
					else
					{
						string name = GetNodeTitle(deleteNode);
						Debug.LogErrorFormat(Localization.GetWord("DeleteError"), name);
					}
				}
			}

			RaiseOnChangedNodes();

			nodeGraph.OnValidateNodes();

			Undo.RecordObject(this, "Delete Nodes");

			for (int deleteNodeIndex = 0; deleteNodeIndex < deleteNodeIDs.Count; deleteNodeIndex++)
			{
				int deleteNodeID = deleteNodeIDs[deleteNodeIndex];
				RemoveSelectionNodeInternal(deleteNodeID);
			}

			Undo.CollapseUndoOperations(undoGroup);

			EditorUtility.SetDirty(this);

			DirtyGraphExtents();
		}

		void DeleteNodes()
		{
			DeleteNodes(GetSelectionNodes());
		}

		void ExpandAll(bool expanded, IList<int> nodes)
		{
			int nodeCount = nodes.Count;
			for (int i = 0; i < nodeCount; i++)
			{
				NodeEditor nodeEditor = GetNodeEditorFromID(nodes[i]);
				if (nodeEditor != null)
				{
					nodeEditor.ExpandAll(expanded);
					VisibleNode(nodeEditor.nodeID);
				}
			}
		}

		void ExpandAll(bool expanded)
		{
			for (int i = 0, count = nodeEditorCount; i < count; i++)
			{
				NodeEditor nodeEditor = GetNodeEditor(i);
				if (nodeEditor != null)
				{
					nodeEditor.ExpandAll(expanded);
					VisibleNode(nodeEditor.nodeID);
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

		void ExpandAllInSelectionNodes()
		{
			ExpandAll(true, _Selection);
		}

		void FoldAllInSelectionNodes()
		{
			ExpandAll(false, _Selection);
		}

		protected virtual void SetCreateNodeContextMenu(GenericMenu menu, MousePosition mousePosition, bool editable)
		{
		}

		public bool OnContextMenu(MousePosition mousePosition)
		{
			if (IsDragBranch())
			{
				return false;
			}

			bool editable = this.editable;

			GenericMenu menu = new GenericMenu();

			SetCreateNodeContextMenu(menu, mousePosition, editable);

			menu.AddSeparator("");

			if (editable)
			{
				menu.AddItem(EditorContents.createCalculator, false, CreateCalculator, mousePosition);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.createCalculator);
			}

			menu.AddSeparator("");

			if (editable)
			{
				menu.AddItem(EditorContents.createGroup, false, CreateGroup, mousePosition.guiPoint);
				menu.AddItem(EditorContents.createComment, false, CreateComment, mousePosition.guiPoint);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.createGroup);
				menu.AddDisabledItem(EditorContents.createComment);
			}

			menu.AddSeparator("");

			bool isCopyable = false;
			for (int selectionIndex = 0; selectionIndex < selectionCount; selectionIndex++)
			{
				NodeEditor nodeEditor = GetSelectionNodeEditor(selectionIndex);
				if (nodeEditor != null && nodeEditor.IsCopyable())
				{
					isCopyable = true;
					break;
				}
			}

			bool isDeletable = false;
			for (int i = 0; i < selectionCount; i++)
			{
				Node node = GetSelectionNode(i);
				if (node != null && node.IsDeletable())
				{
					isDeletable = true;
					break;
				}
			}

			if (isCopyable && isDeletable && editable)
			{
				menu.AddItem(EditorContents.cut, false, CutNodes);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.cut);
			}

			if (isCopyable)
			{
				menu.AddItem(EditorContents.copy, false, CopyNodes);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.copy);
			}

			if (Clipboard.hasCopyedNodes && editable)
			{
				menu.AddItem(EditorContents.paste, false, PasteNodes, mousePosition.guiPoint);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.paste);
			}

			menu.AddSeparator("");

			if (isCopyable && editable)
			{
				menu.AddItem(EditorContents.duplicate, false, DuplicateNodes, mousePosition.guiPoint);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.duplicate);
			}

			if (isDeletable && editable)
			{
				menu.AddItem(EditorContents.delete, false, DeleteNodes);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.delete);
			}

			menu.AddSeparator("");

			if (HasSelection())
			{
				menu.AddItem(EditorContents.expandAll, false, ExpandAllInSelectionNodes);
				menu.AddItem(EditorContents.collapseAll, false, FoldAllInSelectionNodes);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.expandAll);
				menu.AddDisabledItem(EditorContents.collapseAll);
			}

			menu.ShowAsContext();

			return true;
		}

		internal bool OnValidateCommand(string commandName)
		{
			switch (commandName)
			{
				case "Cut":
				case "Duplicate":
				case "Delete":
				case "SoftDelete":
					if (HasSelection() && editable)
					{
						return true;
					}
					break;
				case "Copy":
				case "FrameSelected":
					if (HasSelection())
					{
						return true;
					}
					break;
				case "Paste":
					if (Clipboard.hasCopyedNodes && editable)
					{
						return true;
					}
					break;
				case "SelectAll":
					if (nodeGraph.nodeCount > 0)
					{
						return true;
					}
					break;
			}

			return false;
		}

		internal bool OnExecuteCommand(string commandName, Vector2 mousePosition)
		{
			switch (commandName)
			{
				case "Copy":
					CopyNodes();
					return true;
				case "Cut":
					CutNodes();
					return true;
				case "Paste":
					PasteNodes(mousePosition);
					return true;
				case "Duplicate":
					DuplicateNodes(mousePosition);
					return true;
				case "FrameSelected":
					BeginFrameSelected();
					return true;
				case "Delete":
				case "SoftDelete":
					DeleteNodes();
					return true;
				case "SelectAll":
					SelectAll();
					return true;
			}
			return false;
		}

		protected virtual int NodeListSortComparison(NodeEditor a, NodeEditor b)
		{
			return NodeListElement.Defaults.SortComparison(a, b);
		}

		void OnSelectionChange(IEnumerable<NodeEditor> nodeEditors)
		{
			Undo.IncrementCurrentGroup();

			Undo.RecordObject(this, "Selection Node");

			ClearSelectionInternal(false);

			foreach(var nodeEditor in nodeEditors)
			{
				nodeEditor.nodeElement.BringToFront();
				AddSelectionNodeInternal(nodeEditor.nodeID, false);
			}
		}

		void CreateParameterContainer()
		{
			int undoGroup = Undo.GetCurrentGroup();

			ParameterContainerInternal parameterContainer = ParameterContainerInternal.Create(_NodeGraph.gameObject, ParameterContainerEditorUtility.ParameterContainerType, _NodeGraph);
#if !ARBOR_DEBUG
			parameterContainer.hideFlags |= HideFlags.HideInInspector | HideFlags.HideInHierarchy;
#endif

			Undo.RecordObject(_NodeGraph, "Create ParameterContainer");

			ParameterContainerEditorUtility.SetParameterContainer(_NodeGraph, parameterContainer);

			Undo.CollapseUndoOperations(undoGroup);

			EditorUtility.SetDirty(_NodeGraph);

			UpdateParameterContainerEditor();
		}

		private ParameterContainerInternalInspector _ParameterContainerEditor = null;

		void UpdateParameterContainerEditor()
		{
			var parameterContainer = _NodeGraph != null ? _NodeGraph.parameterContainer : null;
			if (_ParameterContainerEditor != null && (parameterContainer == null || _ParameterContainerEditor.target == null || _ParameterContainerEditor.target != parameterContainer))
			{
				FinalizeParameterContainerEditor();
			}

			if (parameterContainer != null)
			{
				if (_ParameterContainerEditor == null)
				{
					_ParameterContainerEditor = Editor.CreateEditor(parameterContainer) as ParameterContainerInternalInspector;
				}

				_ParameterContainerEditor.onRemoveComponent -= FinalizeParameterContainerEditor;
				_ParameterContainerEditor.onRemoveComponent += FinalizeParameterContainerEditor;

				_ParameterContainerEditor.isInParametersPanel = true;
			}

			UpdateParametersElement();
		}

		void UpdateParametersElement()
		{
			if (_ParametersElement == null)
			{
				return;
			}

			if (_ParameterContainerEditor != null)
			{
				if (_CreateParameterContainerButton != null && _CreateParameterContainerButton.parent != null)
				{
					_CreateParameterContainerButton.RemoveFromHierarchy();
				}

				if (_ParametersEditorElement == null)
				{
					_ParametersEditorElement = new InspectorElement(_ParameterContainerEditor)
					{
						focusable = false,
					};

					IMGUIContainer imguiContainer = _ParametersEditorElement.Q<IMGUIContainer>(className: InspectorElement.iMGUIContainerUssClassName);
					if (imguiContainer != null)
					{
						System.Action onGUIHandler = imguiContainer.onGUIHandler;
						System.Action newOnGUI = () =>
						{
							var editorTarget = _ParameterContainerEditor.target;
							bool isValidObject = ComponentUtility.IsValidObject(editorTarget);
							HideFlags hideFlags = 0;
							if (isValidObject)
							{
								hideFlags = editorTarget.hideFlags;
								if (!imguiContainer.enabledInHierarchy)
								{
									editorTarget.hideFlags |= HideFlags.NotEditable;
								}
							}

							try
							{
								GUI.BeginGroup(imguiContainer.contentRect);

								if (onGUIHandler != null)
								{
									onGUIHandler();
								}

								GUI.EndGroup();
							}
							finally
							{
								if (isValidObject && ComponentUtility.IsValidObject(editorTarget))
								{
									editorTarget.hideFlags = hideFlags;
								}
							}
						};

						imguiContainer.onGUIHandler = newOnGUI;
					}
				}
				if (_ParametersEditorElement.parent == null)
				{
					_ParametersElement.Add(_ParametersEditorElement);
				}
			}
			else
			{
				if (_ParametersEditorElement != null)
				{
					if (_ParametersEditorElement.parent != null)
					{
						_ParametersEditorElement.RemoveFromHierarchy();
					}
					_ParametersEditorElement = null;
				}

				if (_CreateParameterContainerButton == null)
				{
					var button = new Button(CreateParameterContainer);

					_CreateParameterContainerButton = button;
					_CreateParameterContainerButton.AddManipulator(new LocalizationManipulator("Create", LocalizationManipulator.TargetText.Text));
				}

				if (_CreateParameterContainerButton.parent == null)
				{
					_ParametersElement.Add(_CreateParameterContainerButton);
				}
			}
		}

		void ShowDataBranchValueAll(object visible)
		{
			Undo.RecordObject(nodeGraph, "Change Visible Values");
			int branchCount = nodeGraph.dataBranchCount;
			for (int i = 0; i < branchCount; i++)
			{
				DataBranch branch = nodeGraph.GetDataBranchFromIndex(i);
				if (branch != null)
				{
					branch.showDataValue = (bool)visible;
				}
			}
			EditorUtility.SetDirty(nodeGraph);
		}

		protected virtual bool HasViewMenu()
		{
			return false;
		}

		protected virtual void OnSetViewMenu(GenericMenu menu)
		{
		}

		public void SetViewMenu(GenericMenu menu)
		{
			menu.AddItem(EditorContents.expandAll, false, ExpandAll);
			menu.AddItem(EditorContents.collapseAll, false, FoldAll);
			menu.AddSeparator("");
			menu.AddItem(EditorContents.nodeCommentViewModeNormal, ArborSettings.nodeCommentViewMode == NodeCommentViewMode.Normal, () =>
			{
				ArborSettings.nodeCommentViewMode = NodeCommentViewMode.Normal;
				Repaint();
			});
			menu.AddItem(EditorContents.nodeCommentViewModeShowAll, ArborSettings.nodeCommentViewMode == NodeCommentViewMode.ShowAll, () =>
			{
				ArborSettings.nodeCommentViewMode = NodeCommentViewMode.ShowAll;
				Repaint();
			});
			menu.AddItem(EditorContents.nodeCommentViewModeShowCommentedOnly, ArborSettings.nodeCommentViewMode == NodeCommentViewMode.ShowCommentedOnly, () =>
			{
				ArborSettings.nodeCommentViewMode = NodeCommentViewMode.ShowCommentedOnly;
				Repaint();
			});
			menu.AddItem(EditorContents.nodeCommentViewModeHideAll, ArborSettings.nodeCommentViewMode == NodeCommentViewMode.HideAll, () =>
			{
				ArborSettings.nodeCommentViewMode = NodeCommentViewMode.HideAll;
				Repaint();
			});
			menu.AddItem(EditorContents.dataSlotShowOutsideNode, ArborSettings.dataSlotShowMode == DataSlotShowMode.Outside, () =>
			{
				ArborSettings.dataSlotShowMode = DataSlotShowMode.Outside;
				Repaint();
			});
			menu.AddItem(EditorContents.dataSlotShowInsideNode, ArborSettings.dataSlotShowMode == DataSlotShowMode.Inside, () =>
			{
				ArborSettings.dataSlotShowMode = DataSlotShowMode.Inside;
				Repaint();
			});
			menu.AddItem(EditorContents.dataSlotShowFlexibly, ArborSettings.dataSlotShowMode == DataSlotShowMode.Flexibly, () =>
			{
				ArborSettings.dataSlotShowMode = DataSlotShowMode.Flexibly;
				Repaint();
			});

			if (HasViewMenu())
			{
				menu.AddSeparator("");
				OnSetViewMenu(menu);
			}
		}

		protected virtual bool HasDebugMenu()
		{
			return false;
		}

		protected virtual void OnSetDebugMenu(GenericMenu menu)
		{
		}

		public void SetDenugMenu(GenericMenu menu)
		{
			menu.AddItem(EditorContents.showAllDataValuesAlways, ArborSettings.showDataValue, () => ArborSettings.showDataValue = !ArborSettings.showDataValue);
			menu.AddSeparator("");
			if (editable)
			{
				menu.AddItem(EditorContents.showAllDataValues, false, ShowDataBranchValueAll, true);
				menu.AddItem(EditorContents.hideAllDataValues, false, ShowDataBranchValueAll, false);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.showAllDataValues);
				menu.AddDisabledItem(EditorContents.hideAllDataValues);
			}
			if (HasDebugMenu())
			{
				menu.AddSeparator("");
				OnSetDebugMenu(menu);
			}
		}

		void OnDisable()
		{
			if (_NodeListElement != null)
			{
				_NodeListElement.graphEditor = null;
			}
		}

		void OnDestroy()
		{
			OnFinalizeGraph();

			if (_NodeListElement != null)
			{
				_NodeListElement.graphEditor = null;
				_NodeListElement.RemoveFromHierarchy();
			}

			if (_ParametersElement != null)
			{
				_ParametersElement.RemoveFromHierarchy();
			}

			FinalizeDataBranches();
			FinalizeNodeEditors();

			FinalizePopupLayer();

			FinalizeParameterContainerEditor();
		}

		public Node GetHoverNode(Vector2 position)
		{
			int nodeCount = nodeGraph.nodeCount;
			for (int i = 0; i < nodeCount; i++)
			{
				Node node = nodeGraph.GetNodeFromIndex(i);
				if (node is GroupNode)
				{
					continue;
				}

				NodeEditor nodeEditor = GetNodeEditor(node);
				if (nodeEditor != null && nodeEditor.IsHover(position))
				{
					return node;
				}
			}

			return null;
		}

		protected virtual Node GetActiveNode()
		{
			return null;
		}

		public bool LiveTracking()
		{
			if (nodeGraph == null)
			{
#if ARBOR_DEBUG
				Debug.LogWarning("nodeGraph == null");
#endif
				return false;
			}

			if (!ArborSettings.liveTracking)
			{
				return false;
			}

			Node activeNode = GetActiveNode();
			if (activeNode != null)
			{
				if (ArborSettings.liveTrackingHierarchy)
				{
					ISubGraphBehaviour subBehaviour = null;

					INodeBehaviourContainer container = activeNode as INodeBehaviourContainer;
					if (container != null)
					{
						for (int i = 0, count = container.GetNodeBehaviourCount(); i < count; i++)
						{
							NodeBehaviour behaviour = container.GetNodeBehaviour<NodeBehaviour>(i);

							if (behaviour != null && behaviour.enabled)
							{
								ISubGraphBehaviour subGraphBehaviour = behaviour as ISubGraphBehaviour;
								if (subGraphBehaviour != null)
								{
									subBehaviour = subGraphBehaviour;
									break;
								}
							}
						}
					}

					if (subBehaviour != null && !subBehaviour.isExternal && subBehaviour.GetSubGraph() != null)
					{
						NodeBehaviour behaviour = subBehaviour as NodeBehaviour;
						if (behaviour != null)
						{
							_HostWindow.ChangeCurrentNodeGraph(behaviour.GetInstanceID(), true);
							return true;
						}
					}
				}

				BeginFrameSelected(activeNode, false);
			}
			else if (ArborSettings.liveTrackingHierarchy && (!HasPlayState() || GetPlayState() == PlayState.Stopping))
			{
				NodeGraph parentGraph = nodeGraph.parentGraph;
				if (parentGraph != null)
				{
					var ownerObject = parentGraph.ownerBehaviourObject;
					int id = (ownerObject != null)? ownerObject.GetInstanceID() : parentGraph.GetInstanceID();
					_HostWindow.ChangeCurrentNodeGraph(id, true);
					return true;
				}
			}

			return false;
		}
	}
}