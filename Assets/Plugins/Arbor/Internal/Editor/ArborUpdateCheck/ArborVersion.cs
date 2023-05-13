//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace ArborEditor.UpdateCheck
{
	[InitializeOnLoad]
	public sealed class ArborVersion : ScriptableObject
	{
		static ArborVersion s_Instance = null;

		public static ArborVersion instance
		{
			get
			{
				if (!FindInstance())
				{
					CreateInstance();
				}
				return s_Instance;
			}
		}

		public static VersionInfo.BuildType buildType
		{
			get
			{
				return instance._VersionInfo.buildType;
			}
		}

		public static string version
		{
			get
			{
				return instance._VersionInfo.version;
			}
		}

		public static string documentVersion
		{
			get
			{
				return instance._VersionInfo.documentVersion;
			}
		}

		public static string updateCheckURL
		{
			get
			{
				string majorVersion = instance._VersionInfo.version.Split('.')[0];
#if ARBOR_DEBUG
				return string.Format("https://caitsithware.com/assets/arbor/updatecheck/{0}.x.dev.json", majorVersion);
#else
				return string.Format("https://caitsithware.com/assets/arbor/updatecheck/{0}.x.json", majorVersion);
#endif
			}
		}

		public static string baseVersion
		{
			get
			{
				return instance._VersionInfo.baseVersion;
			}
		}

		public static bool isUpdateCheck
		{
			get
			{
				VersionInfo.BuildType buildType = ArborVersion.buildType;
				return (buildType == VersionInfo.BuildType.Release || buildType == VersionInfo.BuildType.Patch);
			}
		}

		public static string fullVersion
		{
			get
			{
				VersionInfo versionInfo = ArborVersion.instance._VersionInfo;
				return string.Format("{0} ({1})", versionInfo.version, versionInfo.buildType);
			}
		}

		public static void OpenAssetStore()
		{
			EditorGUITools.OpenAssetStore(instance._VersionInfo.storeURL);
		}

		[SerializeField]
		private VersionInfo _VersionInfo = new VersionInfo();

		[SerializeField]
		[HideInInspector]
		private long _LastWriteUnixTime = 0L;

		public System.Action onLoaded;

		static string filePath
		{
			get
			{
				return PathUtility.Combine(EditorResources.directory, "ArborVersion.json");
			}
		}



		static ArborVersion()
		{
			if (s_Instance == null)
			{
				EditorApplication.delayCall += () =>
				{
					FindInstance();
				};
			}
			else
			{
				s_Instance.LoadIfNecessary();
			}
		}

		static bool FindInstance()
		{
			if (s_Instance != null)
			{
				return true;
			}

			ArborVersion[] objects = Resources.FindObjectsOfTypeAll<ArborVersion>();
			if (objects == null || objects.Length == 0)
			{
				return false;
			}

			s_Instance = objects[0];
			s_Instance.LoadIfNecessary();

			return true;
		}

		static void CreateInstance()
		{
			if (s_Instance != null)
			{
				return;
			}

			s_Instance = ScriptableObject.CreateInstance<ArborVersion>();
			s_Instance.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable;
			s_Instance.Load();
		}

		void LoadIfNecessary()
		{
			if (File.Exists(filePath))
			{
				System.DateTime lastWriteTime = File.GetLastWriteTime(filePath);
				long lastWriteUnixTime = lastWriteTime.ToUnixTime();
				if (s_Instance._LastWriteUnixTime < lastWriteUnixTime)
				{
					s_Instance.Load();
				}
			}
		}

		public void Load()
		{
			TextAsset jsonAsset = EditorResources.Load<TextAsset>("ArborVersion", ".json");
			if (jsonAsset != null)
			{
				JsonUtility.FromJsonOverwrite(jsonAsset.text, _VersionInfo);
				string path = AssetDatabase.GetAssetPath(jsonAsset);
				System.DateTime lastWriteTime = File.GetLastWriteTime(path);
				_LastWriteUnixTime = lastWriteTime.ToUnixTime();

				onLoaded?.Invoke();
			}
		}

		public void Save()
		{
			string json = JsonUtility.ToJson(_VersionInfo, true);
			string path = filePath;
			using (StreamWriter writer = new StreamWriter(path))
			{
				Debug.Log("Save : " + path);
				writer.Write(json.ToString());
			}

			AssetDatabase.ImportAsset(path);
		}

		private sealed class PostProcessor : AssetPostprocessor
		{
			static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
			{
				if (s_Instance == null)
				{
					return;
				}

				if (importedAssets.Contains(filePath))
				{
					s_Instance.Load();
				}
			}
		}
	}
}
