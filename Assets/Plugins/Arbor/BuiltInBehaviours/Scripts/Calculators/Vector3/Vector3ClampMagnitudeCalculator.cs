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
	[AddBehaviourMenu("Vector3/Vector3.ClampMagnitude")]
	[BehaviourTitle("Vector3.ClampMagnitude")]
	[BuiltInBehaviour]
	public sealed class Vector3ClampMagnitudeCalculator : Calculator
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
		[SerializeField] private FlexibleVector3 _Vector3 = new FlexibleVector3();

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
		[SerializeField] private OutputSlotVector3 _Result = new OutputSlotVector3();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Result.SetValue(Vector3.ClampMagnitude(_Vector3.value, _MaxLength.value));
		}
	}
}
