//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.Collections.Generic;

namespace ArborEditor
{
	using Arbor;
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;
	using ArborEditor.UIElements;

	[System.Serializable]
	public abstract class BehaviourEditorList<TEditor,TBehaviour> where TEditor : BehaviourEditorGUI, new() where TBehaviour : NodeBehaviour
	{
		public NodeEditor nodeEditor = null;
		public NodeGraphEditor graphEditor
		{
			get
			{
				if (nodeEditor == null)
				{
					return null;
				}
				return nodeEditor.graphEditor;
			}
		}
		public Node node
		{
			get
			{
				if (nodeEditor == null)
				{
					return null;
				}
				return nodeEditor.node;
			}
		}

		public virtual Color backgroundColor
		{
			get
			{
				return Color.white;
			}
		}

		public virtual string backgroundClassName
		{
			get
			{
				return "behaviour-list-background";
			}
		}

		public abstract System.Type targetType
		{
			get;
		}

		public virtual bool isDroppableParameter
		{
			get
			{
				return false;
			}
		}

		private List<TEditor> _BehaviourEditors = null;
		private List<TEditor> _NewBehaviourEditors = null;
		private List<DropBehaviourElement> _DropBehaviourElements = new List<DropBehaviourElement>();

		public abstract int GetCount();
		public abstract Object GetObject(int behaviourIndex);
		public abstract void InsertBehaviour(int index, System.Type classType);
		public abstract void MoveBehaviour(Node fromNode, int fromIndex, Node toNode, int toIndex, bool isCopy);
		public abstract void OpenBehaviourMenu(Rect buttonRect, int index);
		public abstract void PasteBehaviour(int index);
		public abstract GUIContent GetInsertButtonContent();
		public abstract GUIContent GetAddBehaviourContent();
		public abstract GUIContent GetPasteBehaviourContent();

		public void RebuildBehaviourEditors()
		{
			if (_NewBehaviourEditors == null)
			{
				_NewBehaviourEditors = new List<TEditor>();
			}
			List<TEditor> newBehaviourEditors = _NewBehaviourEditors;

			int oldCount = (_BehaviourEditors != null) ? _BehaviourEditors.Count : 0;

			int behaviourCount = GetCount();
			for (int i = 0, count = behaviourCount; i < count; i++)
			{
				Object behaviourObj = GetObject(i);

				TEditor behaviourEditor = null;

				if (_BehaviourEditors != null)
				{
					for (int editorIndex = 0; editorIndex < _BehaviourEditors.Count; editorIndex++)
					{
						TEditor e = _BehaviourEditors[editorIndex];
						if (e.behaviourObj == behaviourObj)
						{
							behaviourEditor = e;
							_BehaviourEditors.Remove(e);
							break;
						}
					}
				}

				if (behaviourEditor == null)
				{
					behaviourEditor = new TEditor();
					behaviourEditor.Initialize(nodeEditor, behaviourObj);
				}

				behaviourEditor.behaviourIndex = i;
				Color backgroundColor = this.backgroundColor;
				if (backgroundColor != Color.white)
				{
					behaviourEditor.backgroundColor = backgroundColor;
				}
				else
				{
					behaviourEditor.backgroundColor = null;
				}
				newBehaviourEditors.Add(behaviourEditor);

				var behaviourElement = behaviourEditor.element;
				if (behaviourElement == null)
				{
					behaviourElement = behaviourEditor.CreateElement();
					behaviourElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedBehaviourElement);
					_RootElement.Insert(i, behaviourElement);
				}
				else
				{
					if (_RootElement.hierarchy.IndexOf(behaviourElement) != i)
					{
						_RootElement.Insert(i, behaviourElement);
					}
				}

				if (behaviourEditor.overlayLayer.parent == null)
				{
					nodeEditor.nodeElement.overlayLayer.Add(behaviourEditor.overlayLayer);
				}
			}

			if (_BehaviourEditors != null)
			{
				for (int i = 0; i < _BehaviourEditors.Count; i++)
				{
					TEditor e = _BehaviourEditors[i];

					var behaviourElement = e.element;
					behaviourElement?.UnregisterCallback<GeometryChangedEvent>(OnGeometryChangedBehaviourElement);
					e.Destroy();
				}
				_BehaviourEditors.Clear();
			}

			_NewBehaviourEditors = _BehaviourEditors;

