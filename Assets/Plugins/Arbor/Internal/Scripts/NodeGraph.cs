//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using System.Collections.Generic;

using Arbor.ObjectPooling;

namespace Arbor
{
	using Arbor.Extensions;

#if ARBOR_DOC_JA
	/// <summary>
	/// ノードグラフの基本クラス。
	/// </summary>
#else
	/// <summary>
	/// Base class of node graph.
	/// </summary>
#endif
	[AddComponentMenu("")]
	public abstract class NodeGraph : MonoBehaviour, ISerializationCallbackReceiver, IPoolCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// グラフの名前。<br/>
		/// 一つのGameObjectに複数のグラフがある場合の識別や検索に使用する。
		/// </summary>
#else
		/// <summary>
		/// The Graph name.<br/>
		/// It is used for identification and retrieval when there is more than one Graph in one GameObject.
		/// </summary>
#endif
		[FormerlySerializedAs("fsmName")]
		[FormerlySerializedAs("graphName")]
		[Internal.DocumentLabel("Name")]
		[SerializeField]
		private string _GraphName = "";

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始時に再生するフラグ。
		/// </summary>
#else
		/// <summary>
		/// Flag to be played at the start.
		/// </summary>
#endif
		public bool playOnStart = true;

#if ARBOR_DOC_JA
		/// <summary>
		/// 更新に関する設定。
		/// </summary>
#else
		/// <summary>
		/// Settings related to updating.
		/// </summary>
#endif
		public UpdateSettings updateSettings = new UpdateSettings();

#if ARBOR_DOC_JA
		/// <summary>
		/// 無限ループのデバッグ設定
		/// </summary>
#else
		/// <summary>
		/// Debug setting of infinite loop
		/// </summary>
#endif
		public DebugInfiniteLoopSettings debugInfiniteLoopSettings = new DebugInfiniteLoopSettings();

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		private Object _OwnerBehaviour = null;

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		private ParameterContainerInternal _ParameterContainer = null;

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		private List<CalculatorNode> _Calculators = new List<CalculatorNode>();

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		private List<CommentNode> _Comments = new List<CommentNode>();

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		private List<GroupNode> _Groups = new List<GroupNode>();

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		[FormerlySerializedAs("_CalculatorBranchRerouteNodes")]
		private DataBranchRerouteNodeList _DataBranchRerouteNodes = new DataBranchRerouteNodeList();

		[SerializeField]
#if !ARBOR_DEBUG
		[HideInInspector]
#endif
		[FormerlySerializedAs("_CalculatorBranchies")]
		private List<DataBranch> _DataBranchies = new List<DataBranch>();

		#endregion // Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// グラフツリーが変更される際に呼ばれるコールバック
		/// </summary>
#else
		/// <summary>
		/// Callback called when the graph tree changes
		/// </summary>
#endif
		public event System.Action onChangedGraphTree = null;

#if ARBOR_DOC_JA
		/// <summary>
		/// グラフの名前が変更される際に呼ばれるコールバック
		/// </summary>
#else
		/// <summary>
		/// Callback called when the graph name changes
		/// </summary>
#endif
		public event System.Action onChangedGraphName = null;

#if ARBOR_DOC_JA
		/// <summary>
		/// グラフの名前。<br/>
		/// 一つのGameObjectに複数のグラフがある場合の識別や検索に使用する。
		/// </summary>
#else
		/// <summary>
		/// The Graph name.<br/>
		/// It is used for identification and retrieval when there is more than one Graph in one GameObject.
		/// </summary>
#endif
		public string graphName
		{
			get
			{
				return _GraphName;
			}
			set
			{
				if (_GraphName != value)
				{
					_GraphName = value;

					CallChangedGraphName();
				}
			}
		}

