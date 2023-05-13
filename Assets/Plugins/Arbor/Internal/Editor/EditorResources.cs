//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
#if ARBOR_DLL
using System.Reflection;
#else
using UnityEditor;
#endif
using System.IO;

namespace ArborEditor
{
	public sealed class EditorResources : Arbor.ScriptableSingleton<EditorResources>
	{
		const string k_DirectoryName = "EditorResources";

		private IAssetLoader _AssetLoader;
		private IAssetLoader loader
		{
			get
			{
				if (_AssetLoader == null)
				{
#if ARBOR_DLL
					Assembly assembly = Assembly.GetExecutingAssembly();
					string directory = Path.GetDirectoryName(assembly.Location);
					_Directory = PathUtility.Combine(directory, k_DirectoryName);
					_Directory = _Directory.Replace(Application.dataPath, "Assets");
					_AssetLoader = new AssetDatabaseLoader( _Directory );

					string arborDirectoryName = "/Arbor/";
					int index = _Directory.LastIndexOf(arborDirectoryName);
					if (index >= 0)
					{
						_ArborRootDirectory = _Directory.Substring(0, index + arborDirectoryName.Length);
					}
#else
					MonoScript script = MonoScript.FromScriptableObject(this);

					if (script != null)
					{
						string directory = Path.GetDirectoryName(AssetDatabase.GetAssetPath(script));
						_Directory = PathUtility.Combine(directory, k_DirectoryName);
						_AssetLoader = new AssetDatabaseLoader(_Directory);

						string arborDirectoryName = "/Arbor/";
						int index = _Directory.LastIndexOf(arborDirectoryName, System.StringComparison.Ordinal);
						if (index >= 0)
						{
							_ArborRootDirectory = _Directory.Substring(0, index + arborDirectoryName.Length);
						}
					}
#endif
				}

				return _AssetLoader;
			}
		}

		private string _ArborRootDirectory;
		public static string arborRootDirectory
		{
			get
			{
				return instance._ArborRootDirectory;
			}
		}

		private string _Directory;

		public static string directory
		{
			get
			{
				return instance._Directory;
			}
		}

		public static Object Load(string name, System.Type type)
		{
			IAssetLoader loader = instance.loader;
			if (loader == null)
			{
				return null;
			}

			return loader.Load(name, type);
		}

		public static T Load<T>(string name) where T : Object
		{
			IAssetLoader loader = instance.loader;
			if (loader == null)
			{
				return null;
			}

			return loader.Load<T>(name);
		}

		public static Object Load(string name, string ext, System.Type type)
		{
			IAssetLoader loader = instance.loader;
			if (loader == null)
			{
				return null;
			}

			Object obj = loader.Load(name, type);
			if (obj != null)
			{
				return obj;
			}
			return loader.Load(Path.ChangeExtension(name, ext), type);
		}

		public static T Load<T>(string name, string ext) where T : Object
		{
			IAssetLoader loader = instance.loader;
			if (loader == null)
			{
				return null;
			}

			T obj = loader.Load<T>(name);
			if (obj != null)
			{
				return obj;
			}
			return loader.Load<T>(Path.ChangeExtension(name, ext));
		}

		public static Texture2D LoadTexture(string name)
		{
			Texture2D tex = Load<Texture2D>(name);
			if (tex != null)
			{
				return tex;
			}
			tex = Load<Texture2D>(name + ".png");
			return tex;
		}
	}
}
