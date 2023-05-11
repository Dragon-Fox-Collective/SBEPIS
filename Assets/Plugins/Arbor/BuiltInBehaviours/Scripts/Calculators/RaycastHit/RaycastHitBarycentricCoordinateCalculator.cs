//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 衝突したメッシュの三角形における重心座標
	/// </summary>
#else
	/// <summary>
	/// The barycentric coordinate of the triangle we hit.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit/RaycastHit.BarycentricCoordinate")]
	[BehaviourTitle("RaycastHit.BarycentricCoordinate")]
	[BuiltInBehaviour]
	public sealed class RaycastHitBarycentricCoordinateCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit
		/// </summary>
		[SerializeField] private InputSlotRaycastHit _RaycastHit = new InputSlotRaycastHit();

#if ARBOR_DOC_JA
		/// <summary>
		/// 重心座標を出力
		/// </summary>
#else
		/// <summary>
		/// Output barycentric coordinate
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _BarycentricCoordinate = new OutputSlotVector3();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit raycastHit = new RaycastHit();
			if (_RaycastHit.GetValue(ref raycastHit))
			{
				_BarycentricCoordinate.SetValue(raycastHit.barycentricCoordinate);
			}
		}
	}
}