			_BehaviourEditors = newBehaviourEditors;

			int newCount = (_BehaviourEditors != null) ? _BehaviourEditors.Count : 0;
			if (oldCount != newCount)
			{
				UpdateBackground();
			}
			RebuildDropBehaviourElements();
		}

		void RebuildDropBehaviourElements()
		{
			int handlerCount = _BehaviourEditors.Count + 1;
			int currentCount = _DropBehaviourElements.Count;

			if (currentCount > handlerCount)
			{
				for (int i = currentCount - 1; i >= handlerCount; i--)
				{
					_DropBehaviourElements[i].RemoveFromHierarchy();
					_DropBehaviourElements.RemoveAt(i);
				}
			}
			else
			{
				for (int i = currentCount; i < handlerCount; i++)
				{
					var dropElement = new DropBehaviourElement(this, i);
					_OverlayLayer.Add(dropElement);
					_DropBehaviourElements.Add(dropElement);

					VisualElement target = (i < _BehaviourEditors.Count) ? _BehaviourEditors[i].element : _RootElement;
					if (target != null)
					{
						UpdateDropElementLayout(dropElement, target, i == handlerCount-1);
					}
				}
			}
		}

		void UpdateDropElementLayout(DropBehaviourElement dropElement, VisualElement target, bool last)
		{
			Rect layout = target.layout;
			Rect dropRect = new Rect(0f, (!last) ? 0f : layout.height, layout.width, 0f);

			dropRect = target.ChangeCoordinatesTo(nodeEditor.nodeElement, dropRect);

			dropElement.UpdateLayout(dropRect);
		}

		public TEditor GetBehaviourEditor(int behaviourIndex)
		{
			using (new ProfilerScope("GetBehaviourEditor"))
			{
				TEditor behaviourEditor = _BehaviourEditors[behaviourIndex];

				if (!ComponentUtility.IsValidObject(behaviourEditor.behaviourObj))
				{
					Object behaviourObj = GetObject(behaviourIndex);
					behaviourEditor.Repair(behaviourObj);
				}

				if (behaviourEditor != null)
				{
#if ARBOR_DEBUG
					if (behaviourEditor.behaviourIndex != behaviourIndex)
					{
						Debug.Log(behaviourEditor.behaviourIndex + " -> " + behaviourIndex);
					}
#endif
					behaviourEditor.behaviourIndex = behaviourIndex;
				}

				return behaviourEditor;
			}
		}

		public void OnEnable()
		{
			if (_BehaviourEditors != null)
			{
				for (int i = 0, count = _BehaviourEditors.Count; i < count; i++)
				{
					TEditor behaviourEditor = _BehaviourEditors[i];
					behaviourEditor.DoEnable();
				}
			}
		}

		public void OnDisable()
		{
			if (_BehaviourEditors != null)
			{
				for (int i = 0, count = _BehaviourEditors.Count; i < count; i++)
				{
					TEditor behaviourEditor = _BehaviourEditors[i];
					behaviourEditor.DoDisable();
				}
			}
		}

		void UpdateBackground()
		{
			_RootElement.EnableInClassList(backgroundClassName, _BehaviourEditors.Count > 0);
		}

		public void Update()
		{
			for (int i = 0, count = _BehaviourEditors.Count; i < count; i++)
			{
				TEditor behaviourEditor = _BehaviourEditors[i];

				behaviourEditor.Update();
			}
		}

		public void OnRepainted()
		{
			for (int i = 0, count = _BehaviourEditors.Count; i < count; i++)
			{
				TEditor behaviourEditor = _BehaviourEditors[i];

				behaviourEditor.OnRepainted();
			}
		}

		public void Validate(bool onInitialize)
		{
			RebuildBehaviourEditors();

			for (int i = 0, count = _BehaviourEditors.Count; i < count; i++)
			{
				TEditor behaviourEditor = _BehaviourEditors[i];
				
				if (!ComponentUtility.IsValidObject(behaviourEditor.behaviourObj))
				{
					Object behaviourObj = GetObject(i);
					behaviourEditor.Repair(behaviourObj);
				}
				else if(onInitialize)
				{
					behaviourEditor.RepairEditor();
				}
			}
		}

