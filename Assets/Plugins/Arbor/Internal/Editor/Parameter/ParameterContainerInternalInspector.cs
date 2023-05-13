//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

namespace ArborEditor
{
	using Arbor;

	[CustomEditor(typeof(ParameterContainerInternal), true)]
	internal sealed class ParameterContainerInternalInspector : Editor, IPropertyChanged
	{
		internal bool isInParametersPanel = false;
		internal event System.Action onRemoveComponent = null;

		ParameterContainerInternal _ParameterContainer;
		ReorderableListEx _ParametersList;
		ReorderableListEx _SearchParametersList;

		private SerializedProperty _ParameterListProperty;

		private SerializedProperty _IntParameters;
		private SerializedProperty _LongParameters;
		private SerializedProperty _FloatParameters;
		private SerializedProperty _BoolParameters;
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
		private SerializedProperty _ObjectParameters;

		Dictionary<Object, SerializedObject> _VariableParameterObjects = new Dictionary<Object, SerializedObject>();

		private SerializedProperty _IntListParameters;
		private SerializedProperty _LongListParameters;
		private SerializedProperty _FloatListParameters;
		private SerializedProperty _BoolListParameters;
		private SerializedProperty _StringListParameters;
		private SerializedProperty _EnumListParameters;
		private SerializedProperty _Vector2ListParameters;
		private SerializedProperty _Vector3ListParameters;
		private SerializedProperty _QuaternionListParameters;
		private SerializedProperty _RectListParameters;
		private SerializedProperty _BoundsListParameters;
		private SerializedProperty _ColorListParameters;
		private SerializedProperty _Vector4ListParameters;
		private SerializedProperty _Vector2IntListParameters;
		private SerializedProperty _Vector3IntListParameters;
		private SerializedProperty _RectIntListParameters;
		private SerializedProperty _BoundsIntListParameters;
		private SerializedProperty _GameObjectListParameters;
		private SerializedProperty _ComponentListParameters;
		private SerializedProperty _AssetObjectListParameters;

		private PropertyHeightCache _PropertyHeightCache = new PropertyHeightCache();
		
		SerializedProperty parameterListProperty
		{
			get
			{
				if (_ParameterListProperty == null)
				{
					_ParameterListProperty = serializedObject.FindProperty("_Parameters");
				}
				return _ParameterListProperty;
			}
		}

		internal ReorderableListEx parameterList
		{
			get
			{
				int[] activeElements = this.activeElements;
				if (activeElements != null)
				{
					if (_SearchParametersList == null)
					{
						_SearchParametersList = new ReorderableListEx(activeElements, typeof(int), false, false, false, false)
						{
							headerHeight = 0f,
							footerHeight = 0f,
							elementHeightCallback = GetElementHeight,
							drawElementCallback = DrawElement,
						};
					}

					_SearchParametersList.list = activeElements;

					return _SearchParametersList;
				}
				if (_ParametersList == null)
				{
					_ParametersList = new ReorderableListEx(serializedObject, parameterListProperty, true, false, false, false)
					{
						headerHeight = 0f,
						footerHeight = 0f,
						elementHeightCallback = GetElementHeight,
						drawElementCallback = DrawElement,
					};
				}

				_ParametersList.serializedProperty = parameterListProperty;
				_ParametersList.draggable = parameterListProperty.editable;

				return _ParametersList;
			}
		}

