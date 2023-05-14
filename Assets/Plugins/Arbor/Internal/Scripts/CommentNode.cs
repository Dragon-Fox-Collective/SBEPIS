//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// コメントを表すクラス
	/// </summary>
#else
	/// <summary>
	/// Class that represents a comment
	/// </summary>
#endif
	[System.Serializable]
	public sealed class CommentNode : Node
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// コメントIDを取得。
		/// </summary>
#else
		/// <summary>
		/// Gets the comment identifier.
		/// </summary>
#endif
		[System.Obsolete("use Node.nodeID")]
		public int commentID
		{
			get
			{
				return nodeID;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// コメント文字列。
		/// </summary>
#else
		/// <summary>
		/// Comment string.
		/// </summary>
#endif
		public string comment = string.Empty;

		internal CommentNode(NodeGraph nodeGraph, int nodeID) : base(nodeGraph, nodeID)
		{
			name = "Comment";
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Arbor3.9.0より前のノード名を取得する。
		/// </summary>
		/// <returns>旧ノード名</returns>
#else
		/// <summary>
		/// Get the node name before Arbor 3.9.0.
		/// </summary>
		/// <returns>Old node name</returns>
#endif
		protected override string GetOldName()
		{
			return "Comment";
		}
	}
}