		public void MoveBehaviourEditor(int fromIndex, int toIndex)
		{
			List<TEditor> behaviourEditors = _BehaviourEditors;

			TEditor tempEditor = behaviourEditors[toIndex];
			behaviourEditors[toIndex] = behaviourEditors[fromIndex];
			behaviourEditors[fromIndex] = tempEditor;

			behaviourEditors[fromIndex].behaviourIndex = fromIndex;
			behaviourEditors[toIndex].behaviourIndex = toIndex;

			behaviourEditors[fromIndex].element.PlaceBehind(_RootElement.hierarchy[fromIndex]);
			behaviourEditors[toIndex].element.PlaceBehind(_RootElement.hierarchy[toIndex]);
		}

		public void RemoveBehaviourEditor(int behaviourIndex)
		{
			List<TEditor> behaviourEditors = _BehaviourEditors;

			TEditor behaviourEditor = behaviourEditors[behaviourIndex];

			if (behaviourEditor != null)
			{
				var behaviourElement = behaviourEditor.element;
				behaviourElement?.UnregisterCallback<GeometryChangedEvent>(OnGeometryChangedBehaviourElement);
				behaviourEditor.Destroy();
			}

			behaviourEditors.RemoveAt(behaviourIndex);

			for (int i = behaviourIndex, count = behaviourEditors.Count; i < count; i++)
			{
				TEditor e = behaviourEditors[i];
				e.behaviourIndex = i;
			}

			UpdateBackground();
			RebuildDropBehaviourElements();
		}

		public void DestroyEditors()
		{
			if (_BehaviourEditors == null)
			{
				return;
			}

			int editorCount = _BehaviourEditors.Count;
			for (int editorIndex = 0; editorIndex < editorCount; editorIndex++)
			{
				var e = _BehaviourEditors[editorIndex];

				var behaviourElement = e.element;
				behaviourElement?.UnregisterCallback<GeometryChangedEvent>(OnGeometryChangedBehaviourElement);
				e.Destroy();
			}
			_BehaviourEditors.Clear();

			UpdateBackground();
		}

		void OnGeometryChangedBehaviourElement(GeometryChangedEvent e)
		{
			VisualElement target = e.target as VisualElement;

			BehaviourEditorGUI.BehaviourElement element = target as BehaviourEditorGUI.BehaviourElement;
			int behaviourIndex = (element != null) ? element.editorGUI.behaviourIndex : _DropBehaviourElements.Count - 1;

			DropBehaviourElement dropElement = _DropBehaviourElements[behaviourIndex];

			UpdateDropElementLayout(dropElement, target, behaviourIndex == _DropBehaviourElements.Count -1);
		}

		public TEditor InsertBehaviourEditor(int behaviourIndex, Object behaviourObj)
		{
			TEditor behaviourEditor = new TEditor();
			behaviourEditor.Initialize(nodeEditor, behaviourObj);
			behaviourEditor.behaviourIndex = behaviourIndex;

			VisualElement behaviourElement = behaviourEditor.CreateElement();
			behaviourElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedBehaviourElement);
			_RootElement.Insert(behaviourIndex, behaviourElement);

			nodeEditor.nodeElement.overlayLayer.Add(behaviourEditor.overlayLayer);
			
			if (_BehaviourEditors == null)
			{
				_BehaviourEditors = new List<TEditor>();
			}

			_BehaviourEditors.Insert(behaviourIndex, behaviourEditor);

			for (int i = behaviourIndex + 1, count = _BehaviourEditors.Count; i < count; i++)
			{
				TEditor e = _BehaviourEditors[i];
				e.behaviourIndex = i;
			}

			UpdateBackground();
			RebuildDropBehaviourElements();

