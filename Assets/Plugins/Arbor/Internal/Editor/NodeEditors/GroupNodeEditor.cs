//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

using Arbor;
using UnityEngine.UIElements;

namespace ArborEditor
{
	using ArborEditor.UIElements;

	[CustomNodeEditor(typeof(GroupNode))]
	internal sealed class GroupNodeEditor : NodeEditor
	{
		protected override bool IsWindow()
		{
			return false;
		}

		public override void OnBeginDrag(bool altKey)
		{
			base.OnBeginDrag(altKey);

			if (!altKey)
			{
				List<NodeEditor> nodeEditorsInRect = graphEditor.GetNodeEditorsInRect(this.rect);
				for (int nodeIndex = 0; nodeIndex < nodeEditorsInRect.Count; nodeIndex++)
				{
					NodeEditor targetNodeEditor = nodeEditorsInRect[nodeIndex];
					if (!(targetNodeEditor.node is GroupNode))
					{
						graphEditor.RegisterDragNode(targetNodeEditor.node);
					}
				}
			}
		}

		public override Styles.BaseColor GetStyleBaseColor()
		{
			return Styles.BaseColor.White;
		}

		public override Styles.Color GetStyleColor()
		{
			return Styles.Color.White;
		}

		public override Rect GetSelectableRect()
		{
			return GetHeaderRect();
		}

		protected override void OnEnable()
		{
			base.OnEnable();

			isRenamable = true;
			isShowableComment = true;
			showContextMenuInWindow = ShowContextMenu.None;
		}

		public override Texture2D GetIcon()
		{
			GroupNode group = node as GroupNode;
			switch (group.autoAlignment)
			{
				case GroupNode.AutoAlignment.Vertical:
					return Icons.groupNodeIconVertical;
				case GroupNode.AutoAlignment.Horizonal:
					return Icons.groupNodeIconHorizontal;
			}
			return Icons.groupNodeIcon;
		}

		private PopupWindowContent _SettingsWindow = null;

		protected override void SetContextMenu(GenericMenu menu, Rect headerPosition, bool editable)
		{
			Rect mousePosition = new Rect(0, 0, 0, 0);
			mousePosition.position = Event.current.mousePosition;
			Rect position = GUIUtility.GUIToScreenRect(mousePosition);

			if (editable)
			{
				menu.AddItem(EditorContents.settings, false, () =>
				{
					if (_SettingsWindow == null)
					{
						_SettingsWindow = new GroupNodeSettingsWindow(graphEditor.hostWindow, node as GroupNode);
					}
					position = GUIUtility.ScreenToGUIRect(position);
					PopupWindowUtility.Show(position, _SettingsWindow, false);
				});
			}
			else
			{
				menu.AddDisabledItem(EditorContents.settings);
			}
		}

		private Dictionary<NodeEditor, Rect> _LastNodePositions = new Dictionary<NodeEditor, Rect>();

		struct AlignmentRect
		{
			private Rect _Rect;
			private GroupNode.AutoAlignment _Alignment;

			private float _Position;

			public Rect rect
			{
				get
				{
					return _Rect;
				}
			}

			public float position
			{
				get
				{
					return _Position;
				}
				set
				{
					switch (_Alignment)
					{
						case GroupNode.AutoAlignment.Vertical:
							_Rect.y = value;
							_Position = _Rect.y;
							maxPosition = _Rect.yMax;
							break;
						case GroupNode.AutoAlignment.Horizonal:
							_Rect.x = value;
							_Position = _Rect.x;
							maxPosition = _Rect.xMax;
							break;
					}
				}
			}

			public float maxPosition
			{
				get;
				private set;
			}

			static float GetPosition(Rect rect, GroupNode.AutoAlignment alignment)
			{
				switch (alignment)
				{
					case GroupNode.AutoAlignment.Vertical:
						return rect.y;
					case GroupNode.AutoAlignment.Horizonal:
						return rect.x;
				}

				return rect.x;
			}

			static float GetMaxPosition(Rect rect, GroupNode.AutoAlignment alignment)
			{
				switch (alignment)
				{
					case GroupNode.AutoAlignment.Vertical:
						return rect.yMax;
					case GroupNode.AutoAlignment.Horizonal:
						return rect.xMax;
				}

				return rect.xMax;
			}

