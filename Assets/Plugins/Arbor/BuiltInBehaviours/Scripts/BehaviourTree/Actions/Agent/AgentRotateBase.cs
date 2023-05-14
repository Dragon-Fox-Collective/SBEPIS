//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;


namespace Arbor.BehaviourTree.Actions
{
	[AddComponentMenu("")]
	[HideBehaviour]
	public abstract class AgentRotateBase : AgentIntervalUpdate
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 回転する速さ
		/// </summary>
#else
		/// <summary>
		/// Speed of rotation
		/// </summary>
#endif
		[SerializeField]
		protected FlexibleFloat _AngularSpeed = new FlexibleFloat(120f);

		#endregion // Serialize fields
	}
}