using System;
using UnityEngine;

namespace SBEPIS.Utils
{
	public class ScriptableSingleton<T> : ScriptableObject where T : ScriptableObject
	{
		private static T instance;

		public static T Instance
		{
			get
			{
				if (!instance)
					instance = Load();

				return instance;
			}
		}

		private static T Load()
		{
			T[] assets = Resources.LoadAll<T>("");

			if (assets.Length == 0)
				throw new ArgumentException($"There are no {typeof(T).Name} files found");
			if (assets.Length > 1)
				throw new ArgumentException($"There are too many {typeof(T).Name} files found");

			return assets[0];
		}
	}
}
