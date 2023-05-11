//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Axisの周りをAngle度回転するQuaternionを作成する。
	/// </summary>
#else
	/// <summary>
	/// Create a Quaternion that rotates Angle degrees around Axis.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.AngleAxis")]
	[BehaviourTitle("Quaternion.AngleAxis")]
	[BuiltInBehaviour]
	public sealed class QuaternionAngleAxisCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 角度
		/// </summary>
#else
		/// <summary>
		/// Angle
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Angle = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 回転軸
		/// </summary>
#else
		/// <summary>
		/// Axis of rotation
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Axis = new FlexibleVector3();

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
			_Result.SetValue(Quaternion.AngleAxis(_Angle.value, _Axis.value));
		}
	}
}
