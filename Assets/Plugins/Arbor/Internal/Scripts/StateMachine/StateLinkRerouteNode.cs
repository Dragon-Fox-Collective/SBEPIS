//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// StateLinkのリルートノード
	/// </summary>
#else
	/// <summary>
	/// StateLink's reroute node
	/// </summary>
#endif
	[System.Serializable]
	public sealed class StateLinkRerouteNode : Node
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// デフォルトの方向
		/// </summary>
#else
		/// <summary>
		/// Default direction
		/// </summary>
#endif
		public static readonly Vector2 kDefaultDirection = Vector2.right;

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート
		/// </summary>
#else
		/// <summary>
		/// Transition destination state
		/// </summary>
#endif
		public StateLink link = new StateLink();

#if ARBOR_DOC_JA
		/// <summary>
		/// ラインの方向
		/// </summary>
#else
		/// <summary>
		/// Direction of line
		/// </summary>
#endif
		public Vector2 direction = kDefaultDirection;

		internal StateLinkRerouteNode(NodeGraph nodeGraph, int nodeID) : base(nodeGraph, nodeID)
		{
		}
	}
}