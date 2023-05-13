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
	using ArborEditor.UIElements;

	[CustomNodeEditor(typeof(DataBranchRerouteNode))]
	internal sealed class DataBranchRerouteNodeEditor : NodeEditor
	{
		public DataBranchRerouteNode dataBranchRerouteNode
		{
			get
			{
				return node as DataBranchRerouteNode;
			}
		}

		protected override bool HasHeaderGUI()
		{
			return false;
		}

		public override string GetTitle()
		{
			return Localization.GetWord("DataBranchRerouteNode");
		}

		protected override float GetWidth()
		{
			return 32f;
		}

		protected override bool HasContentBackground()
		{
			return false;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			isNormalInvisibleStyle = true;
			showContextMenuInWindow = ShowContextMenu.Show;
			isUsedMouseDownOnMainGUI = false;
			isResizable = false;
		}

		protected override void OnUndoRedoPerformed()
		{
			if (dataBranchRerouteNode == null)
			{
				return;
			}

			_DirectionElement.SetValueWithoutNotify(dataBranchRerouteNode.direction);
			_LinkElement.DoChangedPosition();
		}

		private DataBranchRerouteElement _LinkElement;
		private DirectionElement _DirectionElement;

		protected override VisualElement CreateContentElement()
		{
			VisualElement root = new VisualElement();
			root.AddToClassList("reroute-node-content");

			_LinkElement = new DataBranchRerouteElement(this);
			root.Add(_LinkElement);

			_DirectionElement = new DirectionElement()
			{
				arrowColor = _LinkElement.pinColor,
			};
			_DirectionElement.StretchToParentSize();
			_DirectionElement.RegisterCallback<ChangeEvent<Vector2>>(OnChangeDirection);

			_DirectionElement.SetValueWithoutNotify(dataBranchRerouteNode.direction);
			root.Add(_DirectionElement);

			if (!isSelection)
			{
				_DirectionElement.style.display = DisplayStyle.None;
			}

			return root;
		}

		void OnChangeDirection(ChangeEvent<Vector2> e)
		{
			Undo.RecordObject(node.nodeGraph, "Change Reroute Direction");
			dataBranchRerouteNode.direction = e.newValue;
			EditorUtility.SetDirty(node.nodeGraph);

			_LinkElement.DoChangedPosition();
		}

		protected override void RegisterCallbackOnElement()
		{
			base.RegisterCallbackOnElement();

			nodeElement.RegisterCallback<RebuildElementEvent>(OnRebuildElement);
		}

		void OnRebuildElement(RebuildElementEvent e)
		{
			DataSlot link = dataBranchRerouteNode.link;
			link.enabledGUI = true;
		}

		protected override void OnChangeSelection(bool isSelection)
		{
			base.OnChangeSelection(isSelection);

			if (isSelection)
			{
				_DirectionElement.style.display = DisplayStyle.Flex;
			}
			else
			{
				_DirectionElement.style.display = DisplayStyle.None;
			}
		}

		protected override void OnRepainted()
		{
			base.OnRepainted();

			DataSlot link = dataBranchRerouteNode.link;
			if (Event.current.type == EventType.Repaint)
			{
				link.SetVisible();

				bool oldVisible = link.isVisible;
				link.ClearVisible();
				if (oldVisible != link.isVisible)
				{
					//changed = true;
				}
			}
		}

		void DeleteKeepConnection()
		{
			Undo.IncrementCurrentGroup();
			int undoGroup = Undo.GetCurrentGroup();

			NodeGraph nodeGraph = node.nodeGraph;

			DataBranch inputBranch = dataBranchRerouteNode.link.inputSlot.GetBranch();

			int outNodeID = inputBranch.outNodeID;
			Object outBehaviour = inputBranch.outBehaviour;
			DataSlot outputSlot = inputBranch.outputSlot;

			graphEditor.DeleteDataBranch(inputBranch);

			for (int count = dataBranchRerouteNode.link.outputSlot.branchCount, i = count - 1; i >= 0; i--)
			{
				DataBranch outputBranch = dataBranchRerouteNode.link.outputSlot.GetBranch(i);

				int inNodeID = outputBranch.inNodeID;
				Object inBehaviour = outputBranch.inBehaviour;
				DataSlot inputSlot = outputBranch.inputSlot;

				graphEditor.DeleteDataBranch(outputBranch);

				Undo.RecordObject(nodeGraph, "Delete Keep Connection");

				DataBranch currentBranch = graphEditor.ConnectDataBranch(inNodeID, inBehaviour, inputSlot, outNodeID, outBehaviour, outputSlot);
				if (currentBranch != null)
				{
					currentBranch.lineBezier = new Bezier2D(inputBranch.lineBezier.startPosition, inputBranch.lineBezier.startControl, outputBranch.lineBezier.endPosition, outputBranch.lineBezier.endControl);
					currentBranch.enabled = true;
				}
			}

			graphEditor.DeleteNodes(new Node[] { node });

			Undo.CollapseUndoOperations(undoGroup);

			EditorUtility.SetDirty(nodeGraph);

			Repaint();
		}

		bool IsConnected()
		{
			DataBranch inputBranch = dataBranchRerouteNode.link.inputSlot.GetBranch();

			if (inputBranch == null)
			{
				return false;
			}

			int count = dataBranchRerouteNode.link.outputSlot.branchCount;
			if (count == 0)
			{
				return false;
			}

			return true;
		}

		protected override void SetDeleteContextMenu(GenericMenu menu, bool deletable, bool editable)
		{
			if (deletable && IsConnected() && editable)
			{
				menu.AddItem(EditorContents.deleteKeepConnection, false, DeleteKeepConnection);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.deleteKeepConnection);
			}
		}

		public override MinimapLayer minimapLayer
		{
			get
			{
				return MinimapLayer.None;
			}
		}
	}
}