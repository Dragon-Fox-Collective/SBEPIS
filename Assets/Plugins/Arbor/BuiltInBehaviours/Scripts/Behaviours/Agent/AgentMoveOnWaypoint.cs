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
	/// Waypointに沿ってAgentを移動させる。
	/// </summary>
#else
	/// <summary>
	/// Move the Agent along the Waypoint.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Agent/AgentMoveOnWaypoint")]
	[BuiltInBehaviour]
	public sealed class AgentMoveOnWaypoint : AgentUpdateBase
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
		private FlexibleFloat _Speed = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// Agentを移動させる経路
		/// </summary>
#else
		/// <summary>
		/// Route to move Agent
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Waypoint))]
		private FlexibleComponent _Waypoint = new FlexibleComponent();

#if ARBOR_DOC_JA
		/// <summary>
		/// Waypointの各ポイント通過時に中心からどれくらい離れるかを設定する。
		/// </summary>
#else
		/// <summary>
		/// Set how far away from the center when passing each point of Waypoint.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Radius = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 再開時にWaypointの最初の位置からやり直すフラグ
		/// </summary>
#else
		/// <summary>
		/// Flag to restart from the first position of Waypoint when restarting
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _ClearDestPoint = new FlexibleBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生タイプ。
		/// </summary>
#else
		/// <summary>
		/// Play type.
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(MoveWaypointType))]
		private FlexibleMoveWayppintType _Type = new FlexibleMoveWayppintType(MoveWaypointType.Once);

#if ARBOR_DOC_JA
		/// <summary>
		/// 停止する距離
		/// </summary>
#else
		/// <summary>
		/// Distance to stop
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _StoppingDistance = new FlexibleFloat(0f);

		[SerializeField]
		[HideInInspector]
		private int _AgentMoveOnWaypoint_SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_Speed")]
		[HideInInspector]
		private float _OldSpeed = 0f;

		[SerializeField]
		[FormerlySerializedAs("_StopOnStateEnd")]
		[HideInInspector]
		private bool _OldStopOnStateEnd = false;

		[SerializeField]
		[FormerlySerializedAs("_Type")]
		[HideInInspector]
		private MoveWaypointType _OldType = MoveWaypointType.Once;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		private WaypointProcessor _Processor = new WaypointProcessor();

		bool GotoNextPoint(AgentController agentController, bool moveDone)
		{
			if (!_Processor.isValid)
			{
				return false;
			}

			if (!_Processor.isDone && moveDone)
			{
				_Processor.Next(_Type.value);
			}

			if (_Processor.isDone)
			{
				return true;
			}

			var nextPoint = _Processor.nextPoint;
			if (nextPoint == null)
			{
				return false;
			}

			Vector2 circle = Random.insideUnitCircle;
			Vector3 toPos = nextPoint.position + new Vector3(circle.x, 0f, circle.y) * _Radius.value;
			
			return agentController.MoveTo(_Speed.value, _StoppingDistance.value, toPos);
		}

		private bool _IsStartExecuted = false;

		// Use this for enter state
		public override void OnStateBegin()
		{
			_IsStartExecuted = false;

			_Processor.Initialize(_Waypoint.value as Waypoint, _ClearDestPoint.value);

			base.OnStateBegin();
		}

		protected override bool IsDone(AgentController agentController)
		{
			return base.IsDone(agentController) && _Processor.isDone;
		}

		protected override bool OnUpdateAgent(AgentController agentController)
		{
			if (!_IsStartExecuted || agentController.isDone)
			{
				if (!GotoNextPoint(agentController, _IsStartExecuted))
				{
					return false;
				}
			}
			_IsStartExecuted = true;

			return true;
		}

		protected override void Reset()
		{
			base.Reset();

			_AgentMoveOnWaypoint_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Speed = (FlexibleFloat)_OldSpeed;
			_StopOnStateEnd = (FlexibleBool)_OldStopOnStateEnd;
			_Type = (FlexibleMoveWayppintType)_OldType;
		}

		void Serialize()
		{
			while (_AgentMoveOnWaypoint_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_AgentMoveOnWaypoint_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_AgentMoveOnWaypoint_SerializeVersion++;
						break;
					default:
						_AgentMoveOnWaypoint_SerializeVersion = kCurrentSerializeVersion;
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