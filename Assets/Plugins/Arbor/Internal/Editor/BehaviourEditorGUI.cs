//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;
using System.Collections.Generic;

using Arbor;

namespace ArborEditor
{
	using ArborEditor.UIElements;
	using ArborEditor.UIElements.Pool;
	using ArborEditor.UnityEditorBridge;
	using ArborEditor.UnityEditorBridge.Extensions;
	using ArborEditor.UnityEditorBridge.UIElements.Extensions;

	[System.Serializable]
	public class BehaviourEditorGUI
	{
		private static Dictionary<int, BehaviourEditorGUI> s_BehaviourEditors = new Dictionary<int, BehaviourEditorGUI>();

		static void Registory(int instanceID, BehaviourEditorGUI editor)
		{
			s_BehaviourEditors[instanceID] = editor;
		}

		static void Unregistory(int instanceID)
		{
			s_BehaviourEditors.Remove(instanceID);
		}

		public static BehaviourEditorGUI Get(int instancceID)
		{
			if (s_BehaviourEditors.TryGetValue(instancceID, out var editor))
			{
				return editor;
			}

			return null;
		}

		private NodeEditor _NodeEditor;
		private Object _BehaviourObj;
		private bool _IsValidObject = false;
		private int _InstanceID;
		private Editor _Editor;
		private MonoScript _Script;

		public int behaviourIndex = 0;

		public NodeEditor nodeEditor
		{
			get
			{
				return _NodeEditor;
			}
		}

		public Object behaviourObj
		{
			get
			{
				return _BehaviourObj;
			}
		}

		public int behaviourInstanceID
		{
			get
			{
				return _InstanceID;
			}
		}

		public Editor editor
		{
			get
			{
				return _Editor;
			}
		}

		public enum MarginType
		{
			Editor,
			ForceDefault,
			ForceFull,
		}

		public MarginType marginType = MarginType.Editor;

		public bool expanded = true;

		private Color? _BackgroundColor = null;

		public Color? backgroundColor
		{
			get
			{
				return _BackgroundColor;
			}
			set
			{
				if (_BackgroundColor != value)
				{
					_BackgroundColor = value;

					if (_HeaderElement != null)
					{
						_HeaderElement.backgroundColor = _BackgroundColor;
					}
				}
			}
		}

		void CreateEditor()
		{
			if (_BehaviourObj is NodeBehaviour)
			{
				_Editor = NodeBehaviourEditor.CreateEditor(_NodeEditor, _BehaviourObj);
			}
		}

		void SetBehaviourObject(Object behaviourObj)
		{
			if (_InspectorGUIElement != null)
			{
				_InspectorGUIElement.RemoveFromHierarchy();
				_InspectorGUIElement = null;
			}

			if (_InstanceID != 0)
			{
				Unregistory(_InstanceID);
				_InstanceID = 0;
			}

			DestroyEditor();

			_BehaviourObj = behaviourObj;
			_IsValidObject = ComponentUtility.IsValidObject(_BehaviourObj);

			CreateEditor();

			_Script = EditorGUITools.GetMonoScript(_BehaviourObj);

			if (_BehaviourObj != null)
			{
				_InstanceID = _BehaviourObj.GetInstanceID();
				Registory(_InstanceID, this);
			}

			CreateInspectorGUIElement();

			OnChangedObject();
		}

		protected virtual void OnChangedObject()
		{
		}

		public void Initialize(NodeEditor nodeEditor, Object behaviourObj)
		{
			_NodeEditor = nodeEditor;

			SetBehaviourObject(behaviourObj);

			OnInitialize();

			DoEnable();
		}

		protected virtual void OnInitialize()
		{
		}

		public void Repair(Object behaviourObj)
		{
			if (ReferenceEquals(_BehaviourObj, behaviourObj) && ComponentUtility.IsValidObject(behaviourObj) == _IsValidObject)
			{
				return;
			}

			SetBehaviourObject(behaviourObj);

			_HeaderElement?.OnRepair();

			OnRepair();
		}

