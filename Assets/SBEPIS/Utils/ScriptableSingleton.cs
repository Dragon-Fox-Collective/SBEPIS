using System;
using UnityEngine;

namespace SBEPIS.Utils
{
	public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
	{
		private static T _instance;

		public static T instance
		{
			get
			{
				if (!_instance)
					_instance = Load();

				return _instance;
			}
		}

		private static T Load()
		{
			T[] assets = Resources.LoadAll<T>("");

			if (assets.Length == 0)
				throw new ArgumentOutOfRangeException($"There are no {typeof(T).Name} files found");
			if (assets.Length > 1)
				throw new ArgumentOutOfRangeException($"There are too many {typeof(T).Name} files found");

			return assets[0];
		}
	}
}
