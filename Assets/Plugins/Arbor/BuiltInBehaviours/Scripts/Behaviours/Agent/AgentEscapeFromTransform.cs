//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// AgentをTargetから逃げるように移動させる。
	/// </summary>
#else
	/// <summary>
	/// Move the Agent to escape from Target.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Agent/AgentEscapeFromTransform")]
	[BuiltInBehaviour]
	public sealed class AgentEscapeFromTransform : AgentMoveBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 離れる距離
		/// </summary>
#else
		/// <summary>
		/// Distance away
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Distance = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// 曲がり角までの距離。<br/>
		/// 隅に追いやられた場合に折り返す判断に使用される。
		/// </summary>
#else
		/// <summary>
		/// Distance to the corner.<br/>
		/// It is used to make a decision to turn back when driven to a corner.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _DistanceToCorner = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// 逃げたい対象のTransform
		/// </summary>
#else
		/// <summary>
		/// Transform of object to escape
		/// </summary>
#endif
		[SerializeField] private FlexibleTransform _Target = new FlexibleTransform();

		[SerializeField]
		[HideInInspector]
		private int _AgentEscape_SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_Distance")]
		[HideInInspector]
		private float _OldDistance = 0f;

		#endregion // old

		#endregion

		private const int kCurrentSerializeVersion = 1;

		protected override bool OnIntervalUpdate(AgentController agentController)
		{
			return agentController.Escape(_Speed.value, _Distance.value, _Target.value, _DistanceToCorner.value);
		}

		protected override void Reset()
		{
			base.Reset();

			_AgentEscape_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Distance = (FlexibleFloat)_OldDistance;
		}

		void Serialize()
		{
			while (_AgentEscape_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_AgentEscape_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_AgentEscape_SerializeVersion++;
						break;
					default:
						_AgentEscape_SerializeVersion = kCurrentSerializeVersion;
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
