using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ArborEditor
{
	public static class GUIContentCaches
	{
		private static Dictionary<string, GUIContent> _TextContents = new Dictionary<string, GUIContent>();

		public static GUIContent Get(string key)
		{
			if (string.IsNullOrEmpty(key))
			{
				return GUIContent.none;
			}

			GUIContent content = null;
			if (!_TextContents.TryGetValue(key, out content))
			{
				content = new GUIContent(key);
				_TextContents.Add(key, content);
			}

			return content;
		}
	}
}