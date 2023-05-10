using KBCore.Refs;
using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class LerpTargetCoupler : ValidatedMonoBehaviour
	{
		[SerializeField, Self] private CouplingSocket socket;
		
		public void Couple(LerpTargetAnimator animator)
		{
			socket.Couple(animator.GetComponent<CouplingPlug>());
		}
	}
}