//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.UpdateCheck
{
	[System.Serializable]
	public sealed class PatchInfo
	{
		public string Version = string.Empty;
		public string BaseVersion = string.Empty;
		public LocalizationText Message = new LocalizationText();
		public LocalizationText DownloadURL = new LocalizationText();

		public bool IsValid()
		{
			return !string.IsNullOrEmpty(Version);
		}

		public void OpenDownalodPage()
		{
			Help.BrowseURL(DownloadURL.GetText());
		}

		public override string ToString()
		{
			if (!IsValid())
			{
				return "Invalid";
			}

			return string.Format("Version : {0}\nTargetVersion : {1}\nDownloadURL : \n{2}", Version, BaseVersion, DownloadURL.ToString());
		}
	}
}