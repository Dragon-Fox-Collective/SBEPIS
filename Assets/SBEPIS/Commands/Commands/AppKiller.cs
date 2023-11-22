using UnityEngine;

namespace SBEPIS.Commands
{
	public class AppKiller : MonoBehaviour
	{
		public void Kill(bool kill)
		{
			if (kill)
			{
#if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
#endif
				Application.Quit();
			}
		}
	}
}
