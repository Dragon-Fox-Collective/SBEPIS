//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Quaternionを回転軸と角度に変換する。
	/// </summary>
#else
	/// <summary>
	/// Convert Quaternion to rotation axis and angle.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Quaternion/Quaternion.ToAngleAxis")]
	[BehaviourTitle("Quaternion.ToAngleAxis")]
	[BuiltInBehaviour]
	public sealed class QuaternionToAngleAxisCalculator : Calculator
	{
		#region Serialize fields

		/// <summary>
		/// Quaternion
		/// </summary>
		[SerializeField] private FlexibleQuaternion _Quaternion = new FlexibleQuaternion();

#if ARBOR_DOC_JA
		/// <summary>
		/// 角度を出力
		/// </summary>
#else
		/// <summary>
		/// Output angle
		/// </summary>
#endif
		[SerializeField] private OutputSlotFloat _Angle = new OutputSlotFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 回転軸を出力
		/// </summary>
#else
		/// <summary>
		/// Output rotation axis
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Axis = new OutputSlotVector3();

		#endregion // Serialize fields

		// Use this for calculate
		public override void OnCalculate()
		{
			float angle;
			Vector3 axis;

			_Quaternion.value.ToAngleAxis(out angle, out axis);

			_Angle.SetValue(angle);
			_Axis.SetValue(axis);
		}
	}
}