		void OnEnable()
		{
			_ParameterContainer = target as ParameterContainerInternal;
			_ParameterContainer.Refresh();

			_IntParameters = serializedObject.FindProperty("_IntParameters");
			_LongParameters = serializedObject.FindProperty("_LongParameters");
			_FloatParameters = serializedObject.FindProperty("_FloatParameters");
			_BoolParameters = serializedObject.FindProperty("_BoolParameters");
			_StringParameters = serializedObject.FindProperty("_StringParameters");
			_Vector2Parameters = serializedObject.FindProperty("_Vector2Parameters");
			_Vector3Parameters = serializedObject.FindProperty("_Vector3Parameters");
			_QuaternionParameters = serializedObject.FindProperty("_QuaternionParameters");
			_RectParameters = serializedObject.FindProperty("_RectParameters");
			_BoundsParameters = serializedObject.FindProperty("_BoundsParameters");
			_ColorParameters = serializedObject.FindProperty("_ColorParameters");
			_Vector4Parameters = serializedObject.FindProperty("_Vector4Parameters");
			_Vector2IntParameters = serializedObject.FindProperty("_Vector2IntParameters");
			_Vector3IntParameters = serializedObject.FindProperty("_Vector3IntParameters");
			_RectIntParameters = serializedObject.FindProperty("_RectIntParameters");
			_BoundsIntParameters = serializedObject.FindProperty("_BoundsIntParameters");
			_ObjectParameters = serializedObject.FindProperty("_ObjectParameters");

			_IntListParameters = serializedObject.FindProperty("_IntListParameters");
			_LongListParameters = serializedObject.FindProperty("_LongListParameters");
			_FloatListParameters = serializedObject.FindProperty("_FloatListParameters");
			_BoolListParameters = serializedObject.FindProperty("_BoolListParameters");
			_StringListParameters = serializedObject.FindProperty("_StringListParameters");
			_EnumListParameters = serializedObject.FindProperty("_EnumListParameters");
			_Vector2ListParameters = serializedObject.FindProperty("_Vector2ListParameters");
			_Vector3ListParameters = serializedObject.FindProperty("_Vector3ListParameters");
			_QuaternionListParameters = serializedObject.FindProperty("_QuaternionListParameters");
			_RectListParameters = serializedObject.FindProperty("_RectListParameters");
			_BoundsListParameters = serializedObject.FindProperty("_BoundsListParameters");
			_ColorListParameters = serializedObject.FindProperty("_ColorListParameters");
			_Vector4ListParameters = serializedObject.FindProperty("_Vector4ListParameters");
			_Vector2IntListParameters = serializedObject.FindProperty("_Vector2IntListParameters");
			_Vector3IntListParameters = serializedObject.FindProperty("_Vector3IntListParameters");
			_RectIntListParameters = serializedObject.FindProperty("_RectIntListParameters");
			_BoundsIntListParameters = serializedObject.FindProperty("_BoundsIntListParameters");
			_GameObjectListParameters = serializedObject.FindProperty("_GameObjectListParameters");
			_ComponentListParameters = serializedObject.FindProperty("_ComponentListParameters");
			_AssetObjectListParameters = serializedObject.FindProperty("_AssetObjectListParameters");

			EditorCallbackUtility.RegisterPropertyChanged(this);
		}

		private void OnDisable()
		{
			EditorCallbackUtility.UnregisterPropertyChanged(this);
		}

		void AddParameter(Parameter.Type parameterType, System.Type variableType)
		{
			if (parameterType == Parameter.Type.Variable)
			{
				Undo.IncrementCurrentGroup();

				VariableBase variable = VariableBase.Create(_ParameterContainer, variableType) as VariableBase;

				var behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(variableType);

				Undo.RecordObject(_ParameterContainer, "Parameter Added");
				Parameter parameter = _ParameterContainer.AddParam("New " + behaviourInfo.titleContent.text, Parameter.Type.Variable);

				parameter.variableObject = variable;

				Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
			}
			else if (parameterType == Parameter.Type.VariableList)
			{
				Undo.IncrementCurrentGroup();

				VariableListBase variable = VariableListBase.Create(_ParameterContainer, variableType) as VariableListBase;

				var behaviourInfo = BehaviourInfoUtility.GetBehaviourInfo(variableType);

				Undo.RecordObject(_ParameterContainer, "Parameter Added");
				Parameter parameter = _ParameterContainer.AddParam("New " + behaviourInfo.titleContent.text, Parameter.Type.VariableList);

				parameter.variableListObject = variable;

				Undo.CollapseUndoOperations(Undo.GetCurrentGroup());
			}
			else
			{
				Undo.RecordObject(_ParameterContainer, "Parameter Added");
				_ParameterContainer.AddParam("New " + parameterType.ToString(), parameterType);
			}
		}

		void OnAddDropdown(Rect buttonRect, ReorderableList list)
		{
			buttonRect = GUIUtility.GUIToScreenRect(buttonRect);
			SelectParameterTypeWindow.instance.Init(buttonRect, AddParameter);
		}

		void OnRemove(ReorderableList list)
		{
			int elementIndex = GetElementIndex(list.index);
			Parameter parameter = _ParameterContainer.GetParameterFromIndex(elementIndex);

			switch (parameter.type)
			{
				case Parameter.Type.Variable:
					RemoveVariableSerializedObject(parameter.variableObject);
					break;
				case Parameter.Type.VariableList:
					RemoveVariableSerializedObject(parameter.variableListObject);
					break;
			}

			EditorWindow focusedWindow = EditorWindow.focusedWindow;

			EditorApplication.delayCall += () =>
			{
				_ParameterContainer.DeleteParam(parameter);

				_SearchElements = null;
				if (focusedWindow != null)
				{
					focusedWindow.Repaint();
				}
			};
		}

