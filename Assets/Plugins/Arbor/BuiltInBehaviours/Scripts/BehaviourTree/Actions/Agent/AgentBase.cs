//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
{
	[AddComponentMenu("")]
	[HideBehaviour()]
	public class AgentBase : ActionBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 制御したいAgentController。
		/// </summary>
#else
		/// <summary>
		/// AgentController you want to control.
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(AgentController))]
		protected FlexibleComponent _AgentController = new FlexibleComponent(FlexibleHierarchyType.Self);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		public AgentController cachedAgentController
		{
			get
			{
				return _AgentController.value as AgentController;
			}
		}

		protected override void OnStart()
		{
			AgentController agentController = cachedAgentController;
			if (agentController == null)
			{
				Debug.LogWarning(actionNode.name + " : AgentController is not set.", behaviourTree);
			}
		}

		protected virtual void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_AgentController.SetHierarchyIfConstantNull();
		}

		void Serialize()
		{
			while (_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_SerializeVersion++;
						break;
					default:
						_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		public virtual void OnBeforeSerialize()
		{
			Serialize();
		}

		public virtual void OnAfterDeserialize()
		{
			Serialize();
		}
	}
}
