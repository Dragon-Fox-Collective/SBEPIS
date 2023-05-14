//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// うろつく中心タイプ
	/// </summary>
#else
	/// <summary>
	/// Wandering center type
	/// </summary>
#endif
	[Internal.Documentable]
	public enum PatrolCenterType
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// AgentControllerオブジェクトの初期配置位置
		/// </summary>
#else
		/// <summary>
		/// Initial placement position of AgentController object
		/// </summary>
#endif
		InitialPlacementPosition,

#if ARBOR_DOC_JA
		/// <summary>
		/// ステート開始時のAgentControllerオブジェクトの位置
		/// </summary>
#else
		/// <summary>
		/// Position of AgentController object at the time of the state start
		/// </summary>
#endif
		StateStartPosition,

#if ARBOR_DOC_JA
		/// <summary>
		/// Transform
		/// </summary>
#else
		/// <summary>
		/// Transform
		/// </summary>
#endif
		Transform,

#if ARBOR_DOC_JA
		/// <summary>
		/// カスタム
		/// </summary>
#else
		/// <summary>
		/// Custom
		/// </summary>
#endif
		Custom,
	}
}