		internal SerializedObject GetVariableSerializedObject(Object obj)
		{
			SerializedObject serializedObject = null;
			if ((object)obj != null && !_VariableParameterObjects.TryGetValue(obj, out serializedObject))
			{
				serializedObject = new SerializedObject(obj);
				_VariableParameterObjects.Add(obj, serializedObject);
			}

			return serializedObject;
		}

		void RemoveVariableSerializedObject(Object obj)
		{
			SerializedObject serializedObject = null;
			if ((object)obj != null && _VariableParameterObjects.TryGetValue(obj, out serializedObject))
			{
				serializedObject.Dispose();
				_VariableParameterObjects.Remove(obj);
			}
		}

		void ClearCache()
		{
			_PropertyHeightCache.Clear();
		}

		internal SerializedProperty GetParametersProperty(Parameter.Type type)
		{
			switch (type)
			{
				case Parameter.Type.Int:
				case Parameter.Type.Enum:
					return _IntParameters;
				case Parameter.Type.Long:
					return _LongParameters;
				case Parameter.Type.Float:
					return _FloatParameters;
				case Parameter.Type.Bool:
					return _BoolParameters;
				case Parameter.Type.String:
					return _StringParameters;
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
				case Parameter.Type.Variable:
				case Parameter.Type.VariableList:
				case Parameter.Type.GameObject:
				case Parameter.Type.AssetObject:
					return _ObjectParameters;
				case Parameter.Type.IntList:
					return _IntListParameters;
				case Parameter.Type.LongList:
					return _LongListParameters;
				case Parameter.Type.FloatList:
					return _FloatListParameters;
				case Parameter.Type.BoolList:
					return _BoolListParameters;
				case Parameter.Type.StringList:
					return _StringListParameters;
				case Parameter.Type.EnumList:
					return _EnumListParameters;
				case Parameter.Type.Vector2List:
					return _Vector2ListParameters;
				case Parameter.Type.Vector3List:
					return _Vector3ListParameters;
				case Parameter.Type.QuaternionList:
					return _QuaternionListParameters;
				case Parameter.Type.RectList:
					return _RectListParameters;
				case Parameter.Type.BoundsList:
					return _BoundsListParameters;
				case Parameter.Type.ColorList:
					return _ColorListParameters;
				case Parameter.Type.Vector4List:
					return _Vector4ListParameters;
				case Parameter.Type.Vector2IntList:
					return _Vector2IntListParameters;
				case Parameter.Type.Vector3IntList:
					return _Vector3IntListParameters;
				case Parameter.Type.RectIntList:
					return _RectIntListParameters;
				case Parameter.Type.BoundsIntList:
					return _BoundsIntListParameters;
				case Parameter.Type.GameObjectList:
					return _GameObjectListParameters;
				case Parameter.Type.ComponentList:
					return _ComponentListParameters;
				case Parameter.Type.AssetObjectList:
					return _AssetObjectListParameters;
				default:
					throw new System.NotImplementedException("It is an unimplemented Parameter type(" + type + ")");
			}
		}

		ParameterPropertyEditor GetParameterPropertyEditor(SerializedProperty property)
		{
			var propertyEditor = PropertyEditorUtility<ParameterPropertyEditor>.GetPropertyEditor(property, null);
			propertyEditor.editor = this;
			return propertyEditor;
		}

		float GetElementHeight(int index)
		{
			int elementIndex = GetElementIndex(index);
			SerializedProperty property = parameterListProperty.GetArrayElementAtIndex(elementIndex);

			float height = 0f;
			if (!_PropertyHeightCache.TryGetHeight(property, out height))
			{
				var propertyEditor = GetParameterPropertyEditor(property);
				height = propertyEditor.DoGetHeight(GUIContent.none);
				
				_PropertyHeightCache.AddHeight(property, height);
			}

			return height;
		}

		void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			int elementIndex = GetElementIndex(index);
			SerializedProperty property = parameterListProperty.GetArrayElementAtIndex(elementIndex);

			var propertyEditor = GetParameterPropertyEditor(property);
			propertyEditor.DoOnGUI(rect, GUIContent.none);
		}

