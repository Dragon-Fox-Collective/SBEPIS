//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// オイラー角からQuaternionを計算する。
	/// </summary>
#else
	/// <summary>
	/// Calculate the Quaternion value from Euler angle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.Euler")]
	[BehaviourTitle("Quaternion.Euler")]
	[BuiltInBehaviour]
	public sealed class QuaternionEulerCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// オイラー角
		/// </summary>
#else
		/// <summary>
		/// Euler angle
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Euler = new FlexibleVector3();

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
			_Result.SetValue(Quaternion.Euler(_Euler.value));
		}
	}
}
