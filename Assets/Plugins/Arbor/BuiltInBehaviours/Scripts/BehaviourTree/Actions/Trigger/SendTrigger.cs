//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.BehaviourTree.Actions
{
#if ARBOR_DOC_JA
	/// <summary>
	/// トリガーを送る。
	/// </summary>
#else
	/// <summary>
	/// It will send a trigger.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Trigger/SendTrigger")]
	[BuiltInBehaviour]
	public sealed class SendTrigger : ActionBehaviour
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// トリガーを送る対象。
		/// </summary>
#else
		/// <summary>
		/// Subject to send a trigger.
		/// </summary>
#endif
		[SerializeField]
		[ClassExtends(typeof(ArborFSM))]
		private FlexibleComponent _Target = new FlexibleComponent((Component)null);

#if ARBOR_DOC_JA
		/// <summary>
		/// 送るトリガー
		/// </summary>
#else
		/// <summary>
		/// Trigger to send
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _Message = new FlexibleString(string.Empty);

#if ARBOR_DOC_JA
		/// <summary>
		/// 送るトリガーフラグ
		/// </summary>
#else
		/// <summary>
		/// Trigger flag to send
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(SendTriggerFlags))]
		private FlexibleSendTriggerFlags _SendFlags = new FlexibleSendTriggerFlags(ArborFSMInternal.allSendTrigger);

		#endregion // Serialize fields

		protected override void OnExecute()
		{
			ArborFSM target = _Target.value as ArborFSM;
			if (target != null)
			{
				target.SendTrigger(_Message.value, _SendFlags.value);
			}
			FinishExecute(true);
		}
	}
}
