//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 与えられた2つの角度（単位は度）間の最小の差を計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculates the shortest difference between two given angles given in degrees.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Mathf/Mathf.DeltaAngle")]
	[BehaviourTitle("Mathf.DeltaAngle")]
	[BuiltInBehaviour]
	public sealed class MathfDeltaAngleCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 現在値
		/// </summary>
#else
		/// <summary>
		/// Current value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Current = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 目標値
		/// </summary>
#else
		/// <summary>
		/// Target value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Target = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Result = new OutputSlotFloat();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(Mathf.DeltaAngle(_Current.value, _Target.value));
		}
	}
}