			public AlignmentRect(Rect rect, GroupNode.AutoAlignment alignment)
			{
				_Rect = rect;
				_Alignment = alignment;

				_Position = GetPosition(rect, alignment);
				maxPosition = GetMaxPosition(rect, alignment);
			}
		}

		public void AutoLayout()
		{
			GroupNode groupNode = node as GroupNode;
			GroupNode.AutoAlignment autoAlignment = groupNode.autoAlignment;

			List<NodeEditor> nodeEditors = new List<NodeEditor>();

			var nodeEditorsInRect = graphEditor.GetNodeEditorsInRect(this.rect);
			for (int nodeIndex = 0; nodeIndex < nodeEditorsInRect.Count; nodeIndex++)
			{
				NodeEditor targetNodeEditor = nodeEditorsInRect[nodeIndex];
				if (!(targetNodeEditor.node is GroupNode))
				{
					nodeEditors.Add(targetNodeEditor);
				}
			}

			int nodeCount = nodeEditors.Count;

			if (autoAlignment != GroupNode.AutoAlignment.None)
			{
				nodeEditors.Sort((a, b) =>
				{
					AlignmentRect alignA = new AlignmentRect(a.rect, autoAlignment);
					AlignmentRect alignB = new AlignmentRect(b.rect, autoAlignment);
					return alignA.position.CompareTo(alignB.position);
				});

				for (int i = 0; i < nodeCount - 1; i++)
				{
					NodeEditor nodeEditor1 = nodeEditors[i];

					Rect nodeEditor1Rect = nodeEditor1.rect;

					Rect lastRect1;
					bool hasLastRect1 = _LastNodePositions.TryGetValue(nodeEditor1, out lastRect1);

					AlignmentRect align1 = new AlignmentRect(nodeEditor1Rect, autoAlignment);
					AlignmentRect lastAlign1 = new AlignmentRect(lastRect1, autoAlignment);

					for (int j = i + 1; j < nodeCount; j++)
					{
						NodeEditor nodeEditor2 = nodeEditors[j];

						Rect nodeEditor2Rect = nodeEditor2.rect;

						Rect lastRect2 = nodeEditor2Rect;
						bool hasLastRect2 = _LastNodePositions.TryGetValue(nodeEditor2, out lastRect2);

						AlignmentRect align2 = new AlignmentRect(nodeEditor2Rect, autoAlignment);
						AlignmentRect lastAlign2 = new AlignmentRect(lastRect2, autoAlignment);

						if (nodeEditor1Rect.Overlaps(nodeEditor2Rect))
						{
							float space = EditorGUITools.GetSnapSpace();
							if (hasLastRect1 && hasLastRect2)
							{
								if (lastAlign1.maxPosition < lastAlign2.position)
								{
									space = lastAlign2.position - lastAlign1.maxPosition;
								}
							}

							align2.position = align1.maxPosition + space;
							Rect position = EditorGUITools.SnapPositionToGrid(align2.rect);

							graphEditor.OnAutoLayoutNode(nodeEditor2.node);

							Undo.RecordObject(graphEditor.nodeGraph, "AutoLayout");

							nodeEditor2.rect = position;

							EditorUtility.SetDirty(graphEditor.nodeGraph);

							nodeEditor2.nodeElement.UpdatePosition();
						}
					}
				}
			}

			_LastNodePositions.Clear();
			for (int i = 0; i < nodeCount; i++)
			{
				NodeEditor n = nodeEditors[i];
				_LastNodePositions[n] = n.rect;
			}
		}

		public override MinimapLayer minimapLayer
		{
			get
			{
				return MinimapLayer.Underlay;
			}
		}

		protected override ResizeDirection GetResizeDirection()
		{
			return ResizeDirection.Left | ResizeDirection.Right |
				ResizeDirection.Top | ResizeDirection.Bottom;
		}

		protected override Color GetBackgroundColor()
		{
			GroupNode groupNode = node as GroupNode;
			return groupNode.color;
		}

		protected override void OnCreatedMinimapNodeElement()
		{
			base.OnCreatedMinimapNodeElement();

			minimapNodeElement.AddToClassList("group");
		}

		public override Color GetListColor()
		{
			return GetBackgroundColor() * Color.gray;
		}
	}
}