		enum SearchMode
		{
			Name,
			Type,
		}

		private string _SearchText = "";
		private SearchMode _SearchMode = SearchMode.Name;
		private Parameter.Type _SearchParameterType = Parameter.Type.Int;
		private Vector2 _ScrollPos = Vector2.zero;

		private int[] _SearchElements = null;

		int GetElementIndex(int index)
		{
			int[] elements = activeElements;
			if (elements != null)
			{
				return elements[index];
			}

			return index;
		}

		void CreateSearchParameterList(string searchText, SearchMode searchMode, Parameter.Type searchParameterType)
		{
			string[] strArray = searchText.ToLower().Split(' ');
			List<int> elementList1 = new List<int>();
			List<int> elementList2 = new List<int>();

			for (int parameterIndex = 0; parameterIndex < parameterListProperty.arraySize; parameterIndex++)
			{
				SerializedProperty parameterProperty = parameterListProperty.GetArrayElementAtIndex(parameterIndex);

				SerializedProperty typeProperty = parameterProperty.FindPropertyRelative("type");

				Parameter.Type type = EnumUtility.GetValueFromIndex<Parameter.Type>(typeProperty.enumValueIndex);

				if (searchMode == SearchMode.Name || (searchMode == SearchMode.Type && searchParameterType == type))
				{
					SerializedProperty nameProperty = parameterProperty.FindPropertyRelative("name");

					string name = nameProperty.stringValue;

					string str1 = name.ToLower().Replace(" ", string.Empty);
					bool flag1 = true;
					bool flag2 = false;
					for (int searchIndex = 0; searchIndex < strArray.Length; ++searchIndex)
					{
						string str2 = strArray[searchIndex];
						if (str1.Contains(str2))
						{
							if (searchIndex == 0 && str1.StartsWith(str2, System.StringComparison.Ordinal))
							{
								flag2 = true;
							}
						}
						else
						{
							flag1 = false;
							break;
						}
					}
					if (flag1)
					{
						if (flag2)
						{
							elementList1.Add(parameterIndex);
						}
						else
						{
							elementList2.Add(parameterIndex);
						}
					}
				}
			}

			List<int> elementList3 = new List<int>();
			elementList3.AddRange(elementList1);
			elementList3.AddRange(elementList2);
			_SearchElements = elementList3.ToArray();
		}

		int[] activeElements
		{
			get
			{
				if (string.IsNullOrEmpty(_SearchText) && _SearchMode == SearchMode.Name)
				{
					return null;
				}

				if (_SearchElements == null)
				{
					CreateSearchParameterList(_SearchText, _SearchMode, _SearchParameterType);
				}

				return _SearchElements;
			}
		}

		void RebuildSearchElements()
		{
			_SearchElements = null;
			Repaint();
		}

		void IPropertyChanged.OnPropertyChanged(PropertyChangedType propertyChangedType)
		{
			if (propertyChangedType != PropertyChangedType.UndoRedoPerformed)
			{
				return;
			}

			RebuildSearchElements();
		}

		private static class SearchGUI
		{
			public static readonly string[] searchModeNames;

			static SearchGUI()
			{
				ParameterTypeMenuItem[] menuItems = ParameterTypeMenuItem.menuItems;
				searchModeNames = new string[2 + menuItems.Length];

				searchModeNames[0] = "Name";
				searchModeNames[1] = "";

				for (int i = 0; i < menuItems.Length; i++)
				{
					searchModeNames[2 + i] = menuItems[i].menuName;
				}
			}

			public static int GetIndex(SearchMode searchMode, Parameter.Type parameterType)
			{
				if (searchMode == SearchMode.Name)
				{
					return 0;
				}

				int index = ParameterTypeMenuItem.GetIndex(parameterType);
				if (index < 0)
				{
					return -1;
				}

				return index + 2;
			}

			public static void GetSeachMode(int index, out SearchMode seachMode, out Parameter.Type parameterType)
			{
				if (index == 0)
				{
					seachMode = SearchMode.Name;
					parameterType = Parameter.Type.Int;
				}
				else
				{
					seachMode = SearchMode.Type;
					parameterType = ParameterTypeMenuItem.menuItems[index - 2].type;
				}
			}
		}

