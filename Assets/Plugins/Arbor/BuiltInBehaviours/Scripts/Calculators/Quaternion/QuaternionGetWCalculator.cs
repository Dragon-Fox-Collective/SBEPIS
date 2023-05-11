//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// QuaternionのW成分を出力する。
	/// </summary>
#else
	/// <summary>
	/// Output the W component of Quaternion.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.GetW")]
	[BehaviourTitle("Quaternion.GetW")]
	[BuiltInBehaviour]
	public sealed class QuaternionGetWCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Quaternion
		/// </summary>
		[SerializeField] private FlexibleQuaternion _Quaternion = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// W成分の出力
		/// </summary>
#else
		/// <summary>
		/// Output W component
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _W = new OutputSlotFloat();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			_W.SetValue(_Quaternion.value.w);
		}
	}
}
