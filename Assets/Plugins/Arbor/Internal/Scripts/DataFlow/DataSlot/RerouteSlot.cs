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
	/// DataBranchの中間点に使用するスロット。
	/// </summary>
#else
	/// <summary>
	/// Slot used for midpoint of DataBranch.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class RerouteSlot : DataSlot, IInputSlot, IOutputSlot, ISerializationCallbackReceiver, ISerializeVersionCallbackReceiver
	{
		#region Serialize Fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力ブランチのID
		/// </summary>
#else
		/// <summary>
		/// Input branch ID
		/// </summary>
#endif
		public int inputBranchID;

#if ARBOR_DOC_JA
		/// <summary>
		/// 出力ブランチのID
		/// </summary>
#else
		/// <summary>
		/// Output branch ID
		/// </summary>
#endif
		public List<int> outputBranchIDs = new List<int>();

#if ARBOR_DOC_JA
		/// <summary>
		/// 接続可能な値の型
		/// </summary>
#else
		/// <summary>
		/// Connectable value type
		/// </summary>
#endif
		public ClassTypeReference type = new ClassTypeReference();

		[SerializeField]
		[HideInInspector]
		private SerializeVersion _RerouteSlot_SerializeVersion = new SerializeVersion();

		#endregion // Serialize Fields

		#region Override

#if ARBOR_DOC_JA
		/// <summary>
		/// スロットの種類
		/// </summary>
#else
		/// <summary>
		/// Slot type
		/// </summary>
#endif
		public override SlotType slotType
		{
			get
			{
				return SlotType.Reroute;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// スロットに格納されるデータの型
		/// </summary>
#else
		/// <summary>
		/// The type of data stored in the slot
		/// </summary>
#endif
		public override System.Type dataType
		{
			get
			{
				return type;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 接続を切断する。
		/// </summary>
#else
		/// <summary>
		/// Disconnect the connection.
		/// </summary>
#endif
		public override void Disconnect()
		{
			if (nodeGraph == null)
			{
				return;
			}

			DataBranch inputBranch = inputSlot.GetBranch();
			if (inputBranch != null && object.ReferenceEquals(inputBranch.inputSlot, this))
			{
				nodeGraph.DeleteDataBranch(inputBranch);
			}

			int outputBranchCount = outputSlot.branchCount;
			for (int i = outputBranchCount - 1; i >= 0; --i)
			{
				DataBranch outputBranch = outputSlot.GetBranch(i);
				if (outputBranch != null && outputBranch.outputSlot == this)
				{
					nodeGraph.DeleteDataBranch(outputBranch);
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchのキャッシュを変更するようマークする
		/// </summary>
#else
		/// <summary>
		/// Mark the Data Branch cache to change
		/// </summary>
#endif
		public sealed override void DirtyBranchCache()
		{
			DirtyInputBranchCache();
			DirtyOutputBranchCache();
		}

		#endregion // Override

		#region IInputSlot

		[System.NonSerialized]
		private bool _DirtyInputBranchCache = true;

		[System.NonSerialized]
		private DataBranch _InputBranchCache = null;

		void DirtyInputBranchCache()
		{
			_DirtyInputBranchCache = true;
			_InputBranchCache = null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 入力スロットのインターフェイスにアクセスする
		/// </summary>
#else
		/// <summary>
		/// Access input slot interface
		/// </summary>
#endif
		public IInputSlot inputSlot
		{
			get
			{
				return this as IInputSlot;
			}
		}

		void IInputSlot.SetBranch(DataBranch branch)
		{
			inputBranchID = branch.branchID;
			DirtyInputBranchCache();

			ConnectionChanged(true);
		}

		bool IInputSlot.RemoveBranch(DataBranch branch)
		{
			if (inputBranchID == branch.branchID)
			{
				return inputSlot.ResetBranch();
			}

			return false;
		}

		bool IInputSlot.ResetBranch()
		{
			if (inputBranchID != 0)
			{
				inputBranchID = 0;
				if (outputBranchIDs.Count == 0)
				{
					nodeGraph = null;
				}

				DirtyInputBranchCache();

				ConnectionChanged(false);

				return true;
			}

			return false;
		}

		DataBranch IInputSlot.GetBranch()
		{
			if (_DirtyInputBranchCache)
			{
				_DirtyInputBranchCache = false;
				_InputBranchCache = nodeGraph != null ? nodeGraph.GetDataBranchFromID(inputBranchID) : null;
			}
			return _InputBranchCache;
		}

		bool IInputSlot.IsConnected(DataBranch branch)
		{
			return inputBranchID == branch.branchID;
		}

		#endregion // IInputSlot

		#region IOutputSlot

		[System.NonSerialized]
		private bool _DirtyOutputBranchCache = true;

		[System.NonSerialized]
		private List<DataBranch> _OutputBranchCacheList = new List<DataBranch>();

		void DirtyOutputBranchCache()
		{
			_DirtyOutputBranchCache = true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 出力スロットのインターフェイスにアクセスする
		/// </summary>
#else
		/// <summary>
		/// Access input slot interface
		/// </summary>
#endif
		public IOutputSlot outputSlot
		{
			get
			{
				return this as IOutputSlot;
			}
		}

		int IOutputSlot.branchCount
		{
			get
			{
				return outputBranchIDs.Count;
			}
		}

		void IOutputSlot.AddBranch(DataBranch branch)
		{
			if (outputBranchIDs.Contains(branch.branchID))
			{
				return;
			}

			outputBranchIDs.Add(branch.branchID);
			DirtyOutputBranchCache();

			ConnectionChanged(true);
		}

		bool IOutputSlot.RemoveBranch(DataBranch branch)
		{
			if (outputBranchIDs.Remove(branch.branchID))
			{
				if (outputBranchIDs.Count == 0 && inputBranchID == 0)
				{
					nodeGraph = null;
				}

				DirtyOutputBranchCache();

				ConnectionChanged(false);

				return true;
			}

			return false;
		}

		bool IOutputSlot.RemoveBranchAt(int index)
		{
			if (index < 0 || outputBranchIDs.Count <= index)
			{
				return false;
			}

			outputBranchIDs.RemoveAt(index);

			if (outputBranchIDs.Count == 0 && inputBranchID == 0)
			{
				nodeGraph = null;
			}

			DirtyOutputBranchCache();

			ConnectionChanged(false);

			return true;
		}

		DataBranch IOutputSlot.GetBranch(int index)
		{
			if (_DirtyOutputBranchCache)
			{
				_DirtyOutputBranchCache = false;
				_OutputBranchCacheList.Clear();

				if (nodeGraph != null)
				{
					int idCount = outputBranchIDs.Count;
					for (int idIndex = 0; idIndex < idCount; idIndex++)
					{
						int branchID = outputBranchIDs[idIndex];

						DataBranch branch = nodeGraph.GetDataBranchFromID(branchID);
#if ARBOR_DEBUG
						if (branch == null && branchID != 0)
						{
							UnityEngine.Debug.LogWarning("branch == null && branchID != 0 : " + branchID + " , " + nodeGraph, nodeGraph);
						}
#endif
						_OutputBranchCacheList.Add(branch);
					}
				}
			}

			return _OutputBranchCacheList[index];
		}

		bool IOutputSlot.IsConnected(DataBranch branch)
		{
			return outputBranchIDs.Contains(branch.branchID);
		}

		#endregion // IOutputSlot

		#region Obsolete

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの設定。
		/// </summary>
		/// <param name="branch">DataBranch</param>
#else
		/// <summary>
		/// Set DataBranch.
		/// </summary>
		/// <param name="branch">DataBranch</param>
#endif
		[System.Obsolete("use inputSlot.SetBranch()")]
		public void SetInputBranch(DataBranch branch)
		{
			inputSlot.SetBranch(branch);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの削除。
		/// </summary>
		/// <param name="branch">DataBranch</param>
#else
		/// <summary>
		/// Remove DataBranch.
		/// </summary>
		/// <param name="branch">DataBranch</param>
#endif
		[System.Obsolete("use inputSlot.RemoveBranch()")]
		public void RemoveInputBranch(DataBranch branch)
		{
			inputSlot.RemoveBranch(branch);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの取得。
		/// </summary>
		/// <returns>DataBranch</returns>
#else
		/// <summary>
		/// Get DataBranch.
		/// </summary>
		/// <returns>DataBranch</returns>
#endif
		[System.Obsolete("use inputSlot.GetBranch()")]
		public DataBranch GetInputBranch()
		{
			return inputSlot.GetBranch();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchと接続しているか判定する。
		/// </summary>
		/// <param name="branch">DataBranch</param>
		/// <returns>接続している場合にtrue、それ以外はfalse。</returns>
#else
		/// <summary>
		/// It judges whether it is connected with DataBranch.
		/// </summary>
		/// <param name="branch">DataBranch</param>
		/// <returns>True if connected, false otherwise.</returns>
#endif
		[System.Obsolete("use inputSlot.IsConnected()")]
		public bool IsConnectedInput(DataBranch branch)
		{
			return inputSlot.IsConnected(branch);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの追加。
		/// </summary>
		/// <param name="branch">DataBranch</param>
#else
		/// <summary>
		/// Addition of DataBranch.
		/// </summary>
		/// <param name="branch">DataBranch</param>
#endif
		[System.Obsolete("use outputSlot.AddBranch")]
		public void AddOutputBranch(DataBranch branch)
		{
			outputSlot.AddBranch(branch);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの削除。
		/// </summary>
		/// <param name="branch">DataBranch</param>
#else
		/// <summary>
		/// Remove DataBranch.
		/// </summary>
		/// <param name="branch">DataBranch</param>
#endif
		[System.Obsolete("use outputSlot.RemoveBranch()")]
		public void RemoveOutputBranch(DataBranch branch)
		{
			outputSlot.RemoveBranch(branch);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの個数を取得。
		/// </summary>
		/// <returns>DataBranchの個数</returns>
#else
		/// <summary>
		/// Get count of DataBranch.
		/// </summary>
		/// <returns>Count of DataBranch</returns>
#endif
		[System.Obsolete("use outputSlot.branchCount")]
		public int GetOutputBranchCount()
		{
			return outputSlot.branchCount;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの取得。
		/// </summary>
		/// <param name="index">インデックス</param>
		/// <returns>DataBranch</returns>
#else
		/// <summary>
		/// Get DataBranch.
		/// </summary>
		/// <param name="index">Index</param>
		/// <returns>DataBranch</returns>
#endif
		[System.Obsolete("use outputSlot.GetBranch()")]
		public DataBranch GetOutputBranch(int index)
		{
			return outputSlot.GetBranch(index);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchと接続しているか判定する。
		/// </summary>
		/// <param name="branch">DataBranch</param>
		/// <returns>接続している場合にtrue、それ以外はfalse。</returns>
#else
		/// <summary>
		/// It judges whether it is connected with DataBranch.
		/// </summary>
		/// <param name="branch">DataBranch</param>
		/// <returns>True if connected, false otherwise.</returns>
#endif
		[System.Obsolete("use outputSlot.IsConnected()")]
		public bool IsConnectedOutput(DataBranch branch)
		{
			return outputSlot.IsConnected(branch);
		}

		#endregion // Obsolete

		#region ISerializeVersionCallbackReceiver

		private const int kCurrentSerializeVersion = 1;

		int ISerializeVersionCallbackReceiver.newestVersion
		{
			get
			{
				return kCurrentSerializeVersion;
			}
		}

		void ISerializeVersionCallbackReceiver.OnInitialize()
		{
			_RerouteSlot_SerializeVersion.version = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			List<int> newOutputBranchIDs = new List<int>();
			for (int index = 0; index < outputBranchIDs.Count; index++)
			{
				int branchID = outputBranchIDs[index];
				if (newOutputBranchIDs.Contains(branchID))
				{
					continue;
				}

				newOutputBranchIDs.Add(branchID);
			}

			outputBranchIDs = newOutputBranchIDs;
		}

		void ISerializeVersionCallbackReceiver.OnSerialize(int version)
		{
			switch (version)
			{
				case 0:
					SerializeVer1();
					break;
			}
		}

		void ISerializeVersionCallbackReceiver.OnVersioning()
		{
		}

		#endregion // ISerializeVersionCallbackReceiver

		#region ISerializationCallbackReceiver

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			_RerouteSlot_SerializeVersion.BeforeDeserialize();
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			_RerouteSlot_SerializeVersion.AfterDeserialize();
		}

		#endregion // ISerializationCallbackReceiver

#if ARBOR_DOC_JA
		/// <summary>
		/// RerouteSlotのコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// RerouteSlot constructor
		/// </summary>
#endif
		public RerouteSlot() : base()
		{
			// Initialize when calling from script.
			_RerouteSlot_SerializeVersion.Initialize(this);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 接続状態をクリアする。DataBranchは残るため、コピー＆ペーストなどで接続状態のみ不要になった時に呼ぶ。
		/// </summary>
#else
		/// <summary>
		/// Clear the connection status. Since the DataBranch remains, call it when the connection status is no longer needed by copy and paste.
		/// </summary>
#endif
		public override void ClearBranch()
		{
			nodeGraph = null;

			inputBranchID = 0;
			DirtyInputBranchCache();

			outputBranchIDs.Clear();
			DirtyOutputBranchCache();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// エディタ用。接続状態をコピーする。
		/// </summary>
		/// <param name="slot">コピー元スロット</param>
#else
		/// <summary>
		/// For editor. Copy the connection status.
		/// </summary>
		/// <param name="slot">Source slot</param>
#endif
		public void Copy(RerouteSlot slot)
		{
			nodeGraph = slot.nodeGraph;

			inputBranchID = slot.inputBranchID;
			DirtyInputBranchCache();

			outputBranchIDs = new List<int>(slot.outputBranchIDs);
			DirtyOutputBranchCache();
		}
	}
}