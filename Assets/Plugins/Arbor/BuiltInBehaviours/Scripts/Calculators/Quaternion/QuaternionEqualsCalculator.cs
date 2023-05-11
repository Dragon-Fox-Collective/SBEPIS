//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 2つのクオータニオンがほぼ等しい場合にtrueを返す。
	/// </summary>
#else
	/// <summary>
	/// Returns true if two quaternions are approximately equal.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.Equals")]
	[BehaviourTitle("Quaternion.Equals")]
	[BuiltInBehaviour]
	public sealed class QuaternionEqualsCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 値1
		/// </summary>
#else
		/// <summary>
		/// Value 1
		/// </summary>
#endif
		[SerializeField] private FlexibleQuaternion _Value1 = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// 値2
		/// </summary>
#else
		/// <summary>
		/// Value 2
		/// </summary>
#endif
		[SerializeField] private FlexibleQuaternion _Value2 = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotBool _Result = new OutputSlotBool();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			Quaternion value1 = _Value1.value;
			Quaternion value2 = _Value2.value;
			_Result.SetValue(value1 == value2);
		}
	}
}
