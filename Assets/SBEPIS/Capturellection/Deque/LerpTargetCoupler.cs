using KBCore.Refs;
using SBEPIS.Controller;
using SBEPIS.Utils;
using UnityEngine;

namespace SBEPIS.Capturellection
{
	public class LerpTargetCoupler : MonoBehaviour
	{
		[SerializeField, Self] private CouplingSocket socket;
		
		private void OnValidate() => this.ValidateRefs();
		
		public void Couple(LerpTargetAnimator animator)
		{
			socket.Couple(animator.GetComponent<CouplingPlug>());
		}
	}
}