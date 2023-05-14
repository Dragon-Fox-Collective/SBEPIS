//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Examples
{
	/// <summary>
	/// Behavior of the billboard. (General MonoBehaviour script)
	/// </summary>
	[AddComponentMenu("Arbor/Examples/Billboard")]
	public sealed class Billboard : MonoBehaviour
	{
		/// <summary>
		/// Transform cache
		/// </summary>
		private Transform _Trasnform;

		private void Start()
		{
			// Cache Transform.
			_Trasnform = transform;
		}

		void LateUpdate()
		{
			// Adjust to camera direction.
			_Trasnform.forward = Camera.main.transform.forward;
		}
	}
}