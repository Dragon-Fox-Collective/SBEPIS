//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// QuaternionのX成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the X component of Quaternion.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.GetX")]
	[BehaviourTitle("Quaternion.GetX")]
	[BuiltInBehaviour]
	public sealed class QuaternionGetXCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Quaternion
		/// </summary>
		[SerializeField] private FlexibleQuaternion _Quaternion = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// X成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output X component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _X = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_X.SetValue(_Quaternion.value.x);
		}
	}
}
