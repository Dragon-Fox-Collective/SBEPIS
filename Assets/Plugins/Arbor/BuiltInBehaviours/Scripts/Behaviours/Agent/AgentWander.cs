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
	/// Agentを徘徊させる。
	/// </summary>
	/// <remarks>
	/// 徘徊の移動先は、Agentの前方方向Distanceを中心としたRadius円上の点<br/>
	/// 円上のどこになるかはJitterによってランダムに変位する。
	/// </remarks>
#else
	/// <summary>
	/// Wander the Agent
	/// </summary>
	/// <remarks>
	/// The destination of the wandering is the point on the Radius circle centered on the agent's forward distance <br />
	/// Where on the circle is randomly displaced by Jitter.
	/// </remarks>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Agent/AgentWander")]
	[BuiltInBehaviour]
	public sealed class AgentWander : AgentMoveBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 徘徊する半径。
		/// </summary>
#else
		/// <summary>
		/// The wandering radius.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Radius = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// 徘徊する距離。
		/// </summary>
#else
		/// <summary>
		/// The wandering distance.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Distance = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動先を決めるランダムな変位の最大値。
		/// </summary>
#else
		/// <summary>
		/// The maximum value of random displacement that determines the destination.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Jitter = new FlexibleFloat();

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
		private FlexibleFloat _StoppingDistance = new FlexibleFloat(0f);

		#endregion

		protected override bool OnIntervalUpdate(AgentController agentController)
		{
			return agentController.Wander(_Speed.value, _Radius.value, _Distance.value, _Jitter.value, _StoppingDistance.value);
		}
	}
}
