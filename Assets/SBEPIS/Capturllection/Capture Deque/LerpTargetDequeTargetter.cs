using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturllection
{
	public class LerpTargetDequeTargetter : MonoBehaviour
	{
		public CouplingSocket socket;
		
		public void Couple(LerpTargetAnimator animator)
		{
			CouplingPlug plug = animator.GetComponent<CouplingPlug>();
			if (!plug)
				return;
			
			socket.Couple(plug);
		}
	}
}
