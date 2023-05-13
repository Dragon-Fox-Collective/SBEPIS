using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	[FilePathAttribute("ProjectSettings/ArborEditorProjectSettings.asset", FilePathAttribute.Location.ProjectFolder)]
	public sealed class ArborEditorProjectSettings : EditorSettings<ArborEditorProjectSettings>
	{
		[SerializeField] AutoOpenWelcomeWindowMode _AutoOpenWelcomeWindow = AutoOpenWelcomeWindowMode.ChangedVersion;

		public static AutoOpenWelcomeWindowMode autoOpenWelcomeWindow
		{
			get
			{
				return instance._AutoOpenWelcomeWindow;
			}
			set
			{
				if (instance._AutoOpenWelcomeWindow != value)
				{
					instance._AutoOpenWelcomeWindow = value;

					Save();
				}
			}
		}
	}
}