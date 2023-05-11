//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 衝突したライトマップのUV座標
	/// </summary>
#else
	/// <summary>
	/// The uv lightmap coordinate at the impact point.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("RaycastHit/RaycastHit.LightmapCoord")]
	[BehaviourTitle("RaycastHit.LightmapCoord")]
	[BuiltInBehaviour]
	public sealed class RaycastHitLightmapCoordCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// RaycastHit
		/// </summary>
		[SerializeField] private InputSlotRaycastHit _RaycastHit = new InputSlotRaycastHit();

#if ARBOR_DOC_JA
		/// <summary>
		/// ライトマップ座標を出力
		/// </summary>
#else
		/// <summary>
		/// Output the lightmap coordinate
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _LightmapCoord = new OutputSlotVector2();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			RaycastHit raycastHit = new RaycastHit();
			if (_RaycastHit.GetValue(ref raycastHit))
			{
				_LightmapCoord.SetValue(raycastHit.lightmapCoord);
			}
		}
	}
}
