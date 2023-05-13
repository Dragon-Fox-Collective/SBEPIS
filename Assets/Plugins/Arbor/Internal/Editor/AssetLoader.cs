//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	internal interface IAssetLoader
	{
		Object Load(string name, System.Type type);
		T Load<T>(string name) where T : Object;
	}

	internal sealed class AssetDatabaseLoader : IAssetLoader
	{
		private string _Path;

		public AssetDatabaseLoader(string path)
		{
			_Path = path;
		}

		public Object Load(string name, System.Type type)
		{
			string path = PathUtility.Combine(_Path, name);

			return AssetDatabase.LoadAssetAtPath(path, type);
		}

		public T Load<T>(string name) where T : Object
		{
			string path = PathUtility.Combine(_Path, name);

			return AssetDatabase.LoadAssetAtPath<T>(path);
		}
	}
}