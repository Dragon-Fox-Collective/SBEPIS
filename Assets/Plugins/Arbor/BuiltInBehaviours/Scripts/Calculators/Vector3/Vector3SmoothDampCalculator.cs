//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.Calculators
{
	using Arbor.Extensions;

#if ARBOR_DOC_JA
	/// <summary>
	/// 目的地に向かって時間の経過とともに徐々にVector3を変化させる。
	/// </summary>
#else
	/// <summary>
	/// Gradually changes a vector towards a desired goal over time.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Vector3/Vector3.SmoothDamp")]
	[BehaviourTitle("Vector3.SmoothDamp")]
	[BuiltInBehaviour]
	public sealed class Vector3SmoothDampCalculator : Calculator
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 現在位置
		/// </summary>
#else
		/// <summary>
		/// The current position.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Current = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 目的地
		/// </summary>
#else
		/// <summary>
		/// The position we are trying to reach.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Target = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 現在の速度。
		/// </summary>
#else
		/// <summary>
		/// The current velocity.
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _CurrentVelocity = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// target へ到達するまでのおおよその時間。<br />
		/// 値が小さいほど、target に速く到達します。
		/// </summary>
#else
		/// <summary>
		/// Approximately the time it will take to reach the target.<br />
		/// A smaller value will reach the target faster.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _SmoothTime = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大の速さ。
		/// </summary>
#else
		/// <summary>
		/// The maximum speed.
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _MaxSpeed = new FlexibleFloat(Mathf.Infinity);

#if ARBOR_DOC_JA
		/// <summary>
		/// 結果出力
		/// </summary>
#else
		/// <summary>
		/// Output result
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _Result = new OutputSlotVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 現在速度の出力
		/// </summary>
#else
		/// <summary>
		/// Output current velocity
		/// </summary>
#endif
		[SerializeField] private OutputSlotVector3 _OutputCurrentVelocity = new OutputSlotVector3();

		#endregion // Serialize fields

		public override void OnCalculate()
		{
			Vector3 currentVelocity = _CurrentVelocity.value;
			Vector3 current = _Current.value;
			current = Vector3.SmoothDamp(current, _Target.value, ref currentVelocity, _SmoothTime.value, _MaxSpeed.value);
			_Result.SetValue(current);
			_OutputCurrentVelocity.SetValue(currentVelocity);
		}
	}
}
