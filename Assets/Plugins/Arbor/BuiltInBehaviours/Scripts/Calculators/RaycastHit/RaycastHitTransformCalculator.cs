//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 衝突したColliderまたはRigidbodyのTransform
	/// </summary>
#else
	/// <summary>
	/// The Transform of the rigidbody or collider that was hit.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit/RaycastHit.Transform")]
	[BehaviourTitle("RaycastHit.Transform")]
	[BuiltInBehaviour]
	public sealed class RaycastHitTransformCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit
		/// </summary>
		[SerializeField] private InputSlotRaycastHit _RaycastHit = new InputSlotRaycastHit();

#if ARBOR_DOC_JA
		/// <summary>
		/// 当たったTransformを出力
		/// </summary>
#else
		/// <summary>
		/// Output the hitting Transform
		/// </summary>
#endif
		[SerializeField] private OutputSlotTransform _Transform = new OutputSlotTransform();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit raycastHit = new RaycastHit();
			if (_RaycastHit.GetValue(ref raycastHit))
			{
				_Transform.SetValue(raycastHit.transform);
			}
		}
	}
}
