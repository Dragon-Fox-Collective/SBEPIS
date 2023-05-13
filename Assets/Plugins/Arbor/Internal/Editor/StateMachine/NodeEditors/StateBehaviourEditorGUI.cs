//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace ArborEditor
{
	using Arbor;
	using Arbor.Serialization;
	using Arbor.Playables;
	using ArborEditor.UIElements;
	using ArborEditor.UnityEditorBridge.Extensions;

	[System.Serializable]
	public sealed class StateBehaviourEditorGUI : BehaviourEditorGUI
	{
		public StateEditor stateEditor
		{
			get
			{
				return nodeEditor as StateEditor;
			}
		}

		public State state
		{
			get
			{
				return (nodeEditor != null) ? nodeEditor.node as State : null;
			}
		}

		private List<StateLinkElement> _StateLinkElements = new List<StateLinkElement>();

		private Dictionary<SerializedPropertyKey, StateLinkElement> _OldStateLinkElements = new Dictionary<SerializedPropertyKey, StateLinkElement>();

		private static System.Text.StringBuilder s_StateLinkPropertyBuilder = new System.Text.StringBuilder();

		protected override void OnCreatedElement()
		{
			element.RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
			element.RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);

			_StateLinkRootElement = new VisualElement();

			BehaviourInfo behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(behaviourObj);
			_StateLinkHeader = new Label()
			{
				text = behaviourInfo.titleContent.text,
			};
			_StateLinkHeader.AddToClassList("state-link-header");
			UIElementsUtility.SetBoldFont(_StateLinkHeader);
			UpdateStateLinkHeader();

			_StateLinkRootElement.Add(_StateLinkHeader);

			_StateLinkContents = new VisualElement()
			{
				style =
						{
							marginLeft = 4f,
							marginRight = 4f,
						}
			};
			_StateLinkRootElement.Add(_StateLinkContents);

			UpdateStateLink();

			UpdateStateLinkShowMode();
		}

		protected override void OnChangedObject()
		{
			base.OnChangedObject();

			SetStateBehaviour(behaviourObj as StateBehaviour);
		}

		StateBehaviour _StateBehaviour;
		NodeGraph _NodeGraph;

		void SetNodeGraph(NodeGraph nodeGraph)
		{
			if (!ReferenceEquals(_NodeGraph, nodeGraph))
			{
				if (_NodeGraph is object)
				{
					_NodeGraph.onPlayStateChanged -= OnPlayStateChanged;
					NodeGraphCallback.UnregisterStateChangedCallback(_NodeGraph, OnStateChanged);
				}

				_NodeGraph = nodeGraph;

				if (_NodeGraph is object)
				{
					_NodeGraph.onPlayStateChanged += OnPlayStateChanged;
					NodeGraphCallback.RegisterStateChangedCallback(_NodeGraph, OnStateChanged);
				}
			}
		}

		void OnPlayStateChanged(PlayState playState)
		{
			UpdateReservedState();
		}

		void OnStateChanged(NodeGraph nodeGraph)
		{
			UpdateReservedState();
		}

		void SetStateBehaviour(StateBehaviour stateBehaviour)
		{
			if (!ReferenceEquals(_StateBehaviour, stateBehaviour))
			{
				if (_StateBehaviour is object)
				{
					_StateBehaviour.onStateLinkRebuilt -= OnStateLinkRebuilt;
					_StateBehaviour.onBehaviourEnabledChanged -= OnBehaviourEnabledChanged;
				}

				_StateBehaviour = stateBehaviour;

				if (_StateBehaviour is object)
				{
					_StateBehaviour.onStateLinkRebuilt += OnStateLinkRebuilt;
					_StateBehaviour.onBehaviourEnabledChanged += OnBehaviourEnabledChanged;
				}
			}

			if (_StateBehaviour is object)
			{
				SetNodeGraph(_StateBehaviour.nodeGraph);
			}
			else
			{
				SetNodeGraph(null);
			}
		}

		private bool _IsDelayUpdateStateLink = false;

		void OnStateLinkRebuilt()
		{
			if (!_IsDelayUpdateStateLink)
			{
				_IsDelayUpdateStateLink = true;
				EditorApplication.update -= OnUpdateStateLink;
				EditorApplication.update += OnUpdateStateLink;
			}
		}

		void OnBehaviourEnabledChanged()
		{
			UpdateReservedState();
		}

		void OnUpdateStateLink()
		{
			if (_IsDelayUpdateStateLink)
			{
				UpdateStateLink();
				_IsDelayUpdateStateLink = false;
			}

			EditorApplication.update -= OnUpdateStateLink;
		}

		void OnAttachToPanel(AttachToPanelEvent e)
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
			SetStateBehaviour(behaviourObj as StateBehaviour);
		}

		void OnDetachFromPanel(DetachFromPanelEvent e)
		{
			EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
			SetStateBehaviour(null);

			EditorApplication.update -= OnUpdateStateLink;
			_IsDelayUpdateStateLink = false;
		}

		void OnPlayModeStateChanged(PlayModeStateChange state)
		{
			UpdateReservedState();
		}

		void AddStateLinkProperty(SerializedProperty property, System.Reflection.FieldInfo fieldInfo, StateLink stateLink)
		{
			if (stateLink == null)
			{
				return;
			}

			SerializedPropertyKey key = new SerializedPropertyKey(property);

			StateLinkElement stateLinkElement = null;
			if (_OldStateLinkElements.TryGetValue(key, out stateLinkElement))
			{
				_OldStateLinkElements.Remove(key);
			}
			else
			{
				stateLinkElement = new StateLinkElement(stateEditor, behaviourObj as StateBehaviour, key, s_StateLinkPropertyBuilder.ToString());
			}

			stateLinkElement.Setup(stateLink, fieldInfo);

			int index = _StateLinkElements.Count;
			if (stateLinkElement.parent == null || _StateLinkContents.hierarchy.IndexOf(stateLinkElement) != index)
			{
				_StateLinkContents.Insert(index, stateLinkElement);
			}

			_StateLinkElements.Add(stateLinkElement);
		}

		void UpdateStateLinkProperty(SerializedProperty property, System.Reflection.FieldInfo fieldInfo, System.Type fieldType, object value)
		{
			if (property.isArray)
			{
				System.Type elementType = SerializationUtility.ElementTypeOfArray(fieldType);

				int currentLength = s_StateLinkPropertyBuilder.Length;

				IList list = (IList)value;

				for (int i = 0; i < property.arraySize; i++)
				{
					s_StateLinkPropertyBuilder.Append("[");
					s_StateLinkPropertyBuilder.Append(i);
					s_StateLinkPropertyBuilder.Append("]");

					SerializedProperty elementProperty = property.GetArrayElementAtIndex(i);

					object elementValue = list[i];

					if (elementType == typeof(StateLink))
					{
						AddStateLinkProperty(elementProperty, fieldInfo, elementValue as StateLink);
					}
					else
					{
						UpdateStateLinkProperty(elementProperty, fieldInfo, elementType, elementValue);
					}

					s_StateLinkPropertyBuilder.Length = currentLength;
				}
			}
			else if (fieldType == typeof(StateLink))
			{
				AddStateLinkProperty(property, fieldInfo, value as StateLink);
			}
			else
			{
				if (property.propertyType == SerializedPropertyType.ManagedReference)
				{
					if(value == null)
					{
						return;
					}

					fieldType = property.GetTypeFromManagedReferenceFullTypeName();
					if (fieldType == null)
					{
						return;
					}
				}

				int currentLength = s_StateLinkPropertyBuilder.Length;

				foreach (Arbor.DynamicReflection.DynamicField dynamicField in EachField<StateLink>.GetFields(fieldType))
				{
					System.Reflection.FieldInfo fi = dynamicField.fieldInfo;

					SerializedProperty p = property.FindPropertyRelative(fi.Name);
					object elementValue = dynamicField.GetValue(value);

					s_StateLinkPropertyBuilder.Append("/");
					s_StateLinkPropertyBuilder.Append(p.displayName);

					UpdateStateLinkProperty(p, fi, fi.FieldType, elementValue);

					s_StateLinkPropertyBuilder.Length = currentLength;
				}
			}
		}

		private void UpdateStateLinkInternal()
		{
			using (new ProfilerScope("UpdateStateLink"))
			{
				if (editor == null)
				{
					return;
				}

				SerializedObject serializedObject = editor.serializedObject;

				Object targetObject = serializedObject.targetObject;
				if (targetObject == null) // Missing
				{
					return;
				}

				if (_StateLinkElements == null)
				{
					_StateLinkElements = new List<StateLinkElement>();
				}
				else
				{
					_StateLinkElements.Clear();
				}

				System.Type classType = targetObject.GetType();
				foreach (Arbor.DynamicReflection.DynamicField dynamicField in EachField<StateLink>.GetFields(classType))
				{
					System.Reflection.FieldInfo fieldInfo = dynamicField.fieldInfo;

					object value = dynamicField.GetValue(targetObject);

					SerializedProperty property = serializedObject.FindProperty(fieldInfo.Name);

					s_StateLinkPropertyBuilder.Length = 0;
					s_StateLinkPropertyBuilder.Append(property.displayName);

					UpdateStateLinkProperty(property, fieldInfo, fieldInfo.FieldType, value);
				}

				foreach (var pair in _OldStateLinkElements)
				{
					var stateLinkElement = pair.Value;
					stateLinkElement.RemoveFromHierarchy();
				}

				_OldStateLinkElements.Clear();

				int stateLinkCount = _StateLinkElements.Count;
				for (int stateLinkIndex = 0; stateLinkIndex < stateLinkCount; stateLinkIndex++)
				{
					StateLinkElement stateLinkElement = _StateLinkElements[stateLinkIndex];
					_OldStateLinkElements.Add(stateLinkElement.propertyKey, stateLinkElement);
				}

				UpdateStateLinkHeader();
			}
		}

		protected override void OnRepair()
		{
			base.OnRepair();

			UpdateStateLink();

			int stateLinkCount = _StateLinkElements.Count;
			for (int stateLinkIndex = 0; stateLinkIndex < stateLinkCount; stateLinkIndex++)
			{
				StateLinkElement stateLinkElement = _StateLinkElements[stateLinkIndex];
				stateLinkElement.Repair(behaviourObj as StateBehaviour);
			}
		}

		void UpdateReservedState()
		{
			if (_ReservedStateElement != null)
			{
				_ReservedStateElement.style.display = IsDrawReservedState() ? DisplayStyle.Flex : DisplayStyle.None;
			}
		}

		public void UpdateStateLink()
		{
			if (editor == null)
			{
				return;
			}

			SerializedObject serializedObject = editor.serializedObject;

			serializedObject.Update();

			UpdateStateLinkInternal();

			serializedObject.ApplyModifiedProperties();
		}

		internal void UpdateStateLinkTargetPosition(int targetNodeID)
		{
			for (int stateLinkIndex = 0; stateLinkIndex < _StateLinkElements.Count; stateLinkIndex++)
			{
				StateLinkElement stateLinkElement = _StateLinkElements[stateLinkIndex];
				if (stateLinkElement.stateLink.stateID == targetNodeID)
				{
					stateLinkElement.UpdateBezier();
				}
			}
		}

		protected override void OnDestroy()
		{
			for (int stateLinkIndex = 0; stateLinkIndex < _StateLinkElements.Count; stateLinkIndex++)
			{
				StateLinkElement stateLinkElement = _StateLinkElements[stateLinkIndex];
				stateLinkElement.RemoveFromHierarchy();
			}

			if (_StateLinkRootElement != null && _StateLinkRootElement.parent != null)
			{
				_StateLinkRootElement.RemoveFromHierarchy();
			}

			_StateLinkElements.Clear();
			_OldStateLinkElements.Clear();
		}

		protected override bool HasBehaviourEnable()
		{
			return true;
		}

		protected override bool GetBehaviourEnable()
		{
			StateBehaviour behaviour = behaviourObj as StateBehaviour;
			if (behaviour is object)
			{
				return behaviour.behaviourEnabled;
			}
			return false;
		}

		protected override void SetBehaviourEnable(bool enable)
		{
			StateBehaviour behaviour = behaviourObj as StateBehaviour;
			behaviour.behaviourEnabled = enable;
		}

		protected override void SetPopupMenu(GenericMenu menu)
		{
			bool editable = nodeEditor.graphEditor.editable;

			int behaviourCount = state.behaviourCount;

			if (behaviourIndex >= 1 && editable)
			{
				menu.AddItem(EditorContents.moveUp, false, MoveUpBehaviourContextMenu);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.moveUp);
			}

			if (behaviourIndex < behaviourCount - 1 && editable)
			{
				menu.AddItem(EditorContents.moveDown, false, MoveDownBehaviourContextMenu);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.moveDown);
			}

			base.SetPopupMenu(menu);

			if (editable)
			{
				menu.AddItem(EditorContents.delete, false, DeleteBehaviourContextMenu);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.delete);
			}
		}

		void MoveUpBehaviourContextMenu()
		{
			if (stateEditor != null)
			{
				stateEditor.MoveBehaviour(behaviourIndex, behaviourIndex - 1);
			}
		}

		void MoveDownBehaviourContextMenu()
		{
			if (stateEditor != null)
			{
				stateEditor.MoveBehaviour(behaviourIndex, behaviourIndex + 1);
			}
		}

		void DeleteBehaviourContextMenu()
		{
			if (stateEditor != null)
			{
				stateEditor.RemoveBehaviour(behaviourIndex);
			}
		}

		internal bool IsDraggingVisible()
		{
			var graphEditor = nodeEditor.graphEditor;

			for (int stateLinkIndex = 0; stateLinkIndex < _StateLinkElements.Count; stateLinkIndex++)
			{
				StateLinkElement stateLinkElement = _StateLinkElements[stateLinkIndex];
				Node targetNode = state.stateMachine.GetNodeFromID(stateLinkElement.stateLink.stateID);
				if (targetNode != null)
				{
					if (graphEditor.IsDraggingNode(targetNode))
					{
						return true;
					}
					StateLinkRerouteNodeEditor rerouteNodeEditor = graphEditor.GetNodeEditor(targetNode) as StateLinkRerouteNodeEditor;
					if (rerouteNodeEditor != null && rerouteNodeEditor.isDragDirection)
					{
						return true;
					}
				}
			}

			return false;
		}

		internal bool DeleteKeepConnection(int deleteNodeID, int nextStateID)
		{
			bool changed = false;

			for (int stateLinkIndex = 0; stateLinkIndex < _StateLinkElements.Count; stateLinkIndex++)
			{
				StateLinkElement stateLinkElement = _StateLinkElements[stateLinkIndex];
				StateLink stateLink = stateLinkElement.stateLink;
				if (stateLink.stateID == deleteNodeID)
				{
					Undo.RecordObject(behaviourObj, "Delete Keep Connection");

					stateLinkElement.SetLinkState(nextStateID);

					EditorUtility.SetDirty(behaviourObj);

					changed = true;
				}
			}

			return changed;
		}

		internal bool IsConnected(int nodeID)
		{
			for (int stateLinkIndex = 0; stateLinkIndex < _StateLinkElements.Count; stateLinkIndex++)
			{
				StateLinkElement stateLinkElement = _StateLinkElements[stateLinkIndex];
				if (stateLinkElement.stateLink.stateID == nodeID)
				{
					return true;
				}
			}
			return false;
		}

		internal bool IsLinkedRerouteNode(StateLinkRerouteNode rerouteNode)
		{
			ArborFSMInternal stateMachine = state.stateMachine;
			for (int stateLinkIndex = 0; stateLinkIndex < _StateLinkElements.Count; stateLinkIndex++)
			{
				StateLinkElement stateLinkElement = _StateLinkElements[stateLinkIndex];
				StateLinkRerouteNode currentRerouteNode = stateMachine.GetNodeFromID(stateLinkElement.stateLink.stateID) as StateLinkRerouteNode;
				while (currentRerouteNode != null)
				{
					if (currentRerouteNode == rerouteNode)
					{
						return true;
					}

					currentRerouteNode = stateMachine.GetNodeFromID(currentRerouteNode.link.stateID) as StateLinkRerouteNode;
				}
			}

			return false;
		}

		internal void ClearTransitionCount()
		{
			for (int stateLinkIndex = 0; stateLinkIndex < _StateLinkElements.Count; stateLinkIndex++)
			{
				StateLinkElement stateLinkElement = _StateLinkElements[stateLinkIndex];
				stateLinkElement.stateLink.transitionCount = 0;
			}
		}

		VisualElement _TopElement;
		VisualElement _BottomElement;
		VisualElement _StateLinkRootElement;
		VisualElement _StateLinkHeader;
		VisualElement _StateLinkContents;

		private bool _ShowStateLinkHeader = false;

		internal bool showStateLinkHeader
		{
			get
			{
				return _ShowStateLinkHeader;
			}
			set
			{
				if (_ShowStateLinkHeader != value)
				{
					_ShowStateLinkHeader = value;
					UpdateStateLinkHeader();
				}
			}
		}

		void UpdateStateLinkHeader()
		{
			if (_StateLinkHeader != null)
			{
				if (_ShowStateLinkHeader && _StateLinkElements != null && _StateLinkElements.Count != 0)
				{
					_StateLinkHeader.style.display = DisplayStyle.Flex;
				}
				else
				{
					_StateLinkHeader.style.display = DisplayStyle.None;
				}
			}
		}

		internal void ShowStateLinkElement(VisualElement parent, int index, bool showHeader)
		{
			if (parent != null)
			{
				showStateLinkHeader = showHeader;
				if (_StateLinkRootElement.parent != parent)
				{
					parent.Add(_StateLinkRootElement);
				}
				if (parent.hierarchy.IndexOf(_StateLinkRootElement) != index)
				{
					_StateLinkRootElement.PlaceBehind(parent.hierarchy[index]);
				}
			}
			else
			{
				if (_StateLinkRootElement != null && _StateLinkRootElement.parent != null)
				{
					_StateLinkRootElement.RemoveFromHierarchy();
				}
			}
		}

		internal void UpdateStateLinkShowMode()
		{
			switch (ArborSettings.stateLinkShowMode)
			{
				case StateLinkShowMode.BehaviourTop:
					{
						ShowStateLinkElement(_TopElement, 0, false);
					}
					break;
				case StateLinkShowMode.BehaviourBottom:
					{
						ShowStateLinkElement(_BottomElement, 0, false);
					}
					break;
				default:
					{
						ShowStateLinkElement(stateEditor._StateLinkListElement, behaviourIndex, true);
					}
					break;
			}
		}

		protected override VisualElement CreateTopElement()
		{
			_TopElement = new VisualElement();
			return _TopElement;
		}

		protected override VisualElement CreateBottomElement()
		{
			_BottomElement = new VisualElement();
			return _BottomElement;
		}

		private VisualElement _ReservedStateElement = null;

		protected override VisualElement CreateUnderlayElement()
		{
			_ReservedStateElement = new VisualElement()
			{
				pickingMode = PickingMode.Ignore,
				style =
				{
					backgroundColor = StateMachineGraphEditor.reservedColor,
					position = Position.Absolute,
					left = 0f,
					width = 5f,
					top = 0f,
					bottom = 0f,
					display = DisplayStyle.None,
				}
			};
			return _ReservedStateElement;
		}

		bool IsDrawReservedState()
		{
			if (!Application.isPlaying)
			{
				return false;
			}

			StateBehaviour stateBehaviour = behaviourObj as StateBehaviour;

			if (stateBehaviour == null || !stateBehaviour.behaviourEnabled)
			{
				return false;
			}

			ArborFSMInternal stateMachine = stateBehaviour.stateMachine;
			if (stateMachine == null ||
				stateMachine.playState == PlayState.Stopping ||
				stateMachine.currentState != state)
			{
				return false;
			}

			return !stateBehaviour.IsActive();
		}
	}
}