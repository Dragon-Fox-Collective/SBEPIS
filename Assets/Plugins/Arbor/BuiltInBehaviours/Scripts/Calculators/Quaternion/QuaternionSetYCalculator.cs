//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// QuaternionのY成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the Y component of Quaternion.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.SetY")]
	[BehaviourTitle("Quaternion.SetY")]
	[BuiltInBehaviour]
	public sealed class QuaternionSetYCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Quaternion
		/// </summary>
		[SerializeField] private FlexibleQuaternion _Quaternion = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y成分
		/// </summary>
#else
		/// <summary>
		/// Y component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Y = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotQuaternion _Result = new OutputSlotQuaternion();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			Quaternion value = _Quaternion.value;
			value.y = _Y.value;
			_Result.SetValue(value);
		}
	}
}
