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

	internal sealed class StateLinkBranchElement : VisualElement
	{
		private readonly StateLinkElementBase _LinkElement;
		private readonly BezierElement _BezierElement;

		private CountBadgeElement _TransitionCountElement;

		private bool _IsHover = false;

		public StateLinkBranchElement(StateLinkElementBase linkElement)
		{
			_LinkElement = linkElement;

			_BezierElement = new BezierElement(linkElement._GraphEditor.graphView.contentContainer)
			{
				shadow = true,
				shadowColor = NodeGraphEditor.bezierShadowColor
			};
			Add(_BezierElement);

			RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
			RegisterCallback<MouseOverEvent>(OnMouseOver);
			RegisterCallback<MouseOutEvent>(OnMouseOut);

			this.AddManipulator(new ContextClickManipulator(OnContextClick));
		}

		StateLink _StateLink;

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			var nodeEditor = _LinkElement.nodeEditor;
			var nodeElement = nodeEditor.nodeElement;
			nodeElement.RegisterCallback<UndoRedoPerformedEvent>(OnUndoRedoPerformed);

			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

			_LinkElement.onSettingsChanged -= OnSettingsChanged;
			_LinkElement.onSettingsChanged += OnSettingsChanged;

			SetStateLink(_LinkElement.stateLink);
			SetNodeGraph(_LinkElement._GraphEditor.nodeGraph);

			ShowTransitionCount(Application.isPlaying);

			UpdateLine();
			UpdateArrow();
			UpdateBezier();
		}

		void OnUndoRedoPerformed(UndoRedoPerformedEvent e)
		{
			UpdateLine();
			UpdateArrow();
		}

		void OnSettingsChanged()
		{
			UpdateLine();
		}

		void SetStateLink(StateLink stateLink)
		{
			if (_StateLink != stateLink)
			{
				if (_StateLink != null)
				{
					_StateLink.onConnectionChanged -= OnConnectionChanged;
					StateLinkCallback.UnregisterTransitionCountCallback(_StateLink, OnTransitionCountChanged);
				}

				_StateLink = stateLink;

				if (_StateLink != null)
				{
					_StateLink.onConnectionChanged += OnConnectionChanged;
					StateLinkCallback.RegisterTransitionCountCallback(_StateLink, OnTransitionCountChanged);
				}
			}
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			var nodeEditor = _LinkElement.nodeEditor;
			var nodeElement = nodeEditor.nodeElement;
			nodeElement.UnregisterCallback<UndoRedoPerformedEvent>(OnUndoRedoPerformed);

			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;

			_LinkElement.onSettingsChanged -= OnSettingsChanged;

			SetStateLink(null);
			SetNodeGraph(null);
		}

		NodeGraph _NodeGraph;

		void SetNodeGraph(NodeGraph nodeGraph)
		{
			if (!ReferenceEquals(_NodeGraph, nodeGraph))
			{
				if (_NodeGraph is object)
				{
					NodeGraphCallback.UnregisterStateChangedCallback(_NodeGraph, OnStateChanged);
					_NodeGraph.onPlayStateChanged -= OnPlayStateChanged;
				}

				_NodeGraph = nodeGraph;

				if (_NodeGraph is object)
				{
					NodeGraphCallback.RegisterStateChangedCallback(_NodeGraph, OnStateChanged);
					_NodeGraph.onPlayStateChanged += OnPlayStateChanged;
				}
			}
		}

		void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			SetStateLink(_LinkElement.stateLink);
			SetNodeGraph(_LinkElement._GraphEditor.nodeGraph);
			ShowTransitionCount(Application.isPlaying);
			
			UpdateLine();
		}

		void OnStateChanged(NodeGraph nodeGraph)
		{
			UpdateLine();
		}

		void OnPlayStateChanged(PlayState playState)
		{
			UpdateLine();
		}

		void OnConnectionChanged()
		{
			UpdateArrow();
		}

		void OnTransitionCountChanged()
		{
			UpdateTransitionCount();
		}

		void ShowTransitionCount(bool show)
		{
			if (show)
			{
				if (_TransitionCountElement == null)
				{
					_TransitionCountElement = new CountBadgeElement()
					{
						pickingMode = PickingMode.Ignore,
						style =
						{
							position = Position.Absolute,
						}
					};

					_TransitionCountElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedTransitionCount);
				}

				if (_TransitionCountElement.parent == null)
				{
					Add(_TransitionCountElement);
					UpdateTransitionCount();
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

		void OnGeometryChangedTransitionCount(GeometryChangedEvent e)
		{
			UpdateTransitionCountPosition();
		}

		void OnContextClick(ContextClickEvent evt)
		{
			StateLink stateLink = _LinkElement.stateLink;
			StateMachineGraphEditor graphEditor = _LinkElement._GraphEditor;
			ArborFSMInternal stateMachine = graphEditor.stateMachine;
			NodeEditor nodeEditor = _LinkElement.nodeEditor;
			Node node = nodeEditor.node;
			StateBehaviour stateBehaviour = _LinkElement._StateBehaviour;
			bool editable = graphEditor.editable;
			Bezier2D bezier = _LinkElement.bezier;
			Node targetNode = stateMachine.GetNodeFromID(stateLink.stateID);

			GenericMenu menu = new GenericMenu();

			State prevState = node as State;
			State nextState = stateMachine.GetState(stateLink);

			if (prevState != null)
			{
				menu.AddItem(GUIContentCaches.Get(Localization.GetWord("Go to Previous State") + " : " + prevState.name), false, () =>
				{
					graphEditor.BeginFrameSelected(prevState);
				});
			}
			else
			{
				StateLinkRerouteNode rerouteNode = node as StateLinkRerouteNode;
				if (rerouteNode != null)
				{
					List<StateEditor> parentStateEditors = graphEditor.GetParentStateEditors(rerouteNode);
					parentStateEditors.Sort((a, b) =>
					{
						return a.position.y.CompareTo(b.position.y);
					});
					HashSet<string> names = new HashSet<string>();
					for (int parentStateIndex = 0; parentStateIndex < parentStateEditors.Count; parentStateIndex++)
					{
						StateEditor stateEditor = parentStateEditors[parentStateIndex];
						State state = stateEditor.state;
						State s = state;

						string stateName = s.name;
						while (names.Contains(stateName))
						{
							// dummy code 001f(US)
							stateName += '\u001f';
						}
						names.Add(stateName);

						menu.AddItem(GUIContentCaches.Get(Localization.GetWord("Go to Previous State") + " : " + stateName), false, () =>
						{
							graphEditor.BeginFrameSelected(s);
						});
					}

					if (parentStateEditors.Count > 1)
					{
						menu.AddSeparator("");
					}
				}
			}

			if (nextState != null)
			{
				menu.AddItem(GUIContentCaches.Get(Localization.GetWord("Go to Next State") + " : " + nextState.name), false, () =>
				{
					graphEditor.BeginFrameSelected(nextState);
				});
			}

			menu.AddSeparator("");

			bool flag1 = false;

			if (prevState == null)
			{
				menu.AddItem(EditorContents.goToPreviousNode, false, () =>
				{
					graphEditor.BeginFrameSelected(node);
				});
				flag1 = true;
			}

			if (targetNode != null && targetNode != nextState)
			{
				menu.AddItem(EditorContents.goToNextNode, false, () =>
				{
					graphEditor.BeginFrameSelected(targetNode);
				});
				flag1 = true;
			}

			if (flag1)
			{
				menu.AddSeparator("");
			}

			if (editable)
			{
				Vector2 mousePosition = graphEditor.graphView.ElementToGraph(evt.currentTarget as VisualElement, evt.localMousePosition);

				menu.AddItem(EditorContents.reroute, false, () =>
				{
					int stateID = stateLink.stateID;

					Undo.IncrementCurrentGroup();
					int undoGroup = Undo.GetCurrentGroup();

					float t = bezier.GetClosestParam(mousePosition);
					Vector2 direction = bezier.GetTangent(t);
					StateLinkRerouteNode newStateLinkNode = graphEditor.CreateStateLinkRerouteNode(EditorGUITools.SnapToGrid(mousePosition - new Vector2(16f, 16f)), stateLink.lineColor, direction, stateID);

					if (stateBehaviour != null)
					{
						Undo.RecordObject(stateBehaviour, "Reroute");
					}
					else
					{
						// reroute
						Undo.RecordObject(stateMachine, "Reroute");
					}

					stateLink.stateID = newStateLinkNode.nodeID;

					Undo.CollapseUndoOperations(undoGroup);

					if (stateBehaviour != null)
					{
						EditorUtility.SetDirty(stateBehaviour);
					}
					else
					{
						// reroute
						EditorUtility.SetDirty(stateMachine);
					}

					graphEditor.VisibleNode(node);

				});

				menu.AddItem(EditorContents.disconnect, false, () =>
				{
					if (stateBehaviour != null)
					{
						Undo.RecordObject(stateBehaviour, "Disconnect StateLink");
					}
					else
					{
						Undo.RecordObject(stateMachine, "Disconnect StateLink");
					}

					stateLink.stateID = 0;

					if (stateBehaviour != null)
					{
						EditorUtility.SetDirty(stateBehaviour);
					}
					else
					{
						EditorUtility.SetDirty(stateMachine);
					}
				});
			}
			else
			{
				menu.AddDisabledItem(EditorContents.reroute);
				menu.AddDisabledItem(EditorContents.disconnect);
			}

			menu.AddSeparator("");

			if (editable)
			{
				VisualElement currentTarget = evt.currentTarget as VisualElement;
				Rect settingRect = new Rect();
				settingRect.position = currentTarget.LocalToScreen(evt.localMousePosition);
				
				menu.AddItem(EditorContents.settings, false, () =>
				{
					_LinkElement.OpenSettingsWindow(GUIUtility.ScreenToGUIRect(settingRect), false);
				});
			}
			else
			{
				menu.AddDisabledItem(EditorContents.settings);
			}

			menu.ShowAsContext();

			evt.StopPropagation();
		}

		void OnMouseOver(MouseOverEvent evt)
		{
			if (!_IsHover)
			{
				_IsHover = true;
				_LinkElement._GraphEditor.graphView.branchOverlayLayer.Add(this);
			}
		}

		void OnMouseOut(MouseOutEvent evt)
		{
			if (_IsHover)
			{
				_IsHover = false;
				_LinkElement._GraphEditor.graphView.branchUnderlayLayer.Add(this);
			}
		}

		static readonly CustomStyleProperty<Color> s_ColorsCountBadgeBackgroundProperty = new CustomStyleProperty<Color>("--local-colors-count_badge_background");

		void UpdateTransitionCountPosition()
		{
			if (Application.isPlaying && _TransitionCountElement != null)
			{
				StateMachineGraphEditor graphEditor = _LinkElement._GraphEditor;
				Bezier2D bezier = _LinkElement.bezier;

				Vector2 size = new Vector2(_TransitionCountElement.resolvedStyle.width, _TransitionCountElement.resolvedStyle.height);
				Vector2 pos = graphEditor.graphView.GraphToElement(this, bezier.GetPoint(0.5f));
				_TransitionCountElement.transform.position = pos - size * 0.5f;
			}
		}

		public void UpdateBezier()
		{
			Bezier2D bezier = _LinkElement.bezier;

			bool changed = false;
			if (_BezierElement.startPosition != bezier.startPosition)
			{
				_BezierElement.startPosition = bezier.startPosition;
				changed = true;
			}
			if (_BezierElement.startControl != bezier.startControl)
			{
				_BezierElement.startControl = bezier.startControl;
				changed = true;
			}
			if (_BezierElement.endPosition != bezier.endPosition)
			{
				_BezierElement.endPosition = bezier.endPosition;
				changed = true;
			}
			if (_BezierElement.endControl != bezier.endControl)
			{
				_BezierElement.endControl = bezier.endControl;
				changed = true;
			}

			if (changed)
			{
				_BezierElement.UpdateLayout();
			}

			UpdateTransitionCountPosition();
		}

		void UpdateArrow()
		{
			StateLink stateLink = _LinkElement.stateLink;
			StateMachineGraphEditor graphEditor = _LinkElement._GraphEditor;
			ArborFSMInternal stateMachine = graphEditor.stateMachine;

			Node targetNode = stateMachine.GetNodeFromID(stateLink.stateID);

			bool changed = false;

			bool arrow = targetNode is State;
			if (_BezierElement.arrow != arrow)
			{
				_BezierElement.arrow = arrow;
				changed = true;
			}

			if (changed)
			{
				_BezierElement.UpdateLayout();
			}
		}

		void UpdateTransitionCount()
		{
			if (Application.isPlaying && _TransitionCountElement != null)
			{
				_TransitionCountElement.count = _LinkElement.stateLink.transitionCount;
			}
		}

		void UpdateLine()
		{
			StateLink stateLink = _LinkElement.stateLink;

			StateMachineGraphEditor graphEditor = _LinkElement._GraphEditor;

			ArborFSMInternal stateMachine = graphEditor.stateMachine;

			Color lineColor = stateLink.lineColor;

			Color shadowColor = NodeGraphEditor.bezierShadowColor;
			float width = 5;
			if (Application.isPlaying)
			{
				int index = stateMachine.IndexOfStateLinkHistory(stateLink);
				if (index != -1)
				{
					float t = (float)index / 4.0f;

					shadowColor = Color.Lerp(new Color(0.0f, 0.5f, 0.5f, 1.0f), Color.black, t);
					if (stateMachine.playState == PlayState.InactivePausing && stateMachine.reservedStateLink == stateLink)
					{
						lineColor *= StateMachineGraphEditor.reservedColor;
					}
					else
					{
						lineColor *= Color.Lerp(Color.white, Color.gray, t);
					}
					width = Mathf.Lerp(15, 5, t);
				}
				else
				{
					if (stateMachine.playState == PlayState.InactivePausing && stateMachine.reservedStateLink == stateLink)
					{
						lineColor *= StateMachineGraphEditor.reservedColor;
					}
					else
					{
						lineColor *= Color.gray;
					}
				}
			}

			_BezierElement.lineColor = lineColor;
			_BezierElement.shadowColor = shadowColor;

			bool changed = false;
			if (_BezierElement.edgeWidth != width)
			{
				_BezierElement.edgeWidth = width;
				changed = true;
			}

			if (changed)
			{
				_BezierElement.UpdateLayout();
			}

			if (Application.isPlaying && _TransitionCountElement != null)
			{
				Color color = Color.white;

				int index = stateMachine.IndexOfStateLinkHistory(stateLink);
				if (index != -1)
				{
					float t = (float)index / 4.0f;

					color *= Color.Lerp(Color.white, Color.gray, t);
				}
				else
				{
					color *= Color.gray;
				}

				if(_TransitionCountElement.customStyle.TryGetValue(s_ColorsCountBadgeBackgroundProperty, out var styleColor))
				{
					color *= styleColor;
				}
				_TransitionCountElement.style.backgroundColor = color;
			}
		}
	}
}