//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.BehaviourTree
{
	using Arbor.BehaviourTree;
	using ArborEditor.BehaviourTree.UIElements;
	using UnityEngine.UIElements;

	[CustomNodeEditor(typeof(RootNode))]
	internal sealed class RootNodeEditor : TreeNodeBaseEditor
	{
		public RootNode rootNode
		{
			get
			{
				return node as RootNode;
			}
		}

		protected override float GetWidth()
		{
			return 180f;
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			isShowContextMenuInHeader = false;
			isShowableComment = true;
			isResizable = false;
		}

		protected override VisualElement CreateFooterElement()
		{
			_ChildLinkSlotElement = new ChildLinkSlotElement(this);
			return _ChildLinkSlotElement;
		}

		protected override Styles.Color GetNormalStyleColor()
		{
			return Styles.Color.Aqua;
		}

		public override Texture2D GetIcon()
		{
			return Icons.rootIcon;
		}

		public override bool IsCopyable()
		{
			return false;
		}

		public override void RegisterDragChildren()
		{
			RegisterDragChild(rootNode.GetChildBranch());
		}
	}
}