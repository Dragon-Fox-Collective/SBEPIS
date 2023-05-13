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
	using ArborEditor.BehaviourTree.UIElements;

	[CustomNodeEditor(typeof(CompositeNode))]
	internal sealed class CompositeNodeEditor : TreeBehaviourNodeEditor
	{
		CompositeNode compositeNode
		{
			get
			{
				return node as CompositeNode;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			isShowableComment = true;
		}

		protected override VisualElement CreateFooterElement()
		{
			_ChildLinkSlotElement = new ChildrenLinkSlotElement(this);
			return _ChildLinkSlotElement;
		}

		protected override Texture2D GetDefaultIcon()
		{
			return Icons.defaultCompositeIcon;
		}

		void OnReplaceComposite(Vector2 position, System.Type classType)
		{
			string oldCompositeName = string.Empty;

			CompositeBehaviour oldComposite = compositeNode.behaviour as CompositeBehaviour;
			if ((object)oldComposite != null)
			{
				BehaviourInfo behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(oldComposite.GetType());
				oldCompositeName = behaviourInfo.titleContent.text;
			}

			Undo.IncrementCurrentGroup();

			compositeNode.DestroyBehaviour();
			var behaviour = compositeNode.CreateCompositeBehaviour(classType);

			if (!string.IsNullOrEmpty(oldCompositeName))
			{
				Undo.RecordObject(graphEditor.nodeGraph, "Replaced CompositeBehaviour");

				BehaviourInfo behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(classType);
				compositeNode.name = compositeNode.name.Replace(oldCompositeName, behaviourInfo.titleContent.text);

				EditorUtility.SetDirty(graphEditor.nodeGraph);
			}

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			ReplaceMainBehaviour(behaviour);

			graphEditor.RaiseOnChangedNodes();
		}

		void OnReplaceContextMenu(object obj)
		{
			Rect buttonRect = (Rect)obj;

			CompositeBehaviourMenuWindow.instance.Init(Vector2.zero, buttonRect, OnReplaceComposite, null, null);
		}

		protected override void SetReplaceBehaviourMenu(GenericMenu menu, Rect headerPosition, bool editable)
		{
			if (!Application.isPlaying && editable)
			{
				Rect buttonRect = GUIUtility.GUIToScreenRect(headerPosition);
				menu.AddItem(EditorContents.replaceComposite, false, OnReplaceContextMenu, buttonRect);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.replaceComposite);
			}
		}

		public override void RegisterDragChildren()
		{
			var childrenLinkSlot = compositeNode.GetChildrenLinkSlot();
			for (int i = 0, count = childrenLinkSlot.branchIDs.Count; i < count; i++)
			{
				int branchID = childrenLinkSlot.branchIDs[i];
				RegisterDragChild(branchID);
			}
		}
	}
}