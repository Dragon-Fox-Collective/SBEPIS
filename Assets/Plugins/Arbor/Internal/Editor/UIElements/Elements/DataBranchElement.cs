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

	internal sealed class DataBranchElement : VisualElement
	{
		public DataBranch branch;

		private NodeGraphEditor _GraphEditor;
		private DottedBezierElement _BezierElement;
		private DataViewElement _DataViewElement;

		private bool _IsHover;

		public DataBranchElement(NodeGraphEditor graphEditor)
		{
			_GraphEditor = graphEditor;

			_BezierElement = new DottedBezierElement(_GraphEditor.graphView.contentContainer)
			{
				shadow = true,
				edgeWidth = 16.0f,
				space = 10.0f,
				tex = EditorContents.dataConnectionTexture,
			};
			Add(_BezierElement);

			RegisterCallback<MouseOverEvent>(OnMouseOver);
			RegisterCallback<MouseOutEvent>(OnMouseOut);

			this.AddManipulator(new ContextClickManipulator(OnContextClick));
		}

		void OnContextClick(ContextClickEvent evt)
		{
			bool editable = _GraphEditor.editable;

			GenericMenu menu = new GenericMenu();

			NodeGraph nodeGraph = _GraphEditor.nodeGraph;

			if (editable)
			{
				menu.AddItem(EditorContents.showDataValue, branch.showDataValue, () =>
				{
					Undo.RecordObject(nodeGraph, "Change ShowDataValue");
					branch.showDataValue = !branch.showDataValue;
					EditorUtility.SetDirty(nodeGraph);
				});
			}
			else
			{
				menu.AddDisabledItem(EditorContents.showDataValue);
			}

			menu.AddSeparator("");

			if (editable)
			{
				Vector2 mousePosition = _GraphEditor.graphView.ElementToGraph(evt.currentTarget as VisualElement, evt.localMousePosition);

				menu.AddItem(EditorContents.reroute, false, () =>
				{
					int inNodeID = branch.inNodeID;
					Object inBehaviour = branch.inBehaviour;
					DataSlot inputSlot = branch.inputSlot;
					int outNodeID = branch.outNodeID;
					Object outBehaviour = branch.outBehaviour;
					DataSlot outputSlot = branch.outputSlot;
					Bezier2D lineBezier = branch.lineBezier;
					System.Type outputType = branch.outputType;

					Undo.IncrementCurrentGroup();
					int undoGroup = Undo.GetCurrentGroup();

					_GraphEditor.DeleteDataBranch(branch);

					float t = lineBezier.GetClosestParam(mousePosition);
					Vector2 pinPosition = lineBezier.GetPoint(t);
					Vector2 pinDirection = lineBezier.GetTangent(t);

					DataBranchRerouteNode newRerouteNode = _GraphEditor.CreateDataBranchRerouteNode(EditorGUITools.SnapToGrid(mousePosition - new Vector2(16f, 16f)), outputType, pinDirection);

					Undo.RecordObject(nodeGraph, "Reroute");

					RerouteSlot rerouteSlot = newRerouteNode.link;

					DataBranch outBranch = _GraphEditor.ConnectDataBranch(newRerouteNode.nodeID, null, rerouteSlot, outNodeID, outBehaviour, outputSlot);
					if (outBranch != null)
					{
						outBranch.enabled = true;
						outBranch.lineBezier = new Bezier2D(lineBezier.startPosition, lineBezier.startControl,
							pinPosition, pinPosition - pinDirection * EditorGUITools.kBezierTangent);
					}

					DataBranch inBranch = _GraphEditor.ConnectDataBranch(inNodeID, inBehaviour, inputSlot, newRerouteNode.nodeID, null, rerouteSlot);
					if (inBranch != null)
					{
						inBranch.enabled = true;
						inBranch.lineBezier = new Bezier2D(pinPosition, pinPosition + pinDirection * EditorGUITools.kBezierTangent,
							lineBezier.endPosition, lineBezier.endControl);
					}

					Undo.CollapseUndoOperations(undoGroup);

					EditorUtility.SetDirty(nodeGraph);

					_GraphEditor.VisibleNode(inNodeID);
					_GraphEditor.VisibleNode(outNodeID);
				});

				menu.AddItem(EditorContents.disconnect, false, () =>
				{
					_GraphEditor.DeleteDataBranch(branch);
				});
			}
			else
			{
				menu.AddDisabledItem(EditorContents.reroute);
				menu.AddDisabledItem(EditorContents.disconnect);
			}

			menu.ShowAsContext();

			evt.StopPropagation();
		}

		void OnMouseOver(MouseOverEvent evt)
		{
			if (!_IsHover)
			{
				_IsHover = true;
				_GraphEditor.graphView.dataBranchOverlayLayer.Add(this);
			}
		}

		void OnMouseOut(MouseOutEvent evt)
		{
			if (_IsHover)
			{
				_IsHover = false;
				_GraphEditor.graphView.dataBranchUnderlayLayer.Add(this);
			}
		}

		public void Update()
		{
			if (branch == null)
			{
				if (_DataViewElement != null && _DataViewElement.parent != null)
				{
					_DataViewElement.RemoveFromHierarchy();
				}
				return;
			}

			DataSlot inputSlot = branch.inputSlot;
			DataSlot outputSlot = branch.outputSlot;

			if (inputSlot == null || outputSlot == null)
			{
				return;
			}

			_GraphEditor.UpdateDataBranchBezier(branch);

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

			Color shadowColor = new Color(0, 0, 0, alpha);

			_BezierElement.startPosition = branch.lineBezier.startPosition;
			_BezierElement.startControl = branch.lineBezier.startControl;
			_BezierElement.startColor = outputSlotColor;
			_BezierElement.endPosition = branch.lineBezier.endPosition;
			_BezierElement.endControl = branch.lineBezier.endControl;
			_BezierElement.endColor = inputSlotColor;
			_BezierElement.shadowColor = shadowColor;

			_BezierElement.UpdateLayout();

			bool isVisibleDataView = false;

			if (Application.isPlaying && branch.isUsed)
			{
				isVisibleDataView = ArborSettings.showDataValue || branch.showDataValue || _IsHover;
			}

			if (isVisibleDataView)
			{
				if (_DataViewElement == null)
				{
					_DataViewElement = new DataViewElement(_GraphEditor.graphView);
				}

				if (_DataViewElement.parent == null)
				{
					Add(_DataViewElement);
				}

				_DataViewElement.branch = branch;

				_DataViewElement.UpdatePosition();
				_DataViewElement.MarkDirtyRepaint();
			}
			else if(_DataViewElement != null && _DataViewElement.parent != null)
			{
				_DataViewElement.RemoveFromHierarchy();
			}
		}

		class DataViewElement : VisualElement
		{
			private GraphView _GraphView;

			public DataBranch branch;

			public DataViewElement(GraphView graphView)
			{
				_GraphView = graphView;
				Add(new NodeContentIMGUIContainer(OnGUI));

				style.position = Position.Absolute;

				RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
			}

			void OnGeometryChanged(GeometryChangedEvent evt)
			{
				UpdatePosition();
			}

			public void UpdatePosition()
			{
				if (branch == null)
				{
					return;
				}

				Vector2 size = new Vector2(resolvedStyle.width, resolvedStyle.height);

				Bezier2D bezier = branch.lineBezier;
				Vector2 pos = _GraphView.GraphToElement(hierarchy.parent, bezier.GetPoint(0.5f));

				transform.position = pos - size * 0.5f;
			}

			void OnGUI()
			{
				if (branch == null)
				{
					return;
				}

				var valueSlot = branch.valueSlot;
				if (valueSlot == null)
				{
					return;
				}

				GUIStyle style = BuiltInStyles.countBadgeLarge;
				System.Type valueType = valueSlot.GetValueType();

				Color color = Color.clear;
				if (valueType == typeof(Color) && valueSlot.TryGetValue<Color>(out color))
				{
					Vector2 size = style.CalcScreenSize(new Vector2(16, 16));

					Rect rect = GUILayoutUtility.GetRect(size.x, size.y);

					if (EditorGUI.DropdownButton(rect, GUIContent.none, FocusType.Passive, style))
					{
						// Output current value to console
						Debug.Log(NodeGraphEditor.GetDataValueLogString(color.ToString(), null));
					}

					if (Event.current.type == EventType.Repaint)
					{
						Rect contentRect = style.padding.Remove(rect);
						EditorGUIUtility.DrawColorSwatch(contentRect, color);
						BuiltInStyles.colorPickerBox.Draw(contentRect, GUIContent.none, false, false, false, false);
					}
				}
				else
				{
					string valueString = NodeGraphEditor.ToDataValueString(valueSlot, valueType);

					GUIContent content = new GUIContent(valueString);
					Vector2 size = style.CalcSize(content);

					Rect rect = GUILayoutUtility.GetRect(size.x, size.y);

					if (EditorGUI.DropdownButton(rect, content, FocusType.Passive, style))
					{
						if (typeof(Object).IsAssignableFrom(valueType))
						{
							Object objValue = valueSlot.GetValue() as Object;
							EditorGUIUtility.PingObject(objValue);
						}
						else
						{
							// Output current value to console
							string detailsString = NodeGraphEditor.ToDataValueDetailsString(valueSlot, valueType);
							Debug.Log(NodeGraphEditor.GetDataValueLogString(valueString, detailsString));
						}
					}
				}
			}
		}
	}
}