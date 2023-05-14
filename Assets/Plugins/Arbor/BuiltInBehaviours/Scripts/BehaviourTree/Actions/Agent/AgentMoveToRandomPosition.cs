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
	/// 指定位置を中心とする半径内のランダム移動。
	/// </summary>
#else
	/// <summary>
	/// Random movement within a radius centered on a specified position.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Agent/AgentMoveToRandomPosition")]
	[BuiltInBehaviour]
	public sealed class AgentMoveToRandomPosition : AgentMoveBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動半径
		/// </summary>
#else
		/// <summary>
		/// Moving radius
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Radius = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// レイキャストによる補正を行うかどうか。<br/>trueの場合壁を迂回せずに壁に向かって移動する。
		/// </summary>
#else
		/// <summary>
		/// Whether to make corrections by raycasting. <br/> If true, move toward the wall without detouring the wall.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _CheckRaycast = new FlexibleBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// 停止する距離
		/// </summary>
#else
		/// <summary>
		/// Distance to stop
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _StoppingDistance = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動範囲の中心タイプ
		/// </summary>
#else
		/// <summary>
		/// Center type of movement range
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(PatrolCenterType))]
		private FlexiblePatrolCenterType _CenterType = new FlexiblePatrolCenterType(PatrolCenterType.InitialPlacementPosition);

#if ARBOR_DOC_JA
		/// <summary>
		/// 中心Transformの指定(CenterTypeがTransformのみ)
		/// </summary>
#else
		/// <summary>
		/// Specifying the center transform (CenterType is Transform only)
		/// </summary>
#endif
		[SerializeField]
		private FlexibleTransform _CenterTransform = new FlexibleTransform();

#if ARBOR_DOC_JA
		/// <summary>
		/// 中心の指定(CenterTypeがCustomのみ)
		/// </summary>
#else
		/// <summary>
		/// Specify the center (CenterType is Custom only)
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _CenterPosition = new FlexibleVector3();

		[SerializeField]
		[HideInInspector]
		private int _AgentPatrol_SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_UpdateType")]
		private AgentUpdateType _OldUpdateType = AgentUpdateType.Time;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_TimeType")]
		private TimeType _OldTimeType = TimeType.Normal;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_CenterType")]
		private PatrolCenterType _OldCenterType = PatrolCenterType.InitialPlacementPosition;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		Vector3 _ActionStartPosition;

		protected override void OnStart()
		{
			base.OnStart();

			AgentController agentController = cachedAgentController;
			if (agentController != null)
			{
				_ActionStartPosition = agentController.agentTransform.position;
			}
		}

		protected override bool OnIntervalUpdate(AgentController agentController)
		{
			switch (_CenterType.value)
			{
				case PatrolCenterType.InitialPlacementPosition:
					agentController.MoveToRandomPosition(_Speed.value, _Radius.value, _StoppingDistance.value, _CheckRaycast.value);
					break;
				case PatrolCenterType.StateStartPosition:
					agentController.MoveToRandomPosition(_ActionStartPosition, _Speed.value, _Radius.value, _StoppingDistance.value, _CheckRaycast.value);
					break;
				case PatrolCenterType.Transform:
					Transform centerTransform = _CenterTransform.value;
					if (centerTransform != null)
					{
						agentController.MoveToRandomPosition(centerTransform.position, _Speed.value, _Radius.value, _StoppingDistance.value, _CheckRaycast.value);
					}
					break;
				case PatrolCenterType.Custom:
					agentController.MoveToRandomPosition(_CenterPosition.value, _Speed.value, _Radius.value, _StoppingDistance.value, _CheckRaycast.value);
					break;
			}

			return true;
		}

		protected override void Reset()
		{
			base.Reset();

			_AgentPatrol_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_UpdateType = (FlexibleAgentUpdateType)_OldUpdateType;
			_TimeType = (FlexibleTimeType)_OldTimeType;
			_CenterType = (FlexiblePatrolCenterType)_OldCenterType;
		}

		void Serialize()
		{
			while (_AgentPatrol_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_AgentPatrol_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_AgentPatrol_SerializeVersion++;
						break;
					default:
						_AgentPatrol_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		public override void OnBeforeSerialize()
		{
			base.OnBeforeSerialize();

			Serialize();
		}

		public override void OnAfterDeserialize()
		{
			base.OnAfterDeserialize();

			Serialize();
		}
	}
}