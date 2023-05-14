//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// DataSlotを接続するクラス。
	/// </summary>
#else
	/// <summary>
	/// Class that connects DataSlot.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class DataBranch : ISerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// ブランチのID。
		/// </summary>
#else
		/// <summary>
		/// ID of branch.
		/// </summary>
#endif
		public int branchID;

#if ARBOR_DOC_JA
		/// <summary>
		/// 描画するかどうか。エディタ用。
		/// </summary>
#else
		/// <summary>
		/// Whether to draw. For the editor.
		/// </summary>
#endif
		public bool enabled;

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力側のBehaviour
		/// </summary>
#else
		/// <summary>
		/// Input side Behaviour
		/// </summary>
#endif
		public Object inBehaviour;

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力側のnodeID.
		/// </summary>
#else
		/// <summary>
		/// Input side nodeID
		/// </summary>
#endif
		public int inNodeID;

#if ARBOR_DOC_JA
		/// <summary>
		/// 出力側のBehaviour
		/// </summary>
#else
		/// <summary>
		/// Output side Behaviour
		/// </summary>
#endif
		public Object outBehaviour;

#if ARBOR_DOC_JA
		/// <summary>
		/// 出力側のnodeID
		/// </summary>
#else
		/// <summary>
		/// Output side nodeID
		/// </summary>
#endif
		public int outNodeID;

#if ARBOR_DOC_JA
		/// <summary>
		/// 接続する線のベジェ曲線。エディタ用
		/// </summary>
#else
		/// <summary>
		/// Bezier curve of the line to be connected. For editor
		/// </summary>
