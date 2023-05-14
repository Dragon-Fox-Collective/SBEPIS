//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// QuaternionのZ成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the Z component of Quaternion.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.GetZ")]
	[BehaviourTitle("Quaternion.GetZ")]
	[BuiltInBehaviour]
	public sealed class QuaternionGetZCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Quaternion
		/// </summary>
		[SerializeField] private FlexibleQuaternion _Quaternion = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// Z成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output Z component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Z = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Z.SetValue(_Quaternion.value.z);
		}
	}
}
