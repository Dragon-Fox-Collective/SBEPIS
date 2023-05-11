//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Time.timeScaleを設定する。
	/// </summary>
#else
	/// <summary>
	/// Set Time.timeScale.
	/// </summary>
#endif
	[AddBehaviourMenu("Time/SetTimeScale")]
	[BuiltInBehaviour]
	[AddComponentMenu("")]
	public sealed class SetTimeScale : StateBehaviour
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 設定するTimeScaleの値
		/// </summary>
#else
		/// <summary>
		/// TimeScale value to set
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _TimeScale = new FlexibleFloat(1f);

		public override void OnStateBegin()
		{
			Time.timeScale = _TimeScale.value;
		}
	}
}