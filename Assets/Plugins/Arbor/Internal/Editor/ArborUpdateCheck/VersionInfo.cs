//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine.Serialization;

namespace ArborEditor.UpdateCheck
{
	[System.Serializable]
	public sealed class VersionInfo
	{
		public enum BuildType
		{
			Release,
			Patch,
			Trial,
			Beta,
		}

		public BuildType buildType = BuildType.Release;
		public string version = string.Empty;
		public string baseVersion = string.Empty;
		public string storeURL = string.Empty;

		[FormerlySerializedAs("documentVarsion")]
		public string documentVersion = string.Empty;
	}
}