		public void RepairEditor()
		{
			var currentEditorType = _Editor.GetType();
			var newEditorType = CustomEditorAttributesBridge.FindEditorType(_BehaviourObj, false);
			
			if (currentEditorType != newEditorType)
			{
				if (_InspectorGUIElement != null)
				{
					_InspectorGUIElement.RemoveFromHierarchy();
					_InspectorGUIElement = null;
				}

				DestroyEditor();

				CreateEditor();

				CreateInspectorGUIElement();
			}
		}

		protected virtual void OnRepair()
		{
		}

		protected virtual void OnDestroy()
		{
		}

		public void Destroy()
		{
			OnDestroy();

			if (element != null)
			{
				element.RemoveFromHierarchy();
				element = null;
			}

			_OverlayLayer?.RemoveFromHierarchy();

			ClearInputSlotLink();

			DestroyEditor();
		}

		void DestroyEditor()
		{
			if (_Editor != null)
			{
				Object.DestroyImmediate(_Editor);
				_Editor = null;
			}

			if (_InstanceID != 0)
			{
				Unregistory(_InstanceID);
				_InstanceID = 0;
			}
		}

		public bool GetExpanded()
		{
			NodeBehaviour nodeBehaviour = _BehaviourObj as NodeBehaviour;
			return (nodeBehaviour != null) ? BehaviourEditorUtility.GetExpanded(nodeBehaviour, nodeBehaviour.expanded) : this.expanded;
		}

		protected virtual void OnChangedExpanded()
		{
		}

		internal void SetExpandedInternal(bool expanded)
		{
			_InspectorElement.style.display = expanded ? DisplayStyle.Flex : DisplayStyle.None;

			NodeBehaviour nodeBehaviour = _BehaviourObj as NodeBehaviour;
			if (nodeBehaviour != null)
			{
				if ((nodeBehaviour.hideFlags & HideFlags.NotEditable) != HideFlags.NotEditable)
				{
					nodeBehaviour.expanded = expanded;
					EditorUtility.SetDirty(nodeBehaviour);
				}
				BehaviourEditorUtility.SetExpanded(nodeBehaviour, expanded);
			}
			else
			{
				this.expanded = expanded;
			}

			OnChangedExpanded();
		}

		public void SetExpanded(bool expanded)
		{
			SetExpandedInternal(expanded);

			_HeaderElement.SetFoldout(expanded);
		}

		protected virtual bool HasBehaviourEnable()
		{
			return false;
		}

		internal bool HasBehaviourEnable_Internal()
		{
			return HasBehaviourEnable();
		}

		protected virtual bool GetBehaviourEnable()
		{
			return false;
		}

		internal bool GetBehaviourEnable_Internal()
		{
			return GetBehaviourEnable();
		}

		protected virtual void SetBehaviourEnable(bool enable)
		{
		}

		internal void SetBehaviourEnable_Internal(bool enable)
		{
			SetBehaviourEnable(enable);
		}

		void CopyBehaviourContextMenu()
		{
			NodeBehaviour behaviour = behaviourObj as NodeBehaviour;

			Clipboard.CopyBehaviour(behaviour);
		}

		void PasteBehaviourContextMenu()
		{
			NodeBehaviour behaviour = behaviourObj as NodeBehaviour;

			Undo.IncrementCurrentGroup();

			Undo.RecordObject(behaviour, "Paste Behaviour");

			Clipboard.PasteBehaviourValues(behaviour);

			Undo.CollapseUndoOperations(Undo.GetCurrentGroup());

			EditorUtility.SetDirty(behaviour);
		}