		void ListHeaderGUI()
		{
			GUIStyle headerStyle = isInParametersPanel ? ArborEditor.BuiltInStyles.toolbar : GUIStyle.none;
			EditorGUILayout.BeginHorizontal(headerStyle, GUILayout.Height(UnityEditorBridge.EditorGUIBridge.toolbarHeight));

			GUILayout.Space(10f);

			int selectedIndex = SearchGUI.GetIndex(_SearchMode, _SearchParameterType);

			GUI.SetNextControlName("ParameterSearch");
			if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.Escape && GUI.GetNameOfFocusedControl() == "ParameterSearch")
			{
				_SearchText = string.Empty;
				CreateSearchParameterList(_SearchText, _SearchMode, _SearchParameterType);
			}

			EditorGUI.BeginChangeCheck();
			string searchText = UnityEditorBridge.EditorGUILayoutBridge.ToolbarSearchField(_SearchText, SearchGUI.searchModeNames, ref selectedIndex);
			if (EditorGUI.EndChangeCheck())
			{
				_SearchText = searchText;

				SearchGUI.GetSeachMode(selectedIndex, out _SearchMode, out _SearchParameterType);

				CreateSearchParameterList(_SearchText, _SearchMode, _SearchParameterType);
			}

			GUILayout.FlexibleSpace();

			bool editable = target != null && (target.hideFlags & HideFlags.NotEditable) != HideFlags.NotEditable;

			EditorGUI.BeginDisabledGroup(!editable);

			using (new EditorGUI.DisabledGroupScope(activeElements != null))
			{
				Rect addButtonRect = GUILayoutUtility.GetRect(EditorContents.iconToolbarPlusMore, ArborEditor.BuiltInStyles.invisibleButton);
				if (GUI.Button(addButtonRect, EditorContents.iconToolbarPlusMore, ArborEditor.BuiltInStyles.invisibleButton))
				{
					OnAddDropdown(addButtonRect, parameterList);
				}
			}

			using (new EditorGUI.DisabledGroupScope(parameterList.index < 0 || parameterList.count <= parameterList.index))
			{
				if (GUILayout.Button(EditorContents.iconToolbarMinus, ArborEditor.BuiltInStyles.invisibleButton))
				{
					OnRemove(parameterList);
					RebuildSearchElements();
				}
			}

			if (isInParametersPanel)
			{
				if (GUILayout.Button(EditorContents.popupIcon, ArborEditor.BuiltInStyles.invisibleButton))
				{
					GenericMenu menu = new GenericMenu();
					menu.AddItem(Localization.GetTextContent("Remove Component"), false, () =>
					{
						int undoGroup = Undo.GetCurrentGroup();

						NodeGraph nodeGraph = _ParameterContainer.owner as NodeGraph;

						Undo.RecordObject(nodeGraph, "Destroy ParameterContainer");

						ParameterContainerEditorUtility.SetParameterContainer(nodeGraph, null);

						ParameterContainerInternal.Destroy(_ParameterContainer);

						Undo.CollapseUndoOperations(undoGroup);

						EditorUtility.SetDirty(nodeGraph);

						onRemoveComponent?.Invoke();
					});
					
					menu.ShowAsContext();
				}
			}

			EditorGUI.EndDisabledGroup();

			EditorGUILayout.EndHorizontal();
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			if (Event.current.type == EventType.Layout)
			{
				ClearCache();
			}

			var oldIndentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			if (!isInParametersPanel)
			{
				EditorGUILayout.BeginVertical(ArborEditor.BuiltInStyles.parameterListHeader);

				EditorGUILayout.PrefixLabel(parameterListProperty.displayName);

				ListHeaderGUI();

				EditorGUILayout.EndVertical();
			}
			else
			{
				ListHeaderGUI();
			}


			if (isInParametersPanel)
			{
				_ScrollPos = EditorGUILayout.BeginScrollView(_ScrollPos);
			}

			//EditorGUI.BeginDisabledGroup(!editable);

			var parameterList = this.parameterList;
			parameterList.DoLayoutList(UnityEditorBridge.GUIClipBridge.visibleRect);

			//EditorGUI.EndDisabledGroup();

			if (isInParametersPanel)
			{
				EditorGUILayout.EndScrollView();
			}
			EditorGUI.indentLevel = oldIndentLevel;

			serializedObject.ApplyModifiedProperties();
		}

		void OnDestroy()
		{
			if (!target && (object)_ParameterContainer != null && !Application.isPlaying)
			{
				_ParameterContainer.DestroySubComponents();
			}
		}

		public override bool UseDefaultMargins()
		{
			return !isInParametersPanel;
		}
	}
}
