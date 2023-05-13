//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.IO;
using Arbor;

namespace ArborEditor
{
	public class EditorSettings<T> : ScriptableObject where T : ScriptableObject
	{
		private static T _Instance;
		public static T instance
		{
			get
			{
				InitIfNeeded();
				return _Instance;
			}
		}

		public static bool isLoaded
		{
			get
			{
				return _Instance != null;
			}
		}

		protected static void InitIfNeeded()
		{
			if (_Instance == null)
			{
				T[] objects = Resources.FindObjectsOfTypeAll<T>();
				if (objects != null && objects.Length > 0)
				{
					_Instance = objects[0];
				}
			}

			if (_Instance == null)
			{
				CreateAndLoad();
			}
		}

		protected EditorSettings()
		{
			_Instance = this as T;
		}

		void OnEnable()
		{
			hideFlags = HideFlags.HideAndDontSave;
		}

		static void CreateAndLoad()
		{
			string filePath = GetFilePath();
			if (!string.IsNullOrEmpty(filePath))
			{
				InternalEditorUtility.LoadSerializedFileAndForget(filePath);
			}
			if (_Instance != null)
			{
				return;
			}

			ScriptableObject.CreateInstance<T>();
		}

		private static bool _DelaySave = false;

		private static void DoSave()
		{
			string filePath = GetFilePath();
			if (string.IsNullOrEmpty(filePath))
			{
				return;
			}

			string directoryName = Path.GetDirectoryName(filePath);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			InternalEditorUtility.SaveToSerializedFileAndForget(new UnityEngine.Object[] { _Instance }, filePath, true);

			_DelaySave = false;
		}

		private static void OnDelaySave()
		{
			if (_DelaySave)
			{
				DoSave();
			}
		}

		protected static void Save()
		{
			if (!_DelaySave)
			{
				_DelaySave = true;
				EditorApplication.delayCall += OnDelaySave;
			}
		}

		static string GetFilePath()
		{
			FilePathAttribute filePathAttribute = AttributeHelper.GetAttribute<FilePathAttribute>(typeof(T));
			if (filePathAttribute != null)
			{
				return filePathAttribute.filepath;
			}
			return null;
		}
	}
}