		protected virtual void SetPopupMenu(GenericMenu menu)
		{
			bool editable = nodeEditor.graphEditor.editable;

			NodeBehaviour behaviour = behaviourObj as NodeBehaviour;
			if (behaviour != null)
			{
				menu.AddItem(EditorContents.copy, false, CopyBehaviourContextMenu);
				if (Clipboard.CompareBehaviourType(behaviourObj.GetType(), false) && editable)
				{
					menu.AddItem(EditorContents.pasteValues, false, PasteBehaviourContextMenu);
				}
				else
				{
					menu.AddDisabledItem(EditorContents.pasteValues);
				}
			}
		}

		static void EditScriptBehaviourContextMenu(object obj)
		{
			MonoScript script = obj as MonoScript;

			AssetDatabase.OpenAsset(script);
		}

		static void HighlightScriptBehaviourContextMenu(object obj)
		{
			MonoScript script = obj as MonoScript;

			EditorGUIUtility.PingObject(script);
		}

		public void SetContextMenu(GenericMenu menu)
		{
			int menuItemCount = menu.GetItemCount();

			SetPopupMenu(menu);

			if (menu.GetItemCount() > menuItemCount)
			{
				menu.AddSeparator("");
			}

			if (_Script != null)
			{
				menu.AddItem(EditorContents.editScript, false, EditScriptBehaviourContextMenu, _Script);
				menu.AddItem(EditorContents.highlighScript, false, HighlightScriptBehaviourContextMenu, _Script);
			}
			else
			{
				menu.AddDisabledItem(EditorContents.editScript);
				menu.AddDisabledItem(EditorContents.highlighScript);
			}

			MonoScript editorScript = EditorGUITools.GetMonoScript(_Editor);

			if (editorScript != null && (editorScript.hideFlags & HideFlags.NotEditable) != HideFlags.NotEditable)
			{
				var editorScriptType = editorScript.GetClass();
				if (editorScriptType != null
#if !ARBOR_DISABLE_DEFAULT_EDITOR
					&& editorScriptType != typeof(NodeBehaviourDefaultEditor)
#endif
					)
				{
					menu.AddItem(EditorContents.editEditorScript, false, EditScriptBehaviourContextMenu, editorScript);
					menu.AddItem(EditorContents.highlightEditorScript, false, HighlightScriptBehaviourContextMenu, editorScript);
				}
			}

			BehaviourMenuItemUtility.AddContextMenu(menu, _BehaviourObj);
		}

		internal bool IsEditorEnabled()
		{
			return _Editor != null && _Editor.IsEnabled() && !(nodeEditor.graphEditor != null && nodeEditor.graphEditor.isExternalGraph);
		}

		private Dictionary<SerializedPropertyKey, InputSlotLinker> _InputSlotLinkers = new Dictionary<SerializedPropertyKey, InputSlotLinker>();
		private Dictionary<SerializedPropertyKey, InputSlotLinker> _NextInputSlotLinkers = new Dictionary<SerializedPropertyKey, InputSlotLinker>();

		void ClearInputSlotLink()
		{
			foreach (var pair in _InputSlotLinkers)
			{
				var linker = pair.Value;
				linker.RemoveFromHierarchy();
			}
			_InputSlotLinkers.Clear();
		}

		void UpdateInputSlotLink()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}

			foreach (var pair in _InputSlotLinkers)
			{
				var linker = pair.Value;
				VisualElementPool<InputSlotLinker>.Release(linker);
			}
			_InputSlotLinkers.Clear();

			var oldLinkers = _InputSlotLinkers;
			_InputSlotLinkers = _NextInputSlotLinkers;
			_NextInputSlotLinkers = oldLinkers;

