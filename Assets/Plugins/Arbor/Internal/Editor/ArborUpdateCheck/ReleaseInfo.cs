//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEditor;

namespace ArborEditor.UpdateCheck
{
	[System.Serializable]
	public sealed class ReleaseInfo
	{
		public string Version = string.Empty;
		public string StoreURL = string.Empty;
		public LocalizationText Message = new LocalizationText();
		public LocalizationText ReleaseNote = new LocalizationText();

		public bool IsValid()
		{
			return !string.IsNullOrEmpty(Version);
		}

		public void OpenAssetStore()
		{
			EditorGUITools.OpenAssetStore(StoreURL);
		}

		public void OpenReleaseNote()
		{
			Help.BrowseURL(ReleaseNote.GetText());
		}

		public override string ToString()
		{
			if (!IsValid())
			{
				return "Invalid";
			}

			return string.Format("Version : {0}\nStoreURL : {1}\nReleaseNode : \n{2}", Version, StoreURL, ReleaseNote.ToString());
		}
	}
}
