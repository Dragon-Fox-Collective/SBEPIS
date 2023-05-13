//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// 入力スロット
	/// </summary>
#else
	/// <summary>
	/// Input slot
	/// </summary>
#endif
	[System.Serializable]
	public abstract class InputSlotBase : DataSlot, IInputSlot, IValueGetter, IValueTryGetter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// ブランチのID
		/// </summary>
#else
		/// <summary>
		/// Branch ID
		/// </summary>
#endif
		public int branchID;

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
				return SlotType.Input;
			}
		}

		[System.NonSerialized]
		private bool _DirtyBranchCache = true;

		[System.NonSerialized]
		private DataBranch _BranchCache = null;

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchのキャッシュを変更するようマークする
		/// </summary>
#else
		/// <summary>
		/// Mark the Data Branch cache to change
		/// </summary>
#endif
		public override sealed void DirtyBranchCache()
		{
			_DirtyBranchCache = true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ブランチを取得する。
		/// </summary>
#else
		/// <summary>
		/// Get branch
		/// </summary>
#endif
		public DataBranch branch
		{
			get
			{
				if (_DirtyBranchCache)
				{
					_DirtyBranchCache = false;
					_BranchCache = nodeGraph != null ? nodeGraph.GetDataBranchFromID(branchID) : null;
				}

				return _BranchCache;
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
				DataBranch b = branch;
				if (b != null)
				{
					return b.isUsed;
				}
				return false;
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
				DataBranch b = branch;
				if (b != null)
				{
					return b.updatedTime;
				}
				return 0.0f;
			}
		}

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
		public void SetBranch(DataBranch branch)
		{
			this.branchID = branch.branchID;
			DirtyBranchCache();

			ConnectionChanged(true);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchの削除。
		/// </summary>
		/// <param name="branch">DataBranch</param>
		/// <returns>削除したらtrueを返す。</returns>
#else
		/// <summary>
		/// Remove DataBranch.
		/// </summary>
		/// <param name="branch">DataBranch</param>
		/// <returns>Returns true if removed.</returns>
#endif
		public bool RemoveBranch(DataBranch branch)
		{
			if (branchID == branch.branchID)
			{
				return ResetBranch();
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataBranchのリセット。
		/// </summary>
		/// <returns>削除したらtrueを返す。</returns>
#else
		/// <summary>
		/// Reset DataBranch
		/// </summary>
		/// <returns>Returns true if removed.</returns>
#endif
		public bool ResetBranch()
		{
			if (branchID != 0)
			{
				ClearBranch();

				ConnectionChanged(false);

				return true;
			}

			return false;
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
		public DataBranch GetBranch()
		{
			return branch;
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
		public bool IsConnected(DataBranch branch)
		{
			return branchID == branch.branchID;
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
			if (nodeGraph == null || branch == null || !object.ReferenceEquals(branch.inputSlot, this))
			{
				return;
			}

			nodeGraph.DeleteDataBranch(branch);
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
			branchID = 0;
			DirtyBranchCache();
		}

		internal void Copy(InputSlotBase inputSlot)
		{
			nodeGraph = inputSlot.nodeGraph;
			branchID = inputSlot.branchID;
			DirtyBranchCache();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 演算を行う。
		/// </summary>
		/// <returns>演算結果が格納されている出力スロットを返す。スロットが接続されていない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Perform the calculation.
		/// </summary>
		/// <returns>Returns the output slot in which the operation result is stored. Returns null if the slot is not connected.</returns>
#endif
		public OutputSlotBase Calculate()
		{
			DataBranch b = branch;

			if (b == null)
			{
				return null;
			}

			OutputSlotBase valueSlot = b.Calculate();
			if (valueSlot == null)
			{
				return null;
			}

			if (!b.isUsed)
			{
				return null;
			}

			return valueSlot;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を取得する
		/// </summary>
		/// <param name="value">取得する値</param>
		/// <returns>値が取得できたらtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value
		/// </summary>
		/// <param name="value">The value you get</param>
		/// <returns>Returns true if the value can be obtained.</returns>
#endif
		public bool GetValue(ref object value)
		{
			OutputSlotBase valueSlot = Calculate();
			if (valueSlot == null)
			{
				return false;
			}

			value = valueSlot.GetValue();

			return true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を取得する
		/// </summary>
		/// <typeparam name="T">取得する値の型</typeparam>
		/// <param name="value">取得する値</param>
		/// <returns>値が取得できたらtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value
		/// </summary>
		/// <typeparam name="T">Type of value to get</typeparam>
		/// <param name="value">The value you get</param>
		/// <returns>Returns true if the value can be obtained.</returns>
#endif
		[System.Obsolete("use TryGetValue<T>")]
		public bool GetValue<T>(ref T value)
		{
			return TryGetValue<T>(out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定した型での値の取得を試みる。
		/// </summary>
		/// <typeparam name="T">取得する値の型</typeparam>
		/// <param name="value">値の出力引数</param>
		/// <returns>値の取得に成功した場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Try to get a value of the specified type.
		/// </summary>
		/// <typeparam name="T">Value type</typeparam>
		/// <param name="value">Value Output Arguments</param>
		/// <returns>Returns true if the value is successfully retrieved.</returns>
#endif
		public bool TryGetValue<T>(out T value)
		{
			OutputSlotBase valueSlot = Calculate();
			if (valueSlot == null)
			{
				value = default(T);
				return false;
			}

			return valueSlot.TryGetValue<T>(out value);
		}

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
		[System.Obsolete("use SetBranch()")]
		public void SetInputBranch(DataBranch branch)
		{
			SetBranch(branch);
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
		[System.Obsolete("use RemoveBranch()")]
		public void RemoveInputBranch(DataBranch branch)
		{
			RemoveBranch(branch);
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
		[System.Obsolete("use GetBranch()")]
		public DataBranch GetInputBranch()
		{
			return GetBranch();
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
		[System.Obsolete("use IsConnected()")]
		public bool IsConnectedInput(DataBranch branch)
		{
			return branchID == branch.branchID;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値のobjectを取得する。
		/// </summary>
		/// <returns>値のobjectを返す。</returns>
#else
		/// <summary>
		/// Get the value object.
		/// </summary>
		/// <returns>Returns the value object.</returns>
#endif
		public object GetValueObject()
		{
			object value = null;
			if (GetValue(ref value))
			{
				return value;
			}
			return null;
		}
	}
}