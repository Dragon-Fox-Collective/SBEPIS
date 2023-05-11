//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// レイで衝突が発生した地点の、距離に対する割合
	/// </summary>
#else
	/// <summary>
	/// Fraction of the distance along the ray that the hit occurred.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit2D/RaycastHit2D.Fraction")]
	[BehaviourTitle("RaycastHit2D.Fraction")]
	[BuiltInBehaviour]
	public sealed class RaycastHit2DFractionCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit2D
		/// </summary>
		[SerializeField] private InputSlotRaycastHit2D _RaycastHit2D = new InputSlotRaycastHit2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// 距離に対する割合を出力
		/// </summary>
#else
		/// <summary>
		/// Output fraction of the distance
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Fraction = new OutputSlotFloat();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit2D raycastHit2D = new RaycastHit2D();
			if (_RaycastHit2D.GetValue(ref raycastHit2D))
			{
				_Fraction.SetValue(raycastHit2D.fraction);
			}
		}
	}
}
