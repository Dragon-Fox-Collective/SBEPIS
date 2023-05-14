//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ForwardへのQuaternionを作成する。
	/// </summary>
#else
	/// <summary>
	/// Create a Quaternion to Forward.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.LookRotation")]
	[BehaviourTitle("Quaternion.LookRotation")]
	[BuiltInBehaviour]
	public sealed class QuaternionLookRotationCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 前方向
		/// </summary>
#else
		/// <summary>
		/// Forward
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _Forward = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 上方向
		/// </summary>
#else
		/// <summary>
		/// Upward
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _Upwards = new FlexibleVector3(Vector3.up);

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
		private OutputSlotQuaternion _Result = new OutputSlotQuaternion();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(Quaternion.LookRotation(_Forward.value, _Upwards.value));
		}
	}
}
