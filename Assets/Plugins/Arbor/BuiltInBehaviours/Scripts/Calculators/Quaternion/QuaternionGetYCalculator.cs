//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// QuaternionのY成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the Y component of Quaternion.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.GetY")]
	[BehaviourTitle("Quaternion.GetY")]
	[BuiltInBehaviour]
	public sealed class QuaternionGetYCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Quaternion
		/// </summary>
		[SerializeField] private FlexibleQuaternion _Quaternion = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// Y成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output Y component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Y = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_Y.SetValue(_Quaternion.value.y);
		}
	}
}