		void CallChangedGraphName()
		{
			onChangedGraphName?.Invoke();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Startが呼ばれている場合にtrueを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns true if Start has been called.
		/// </summary>
#endif
		public bool isStarted
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// playStateが変更されたときに呼び出される。
		/// </summary>
#else
		/// <summary>
		/// Called when playState changes.
		/// </summary>
#endif
		public event System.Action<PlayState> onPlayStateChanged;

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生状態
		/// </summary>
#else
		/// <summary>
		/// Play state
		/// </summary>
#endif
		public PlayState playState
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 現在の無限ループデバッグ設定。
		/// </summary>
		/// <remarks>子グラフの場合はルートグラフの無限ループデバッグ設定を返す。</remarks>
#else
		/// <summary>
		/// Current infinite loop debug setting.
		/// </summary>
		/// <remarks>If it is a child graph, return the infinite loop debug setting of the route graph.</remarks>
#endif
		public DebugInfiniteLoopSettings currentDebugInfiniteLoopSettings
		{
			get
			{
				NodeGraph rootGraph = this.rootGraph;
				if (rootGraph != null)
				{
					return rootGraph.debugInfiniteLoopSettings;
				}

				return debugInfiniteLoopSettings;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 親グラフ
		/// </summary>
#else
		/// <summary>
		/// Parent graph
		/// </summary>
#endif
		public NodeGraph parentGraph
		{
			get
			{
				NodeBehaviour owner = ownerBehaviour;
				if (owner != null)
				{
					return owner.nodeGraph;
				}
				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ルートグラフ
		/// </summary>
#else
		/// <summary>
		/// Root graph
		/// </summary>
#endif
		public NodeGraph rootGraph
		{
			get
			{
				NodeGraph current = this;
				while (true)
				{
					NodeGraph parent = current.parentGraph;
					if (parent == null)
					{
						break;
					}
					if (current == parent)
					{
						Debug.LogError("current == this");
						break;
					}
					current = parent;
				}
				return current;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// このグラフの所有者であるNodeBehaviourのObject
		/// </summary>
#else
		/// <summary>
		/// Object of NodeBehaviour own this graph
		/// </summary>
#endif
		public Object ownerBehaviourObject
		{
			get
			{
				return _OwnerBehaviour;
			}
			set
			{
				if (_OwnerBehaviour != value)
				{
					var oldRootGraph = this.rootGraph;
					
					_OwnerBehaviour = value;

					var currentRootGraph = this.rootGraph;

					if (oldRootGraph != null && oldRootGraph != currentRootGraph)
					{
						oldRootGraph.ChangedGraphTree();
					}

					if (currentRootGraph != null)
					{
						var behaviour = _OwnerBehaviour as NodeBehaviour;
						if (behaviour != null && !behaviour.attachedNode)
						{
							var graph = currentRootGraph;
							behaviour.delayAttachToNode += () =>
							{
								graph.ChangedGraphTree();
							};
						}
						else
						{
							currentRootGraph.ChangedGraphTree();
						}
					}
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// このグラフの所有者であるNodeBehaviour
		/// </summary>
#else
		/// <summary>
		/// NodeBehaviour is the owner of this graph
		/// </summary>
#endif
		public NodeBehaviour ownerBehaviour
		{
			get
			{
				return ownerBehaviourObject as NodeBehaviour;
			}
			set
			{
				ownerBehaviourObject = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// このグラフ内に割り当てられているParameterContainer
		/// </summary>
#else
		/// <summary>
		/// The ParameterContainer assigned in this graph
		/// </summary>
#endif
		public ParameterContainerInternal parameterContainer
		{
			get
			{
				return _ParameterContainer;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードグラフの表示名。graphNameが空かnullの場合は"(No Name)"を返す。
		/// </summary>
#else
		/// <summary>
		/// Display name of the node graph. If graphName is empty or null, it returns "(No Name)".
		/// </summary>
#endif
		public string displayGraphName
		{
			get
			{
				if (string.IsNullOrEmpty(graphName))
				{
					return "(No Name)";
				}
				return graphName;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 外部グラフとして利用されているフラグ
		/// </summary>
#else
		/// <summary>
		/// Flag used as an external graph
		/// </summary>
#endif
		public bool external
		{
			get;
			private set;
		}

		private bool _ExecuteLateUpdate = false;

		[System.NonSerialized]
		private List<Node> _Nodes = new List<Node>();

		[System.NonSerialized]
		private Dictionary<int, Node> _DicNodes = new Dictionary<int, Node>();

		[System.NonSerialized]
		private Dictionary<int, CalculatorNode> _DicCalculators = new Dictionary<int, CalculatorNode>();

		[System.NonSerialized]
		private Dictionary<int, CommentNode> _DicComments = new Dictionary<int, CommentNode>();

		[System.NonSerialized]
		private Dictionary<int, GroupNode> _DicGroups = new Dictionary<int, GroupNode>();

		[System.NonSerialized]
		private Dictionary<int, DataBranch> _DicDataBranchies = new Dictionary<int, DataBranch>();

#if ARBOR_DOC_JA
		/// <summary>
		/// Nodeの数を取得。
		/// </summary>
#else
		/// <summary>
		///  Get a count of Node.
		/// </summary>
#endif
		public int nodeCount
		{
			get
			{
				return _Nodes.Count;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Nodeをインデックスから取得
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>Node</returns>
#else
		/// <summary>
		/// Get Node from index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>Node</returns>
#endif
		public Node GetNodeFromIndex(int index)
		{
			return _Nodes[index];
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードIDを指定して<see cref="Arbor.Node" />を取得する。
		/// </summary>
		/// <param name="nodeID">ノードID</param>
		/// <returns>見つかった<see cref="Arbor.Node" />。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Gets <see cref="Arbor.Node" /> from the node identifier.
		/// </summary>
		/// <param name="nodeID">The node identifier.</param>
		/// <returns>Found <see cref = "Arbor.Node" />. Returns null if not found.</returns>
#endif
		public Node GetNodeFromID(int nodeID)
		{
			Node result = null;
			if (_DicNodes.TryGetValue(nodeID, out result))
			{
				return result;
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 一意のノードIDかを確認する。
		/// </summary>
		/// <param name="nodeID">一意か確認するnodeID</param>
		/// <returns>一意の場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Check if it is a unique node ID.
		/// </summary>
		/// <param name="nodeID">Node ID to check if it is unique</param>
		/// <returns>Returns true if unique.</returns>
#endif
		protected bool IsUniqueNodeID(int nodeID)
		{
			return nodeID != 0 && GetNodeFromID(nodeID) == null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 一意のノードIDを取得する。
		/// </summary>
		/// <returns>一意のノードID</returns>
#else
		/// <summary>
		/// Get a unique node ID.
		/// </summary>
		/// <returns>Unique node ID</returns>
#endif
		protected int GetUniqueNodeID()
		{
			int count = _Nodes.Count;

			System.Random random = new System.Random(count);

			while (true)
			{
				int nodeID = random.Next();

				if (IsUniqueNodeID(nodeID))
				{
					return nodeID;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// CalculatorNodeの数を取得。
		/// </summary>
#else
		/// <summary>
		///  Get a count of CalculatorNode.
		/// </summary>
#endif
		public int calculatorCount
		{
			get
			{
				return _Calculators.Count;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// CalculatorNodeをインデックスから取得
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>CalculatorNode</returns>
#else
		/// <summary>
		/// Get CalculatorNode from index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>CalculatorNode</returns>
#endif
		public CalculatorNode GetCalculatorFromIndex(int index)
		{
			return _Calculators[index];
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// CalculatorNodeのインデックスを取得
		/// </summary>
		/// <param name="calculator">CalculatorNode</param>
		/// <returns>インデックス。ない場合は-1を返す。</returns>
#else
		/// <summary>
		/// Get CalculatorNode index.
		/// </summary>
		/// <param name="calculator">CalculatorNode</param>
		/// <returns>Index. If not, it returns -1.</returns>
#endif
		public int GetCalculatorIndex(CalculatorNode calculator)
		{
			return _Calculators.IndexOf(calculator);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 全ての<see cref="Arbor.CalculatorNode" />を取得する。
		/// </summary>
#else
		/// <summary>
		/// Gets all of <see cref = "Arbor.CalculatorNode" />.
		/// </summary>
#endif
		[System.Obsolete("use calculatorCount and GetCalculatorFromIndex()")]
		public CalculatorNode[] calculators
		{
			get
			{
				return _Calculators.ToArray();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算ノードIDを指定して<see cref="Arbor.CalculatorNode" />を取得する。
		/// </summary>
		/// <param name="calculatorID">演算ノードID</param>
		/// <returns>見つかった<see cref="Arbor.CalculatorNode" />。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Gets <see cref="Arbor.CalculatorNode" /> from the calculator identifier.
		/// </summary>
		/// <param name="calculatorID">The calculator identifier.</param>
		/// <returns>Found <see cref = "Arbor.CalculatorNode" />. Returns null if not found.</returns>
#endif
		public CalculatorNode GetCalculatorFromID(int calculatorID)
		{
			CalculatorNode result = null;
			if (_DicCalculators.TryGetValue(calculatorID, out result))
			{
				return result;
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算ノードを生成。
		/// </summary>
		/// <param name="nodeID">ノード ID</param>
		/// <param name="calculatorType">Calculatorの型</param>
		/// <returns>生成した演算ノード。ノードIDが重複している場合は生成せずにnullを返す。</returns>
#else
		/// <summary>
		/// Create calculator.
		/// </summary>
		/// <param name="nodeID">Node ID</param>
		/// <param name="calculatorType">Calculator type</param>
		/// <returns>The created calculator. If the node ID is not unique, return null without creating it.</returns>
#endif
		public CalculatorNode CreateCalculator(int nodeID, System.Type calculatorType)
		{
			if (!IsUniqueNodeID(nodeID))
			{
				Debug.LogWarning("CreateCalculator id(" + nodeID + ") is not unique.");
				return null;
			}

			CalculatorNode calculator = new CalculatorNode(this, nodeID, calculatorType);

			ComponentUtility.RecordObject(this, "Created Calculator");

			_Calculators.Add(calculator);
			_DicCalculators.Add(calculator.nodeID, calculator);
			RegisterNode(calculator);

			ComponentUtility.SetDirty(this);

			return calculator;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算ノードを生成。
		/// </summary>
		/// <param name="calculatorType">Calculatorの型</param>
		/// <returns>生成した演算ノード。</returns>
#else
		/// <summary>
		/// Create calculator.
		/// </summary>
		/// <param name="calculatorType">Calculator type</param>
		/// <returns>The created calculator.</returns>
#endif
		public CalculatorNode CreateCalculator(System.Type calculatorType)
		{
			return CreateCalculator(GetUniqueNodeID(), calculatorType);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Calculatorが属しているCalculatorNodeの取得。
		/// </summary>
		/// <param name="calculator">Calculator</param>
		/// <returns> Calculatorが属しているCalculatorNode。ない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Acquisition of CalculatorNodes Calculator belongs.
		/// </summary>
		/// <param name="calculator">Calculator</param>
		/// <returns>CalculatorNodes Calculator belongs. Return null if not.</returns>
#endif
		public CalculatorNode FindCalculator(Calculator calculator)
		{
			for (int i = 0; i < _Calculators.Count; i++)
			{
				var calculatorNode = _Calculators[i];
				if (calculatorNode.calculator == calculator)
				{
					return calculatorNode;
				}
			}
			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算ノードの削除。
		/// </summary>
		/// <param name="calculatorNode">削除する演算ノード。</param>
		/// <returns>削除した場合にtrue</returns>
#else
		/// <summary>
		/// Delete calculator.
		/// </summary>
		/// <param name="calculatorNode">Calculator that you want to delete.</param>
		/// <returns>true if deleted</returns>
#endif
		public bool DeleteCalculator(CalculatorNode calculatorNode)
		{
			Object calculatorObj = calculatorNode.GetObject();

			ComponentUtility.RegisterCompleteObjectUndo(this, "Delete Nodes");

			ComponentUtility.RecordObject(this, "Delete Nodes");
			_Calculators.Remove(calculatorNode);

			_DicCalculators.Remove(calculatorNode.nodeID);
			RemoveNode(calculatorNode);

			DisconnectDataBranch(calculatorObj);

			NodeBehaviour nodeBehaviour = calculatorObj as NodeBehaviour;
			if (nodeBehaviour != null)
			{
				NodeBehaviour.Destroy(nodeBehaviour);
			}
			else if (calculatorObj != null)
			{
				ComponentUtility.Destroy(calculatorObj);
			}

			ComponentUtility.SetDirty(this);

			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// CommentNodeの数を取得。
		/// </summary>
#else
		/// <summary>
		///  Get a count of CommentNode.
		/// </summary>
#endif
		public int commentCount
		{
			get
			{
				return _Comments.Count;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// CommentNodeをインデックスから取得
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>CommentNode</returns>
#else
		/// <summary>
		/// Get CommentNode from index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>CommentNode</returns>
#endif
		public CommentNode GetCommentFromIndex(int index)
		{
			return _Comments[index];
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// CommentNodeのインデックスを取得
		/// </summary>
		/// <param name="comment">CommentNode</param>
		/// <returns>インデックス。ない場合は-1を返す。</returns>
#else
		/// <summary>
		/// Get CommentNode index.
		/// </summary>
		/// <param name="comment">CommentNode</param>
		/// <returns>Index. If not, it returns -1.</returns>
#endif
		public int GetCommentIndex(CommentNode comment)
		{
			return _Comments.IndexOf(comment);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 全ての<see cref="Arbor.CommentNode" />を取得する。
		/// </summary>
#else
		/// <summary>
		/// Gets all of <see cref = "Arbor.CommentNode" />.
		/// </summary>
#endif
		[System.Obsolete("use commentCount and GetCommentFromIndex()")]
		public CommentNode[] comments
		{
			get
			{
				return _Comments.ToArray();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// コメントIDを指定して<see cref="Arbor.CommentNode" />を取得する。
		/// </summary>
		/// <param name="commentID">コメントID</param>
		/// <returns>見つかった<see cref="Arbor.CommentNode" />。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Gets <see cref="Arbor.CommentNode" /> from the comment identifier.
		/// </summary>
		/// <param name="commentID">The comment identifier.</param>
		/// <returns>Found <see cref = "Arbor.CommentNode" />. Returns null if not found.</returns>
#endif
		public CommentNode GetCommentFromID(int commentID)
		{
			CommentNode result = null;
			if (_DicComments.TryGetValue(commentID, out result))
			{
				return result;
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// コメントを生成。
		/// </summary>
		/// <param name="nodeID">ノードID</param>
		/// <returns>生成したコメント。ノードIDが重複している場合は生成せずにnullを返す。</returns>
#else
		/// <summary>
		/// Create comment.
		/// </summary>
		/// <param name="nodeID">Node ID</param>
		/// <returns>The created comment. If the node ID is not unique, return null without creating it.</returns>
#endif
		public CommentNode CreateComment(int nodeID)
		{
			if (!IsUniqueNodeID(nodeID))
			{
				Debug.LogWarning("CreateComment id(" + nodeID + ") is not unique.");
				return null;
			}

			CommentNode comment = new CommentNode(this, nodeID);

			ComponentUtility.RecordObject(this, "Created Comment");

			_Comments.Add(comment);
			_DicComments.Add(comment.nodeID, comment);
			RegisterNode(comment);

			ComponentUtility.SetDirty(this);

			return comment;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// コメントを生成。
		/// </summary>
		/// <returns>生成したコメント。</returns>
#else
		/// <summary>
		/// Create comment.
		/// </summary>
		/// <returns>The created comment.</returns>
#endif
		public CommentNode CreateComment()
		{
			return CreateComment(GetUniqueNodeID());
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// コメントの削除。
		/// </summary>
		/// <param name="comment">削除するコメント。</param>
#else
		/// <summary>
		/// Delete comment.
		/// </summary>
		/// <param name="comment">Comment that you want to delete.</param>
#endif
		public void DeleteComment(CommentNode comment)
		{
			ComponentUtility.RegisterCompleteObjectUndo(this, "Delete Nodes");

			ComponentUtility.RecordObject(this, "Delete Nodes");
			_Comments.Remove(comment);

			_DicComments.Remove(comment.nodeID);			
			RemoveNode(comment);

			ComponentUtility.SetDirty(this);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GroupNodeの数を取得。
		/// </summary>
#else
		/// <summary>
		///  Get a count of GroupNode.
		/// </summary>
#endif
		public int groupCount
		{
			get
			{
				return _Groups.Count;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GroupNodeをインデックスから取得
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>GroupNode</returns>
#else
		/// <summary>
		/// Get GroupNode from index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>GroupNode</returns>
#endif
		public GroupNode GetGroupFromIndex(int index)
		{
			return _Groups[index];
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// グループを生成。
		/// </summary>
		/// <param name="nodeID">ノード ID</param>
		/// <returns>生成したグループ。ノードIDが重複している場合は生成せずにnullを返す。</returns>
#else
		/// <summary>
		/// Create group.
		/// </summary>
		/// <param name="nodeID">Node ID</param>
		/// <returns>The created group. If the node ID is not unique, return null without creating it.</returns>
#endif
		public GroupNode CreateGroup(int nodeID)
		{
			if (!IsUniqueNodeID(nodeID))
			{
				Debug.LogWarning("CreateGroup id(" + nodeID + ") is not unique.");
				return null;
			}

			GroupNode group = new GroupNode(this, nodeID);

			ComponentUtility.RecordObject(this, "Created Group");

			_Groups.Add(group);
			_DicGroups.Add(group.nodeID, group);
			RegisterNode(group);

			ComponentUtility.SetDirty(this);

			return group;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// グループを生成。
		/// </summary>
		/// <returns>生成したグループ。</returns>
#else
		/// <summary>
		/// Create group.
		/// </summary>
		/// <returns>The created group.</returns>
#endif
		public GroupNode CreateGroup()
		{
			return CreateGroup(GetUniqueNodeID());
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// グループの削除。
		/// </summary>
		/// <param name="group">削除するグループ。</param>
#else
		/// <summary>
		/// Delete group.
		/// </summary>
		/// <param name="group">Group that you want to delete.</param>
#endif
		public void DeleteGroup(GroupNode group)
		{
			ComponentUtility.RegisterCompleteObjectUndo(this, "Delete Nodes");

			ComponentUtility.RecordObject(this, "Delete Nodes");
			_Groups.Remove(group);

			_DicGroups.Remove(group.nodeID);
			RemoveNode(group);

			ComponentUtility.SetDirty(this);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// グループIDを指定して<see cref="Arbor.GroupNode" />を取得する。
		/// </summary>
		/// <param name="groupID">グループID</param>
		/// <returns>見つかった<see cref="Arbor.GroupNode" />。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Gets <see cref="Arbor.GroupNode" /> from the group identifier.
		/// </summary>
		/// <param name="groupID">The group identifier.</param>
		/// <returns>Found <see cref = "Arbor.GroupNode" />. Returns null if not found.</returns>
#endif
		public GroupNode GetGroupFromID(int groupID)
		{
			GroupNode result = null;
			if (_DicGroups.TryGetValue(groupID, out result))
			{
				return result;
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchRerouteNodeリスト
		/// </summary>
#else
		/// <summary>
		/// DataBranchRerouteNode list
		/// </summary>
#endif
		public DataBranchRerouteNodeList dataBranchRerouteNodes
		{
			get
			{
				return _DataBranchRerouteNodes;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchRerouteNodeリスト
		/// </summary>
#else
		/// <summary>
		/// DataBranchRerouteNode list
		/// </summary>
#endif
		[System.Obsolete("use dataBranchRerouteNode")]
		public DataBranchRerouteNodeList calculatorBranchRerouteNodes
		{
			get
			{
				return dataBranchRerouteNodes;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchRerouteNodeを生成。
		/// </summary>
		/// <param name="position">ノードの位置</param>
		/// <param name="type">値の型</param>
		/// <param name="nodeID">ノード ID</param>
		/// <param name="direction">向き</param>
		/// <returns>生成したDataBranchRerouteNode。ノードIDが重複している場合は生成せずにnullを返す。</returns>
#else
		/// <summary>
		/// Create DataBranchRerouteNode.
		/// </summary>
		/// <param name="position">Position of the node</param>
		/// <param name="type">Value type</param>
		/// <param name="nodeID">Node ID</param>
		/// <param name="direction">Direction</param>
		/// <returns>The created DataBranchRerouteNode. If the node ID is not unique, return null without creating it.</returns>
#endif
		public DataBranchRerouteNode CreateDataBranchRerouteNode(Vector2 position, System.Type type, int nodeID, Vector2 direction)
		{
			if (!IsUniqueNodeID(nodeID))
			{
				Debug.LogWarning("CreateDataBranchRerouteNode id(" + nodeID + ") is not unique.");
				return null;
			}

			DataBranchRerouteNode rerouteNode = new DataBranchRerouteNode(this, nodeID, type);
			rerouteNode.position = new Rect(position.x, position.y, Node.defaultWidth, 0);
			rerouteNode.direction = direction;

			ComponentUtility.RecordObject(this, "Created DataBranchRerouteNode");

			_DataBranchRerouteNodes.Add(rerouteNode);
			RegisterNode(rerouteNode);

			ComponentUtility.SetDirty(this);

			return rerouteNode;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchRerouteNodeを生成。
		/// </summary>
		/// <param name="position">ノードの位置</param>
		/// <param name="type">値の型</param>
		/// <param name="nodeID">ノード ID</param>
		/// <returns>生成したDataBranchRerouteNode。ノードIDが重複している場合は生成せずにnullを返す。</returns>
#else
		/// <summary>
		/// Create DataBranchRerouteNode.
		/// </summary>
		/// <param name="position">Position of the node</param>
		/// <param name="type">Value type</param>
		/// <param name="nodeID">Node ID</param>
		/// <returns>The created DataBranchRerouteNode. If the node ID is not unique, return null without creating it.</returns>
#endif
		public DataBranchRerouteNode CreateDataBranchRerouteNode(Vector2 position, System.Type type, int nodeID)
		{
			return CreateDataBranchRerouteNode(position, type, nodeID, DataBranchRerouteNode.kDefaultDirection);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchRerouteNodeを生成。
		/// </summary>
		/// <param name="position">ノードの位置</param>
		/// <param name="type">値の型</param>
		/// <param name="nodeID">ノード ID</param>
		/// <returns>生成したDataBranchRerouteNode。ノードIDが重複している場合は生成せずにnullを返す。</returns>
#else
		/// <summary>
		/// Create DataBranchRerouteNode.
		/// </summary>
		/// <param name="position">Position of the node</param>
		/// <param name="type">Value type</param>
		/// <param name="nodeID">Node ID</param>
		/// <returns>The created DataBranchRerouteNode. If the node ID is not unique, return null without creating it.</returns>
#endif
		[System.Obsolete("use CreateDataBranchRerouteNode(Vector2 position, System.Type type, int nodeID)")]
		public DataBranchRerouteNode CreateCalculatorBranchRerouteNode(Vector2 position, System.Type type, int nodeID)
		{
			return CreateDataBranchRerouteNode(position, type, nodeID, DataBranchRerouteNode.kDefaultDirection);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchRerouteNodeを生成。
		/// </summary>
		/// <param name="position">ノードの位置</param>
		/// <param name="type">値の型</param>
		/// <param name="direction">向き</param>
		/// <returns>生成したDataBranchRerouteNode。</returns>
#else
		/// <summary>
		/// Create DataBranchRerouteNode.
		/// </summary>
		/// <param name="position">Position of the node</param>
		/// <param name="type">Value type</param>
		/// <param name="direction">Direction</param>
		/// <returns>The created DataBranchRerouteNode.</returns>
#endif
		public DataBranchRerouteNode CreateDataBranchRerouteNode(Vector2 position, System.Type type, Vector2 direction)
		{
			return CreateDataBranchRerouteNode(position, type, GetUniqueNodeID(), direction);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchRerouteNodeを生成。
		/// </summary>
		/// <param name="position">ノードの位置</param>
		/// <param name="type">値の型</param>
		/// <returns>生成したDataBranchRerouteNode。</returns>
#else
		/// <summary>
		/// Create DataBranchRerouteNode.
		/// </summary>
		/// <param name="position">Position of the node</param>
		/// <param name="type">Value type</param>
		/// <returns>The created DataBranchRerouteNode.</returns>
#endif
		public DataBranchRerouteNode CreateDataBranchRerouteNode(Vector2 position, System.Type type)
		{
			return CreateDataBranchRerouteNode(position, type, GetUniqueNodeID(), DataBranchRerouteNode.kDefaultDirection);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchRerouteNodeを生成。
		/// </summary>
		/// <param name="position">ノードの位置</param>
		/// <param name="type">値の型</param>
		/// <returns>生成したDataBranchRerouteNode。</returns>
#else
		/// <summary>
		/// Create DataBranchRerouteNode.
		/// </summary>
		/// <param name="position">Position of the node</param>
		/// <param name="type">Value type</param>
		/// <returns>The created DataBranchRerouteNode.</returns>
#endif
		[System.Obsolete("use CreateDataBranchRerouteNode(Vector2 position,System.Type type)")]
		public DataBranchRerouteNode CreateCalculatorBranchRerouteNode(Vector2 position, System.Type type)
		{
			return CreateDataBranchRerouteNode(position, type, GetUniqueNodeID(), DataBranchRerouteNode.kDefaultDirection);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchRerouteNodeの削除。
		/// </summary>
		/// <param name="rerouteNode">削除するDataBranchRerouteNode。</param>
#else
		/// <summary>
		/// Delete DataBranchRerouteNode.
		/// </summary>
		/// <param name="rerouteNode">DataBranchRerouteNode that you want to delete.</param>
#endif
		public void DeleteDataBranchRerouteNode(DataBranchRerouteNode rerouteNode)
		{
			ComponentUtility.RegisterCompleteObjectUndo(this, "Delete Nodes");

			ComponentUtility.RecordObject(this, "Delete Nodes");

			DataBranch inputBranch = rerouteNode.link.inputSlot.GetBranch();
			if (inputBranch != null)
			{
				DeleteDataBranch(inputBranch);
			}

			int branchCount = rerouteNode.link.outputSlot.branchCount;
			for (int branchIndex = branchCount - 1; branchIndex >= 0; --branchIndex)
			{
				DataBranch branch = rerouteNode.link.outputSlot.GetBranch(branchIndex);
				if (branch != null)
				{
					DeleteDataBranch(branch);
				}
			}

			_DataBranchRerouteNodes.Remove(rerouteNode);
			RemoveNode(rerouteNode);

			ComponentUtility.SetDirty(this);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchRerouteNodeの削除。
		/// </summary>
		/// <param name="rerouteNode">削除するDataBranchRerouteNode。</param>
#else
		/// <summary>
		/// Delete DataBranchRerouteNode.
		/// </summary>
		/// <param name="rerouteNode">DataBranchRerouteNode that you want to delete.</param>
#endif
		[System.Obsolete("use DeleteDataBranchRerouteNode(DataBranchRerouteNode rerouteNode)")]
		public void DeleteCalculatorBranchRerouteNode(DataBranchRerouteNode rerouteNode)
		{
			DeleteDataBranchRerouteNode(rerouteNode);
		}

		internal bool ContainsNode(Node node)
		{
			return _Nodes.Contains(node);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードを一覧に登録する
		/// </summary>
		/// <param name="node">登録するノード</param>
#else
		/// <summary>
		/// Register the node in the list
		/// </summary>
		/// <param name="node">Node to register</param>
#endif
		protected void RegisterNode(Node node)
		{
			if (_Nodes.Contains(node))
			{
				return;
			}

			_Nodes.Add(node);
			_DicNodes.Add(node.nodeID, node);

			var behaviourContainer = node as INodeBehaviourContainer;
			if (behaviourContainer != null)
			{
				for (int i = 0, count = behaviourContainer.GetNodeBehaviourCount(); i < count; i++)
				{
					var behaviour = behaviourContainer.GetNodeBehaviour<NodeBehaviour>(i);
					if (!ReferenceEquals(behaviour,null) && !behaviour.attachedNode)
					{
						behaviour.OnAttachToNode();
					}
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードを一覧から削除する。
		/// </summary>
		/// <param name="node">削除するノード</param>
#else
		/// <summary>
		/// Remove the node from the list.
		/// </summary>
		/// <param name="node">Node to remove</param>
#endif
		protected void RemoveNode(Node node)
		{
			_Nodes.Remove(node);
			_DicNodes.Remove(node.nodeID);
		}

		void ClearNodes()
		{
			_DicCalculators.Clear();
			_DicComments.Clear();
			_DicGroups.Clear();

			_DicNodes.Clear();
			_Nodes.Clear();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードの削除。
		/// </summary>
		/// <param name="node">削除するノード</param>
		/// <returns>削除した場合はtrue、していなければfalseを返す。</returns>
#else
		/// <summary>
		/// Delete node.
		/// </summary>
		/// <param name="node">The node to delete</param>
		/// <returns>Returns true if deleted, false otherwise.</returns>
#endif
		protected abstract bool OnDeleteNode(Node node);

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードが変更された際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when the node is changed.
		/// </summary>
#endif
		public virtual void OnValidateNodes()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードの削除。
		/// </summary>
		/// <param name="node">削除するノード。</param>
		/// <returns>削除した場合にtrue</returns>
#else
		/// <summary>
		/// Delete node.
		/// </summary>
		/// <param name="node">Node that you want to delete.</param>
		/// <returns>true if deleted</returns>
#endif
		public bool DeleteNode(Node node)
		{
			if (!node.IsDeletable())
			{
				return false;
			}

			CalculatorNode calculatorNode = node as CalculatorNode;
			if (calculatorNode != null)
			{
				return DeleteCalculator(calculatorNode);
			}

			CommentNode commentNode = node as CommentNode;
			if (commentNode != null)
			{
				DeleteComment(commentNode);
				return true;
			}

			GroupNode groupNode = node as GroupNode;
			if (groupNode != null)
			{
				DeleteGroup(groupNode);
				return true;
			}

			DataBranchRerouteNode rerouteNode = node as DataBranchRerouteNode;
			if (rerouteNode != null)
			{
				DeleteDataBranchRerouteNode(rerouteNode);
				return true;
			}

			return OnDeleteNode(node);
		}

#if !NETFX_CORE
		[System.Reflection.Obfuscation(Exclude = true)]
#endif
		private bool _IsEditor = false;

		bool IsEditor()
		{
			NodeGraph current = this;

			while (current != null)
			{
				if (current._IsEditor)
				{
					return true;
				}
				current = current.parentGraph;
			}

			return false;
		}

		bool IsMove()
		{
			if (IsEditor())
			{
				return false;
			}

			int count = nodeCount;
			for (int nodeIndex = 0; nodeIndex < count; nodeIndex++)
			{
				Node node = GetNodeFromIndex(nodeIndex);
				if (node.nodeGraph != this)
				{
					return true;
				}
				else
				{
					INodeBehaviourContainer behaviours = node as INodeBehaviourContainer;
					if (behaviours != null)
					{
						int behaviourCount = behaviours.GetNodeBehaviourCount();
						for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
						{
							NodeBehaviour behaviour = behaviours.GetNodeBehaviour<NodeBehaviour>(behaviourIndex);
							if (behaviour != null && behaviour.nodeGraph != this)
							{
								return true;
							}
						}
					}
				}
			}

			return false;
		}

		bool IsMoveContainer()
		{
			if (IsEditor())
			{
				return false;
			}

			if (_ParameterContainer != null && _ParameterContainer.owner != this)
			{
				return true;
			}

			return false;
		}

		void MoveBranch(DataBranch branch)
		{
			int inNodeID = branch.inNodeID;
			Object inBehaviour = branch.inBehaviour;
			int outNodeID = branch.outNodeID;
			Object outBehaviour = branch.outBehaviour;
			
			int count = nodeCount;
			for (int nodeIndex = 0; nodeIndex < count; nodeIndex++)
			{
				Node node = GetNodeFromIndex(nodeIndex);

				INodeBehaviourContainer nodeBehaviours = node as INodeBehaviourContainer;
				if (nodeBehaviours != null)
				{
					int behaviourCount = nodeBehaviours.GetNodeBehaviourCount();
					for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
					{
						NodeBehaviour behaviour = nodeBehaviours.GetNodeBehaviour<NodeBehaviour>(behaviourIndex);

						int slotCount = behaviour.dataSlotCount;
						for (int slotIndex = 0; slotIndex < slotCount; slotIndex++)
						{
							DataSlot s = behaviour.GetDataSlot(slotIndex);
							if (s == null)
							{
								continue;
							}

							IInputSlot inSlot = s as IInputSlot;
							if (inSlot != null)
							{
								if (inSlot.IsConnected(branch))
								{
									inNodeID = node.nodeID;
									inBehaviour = behaviour;
								}
							}

							IOutputSlot outSlot = s as IOutputSlot;
							if (outSlot != null)
							{
								if (outSlot.IsConnected(branch))
								{
									outNodeID = node.nodeID;
									outBehaviour = behaviour;
								}
							}
						}
					}
				}

				DataBranchRerouteNode rerouteNode = node as DataBranchRerouteNode;
				if (rerouteNode != null)
				{
					RerouteSlot link = rerouteNode.link;
					if (link.inputSlot.IsConnected(branch))
					{
						inNodeID = node.nodeID;
						inBehaviour = null;
					}

					if (link.outputSlot.IsConnected(branch))
					{
						outNodeID = node.nodeID;
						outBehaviour = null;
					}
				}
			}

			if (inBehaviour == null)
			{
				inBehaviour = this;
			}
			if (outBehaviour == null)
			{
				outBehaviour = this;
			}
			branch.SetBehaviour(inNodeID, inBehaviour, outNodeID, outBehaviour);

			branch.inputSlot.nodeGraph = branch.outputSlot.nodeGraph = this;

			branch.RebuildSlotField();
		}

		void DestroyUnusedNodeBehaviour(NodeBehaviour behaviour)
		{
			if (behaviour.nodeGraph != this)
			{
				return;
			}

			Node node = FindNodeContainsBehaviour(behaviour);
			if (node == null)
			{
				NodeBehaviour.Destroy(behaviour);
			}
		}

		void DestroyUnusedParameterContainer(ParameterContainerInternal container)
		{
			if (container.owner != this)
			{
				return;
			}

			if (_ParameterContainer != container)
			{
				ParameterContainerInternal.Destroy(container);
			}
		}

		void DestroyUnusedSubComponents()
		{
			if (this == null)
			{
				return;
			}

			var containers = this.GetComponentsTemp<ParameterContainerInternal>();
			for (int containerIndex = 0; containerIndex < containers.Count; containerIndex++)
			{
				ParameterContainerInternal container = containers[containerIndex];
				DestroyUnusedParameterContainer(container);
			}

			var behaviours = this.GetComponentsTemp<NodeBehaviour>();
			for (int behaviourIndex = 0; behaviourIndex < behaviours.Count; behaviourIndex++)
			{
				NodeBehaviour behaviour = behaviours[behaviourIndex];
				DestroyUnusedNodeBehaviour(behaviour);
			}
		}

		private bool _IsValidateDelay = false;
		private string _OldGraphName;

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はスクリプトがロードされた時やインスペクターの値が変更されたときに呼び出される（この呼出はエディター上のみ）
		/// </summary>
#else
		/// <summary>
		/// This function is called when the script is loaded or when the inspector value changes (this call is only in the editor)
		/// </summary>
#endif
		protected virtual void OnValidate()
		{
			if (_GraphName != _OldGraphName)
			{
				_OldGraphName = _GraphName;
				CallChangedGraphName();
			}

			if (!_IsValidateDelay)
			{
				_IsValidateDelay = true;
				ComponentUtility.DelayCall(OnValidateDelay);
			}

			ComponentUtility.RefreshNodeGraph(this);
		}

		internal bool IsValidDataBranch(DataBranch branch)
		{
			if (branch == null)
			{
				return false;
			}

			if (!DataSlot.IsConnectable(branch.inputSlot, branch.outputSlot))
			{
				return false;
			}

			if (CheckLoopDataBranch(branch.inNodeID, branch.inBehaviour, branch.outNodeID, branch.outBehaviour))
			{
				return false;
			}

			return true;
		}

		void GarbageCollectDataBranchies()
		{
			List<DataBranch> deleteBranchies = new List<DataBranch>();

			int branchCount = _DataBranchies.Count;
			for (int branchIndex = 0; branchIndex < branchCount; branchIndex++)
			{
				DataBranch branch = _DataBranchies[branchIndex];
				if (branch.branchID == 0)
				{
					Debug.LogError("branch id 0");
				}

				if (!IsValidDataBranch(branch))
				{
					deleteBranchies.Add(branch);
				}
			}

			for (int branchIndex = 0; branchIndex < deleteBranchies.Count; branchIndex++)
			{
				DataBranch branch = deleteBranchies[branchIndex];
				DeleteDataBranch(branch);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの接続状態を更新する。
		/// </summary>
#else
		/// <summary>
		/// Update the connection status of Data Branch.
		/// </summary>
#endif
		public void RefreshDataBranchies()
		{
			if (_IsEditor)
			{
				return;
			}

			int branchCount = _DataBranchies.Count;
			for (int branchIndex = 0; branchIndex < branchCount; branchIndex++)
			{
				DataBranch branch = _DataBranchies[branchIndex];
				branch.SetDirtySlotField();
			}

			for (int nodeIndex = 0, nodeCount = _Nodes.Count; nodeIndex < nodeCount; nodeIndex++)
			{
				Node node = _Nodes[nodeIndex];
				INodeBehaviourContainer behaviours = node as INodeBehaviourContainer;
				if (behaviours != null)
				{
					int behaviourCount = behaviours.GetNodeBehaviourCount();
					for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
					{
						NodeBehaviour behaviour = behaviours.GetNodeBehaviour<NodeBehaviour>(behaviourIndex);
						if (behaviour == null)
						{
							continue;
						}

						behaviour.RefreshDataSlots();
					}
				}
				else
				{
					DataBranchRerouteNode rerouteNode = node as DataBranchRerouteNode;
					if (rerouteNode != null)
					{
						rerouteNode.RefreshDataSlots();
					}
				}
			}

			GarbageCollectDataBranchies();
		}

		void OnValidateDelay()
		{
			if (this == null || gameObject == null)
			{
				return;
			}

			// Maintains compatibility with versions (Arbor 3.0 or earlier) that allowed NodeGraph to be moved to another GameObject.
			if (IsMove())
			{
				for (int count = nodeCount, nodeIndex = 0; nodeIndex < count; nodeIndex++)
				{
					Node node = GetNodeFromIndex(nodeIndex);
					node.ChangeGraph(this);
				}

				int branchCount = dataBranchCount;
				for (int branchIndex = 0; branchIndex < branchCount; branchIndex++)
				{
					DataBranch branch = GetDataBranchFromIndex(branchIndex);

					MoveBranch(branch);
				}
			}

			if (IsMoveContainer())
			{
				if (_ParameterContainer != null)
				{
					ComponentUtility.MoveParameterContainer(this);
				}
			}

			DestroyUnusedSubComponents();

			_IsValidateDelay = false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeBehaviourが属しているノードの取得。
		/// </summary>
		/// <param name="behaviour">NodeBehaviour</param>
		/// <returns>NodeBehaviourが属しているノード。ない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Acquisition of nodes NodeBehaviour belongs.
		/// </summary>
		/// <param name="behaviour">NodeBehaviour</param>
		/// <returns>Nodess NodeBehaviour belongs. Return null if not.</returns>
#endif
		public Node FindNodeContainsBehaviour(NodeBehaviour behaviour)
		{
			for (int count = _Nodes.Count, nodeIndex = 0; nodeIndex < count; nodeIndex++)
			{
				Node node = _Nodes[nodeIndex];
				if (node.IsContainsBehaviour(behaviour))
				{
					return node;
				}
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの数を取得。
		/// </summary>
#else
		/// <summary>
		///  Get a count of DataBranch.
		/// </summary>
#endif
		public int dataBranchCount
		{
			get
			{
				return _DataBranchies.Count;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの数を取得。
		/// </summary>
#else
		/// <summary>
		///  Get a count of DataBranch.
		/// </summary>
#endif
		[System.Obsolete("use dataBranchCount")]
		public int calculatorBranchCount
		{
			get
			{
				return dataBranchCount;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchをインデックスから取得
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>DataBranch</returns>
#else
		/// <summary>
		/// Get DataBranch from index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>DataBranch</returns>
#endif
		public DataBranch GetDataBranchFromIndex(int index)
		{
			return _DataBranchies[index];
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchをインデックスから取得
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>DataBranch</returns>
#else
		/// <summary>
		/// Get DataBranch from index.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>DataBranch</returns>
#endif
		[System.Obsolete("use GetDataBranchFromIndex(int index)")]
		public DataBranch GetCalculatorBranchFromIndex(int index)
		{
			return GetDataBranchFromIndex(index);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchのインデックスを取得
		/// </summary>
		/// <param name="branch">DataBranch</param>
		/// <returns>インデックス。ない場合は-1を返す。</returns>
#else
		/// <summary>
		/// Get DataBranch index.
		/// </summary>
		/// <param name="branch">DataBranch</param>
		/// <returns>Index. If not, it returns -1.</returns>
#endif
		public int GetDataBranchIndex(DataBranch branch)
		{
			return _DataBranchies.IndexOf(branch);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchのインデックスを取得
		/// </summary>
		/// <param name="branch">DataBranch</param>
		/// <returns>インデックス。ない場合は-1を返す。</returns>
#else
		/// <summary>
		/// Get DataBranch index.
		/// </summary>
		/// <param name="branch">DataBranch</param>
		/// <returns>Index. If not, it returns -1.</returns>
#endif
		[System.Obsolete("use GetDataBranchIndex(DataBranch)")]
		public int GetCalculatorBranchIndex(DataBranch branch)
		{
			return GetDataBranchIndex(branch);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 全ての<see cref="Arbor.DataBranch" />を取得する。
		/// </summary>
#else
		/// <summary>
		/// Gets all of <see cref = "Arbor.DataBranch" />.
		/// </summary>
#endif
		[System.Obsolete("use dataBranchCount and GetDataBranchFromIndex()")]
		public DataBranch[] calculatorBranchies
		{
			get
			{
				return _DataBranchies.ToArray();
			}
		}

		int GetUniqueBranchID()
		{
			int count = _DataBranchies.Count;

			System.Random random = new System.Random(count);

			while (true)
			{
				int branchID = random.Next();

				if (branchID != 0 && !_DicDataBranchies.ContainsKey(branchID))
				{
					return branchID;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算ブランチIDを指定して<see cref="Arbor.DataBranch" />を取得する。
		/// </summary>
		/// <param name="branchID">演算ブランチID</param>
		/// <returns>見つかった<see cref="Arbor.DataBranch" />。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Gets <see cref="Arbor.DataBranch" /> from the calculator branch identifier.
		/// </summary>
		/// <param name="branchID">The calculator branch identifier.</param>
		/// <returns>Found <see cref = "Arbor.DataBranch" />. Returns null if not found.</returns>
#endif
		public DataBranch GetDataBranchFromID(int branchID)
		{
			if (branchID == 0)
			{
				return null;
			}

			DataBranch branch = null;
			if (_DicDataBranchies.TryGetValue(branchID, out branch))
			{
				return branch;
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算ブランチIDを指定して<see cref="Arbor.DataBranch" />を取得する。
		/// </summary>
		/// <param name="branchID">演算ブランチID</param>
		/// <returns>見つかった<see cref="Arbor.DataBranch" />。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Gets <see cref="Arbor.DataBranch" /> from the calculator branch identifier.
		/// </summary>
		/// <param name="branchID">The calculator branch identifier.</param>
		/// <returns>Found <see cref = "Arbor.DataBranch" />. Returns null if not found.</returns>
#endif
		[System.Obsolete("use GetDataBranchFromID(int)")]
		public DataBranch GetCalculatorBranchFromID(int branchID)
		{
			return GetDataBranchFromID(branchID);
		}

		DataBranch CreateDataBranch(int branchID)
		{
			DataBranch branch = new DataBranch();
			branch.branchID = branchID;

			ComponentUtility.RecordObject(this, "Created Branch");

			_DataBranchies.Add(branch);
			_DicDataBranchies.Add(branch.branchID, branch);

			ComponentUtility.SetDirty(this);

			return branch;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotの接続
		/// </summary>
		/// <param name="branchID">作成するDataBranchのID</param>
		/// <param name="inputNodeID">入力ノードID</param>
		/// <param name="inputObj">入力オブジェクト</param>
		/// <param name="inputSlot">入力スロット</param>
		/// <param name="outputNodeID">出力ノードID</param>
		/// <param name="outputObj">出力オブジェクト</param>
		/// <param name="outputSlot">出力スロット</param>
		/// <returns>接続したDataBranch</returns>
#else
		/// <summary>
		/// Connect DataSlot.
		/// </summary>
		/// <param name="branchID">ID of the DataBranch to be created</param>
		/// <param name="inputNodeID">Input node ID.</param>
		/// <param name="inputObj">Input object.</param>
		/// <param name="inputSlot">Input slot.</param>
		/// <param name="outputNodeID">Output node ID.</param>
		/// <param name="outputObj">Output object.</param>
		/// <param name="outputSlot">Output slot.</param>
		/// <returns>Connected DataBranch</returns>
#endif
		public DataBranch ConnectDataBranch(int branchID, int inputNodeID, Object inputObj, DataSlot inputSlot, int outputNodeID, Object outputObj, DataSlot outputSlot)
		{
			if (GetDataBranchFromID(branchID) != null)
			{
				Debug.LogError("It already exists branchID.");
				return null;
			}

			if (CheckLoopDataBranch(inputNodeID, inputObj, outputNodeID, outputObj))
			{
				Debug.LogError("Calculator node has become an infinite loop.");
				return null;
			}

			if (inputSlot != null && inputSlot.slotType == SlotType.Output)
			{
				throw new System.ArgumentException("inputSlot is not InputSlot or RerouteSlot");
			}

			if (outputSlot != null && outputSlot.slotType == SlotType.Input)
			{
				throw new System.ArgumentException("outputSlot is not OutputSlot or RerouteSlot");
			}

			DataBranch branch = CreateDataBranch(branchID);

			Object setInputObj = inputObj;
			if (setInputObj == null)
			{
				setInputObj = this;
			}
			Object setOutputObj = outputObj;
			if (setOutputObj == null)
			{
				setOutputObj = this;
			}
			branch.SetBehaviour(inputNodeID, setInputObj, outputNodeID, setOutputObj);

			List<Object> records = new List<Object>();
			records.Add(this);
			if (inputObj != null)
			{
				records.Add(inputObj);
			}
			if (outputObj != null)
			{
				records.Add(outputObj);
			}
			ComponentUtility.RecordObjects(records.ToArray(), "Connect Calculator");

			if (outputSlot != null)
			{
				Node outputNode = GetNodeFromID(outputNodeID);
				if (outputNode != null) // Do not rewrite uncopied nodes when copying to the clipboard.
				{
					outputSlot.nodeGraph = this;
					IOutputSlot outSlot = outputSlot as IOutputSlot;
					if (outSlot != null)
					{
						outSlot.AddBranch(branch);
					}
				}
				branch._OutputSlot = outputSlot;
			}

			if (inputSlot != null)
			{
				Node inputNode = GetNodeFromID(inputNodeID);
				if (inputNode != null)  // Do not rewrite uncopied nodes when copying to the clipboard.
				{
					inputSlot.nodeGraph = this;
					IInputSlot inSlot = inputSlot as IInputSlot;
					if (inSlot != null)
					{
						inSlot.SetBranch(branch);
					}
				}
				branch._InputSlot = inputSlot;
			}

			if (inputObj != null)
			{
				ComponentUtility.SetDirty(inputObj);
			}
			if (outputObj != null)
			{
				ComponentUtility.SetDirty(outputObj);
			}
			ComponentUtility.SetDirty(this);

			return branch;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotの接続
		/// </summary>
		/// <param name="branchID">作成するDataBranchのID</param>
		/// <param name="inputNodeID">入力ノードID</param>
		/// <param name="inputObj">入力オブジェクト</param>
		/// <param name="inputSlot">入力スロット</param>
		/// <param name="outputNodeID">出力ノードID</param>
		/// <param name="outputObj">出力オブジェクト</param>
		/// <param name="outputSlot">出力スロット</param>
		/// <returns>接続したDataBranch</returns>
#else
		/// <summary>
		/// Connect DataSlot.
		/// </summary>
		/// <param name="branchID">ID of the DataBranch to be created</param>
		/// <param name="inputNodeID">Input node ID.</param>
		/// <param name="inputObj">Input object.</param>
		/// <param name="inputSlot">Input slot.</param>
		/// <param name="outputNodeID">Output node ID.</param>
		/// <param name="outputObj">Output object.</param>
		/// <param name="outputSlot">Output slot.</param>
		/// <returns>Connected DataBranch</returns>
#endif
		[System.Obsolete("use ConnectDataBranch(int branchID, int inputNodeID, Object inputObj, DataSlot inputSlot, int outputNodeID, Object outputObj, DataSlot outputSlot)")]
		public DataBranch ConnectCalculatorBranch(int branchID, int inputNodeID, Object inputObj, DataSlot inputSlot, int outputNodeID, Object outputObj, DataSlot outputSlot)
		{
			return ConnectDataBranch(branchID, inputNodeID, inputObj, inputSlot, outputNodeID, outputObj, outputSlot);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotの接続
		/// </summary>
		/// <param name="inputNodeID">入力ノードID</param>
		/// <param name="inputObj">入力オブジェクト</param>
		/// <param name="inputSlot">入力スロット</param>
		/// <param name="outputNodeID">出力ノードID</param>
		/// <param name="outputObj">出力オブジェクト</param>
		/// <param name="outputSlot">出力スロット</param>
		/// <returns>接続したDataBranch</returns>
#else
		/// <summary>
		/// Connect DataSlot.
		/// </summary>
		/// <param name="inputNodeID">Input node ID.</param>
		/// <param name="inputObj">Input object.</param>
		/// <param name="inputSlot">Input slot.</param>
		/// <param name="outputNodeID">Output node ID.</param>
		/// <param name="outputObj">Output object.</param>
		/// <param name="outputSlot">Output slot.</param>
		/// <returns>Connected DataBranch</returns>
#endif
		public DataBranch ConnectDataBranch(int inputNodeID, Object inputObj, DataSlot inputSlot, int outputNodeID, Object outputObj, DataSlot outputSlot)
		{
			return ConnectDataBranch(GetUniqueBranchID(), inputNodeID, inputObj, inputSlot, outputNodeID, outputObj, outputSlot);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotの接続
		/// </summary>
		/// <param name="inputNodeID">入力ノードID</param>
		/// <param name="inputObj">入力オブジェクト</param>
		/// <param name="inputSlot">入力スロット</param>
		/// <param name="outputNodeID">出力ノードID</param>
		/// <param name="outputObj">出力オブジェクト</param>
		/// <param name="outputSlot">出力スロット</param>
		/// <returns>接続したDataBranch</returns>
#else
		/// <summary>
		/// Connect DataSlot.
		/// </summary>
		/// <param name="inputNodeID">Input node ID.</param>
		/// <param name="inputObj">Input object.</param>
		/// <param name="inputSlot">Input slot.</param>
		/// <param name="outputNodeID">Output node ID.</param>
		/// <param name="outputObj">Output object.</param>
		/// <param name="outputSlot">Output slot.</param>
		/// <returns>Connected DataBranch</returns>
#endif
		[System.Obsolete("use ConnectDataBranch(int inputNodeID, Object inputObj, DataSlot inputSlot, int outputNodeID, Object outputObj, DataSlot outputSlot)")]
		public DataBranch ConnectCalculatorBranch(int inputNodeID, Object inputObj, DataSlot inputSlot, int outputNodeID, Object outputObj, DataSlot outputSlot)
		{
			return ConnectDataBranch(inputNodeID, inputObj, inputSlot, outputNodeID, outputObj, outputSlot);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 内部的に使用するメソッド。特に呼び出す必要はありません。
		/// </summary>
		/// <param name="obj">Object</param>
#else
		/// <summary>
		/// Method to be used internally. In particular there is no need to call.
		/// </summary>
		/// <param name="obj">Object</param>
#endif
		public void DisconnectDataBranch(Object obj)
		{
			if (obj == null)
			{
				return;
			}

			ComponentUtility.RecordObject(obj, "Disconnect DataBranch");

			List<DataBranch> branchies = new List<DataBranch>();

			for (int branchIndex = 0; branchIndex < _DataBranchies.Count; branchIndex++)
			{
				DataBranch branch = _DataBranchies[branchIndex];
				if (branch.inBehaviour == obj || branch.outBehaviour == obj)
				{
					branchies.Add(branch);
				}
			}

			int branchCount = branchies.Count;
			for (int branchIndex = 0; branchIndex < branchCount; branchIndex++)
			{
				DataBranch branch = branchies[branchIndex];
				DeleteDataBranch(branch);
			}

			ComponentUtility.SetDirty(obj);
		}

		internal void Internal_DeleteDataBranch(DataBranch branch, Object ignoreRecord)
		{
			branch.OnDisconnected();

			if (_DataBranchies.Remove(branch))
			{
				_DicDataBranchies.Remove(branch.branchID);
			}

			if (branch.outBehaviour != ignoreRecord)
			{
				DataSlot outputSlot = branch.outputSlot;
				IOutputSlot outSlot = outputSlot as IOutputSlot;
				if (outSlot != null)
				{
					outSlot.RemoveBranch(branch);
				}
			}

			if (branch.inBehaviour != ignoreRecord)
			{
				DataSlot inputSlot = branch.inputSlot;
				IInputSlot inSlot = inputSlot as IInputSlot;
				if (inSlot != null)
				{
					inSlot.RemoveBranch(branch);
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの削除。
		/// </summary>
		/// <param name="branch">削除するDataBranch。</param>
		/// <param name="ignoreRecord">Undoへの記録を無視するオブジェクト</param>
#else
		/// <summary>
		/// Delete DataBranch.
		/// </summary>
		/// <param name="branch">DataBranch that you want to delete.</param>
		/// <param name="ignoreRecord">Objects that ignore recordings to Undo</param>
#endif
		public void DeleteDataBranch(DataBranch branch, Object ignoreRecord)
		{
			List<Object> records = new List<Object>();
			records.Add(this);

			Object inBehaviour = branch.inBehaviour;
			if (inBehaviour != null && inBehaviour is MonoBehaviour && inBehaviour != ignoreRecord)
			{
				records.Add(inBehaviour);
			}
			Object outBehaviour = branch.outBehaviour;
			if (outBehaviour != null && outBehaviour is MonoBehaviour && outBehaviour != ignoreRecord)
			{
				records.Add(outBehaviour);
			}
			ComponentUtility.RecordObjects(records.ToArray(), "Delete Branch");

			Internal_DeleteDataBranch(branch, ignoreRecord);

			ComponentUtility.SetDirty(this);
			if (outBehaviour != null && outBehaviour != ignoreRecord)
			{
				ComponentUtility.SetDirty(outBehaviour);
			}
			if (inBehaviour != null && inBehaviour != ignoreRecord)
			{
				ComponentUtility.SetDirty(inBehaviour);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの削除。
		/// </summary>
		/// <param name="branch">削除するDataBranch。</param>
#else
		/// <summary>
		/// Delete DataBranch.
		/// </summary>
		/// <param name="branch">DataBranch that you want to delete.</param>
#endif
		public void DeleteDataBranch(DataBranch branch)
		{
			DeleteDataBranch(branch, null);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの削除。
		/// </summary>
		/// <param name="branch">削除するDataBranch。</param>
#else
		/// <summary>
		/// Delete DataBranch.
		/// </summary>
		/// <param name="branch">DataBranch that you want to delete.</param>
#endif
		[System.Obsolete("use DeleteDataBranch(DataBranch)")]
		public void DeleteCalculatorBranch(DataBranch branch)
		{
			DeleteDataBranch(branch);
		}

		bool _IsDelayRefresh = false;

		internal void DelayRefresh()
		{
			if (!_IsDelayRefresh)
			{
				_IsDelayRefresh = true;
				ComponentUtility.DelayCall(OnRefreshDelay);
			}
		}

		void OnRefreshDelay()
		{
			if (_IsDelayRefresh && this != null)
			{
				Refresh();
			}

			_IsDelayRefresh = false;
		}

		internal void Refresh()
		{
			bool isPlaying = Application.isPlaying && isActiveAndEnabled;

			if (!_IsEditor && rootGraph == this)
			{
				hideFlags &= ~(HideFlags.HideInInspector);
			}

			int nodeCount = _Nodes.Count;
			for (int nodeIndex = 0; nodeIndex < nodeCount; nodeIndex++)
			{
				INodeBehaviourContainer behaviours = _Nodes[nodeIndex] as INodeBehaviourContainer;
				if (behaviours == null)
				{
					continue;
				}

				int behaviourCount = behaviours.GetNodeBehaviourCount();
				for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
				{
					Object behaviourObj = behaviours.GetNodeBehaviour<Object>(behaviourIndex);
					NodeBehaviour.RefreshBehaviour(behaviourObj, isPlaying);
				}
			}

			RefreshDataBranchies();
			
			_IsDelayRefresh = false;
		}

		/// <summary>
		/// Register nodes
		/// </summary>
		protected abstract void OnRegisterNodes();

		void RegisterNodes()
		{
			ClearNodes();

			for (int i = 0; i < _Calculators.Count; i++)
			{
				CalculatorNode calculatorNode = _Calculators[i];
				_DicCalculators.Add(calculatorNode.nodeID, calculatorNode);
				RegisterNode(calculatorNode);
			}

			for (int i = 0; i < _Comments.Count; i++)
			{
				CommentNode commentNode = _Comments[i];
				_DicComments.Add(commentNode.nodeID, commentNode);
				RegisterNode(_Comments[i]);
			}

			for (int i = 0; i < _Groups.Count; i++)
			{
				GroupNode groupNode = _Groups[i];
				_DicGroups.Add(groupNode.nodeID, groupNode);
				RegisterNode(_Groups[i]);
			}

			for (int i = 0; i < _DataBranchRerouteNodes.count; i++)
			{
				RegisterNode(_DataBranchRerouteNodes[i]);
			}

			OnRegisterNodes();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// デシリアライズ済みかどうかを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns whether or not deserialization has been done.
		/// </summary>
#endif
		public bool isDeserialized
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// デシリアライズ後のコールバック
		/// </summary>
#else
		/// <summary>
		/// Callback after deserialization
		/// </summary>
#endif
		public event System.Action onAfterDeserialize;

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			_DicDataBranchies.Clear();

			for (int i = 0, count = _DataBranchies.Count; i < count; i++)
			{
				DataBranch branch = _DataBranchies[i];
				if (branch == null || branch.branchID == 0)
				{
					continue;
				}

				_DicDataBranchies.Add(branch.branchID, branch);
			}

			RegisterNodes();

			onAfterDeserialize?.Invoke();
			onAfterDeserialize = null;

			isDeserialized = true;

			if (ComponentUtility.editorProcessor != null)
			{
				DelayRefresh();
			}
		}

		private static NodeGraph GetGraphInternal(IList<NodeGraph> graphs, System.Type type, string name)
		{
			for (int graphIndex = 0; graphIndex < graphs.Count; graphIndex++)
			{
				NodeGraph graph = graphs[graphIndex];
				if (graph.graphName.Equals(name))
				{
					System.Type classType = graph.GetType();

					if (TypeUtility.IsAssignableFrom(type, classType))
					{
						return graph;
					}
				}
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// シーン内にあるNodeGraphを名前で取得する。
		/// </summary>
		/// <param name="name">検索するNodeGraphの名前。</param>
		/// <returns>見つかったNodeGraph。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the NodeGraph that in the scene with the name.
		/// </summary>
		/// <param name="name">The name of the search NodeGraph.</param>
		/// <returns>Found NodeGraph. Returns null if not found.</returns>
#endif
		public static NodeGraph FindGraph(string name)
		{
			return FindGraph(name, typeof(NodeGraph));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// シーン内にあるNodeGraphを名前で取得する。
		/// </summary>
		/// <param name="name">検索するNodeGraphの名前。</param>
		/// <param name="type">検索するNodeGraphのType。</param>
		/// <returns>見つかったNodeGraph。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the NodeGraph that in the scene with the name.
		/// </summary>
		/// <param name="name">The name of the search NodeGraph.</param>
		/// <param name="type">The type of the search NodeGraph.</param>
		/// <returns>Found NodeGraph. Returns null if not found.</returns>
#endif
		public static NodeGraph FindGraph(string name, System.Type type)
		{
			return GetGraphInternal(ObjectUtility.FindObjectsOfType<NodeGraph>(), type, name);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// シーン内にあるNodeGraphを名前で取得する。
		/// </summary>
		/// <typeparam name="T">検索するNodeGraphのType。</typeparam>
		/// <param name="name">検索するNodeGraphの名前。</param>
		/// <returns>見つかったNodeGraph。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the NodeGraph that in the scene with the name.
		/// </summary>
		/// <typeparam name="T">The type of the search NodeGraph.</typeparam>
		/// <param name="name">The name of the search NodeGraph.</param>
		/// <returns>Found NodeGraph. Returns null if not found.</returns>
#endif
		public static T FindGraph<T>(string name) where T : NodeGraph
		{
			return GetGraphInternal(ObjectUtility.FindObjectsOfType<NodeGraph>(), typeof(T), name) as T;
		}

		private static System.Array GetGraphsInternal(IList<NodeGraph> graphs, System.Type type, string name, bool useSearchTypeAsArrayReturnType)
		{
			System.Collections.ArrayList array = new System.Collections.ArrayList();

			for (int graphIndex = 0; graphIndex < graphs.Count; graphIndex++)
			{
				NodeGraph graph = graphs[graphIndex];
				if (graph.graphName.Equals(name))
				{
					System.Type classType = graph.GetType();

					if (TypeUtility.IsAssignableFrom(type, classType))
					{
						array.Add(graph as NodeGraph);
					}
				}
			}

			if (useSearchTypeAsArrayReturnType)
			{
				return array.ToArray(type);
			}
			else
			{
				return array.ToArray();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// シーン内にある同一名のNodeGraphを取得する。
		/// </summary>
		/// <param name="name">検索するNodeGraphの名前。</param>
		/// <returns>見つかったNodeGraphの配列。</returns>
#else
		/// <summary>
		/// Get the NodeGraph of the same name that is in the scene.
		/// </summary>
		/// <param name="name">The name of the search NodeGraph.</param>
		/// <returns>Array of found NodeGraph.</returns>
#endif
		public static NodeGraph[] FindGraphs(string name)
		{
			return FindGraphs(name, typeof(NodeGraph));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// シーン内にある同一名のNodeGraphを取得する。
		/// </summary>
		/// <param name="name">検索するNodeGraphの名前。</param>
		/// <param name="type">検索するNodeGraphのType。</param>
		/// <returns>見つかったNodeGraphの配列。</returns>
#else
		/// <summary>
		/// Get the NodeGraph of the same name that is in the scene.
		/// </summary>
		/// <param name="name">The name of the search NodeGraph.</param>
		/// <param name="type">The type of the search NodeGraph.</param>
		/// <returns>Array of found NodeGraph.</returns>
#endif
		public static NodeGraph[] FindGraphs(string name, System.Type type)
		{
			return (NodeGraph[])GetGraphsInternal(ObjectUtility.FindObjectsOfType<NodeGraph>(), type, name, false);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// シーン内にある同一名のNodeGraphを取得する。
		/// </summary>
		/// <typeparam name="T">検索するNodeGraphのType。</typeparam>
		/// <param name="name">検索するNodeGraphの名前。</param>
		/// <returns>見つかったNodeGraphの配列。</returns>
#else
		/// <summary>
		/// Get the NodeGraph of the same name that is in the scene.
		/// </summary>
		/// <typeparam name="T">The type of the search NodeGraph.</typeparam>
		/// <param name="name">The name of the search NodeGraph.</param>
		/// <returns>Array of found NodeGraph.</returns>
#endif
		public static T[] FindGraphs<T>(string name) where T : NodeGraph
		{
			return (T[])GetGraphsInternal(ObjectUtility.FindObjectsOfType<NodeGraph>(), typeof(T), name, true);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectにアタッチされているNodeGraphを名前で取得する。
		/// </summary>
		/// <param name="gameObject">検索したいGameObject。</param>
		/// <param name="name">検索するNodeGraphの名前。</param>
		/// <returns>見つかったNodeGraph。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get NodeGraph in the name that has been attached to the GameObject.
		/// </summary>
		/// <param name="gameObject">Want to search GameObject.</param>
		/// <param name="name">The name of the search NodeGraph.</param>
		/// <returns>Found NodeGraph. Returns null if not found.</returns>
#endif
		public static NodeGraph FindGraph(GameObject gameObject, string name)
		{
			return FindGraph(gameObject, name, typeof(NodeGraph));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectにアタッチされているNodeGraphを名前で取得する。
		/// </summary>
		/// <param name="gameObject">検索したいGameObject。</param>
		/// <param name="name">検索するNodeGraphの名前。</param>
		/// <param name="type">検索するNodeGraphのType。</param>
		/// <returns>見つかったNodeGraph。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get NodeGraph in the name that has been attached to the GameObject.
		/// </summary>
		/// <param name="gameObject">Want to search GameObject.</param>
		/// <param name="name">The name of the search NodeGraph.</param>
		/// <param name="type">The type of the search NodeGraph.</param>
		/// <returns>Found NodeGraph. Returns null if not found.</returns>
#endif
		public static NodeGraph FindGraph(GameObject gameObject, string name, System.Type type)
		{
			return GetGraphInternal(gameObject.GetComponentsTemp<NodeGraph>(), type, name);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectにアタッチされているNodeGraphを名前で取得する。
		/// </summary>
		/// <typeparam name="T">検索するNodeGraphのType。</typeparam>
		/// <param name="gameObject">検索したいGameObject。</param>
		/// <param name="name">検索するNodeGraphの名前。</param>
		/// <returns>見つかったNodeGraph。見つからなかった場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get NodeGraph in the name that has been attached to the GameObject.
		/// </summary>
		/// <typeparam name="T">The type of the search NodeGraph.</typeparam>
		/// <param name="gameObject">Want to search GameObject.</param>
		/// <param name="name">The name of the search NodeGraph.</param>
		/// <returns>Found NodeGraph. Returns null if not found.</returns>
#endif
		public static T FindGraph<T>(GameObject gameObject, string name) where T : NodeGraph
		{
			return (T)GetGraphInternal(gameObject.GetComponentsTemp<NodeGraph>(), typeof(T), name);
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectにアタッチされている同一名のNodeGraphを取得する。
		/// </summary>
		/// <param name="gameObject">検索したいGameObject。</param>
		/// <param name="name">検索するNodeGraphの名前。</param>
		/// <returns>見つかったNodeGraphの配列。</returns>
#else
		/// <summary>
		/// Get the NodeGraph of the same name that is attached to a GameObject.
		/// </summary>
		/// <param name="gameObject">Want to search GameObject.</param>
		/// <param name="name">The name of the search NodeGraph.</param>
		/// <returns>Array of found NodeGraph.</returns>
#endif
		public static NodeGraph[] FindGraphs(GameObject gameObject, string name)
		{
			return FindGraphs(gameObject, name, typeof(NodeGraph));
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectにアタッチされている同一名のNodeGraphを取得する。
		/// </summary>
		/// <param name="gameObject">検索したいGameObject。</param>
		/// <param name="name">検索するNodeGraphの名前。</param>
		/// <param name="type">検索するNodeGraphのType。</param>
		/// <returns>見つかったNodeGraphの配列。</returns>
#else
		/// <summary>
		/// Get the NodeGraph of the same name that is attached to a GameObject.
		/// </summary>
		/// <param name="gameObject">Want to search GameObject.</param>
		/// <param name="name">The name of the search NodeGraph.</param>
		/// <param name="type">The type of the search NodeGraph.</param>
		/// <returns>Array of found NodeGraph.</returns>
#endif
		public static NodeGraph[] FindGraphs(GameObject gameObject, string name, System.Type type)
		{
			return (NodeGraph[])GetGraphsInternal(gameObject.GetComponentsTemp<NodeGraph>(), type, name, false);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectにアタッチされている同一名のNodeGraphを取得する。
		/// </summary>
		/// <typeparam name="T">検索するNodeGraphのType。</typeparam>
		/// <param name="gameObject">検索したいGameObject。</param>
		/// <param name="name">検索するNodeGraphの名前。</param>
		/// <returns>見つかったNodeGraphの配列。</returns>
#else
		/// <summary>
		/// Get the NodeGraph of the same name that is attached to a GameObject.
		/// </summary>
		/// <typeparam name="T">The type of the search NodeGraph.</typeparam>
		/// <param name="gameObject">Want to search GameObject.</param>
		/// <param name="name">The name of the search NodeGraph.</param>
		/// <returns>Array of found NodeGraph.</returns>
#endif
		public static T[] FindGraphs<T>(GameObject gameObject, string name) where T : NodeGraph
		{
			return (T[])GetGraphsInternal(gameObject.GetComponentsTemp<NodeGraph>(), typeof(T), name, true);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchがループしているかをチェックする。
		/// </summary>
		/// <param name="inputNodeID">入力スロット側ノードID</param>
		/// <param name="inputObj">入力スロット側Object</param>
		/// <param name="outputNodeID">出力スロット側ノードID</param>
		/// <param name="outputObj">出力スロット側Object</param>
		/// <returns>ループしている場合はtrueを返す。</returns>
#else
		/// <summary>
		/// Check if DataBranch is looping.
		/// </summary>
		/// <param name="inputNodeID">Input slot side node ID</param>
		/// <param name="inputObj">Input slot side Object</param>
		/// <param name="outputNodeID">Output slot side node ID</param>
		/// <param name="outputObj">Output slot side Object</param>
		/// <returns>Returns true if it is looping.</returns>
#endif
		public bool CheckLoopDataBranch(int inputNodeID, Object inputObj, int outputNodeID, Object outputObj)
		{
			Node outputNode = GetNodeFromID(outputNodeID);
			if (!(outputNode is CalculatorNode || outputNode is DataBranchRerouteNode))
			{
				return false;
			}

			NodeBehaviour inBehaviour = inputObj as NodeBehaviour;
			NodeBehaviour outBehaviour = outputObj as NodeBehaviour;
			if (inBehaviour != null && outBehaviour != null && inBehaviour == outBehaviour)
			{
				// Same Behaviour
				return true;
			}

			if (inBehaviour == null && outBehaviour == null && inputNodeID == outputNodeID)
			{
				// Same RerouteNode
				return true;
			}

			if (outBehaviour != null)
			{
				int slotCount = outBehaviour.dataSlotCount;
				for (int slotIndex = 0; slotIndex < slotCount; slotIndex++)
				{
					InputSlotBase slot = outBehaviour.GetDataSlot(slotIndex) as InputSlotBase;
					if (slot == null)
					{
						continue;
					}
					
					DataBranch branch = slot.branch;
					if (branch == null)
					{
						continue;
					}

					if (CheckLoopDataBranch(inputNodeID, inputObj, branch.outNodeID, branch.outBehaviour))
					{
						return true;
					}
				}
			}
			else
			{
				DataBranchRerouteNode rerouteNode = _DataBranchRerouteNodes.GetFromID(outputNodeID);
				if (rerouteNode != null)
				{
					DataBranch branch = rerouteNode.link.inputSlot.GetBranch();
					if (branch != null)
					{
						if (CheckLoopDataBranch(inputNodeID, inputObj, branch.outNodeID, branch.outBehaviour))
						{
							return true;
						}
					}
				}
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchがループしているかをチェックする。
		/// </summary>
		/// <param name="inputNodeID">入力スロット側ノードID</param>
		/// <param name="inputObj">入力スロット側Object</param>
		/// <param name="outputNodeID">出力スロット側ノードID</param>
		/// <param name="outputObj">出力スロット側Object</param>
		/// <returns>ループしている場合はtrueを返す。</returns>
#else
		/// <summary>
		/// Check if DataBranch is looping.
		/// </summary>
		/// <param name="inputNodeID">Input slot side node ID</param>
		/// <param name="inputObj">Input slot side Object</param>
		/// <param name="outputNodeID">Output slot side node ID</param>
		/// <param name="outputObj">Output slot side Object</param>
		/// <returns>Returns true if it is looping.</returns>
#endif
		[System.Obsolete("use CheckLoopDataBranch(int inputNodeID, Object inputObj, int outputNodeID, Object outputObj)")]
		public bool CheckLoopCalculatorBranch(int inputNodeID, Object inputObj, int outputNodeID, Object outputObj)
		{
			return CheckLoopDataBranch(inputNodeID, inputObj, outputNodeID, outputObj);
		}

		void DestroySubComponents(Node node)
		{
			INodeBehaviourContainer behaviours = node as INodeBehaviourContainer;
			if (behaviours != null)
			{
				int behaviourCount = behaviours.GetNodeBehaviourCount();
				for (int behaviourIndex = 0; behaviourIndex < behaviourCount; behaviourIndex++)
				{
					Object behaviourObj = behaviours.GetNodeBehaviour<Object>(behaviourIndex);
					NodeBehaviour behaviour = behaviourObj as NodeBehaviour;
					if (behaviour != null)
					{
						NodeBehaviour.Destroy(behaviour);
					}
					else if (behaviourObj != null)
					{
						ComponentUtility.Destroy(behaviourObj);
					}
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeGraphのコールバック用デリゲート
		/// </summary>
		/// <param name="nodeGraph">イベントが起きたNodeGraph</param>
#else
		/// <summary>
		/// Delegate for NodeGraph callback
		/// </summary>
		/// <param name="nodeGraph">Event occurred NodeGraph</param>
#endif
		[System.Obsolete("Use System.Action<NodeGraph>", true)]
		public delegate void NodeGraphCallback(NodeGraph nodeGraph);

#if ARBOR_DOC_JA
		/// <summary>
		/// 破棄される際のコールバック
		/// </summary>
#else
		/// <summary>
		/// Call back when being destroyed
		/// </summary>
#endif
		public event System.Action<NodeGraph> destroyCallback;

#if ARBOR_DOC_JA
		/// <summary>
		/// 状態が変わった際のコールバック
		/// </summary>
#else
		/// <summary>
		/// Call back when the state changes
		/// </summary>
#endif
		public event System.Action<NodeGraph> stateChangedCallback;

#if ARBOR_DOC_JA
		/// <summary>
		/// グラフの状態が更新されたことを通知する。
		/// </summary>
#else
		/// <summary>
		/// Notifies that the status of the graph has been updated.
		/// </summary>
#endif
		protected internal void StateChanged()
		{
			stateChangedCallback?.Invoke(this);
		}

		internal bool disableCallbackGraphTree = false;

		internal void ChangedGraphTree()
		{
			if (disableCallbackGraphTree)
			{
				return;
			}

			onChangedGraphTree?.Invoke();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はMonoBehaviourが破棄されるときに呼び出される。
		/// </summary>
#else
		/// <summary>
		/// This function is called when MonoBehaivour will be destroyed.
		/// </summary>
#endif
		public virtual void OnDestroy()
		{
			DestroySubComponents();

			var rootGraph = this.rootGraph;
			if (rootGraph != null)
			{
				rootGraph.ChangedGraphTree();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 内部的に使用するメソッド。特に呼び出す必要はありません。
		/// </summary>
		/// <param name="callback">destroyCallbackを呼び出すかどうか。</param>
#else
		/// <summary>
		/// Method to be used internally. In particular there is no need to call.
		/// </summary>
		/// <param name="callback">Whether to call destroyCallback.</param>
#endif
		public void DestroySubComponents(bool callback = true)
		{
			int nodeCount = _Nodes.Count;
			for (int nodeIndex = 0; nodeIndex < nodeCount; nodeIndex++)
			{
				Node node = _Nodes[nodeIndex];
				DestroySubComponents(node);
			}

			if (_ParameterContainer != null)
			{
				ParameterContainerInternal.Destroy(_ParameterContainer);
			}

			if (callback && destroyCallback != null)
			{
				destroyCallback(this);
				destroyCallback = null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Resetもしくは生成時のコールバック。
		/// </summary>
#else
		/// <summary>
		/// Reset or create callback.
		/// </summary>
#endif
		protected virtual void OnReset()
		{
		}

		void ResetInternal()
		{
			var behaviours = this.GetComponentsTemp<NodeBehaviour>();
			for (int behaviourIndex = 0; behaviourIndex < behaviours.Count; behaviourIndex++)
			{
				NodeBehaviour behaviour = behaviours[behaviourIndex];
				if (behaviour.nodeGraph == this)
				{
					ComponentUtility.Destroy(behaviour);
				}
			}

			OnReset();
		}

		void Initialize()
		{
			if (Application.isPlaying)
			{
				ResetInternal();
			}

			isDeserialized = true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// デフォルト値にリセットする。
		/// </summary>
#else
		/// <summary>
		/// Reset to default values.
		/// </summary>
#endif
		protected virtual void Reset()
		{
			ResetInternal();
		}

		void PauseInternal()
		{
			updateSettings.Pause();

			int calculatorCount = _Calculators.Count;
			for (int i = 0; i < calculatorCount; i++)
			{
				CalculatorNode calculatorNode = _Calculators[i];
				Calculator calculator = calculatorNode.calculator;
				if (ComponentUtility.IsValidObject(calculator))
				{
					calculator.CallPauseEvent();
				}
			}
		}

		void ResumeInternal()
		{
			updateSettings.Resume();

			int calculatorCount = _Calculators.Count;
			for (int i = 0; i < calculatorCount; i++)
			{
				CalculatorNode calculatorNode = _Calculators[i];
				Calculator calculator = calculatorNode.calculator;
				if (ComponentUtility.IsValidObject(calculator))
				{
					calculator.CallResumeEvent();
				}
			}
		}

		void StopInternal()
		{
			int calculatorCount = _Calculators.Count;
			for (int i = 0; i < calculatorCount; i++)
			{
				CalculatorNode calculatorNode = _Calculators[i];
				Calculator calculator = calculatorNode.calculator;
				if (ComponentUtility.IsValidObject(calculator))
				{
					calculator.CallStopEvent();
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeGraphの作成
		/// </summary>
		/// <param name="gameObject">GameObject</param>
		/// <param name="classType">NodeGraphの型</param>
		/// <returns>作成したNodeGraph</returns>
#else
		/// <summary>
		/// Create NodeGraph
		/// </summary>
		/// <param name="gameObject">GameObject</param>
		/// <param name="classType">NodeGraph type</param>
		/// <returns>The created NodeGraph</returns>
#endif
		public static NodeGraph Create(GameObject gameObject, System.Type classType)
		{
			if (!TypeUtility.IsSubclassOf(classType, typeof(NodeGraph)))
			{
				return null;
			}

			NodeGraph nodeGraph = ComponentUtility.AddComponent(gameObject, classType) as NodeGraph;
			nodeGraph.Initialize();

			return nodeGraph;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeGraphの作成
		/// </summary>
		/// <typeparam name="GraphType">NodeGraphの型</typeparam>
		/// <param name="gameObject">GameObject</param>
		/// <returns>作成したNodeGraph</returns>
#else
		/// <summary>
		/// Create NodeGraph
		/// </summary>
		/// <typeparam name="GraphType">NodeGraph type</typeparam>
		/// <param name="gameObject">GameObject</param>
		/// <returns>The created NodeGraph</returns>
#endif
		public static GraphType Create<GraphType>(GameObject gameObject) where GraphType : NodeGraph
		{
			GraphType nodeGraph = ComponentUtility.AddComponent<GraphType>(gameObject);
			nodeGraph.Initialize();

			return nodeGraph;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeGraphを生成
		/// </summary>
		/// <param name="sourceGraph">生成元のグラフ</param>
		/// <param name="usePool">ObjectPoolを使用してインスタンス化するフラグ。</param>
		/// <returns>生成したグラフ</returns>
#else
		/// <summary>
		/// Instantiate NodeGraph
		/// </summary>
		/// <param name="sourceGraph">Source graph</param>
		/// <param name="usePool">Flag to instantiate using ObjectPool.</param>
		/// <returns>Instantiated graph</returns>
#endif
		public static NodeGraph Instantiate(NodeGraph sourceGraph, bool usePool)
		{
			var nodeGraph = usePool ?
				ObjectPool.Instantiate(sourceGraph) as NodeGraph :
				Object.Instantiate(sourceGraph) as NodeGraph;

			if (nodeGraph != null)
			{
				nodeGraph.Refresh();
			}

			return nodeGraph;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeGraphを生成
		/// </summary>
		/// <typeparam name="GraphType">グラフの型</typeparam>
		/// <param name="sourceGraph">生成元のグラフ</param>
		/// <param name="usePool">ObjectPoolを使用してインスタンス化するフラグ。</param>
		/// <returns>生成したグラフ</returns>
#else
		/// <summary>
		/// Instantiate NodeGraph
		/// </summary>
		/// <typeparam name="GraphType">Graph type</typeparam>
		/// <param name="sourceGraph">Source graph</param>
		/// <param name="usePool">Flag to instantiate using ObjectPool.</param>
		/// <returns>Instantiated graph</returns>
#endif
		public static GraphType Instantiate<GraphType>(GraphType sourceGraph, bool usePool) where GraphType : NodeGraph
		{
			var nodeGraph = usePool ?
				ObjectPool.Instantiate<GraphType>(sourceGraph) :
				Object.Instantiate<GraphType>(sourceGraph);

			if (nodeGraph != null)
			{
				nodeGraph.Refresh();
			}

			return nodeGraph;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeGraphを生成
		/// </summary>
		/// <param name="sourceGraph">生成元のグラフ</param>
		/// <param name="ownerBehaviour">グラフの所有権を持つNodeBehaviour</param>
		/// <param name="usePool">ObjectPoolを使用してインスタンス化するフラグ。</param>
		/// <returns>生成したグラフ</returns>
#else
		/// <summary>
		/// Instantiate NodeGraph
		/// </summary>
		/// <param name="sourceGraph">Source graph</param>
		/// <param name="ownerBehaviour">NodeBehaviour with chart ownership</param>
		/// <param name="usePool">Flag to instantiate using ObjectPool.</param>
		/// <returns>Instantiated graph</returns>
#endif
		public static NodeGraph Instantiate(NodeGraph sourceGraph, NodeBehaviour ownerBehaviour, bool usePool = false)
		{
			NodeGraph nodeGraph = Instantiate(sourceGraph, usePool);
			if (nodeGraph != null)
			{
				nodeGraph.SetExternal(ownerBehaviour, true);
			}
			return nodeGraph;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeGraphを生成
		/// </summary>
		/// <typeparam name="GraphType">グラフの型</typeparam>
		/// <param name="sourceGraph">生成元のグラフ</param>
		/// <param name="ownerBehaviour">グラフの所有権を持つNodeBehaviour</param>
		/// <param name="usePool">ObjectPoolを使用してインスタンス化するフラグ。</param>
		/// <returns>生成したグラフ</returns>
#else
		/// <summary>
		/// Instantiate NodeGraph
		/// </summary>
		/// <typeparam name="GraphType">Graph type</typeparam>
		/// <param name="sourceGraph">Source graph</param>
		/// <param name="ownerBehaviour">NodeBehaviour with chart ownership</param>
		/// <param name="usePool">Flag to instantiate using ObjectPool.</param>
		/// <returns>Instantiated graph</returns>
#endif
		public static GraphType Instantiate<GraphType>(GraphType sourceGraph, NodeBehaviour ownerBehaviour, bool usePool = false) where GraphType : NodeGraph
		{
			GraphType nodeGraph = Instantiate<GraphType>(sourceGraph, usePool);
			if (nodeGraph != null)
			{
				nodeGraph.SetExternal(ownerBehaviour, true);
			}
			return nodeGraph;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// このグラフを外部グラフに設定する。
		/// </summary>
		/// <param name="ownerBehaviour">グラフの所有権を持つNodeBehaviour。nullを指定した場合は外部グラフ指定を解除する。</param>
		/// <param name="changeParent">Transformの親をオーナーオブジェクトに変更するフラグ(デフォルト false)</param>
		/// <remarks>子グラフとして生成する場合は<see cref="Instantiate(NodeGraph, NodeBehaviour, bool)"/>または<see cref="Instantiate{GraphType}(GraphType, NodeBehaviour, bool)"/>を使用してください。</remarks>
#else
		/// <summary>
		/// Set this graph as an external graph.
		/// </summary>
		/// <param name="ownerBehaviour">NodeBehaviour with chart ownership. If null is specified, the external graph specification is canceled.</param>
		/// <param name="changeParent">Flag to change the parent of Transform to owner object(Default false)</param>
		/// <remarks>Use <see cref="Instantiate(NodeGraph, NodeBehaviour, bool)" /> or <see cref="Instantiate{GraphType}(GraphType, NodeBehaviour, bool)" /> to instantiate as a child graph.</remarks>
#endif
		public void SetExternal(NodeBehaviour ownerBehaviour, bool changeParent = false)
		{
			this.external = ownerBehaviour != null;
			this.ownerBehaviour = ownerBehaviour;
			if (ownerBehaviour != null && changeParent)
			{
				transform.SetParent(ownerBehaviour.transform, false);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// NodeGraphの破棄
		/// </summary>
		/// <param name="nodeGraph">NodeGraph</param>
#else
		/// <summary>
		/// Destroy NodeGraph
		/// </summary>
		/// <param name="nodeGraph">NodeGraph</param>
#endif
		public static void Destroy(NodeGraph nodeGraph)
		{
			if (!Application.isPlaying)
			{
				nodeGraph.OnDestroy();
			}
			ComponentUtility.Destroy(nodeGraph);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// グラフを文字列に変換（デバッグ用）。
		/// </summary>
		/// <returns>変換された文字列</returns>
#else
		/// <summary>
		/// Convert graph to string (for debugging).
		/// </summary>
		/// <returns>Converted string</returns>
#endif
		public override string ToString()
		{
			return string.Format("{0} ({1})", graphName, GetType().Name);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再開する際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when resuming.
		/// </summary>
#endif
		protected virtual void OnPoolResume()
		{
			if (rootGraph == this && playOnStart)
			{
				Play();

				OnPlayOnStart();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プールに格納された際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when stored in the pool.
		/// </summary>
#endif
		protected virtual void OnPoolSleep()
		{
			if (rootGraph == this)
			{
				Stop();
			}
			SetExternal(null);
		}

		void IPoolCallbackReceiver.OnPoolResume()
		{
			OnPoolResume();
		}

		void IPoolCallbackReceiver.OnPoolSleep()
		{
			OnPoolSleep();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// プレイ開始した際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when you start playing.
		/// </summary>
#endif
		protected abstract void OnPlay();

#if ARBOR_DOC_JA
		/// <summary>
		/// プレイ停止した際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when play is stopped.
		/// </summary>
#endif
		protected abstract void OnStop();

#if ARBOR_DOC_JA
		/// <summary>
		/// ポーズした際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when you pause.
		/// </summary>
#endif
		protected abstract void OnPause();

#if ARBOR_DOC_JA
		/// <summary>
		/// 再開した際に呼ばれる
		/// </summary>
#else
		/// <summary>
		/// Called when resuming
		/// </summary>
#endif
		protected abstract void OnResume();

#if ARBOR_DOC_JA
		/// <summary>
		/// 更新する際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when updating.
		/// </summary>
#endif
		protected abstract void OnUpdate();

#if ARBOR_DOC_JA
		/// <summary>
		/// LateUpdateの際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when LateUpdate.
		/// </summary>
#endif
		protected virtual void OnLateUpdate()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FixedUpdateの際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when FixedUpdate.
		/// </summary>
#endif
		protected virtual void OnFixedUpdate()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Startメソッドでプレイ開始した際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when playing is started with the Start method.
		/// </summary>
#endif
		protected virtual void OnPlayOnStart()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// OnEnableメソッドで再開した際に呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when resumed with the OnEnable method.
		/// </summary>
#endif
		protected virtual void OnResumeOnEnable()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生開始。
		/// </summary>
#else
		/// <summary>
		/// Start playing.
		/// </summary>
#endif
		public void Play()
		{
			if (!isActiveAndEnabled)
			{
				Debug.LogWarning("Only active can be played.");
				return;
			}

			if (playState != PlayState.Stopping)
			{
				return;
			}

#if ARBOR_TRIAL
			if( Trial.TrialGUI.IsTrialLimitTime() )
			{
				Trial.TrialGUI.DisplayLimitLog();
				return;
			}
#endif
			using (CalculateScope.OpenScope())
			{
				playState = PlayState.Playing;
				updateSettings.ClearTime();

				OnPlay();

				onPlayStateChanged?.Invoke(playState);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生停止。
		/// </summary>
#else
		/// <summary>
		/// Stopping playback.
		/// </summary>
#endif
		public void Stop()
		{
			if (playState == PlayState.Stopping)
			{
				return;
			}

			using (CalculateScope.OpenScope())
			{
				playState = PlayState.Stopping;
				StopInternal();

				OnStop();

				StateChanged();

				onPlayStateChanged?.Invoke(playState);
			}
		}

		void DoPause()
		{
			PauseInternal();

			OnPause();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生を一時停止。
		/// </summary>
#else
		/// <summary>
		/// Pause playback.
		/// </summary>
#endif
		public void Pause()
		{
			if (!isActiveAndEnabled || playState != PlayState.Playing)
			{
				return;
			}

			using (CalculateScope.OpenScope())
			{
				playState = PlayState.Pausing;

				DoPause();

				onPlayStateChanged?.Invoke(playState);
			}
		}

		void DoResume()
		{
			ResumeInternal();

			OnResume();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生を再開。
		/// </summary>
#else
		/// <summary>
		/// Resume playing.
		/// </summary>
#endif
		public void Resume()
		{
			if (!isActiveAndEnabled || playState != PlayState.Pausing)
			{
				return;
			}

			using (CalculateScope.OpenScope())
			{
				playState = PlayState.Playing;

				DoResume();

				onPlayStateChanged?.Invoke(playState);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Unityから呼び出される開始メソッド
		/// </summary>
#else
		/// <summary>
		/// Start method called from Unity
		/// </summary>
#endif
		protected virtual void Start()
		{
			isStarted = true;

			Refresh();

			if (playOnStart)
			{
				Play();

				OnPlayOnStart();
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Unityから呼び出される有効化メソッド
		/// </summary>
#else
		/// <summary>
		/// Enabled method called from Unity
		/// </summary>
#endif
		protected virtual void OnEnable()
		{
			if (playState != PlayState.InactivePausing)
			{
				return;
			}

			using (CalculateScope.OpenScope())
			{
				updateSettings.ClearTime();

				playState = PlayState.Playing;

				DoResume();

				if (playState == PlayState.Playing)
				{
					OnResumeOnEnable();
				}

				onPlayStateChanged?.Invoke(playState);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Unityから呼び出される無効化メソッド
		/// </summary>
#else
		/// <summary>
		/// Disabled method called from Unity
		/// </summary>
#endif
		protected virtual void OnDisable()
		{
			if (playState != PlayState.Playing)
			{
				return;
			}

			using (CalculateScope.OpenScope())
			{
				playState = PlayState.InactivePausing;

				DoPause();

				onPlayStateChanged?.Invoke(playState);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Unityから呼び出される毎フレーム更新メソッド
		/// </summary>
#else
		/// <summary>
		/// Every frame update method called from Unity
		/// </summary>
#endif
		protected virtual void Update()
		{
			if (playState != PlayState.Playing)
			{
				return;
			}

#if ARBOR_TRIAL
			if( !Application.isEditor )
			{
				Trial.TrialGUI.InitIfNecessary();
				if( Trial.TrialGUI.IsTrialLimitTime() )
				{
					Stop();
					Trial.TrialGUI.DisplayLimitLog();
					return;
				}
			}
#endif

			if (updateSettings.isUpdatableOnUpdate)
			{
				CallOnUpdate(true);
			}
		}

		void CallOnUpdate(bool autoExecuteLateUpdate)
		{
			_ExecuteLateUpdate = autoExecuteLateUpdate;
			OnUpdate();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Updateを実行する。
		/// UpdateSettings.typeがManualの場合に任意のタイミングでこのメソッドを呼んでください。
		/// </summary>
		/// <param name="autoExecuteLateUpdate">自動的にExecuteLateUpdateを行うフラグ</param>
#else
		/// <summary>
		/// Perform an update.
		/// Please call this method at any timing when UpdateSettings.type is Manual.
		/// </summary>
		/// <param name="autoExecuteLateUpdate">Flag for ExecuteLateUpdate automatically</param>
#endif
		public void ExecuteUpdate(bool autoExecuteLateUpdate = false)
		{
			if (playState != PlayState.Playing || updateSettings.type != UpdateType.Manual)
			{
				return;
			}

			CallOnUpdate(autoExecuteLateUpdate);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// LateUpdateを実行する。
		/// UpdateSettings.typeがManualの場合に任意のタイミングでこのメソッドを呼んでください。
		/// </summary>
#else
		/// <summary>
		/// Perform an LateUpdate.
		/// Please call this method at any timing when UpdateSettings.type is Manual.
		/// </summary>
#endif
		public void ExecuteLateUpdate()
		{
			if (playState != PlayState.Playing || updateSettings.type != UpdateType.Manual)
			{
				return;
			}

			CallOnLateUpdate();
		}

		void CallOnLateUpdate()
		{
			OnLateUpdate();
			_ExecuteLateUpdate = false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Unityから呼び出される毎フレーム更新後の遅延更新メソッド
		/// </summary>
#else
		/// <summary>
		/// Delayed update method after every frame update called from Unity
		/// </summary>
#endif
		protected virtual void LateUpdate()
		{
			if (playState != PlayState.Playing)
			{
				return;
			}

			if (_ExecuteLateUpdate)
			{
				CallOnLateUpdate();
			}

#if ARBOR_TRIAL
			if( !Application.isEditor )
			{
				Trial.TrialGUI.InitIfNecessary();
				if( Trial.TrialGUI.IsTrialLimitTime() )
				{
					Stop();
					Trial.TrialGUI.DisplayLimitLog();
					return;
				}
			}
#endif
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Unityから呼び出される物理演算用のフレームレートに依存しない更新メソッド
		/// </summary>
#else
		/// <summary>
		/// Frame rate independent update method for physics called from Unity
		/// </summary>
#endif
		protected virtual void FixedUpdate()
		{
			if (playState != PlayState.Playing)
			{
				return;
			}

			OnFixedUpdate();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ノードがブレークポイントによって停止したときに呼ばれる。
		/// </summary>
#else
		/// <summary>
		/// Called when a node is stopped by a breakpoint.
		/// </summary>
#endif
		public static event System.Action<Node> onBreakNode;

		internal void BreakNode(Node node)
		{
			if (Application.isEditor)
			{
				Debug.Break();
				onBreakNode?.Invoke(node);
			}
		}
	}
}