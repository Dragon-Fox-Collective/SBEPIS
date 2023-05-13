//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 実行するメソッドを設定するフラグ
	/// </summary>
#else
	/// <summary>
	/// Flag to set the method to be executed
	/// </summary>
#endif
	[System.Flags]
	[Internal.Documentable]
	public enum ExecuteMethodFlags
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// ノードに入ったメソッドで実行する。
		/// </summary>
#else
		/// <summary>
		/// Execute with the method entered in the node.
		/// </summary>
#endif
		Enter = 1 << 0,

#if ARBOR_DOC_JA
		/// <summary>
		/// ノード更新メソッドで実行する。
		/// </summary>
#else
		/// <summary>
		/// Execute with node update method.
		/// </summary>
#endif
		Update = 1 << 1,

#if ARBOR_DOC_JA
		/// <summary>
		/// LateUpdateで実行する。
		/// </summary>
#else
		/// <summary>
		/// Execute at LateUpdate.
		/// </summary>
#endif
		LateUpdate = 1 << 2,

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードから抜けるときに実行する。
		/// </summary>
#else
		/// <summary>
		/// Execute when leaving a node.
		/// </summary>
#endif
		Leave = 1 << 3,

#if ARBOR_DOC_JA
		/// <summary>
		/// FixedUpdateメソッドで実行する。
		/// </summary>
#else
		/// <summary>
		/// Execute with FixedUpdate method.
		/// </summary>
#endif
		FixedUpdate = 1 << 4,
	}
}