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
	using Arbor;
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	internal abstract class StateLinkElementBase : VisualElement
	{
		private static StateLinkSettingWindow s_StateLinkSettingWindow = new StateLinkSettingWindow();

		public readonly NodeEditor nodeEditor;
		public readonly StateMachineGraphEditor _GraphEditor;

		internal StateBehaviour _StateBehaviour;

		public StateLink stateLink;
		public System.Reflection.FieldInfo fieldInfo;
		public Bezier2D bezier = new Bezier2D();

		private StateLinkBranchElement _StateLinkBranchElement = null;
		private BezierElement _MinimapStateLinkBranchElement = null;

		private bool _On;
		private bool _IsLayouted = false;

		public bool on
		{
			get
			{
				return _On;
			}
		}

		public event System.Action onSettingsChanged;

		public void OpenSettingsWindow(Rect settingRect, bool onGUI)
		{
			NodeEditor nodeEditor = this.nodeEditor;
			s_StateLinkSettingWindow.Init(_GraphEditor.hostWindow, _StateBehaviour != null ? (Object)_StateBehaviour : (Object)_GraphEditor.nodeGraph, stateLink, fieldInfo, nodeEditor.node is StateLinkRerouteNode, OnSettingsChangedInternal);
			PopupWindowUtility.Show(settingRect, s_StateLinkSettingWindow, onGUI);
		}

		public StateLinkElementBase(NodeEditor nodeEditor, StateBehaviour behaviour)
		{
			this.nodeEditor = nodeEditor;
			_GraphEditor = nodeEditor.graphEditor as StateMachineGraphEditor;
			_StateBehaviour = behaviour;

			RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

			var graphEditor = nodeEditor.graphEditor;
			if (graphEditor != null)
			{
				SetEnabled(graphEditor.editable);
			}
		}

		internal void Repair(StateBehaviour stateBehaviour)
		{
			_StateBehaviour = stateBehaviour;
		}

		void OnGeometryChanged(GeometryChangedEvent e)
		{
			VisualElement target = e.target as VisualElement;
			if (UIElementsUtility.IsVisible(target))
			{
				_IsLayouted = true;
				UpdateBezier();
			}
		}

		List<VisualElement> _RegisterElements = new List<VisualElement>();

		void UnregisterParentElements()
		{
			foreach (var element in _RegisterElements)
			{
				element.UnregisterCallback<GeometryChangedEvent>(OnGeometryChangedParent);
			}
			_RegisterElements.Clear();
		}

		protected virtual void OnAttachToPanel(AttachToPanelEvent e)
		{
			var nodeEditor = this.nodeEditor;
			var nodeElement = nodeEditor.nodeElement;

			nodeElement.RegisterCallback<ChangeNodePositionEvent>(OnChangeNodePosition);
			nodeElement.RegisterCallback<RebuildElementEvent>(OnRebuildElement);
			nodeElement.RegisterCallback<UndoRedoPerformedEvent>(OnUndoRedoPerformed);

			UnregisterParentElements();

			var nodeElementParent = nodeElement.hierarchy.parent;
			var current = this.hierarchy.parent;
			while (current != null && current != nodeElementParent)
			{
				current.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedParent);

				_RegisterElements.Add(current);

				current = current.hierarchy.parent;
			}

			var stateLink = this.stateLink;
			if (stateLink != null)
			{
				stateLink.onConnectionChanged -= OnConnectionChanged;
				stateLink.onConnectionChanged += OnConnectionChanged;
			}

			var graphEditor = nodeEditor.graphEditor;
			if (graphEditor != null)
			{
				graphEditor.onChangedEditable -= OnChangedEditable;
				graphEditor.onChangedEditable += OnChangedEditable;
			}

			ShowBranch(stateLink.stateID != 0);
		}

		protected virtual void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			var nodeEditor = this.nodeEditor;
			var nodeElement = nodeEditor.nodeElement;
			nodeElement.UnregisterCallback<ChangeNodePositionEvent>(OnChangeNodePosition);
			nodeElement.UnregisterCallback<RebuildElementEvent>(OnRebuildElement);
			nodeElement.UnregisterCallback<UndoRedoPerformedEvent>(OnUndoRedoPerformed);

			UnregisterParentElements();

			var graphEditor = nodeEditor.graphEditor;
			if (graphEditor != null)
			{
				graphEditor.onChangedEditable -= OnChangedEditable;
			}

			var stateLink = this.stateLink;
			if (stateLink != null)
			{
				stateLink.onConnectionChanged -= OnConnectionChanged;
			}

			ShowBranch(false);
		}

		void OnGeometryChangedParent(GeometryChangedEvent e)
		{
			UpdateBezier();
		}

		void OnChangeNodePosition(ChangeNodePositionEvent e)
		{
			UpdateBezier();
		}

		protected virtual void OnRebuildElement(RebuildElementEvent e)
		{
			UpdateBezier();
		}

		void OnUndoRedoPerformed(UndoRedoPerformedEvent e)
		{
			UpdateBezier();

			OnConnectionChanged();

			OnUndoRedoPerformed();

			UpdateMinimapLineColor();
			UpdateMinimapLineArrow();
		}

		void OnChangedEditable(bool editable)
		{
			SetEnabled(editable);
		}

		public void Setup(StateLink stateLink, System.Reflection.FieldInfo fieldInfo)
		{
			bool changedStateLink = this.stateLink != stateLink;
			if (changedStateLink)
			{
				if (this.stateLink != null)
				{
					this.stateLink.onConnectionChanged -= OnConnectionChanged;
				}
				this.stateLink = stateLink;
				if (this.stateLink != null)
				{
					this.stateLink.onConnectionChanged += OnConnectionChanged;
				}
			}

			bool changedFieldInfo = this.fieldInfo != fieldInfo;
			if (changedFieldInfo)
			{
				this.fieldInfo = fieldInfo;
			}

			if (changedStateLink)
			{
				OnConnectionChanged();
			}

			OnSetup(changedStateLink, changedFieldInfo);
		}

		public void ShowBranch(bool show)
		{
			if (show)
			{
				if (_StateLinkBranchElement == null)
				{
					_StateLinkBranchElement = new StateLinkBranchElement(this);
				}

				if (_StateLinkBranchElement.parent == null)
				{
					_GraphEditor.graphView.branchUnderlayLayer.Add(_StateLinkBranchElement);
				}

				var minimapView = _GraphEditor.minimapView;

				if (_MinimapStateLinkBranchElement == null)
				{
					_MinimapStateLinkBranchElement = new BezierElement(minimapView.contentContainer)
					{
						shadow = false,
						edgeWidth = 4f,
						arrowWidth = 5f,
						pickingMode = UnityEngine.UIElements.PickingMode.Ignore,
					};
					_MinimapStateLinkBranchElement.AddManipulator(new MinimapTransformManipulator(minimapView, () =>
					{
						UpdateMinimapBezier();
					}));
				}

				if (_MinimapStateLinkBranchElement.parent == null)
				{
					minimapView.branchLayer.Add(_MinimapStateLinkBranchElement);

					UpdateMinimapLineColor();
					UpdateMinimapLineArrow();
					UpdateMinimapBezier();
				}
			}
			else
			{
				if (_StateLinkBranchElement != null && _StateLinkBranchElement.parent != null)
				{
					_StateLinkBranchElement.RemoveFromHierarchy();
				}

				if (_MinimapStateLinkBranchElement != null && _MinimapStateLinkBranchElement.parent != null)
				{
					_MinimapStateLinkBranchElement.RemoveFromHierarchy();
				}
			}
		}

		void UpdateMinimapLineColor()
		{
			if (_MinimapStateLinkBranchElement == null)
			{
				return;
			}

			Color lineColor = stateLink.lineColor;

			_MinimapStateLinkBranchElement.lineColor = lineColor;
		}

		void UpdateMinimapLineArrow()
		{
			if (_MinimapStateLinkBranchElement == null)
			{
				return;
			}

			StateLink stateLink = this.stateLink;
			ArborFSMInternal stateMachine = _GraphEditor.stateMachine;
			Node targetNode = stateMachine.GetNodeFromID(stateLink.stateID);
			if (targetNode == null)
			{
				return;
			}

			bool changed = true;
			bool arrow = targetNode is State;
			if (_MinimapStateLinkBranchElement.arrow != arrow)
			{
				_MinimapStateLinkBranchElement.arrow = arrow;
				changed = true;
			}

			if (changed)
			{
				_MinimapStateLinkBranchElement.arrow = arrow;
			}
		}

		void UpdateMinimapBezier()
		{
			if (_MinimapStateLinkBranchElement == null)
			{
				return;
			}

			StateLink stateLink = this.stateLink;
			ArborFSMInternal stateMachine = _GraphEditor.stateMachine;
			Node targetNode = stateMachine.GetNodeFromID(stateLink.stateID);
			if (targetNode == null)
			{
				return;
			}

			Bezier2D bezier = _GraphEditor.minimapView.GraphToMinimap(this.bezier);

			_MinimapStateLinkBranchElement.startPosition = bezier.startPosition;
			_MinimapStateLinkBranchElement.startControl = bezier.startControl;
			_MinimapStateLinkBranchElement.endPosition = bezier.endPosition;
			_MinimapStateLinkBranchElement.endControl = bezier.endControl;

			_MinimapStateLinkBranchElement.UpdateLayout();
		}

		internal void UpdateBezier()
		{
			if (!_IsLayouted)
			{
				return;
			}

			StateLink stateLink = this.stateLink;
			StateMachineGraphEditor graphEditor = _GraphEditor;

			NodeEditor targetNodeEditor = graphEditor != null? graphEditor.GetNodeEditorFromID(stateLink.stateID) : null;
			if (targetNodeEditor == null)
			{
				return;
			}

			NodeEditor nodeEditor = this.nodeEditor;
			Rect nodePositon = nodeEditor.rect;

			Rect rect = this.ChangeCoordinatesTo(nodeEditor.nodeElement, contentRect);
			var bezier = OnUpdateBezier(rect, targetNodeEditor);

			Vector2 statePosition = new Vector2(nodePositon.x, nodePositon.y);

			if (nodeEditor != null)
			{
				bezier.startPosition = nodeEditor.NodeToGraphPoint(bezier.startPosition);
				bezier.startControl = nodeEditor.NodeToGraphPoint(bezier.startControl);
			}
			else
			{
				bezier.startPosition += statePosition;
				bezier.startControl += statePosition;
			}
			bezier.endPosition += statePosition;
			bezier.endControl += statePosition;

			if (this.bezier != bezier)
			{
				this.bezier = bezier;

				if (_StateLinkBranchElement != null)
				{
					_StateLinkBranchElement.UpdateBezier();
				}

				UpdateMinimapBezier();

				if (_GraphEditor != null)
				{
					_GraphEditor.Repaint();
				}
			}
		}

		private void OnConnectionChanged()
		{
			StateLink stateLink = this.stateLink;

			var graphEditor = this.nodeEditor.graphEditor;
			Node targetNode = graphEditor != null? graphEditor.nodeGraph.GetNodeFromID(stateLink.stateID) : null;
			bool on = targetNode != null;
			if (_On != on)
			{
				_On = on;
				if (this.panel != null)
				{
					ShowBranch(_On);
				}

				OnConnectionChanged(_On);
			}

			UpdateMinimapLineArrow();
		}

		void OnSettingsChangedInternal()
		{
			OnSettingsChanged();
			UpdateMinimapLineColor();
			onSettingsChanged?.Invoke();
		}

		internal void SetLinkState(int stateID, bool updateBezier = true)
		{
			stateLink.stateID = stateID;
			if (updateBezier)
			{
				UpdateBezier();
			}
		}

		protected abstract void OnSetup(bool changedStateLink, bool changedFieldInfo);
		protected abstract Bezier2D OnUpdateBezier(Rect rect, NodeEditor targetNodeEditor);
		protected abstract void OnSettingsChanged();
		protected abstract void OnConnectionChanged(bool on);
		protected abstract void OnUndoRedoPerformed();

		private sealed class Pivot
		{
			public Vector2 position;
			public Vector2 pivotPosition;
			public Vector2 normal;

			public Pivot(Vector2 position, Vector2 normal)
			{
				this.position = position;
				this.pivotPosition = position;
				this.normal = normal;
			}

			public Pivot(Vector2 position, Vector2 pivotPosition, Vector2 normal)
			{
				this.position = position;
				this.pivotPosition = pivotPosition;
				this.normal = normal;
			}
		}

		public static Bezier2D GetTargetBezier(NodeEditor currentNodeEditor, NodeEditor targetNodeEditor, Vector2 leftPos, Vector2 rightPos, ref bool right)
		{
			Vector2 startPos = Vector2.zero;
			Vector2 startTangent = Vector2.zero;
			Vector2 endPos = Vector2.zero;
			Vector2 endTangent = Vector2.zero;

			right = true;

			Rect currentRect = currentNodeEditor.rect;

			if (targetNodeEditor != null)
			{
				Rect targetRect = targetNodeEditor.rect;
				targetRect.x -= currentRect.x;
				targetRect.y -= currentRect.y;

				Pivot findPivot = null;

				List<Pivot> pivots = new List<Pivot>();

				StateLinkRerouteNode targetRerouteNode = targetNodeEditor.node as StateLinkRerouteNode;
				if (targetRerouteNode != null)
				{
					Vector2 direction = targetRerouteNode.direction;
					Pivot leftPivot = new Pivot(targetRect.center - direction * 6f, targetRect.center, -direction);
					pivots.Add(leftPivot);
					pivots.Add(leftPivot);
				}
				else
				{
					pivots.Add(new Pivot(new Vector2(targetRect.xMin, targetRect.yMin + EditorGUITools.kStateBezierTargetOffsetY), -Vector2.right));
					pivots.Add(new Pivot(new Vector2(targetRect.xMax, targetRect.yMin + EditorGUITools.kStateBezierTargetOffsetY), Vector2.right));
				}

				if (targetRect.x == 0.0f)
				{
					if (targetRect.y > 0.0f)
					{
						findPivot = pivots[0];
						right = false;
					}
					else
					{
						findPivot = pivots[1];
						right = true;
					}
				}
				else
				{
					float findDistance = 0.0f;

					int pivotCount = pivots.Count;
					for (int pivotIndex = 0; pivotIndex < pivotCount; pivotIndex++)
					{
						Pivot pivot = pivots[pivotIndex];

						Vector2 vl = leftPos - pivot.pivotPosition;
						Vector2 vr = rightPos - pivot.pivotPosition;

						float leftDistance = vl.magnitude;
						float rightDistance = vr.magnitude;

						float distance = 0.0f;
						bool checkRight = false;

						if (leftDistance > rightDistance)
						{
							distance = rightDistance;
							checkRight = true;
						}
						else
						{
							distance = leftDistance;
							checkRight = false;
						}

						if (findPivot == null || distance < findDistance)
						{
							findPivot = pivot;
							findDistance = distance;
							right = checkRight;
						}
					}
				}

				StateLinkRerouteNode currentRerouteNode = currentNodeEditor.node as StateLinkRerouteNode;
				if (currentRerouteNode != null)
				{
					startPos = rightPos;
					startTangent = startPos + currentRerouteNode.direction * EditorGUITools.kBezierTangent;
				}
				else if (right)
				{
					startPos = rightPos;
					startTangent = rightPos + EditorGUITools.kBezierTangentOffset;
				}
				else
				{
					startPos = leftPos;
					startTangent = leftPos - EditorGUITools.kBezierTangentOffset;
				}

				endPos = findPivot.position;
				endTangent = endPos + findPivot.normal * EditorGUITools.kBezierTangent;
			}

			return new Bezier2D(startPos, startTangent, endPos, endTangent);
		}
	}
}