			return behaviourEditor;
		}

		public virtual void InsertSetParameter(int index, Parameter parameter)
		{
		}

		private VisualElement _RootElement;
		private VisualElement _OverlayLayer;

		public VisualElement CreateElement()
		{
			var background = new VisualElement();

			_RootElement = background;
			_RootElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedBehaviourElement);

			_OverlayLayer = new VisualElement();
			nodeEditor.nodeElement.overlayLayer.Add(_OverlayLayer);

			RebuildBehaviourEditors();
			UpdateBackground();

			return _RootElement;
		}

		class DropBehaviourElement : VisualElement, IDropElement
		{
			private BehaviourEditorList<TEditor, TBehaviour> _Owner;

			private int _Index;

			private bool _IsEntered;
			private bool _IsDragging;

			private VisualElement _InsertBar;
			private VisualElement _PopupButton;
			private VisualElement _InsertionElement;

			private const float kInsertionBarHeight = 8f;

			public DropBehaviourElement(BehaviourEditorList<TEditor, TBehaviour> owner, int index)
			{
				_Owner = owner;
				_Index = index;
				pickingMode = PickingMode.Ignore;

				RegisterCallback<DragEnterEvent>(OnDragEnter);
				RegisterCallback<DragLeaveEvent>(OnDragLeave);
				RegisterCallback<DragUpdatedEvent>(OnDragUpdated);
				RegisterCallback<DragPerformEvent>(OnDragPerform);
				RegisterCallback<DragExitedEvent>(OnDragExited);

				_InsertionElement = new VisualElement()
				{
					pickingMode = PickingMode.Ignore,
				};
				_InsertionElement.AddToClassList("drop-insertionbar");

				_InsertBar = new VisualElement()
				{
					style =
					{
						position = Position.Absolute,
					}
				};
				Add(_InsertBar);

				_InsertBar.RegisterCallback<MouseMoveEvent>(OnMouseEnterInsertBar);

				RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
			}

			void OnMouseEnterInsertBar(MouseMoveEvent e)
			{
				if ((_IsEntered && _IsDragging) || _PopupButton != null)
				{
					return;
				}

				NodeGraphEditor graphEditor = _Owner.graphEditor;
				if (graphEditor != null && !graphEditor.editable)
				{
					return;
				}

				VisualElement target = e.target as VisualElement;

				Rect position = target.contentRect;
				
				GUIStyle style = Styles.addBehaviourButton;
				GUIContent content = _Owner.GetInsertButtonContent();

				GraphView graphView = graphEditor.graphView;

				Vector2 attachPoint = new Vector2(e.localMousePosition.x, position.center.y);
				attachPoint = graphView.ElementToGraph(target, attachPoint);

				Rect buttonRect = target.LocalToScreen(position);

				Vector2 center = buttonRect.center;
				buttonRect.width = 300f;
				buttonRect.height = 0f;
				buttonRect.center = center;

				_PopupButton = _Owner.graphEditor.ShowPopupButtonControl(attachPoint, content, 0, style, (Rect rect) =>
				{
					GenericMenu menu = new GenericMenu();
					menu.AddItem(_Owner.GetAddBehaviourContent(), false, () =>
					{
						_Owner.OpenBehaviourMenu(buttonRect, _Index);
					});

					GUIContent pasteContent = _Owner.GetPasteBehaviourContent();

					if (Clipboard.CompareBehaviourType(typeof(TBehaviour), true))
					{
						menu.AddItem(pasteContent, false, () =>
						{
							_Owner.PasteBehaviour(_Index);
						});
					}
					else
					{
						menu.AddDisabledItem(pasteContent);
					}

					menu.DropDown(rect);
				});

				ShowInsertionBar(true);

				_PopupButton.RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanelPopupButton);
			}

			void ShowInsertionBar(bool show)
			{
				if (show)
				{
					if (_InsertionElement.parent == null)
					{
						Add(_InsertionElement);
					}
				}
				else
				{
					if (_InsertionElement.parent != null)
					{
						_InsertionElement.RemoveFromHierarchy();
					}
				}
			}

			void OnDetachFromPanel(DetachFromPanelEvent e)
			{
				_PopupButton?.RemoveFromHierarchy();
			}

			void OnDetachFromPanelPopupButton(DetachFromPanelEvent e)
			{
				_PopupButton.UnregisterCallback<DetachFromPanelEvent>(OnDetachFromPanelPopupButton);
				_PopupButton = null;

				ShowInsertionBar(false);
			}

			void OnDragEnter(DragEnterEvent e)
			{
				_IsEntered = true;
			}

			void OnDragUpdated(DragUpdatedEvent e)
			{
				DragUpdated(false, e);
				e.StopPropagation();
			}

			void OnDragPerform(DragPerformEvent e)
			{
				DragUpdated(true, e);
			}

			void OnDragLeave(DragLeaveEvent e)
			{
				_IsEntered = false;
				_IsDragging = false;

				ShowInsertionBar(false);
			}

			void OnDragExited(DragExitedEvent e)
			{
				_IsEntered = false;
				_IsDragging = false;

				ShowInsertionBar(false);
			}

			void DragUpdated(bool perform, IMouseEvent mouseEvent)
			{
				NodeGraphEditor graphEditor = _Owner.graphEditor;
				if (graphEditor != null && !graphEditor.editable)
				{
					return;
				}
				BehaviourDragInfo behaviourDragInfo = BehaviourDragInfo.GetBehaviourDragInfo();
				bool isDroppableParameter = _Owner.isDroppableParameter;

				int insertIndex = _Index;
				bool isAccepted = false;
				_IsDragging = false;

				var objectReferences = DragAndDrop.objectReferences;
				for (int i = 0; i < objectReferences.Length; i++)
				{
					Object draggedObject = objectReferences[i];
					MonoScript script = draggedObject as MonoScript;
					if (script != null)
					{
						System.Type classType = script.GetClass();

						if (classType != null && classType.IsSubclassOf(_Owner.targetType))
						{
							_IsDragging = true;
							DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

							if (perform)
							{
								_Owner.InsertBehaviour(insertIndex, classType);
								insertIndex++;

								isAccepted = true;
							}
						}
					}

					if (isDroppableParameter)
					{
						ParameterDraggingObject parameterDraggingObject = draggedObject as ParameterDraggingObject;
						if (parameterDraggingObject != null)
						{
							_IsDragging = true;
							DragAndDrop.visualMode = DragAndDropVisualMode.Link;

							if (perform)
							{
								_Owner.InsertSetParameter(insertIndex, parameterDraggingObject.parameter);
								insertIndex++;

								isAccepted = true;
							}
						}
					}

					if (behaviourDragInfo != null && behaviourDragInfo.behaviourEditor != null && behaviourDragInfo.dragging && behaviourDragInfo.behaviourEditor.behaviourObj == draggedObject)
					{
						BehaviourEditorGUI behaviourEditor = behaviourDragInfo.behaviourEditor;

						if (typeof(TEditor).IsAssignableFrom(behaviourEditor.GetType()))
						{
							Node fromNode = null;
							int fromIndex = -1;

							bool isCopy = (Application.platform == RuntimePlatform.OSXEditor) ? mouseEvent.altKey : mouseEvent.ctrlKey;

							if (behaviourEditor.nodeEditor.graphEditor.nodeGraph == _Owner.node.nodeGraph)
							{
								fromNode = behaviourEditor.nodeEditor.node;
							}
							else
							{
								fromNode = _Owner.node;
							}

							fromIndex = behaviourEditor.behaviourIndex;
							if (fromIndex >= 0)
							{
								if (!isCopy && fromNode == _Owner.node)
								{
									if (fromIndex < insertIndex)
									{
										insertIndex--;
									}
								}

								if (fromNode == _Owner.node && !isCopy)
								{
									_IsDragging = fromIndex != insertIndex;
								}
								else if (behaviourEditor.behaviourObj != null)
								{
									System.Type classType = behaviourEditor.behaviourObj.GetType();
									_IsDragging = classType.IsSubclassOf(_Owner.targetType);
								}

								if (_IsDragging)
								{
									if (isCopy)
									{
										DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
									}
									else
									{
										DragAndDrop.visualMode = DragAndDropVisualMode.Move;
									}

									if (perform)
									{
										_Owner.MoveBehaviour(fromNode, fromIndex, _Owner.node, insertIndex, isCopy);

										isAccepted = true;
									}
								}
							}
						}
					}
				}

				if (isAccepted)
				{
					DragAndDrop.AcceptDrag();
					DragAndDrop.activeControlID = 0;
				}

				ShowInsertionBar(_IsEntered && _IsDragging);
			}

			public void UpdateLayout(Rect position)
			{
				Rect dropRect = new Rect(position);
				dropRect.height = EditorGUIUtility.singleLineHeight;
				dropRect.y -= dropRect.height * 0.5f;

				this.SetLayout(dropRect);
				dropRect.position = Vector2.zero;

				Rect insertBarRect = new Rect(0f, 0f, 32f, EditorGUIUtility.singleLineHeight * 0.5f);
				insertBarRect.position = dropRect.center - insertBarRect.size * 0.5f;
				
				_InsertBar.SetLayout(insertBarRect);

				_InsertionElement.style.top = (dropRect.height - kInsertionBarHeight) * 0.5f;
			}
		}
	}
}