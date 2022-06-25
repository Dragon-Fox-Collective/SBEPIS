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
					_instance = Resources.Load<T>(typeof(T).Name);

				return _instance;
			}
		}

		protected ScriptableSingleton()
		{
			if (!_instance)
				_instance = this as T;
		}
	}
}