#endif
		public Bezier2D lineBezier = new Bezier2D();

		#endregion // Serialize fields

		#region Private fields

		[System.NonSerialized]
		internal DataSlot _OutputSlot = null;

		[System.NonSerialized]
		internal DataSlot _InputSlot = null;

		[System.NonSerialized]
		private DataBranch _InputRerouteBranch = null;

		[System.NonSerialized]
		private List<DataBranch> _OutputRerouteBranchies = null;

		#endregion // Private fields

		#region Properties

		internal void SetDirty()
		{
			Calculator calculator = inBehaviour as Calculator;
			if (calculator != null)
			{
				calculator.SetDirty();
				return;
			}

			NodeGraph nodeGraph = inBehaviour as NodeGraph;
			if (nodeGraph == null)
			{
				return;
			}

			DataBranchRerouteNode rerouteNode = nodeGraph.dataBranchRerouteNodes.GetFromID(inNodeID);
			if (rerouteNode == null)
			{
				return;
			}

			IOutputSlot outputSlot = rerouteNode.link.outputSlot;
			for (int i = 0, count = outputSlot.branchCount; i < count; i++)
			{
				DataBranch branch = outputSlot.GetBranch(i);
				if (branch == null)
				{
					continue;
				}

				branch.SetDirty();
			}
		}

		internal bool IsDirty()
		{
			Calculator calculator = outBehaviour as Calculator;
			if (calculator != null)
			{
				return calculator.isDirty;
			}

			NodeGraph nodeGraph = outBehaviour as NodeGraph;
			if (nodeGraph == null)
			{
				return false;
			}

			RebuildInputRerouteBranch(nodeGraph);
			if (_InputRerouteBranch != null)
			{
				return _InputRerouteBranch.IsDirty();
			}

			return false;
		}

		internal OutputSlotBase Calculate()
		{
			Calculator calculator = outBehaviour as Calculator;
			if (calculator != null)
			{
				calculator.Calculate();
				valueSlot = outputSlot as OutputSlotBase;
			}
			else
			{
				RebuildInputRerouteBranch(outBehaviour as NodeGraph);

				if (_InputRerouteBranch != null)
				{
					valueSlot = _InputRerouteBranch.Calculate();
				}
				else
				{
					valueSlot = outputSlot as OutputSlotBase;
				}
			}

			return valueSlot;
		}

		void RebuildInputRerouteBranch(NodeGraph nodeGraph)
		{
			if (_InputRerouteBranch != null)
			{
				return;
			}

			if (nodeGraph == null)
			{
				return;
			}

			DataBranchRerouteNode rerouteNode = nodeGraph.dataBranchRerouteNodes.GetFromID(outNodeID);
			if (rerouteNode != null)
			{
				_InputRerouteBranch = rerouteNode.link.inputSlot.GetBranch();
			}
		}

		void RebuildOutputRerouteBranchies(NodeGraph nodeGraph)
		{
			if (_OutputRerouteBranchies != null)
			{
				return;
			}

			if (nodeGraph == null)
			{
				return;
			}

			_OutputRerouteBranchies = new List<DataBranch>();

			DataBranchRerouteNode rerouteNode = nodeGraph.dataBranchRerouteNodes.GetFromID(inNodeID);
			if (rerouteNode != null)
			{
				int count = rerouteNode.link.outputSlot.branchCount;
				for (int i = 0; i < count; i++)
				{
					DataBranch branch = rerouteNode.link.outputSlot.GetBranch(i);
					_OutputRerouteBranchies.Add(branch);
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値が格納されているスロット。
		/// </summary>
		/// <remarks>入力スロットから値を取得した際に有効なスロットが格納される。</remarks>
#else
		/// <summary>
		/// The slot where the value is stored.
		/// </summary>
		/// <remarks>The slot that is valid when the value is obtained from the input slot is stored.</remarks>
#endif
		public OutputSlotBase valueSlot
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を取得設定する。
		/// Calculatorの出力スロットと接続している場合は必要に応じて値を更新してから取得する。
		/// </summary>
#else
		/// <summary>
		/// get set the value.
		/// When connecting to the output slot of Calculator, update it as necessary and obtain it.
		/// </summary>
#endif
		public object value
		{
			get
			{
				OutputSlotBase valueSlot = Calculate();
				if (valueSlot != null)
				{
					return valueSlot.GetValue();
				}
				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 現在の値を取得する。
		/// </summary>
#else
		/// <summary>
		/// Get the current value.
		/// </summary>
#endif
		public object currentValue
		{
			get
			{
				if (valueSlot != null)
				{
					return valueSlot.GetValue();
				}
				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値が使われているかどうかを取得する。
		/// </summary>
#else
		/// <summary>
		/// Gets whether or not a value is used.
		/// </summary>
#endif
		public bool isUsed
		{
			get
			{
				if (valueSlot != null)
				{
					return valueSlot.isUsed;
				}
				return false;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を表示するかどうかを取得する。
		/// </summary>
#else
		/// <summary>
		/// Gets whether or not a value is visible.
		/// </summary>
#endif
		public bool showDataValue = false;

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を表示するかどうかを取得する。
		/// </summary>
#else
		/// <summary>
		/// Gets whether or not a value is visible.
		/// </summary>
#endif
		[System.Obsolete("use showDataValue")]
		public bool isVisible
		{
			get
			{
				return showDataValue;
			}
			set
			{
				showDataValue = true;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// valueを更新した timeScale に依存しない時間。
		/// </summary>
#else
		/// <summary>
		/// Time that does not depend on timeScale when value is updated.
		/// </summary>
#endif
		public float updatedTime
		{
			get
			{
				if (valueSlot != null)
				{
					return valueSlot.updatedTime;
				}
				return 0f;
			}
		}

		void FindOutputSlot()
		{
			NodeBehaviour nodeBehaviour = outBehaviour as NodeBehaviour;
			if (nodeBehaviour != null)
			{
				int slotCount = nodeBehaviour.dataSlotCount;
				for (int slotIndex = 0; slotIndex < slotCount; slotIndex++)
				{
					OutputSlotBase s = nodeBehaviour.GetDataSlot(slotIndex) as OutputSlotBase;
					if (s != null && s.IsConnected(this))
					{
						_OutputSlot = s;
						return;
					}
				}
			}
		}

		void RebuildOutputSlot()
		{
			// Maintains compatibility with versions (Arbor 3.0 or earlier)
			if (_OutputSlot == null && outBehaviour != null)
			{
				_OutputSlot = null;

				NodeBehaviour nodeBehaviour = outBehaviour as NodeBehaviour;
				if (nodeBehaviour != null)
				{
					FindOutputSlot();

					//if (_OutputSlotField == null)
					//{
					//	nodeBehaviour.RebuildDataSlotFields();

					//	FindOutputSlot();
					//}
				}
				else
				{
					NodeGraph nodeGraph = outBehaviour as NodeGraph;
					if (nodeGraph != null)
					{
						DataBranchRerouteNode rerouteNode = nodeGraph.GetNodeFromID(outNodeID) as DataBranchRerouteNode;
						if (rerouteNode != null)
						{
							_OutputSlot = rerouteNode.link;
						}
					}
				}
			}
		}

		void FindInputSlot()
		{
			NodeBehaviour nodeBehaviour = inBehaviour as NodeBehaviour;
			if (nodeBehaviour != null)
			{
				int slotCount = nodeBehaviour.dataSlotCount;
				for (int slotIndex = 0; slotIndex < slotCount; slotIndex++)
				{
					InputSlotBase s = nodeBehaviour.GetDataSlot(slotIndex) as InputSlotBase;
					if (s != null && s.IsConnected(this))
					{
						_InputSlot = s;
						break;
					}
				}
			}
		}

		void RebuildInputSlot()
		{
			// Maintains compatibility with versions (Arbor 3.0 or earlier)
			if (_InputSlot == null && inBehaviour != null)
			{
				_InputSlot = null;

				NodeBehaviour nodeBehaviour = inBehaviour as NodeBehaviour;
				if (nodeBehaviour != null)
				{
					FindInputSlot();

					//if (_InputSlotField == null)
					//{
					//	nodeBehaviour.RebuildDataSlotFields();

					//	FindInputSlot();
					//}
				}
				else
				{
					NodeGraph nodeGraph = inBehaviour as NodeGraph;
					if (nodeGraph != null)
					{
						DataBranchRerouteNode rerouteNode = nodeGraph.GetNodeFromID(inNodeID) as DataBranchRerouteNode;
						if (rerouteNode != null)
						{
							_InputSlot = rerouteNode.link;
						}
					}
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 出力スロットのDataSlotFieldを取得する。
		/// </summary>
#else
		/// <summary>
		/// Get DataSlotField of output slot.
		/// </summary>
#endif
		[System.Obsolete("use outputSlot")]
		public DataSlotField outputSlotField
		{
			get
			{
				DataSlot slot = outputSlot;
				if (slot != null)
				{
					return slot.slotField;
				}

				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 出力スロットを取得する。
		/// </summary>
#else
		/// <summary>
		/// Get the output slot.
		/// </summary>
#endif
		public DataSlot outputSlot
		{
			get
			{
				RebuildOutputSlot();

				return _OutputSlot;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 出力スロットのFieldInfoを取得する。
		/// </summary>
#else
		/// <summary>
		/// Get the FieldInfo of the output slot.
		/// </summary>
#endif
		public System.Reflection.FieldInfo outputSlotFieldInfo
		{
			get
			{
				DataSlot slot = outputSlot;
				if (slot != null)
				{
					return slot.fieldInfo;
				}

				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 出力する型
		/// </summary>
#else
		/// <summary>
		/// Output type.
		/// </summary>
#endif
		public System.Type outputType
		{
			get
			{
				DataSlot slot = outputSlot;
				if (slot != null)
				{
					return slot.connectableType;
				}

				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 出力スロットが有効であるかを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns whether the output slot is valid.
		/// </summary>
#endif
		public bool isValidOutputSlot
		{
			get
			{
				return (outNodeID != 0 || outBehaviour is MonoBehaviour) && outputSlot != null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力スロットのFieldInfoを取得する。
		/// </summary>
#else
		/// <summary>
		/// Get the FieldInfo of the input slot.
		/// </summary>
#endif
		[System.Obsolete("use inputSlot")]
		public DataSlotField inputSlotField
		{
			get
			{
				DataSlot slot = inputSlot;
				if (slot != null)
				{
					return slot.slotField;
				}

				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力スロットを取得する。
		/// </summary>
#else
		/// <summary>
		/// Get the input slot.
		/// </summary>
#endif
		public DataSlot inputSlot
		{
			get
			{
				RebuildInputSlot();

				return _InputSlot;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力スロットのFieldInfoを取得する。
		/// </summary>
#else
		/// <summary>
		/// Get the FieldInfo of the input slot.
		/// </summary>
#endif
		public System.Reflection.FieldInfo inputSlotFieldInfo
		{
			get
			{
				DataSlot slot = inputSlot;
				if (slot != null)
				{
					return slot.fieldInfo;
				}

				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力する型。
		/// </summary>
#else
		/// <summary>
		/// Input type.
		/// </summary>
#endif
		public System.Type inputType
		{
			get
			{
				DataSlot slot = inputSlot;
				if (slot != null)
				{
					return slot.connectableType;
				}

				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力スロットが有効であるかを返す。
		/// </summary>
#else
		/// <summary>
		/// Returns whether the input slot is valid.
		/// </summary>
#endif
		public bool isValidInputSlot
		{
			get
			{
				return (inNodeID != 0 || inBehaviour is MonoBehaviour) && !object.ReferenceEquals(inputSlot, null);
			}
		}

		#endregion // Properties

		#region Public methods

#if ARBOR_DOC_JA
		/// <summary>
		/// Behaviourを変更する。
		/// </summary>
		/// <param name="inNodeID">入力スロット側のノードID</param>
		/// <param name="inBehaviour">入力スロット側のObject</param>
		/// <param name="outNodeID">出力スロット側のノードID</param>
		/// <param name="outBehaviour">出力スロット側のObject</param>
#else
		/// <summary>
		/// Change Behavior.
		/// </summary>
		/// <param name="inNodeID">Node ID on the input slot side</param>
		/// <param name="inBehaviour">Object on the input slot side</param>
		/// <param name="outNodeID">Node ID on the output slot side</param>
		/// <param name="outBehaviour">Object on the output slot side</param>
#endif
		public void SetBehaviour(int inNodeID, Object inBehaviour, int outNodeID, Object outBehaviour)
		{
			this.inNodeID = inNodeID;
			this.inBehaviour = inBehaviour;
			this.outNodeID = outNodeID;
			this.outBehaviour = outBehaviour;

			_InputRerouteBranch = null;
			_OutputRerouteBranchies = null;

			Calculator calculator = outBehaviour as Calculator;
			if (calculator != null)
			{
				calculator.SetDirty();
			}
		}

		internal void RebuildSlotField()
		{
			// Maintains compatibility with versions (Arbor 3.0 or earlier)
			SetDirtySlotField();

			RebuildInputSlot();
			RebuildOutputSlot();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// スロットフィールドがダーティであるとマークする
		/// </summary>
#else
		/// <summary>
		/// Mark slot field as dirty
		/// </summary>
#endif
		public void SetDirtySlotField()
		{
			_InputSlot = null;
			_OutputSlot = null;

			_InputRerouteBranch = null;
			_OutputRerouteBranchies = null;
		}

		void ClearValueSlot()
		{
			valueSlot = null;

			RebuildOutputRerouteBranchies(inBehaviour as NodeGraph);
			if (_OutputRerouteBranchies != null)
			{
				for (int branchIndex = 0; branchIndex < _OutputRerouteBranchies.Count; branchIndex++)
				{
					DataBranch branch = _OutputRerouteBranchies[branchIndex];
					branch.ClearValueSlot();
				}
			}
		}

		internal void OnDisconnected()
		{
			if (_InputRerouteBranch != null)
			{
				if (_InputRerouteBranch._OutputRerouteBranchies != null)
				{
					_InputRerouteBranch._OutputRerouteBranchies.Remove(this);
					if (_InputRerouteBranch._OutputRerouteBranchies.Count == 0)
					{
						_InputRerouteBranch._OutputRerouteBranchies = null;
					}
				}
				_InputRerouteBranch = null;
			}

			ClearValueSlot();
			if (_OutputRerouteBranchies != null)
			{
				for (int branchIndex = 0; branchIndex < _OutputRerouteBranchies.Count; branchIndex++)
				{
					DataBranch branch = _OutputRerouteBranchies[branchIndex];
					branch._InputRerouteBranch = null;
				}

				_OutputRerouteBranchies = null;
			}
		}

		#endregion // Public methods

		#region ISerializationCallbackReceiver

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			_InputSlot = null;
			_OutputSlot = null;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
		}

		#endregion // ISerializationCallbackReceiver
	}
}
