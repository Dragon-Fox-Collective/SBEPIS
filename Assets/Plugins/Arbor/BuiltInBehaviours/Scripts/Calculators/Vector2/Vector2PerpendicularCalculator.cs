//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 指定したVector2に垂直なVector2を返す<br />
	/// 結果は、正のY軸が上がる2D座標系の場合、常に反時計回りに90度回転します。
	/// </summary>
#else
	/// <summary>
	/// Returns Vector2 perpendicular to the specified Vector2<br />
	/// The result is always rotated 90-degrees in a counter-clockwise direction for a 2D coordinate system where the positive Y axis goes up.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2/Vector2.Perpendicular")]
	[BehaviourTitle("Vector2.Perpendicular")]
	[BuiltInBehaviour]
	public sealed class Vector2PerpendicularCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 入射ベクトル
		/// </summary>
#else
		/// <summary>
		/// Incident vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _InDirection = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector2 _Result = new OutputSlotVector2();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Result.SetValue(Vector2.Perpendicular(_InDirection.value));
		}
	}
}
