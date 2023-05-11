//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.BehaviourTree.Actions
{
	using Arbor.Extensions;

#if ARBOR_DOC_JA
	/// <summary>
	/// GameObjectに割り当てられているすべてのArborFSMにトリガーを送ります。
	/// </summary>
#else
	/// <summary>
	/// Send the trigger to all of ArborFSM that is assigned to the GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Trigger/SendTriggerGameObject")]
	[BuiltInBehaviour]
	public sealed class SendTriggerGameObject : ActionBehaviour
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象のGameObject
		/// </summary>
#else
		/// <summary>
		/// GameObject target
		/// </summary>
#endif
		[SerializeField]
		private FlexibleGameObject _Target = new FlexibleGameObject(FlexibleHierarchyType.Self);

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

		public GameObject target
		{
			get
			{
				return _Target.value;
			}
		}

		protected override void OnExecute()
		{
			GameObject target = this.target;
			string message = _Message.value;

			if (target != null)
			{
				var arborFSMs = target.GetComponentsTemp<ArborFSM>();
				for (int fsmIndex = 0; fsmIndex < arborFSMs.Count; fsmIndex++)
				{
					ArborFSM fsm = arborFSMs[fsmIndex];
					if (fsm.parentGraph == null)
					{
						fsm.SendTrigger(message, _SendFlags.value);
					}
				}
			}

			FinishExecute(true);
		}
	}
}