			_NextInputSlotLinkers.Clear();
		}

		sealed class InputSlotLinker : VisualElement
		{
			public readonly InputSlotElement slotElement;

			public InputSlotLinker()
			{
				style.position = Position.Absolute;

				slotElement = new InputSlotElement()
				{
					style =
					{
						position = Position.Absolute,
						right = 0f,
					}
				};
				Add(slotElement);
			}
		}

		internal DataSlotGUI GetDataSlotGUI(SerializedProperty property)
		{
			return DataSlotGUI.GetGUI(this, property);
		}

		public void SetInputSlotLink(Rect position, SerializedProperty property)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}

			SerializedPropertyKey key = new SerializedPropertyKey(property);

			if (!_NextInputSlotLinkers.TryGetValue(key, out var linker))
			{
				if (!_InputSlotLinkers.TryGetValue(key, out linker))
				{
					linker = VisualElementPool<InputSlotLinker>.Get();
					
					linker.slotElement.Set(this);
					overlayLayer.Add(linker);
				}
				else
				{
					_InputSlotLinkers.Remove(key);
				}

				_NextInputSlotLinkers.Add(key, linker);
			}

			var matrix = GUI.matrix;
			GUI.matrix = Matrix4x4.identity;

			position = overlayLayer.GUIToLocal(position);
			position.x = 6f;
			position.width = 0f;
			position.height = 0f;

			GUI.matrix = matrix;

			linker.style.left = 6f;
			linker.style.top = position.y;
			
			var slotElement = linker.slotElement;

			slotElement.SetEnabled(GUI.enabled);

			DataSlotGUI slotGUI = GetDataSlotGUI(property);
			slotElement.slotGUI = slotGUI;

			slotElement.UpdateOn();

			if (slotGUI != null)
			{
				DataSlot slot = slotGUI.slot;
				if (slot != null)
				{
					slot.enabledGUI = GUI.enabled;

					slotGUI.UpdateHighlight();

					slot.SetVisible();
				}
			}

			slotElement.DoChangedPosition();
		}

		GUIStyle GetMarginStyle()
		{
			switch (marginType)
			{
				case MarginType.Editor:
					return (editor == null || editor.UseDefaultMargins()) ? EditorStyles.inspectorDefaultMargins : EditorStyles.inspectorFullWidthMargins;
				case MarginType.ForceDefault:
					return EditorStyles.inspectorDefaultMargins;
				case MarginType.ForceFull:
					return EditorStyles.inspectorFullWidthMargins;
			}

			return EditorStyles.inspectorDefaultMargins;
		}

		internal VisualElement element
		{
			get;
			private set;
		}

		private VisualElement _OverlayLayer;
		public VisualElement overlayLayer
		{
			get
			{
				if (_OverlayLayer == null)
				{
					_OverlayLayer = new VisualElement()
					{
						pickingMode = PickingMode.Ignore,
					};
					_OverlayLayer.StretchToParentSize();
				}
				return _OverlayLayer;
			}
		}

		public Rect GetHeaderPosition()
		{
			GraphView graphView = (_NodeEditor.graphEditor != null) ? _NodeEditor.graphEditor.graphView : null;
			if (graphView != null)
			{
				return graphView.ElementToGraph(_HeaderElement.parent, _HeaderElement.layout);
			}
			return _HeaderElement.parent.ChangeCoordinatesTo(_NodeEditor.nodeElement, _HeaderElement.layout);
		}

		private VisualElement _InspectorElement;
		private VisualElement _InspectorGUIElement;
		private VisualElement _MissingGUIElement;

		static class Defaults
		{
			public readonly static StyleSheet inspectorWindowStyleSheet;
			public const float kEditorElementPaddingBottom = 2f; // InspectorWindow.kEditorElementPaddingBottom

			static Defaults()
			{
				inspectorWindowStyleSheet = EditorGUIUtility.Load("StyleSheets/InspectorWindow/InspectorWindow.uss") as StyleSheet;
			}
		}

		void CreateInspectorGUIElement()
		{
			if (_InspectorElement != null)
			{
				if (_Editor != null)
				{
					_MissingGUIElement?.RemoveFromHierarchy();
					if (_InspectorGUIElement == null)
					{
						string editorTitle = ObjectNames.GetInspectorTitle(_Editor.targets[0]);

						_InspectorGUIElement = new InspectorElement(_Editor)
						{
							focusable = false,
							name = editorTitle + "Inspector",
							style =
							{
								paddingBottom = Defaults.kEditorElementPaddingBottom,
							}
						};

						_InspectorGUIElement.styleSheets.Add(Defaults.inspectorWindowStyleSheet);

						IMGUIContainer imguiContainer = _InspectorGUIElement.Q<IMGUIContainer>(className: InspectorElement.iMGUIContainerUssClassName);
						if(imguiContainer != null)
						{
							System.Action onGUIHandler = imguiContainer.onGUIHandler;
							System.Action newOnGUI = () =>
							{
								var editorTarget = _Editor.target;
								bool isValidObject = ComponentUtility.IsValidObject(editorTarget);
								HideFlags hideFlags = 0;
								if (isValidObject)
								{
									hideFlags = editorTarget.hideFlags;
									if (!imguiContainer.enabledInHierarchy)
									{
										editorTarget.hideFlags |= HideFlags.NotEditable;
									}
								}

								var savedMatrix = GUI.matrix;

								if (RenderTexture.active != null)
								{
									float scaling = 1f / EditorGUIUtility.pixelsPerPoint;
									Vector2 min = imguiContainer.worldBound.min;
									Vector2 pos = -(min - min * scaling);
									GUI.matrix = Matrix4x4.TRS(pos, Quaternion.identity, Vector3.one * scaling);
								}

								try
								{
									GUI.BeginGroup(imguiContainer.contentRect);

									onGUIHandler?.Invoke();

									GUI.EndGroup();
								}
								finally
								{
									GUI.matrix = savedMatrix;
									if (isValidObject && ComponentUtility.IsValidObject(editorTarget))
									{
										editorTarget.hideFlags = hideFlags;
									}
								}
							};

							imguiContainer.onGUIHandler = newOnGUI;
						}
					}
					_InspectorElement.Add(_InspectorGUIElement);
				}
				else
				{
					_InspectorGUIElement?.RemoveFromHierarchy();
					if (_MissingGUIElement == null)
					{
						_MissingGUIElement = new NodeContentIMGUIContainer(OnMissingGUI);
					}
					_InspectorElement.Add(_MissingGUIElement);
				}
			}
		}

		protected virtual VisualElement CreateUnderlayElement()
		{
			return null;
		}

		protected virtual VisualElement CreateTopElement()
		{
			return null;
		}

		protected virtual VisualElement CreateBottomElement()
		{
			return null;
		}

		protected virtual void OnCreatedElement()
		{
		}

		private BehaviourTitlebarElement _HeaderElement;

		public VisualElement CreateElement()
		{
			if (element == null)
			{
				element = new BehaviourElement(this);

				_HeaderElement = new BehaviourTitlebarElement(this);
				_HeaderElement.backgroundColor = backgroundColor;
				element.Add(_HeaderElement);

				VisualElement underlayElement = CreateUnderlayElement();
				if (underlayElement != null)
				{
					element.Add(underlayElement);
				}

				BehaviourInfo behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(_BehaviourObj);
				if (behaviourInfo.obsolete != null)
				{
					element.Add(new ObsoleteHelpBoxElement(behaviourInfo.obsolete));
				}

				VisualElement topElement = CreateTopElement();
				if (topElement != null)
				{
					element.Add(topElement);
				}

				_InspectorElement = new VisualElement();
				element.Add(_InspectorElement);

				_InspectorElement.style.display = GetExpanded() ? DisplayStyle.Flex : DisplayStyle.None;
				
				CreateInspectorGUIElement();

				VisualElement bottomElement = CreateBottomElement();
				if (bottomElement != null)
				{
					element.Add(bottomElement);
				}

				var eventContainer = new IMGUIContainer(OnEventGUI)
				{
					pickingMode = PickingMode.Ignore,
					style =
					{
						width = 0f,
						height = 0f,
					}
				};
				element.Add(eventContainer);

				OnCreatedElement();
			}

			return element;
		}

		internal void DoEnable()
		{
			if (_BehaviourObj != null)
			{
				int instanceID = _BehaviourObj.GetInstanceID();
				if (_InstanceID != 0 && _InstanceID != instanceID)
				{
					Unregistory(_InstanceID);
				}
				_InstanceID = instanceID;
				Registory(_InstanceID, this);
			}
			else if (_InstanceID != 0)
			{
				Unregistory(_InstanceID);
				_InstanceID = 0;
			}

			OnEnable();
		}

		protected virtual void OnEnable()
		{
		}

		internal void DoDisable()
		{
			OnDisable();
		}

		protected virtual void OnDisable()
		{
		}

		internal void Update()
		{
			_HeaderElement?.OnUpdate();

			if (_Editor != null && _InspectorGUIElement != null)
			{
				_InspectorGUIElement.SetEnabled(IsEditorEnabled());
			}

			var graphEditor = (nodeEditor != null) ? nodeEditor.graphEditor : null;
			if (graphEditor != null)
			{
				if (_Editor != null && _Editor.RequiresConstantRepaint())
				{
					graphEditor.wantsRepaint = true;
				}
			}
		}

		void OnMissingGUI()
		{
			GUIStyle marginStyle = GetMarginStyle();

			EditorGUILayout.BeginVertical(marginStyle);

			if (behaviourObj != null)
			{
				if (_Script != null)
				{
					EditorGUI.BeginDisabledGroup(true);
					EditorGUILayout.ObjectField("Script", _Script, typeof(MonoScript), false);
					EditorGUI.EndDisabledGroup();
				}
			}

			EditorGUILayout.HelpBox(Localization.GetWord("MissingError"), MessageType.Error);

			EditorGUILayout.EndVertical();
		}

		void UpdateVisibleDataSlot()
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}

			NodeBehaviour behaviour = _BehaviourObj as NodeBehaviour;
			if (behaviour == null)
			{
				return;
			}

			for (int slotCount = behaviour.dataSlotCount, slotIndex = 0; slotIndex < slotCount; slotIndex++)
			{
				DataSlot slot = behaviour.GetDataSlot(slotIndex);
				if (slot == null)
				{
					continue;
				}

				bool oldVisible = slot.isVisible;
				slot.ClearVisible();
				if (oldVisible != slot.isVisible)
				{
					//changed = true;
				}
			}
		}

		internal void OnEventGUI()
		{
			UpdateInputSlotLink();
		}

		internal void OnRepainted()
		{
			UpdateVisibleDataSlot();
		}

		internal sealed class BehaviourElement : VisualElement
		{
			public readonly BehaviourEditorGUI editorGUI;

			public BehaviourElement(BehaviourEditorGUI editorGUI)
			{
				this.editorGUI = editorGUI;
			}
		}

		internal sealed class ObsoleteHelpBoxElement : VisualElement
		{
			private System.ObsoleteAttribute _Obsolete;
			public System.ObsoleteAttribute obsolete
			{
				get
				{
					return _Obsolete;
				}
				set
				{
					if (_Obsolete != value)
					{
						_Obsolete = value;
						MarkDirtyRepaint();
					}
				}
			}

			public ObsoleteHelpBoxElement(System.ObsoleteAttribute obsolete)
			{
				_Obsolete = obsolete;
				Add(new NodeContentIMGUIContainer(OnGUI));

				RegisterCallback<GeometryChangedEvent>(OnGeometryChanged);
			}

			void OnGeometryChanged(GeometryChangedEvent e)
			{
				GUIStyle guiStyle = EditorStyles.helpBox;

				style.marginTop = guiStyle.margin.top;
				style.marginBottom = guiStyle.margin.bottom;
				style.marginLeft = guiStyle.margin.left;
				style.marginRight = guiStyle.margin.right;
			}

			void OnGUI()
			{
				if (_Obsolete == null)
				{
					return;
				}

				EditorGUILayout.HelpBox(_Obsolete.Message, _Obsolete.IsError ? MessageType.Error : MessageType.Warning);
			}
		}
	}
}