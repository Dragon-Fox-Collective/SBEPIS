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
	/// AgentをTargetから隠れるように移動させる。
	/// </summary>
#else
	/// <summary>
	/// Move the Agent to hide from the Target.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Agent/AgentHideFromPosition")]
	[BuiltInBehaviour]
	public sealed class AgentHideFromPosition : AgentMoveBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// どの障害物のタイプに隠れるかを設定する
		/// </summary>
#else
		/// <summary>
		/// Set which obstacle type to hide
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(ObstacleTargetFlags))]
		private FlexibleObstacleTargetFlags _ObstacleTargetFlags = new FlexibleObstacleTargetFlags(ObstacleTargetFlags.NavMeshObstacle);

#if ARBOR_DOC_JA
		/// <summary>
		/// 障害物の検索方法を設定する
		/// </summary>
#else
		/// <summary>
		/// Set the obstacle search method
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(ObstacleSearchFlags))]
		private FlexibleObstacleSearchFlags _ObstacleSearchFlags = new FlexibleObstacleSearchFlags();

#if ARBOR_DOC_JA
		/// <summary>
		/// 障害物のレイヤー
		/// </summary>
#else
		/// <summary>
		/// Obstacle layer
		/// </summary>
#endif
		[SerializeField]
		private FlexibleLayerMask _ObstacleLayer = new FlexibleLayerMask(Physics.DefaultRaycastLayers);

#if ARBOR_DOC_JA
		/// <summary>
		/// ターゲットとの最小距離。この距離よりも近い位置にある障害物は隠れ先の候補から除外される。
		/// </summary>
#else
		/// <summary>
		/// Minimum distance to the target. Obstacles closer than this distance are excluded from potential hiding places.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _MinDistanceToTarget = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象の位置
		/// </summary>
#else
		/// <summary>
		/// 	Target position
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _TargetPosition = new FlexibleVector3();

		#endregion

		protected override bool OnIntervalUpdate(AgentController agentController)
		{
			return agentController.Hide(_Speed.value, _TargetPosition.value, _MinDistanceToTarget.value, _ObstacleTargetFlags.value, _ObstacleSearchFlags.value, _ObstacleLayer.value);
		}
	}
}
