//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	using Arbor;

	internal sealed class InputSlotGUI : DataSlotGUI
	{
		public override void RebuildConnectGUI()
		{
			DataBranch branch = null;

			DataSlot slot = this.slot;
			InputSlotBase inputSlot = slot as InputSlotBase;
			if (inputSlot != null)
			{
				branch = inputSlot.branch;

				bool clearBranchID = false;
				if (branch == null)
				{
					clearBranchID = inputSlot.branchID != 0;
				}
				else if (branch != null)
				{
					DataSlot branchInputSlot = branch.inputSlot;
					if (!object.ReferenceEquals(branchInputSlot, inputSlot))
					{
						//branch.ClearInputSlotField();
						//branchInputSlot = branch.inputSlot;
						//Debug.Log("inputSlot.branchID = 0");
						clearBranchID = true;
					}
				}

				if (clearBranchID)
				{
					inputSlot.ClearBranch();
					EditorUtility.SetDirty(targetObject);
					branch = null;
				}
			}

			if (branch != null)
			{
				if (!branch.isValidOutputSlot || !DataSlot.IsConnectable(slot, branch.outputSlot))
				{
					var nodeEditor = behaviourEditorGUI?.nodeEditor;
					var graphEditor = nodeEditor != null? nodeEditor.graphEditor : null;
					if (graphEditor != null && graphEditor.nodeGraph == inputSlot.nodeGraph)
					{
						graphEditor.DeleteDataBranch(branch);
					}
					else
					{
						inputSlot.nodeGraph.DeleteDataBranch(branch);
					}
				}
			}
		}

		protected override void OnGUI(Rect position, GUIContent label, bool isElement)
		{
			DataSlot slot = this.slot;
			InputSlotBase inputSlot = slot as InputSlotBase;

			int controlID = EditorGUIUtility.GetControlID(s_DataSlotHash, FocusType.Passive, position);
			DataBranch branch = inputSlot.branch;

			Event current = Event.current;

			var nodeEditor = behaviourEditorGUI?.nodeEditor;
			var node = nodeEditor != null? nodeEditor.node : null;
			var graphEditor = nodeEditor != null? nodeEditor.graphEditor : null;
			var nodeGraph = graphEditor != null? graphEditor.nodeGraph : null;
			var graphView = graphEditor != null? graphEditor.graphView : null;

			Vector2 pinPos = new Vector2(position.x, position.center.y);
			Vector2 targetPos = current.mousePosition;

			if (graphView != null)
			{
				pinPos = graphView.GUIToGraph(pinPos);
				pinPos.x = nodeEditor.position.x;
				targetPos = graphView.GUIToGraph(targetPos);
			}

			Vector2 pinControlPos = pinPos - EditorGUITools.kBezierTangentOffset;
			Vector2 targetControlPos = targetPos + EditorGUITools.kBezierTangentOffset;
			
			if (_HoverSlot != null)
			{
				DataBranchRerouteNode hoverRerouteNode = _HoverNodeEditor != null? _HoverNodeEditor.node as DataBranchRerouteNode : null;
				if (hoverRerouteNode != null)
				{
					targetPos = _HoverSlot.position.center;
					targetControlPos = targetPos + hoverRerouteNode.direction * EditorGUITools.kBezierTangent;
				}
				else
				{
					targetPos = new Vector2(_HoverNodeEditor.rect.xMax, _HoverSlot.position.center.y);
					targetControlPos = targetPos + EditorGUITools.kBezierTangentOffset;
				}
			}

			switch (current.GetTypeForControl(controlID))
			{
				case EventType.ContextClick:
					if (position.Contains(current.mousePosition))
					{
						GenericMenu menu = new GenericMenu();

						if (branch != null)
						{
							menu.AddItem(EditorContents.disconnect, false, () =>
							{
								graphEditor.DeleteDataBranch(branch);
								branch = null;
							});
						}
						else
						{
							menu.AddDisabledItem(EditorContents.disconnect);
						}

						menu.ShowAsContext();

						current.Use();
					}
					break;
				case EventType.MouseDown:
					if (position.Contains(current.mousePosition) && current.button == 0)
					{
						GUIUtility.hotControl = GUIUtility.keyboardControl = controlID;

						if (graphEditor != null)
						{
							BeginDragSlot(node, slot, targetObject);

							if (branch != null)
							{
								branch.enabled = false;
							}

							graphEditor.BeginDragDataBranch(node.nodeID);
							graphEditor.DragDataBranchBezier(targetPos, targetControlPos, pinPos, pinControlPos);
						}

						current.Use();
					}
					break;
				case EventType.MouseDrag:
					if (GUIUtility.hotControl == controlID && current.button == 0)
					{
						DragAndDrop.PrepareStartDrag();

						UpdateHoverSlot(graphEditor, graphEditor.GetNodeEditorFromID(node.nodeID), targetObject, inputSlot, graphView.GUIToGraph(current.mousePosition));

						current.Use();
					}
					break;
				case EventType.MouseUp:
					if (GUIUtility.hotControl == controlID)
					{
						if (current.button == 0)
						{
							GUIUtility.hotControl = 0;

							if (graphEditor != null)
							{
								EndDragSlot();

								graphEditor.EndDragDataBranch();

								if (_HoverSlot == null)
								{
									GenericMenu menu = new GenericMenu();

									Vector2 mousePosition = graphView.GUIToGraph(current.mousePosition);

									DataBranch currentBranch = branch;
									Bezier2D lineBezier = new Bezier2D(targetPos, targetControlPos, pinPos, pinControlPos);

									if (inputSlot.connectableType != null || inputSlot.GetConstraint() == null)
									{
										menu.AddItem(EditorContents.reroute, false, () =>
										{
											if (currentBranch != null)
											{
												graphEditor.DeleteDataBranch(currentBranch);
												currentBranch = null;
											}

											Undo.IncrementCurrentGroup();
											int undoGroup = Undo.GetCurrentGroup();

											mousePosition -= new Vector2(16f, 16f);

											DataBranchRerouteNode newRerouteNode = graphEditor.CreateDataBranchRerouteNode(EditorGUITools.SnapToGrid(mousePosition), inputSlot.connectableType);

											Undo.RecordObject(nodeGraph, "Reroute");

											RerouteSlot rerouteSlot = newRerouteNode.link;

											currentBranch = graphEditor.ConnectDataBranch(node.nodeID, targetObject, inputSlot, newRerouteNode.nodeID, null, rerouteSlot);
											if (currentBranch != null)
											{
												currentBranch.enabled = true;
												currentBranch.lineBezier = lineBezier;
											}

											Undo.CollapseUndoOperations(undoGroup);

											EditorUtility.SetDirty(nodeGraph);
										});
									}
									else
									{
										menu.AddDisabledItem(EditorContents.reroute);
									}

									if (currentBranch != null)
									{
										menu.AddSeparator("");
										menu.AddItem(EditorContents.disconnect, false, () =>
										{
											graphEditor.DeleteDataBranch(currentBranch);
										});
									}

									menu.ShowAsContext();
								}
								else if (_HoverSlot != null)
								{
									if (branch != null)
									{
										graphEditor.DeleteDataBranch(branch);
										branch = null;
									}

									branch = graphEditor.ConnectDataBranch(node.nodeID, targetObject, inputSlot, _HoverNodeEditor.nodeID, _HoverObj, _HoverSlot);
									if (branch != null)
									{
										branch.lineBezier = new Bezier2D(targetPos, targetControlPos, pinPos, pinControlPos);
									}
								}

								if (branch != null)
								{
									branch.enabled = true;
								}

								ClearHoverSlot();
							}
						}

						current.Use();
					}
					break;
				case EventType.KeyDown:
					if (GUIUtility.hotControl == controlID && current.keyCode == KeyCode.Escape)
					{
						GUIUtility.hotControl = 0;

						if (graphEditor != null)
						{
							EndDragSlot();

							graphEditor.EndDragDataBranch();

							if (branch != null)
							{
								branch.enabled = true;
							}

							ClearHoverSlot();
						}

						current.Use();
					}
					break;
				case EventType.Repaint:
					{
						if (GUIUtility.hotControl == controlID && current.button == 0)
						{
							graphEditor.DragDataBranchBezier(targetPos, targetControlPos, pinPos, pinControlPos);
						}
						else
						{
							if (!isElement)
							{
								if (branch != null)
								{
									branch.lineBezier.endPosition = pinPos;
									branch.lineBezier.endControl = pinControlPos;
								}
							}
						}

						if (!isElement)
						{
							bool on = branch != null;

							DrawSlot(position, label, controlID, on, true);
						}
					}
					break;
			}
		}

		public override void UpdateBezier(Rect position)
		{
			InputSlotBase inputSlot = slot as InputSlotBase;
			DataBranch branch = inputSlot.branch;
			if (branch == null)
			{
				return;
			}

			var nodeEditor = behaviourEditorGUI?.nodeEditor;
			var node = nodeEditor != null? nodeEditor.node : null;
			var graphEditor = nodeEditor != null? nodeEditor.graphEditor : null;
			var graphView = graphEditor != null? graphEditor.graphView : null;

			Vector2 pinPos = new Vector2(position.x, position.center.y);

			if (graphView != null)
			{
				pinPos = graphView.GUIToGraph(pinPos);
			}
			if (ArborSettings.dataSlotShowMode == DataSlotShowMode.Inside)
			{
				pinPos.x = nodeEditor.rect.x;
			}
			Vector2 pinControlPos = pinPos - EditorGUITools.kBezierTangentOffset;

			if (branch != null)
			{
				branch.lineBezier.endPosition = pinPos;
				branch.lineBezier.endControl = pinControlPos;
			}
		}
	}
}