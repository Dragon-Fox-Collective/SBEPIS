//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 正規化したクオータニオンを返す。
	/// </summary>
#else
	/// <summary>
	/// Returns the normalized quaternion.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.Normalize")]
	[BehaviourTitle("Quaternion.Normalize")]
	[BuiltInBehaviour]
	public sealed class QuaternionNormalizeCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Quaternion
		/// </summary>
		[SerializeField] private FlexibleQuaternion _Quaternion = new FlexibleQuaternion();

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
			_Result.SetValue(Quaternion.Normalize(_Quaternion.value));
		}
	}
}
