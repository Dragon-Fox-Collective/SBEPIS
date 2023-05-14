//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
	[AddComponentMenu("")]
	[HideBehaviour()]
	public abstract class AgentMoveBase : AgentIntervalUpdate
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動する速さ
		/// </summary>
#else
		/// <summary>
		/// Speed to move
		/// </summary>
#endif
		[SerializeField]
		protected FlexibleFloat _Speed = new FlexibleFloat(0f);

		[SerializeField]
		[HideInInspector]
		private int _AgentMoveBase_SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_Speed")]
		[HideInInspector]
		private float _OldSpeed = 0f;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		protected override void Reset()
		{
			base.Reset();

			_AgentMoveBase_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Speed = (FlexibleFloat)_OldSpeed;
		}

		void Serialize()
		{
			while (_AgentMoveBase_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_AgentMoveBase_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_AgentMoveBase_SerializeVersion++;
						break;
					default:
						_AgentMoveBase_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		public override void OnAfterDeserialize()
		{
			base.OnAfterDeserialize();

			Serialize();
		}

		public override void OnBeforeSerialize()
		{
			base.OnBeforeSerialize();

			Serialize();
		}
	}
}
