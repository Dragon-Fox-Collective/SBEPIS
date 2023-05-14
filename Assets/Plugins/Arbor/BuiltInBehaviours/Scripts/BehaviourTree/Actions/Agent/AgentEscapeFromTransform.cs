//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.BehaviourTree.Actions
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
		[SerializeField] private FlexibleFloat _Distance = new FlexibleFloat();

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
		/// 対象のTransform
		/// </summary>
#else
		/// <summary>
		/// Target Transform
		/// </summary>
#endif
		[SerializeField]
		private FlexibleTransform _TargetTransform = new FlexibleTransform();

		#endregion // Serialize fields

		protected override bool OnIntervalUpdate(AgentController agentController)
		{
			return agentController.Escape(_Speed.value, _Distance.value, _TargetTransform.value, _DistanceToCorner.value);
		}
	}
}