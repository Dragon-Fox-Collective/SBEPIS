//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Quaternionからオイラー角の値を計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculate the Euler angle value from Quaternion.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.EulerAngles")]
	[BehaviourTitle("Quaternion.EulerAngles")]
	[BuiltInBehaviour]
	public sealed class QuaternionEulerAnglesCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力値
		/// </summary>
#else
		/// <summary>
		/// Input value
		/// </summary>
#endif
		[SerializeField] private FlexibleQuaternion _Input = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Result = new OutputSlotVector3();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(_Input.value.eulerAngles);
		}
	}
}
