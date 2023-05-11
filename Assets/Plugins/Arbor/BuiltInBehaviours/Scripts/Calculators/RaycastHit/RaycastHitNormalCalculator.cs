//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 衝突した際の面の法線
	/// </summary>
#else
	/// <summary>
	/// The normal of the surface the ray hit.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit/RaycastHit.Normal")]
	[BehaviourTitle("RaycastHit.Normal")]
	[BuiltInBehaviour]
	public sealed class RaycastHitNormalCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit
		/// </summary>
		[SerializeField] private InputSlotRaycastHit _RaycastHit = new InputSlotRaycastHit();

#if ARBOR_DOC_JA
		/// <summary>
		/// 法線を出力
		/// </summary>
#else
		/// <summary>
		/// Output the normal
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Normal = new OutputSlotVector3();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit raycastHit = new RaycastHit();
			if (_RaycastHit.GetValue(ref raycastHit))
			{
				_Normal.SetValue(raycastHit.normal);
			}
		}
	}
}
