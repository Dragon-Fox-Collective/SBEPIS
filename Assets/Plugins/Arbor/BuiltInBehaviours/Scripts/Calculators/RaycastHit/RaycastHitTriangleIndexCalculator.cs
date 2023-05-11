//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 衝突したメッシュの三角形におけるインデックス
	/// </summary>
#else
	/// <summary>
	/// The index of the triangle that was hit.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit/RaycastHit.TriangleIndex")]
	[BehaviourTitle("RaycastHit.TriangleIndex")]
	[BuiltInBehaviour]
	public sealed class RaycastHitTriangleIndexCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit
		/// </summary>
		[SerializeField] private InputSlotRaycastHit _RaycastHit = new InputSlotRaycastHit();

#if ARBOR_DOC_JA
		/// <summary>
		/// 当たった三角形インデックスを出力
		/// </summary>
#else
		/// <summary>
		/// Output the hit triangle index
		/// </summary>
#endif
		[SerializeField] private OutputSlotInt _TriangleIndex = new OutputSlotInt();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit raycastHit = new RaycastHit();
			if (_RaycastHit.GetValue(ref raycastHit))
			{
				_TriangleIndex.SetValue(raycastHit.triangleIndex);
			}
		}
	}
}
