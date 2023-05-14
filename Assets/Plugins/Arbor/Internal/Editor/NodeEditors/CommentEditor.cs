//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

using Arbor;
using UnityEngine.UIElements;

namespace ArborEditor
{
	using ArborEditor.UIElements;

	[CustomNodeEditor(typeof(CommentNode))]
	internal sealed class CommentEditor : NodeEditor
	{
		public CommentNode comment
		{
			get
			{
				return node as CommentNode;
			}
		}

		public override Texture2D GetIcon()
		{
			return Icons.commentNodeIcon;
		}

		public override Styles.Color GetStyleColor()
		{
			return Styles.Color.Yellow;
		}

		void CommentField()
		{
			GUIStyle style = EditorStyles.textArea;

			EditorGUI.BeginDisabledGroup(!graphEditor.editable);

			EditorGUI.BeginChangeCheck();
			string commentText = EditorGUILayout.TextArea(comment.comment, style);
			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(comment.nodeGraph, "Change Comment");

				comment.comment = commentText;

				EditorUtility.SetDirty(comment.nodeGraph);
			}

			EditorGUI.EndDisabledGroup();
		}

		protected override VisualElement CreateContentElement()
		{
			return new NodeContentIMGUIContainer(DoGUI);
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			isRenamable = true;
		}

		void DoGUI()
		{
			using (new ProfilerScope("OnCommentGUI"))
			{
				EditorGUITools.DrawSeparator();

				CommentField();
			}
		}
	}
}
