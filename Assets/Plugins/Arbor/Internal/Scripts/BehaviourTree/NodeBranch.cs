//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor.BehaviourTree
{
#if ARBOR_DOC_JA
	/// <summary>
	/// BehaviourTreeのノード間のブランチクラス
	/// </summary>
#else
	/// <summary>
	/// Branch classes between nodes of Behavior Tree
	/// </summary>
#endif
	[System.Serializable]
	public sealed class NodeBranch
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// ブランチID
		/// </summary>
#else
		/// <summary>
		/// Branch ID
		/// </summary>
#endif
		public int branchID;

#if ARBOR_DOC_JA
		/// <summary>
		/// 親ノードID
		/// </summary>
#else
		/// <summary>
		/// Parent node ID
		/// </summary>
#endif
		public int parentNodeID;

#if ARBOR_DOC_JA
		/// <summary>
		/// 子ノードID
		/// </summary>
#else
		/// <summary>
		/// Child node ID
		/// </summary>
#endif
		public int childNodeID;

		#endregion // Seralize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 接続ラインのベジェ。エディタで使用する。
		/// </summary>
#else
		/// <summary>
		/// Bezier of the connection line. Use it in the editor.
		/// </summary>
#endif
		[System.NonSerialized]
		public Bezier2D bezier = new Bezier2D();

#if ARBOR_DOC_JA
		/// <summary>
		/// isActiveが変更されたときに呼ばれる
		/// </summary>
#else
		/// <summary>
		/// Called when isActive changes
		/// </summary>
#endif
		public System.Action<bool> onActiveChanged;

		private bool _IsActive;

#if ARBOR_DOC_JA
		/// <summary>
		/// アクティブかどうかのフラグ。エディタで使用する。
		/// </summary>
#else
		/// <summary>
		/// Flag for active or not. Use it in the editor.
		/// </summary>
#endif
		public bool isActive
		{
			get
			{
				return _IsActive;
			}
			set
			{
				if (_IsActive != value)
				{
					_IsActive = value;
					onActiveChanged?.Invoke(_IsActive);
				}
			}
		}
	}
}