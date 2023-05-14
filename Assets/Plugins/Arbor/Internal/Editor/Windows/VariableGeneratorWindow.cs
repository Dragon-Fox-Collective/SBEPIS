//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ArborEditor
{
	internal sealed class VariableGeneratorWindow : EditorWindow
	{
		private static VariableGeneratorWindow _Instance = null;

		public static VariableGeneratorWindow instance
		{
			get
			{
				if (_Instance == null)
				{
					VariableGeneratorWindow[] objects = Resources.FindObjectsOfTypeAll<VariableGeneratorWindow>();
					if (objects.Length > 0)
					{
						_Instance = objects[0];
					}
				}
				if (_Instance == null)
				{
					_Instance = CreateInstance<VariableGeneratorWindow>();
				}

				return _Instance;
			}
		}

		[MenuItem("Assets/Create/Arbor/Variable C# Script", false, 102)]
		static void OnMenu()
		{
			instance.Open(UnityEditorBridge.ProjectWindowUtilBridge.GetActiveFolderPath());
		}

		private string _Path;
		private string _VariableName;
		private bool _OpenEditor = true;
		private string _ErrorMessage = string.Empty;

		void Open(string path)
		{
			_Path = path;

			_VariableName = string.Empty;
			_ErrorMessage = GetErrorMessage(_Path, _VariableName);

			instance.ShowAuxWindow();
		}

		static string GetVariableClassName(string variableName)
		{
			return variableName + "Variable";
		}

		static string GetVariableListClassName(string variableName)
		{
			return variableName + "ListVariable";
		}

		static string GetFlexibleClassName(string variableName)
		{
			return "Flexible" + variableName;
		}

		static string GetInputSlotClassName(string variableName)
		{
			return "InputSlot" + variableName;
		}

		static string GetOutputSlotClassName(string variableName)
		{
			return "OutputSlot" + variableName;
		}

		static string GetVariableFileName(string variableName)
		{
			return GetVariableClassName(variableName) + ".cs";
		}

		static string GetVariableListFileName(string variableName)
		{
			return GetVariableListClassName(variableName) + ".cs";
		}

		static Object CreateVariable(string path, string variableName)
		{
			string template = string.Empty;
			TextAsset templateAsset = EditorResources.Load<TextAsset>(PathUtility.Combine("ScriptTemplates", "C# Script-NewVariableScript"), ".txt");
			if (templateAsset != null)
			{
				template = templateAsset.text;
			}

			string fileName = GetVariableFileName(variableName);

			string pathName = PathUtility.Combine(path, fileName);

			string fullPath = Path.GetFullPath(pathName);

			string scriptCode = Regex.Replace(template, "#VARIABLENAME#", variableName);

			UTF8Encoding utF8Encoding = new UTF8Encoding(true, false);

			bool append = false;
			StreamWriter streamWriter = new StreamWriter(fullPath, append, utF8Encoding);
			streamWriter.Write(scriptCode);
			streamWriter.Close();

			AssetDatabase.ImportAsset(pathName);

			return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
		}

		static Object CreateVariableList(string path, string variableName)
		{
			string template = string.Empty;
			TextAsset templateAsset = EditorResources.Load<TextAsset>(PathUtility.Combine("ScriptTemplates", "C# Script-NewVariableListScript"), ".txt");
			if (templateAsset != null)
			{
				template = templateAsset.text;
			}

			string fileName = GetVariableListFileName(variableName);

			string pathName = PathUtility.Combine(path, fileName);

			string fullPath = Path.GetFullPath(pathName);

			string scriptCode = Regex.Replace(template, "#VARIABLENAME#", variableName);

			UTF8Encoding utF8Encoding = new UTF8Encoding(true, false);

			bool append = false;
			StreamWriter streamWriter = new StreamWriter(fullPath, append, utF8Encoding);
			streamWriter.Write(scriptCode);
			streamWriter.Close();

			AssetDatabase.ImportAsset(pathName);
			return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
		}

		static string GetErrorMessage(string path, string variableName)
		{
			if (string.IsNullOrEmpty(variableName))
			{
				return "Please enter VariableName.";
			}
			else
			{
				char[] invalidChars = System.IO.Path.GetInvalidFileNameChars();
				string variableFileName = GetVariableFileName(variableName);
				if (variableFileName.IndexOfAny(invalidChars) >= 0)
				{
					return variableFileName + " is invalid file name.";
				}

				string variableFilePath = PathUtility.Combine(path, variableFileName);
				if (File.Exists(variableFilePath))
				{
					return variableFilePath + " File is already exists.";
				}

				string variableListFileName = GetVariableListFileName(variableName);
				if (variableListFileName.IndexOfAny(invalidChars) >= 0)
				{
					return variableListFileName + " is invalid file name.";
				}

				string variableListFilePath = PathUtility.Combine(path, variableListFileName);
				if (File.Exists(variableListFilePath))
				{
					return variableListFilePath + " File is already exists.";
				}

				if (!System.CodeDom.Compiler.CodeGenerator.IsValidLanguageIndependentIdentifier(variableName))
				{
					return variableName + " Class is invalid name.";
				}
				System.Type dataClassType = AssemblyHelper.GetTypeByName(variableName);
				if (dataClassType != null)
				{
					return variableName + " Class is already exists";
				}

				string variableClassName = GetVariableClassName(variableName);
				System.Type variableClassType = AssemblyHelper.GetTypeByName(variableClassName);
				if (variableClassType != null)
				{
					return variableClassName + " Class is already exists";
				}

				string flexibleClassName = GetFlexibleClassName(variableName);
				System.Type flexibleClassType = AssemblyHelper.GetTypeByName(flexibleClassName);
				if (flexibleClassType != null)
				{
					return flexibleClassName + " Class is already exists";
				}

				string inputSlotClassName = GetInputSlotClassName(variableName);
				System.Type inputSlotClassType = AssemblyHelper.GetTypeByName(inputSlotClassName);
				if (inputSlotClassType != null)
				{
					return inputSlotClassName + " Class is already exists";
				}

				string outputSlotClassName = GetOutputSlotClassName(variableName);
				System.Type outputClassType = AssemblyHelper.GetTypeByName(outputSlotClassName);
				if (outputClassType != null)
				{
					return outputSlotClassName + " Class is already exists";
				}

				string variableListClassName = GetVariableListClassName(variableName);
				System.Type variableListClassType = AssemblyHelper.GetTypeByName(variableListClassName);
				if (variableListClassType != null)
				{
					return variableListClassName + " Class is already exists";
				}
			}

			return string.Empty;
		}

		private void OnEnable()
		{
			titleContent = GUIContentCaches.Get("Variable Generator");
		}

		private void OnGUI()
		{
			EditorGUI.BeginChangeCheck();
			_VariableName = EditorGUILayout.TextField(GUIContentCaches.Get("Variable Name"), _VariableName);
			if (EditorGUI.EndChangeCheck())
			{
				_ErrorMessage = GetErrorMessage(_Path, _VariableName);
			}

			if (!string.IsNullOrEmpty(_ErrorMessage))
			{
				EditorGUILayout.HelpBox(_ErrorMessage, MessageType.Error);
			}

			GUILayout.FlexibleSpace();

			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.FlexibleSpace();
				_OpenEditor = EditorGUILayout.Toggle("OpenEditor", _OpenEditor, GUILayout.ExpandWidth(false));
			}
			using (new EditorGUILayout.HorizontalScope())
			{
				GUILayout.FlexibleSpace();
				using (new EditorGUI.DisabledScope(!string.IsNullOrEmpty(_ErrorMessage)))
				{
					if (GUILayout.Button("Create"))
					{
						Object script = CreateVariable(_Path, _VariableName);
						CreateVariableList(_Path, _VariableName);
						if (_OpenEditor)
						{
							AssetDatabase.OpenAsset(script);
						}
						Selection.activeObject = script;
					}
				}
			}

			if (Event.current.type == EventType.MouseDown)
			{
				GUIUtility.hotControl = GUIUtility.keyboardControl = 0;
				Event.current.Use();
			}
		}
	}
}