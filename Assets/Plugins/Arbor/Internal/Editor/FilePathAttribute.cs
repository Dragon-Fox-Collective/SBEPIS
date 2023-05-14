//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditorInternal;
using System;

namespace ArborEditor
{
	[AttributeUsage(AttributeTargets.Class)]
	internal sealed class FilePathAttribute : Attribute
	{
		private string _RelativePath;
		private Location _Location;

		public string filepath
		{
			get
			{
				string filePath = _RelativePath;

				if (!string.IsNullOrEmpty(_RelativePath) && _Location == Location.PreferencesFolder)
				{
					filePath = InternalEditorUtility.unityPreferencesFolder + "/" + _RelativePath;
				}

				return filePath;
			}
		}

		public FilePathAttribute(string relativePath, Location location)
		{
			_RelativePath = relativePath;
			_Location = location;

			if (string.IsNullOrEmpty(_RelativePath))
			{
				Debug.LogError((object)"Invalid relative path! (its null or empty)");
			}
			else if (_RelativePath[0] == '/')
			{
				_RelativePath = _RelativePath.Substring(1);
			}
		}

		public enum Location
		{
			PreferencesFolder,
			ProjectFolder,
		}
	}
}
