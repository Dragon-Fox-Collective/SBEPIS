//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// QuaternionのZ成分を設定する。
	/// </summary>
#else
	/// <summary>
	/// Sets the Z component of Quaternion.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.SetZ")]
	[BehaviourTitle("Quaternion.SetZ")]
	[BuiltInBehaviour]
	public sealed class QuaternionSetZCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Quaternion
		/// </summary>
		[SerializeField] private FlexibleQuaternion _Quaternion = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z成分
		/// </summary>
#else
		/// <summary>
		/// Z component
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Z = new FlexibleFloat();

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
			value.z = _Z.value;
			_Result.SetValue(value);
		}
	}
}
