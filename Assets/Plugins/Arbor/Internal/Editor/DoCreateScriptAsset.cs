//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ArborEditor
{
	public sealed class DoCreateScriptAsset : EndNameEditAction
	{
		static UnityEngine.Object CreateScriptAssetFromTemplate(string pathName, string template)
		{
			string fullPath = Path.GetFullPath(pathName);

			string input1 = template;

			string withoutExtension = Path.GetFileNameWithoutExtension(pathName);
			string str1 = Regex.Replace(withoutExtension, " ", string.Empty);
			string str2 = Regex.Replace(input1, "#SCRIPTNAME#", str1);

			UTF8Encoding utF8Encoding = new UTF8Encoding(true, false);

			bool append = false;
			StreamWriter streamWriter = new StreamWriter(fullPath, append, utF8Encoding);
			streamWriter.Write(str2);
			streamWriter.Close();

			AssetDatabase.ImportAsset(pathName);
			return AssetDatabase.LoadAssetAtPath(pathName, typeof(UnityEngine.Object));
		}

		public override void Action(int instanceId, string pathName, string resourceFile)
		{
			string template = "";

			TextAsset templateAsset = EditorResources.Load<TextAsset>(PathUtility.Combine("ScriptTemplates", resourceFile), ".txt");
			if (templateAsset != null)
			{
				template = templateAsset.text;
			}

			ProjectWindowUtil.ShowCreatedAsset(CreateScriptAssetFromTemplate(pathName, template));
		}
	}
}
