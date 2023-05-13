using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace ArborEditor
{
	internal sealed class LanguageImporter : AssetPostprocessor
	{
		static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
		{
			bool rebuild = false;

			for (int importIndex = 0; importIndex < importedAssets.Length; importIndex++)
			{
				string importAssetPath = importedAssets[importIndex];
				if (!importAssetPath.StartsWith("Assets/", System.StringComparison.Ordinal))
				{
					continue;
				}

				string directory = PathUtility.GetDirectoryName(importAssetPath);
				string languageName = Path.GetFileNameWithoutExtension(importAssetPath);
				string ext = Path.GetExtension(importAssetPath);

				if (ext == ".txt" && LanguageManager.Contains(directory))
				{
					SystemLanguage language;
					rebuild = EnumUtility.TryParse<SystemLanguage>(languageName, out language);
				}
				else
				{
					LanguagePathInternal languagePath = AssetDatabase.LoadAssetAtPath<LanguagePathInternal>(importAssetPath);
					if (languagePath != null)
					{
						languagePath.Setup();
						LanguageManager.AddLanguagePath(languagePath);
						rebuild = true;
					}
				}
			}

			if (deletedAssets != null && deletedAssets.Length > 0)
			{
				rebuild = true;
			}

			if (rebuild)
			{
				LanguageManager.Rebuild();
			}
		}
	}
}