//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Vector3にQuaternionを乗算する。
	/// </summary>
#else
	/// <summary>
	/// Multiply Vector3 by Quaternion.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.MulVector3")]
	[BehaviourTitle("Quaternion.MulVector3")]
	[BuiltInBehaviour]
	public sealed class QuaternionMulVector3Calculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Quaternion
		/// </summary>
		[SerializeField]
		private FlexibleQuaternion _Quaternion = new FlexibleQuaternion();

		/// <summary>
		/// Vector3
		/// </summary>
		[SerializeField]
		private FlexibleVector3 _Vector3 = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField]
		private OutputSlotVector3 _Result = new OutputSlotVector3();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(_Quaternion.value * _Vector3.value);
		}
	}
}
