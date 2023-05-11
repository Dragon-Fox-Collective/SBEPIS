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
		private FlexibleFloat _Radius = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// レイキャストによる補正を行うかどうかを設定する<br/>
		/// trueにした場合、移動先との間に壁がある場合迂回せずに壁の手前まで移動する。
		/// </summary>
#else
		/// <summary>
		/// Set whether to perform correction by raycast <br />
		/// If set to true, if there is a wall between the destination and the destination, move to the front of the wall without detouring.
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
		[SerializeField] private FlexibleFloat _StoppingDistance = new FlexibleFloat(0f);

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
		[FormerlySerializedAs("_Radius")]
		[HideInInspector]
		private float _OldRadius = 0f;

		[SerializeField]
		[FormerlySerializedAs("_CenterType")]
		[HideInInspector]
		private PatrolCenterType _OldCenterType = PatrolCenterType.InitialPlacementPosition;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		Vector3 _StateStartPosition;

		public override void OnStateBegin()
		{
			AgentController agentController = cachedAgentController;
			if (agentController != null)
			{
				_StateStartPosition = agentController.agentTransform.position;
			}

			base.OnStateBegin();
		}

		protected override bool OnIntervalUpdate(AgentController agentController)
		{
			float speed = _Speed.value;
			float radius = _Radius.value;

			switch (_CenterType.value)
			{
				case PatrolCenterType.InitialPlacementPosition:
					agentController.MoveToRandomPosition(speed, radius, _StoppingDistance.value, _CheckRaycast.value);
					break;
				case PatrolCenterType.StateStartPosition:
					agentController.MoveToRandomPosition(_StateStartPosition, speed, radius, _StoppingDistance.value, _CheckRaycast.value);
					break;
				case PatrolCenterType.Transform:
					Transform centerTransform = _CenterTransform.value;
					if (centerTransform != null)
					{
						agentController.MoveToRandomPosition(centerTransform.position, speed, radius, _StoppingDistance.value, _CheckRaycast.value);
					}
					break;
				case PatrolCenterType.Custom:
					agentController.MoveToRandomPosition(_CenterPosition.value, speed, radius, _StoppingDistance.value, _CheckRaycast.value);
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
			_Radius = (FlexibleFloat)_OldRadius;
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
