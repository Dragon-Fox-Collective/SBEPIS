//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// FromからToへのQuaternionを計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculates the Quaternion to To from From.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.RotateTowards")]
	[BehaviourTitle("Quaternion.RotateTowards")]
	[BuiltInBehaviour]
	public sealed class QuaternionRotateTowardsCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始Quaternion
		/// </summary>
#else
		/// <summary>
		/// Starting quaternion
		/// </summary>
#endif
		[SerializeField] private FlexibleQuaternion _From = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// 終了Quaternion
		/// </summary>
#else
		/// <summary>
		/// End quaternion
		/// </summary>
#endif
		[SerializeField] private FlexibleQuaternion _To = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大変化角度量
		/// </summary>
#else
		/// <summary>
		/// Maximum change angle amount
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _MaxDegreesDelta = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotQuaternion _Result = new OutputSlotQuaternion();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(Quaternion.RotateTowards(_From.value, _To.value, _MaxDegreesDelta.value));
		}
	}
}
