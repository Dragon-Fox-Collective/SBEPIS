using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	[InitializeOnLoad]
	internal static class LanguageManager
	{
		private sealed class WordDictionary : Dictionary<string, GUIContent>
		{
		}

		private sealed class SystemLanguageCompare : IComparer<SystemLanguage>
		{
			public int Compare(SystemLanguage x, SystemLanguage y)
			{
				return ((int)x).CompareTo((int)y);
			}
		}

		private static List<LanguagePathInternal> _LanguagePaths = new List<LanguagePathInternal>();
		private static SortedDictionary<SystemLanguage, WordDictionary> _LanguageDics = new SortedDictionary<SystemLanguage, WordDictionary>(new SystemLanguageCompare());
		private static List<SystemLanguage> _Languages = new List<SystemLanguage>();

		public static event System.Action onRebuild;

		const string k_DirectoryName = "Languages";

		public static string languageDirectory
		{
			get
			{
				return PathUtility.Combine(EditorResources.directory, k_DirectoryName);
			}
		}

		public static int languageCount => _Languages.Count;

		static LanguageManager()
		{
			Initialize();
		}

		static void Initialize()
		{
			var assets = AssetDatabase.FindAssets("t:ArborEditor.LanguagePath");
			for (int i = 0; i < assets.Length; i++)
			{
				string guid = assets[i];
				string path = AssetDatabase.GUIDToAssetPath(guid);
				LanguagePathInternal languagePath = AssetDatabase.LoadAssetAtPath<LanguagePathInternal>(path);
				if (languagePath != null)
				{
					languagePath.Setup();
					AddLanguagePath(languagePath);
				}
			}

			Rebuild();
		}

		public static SystemLanguage GetLanguageAt(int index)
		{
			return _Languages[index];
		}

		public static bool Contains(string path)
		{
			if (path == languageDirectory)
			{
				return true;
			}

			for (int pathIndex = 0; pathIndex < _LanguagePaths.Count; pathIndex++)
			{
				LanguagePathInternal languagePath = _LanguagePaths[pathIndex];
				if (path == languagePath.path)
				{
					return true;
				}
			}

			return false;
		}

		public static void Rebuild()
		{
			for (int i = _LanguagePaths.Count - 1; i >= 0; i--)
			{
				LanguagePathInternal languagePath = _LanguagePaths[i];
				if (languagePath == null)
				{
					_LanguagePaths.RemoveAt(i);
				}
			}

			_Languages.Clear();
			_LanguageDics.Clear();

			var languages = EnumUtility.GetValues<SystemLanguage>();
			for (int languageIndex = 0; languageIndex < languages.Length; languageIndex++)
			{
				SystemLanguage language = languages[languageIndex];
				Load(language);
			}

			onRebuild?.Invoke();
		}

		internal static void AddLanguagePath(LanguagePathInternal languagePath)
		{
			if (!_LanguagePaths.Contains(languagePath))
			{
				_LanguagePaths.Add(languagePath);
			}
		}

		static T LoadAssetAtPath<T>(string path, string ext) where T : Object
		{
			T obj = AssetDatabase.LoadAssetAtPath<T>(path);
			if (obj != null)
			{
				return obj;
			}
			return AssetDatabase.LoadAssetAtPath<T>(Path.ChangeExtension(path, ext));
		}

		static void Load(SystemLanguage language)
		{
			if (_LanguageDics.ContainsKey(language))
			{
				return;
			}

			WordDictionary wordDic = new WordDictionary();

			string languageName = language.ToString();

			string path = PathUtility.Combine(k_DirectoryName, languageName);

			// load default words
			TextAsset languageAsset = EditorResources.Load<TextAsset>(path, ".txt");
			if (languageAsset != null)
			{
				Load(wordDic, languageAsset.text);
			}

			// sort language path order
			_LanguagePaths.Sort((a, b) =>
			{
				return a.order.CompareTo(b.order);
			});

			// load additional words
			for (int pathIndex = 0; pathIndex < _LanguagePaths.Count; pathIndex++)
			{
				LanguagePathInternal languagePath = _LanguagePaths[pathIndex];
				path = languagePath.path;
				if (string.IsNullOrEmpty(path))
				{
					continue;
				}
				path = PathUtility.Combine(path, languageName);

				languageAsset = LoadAssetAtPath<TextAsset>(path, ".txt");
				if (languageAsset != null)
				{
					Load(wordDic, languageAsset.text);
				}
			}

			if (wordDic.Count > 0)
			{
				_LanguageDics.Add(language, wordDic);
				_Languages.Add(language);
			}
		}

		static void Load(WordDictionary wordDic, string text)
		{
			var lines = text.Split('\n');
			for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++)
			{
				string line = lines[lineIndex];
				if (line.StartsWith("//", System.StringComparison.Ordinal))
				{
					continue;
				}

				int firstColonIndex = line.IndexOf(':');
				if (firstColonIndex < 0)
				{
					continue;
				}

				string key = line.Substring(0, firstColonIndex);
				string word = line.Substring(firstColonIndex + 1).Trim().Replace("\\n", "\n");

				wordDic[key] = new GUIContent(word);
			}
		}

		public static GUIContent GetTextContent(SystemLanguage language, string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return GUIContent.none;
			}

			WordDictionary wordDic = null;
			if (_LanguageDics.TryGetValue(language, out wordDic))
			{
				GUIContent content;
				if (wordDic.TryGetValue(key, out content))
				{
					return content;
				}
			}

			if (language != SystemLanguage.English)
			{
				return GetTextContent(SystemLanguage.English, key);
			}

			return GUIContentCaches.Get(key);
		}

		public static string GetWord(SystemLanguage language, string key)
		{
			return GetTextContent(language, key).text;
		}

		public static bool ContainsLanguage(SystemLanguage language)
		{
			return _LanguageDics.ContainsKey(language);
		}

		public static SystemLanguage GetSystemLanguage()
		{
			SystemLanguage language = Application.systemLanguage;
			if (!ContainsLanguage(language))
			{
				language = SystemLanguage.English;
			}

			return language;
		}

		public static SystemLanguage GetEditorLanguage()
		{
			SystemLanguage language = UnityEditorBridge.LocalizationDatabaseBridge.currentEditorLanguage;
			if (!ContainsLanguage(language))
			{
				language = SystemLanguage.English;
			}

			return language;
		}
	}
}