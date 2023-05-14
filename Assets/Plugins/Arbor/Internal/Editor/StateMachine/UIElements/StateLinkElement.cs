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

	internal sealed class StateLinkElement : StateLinkElementBase
	{
		public readonly SerializedPropertyKey propertyKey;

		private readonly string _DefaultLabel;
		
		private Image _Icon;
		private Label _Label;

		private VisualElement _SettingsButtonElement;
		private ConnectManipulator _ConnectManipulator = null;

		private StateLinkPinElement _Pin;

		private PolyLineElement _ConnectLines;
		
		protected override void OnConnectionChanged(bool on)
		{
			EnableInClassList("node-link-slot-on", on);
			_ConnectLines.visible = on;
			if (!on)
			{
				_Pin.isPinRight = true;
			}

			UpdateLineColor();
		}

		protected override void OnSetup(bool changedStateLink, bool changedFieldInfo)
		{
			if (changedStateLink || changedFieldInfo)
			{
				UpdateIcon();
			}

			if (changedStateLink)
			{
				UpdateLabel();
				UpdateLineColor();
			}
		}

		public StateLinkElement(NodeEditor nodeEditor, StateBehaviour behaviour, SerializedPropertyKey propertyKey, string defaultLabel) : base(nodeEditor, behaviour)
		{
			this.propertyKey = propertyKey;
			_DefaultLabel = defaultLabel;

			AddToClassList("node-link-slot");
			AddToClassList("state-link-slot");

			VisualElement content = new VisualElement();
			content.AddToClassList("content");
			Add(content);

			_Icon = new Image();
			_Icon.AddToClassList("content-icon");
			content.Add(_Icon);

			_Label = new Label();
			_Label.AddToClassList("content-label");
			content.Add(_Label);

			_SettingsButtonElement = new MouseDownButton(() =>
			{
				OpenSettingsWindow();
			});
			_SettingsButtonElement.RemoveFromClassList("unity-button");
			_SettingsButtonElement.AddToClassList("settings-button");
			VisualElement popupButtonImage = new Image()
			{
				image = Icons.stateLinkPopupIcon,
			};
			popupButtonImage.AddManipulator(new LocalizationManipulator("Settings", LocalizationManipulator.TargetText.Tooltip));
			_SettingsButtonElement.Add(popupButtonImage);
			Add(_SettingsButtonElement);

			_ConnectLines = new PolyLineElement()
			{
				edgeWidth = 8f,
			};
			_ConnectLines.visible = false;
			Add(_ConnectLines);

			_Pin = new StateLinkPinElement();
			_Pin.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedPin);
			Add(_Pin);

			RegisterCallback<CustomStyleResolvedEvent>(OnCustomStyleResolved);

			_ConnectManipulator = new ConnectManipulator();
			_ConnectManipulator.onChangedActive += isActive =>
			{
				EnableInClassList("node-link-slot-active", isActive);
				UpdateLineColor();
			};
			this.AddManipulator(_ConnectManipulator);

			this.AddManipulator(new ContextClickManipulator(OnContextClick));
		}

		void OnCustomStyleResolved(CustomStyleResolvedEvent e)
		{
			UpdateLineColor();
		}

		void OnGeometryChangedPin(GeometryChangedEvent e)
		{
			NodeEditor nodeEditor = this.nodeEditor;

			Vector2 pinPos = e.newRect.center;
			Vector2 endPos = _Pin.isPinRight ? new Vector2(nodeEditor.rect.width, 0f) : new Vector2(0f, 0f);
			endPos = nodeEditor.nodeElement.ChangeCoordinatesTo(this, endPos);
			endPos.y = pinPos.y;

			_ConnectLines.SetPoints(pinPos, endPos);
		}

		protected override void OnUndoRedoPerformed()
		{
			UpdateLabel();
			UpdateIcon();
			UpdateLineColor();
		}

		protected override Bezier2D OnUpdateBezier(Rect rect, NodeEditor targetNodeEditor)
		{
			NodeEditor nodeEditor = this.nodeEditor;

			Vector2 leftPos = new Vector2(0f, rect.center.y);
			Vector2 rightPos = new Vector2(nodeEditor.rect.width, rect.center.y);

			bool isPinRight = true;
			Bezier2D bezier = GetTargetBezier(nodeEditor, targetNodeEditor, leftPos, rightPos, ref isPinRight);

			_Pin.isPinRight = isPinRight;

			return bezier;
		}

		void OpenSettingsWindow()
		{
			OpenSettingsWindow(_SettingsButtonElement.worldBound, false);
		}

		void OnContextClick(ContextClickEvent e)
		{
			OpenSettingsWindow();
			e.StopPropagation();
		}

		protected override void OnSettingsChanged()
		{
			UpdateLabel();
			UpdateIcon();
			UpdateLineColor();
		}

		static readonly CustomStyleProperty<Color> s_ColorsBackgroundProperty = new CustomStyleProperty<Color>("--local-colors-node_link_slot-background");

		void UpdateLabel()
		{
			string label = stateLink.name;
			if (string.IsNullOrEmpty(label))
			{
				label = _DefaultLabel;
			}
			_Label.text = label;
		}

		void UpdateIcon()
		{
			TransitionTiming transitionTiming = GetTransitionTiming(stateLink, fieldInfo);
			_Icon.image = GetTransitionTimingIcon(transitionTiming);
		}

		void UpdateLineColor()
		{
			Color lineColor = stateLink.lineColor;

			_ConnectLines.lineColor = lineColor;

			Color slotColor = lineColor;
			slotColor.a = 1f;

			_Pin.style.unityBackgroundImageTintColor = slotColor;

			if (_ConnectManipulator.isActive)
			{
				style.unityBackgroundImageTintColor = StyleKeyword.Null;
			}
			else
			{
				if (!on)
				{
					if (customStyle.TryGetValue(s_ColorsBackgroundProperty, out var value))
					{
						slotColor = slotColor * value;
					}
				}
				style.unityBackgroundImageTintColor = slotColor;
			}
		}

		static TransitionTiming GetTransitionTiming(StateLink stateLink, System.Reflection.FieldInfo stateLinkFieldInfo)
		{
			FixedTransitionTiming fixedTransitionTiming = AttributeHelper.GetAttribute<FixedTransitionTiming>(stateLinkFieldInfo);
			FixedImmediateTransition fixedImmediateTransition = AttributeHelper.GetAttribute<FixedImmediateTransition>(stateLinkFieldInfo);

			TransitionTiming transitionTiming = TransitionTiming.LateUpdateDontOverwrite;

			if (fixedTransitionTiming != null)
			{
				transitionTiming = fixedTransitionTiming.transitionTiming;
			}
			else if (fixedImmediateTransition != null)
			{
				transitionTiming = fixedImmediateTransition.immediate ? TransitionTiming.Immediate : TransitionTiming.LateUpdateOverwrite;
			}
			else
			{
				transitionTiming = stateLink.transitionTiming;
			}

			return transitionTiming;
		}

		static Texture GetTransitionTimingIcon(TransitionTiming transitionTiming)
		{
			switch (transitionTiming)
			{
				case TransitionTiming.LateUpdateOverwrite:
					return Icons.transitionTimingLateUpdateOverwrite;
				case TransitionTiming.Immediate:
					return Icons.transitionTimingImmediate;
				case TransitionTiming.LateUpdateDontOverwrite:
					return Icons.transitionTimingLateUpdateDontOverwrite;
				case TransitionTiming.NextUpdateOverwrite:
					return Icons.transitionTimingNextUpdateOverwrite;
				case TransitionTiming.NextUpdateDontOverwrite:
					return Icons.transitionTimingNextUpdateDontOverwrite;
			}

			return null;
		}

		sealed class ConnectManipulator : DragOnGraphManipulator
		{
			private NodeEditor _DragTargetNodeEditor = null;

			private PolyLineElement _ConnectLines;
			private StateLinkPinElement _ActivePin;
			private bool _OldOn;

			public ConnectManipulator()
			{
				activators.Add(new ManipulatorActivationFilter() { button = MouseButton.LeftMouse });

				_ConnectLines = new PolyLineElement()
				{
					edgeWidth = 8f,
				};

				_ActivePin = new StateLinkPinElement();
				_ActivePin.AddToClassList("pin-active");

				_ActivePin.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedActivePin);
			}

			private void OnGeometryChangedActivePin(GeometryChangedEvent e)
			{
				StateLinkElement linkElement = target as StateLinkElement;
				if (linkElement == null)
				{
					return;
				}

				if (isActive)
				{
					NodeEditor nodeEditor = linkElement.nodeEditor;

					Vector2 pinPos = e.newRect.center;
					Vector2 endPos = _ActivePin.isPinRight? new Vector2(nodeEditor.rect.width, 0f) : new Vector2(0f, 0f);
					endPos = nodeEditor.nodeElement.ChangeCoordinatesTo(linkElement, endPos);
					endPos.y = pinPos.y;

					_ConnectLines.SetPoints(pinPos, endPos);
				}
			}

			protected override void RegisterCallbacksOnTarget()
			{
				base.RegisterCallbacksOnTarget();

				target.RegisterCallback<KeyDownEvent>(OnKeyDown);
			}

			protected override void UnregisterCallbacksFromTarget()
			{
				base.UnregisterCallbacksFromTarget();

				target.UnregisterCallback<KeyDownEvent>(OnKeyDown);
			}

			protected override void OnChangeGraphView(IChangeGraphViewEvent e)
			{
				EventBase evtBase = e as EventBase;
				GraphView graphView = evtBase.target as GraphView;

				Vector2 mousePosition = graphView.GetMousePosition(target);
				UpdateMousePosition(mousePosition);
			}

			static readonly CustomStyleProperty<Color> s_ColorsPinBackgroundPressedProperty = new CustomStyleProperty<Color>("--colors-pin-background-pressed");

			protected override void OnMouseDown(MouseDownEvent e)
			{
				StateLinkElement linkElement = target as StateLinkElement;
				if (linkElement == null)
				{
					return;
				}

				_DragTargetNodeEditor = null;
				var graphEditor = linkElement._GraphEditor;
				if (graphEditor != null)
				{
					StateBehaviour behaviour = linkElement._StateBehaviour;

					ArborFSMInternal stateMachine = behaviour.stateMachine;
					State state = stateMachine.GetStateFromID(behaviour.nodeID);
					NodeEditor nodeEditor = linkElement.nodeEditor;
					Rect nodePosition = nodeEditor.rect;

					Vector2 mousePosition = (e.target as VisualElement).ChangeCoordinatesTo(nodeEditor.nodeElement, e.localMousePosition);

					Rect rect = linkElement.parent.ChangeCoordinatesTo(nodeEditor.nodeElement, linkElement.layout);

					Vector2 leftPos = new Vector2(0f, rect.center.y);
					Vector2 rightPos = new Vector2(nodePosition.width, rect.center.y);

					bool isPinRight = false;
					Bezier2D bezier = GetTargetBezier(mousePosition, leftPos, rightPos, ref isPinRight);

					Vector2 bezierStartPosition = nodeEditor.nodeElement.ChangeCoordinatesTo(linkElement, bezier.startPosition);

					Vector2 statePosition = new Vector2(nodePosition.x, nodePosition.y);

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

					graphEditor.BeginDragStateBranch(state.nodeID);
					graphEditor.DragStateBranchBezie(bezier);

					linkElement.Add(_ConnectLines);
					if (linkElement.customStyle.TryGetValue(s_ColorsPinBackgroundPressedProperty, out var value))
					{
						_ConnectLines.lineColor = value;
					}

					linkElement.Add(_ActivePin);
					_ActivePin.isPinRight = isPinRight;

					_OldOn = linkElement.on;
					if (!_OldOn)
					{
						linkElement._Pin.visible = false;
					}
				}

				e.StopPropagation();
			}

			void UpdateMousePosition(Vector2 localMousePosition)
			{
				StateLinkElement linkElement = target as StateLinkElement;
				if (linkElement == null)
				{
					return;
				}

				var nodeEditor = linkElement.nodeEditor;
				Rect nodePosition = nodeEditor.rect;
				var graphEditor = linkElement._GraphEditor;

				StateBehaviour behaviour = linkElement._StateBehaviour;

				ArborFSMInternal stateMachine = behaviour.stateMachine;
				State state = stateMachine.GetStateFromID(behaviour.nodeID);

				Vector2 mousePosition = linkElement.ChangeCoordinatesTo(nodeEditor.nodeElement, localMousePosition);
				Rect rect = linkElement.parent.ChangeCoordinatesTo(nodeEditor.nodeElement, linkElement.layout);

				NodeEditor hoverNodeEditor = !rect.Contains(mousePosition) ? graphEditor.GetTargetNodeEditorFromPosition(graphEditor.graphView.ElementToGraph(nodeEditor.nodeElement, mousePosition), state) : null;
				if (hoverNodeEditor != null)
				{
					if (graphEditor != null)
					{
						graphEditor.DragStateBranchHoverStateID(hoverNodeEditor.nodeID);
					}

					_DragTargetNodeEditor = hoverNodeEditor;
				}
				else
				{
					if (graphEditor != null)
					{
						graphEditor.DragStateBranchHoverStateID(0);
					}

					_DragTargetNodeEditor = null;
				}

				Vector2 leftPos = new Vector2(0f, rect.center.y);
				Vector2 rightPos = new Vector2(nodePosition.width, rect.center.y);

				Bezier2D bezier = null;
				bool isPinRight = false;
				if (_DragTargetNodeEditor != null)
				{
					bezier = StateLinkElementBase.GetTargetBezier(nodeEditor, _DragTargetNodeEditor, leftPos, rightPos, ref isPinRight);
				}
				else
				{
					bezier = GetTargetBezier(mousePosition, leftPos, rightPos, ref isPinRight);
				}

				Vector2 bezierStartPosition = nodeEditor.nodeElement.ChangeCoordinatesTo(linkElement, bezier.startPosition);

				Vector2 statePosition = new Vector2(nodePosition.x, nodePosition.y);

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

				if (graphEditor != null)
				{
					graphEditor.DragStateBranchBezie(bezier);
				}

				_ActivePin.isPinRight = isPinRight;
			}

			protected override void OnMouseMove(MouseMoveEvent e)
			{
				StateLinkElement linkElement = target as StateLinkElement;
				if (linkElement == null)
				{
					return;
				}

				DragAndDrop.PrepareStartDrag();

				UpdateMousePosition(e.localMousePosition);

				e.StopPropagation();
			}

			protected override void OnMouseUp(MouseUpEvent e)
			{
				StateLinkElement linkElement = target as StateLinkElement;
				if (linkElement == null)
				{
					return;
				}

				var stateLink = linkElement.stateLink;
				var graphEditor = linkElement._GraphEditor;

				var behaviour = linkElement._StateBehaviour;
				
				if (_DragTargetNodeEditor == null)
				{
					Vector2 localMousePosition = e.localMousePosition;
					VisualElement eventTarget = e.target as VisualElement;

					GenericMenu menu = new GenericMenu();

					Vector2 graphMousePosition = graphEditor.graphView.ElementToGraph(eventTarget, localMousePosition);
					Vector2 screenMousePosition = eventTarget.LocalToScreen(localMousePosition);

					menu.AddItem(EditorContents.createState, false, () =>
					{
						graphMousePosition -= new Vector2(8f, 12f);

						State newState = graphEditor.CreateState(graphMousePosition, false);

						Undo.RecordObject(behaviour, "Link State");

						linkElement.SetLinkState(newState.nodeID, false);

						EditorUtility.SetDirty(behaviour);
					});

					menu.AddItem(EditorContents.reroute, false, () =>
					{
						Undo.IncrementCurrentGroup();
						int undoGroup = Undo.GetCurrentGroup();

						graphMousePosition -= new Vector2(16f, 16f);

						Color lineColor = stateLink.lineColor;

						StateLinkRerouteNode newStateLinkNode = graphEditor.CreateStateLinkRerouteNode(graphMousePosition, lineColor);

						Undo.RecordObject(behaviour, "Link State");

						linkElement.SetLinkState(newStateLinkNode.nodeID, false);

						Undo.CollapseUndoOperations(undoGroup);

						EditorUtility.SetDirty(behaviour);
					});

					menu.AddSeparator("");

					menu.AddItem(EditorContents.nodeListSelection, false, () =>
					{
						StateLinkElement currentLinkElement = linkElement;
						StateBehaviour currentBehaviour = behaviour;

						StateLinkSelectorWindow.instance.Open(graphEditor, new Rect(screenMousePosition, Vector2.zero), currentLinkElement.stateLink.stateID,
							(selectedNodeEditor) =>
							{
								Undo.RecordObject(currentBehaviour, "Link State");

								currentLinkElement.SetLinkState(selectedNodeEditor.nodeID);

								EditorUtility.SetDirty(currentBehaviour);

								//graphEditor.BeginFrameSelected(selectedNodeEditor.node);
							}
						);
					});

					if (stateLink.stateID != 0)
					{
						menu.AddSeparator("");
						menu.AddItem(EditorContents.disconnect, false, () =>
						{
							Undo.RecordObject(behaviour, "Disconect StateLink");

							linkElement.SetLinkState(0);

							EditorUtility.SetDirty(behaviour);
						});
					}
					menu.ShowAsContext();

					return;
				}

				NodeEditor targetNodeEditor = graphEditor.GetNodeEditorFromID(stateLink.stateID);
				if (_DragTargetNodeEditor != targetNodeEditor)
				{
					Undo.RecordObject(behaviour, "Link State");

					linkElement.SetLinkState(_DragTargetNodeEditor.nodeID);

					EditorUtility.SetDirty(behaviour);
				}
			}

			protected override void OnEndDrag()
			{
				base.OnEndDrag();

				StateLinkElement linkElement = target as StateLinkElement;
				if (linkElement == null)
				{
					return;
				}

				var graphEditor = linkElement._GraphEditor;

				if (graphEditor != null)
				{
					graphEditor.EndDragStateBranch();
				}

				_ConnectLines.RemoveFromHierarchy();
				_ActivePin.RemoveFromHierarchy();

				if (!_OldOn)
				{
					linkElement._Pin.visible = true;
				}

				_DragTargetNodeEditor = null;
			}

			void OnKeyDown(KeyDownEvent e)
			{
				StateLinkElement linkElement = target as StateLinkElement;
				if (linkElement == null)
				{
					return;
				}

				if (!isActive || e.keyCode != KeyCode.Escape)
				{
					return;
				}

				EndDrag();
				e.StopPropagation();
			}

			public static Bezier2D GetTargetBezier(Vector2 targetPos, Vector2 leftPos, Vector2 rightPos, ref bool isRight)
			{
				bool right = (targetPos - leftPos).magnitude > (targetPos - rightPos).magnitude;

				Vector2 startPos;
				Vector2 startTangent;

				if (right)
				{
					isRight = true;
					startPos = rightPos;
					startTangent = rightPos + EditorGUITools.kBezierTangentOffset;
				}
				else
				{
					isRight = false;
					startPos = leftPos;
					startTangent = leftPos - EditorGUITools.kBezierTangentOffset;
				}

				return new Bezier2D(startPos, startTangent, targetPos, startTangent);
			}
		}
	}
}