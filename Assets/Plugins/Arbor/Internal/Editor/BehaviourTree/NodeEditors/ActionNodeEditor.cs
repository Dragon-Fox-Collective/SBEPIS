//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor.BehaviourTree
{
	using Arbor;
	using Arbor.BehaviourTree;
	using UnityEngine.UIElements;

	[CustomNodeEditor(typeof(ActionNode))]
	internal sealed class ActionNodeEditor : TreeBehaviourNodeEditor
	{
		ActionNode actionNode
		{
			get
			{
				return node as ActionNode;
			}
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			isShowableComment = true;
		}

		protected override Styles.Color GetNormalStyleColor()
		{
			return Styles.Color.Purple;
		}

		protected override Texture2D GetDefaultIcon()
		{
			return Icons.defaultActionIcon;
		}

		void OnReplaceAction(Vector2 position, System.Type classType)
		{
			string oldActionName = string.Empty;

			ActionBehaviour oldAction = actionNode.behaviour as ActionBehaviour;
			if ((object)oldAction != null)
			{
				BehaviourInfo behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(oldAction.GetType());
				oldActionName = behaviourInfo.titleContent.text;
			}

			Undo.IncrementCurrentGroup();

			actionNode.DestroyBehaviour();
			var behaviour = actionNode.CreateActionBehaviour(classType);

			if (!string.IsNullOrEmpty(oldActionName))
			{
				Undo.RecordObject(graphEditor.nodeGraph, "Replaced ActionBehaviour");

				BehaviourInfo behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(classType);
				actionNode.name = actionNode.name.Replace(oldActionName, behaviourInfo.titleContent.text);

				EditorUtility.SetDirty(graphEditor.nodeGraph);
			}

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			ReplaceMainBehaviour(behaviour);

			graphEditor.RaiseOnChangedNodes();
		}

		void OnReplaceContextMenu(object obj)
		{
			Rect buttonRect = (Rect)obj;

			ActionBehaviourMenuWindow.instance.Init(Vector2.zero, buttonRect, OnReplaceAction, null, null);
		}

		protected override void SetReplaceBehaviourMenu(GenericMenu menu, Rect headerPosition, bool editable)
		{
			if (!Application.isPlaying && editable)
			{
				Rect buttonRect = GUIUtility.GUIToScreenRect(headerPosition);
				menu.AddItem(EditorContents.replaceAction, false, OnReplaceContextMenu, buttonRect);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.replaceAction);
			}
		}
	}
}