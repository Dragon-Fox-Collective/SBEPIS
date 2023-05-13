//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace ArborEditor.UpdateCheck
{
	[System.Serializable]
	internal sealed class UpdateInfo
	{
		public ReleaseInfo Release = new ReleaseInfo();
		public ReleaseInfo Upgrade = new ReleaseInfo();
		public PatchInfo Patch = new PatchInfo();

		public override string ToString()
		{
			return string.Format("##Release\n{0}\n\n##Upgrade\n{1}\n\n##Patch\n{2}", Release.ToString(), Upgrade.ToString(), Patch.ToString());
		}
	}
}