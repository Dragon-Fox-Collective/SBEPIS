//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 衝突したCollider2DまたはRigidbody2DのTransform
	/// </summary>
#else
	/// <summary>
	/// The Transform of the Rigidbody2D or Collider2D that was hit.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit2D/RaycastHit2D.Transform")]
	[BehaviourTitle("RaycastHit2D.Transform")]
	[BuiltInBehaviour]
	public sealed class RaycastHit2DTransformCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit2D
		/// </summary>
		[SerializeField] private InputSlotRaycastHit2D _RaycastHit2D = new InputSlotRaycastHit2D();

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
			RaycastHit2D raycastHit2D = new RaycastHit2D();
			if (_RaycastHit2D.GetValue(ref raycastHit2D))
			{
				_Transform.SetValue(raycastHit2D.transform);
			}
		}
	}
}
