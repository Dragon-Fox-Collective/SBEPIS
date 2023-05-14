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
	/// GameObjectとその子オブジェクトにトリガーを送る。
	/// </summary>
#else
	/// <summary>
	/// It will send a trigger to a GameObject and child objects.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Trigger/BroadcastTrigger")]
	[BuiltInBehaviour]
	public sealed class BroadcastTrigger : ActionBehaviour
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

		void Broadcast(GameObject target)
		{
			if (target != null && target.activeInHierarchy)
			{
				string message = _Message.value;

				var arborFSMs = target.GetComponentsTemp<ArborFSM>();
				for (int fsmIndex = 0; fsmIndex < arborFSMs.Count; fsmIndex++)
				{
					ArborFSM fsm = arborFSMs[fsmIndex];
					if (fsm.parentGraph == null)
					{
						fsm.SendTrigger(message, _SendFlags.value);
					}
				}

				var targetTransform = target.transform;
				for (int childIndex = 0; childIndex < targetTransform.childCount; childIndex++)
				{
					Transform child = targetTransform.GetChild(childIndex);
					Broadcast(child.gameObject);
				}
			}
		}

		protected override void OnExecute()
		{
			Broadcast(target);
			FinishExecute(true);
		}
	}
}
