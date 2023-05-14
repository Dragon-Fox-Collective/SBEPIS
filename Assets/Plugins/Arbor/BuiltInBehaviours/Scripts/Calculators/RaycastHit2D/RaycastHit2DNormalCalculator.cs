//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// レイにより衝突された表面の法線ベクトル
	/// </summary>
#else
	/// <summary>
	/// The normal vector of the surface hit by the ray.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit2D/RaycastHit2D.Normal")]
	[BehaviourTitle("RaycastHit2D.Normal")]
	[BuiltInBehaviour]
	public sealed class RaycastHit2DNormalCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit2D
		/// </summary>
		[SerializeField] private InputSlotRaycastHit2D _RaycastHit2D = new InputSlotRaycastHit2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// 法線を出力
		/// </summary>
#else
		/// <summary>
		/// Output the normal
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Normal = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit2D raycastHit2D = new RaycastHit2D();
			if (_RaycastHit2D.GetValue(ref raycastHit2D))
			{
				_Normal.SetValue(raycastHit2D.normal);
			}
		}
	}
}
