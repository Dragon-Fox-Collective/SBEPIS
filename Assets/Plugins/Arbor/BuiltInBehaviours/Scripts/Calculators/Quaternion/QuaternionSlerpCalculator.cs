//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// FromとToの間でQuaternionを球面線形補間する。
	/// </summary>
#else
	/// <summary>
	/// Spherical linear interpolation of Quaternion between From and To.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.Slerp")]
	[BehaviourTitle("Quaternion.Slerp")]
	[BuiltInBehaviour]
	public sealed class QuaternionSlerpCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始Quaternion
		/// </summary>
#else
		/// <summary>
		/// Starting quaternion
		/// </summary>
#endif
		[SerializeField] private FlexibleQuaternion _From = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// 終了Quaternion
		/// </summary>
#else
		/// <summary>
		/// End quaternion
		/// </summary>
#endif
		[SerializeField] private FlexibleQuaternion _To = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// 補間パラメータ<br />
		/// [0, 1]の間にクランプされる。
		/// </summary>
#else
		/// <summary>
		/// The interpolation parameter<br />
		/// Clamped between [0, 1].
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _T = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Result output
		/// </summary>
#endif
		[SerializeField] private OutputSlotQuaternion _Result = new OutputSlotQuaternion();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			_Result.SetValue(Quaternion.Slerp(_From.value, _To.value, _T.value));
		}
	}
}
