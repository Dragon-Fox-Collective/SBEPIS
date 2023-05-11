//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;


namespace Arbor.BehaviourTree.Actions
{
	[AddComponentMenu("")]
	[HideBehaviour]
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

		#endregion // Serialize fields
	}
}