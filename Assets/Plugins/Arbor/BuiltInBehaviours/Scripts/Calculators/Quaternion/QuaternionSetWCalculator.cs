//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// QuaternionのW成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the W component of Quaternion.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.SetW")]
	[BehaviourTitle("Quaternion.SetW")]
	[BuiltInBehaviour]
	public sealed class QuaternionSetWCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Quaternion
		/// </summary>
		[SerializeField] private FlexibleQuaternion _Quaternion = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// W成分
		/// </summary>
#else
		/// <summary>
		/// W component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _W = new FlexibleFloat();

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
			value.w = _W.value;
			_Result.SetValue(value);
		}
	}
}
