//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 大きさを MaxLength に制限したベクトルを計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculates a vector with its magnitude clamped to MaxLength.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector2/Vector2.ClampMagnitude")]
	[BehaviourTitle("Vector2.ClampMagnitude")]
	[BuiltInBehaviour]
	public sealed class Vector2ClampMagnitudeCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// ベクトル
		/// </summary>
#else
		/// <summary>
		/// Vector
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _Vector2 = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大の長さ
		/// </summary>
#else
		/// <summary>
		/// Max length
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _MaxLength = new FlexibleFloat();

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
			_Result.SetValue(Vector2.ClampMagnitude(_Vector2.value, _MaxLength.value));
		}
	}
}
