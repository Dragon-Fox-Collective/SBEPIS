//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace ArborEditor
{
	using Arbor;

	internal sealed class GraphArgumentProperty
	{
		private const string kParameterIDPath = "_ParameterID";
		private const string kParameterNamePath = "_ParameterName";
		private const string kParameterTypePath = "_ParameterType";
		private const string kTypePath = "_Type";
		private const string kParameterIndexPath = "_ParameterIndex";
		private const string kOutputSlotIndexPath = "_OutputSlotIndex";
		private const string kUpdateTimingPath = "_UpdateTiming";
		private const string kIsPublicSetPath = "_IsPublicSet";
		private const string kIsPublicGetPath = "_IsPublicGet";

		private SerializedProperty _ParameterID;
		private SerializedProperty _ParameterName;
		private SerializedProperty _ParameterType;
		private ClassTypeReferenceProperty _Type;
		private SerializedProperty _ParameterIndex;
		private SerializedProperty _OutputSlotIndex;
		private SerializedProperty _UpdateTiming;
		private SerializedProperty _IsPublicSet;
		private SerializedProperty _IsPublicGet;

		public SerializedProperty property
		{
			get;
			private set;
		}

		private SerializedProperty parameterIDProperty
		{
			get
			{
				if (_ParameterID == null)
				{
					_ParameterID = property.FindPropertyRelative(kParameterIDPath);
				}
				return _ParameterID;
			}
		}

		public int parameterID
		{
			get
			{
				return parameterIDProperty.intValue;
			}
			set
			{
				parameterIDProperty.intValue = value;
			}
		}

		public SerializedProperty parameterNameProperty
		{
			get
			{
				if (_ParameterName == null)
				{
					_ParameterName = property.FindPropertyRelative(kParameterNamePath);
				}
				return _ParameterName;
			}
		}

		public string parameterName
		{
			get
			{
				return parameterNameProperty.stringValue;
			}
			set
			{
				parameterNameProperty.stringValue = value;
			}
		}

		private SerializedProperty parameterTypeProperty
		{
			get
			{
				if (_ParameterType == null)
				{
					_ParameterType = property.FindPropertyRelative(kParameterTypePath);
				}
				return _ParameterType;
			}
		}

		public Parameter.Type parameterType
		{
			get
			{
				return EnumUtility.GetValueFromIndex<Parameter.Type>(parameterTypeProperty.enumValueIndex);
			}
			set
			{
				parameterTypeProperty.enumValueIndex = EnumUtility.GetIndexFromValue<Parameter.Type>(value);
			}
		}

		private ClassTypeReferenceProperty typeProperty
		{
			get
			{
				if (_Type == null)
				{
					_Type = new ClassTypeReferenceProperty(property.FindPropertyRelative(kTypePath));
				}
				return _Type;
			}
		}

		public System.Type type
		{
			get
			{
				return typeProperty.type;
			}
			set
			{
				typeProperty.type = value;
			}
		}

		private SerializedProperty parameterIndexProperty
		{
			get
			{
				if (_ParameterIndex == null)
				{
					_ParameterIndex = property.FindPropertyRelative(kParameterIndexPath);
				}
				return _ParameterIndex;
			}
		}

		public int parameterIndex
		{
			get
			{
				return parameterIndexProperty.intValue;
			}
			set
			{
				parameterIndexProperty.intValue = value;
			}
		}

		private SerializedProperty outputSlotIndexProperty
		{
			get
			{
				if (_OutputSlotIndex == null)
				{
					_OutputSlotIndex = property.FindPropertyRelative(kOutputSlotIndexPath);
				}
				return _OutputSlotIndex;
			}
		}

		public int outputSlotIndex
		{
			get
			{
				return outputSlotIndexProperty.intValue;
			}
			set
			{
				outputSlotIndexProperty.intValue = value;
			}
		}

		public SerializedProperty updateTimingProperty
		{
			get
			{
				if (_UpdateTiming == null)
				{
					_UpdateTiming = property.FindPropertyRelative(kUpdateTimingPath);
				}
				return _UpdateTiming;
			}
		}

		public GraphArgumentUpdateTiming updateTiming
		{
			get
			{
				return (GraphArgumentUpdateTiming)updateTimingProperty.intValue;
			}
			set
			{
				updateTimingProperty.intValue = (int)value;
			}
		}

		private SerializedProperty isPublicSetProperty
		{
			get
			{
				if (_IsPublicSet == null)
				{
					_IsPublicSet = property.FindPropertyRelative(kIsPublicSetPath);
				}
				return _IsPublicSet;
			}
		}

		public bool isPublicSet
		{
			get
			{
				return isPublicSetProperty.boolValue;
			}
			set
			{
				isPublicSetProperty.boolValue = value;
			}
		}

		private SerializedProperty isPublicGetProperty
		{
			get
			{
				if (_IsPublicGet == null)
				{
					_IsPublicGet = property.FindPropertyRelative(kIsPublicGetPath);
				}
				return _IsPublicGet;
			}
		}

		public bool isPublicGet
		{
			get
			{
				return isPublicGetProperty.boolValue;
			}
			set
			{
				isPublicGetProperty.boolValue = value;
			}
		}

		public GraphArgumentProperty(SerializedProperty property)
		{
			this.property = property;
		}
	}

	internal static class ArgumentGUITools
	{
		private const string kVariablePopupChangedMessage = "VariablePopupChanged";

		private static readonly int s_VariableTypePopupHash = "s_VariableTypePopupHash".GetHashCode();

		private static int _ControlID;
		private static System.Type _Selected;

		internal static System.Type GetSelectedValueForControl(int controlID, System.Type selected)
		{
			Event current = Event.current;
			if (current.type == EventType.ExecuteCommand && current.commandName == kVariablePopupChangedMessage)
			{
				if (_ControlID == controlID)
				{
					if (selected != _Selected)
					{
						selected = _Selected;
						GUI.changed = true;
					}
					current.Use();
				}
			}
			return selected;
		}

		internal static System.Type VariableTypePopup(Rect position, System.Type selected, GUIContent label)
		{
			int controlID = GUIUtility.GetControlID(s_VariableTypePopupHash, FocusType.Passive, position);

			position = EditorGUI.PrefixLabel(position, controlID, label);

			selected = GetSelectedValueForControl(controlID, selected);

			Event current = Event.current;

			EventType eventType = current.GetTypeForControl(controlID);

			System.Type selectedVariableType = VariableEditorUtility.GetVariableTypeFromDataType(selected);

			string typeName = "None";
			if (selectedVariableType != null)
			{
				BehaviourInfo behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(selectedVariableType);
				if (behaviourInfo != null)
				{
					typeName = behaviourInfo.titleContent.text;
				}
				else
				{
					typeName = VariableEditorUtility.GetVariableName(selectedVariableType);
				}
			}
			GUIContent content = new GUIContent(typeName);
			GUIStyle style = EditorStyles.popup;

			switch (eventType)
			{
				case EventType.MouseDown:
					if (current.button == 0 && position.Contains(current.mousePosition))
					{
						GenericMenu genericMenu = new GenericMenu();
						EditorWindow sourceView = EditorWindow.focusedWindow;

						_ControlID = controlID;
						_Selected = selected;

						VariableEditorUtility.GenerateVariableMenus(genericMenu, selected, false, (type) =>
						{
							_Selected = VariableBase.GetDataType(type);
							sourceView.SendEvent(EditorGUIUtility.CommandEvent(kVariablePopupChangedMessage));
						});

						genericMenu.DropDown(position);
						current.Use();
					}
					break;
				case EventType.Repaint:
					style.Draw(position, content, controlID);
					break;
			}

			return selected;
		}

		internal static System.Type VariableListTypePopup(Rect position, System.Type selected, GUIContent label)
		{
			int controlID = GUIUtility.GetControlID(s_VariableTypePopupHash, FocusType.Passive, position);

			position = EditorGUI.PrefixLabel(position, controlID, label);

			selected = GetSelectedValueForControl(controlID, selected);

			Event current = Event.current;

			EventType eventType = current.GetTypeForControl(controlID);

			System.Type selectedVariableType = VariableEditorUtility.GetVariableListTypeFromDataType(selected);

			string typeName = "None";
			if (selectedVariableType != null)
			{
				BehaviourInfo behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(selectedVariableType);
				if (behaviourInfo != null)
				{
					typeName = behaviourInfo.titleContent.text;
				}
				else
				{
					typeName = VariableEditorUtility.GetVariableListName(selectedVariableType);
				}
			}
			GUIContent content = new GUIContent(typeName);
			GUIStyle style = EditorStyles.popup;

			switch (eventType)
			{
				case EventType.MouseDown:
					if (current.button == 0 && position.Contains(current.mousePosition))
					{
						GenericMenu genericMenu = new GenericMenu();
						EditorWindow sourceView = EditorWindow.focusedWindow;

						_ControlID = controlID;
						_Selected = selected;

						VariableEditorUtility.GenerateVariableListMenus(genericMenu, selected, false, (type) =>
						{
							_Selected = VariableListBase.GetDataType(type);
							sourceView.SendEvent(EditorGUIUtility.CommandEvent(kVariablePopupChangedMessage));
						});

						genericMenu.DropDown(position);
						current.Use();
					}
					break;
				case EventType.Repaint:
					style.Draw(position, content, controlID);
					break;
			}

			return selected;
		}
	}

	abstract class ExternalArgumentPopupBase : PopupWindowContent
	{
		protected GraphArgumentListEditor _ArgumentListEditor;
		private EditorWindow _FocusedWindow;

		private LayoutArea _LayoutArea = new LayoutArea();

		protected string _ParameterName;
		protected Parameter.Type _ParameterType;
		protected System.Type _ReferenceType;
		protected bool _IsPublicSet = true;
		protected bool _IsPublicGet = true;

		private GUIContent _ButtonContent;

		public void Show(Rect buttonRect, GraphArgumentListEditor argumentListEditor, GUIContent buttonContent)
		{
			_ArgumentListEditor = argumentListEditor;
			_FocusedWindow = EditorWindow.focusedWindow;

			_ButtonContent = buttonContent;

			PopupWindowUtility.Show(buttonRect, this, true);
		}

		protected virtual void DoGUI()
		{
			_ParameterName = _LayoutArea.TextField(GUIContentCaches.Get("Parameter Name"), _ParameterName);

			Rect popupRect = _LayoutArea.GetRect(0, EditorGUIUtility.singleLineHeight);

			if (_LayoutArea.IsDraw())
			{
				EditorGUI.BeginChangeCheck();
				Parameter.Type parameterType = ParameterTypeMenuItem.Popup(popupRect, GUIContentCaches.Get("Parameter Type"), _ParameterType);
				if (EditorGUI.EndChangeCheck() && parameterType != _ParameterType)
				{
					_ParameterType = parameterType;
					_ReferenceType = null;
				}
			}

			_LayoutArea.BeginHorizontal();

			_IsPublicSet = _LayoutArea.VisibilityToggle(GUIContentCaches.Get("Set"), _IsPublicSet, LayoutArea.Width(50f));
			_IsPublicGet = _LayoutArea.VisibilityToggle(GUIContentCaches.Get("Get"), _IsPublicGet, LayoutArea.Width(50f));

			_LayoutArea.EndHorizontal();

			switch (_ParameterType)
			{
				case Parameter.Type.Component:
				case Parameter.Type.ComponentList:
					{
						Rect lineRect = _LayoutArea.GetRect(0f, EditorGUIUtility.singleLineHeight);

						if (_LayoutArea.IsDraw())
						{
							ClassTypeConstraintFilter filter = new ClassTypeConstraintFilter(ClassTypeConstraintEditorUtility.component, null);

							_ReferenceType = TypeSelector.PopupField(lineRect, _ReferenceType, GUIContentCaches.Get("Reference Type"), filter);
						}
					}
					break;
				case Parameter.Type.AssetObject:
				case Parameter.Type.AssetObjectList:
					{
						Rect lineRect = _LayoutArea.GetRect(0f, EditorGUIUtility.singleLineHeight);

						if (_LayoutArea.IsDraw())
						{
							ClassTypeConstraintFilter filter = new ClassTypeConstraintFilter(ClassTypeConstraintEditorUtility.asset, null);

							_ReferenceType = TypeSelector.PopupField(lineRect, _ReferenceType, GUIContentCaches.Get("Reference Type"), filter);
						}
					}
					break;
				case Parameter.Type.Enum:
				case Parameter.Type.EnumList:
					{
						Rect lineRect = _LayoutArea.GetRect(0f, EditorGUIUtility.singleLineHeight);

						if (_LayoutArea.IsDraw())
						{
							ClassTypeConstraintFilter filter = new ClassTypeConstraintFilter(ClassTypeConstraintEditorUtility.enumField, null);

							_ReferenceType = TypeSelector.PopupField(lineRect, _ReferenceType, GUIContentCaches.Get("Reference Type"), filter);
						}
					}
					break;
				case Parameter.Type.Variable:
					{
						Rect lineRect = _LayoutArea.GetRect(0f, EditorGUIUtility.singleLineHeight);

						if (_LayoutArea.IsDraw())
						{
							_ReferenceType = ArgumentGUITools.VariableTypePopup(lineRect, _ReferenceType, GUIContentCaches.Get("Reference Type"));
						}
					}
					break;
				case Parameter.Type.VariableList:
					{
						Rect lineRect = _LayoutArea.GetRect(0f, EditorGUIUtility.singleLineHeight);

						if (_LayoutArea.IsDraw())
						{
							_ReferenceType = ArgumentGUITools.VariableListTypePopup(lineRect, _ReferenceType, GUIContentCaches.Get("Reference Type"));
						}
					}
					break;
			}

			bool disableButton = string.IsNullOrEmpty(_ParameterName) || (!_IsPublicSet && !_IsPublicGet) || (_ParameterType == Parameter.Type.EnumList && _ReferenceType == null) || IsDisableButton();

			EditorGUI.BeginDisabledGroup(disableButton);
			if (_LayoutArea.Button(_ButtonContent))
			{
				OnClickButton();

				if (_FocusedWindow != null)
				{
					_FocusedWindow.Repaint();
				}
			}
			EditorGUI.EndDisabledGroup();
		}

		protected virtual bool IsDisableButton()
		{
			return false;
		}

		protected abstract void OnClickButton();

		public override sealed void OnGUI(Rect rect)
		{
			_LayoutArea.Begin(rect, false);

			DoGUI();

			_LayoutArea.End();
		}

		public override sealed Vector2 GetWindowSize()
		{
			Rect rect = new Rect(0, 0, 300, 0);

			_LayoutArea.Begin(rect, true);

			DoGUI();

			_LayoutArea.End();

			return _LayoutArea.rect.size;
		}
	}

	sealed class CreateExternalArgumentPopup : ExternalArgumentPopupBase
	{
		private static CreateExternalArgumentPopup s_Instance = null;

		public static void Open(Rect buttonRect, GraphArgumentListEditor argumentListEditor)
		{
			if (s_Instance == null)
			{
				s_Instance = new CreateExternalArgumentPopup();
			}

			s_Instance.Show(buttonRect, argumentListEditor, EditorContents.create);
		}

		protected sealed override void OnClickButton()
		{
			_ArgumentListEditor.AddArgument(_ParameterName, _ParameterType, _ReferenceType, _IsPublicSet, _IsPublicGet);
		}
	}

	sealed class SettingArgumentPopup : PopupWindowContent
	{
		private static SettingArgumentPopup s_Instance = null;

		internal static void Open(Rect buttonRect, GraphArgumentListEditor argumentListEditor, GraphArgumentProperty argumentProperty)
		{
			if (s_Instance == null)
			{
				s_Instance = new SettingArgumentPopup();
			}
			s_Instance._ArgumentListEditor = argumentListEditor;
			s_Instance._ArgumentProperty = argumentProperty;

			s_Instance._IsPublicSet = argumentProperty.isPublicSet;
			s_Instance._IsPublicGet = argumentProperty.isPublicGet;

			s_Instance._FocusedWindow = EditorWindow.focusedWindow;

			PopupWindowUtility.Show(buttonRect, s_Instance, true);
		}

		GraphArgumentListEditor _ArgumentListEditor;
		GraphArgumentProperty _ArgumentProperty;
		EditorWindow _FocusedWindow;
		bool _IsPublicSet;
		bool _IsPublicGet;

		LayoutArea _LayoutArea = new LayoutArea();

		void DoGUI()
		{
			_IsPublicSet = _LayoutArea.VisibilityToggle(GUIContentCaches.Get("Set"), _IsPublicSet);
			_IsPublicGet = _LayoutArea.VisibilityToggle(GUIContentCaches.Get("Get"), _IsPublicGet);

			EditorGUI.BeginDisabledGroup((!_IsPublicSet && !_IsPublicGet) ||
					(_ArgumentProperty.isPublicSet == _IsPublicSet &&
					_ArgumentProperty.isPublicGet == _IsPublicGet));

			if (_LayoutArea.Button(EditorContents.apply))
			{
				_ArgumentListEditor.SetArgument(_ArgumentProperty, _IsPublicSet, _IsPublicGet);

				if (_FocusedWindow != null)
				{
					_FocusedWindow.Repaint();
				}
			}

			EditorGUI.EndDisabledGroup();
		}

		public override void OnGUI(Rect rect)
		{
			_LayoutArea.Begin(rect, false);

			DoGUI();

			_LayoutArea.End();
		}

		public override Vector2 GetWindowSize()
		{
			Rect rect = new Rect(0, 0, 150, 0);

			_LayoutArea.Begin(rect, true);

			DoGUI();

			_LayoutArea.End();

			return _LayoutArea.rect.size;
		}
	}

	sealed class SettingExternalArgumentPopup : ExternalArgumentPopupBase
	{
		private static SettingExternalArgumentPopup s_Instance = null;

		internal static void Open(Rect buttonRect, GraphArgumentListEditor argumentListEditor, GraphArgumentProperty argumentProperty)
		{
			if (s_Instance == null)
			{
				s_Instance = new SettingExternalArgumentPopup();
			}

			s_Instance._ArgumentProperty = argumentProperty;

			s_Instance._ParameterName = argumentProperty.parameterName;
			s_Instance._ParameterType = argumentProperty.parameterType;
			s_Instance._IsPublicSet = argumentProperty.isPublicSet;
			s_Instance._IsPublicGet = argumentProperty.isPublicGet;
			s_Instance._ReferenceType = argumentProperty.type;

			s_Instance.Show(buttonRect, argumentListEditor, EditorContents.apply);
		}

		private GraphArgumentProperty _ArgumentProperty;

		protected sealed override bool IsDisableButton()
		{
			return _ArgumentProperty.parameterName == _ParameterName &&
					_ArgumentProperty.parameterType == _ParameterType &&
					_ArgumentProperty.type == ParameterUtility.GetValueType(_ParameterType, _ReferenceType) &&
					_ArgumentProperty.isPublicSet == _IsPublicSet &&
					_ArgumentProperty.isPublicGet == _IsPublicGet;
		}

		protected sealed override void OnClickButton()
		{
			_ArgumentListEditor.SetArgument(_ArgumentProperty, _ParameterName, _ParameterType, _ReferenceType, _IsPublicSet, _IsPublicGet);
		}
	}

	internal sealed class GraphArgumentListEditor
	{
		private NodeGraph _NodeGraph = null;
		private bool _IsExternal = true;

		private ParameterContainerInternal _ParameterContainer;

		private SerializedProperty _Property;
		private SerializedProperty _Arguments;
		private ReorderableList _ReorderableList;

		private SerializedProperty _IntParameters;
		private SerializedProperty _LongParameters;
		private SerializedProperty _FloatParameters;
		private SerializedProperty _BoolParameters;
		private SerializedProperty _EnumParameters;
		private SerializedProperty _StringParameters;
		private SerializedProperty _Vector2Parameters;
		private SerializedProperty _Vector3Parameters;
		private SerializedProperty _QuaternionParameters;
		private SerializedProperty _RectParameters;
		private SerializedProperty _BoundsParameters;
		private SerializedProperty _ColorParameters;
		private SerializedProperty _Vector4Parameters;
		private SerializedProperty _Vector2IntParameters;
		private SerializedProperty _Vector3IntParameters;
		private SerializedProperty _RectIntParameters;
		private SerializedProperty _BoundsIntParameters;
		private SerializedProperty _GameObjectParameters;
		private SerializedProperty _ComponentParameters;
		private SerializedProperty _AssetObjectParameters;
		private SerializedProperty _InputSlots;
		private SerializedProperty _OutputSlots;

		public NodeGraph nodeGraph
		{
			get
			{
				return _NodeGraph;
			}
			set
			{
				NodeGraph nodeGraph = value;

				ParameterContainerInternal parameterContainer = nodeGraph != null ? nodeGraph.parameterContainer : null;

				if (_NodeGraph != nodeGraph || _ParameterContainer != parameterContainer)
				{
					_NodeGraph = nodeGraph;
					_ParameterContainer = parameterContainer;
					_IsExternal = nodeGraph == null;
				}
			}
		}

		public GraphArgumentListEditor(SerializedProperty property)
		{
			_Property = property;
			_Arguments = property.FindPropertyRelative("_Arguments");
			_ReorderableList = new ReorderableList(property.serializedObject, _Arguments, true, false, true, true)
			{
				headerHeight = 0f,
				onAddDropdownCallback = OnAddDropdown,
				drawElementCallback = DrawElement,
				elementHeightCallback = GetElementHeight,
				onRemoveCallback = OnRemove,
			};

			_IntParameters = property.FindPropertyRelative("_IntParameters");
			_LongParameters = property.FindPropertyRelative("_LongParameters");
			_FloatParameters = property.FindPropertyRelative("_FloatParameters");
			_BoolParameters = property.FindPropertyRelative("_BoolParameters");
			_EnumParameters = property.FindPropertyRelative("_EnumParameters");
			_StringParameters = property.FindPropertyRelative("_StringParameters");
			_Vector2Parameters = property.FindPropertyRelative("_Vector2Parameters");
			_Vector3Parameters = property.FindPropertyRelative("_Vector3Parameters");
			_QuaternionParameters = property.FindPropertyRelative("_QuaternionParameters");
			_RectParameters = property.FindPropertyRelative("_RectParameters");
			_BoundsParameters = property.FindPropertyRelative("_BoundsParameters");
			_ColorParameters = property.FindPropertyRelative("_ColorParameters");
			_Vector4Parameters = property.FindPropertyRelative("_Vector4Parameters");
			_Vector2IntParameters = property.FindPropertyRelative("_Vector2IntParameters");
			_Vector3IntParameters = property.FindPropertyRelative("_Vector3IntParameters");
			_RectIntParameters = property.FindPropertyRelative("_RectIntParameters");
			_BoundsIntParameters = property.FindPropertyRelative("_BoundsIntParameters");
			_GameObjectParameters = property.FindPropertyRelative("_GameObjectParameters");
			_ComponentParameters = property.FindPropertyRelative("_ComponentParameters");
			_AssetObjectParameters = property.FindPropertyRelative("_AssetObjectParameters");
			_InputSlots = property.FindPropertyRelative("_InputSlots");
			_OutputSlots = property.FindPropertyRelative("_OutputSlots");
		}

		public void UpdateNodeGraph(NodeGraph nodeGraph)
		{
			if (_NodeGraph == nodeGraph)
			{
				return;
			}

			ParameterContainerInternal parameterContainer = nodeGraph != null ? nodeGraph.parameterContainer : null;
			bool isExternal = nodeGraph == null;

			for (int i = 0, count = _Arguments.arraySize; i < count; i++)
			{
				GraphArgumentProperty argumentProperty = new GraphArgumentProperty(_Arguments.GetArrayElementAtIndex(i));
				if (!isExternal && parameterContainer != null)
				{
					argumentProperty.parameterID = parameterContainer.GetParamID(argumentProperty.parameterName);
				}
				else if (!_IsExternal && isExternal && _ParameterContainer != null)
				{
					Parameter parameter = _ParameterContainer.GetParam(argumentProperty.parameterID);
					argumentProperty.parameterName = parameter.name;
					argumentProperty.parameterID = 0;
				}
			}

			_NodeGraph = nodeGraph;
			_ParameterContainer = parameterContainer;
			_IsExternal = isExternal;
		}

		SerializedProperty GetParametersProperty(Parameter.Type parameterType)
		{
			switch (parameterType)
			{
				case Parameter.Type.Int:
					return _IntParameters;
				case Parameter.Type.Long:
					return _LongParameters;
				case Parameter.Type.Float:
					return _FloatParameters;
				case Parameter.Type.Bool:
					return _BoolParameters;
				case Parameter.Type.String:
					return _StringParameters;
				case Parameter.Type.Enum:
					return _EnumParameters;
				case Parameter.Type.GameObject:
					return _GameObjectParameters;
				case Parameter.Type.Vector2:
					return _Vector2Parameters;
				case Parameter.Type.Vector3:
					return _Vector3Parameters;
				case Parameter.Type.Quaternion:
					return _QuaternionParameters;
				case Parameter.Type.Rect:
					return _RectParameters;
				case Parameter.Type.Bounds:
					return _BoundsParameters;
				case Parameter.Type.Color:
					return _ColorParameters;
				case Parameter.Type.Vector4:
					return _Vector4Parameters;
				case Parameter.Type.Vector2Int:
					return _Vector2IntParameters;
				case Parameter.Type.Vector3Int:
					return _Vector3IntParameters;
				case Parameter.Type.RectInt:
					return _RectIntParameters;
				case Parameter.Type.BoundsInt:
					return _BoundsIntParameters;
				case Parameter.Type.Transform:
				case Parameter.Type.RectTransform:
				case Parameter.Type.Rigidbody:
				case Parameter.Type.Rigidbody2D:
				case Parameter.Type.Component:
					return _ComponentParameters;
				case Parameter.Type.AssetObject:
					return _AssetObjectParameters;
			}

			if (parameterType >= Parameter.Type.Variable)
			{
				return _InputSlots;
			}

			return null;
		}

		private bool ContainsParameter(Parameter parameter)
		{
			for (int i = 0, count = _Arguments.arraySize; i < count; i++)
			{
				GraphArgumentProperty argumentProperty = new GraphArgumentProperty(_Arguments.GetArrayElementAtIndex(i));
				if (argumentProperty.parameterID == parameter.id)
				{
					return true;
				}
			}

			return false;
		}

		private bool ContainsAllParameters()
		{
			if (_IsExternal)
			{
				return false;
			}

			if (_ParameterContainer != null)
			{
				for (int i = 0, count = _ParameterContainer.parameterCount; i < count; i++)
				{
					Parameter parameter = _ParameterContainer.GetParameterFromIndex(i);

					if (!parameter.isPublicGet && !parameter.isPublicSet)
					{
						continue;
					}

					if (!ContainsParameter(parameter))
					{
						return false;
					}
				}
			}
			return true;
		}

		void SetInputField(GraphArgumentProperty argumentProperty, Parameter.Type parameterType, bool isPublicSet, System.Type valueType)
		{
			argumentProperty.isPublicSet = isPublicSet;

			if (!isPublicSet)
			{
				return;
			}

			SerializedProperty parametersProperty = GetParametersProperty(parameterType);

			parametersProperty.arraySize++;
			int parameterIndex = parametersProperty.arraySize - 1;

			argumentProperty.parameterIndex = parameterIndex;

			SerializedProperty parameterProperty = parametersProperty.GetArrayElementAtIndex(parameterIndex);

			switch (parameterType)
			{
				case Parameter.Type.Int:
				case Parameter.Type.Float:
				case Parameter.Type.Long:
					{
						FlexibleNumericProperty flexibleProperty = new FlexibleNumericProperty(parameterProperty);
						flexibleProperty.ClearSlot();
						flexibleProperty.Clear(true);
					}
					break;
				case Parameter.Type.Bool:
					{
						FlexibleBoolProperty flexibleProperty = new FlexibleBoolProperty(parameterProperty);
						flexibleProperty.ClearSlot();
						flexibleProperty.Clear(true);
					}
					break;
				default:
					if (parameterType >= Parameter.Type.Variable)
					{
						InputSlotTypableProperty inputSlotProperty = new InputSlotTypableProperty(parameterProperty);
						inputSlotProperty.Clear();
						inputSlotProperty.type = valueType;
					}
					else
					{
						FlexibleFieldProperty flexibleProperty = new FlexibleFieldProperty(parameterProperty);
						flexibleProperty.ClearSlot();
						flexibleProperty.Clear(true);
					}
					break;
			}
		}

		void SetInputField(GraphArgumentProperty argumentProperty, Parameter parameter)
		{
			SetInputField(argumentProperty, parameter.type, parameter.isPublicSet, parameter.valueType);
		}

		void SetOutputField(GraphArgumentProperty argumentProperty, bool isPublicGet, System.Type valueType)
		{
			argumentProperty.isPublicGet = isPublicGet;

			if (!isPublicGet)
			{
				return;
			}

			_OutputSlots.arraySize++;
			int outputSlotIndex = _OutputSlots.arraySize - 1;

			argumentProperty.outputSlotIndex = outputSlotIndex;

			OutputSlotTypableProperty outputSlotProperty = new OutputSlotTypableProperty(_OutputSlots.GetArrayElementAtIndex(outputSlotIndex));
			outputSlotProperty.Clear();
			outputSlotProperty.type = valueType;
		}

		void SetOutputField(GraphArgumentProperty argumentProperty, Parameter parameter)
		{
			SetOutputField(argumentProperty, parameter.isPublicGet, parameter.valueType);
		}

		void SetParameter(GraphArgumentProperty argumentProperty, Parameter parameter)
		{
			argumentProperty.parameterID = parameter.id;
			argumentProperty.parameterName = parameter.name;
			argumentProperty.parameterType = parameter.type;
			argumentProperty.type = parameter.valueType;
			argumentProperty.updateTiming = GraphArgumentUpdateTiming.Enter; // Set Default;

			SetInputField(argumentProperty, parameter);
			SetOutputField(argumentProperty, parameter);
		}

		void AddArgument(Parameter parameter)
		{
			_Arguments.arraySize++;

			GraphArgumentProperty argumentProperty = new GraphArgumentProperty(_Arguments.GetArrayElementAtIndex(_Arguments.arraySize - 1));

			SetParameter(argumentProperty, parameter);
		}

		void SetParameter(GraphArgumentProperty argumentProperty, string parameterName, Parameter.Type parameterType, System.Type referenceType, bool isPublicSet, bool isPublicGet)
		{
			argumentProperty.parameterID = 0;
			argumentProperty.parameterName = parameterName;
			argumentProperty.parameterType = parameterType;

			System.Type valueType = ParameterUtility.GetValueType(parameterType, referenceType);

			argumentProperty.type = valueType;
			argumentProperty.updateTiming = GraphArgumentUpdateTiming.Enter; // Set Default;

			SetInputField(argumentProperty, parameterType, isPublicSet, valueType);
			SetOutputField(argumentProperty, isPublicGet, valueType);
		}

		internal void AddArgument(string parameterName, Parameter.Type parameterType, System.Type referenceType, bool isPublicSet, bool isPublicGet)
		{
			SerializedObject serializedObject = _Property.serializedObject;

			serializedObject.Update();

			_Arguments.arraySize++;

			GraphArgumentProperty argumentProperty = new GraphArgumentProperty(_Arguments.GetArrayElementAtIndex(_Arguments.arraySize - 1));

			SetParameter(argumentProperty, parameterName, parameterType, referenceType, isPublicSet, isPublicGet);

			serializedObject.ApplyModifiedProperties();
		}

		internal void SetArgument(GraphArgumentProperty argumentProperty, bool isPublicSet, bool isPublicGet)
		{
			SerializedObject serializedObject = _Property.serializedObject;

			serializedObject.Update();

			bool isPublicSetOld = argumentProperty.isPublicSet;
			bool isPublicGetOld = argumentProperty.isPublicGet;

			if (isPublicSet != isPublicSetOld || isPublicGet != isPublicGetOld)
			{
				if (isPublicSetOld != isPublicSet)
				{
					if (isPublicSetOld && !isPublicSet)
					{
						DeleteInputField(argumentProperty);
					}
					else
					{
						SetInputField(argumentProperty, argumentProperty.parameterType, isPublicSet, argumentProperty.type);
					}
				}

				if (isPublicGetOld != isPublicGet)
				{
					if (isPublicGetOld && !isPublicGet)
					{
						DeleteOutputField(argumentProperty);
					}
					else
					{
						SetOutputField(argumentProperty, isPublicGet, argumentProperty.type);
					}
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

		internal void SetArgument(GraphArgumentProperty argumentProperty, string parameterName, Parameter.Type parameterType, System.Type referenceType, bool isPublicSet, bool isPublicGet)
		{
			SerializedObject serializedObject = _Property.serializedObject;

			serializedObject.Update();

			if (argumentProperty.parameterType != parameterType)
			{
				DeleteParameter(argumentProperty);
				SetParameter(argumentProperty, parameterName, parameterType, referenceType, isPublicSet, isPublicGet);
			}
			else
			{
				argumentProperty.parameterName = parameterName;

				bool isPublicSetOld = argumentProperty.isPublicSet;
				bool isPublicGetOld = argumentProperty.isPublicGet;

				System.Type valueTypeOld = argumentProperty.type;

				System.Type valueType = ParameterUtility.GetValueType(parameterType, referenceType);

				if (valueTypeOld != valueType)
				{
					argumentProperty.type = valueType;
				}

				if (isPublicSet != isPublicSetOld || isPublicGet != isPublicGetOld)
				{
					if (isPublicSetOld != isPublicSet)
					{
						if (isPublicSetOld && !isPublicSet)
						{
							DeleteInputField(argumentProperty);
						}
						else
						{
							SetInputField(argumentProperty, parameterType, isPublicSet, valueType);
						}
					}

					if (isPublicGetOld != isPublicGet)
					{
						if (isPublicGetOld && !isPublicGet)
						{
							DeleteOutputField(argumentProperty);
						}
						else
						{
							SetOutputField(argumentProperty, isPublicGet, valueType);
						}
					}
				}
				else if ((parameterType == Parameter.Type.Component || parameterType == Parameter.Type.AssetObject || parameterType == Parameter.Type.Enum) && valueType != valueTypeOld)
				{
					if (isPublicGet)
					{
						int outputSlotIndex = argumentProperty.outputSlotIndex;
						SerializedProperty outputSlotProperty = isPublicGet ? _OutputSlots.GetArrayElementAtIndex(outputSlotIndex) : null;

						OutputSlotTypableProperty outputProperty = new OutputSlotTypableProperty(outputSlotProperty);
						outputProperty.type = valueType;
					}
				}
				else if (parameterType >= Parameter.Type.Variable)
				{
					if (isPublicSet)
					{
						int parameterIndex = argumentProperty.parameterIndex;
						SerializedProperty parametersProperty = GetParametersProperty(parameterType);
						SerializedProperty parameterProperty = parametersProperty.GetArrayElementAtIndex(parameterIndex);

						InputSlotTypableProperty inputProperty = new InputSlotTypableProperty(parameterProperty);

						if (valueType != inputProperty.type)
						{
							inputProperty.type = valueType;
						}
					}

					if (isPublicGet)
					{
						int outputSlotIndex = argumentProperty.outputSlotIndex;
						SerializedProperty outputSlotProperty = _OutputSlots.GetArrayElementAtIndex(outputSlotIndex);

						OutputSlotTypableProperty outputProperty = new OutputSlotTypableProperty(outputSlotProperty);

						if (valueType != outputProperty.type)
						{
							outputProperty.type = valueType;
						}
					}
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

		private void OnAddDropdown(Rect buttonRect, ReorderableList list)
		{
			GenericMenu genericMenu = new GenericMenu();
			if (_IsExternal)
			{
				// Open Argument Create Popup
				CreateExternalArgumentPopup.Open(buttonRect, this);
			}
			else if (_ParameterContainer != null)
			{
				for (int i = 0, count = _ParameterContainer.parameterCount; i < count; i++)
				{
					Parameter parameter = _ParameterContainer.GetParameterFromIndex(i);

					if (ContainsParameter(parameter))
					{
						continue;
					}

					if (!parameter.isPublicSet && !parameter.isPublicGet)
					{
						continue;
					}

					genericMenu.AddItem(GUIContentCaches.Get(parameter.name), false, () =>
					{
						SerializedObject serializedObject = _Property.serializedObject;

						serializedObject.Update();

						AddArgument(parameter);

						serializedObject.ApplyModifiedProperties();
					});
				}
			}
			genericMenu.DropDown(buttonRect);
		}

		void DeleteInputField(GraphArgumentProperty argumentProperty)
		{
			bool isPublicSet = argumentProperty.isPublicSet;

			if (!isPublicSet)
			{
				return;
			}

			Parameter.Type parameterType = argumentProperty.parameterType;

			SerializedProperty parametersProperty = GetParametersProperty(parameterType);

			int parameterIndex = argumentProperty.parameterIndex;

			for (int i = 0, count = _Arguments.arraySize; i < count; i++)
			{
				GraphArgumentProperty a = new GraphArgumentProperty(_Arguments.GetArrayElementAtIndex(i));
				if (a.isPublicSet)
				{
					SerializedProperty parameters = GetParametersProperty(a.parameterType);
					if (SerializedPropertyUtility.EqualContents(parameters, parametersProperty) && a.parameterIndex > parameterIndex)
					{
						a.parameterIndex--;
					}
				}
			}

			// disconnect data
			SerializedProperty parameterProperty = parametersProperty.GetArrayElementAtIndex(parameterIndex);
			switch (FlexibleUtility.GetPropertyType(parameterType))
			{
				case FlexiblePropertyType.Primitive:
					new FlexiblePrimitiveProperty(parameterProperty).Disconnect();
					break;
				case FlexiblePropertyType.Field:
					new FlexibleFieldProperty(parameterProperty).Disconnect();
					break;
				case FlexiblePropertyType.SceneObject:
					new FlexibleSceneObjectProperty(parameterProperty).Disconnect();
					break;
				default:
					if (parameterType >= Parameter.Type.Variable)
					{
						new InputSlotTypableProperty(parameterProperty).Disconnect();
					}
					break;
			}

			parametersProperty.DeleteArrayElementAtIndex(parameterIndex);

			argumentProperty.isPublicSet = false;
		}

		void DeleteOutputField(GraphArgumentProperty argumentProperty)
		{
			bool isPublicGet = argumentProperty.isPublicGet;
			if (!isPublicGet)
			{
				return;
			}

			int outputSlotIndex = argumentProperty.outputSlotIndex;

			for (int i = 0, count = _Arguments.arraySize; i < count; i++)
			{
				GraphArgumentProperty a = new GraphArgumentProperty(_Arguments.GetArrayElementAtIndex(i));
				if (a.isPublicGet)
				{
					if (a.outputSlotIndex > outputSlotIndex)
					{
						a.outputSlotIndex--;
					}
				}
			}

			// disconnect data
			SerializedProperty ouputSlotProperty = _OutputSlots.GetArrayElementAtIndex(outputSlotIndex);
			OutputSlotTypableProperty outputSlotTypableProperty = new OutputSlotTypableProperty(ouputSlotProperty);
			outputSlotTypableProperty.Disconnect();

			_OutputSlots.DeleteArrayElementAtIndex(outputSlotIndex);

			argumentProperty.isPublicGet = false;
		}

		void DeleteParameter(GraphArgumentProperty argumentProperty)
		{
			DeleteInputField(argumentProperty);
			DeleteOutputField(argumentProperty);
		}

		private void OnRemove(ReorderableList list)
		{
			GraphArgumentProperty argumentProperty = new GraphArgumentProperty(list.serializedProperty.GetArrayElementAtIndex(list.index));

			DeleteParameter(argumentProperty);

			ReorderableList.defaultBehaviours.DoRemoveButton(list);
		}

		PropertyHeightCache _PropertyHeights = new PropertyHeightCache();

		LayoutArea _LayoutArea = new LayoutArea();

		void ClearCache()
		{
			_PropertyHeights.Clear();
		}

		enum RepairType
		{
			None,
			ParameterType,
			ReferenceType,
			AccessLevel,
			VariableType,
		}

		void DoSettingPopup(GraphArgumentProperty argumentProperty, params LayoutOption[] options)
		{
			if (_LayoutArea.ButtonMouseDown(EditorContents.popupIcon, FocusType.Passive, Styles.popupIconButton, options))
			{
				if (_IsExternal)
				{
					SettingExternalArgumentPopup.Open(_LayoutArea.lastRect, this, argumentProperty);
				}
				else
				{
					SettingArgumentPopup.Open(_LayoutArea.lastRect, this, argumentProperty);
				}
			}
		}

		void DoGUI(SerializedProperty elementProperty)
		{
			GraphArgumentProperty argumentProperty = new GraphArgumentProperty(elementProperty);

			Parameter.Type parameterType = argumentProperty.parameterType;

			bool isPublicSet = argumentProperty.isPublicSet;
			bool isPublicGet = argumentProperty.isPublicGet;

			int parameterIndex = argumentProperty.parameterIndex;
			SerializedProperty parametersProperty = GetParametersProperty(parameterType);
			SerializedProperty parameterProperty = isPublicSet ? parametersProperty.GetArrayElementAtIndex(parameterIndex) : null;

			int outputSlotIndex = argumentProperty.outputSlotIndex;
			SerializedProperty outputSlotProperty = isPublicGet ? _OutputSlots.GetArrayElementAtIndex(outputSlotIndex) : null;

			Parameter parameter = _ParameterContainer != null ? _ParameterContainer.GetParam(argumentProperty.parameterID) : null;

			string parameterName = parameter != null && parameter.type == parameterType ? parameter.name : argumentProperty.parameterName;

			System.Type valueType = argumentProperty.type;

			bool isSetting = _IsExternal || (parameter != null && parameter.isPublicGet && parameter.isPublicSet);

			float verticalSpacing = Mathf.FloorToInt((_ReorderableList.elementHeight - EditorGUIUtility.singleLineHeight) * 0.5f) - 2f;

			_LayoutArea.Space(verticalSpacing);

			_LayoutArea.BeginHorizontal();

			if (isPublicSet)
			{
				float width = _LayoutArea.rect.width;
				if (isSetting)
				{
					width -= EditorGUITools.kSubtrackPopupWidth;
				}

				argumentProperty.updateTiming = (GraphArgumentUpdateTiming)_LayoutArea.EnumMaskField(GUIContentCaches.Get(parameterName), argumentProperty.updateTiming, LayoutArea.Width(width));

				if (isSetting)
				{
					_LayoutArea.GetRect(EditorGUITools.kSubtrackPopupWidth - EditorGUITools.kPopupWidth, 0f);

					DoSettingPopup(argumentProperty);
				}
			}
			else
			{
				float width = _LayoutArea.rect.width;

				if (isPublicGet)
				{
					width -= 20f;
				}

				if (isSetting)
				{
					width -= EditorGUITools.kSubtrackPopupWidth;
				}

				_LayoutArea.LabelField(GUIContentCaches.Get(parameterName), LayoutArea.Width(width));

				if (isSetting)
				{
					_LayoutArea.Space(EditorGUITools.kSubtrackPopupWidth - EditorGUITools.kPopupWidth);

					width = EditorGUITools.kPopupWidth;

					DoSettingPopup(argumentProperty, LayoutArea.Width(width));
				}

				if (isPublicGet)
				{
					_LayoutArea.Space(2f);

					_LayoutArea.PropertyField(outputSlotProperty, GUIContent.none);
				}
			}

			_LayoutArea.EndHorizontal();

			_LayoutArea.Space(verticalSpacing);

			_LayoutArea.BeginHorizontal();

			if (isPublicSet)
			{
				float width = _LayoutArea.rect.width;

				if (isPublicGet)
				{
					width -= 18f;
				}

				_LayoutArea.PropertyField(parameterProperty, GUIContentCaches.Get("Value"), LayoutArea.Width(width));

				if (isPublicGet)
				{
					_LayoutArea.PropertyField(outputSlotProperty, GUIContent.none, LayoutArea.Width(18f));
				}
			}

			_LayoutArea.EndHorizontal();

			if (!_IsExternal)
			{
				// Check Broken & Repair
				// * Not found Parameter
				// * Chagend Parameter.Type
				// * Changed Reference Type
				// * Changed Access Level
				// * Changed Variable Type
				System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
				RepairType repairType = RepairType.None;
				if (parameter == null)
				{
					stringBuilder.AppendLine(Localization.GetWord("GraphArgument.Message.NotFoundParameter"));
				}
				else
				{
					if (parameter.type != parameterType)
					{
						stringBuilder.AppendLine(Localization.GetWord("ArborEvent.ChangedParameterType"));
						repairType = RepairType.ParameterType;
					}
					else
					{
						if ((isPublicSet && !parameter.isPublicSet) || (isPublicGet && !parameter.isPublicGet))
						{
							stringBuilder.AppendLine(Localization.GetWord("GraphArgument.Message.ChangedAccessLevel"));
							if (!parameter.isPublicSet && !parameter.isPublicGet)
							{
								stringBuilder.AppendLine(Localization.GetWord("GraphArgument.Message.CantAccessPrivateParameter"));
							}
							else
							{

								repairType = RepairType.AccessLevel;
							}
						}
						else if ((parameter.type == Parameter.Type.Component || parameter.type == Parameter.Type.AssetObject || parameter.type == Parameter.Type.Enum) && parameter.referenceType != valueType)
						{
							stringBuilder.AppendLine(Localization.GetWord("GraphArgument.Message.ChangedReferenceType"));
							repairType = RepairType.ReferenceType;
						}
						else if (parameter.type >= Parameter.Type.Variable)
						{
							InputSlotTypableProperty inputProperty = isPublicSet ? new InputSlotTypableProperty(parameterProperty) : null;
							OutputSlotTypableProperty outputProperty = isPublicGet ? new OutputSlotTypableProperty(outputSlotProperty) : null;

							System.Type parameterValueType = parameter.valueType;
							if (inputProperty != null && parameterValueType != inputProperty.type ||
								outputProperty != null && parameterValueType != outputProperty.type)
							{
								stringBuilder.AppendLine(Localization.GetWord("GraphArgument.Message.ChangedVariableType"));
								repairType = RepairType.VariableType;
							}
						}
					}
				}

				if (stringBuilder.Length > 0)
				{
					_LayoutArea.HelpBox(stringBuilder.ToString(), MessageType.Error);

					if (repairType != RepairType.None)
					{
						if (_LayoutArea.Button(EditorContents.repair))
						{
							switch (repairType)
							{
								case RepairType.ParameterType:
									{
										DeleteParameter(argumentProperty);
										SetParameter(argumentProperty, parameter);
									}
									break;
								case RepairType.ReferenceType:
									{
										System.Type referenceType = parameter.referenceType.type;
										argumentProperty.type = referenceType;

										if (isPublicGet)
										{
											OutputSlotTypableProperty outputProperty = new OutputSlotTypableProperty(outputSlotProperty);
											outputProperty.type = referenceType;
										}
									}
									break;
								case RepairType.AccessLevel:
									{
										if (parameter.isPublicSet != isPublicSet)
										{
											if (isPublicSet && !parameter.isPublicSet)
											{
												DeleteInputField(argumentProperty);
											}
											else
											{
												SetInputField(argumentProperty, parameter);
											}
										}

										if (parameter.isPublicGet != isPublicGet)
										{
											if (isPublicGet && !parameter.isPublicGet)
											{
												DeleteOutputField(argumentProperty);
											}
											else
											{
												SetOutputField(argumentProperty, parameter);
											}
										}
									}
									break;
								case RepairType.VariableType:
									{
										System.Type vt = parameter.valueType;

										if (isPublicSet)
										{
											InputSlotTypableProperty inputProperty = new InputSlotTypableProperty(parameterProperty);
											inputProperty.type = vt;
										}

										if (isPublicGet)
										{
											OutputSlotTypableProperty outputProperty = new OutputSlotTypableProperty(outputSlotProperty);
											outputProperty.type = vt;
										}
									}
									break;
							}
						}
					}
				}
			}
		}

		static readonly RectOffset s_LayoutMargin = new RectOffset(0, 0, 0, 2);

		void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty property = _ReorderableList.serializedProperty.GetArrayElementAtIndex(index);

			float height = 0f;
			if (_PropertyHeights.TryGetHeight(property, out height))
			{
				rect.height = height;
			}

			_LayoutArea.Begin(rect, false, s_LayoutMargin);

			DoGUI(property);

			_LayoutArea.End();
		}

		private Rect GetContentRect(Rect rect)
		{
			Rect r = rect;

			if (_ReorderableList.draggable)
				r.xMin += ReorderableList.Defaults.dragHandleWidth;
			else
				r.xMin += ReorderableList.Defaults.padding;
			r.xMax -= ReorderableList.Defaults.padding;
			return r;
		}

		float GetElementHeight(int index)
		{
			SerializedProperty property = _ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
			float height = 0f;
			if (!_PropertyHeights.TryGetHeight(property, out height))
			{
				_LayoutArea.Begin(new Rect(0, 0, UnityEditorBridge.EditorGUIUtilityBridge.contextWidth, 0), true, s_LayoutMargin);

				DoGUI(property);

				_LayoutArea.End();

				height = Mathf.Max(_LayoutArea.rect.height, _ReorderableList.elementHeight);

				_PropertyHeights.AddHeight(property, height);
			}

			return height;
		}

		public void DoLayoutList()
		{
			if (Event.current.type == EventType.Layout)
			{
				ClearCache();
			}

			if (_IsExternal || _ParameterContainer != null && _ParameterContainer.parameterCount > 0 || _Arguments.arraySize > 0)
			{
				if (ContainsAllParameters())
				{
					_ReorderableList.displayAdd = false;
				}
				else
				{
					_ReorderableList.displayAdd = true;
				}
				_ReorderableList.DoLayoutList();
			}
		}
	}
}