//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// キャストに使用されるプリミティブの重心
	/// </summary>
#else
	/// <summary>
	/// The centroid of the primitive used to perform the cast.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit2D/RaycastHit2D.Centroid")]
	[BehaviourTitle("RaycastHit2D.Centroid")]
	[BuiltInBehaviour]
	public sealed class RaycastHit2DCentroidCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit2D
		/// </summary>
		[SerializeField] private InputSlotRaycastHit2D _RaycastHit2D = new InputSlotRaycastHit2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// 重心座標を出力
		/// </summary>
#else
		/// <summary>
		/// Output barycentric coordinate
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Centroid = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit2D raycastHit2D = new RaycastHit2D();
			if (_RaycastHit2D.GetValue(ref raycastHit2D))
			{
				_Centroid.SetValue(raycastHit2D.centroid);
			}
		}
	}
}
