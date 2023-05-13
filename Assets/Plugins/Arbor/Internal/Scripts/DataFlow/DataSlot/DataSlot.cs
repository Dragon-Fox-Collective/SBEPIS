//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// データフローと接続するスロットのインターフェイス
	/// </summary>
#else
	/// <summary>
	/// The interface of the slot that connects to the data flow
	/// </summary>
#endif
	public interface IDataSlot
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// スロットの種類
		/// </summary>
#else
		/// <summary>
		/// Slot type
		/// </summary>
#endif
		SlotType slotType
		{
			get;
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
		System.Type dataType
		{
			get;
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
		void Disconnect();
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// 演算ノードを接続するためのスロット。
	/// </summary>
#else
	/// <summary>
	/// Slot for connecting a calculator node.
	/// </summary>
#endif
	[System.Serializable]
	public abstract class DataSlot : IDataSlot, IOverrideConstraint
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// スロットの種類
		/// </summary>
#else
		/// <summary>
		/// Slot type
		/// </summary>
#endif
		public abstract SlotType slotType
		{
			get;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// スロットが属しているNodeGraph
		/// </summary>
#else
		/// <summary>
		/// NodeGraph slot belongs
		/// </summary>
#endif
		[FormerlySerializedAs("stateMachine")]
		public NodeGraph nodeGraph;

#if ARBOR_DOC_JA
		/// <summary>
		/// スロットのArborEditor上の位置(Editor Only)
		/// </summary>
#else
		/// <summary>
		/// Position on ArborEditor of slot(Editor Only)
		/// </summary>
#endif
		[System.NonSerialized]
		public Rect position;

#if ARBOR_DOC_JA
		/// <summary>
		/// 接続が変更されたときのコールバックイベント
		/// </summary>
#else
		/// <summary>
		/// Callback event when connection is changed
		/// </summary>
#endif
		public event System.Action<bool> onConnectionChanged;

#if ARBOR_DOC_JA
		/// <summary>
		/// エディタ用
		/// </summary>
#else
		/// <summary>
		/// For Editor
		/// </summary>
#endif
		public bool isValidField
		{
			get
			{
				return fieldInfo != null;
			}
		}

		internal System.Reflection.FieldInfo fieldInfo
		{
			get;
			private set;
		}

#pragma warning disable 0618
		private DataSlotField _SlotField = null;
#pragma warning restore 0618

		/// <summary>
		/// DataSlotField
		/// </summary>
		[System.Obsolete("use this slot direct")]
		public DataSlotField slotField
		{
			get
			{
				return _SlotField ?? (_SlotField = new DataSlotField(this, null));
			}
		}

		private ClassConstraintInfo _ConstraintInfo;

		[System.NonSerialized]
		private ClassConstraintInfo _OverrideConstraint;

#if ARBOR_DOC_JA
		/// <summary>
		/// 接続可能な型が変更されたときに呼び出される。
		/// </summary>
#else
		/// <summary>
		/// Called when the connectable type changes.
		/// </summary>
#endif
		public event System.Action onConnectableTypeChanged;

#if ARBOR_DOC_JA
		/// <summary>
		/// 接続可能な型を変更したときに呼び出す。
		/// </summary>
#else
		/// <summary>
		/// Called when the connectable type is changed.
		/// </summary>
#endif
		protected void ConnectableTypeChanged()
		{
			onConnectableTypeChanged?.Invoke();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 上書きする型制約の情報
		/// </summary>
#else
		/// <summary>
		/// override ClassConstraintInfo
		/// </summary>
#endif
		public ClassConstraintInfo overrideConstraint
		{
			get
			{
				return _OverrideConstraint;
			}
			set
			{
				if (_OverrideConstraint != value)
				{
					_OverrideConstraint = value;

					ConnectableTypeChanged();
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 型制約の情報を返す。
		/// </summary>
		/// <returns>型制約の情報</returns>
#else
		/// <summary>
		/// Return information on type constraints.
		/// </summary>
		/// <returns>Type constraint information</returns>
#endif
		public ClassConstraintInfo GetConstraint()
		{
			if (overrideConstraint != null)
			{
				return overrideConstraint;
			}

			if (_ConstraintInfo != null)
			{
				return _ConstraintInfo;
			}

			return null;
		}

		internal void SetupField(System.Reflection.FieldInfo fieldInfo)
		{
			this.fieldInfo = fieldInfo;

			DataSlot slot = this;
			bool isConstraintableInputSlot = slot is InputSlotComponent || slot is InputSlotUnityObject || slot is InputSlotAny;
			if (isConstraintableInputSlot)
			{
				ClassTypeConstraintAttribute constraint = AttributeHelper.GetAttribute<ClassTypeConstraintAttribute>(fieldInfo);
				if (constraint != null)
				{
					_ConstraintInfo = new ClassConstraintInfo() { constraintAttribute = constraint, constraintFieldInfo = fieldInfo };
				}
			}

			if (_ConstraintInfo == null)
			{
				bool isConstraintableOutputSlot = slot is OutputSlotAny;
				if (isConstraintableInputSlot || isConstraintableOutputSlot)
				{
					SlotTypeAttribute slotType = AttributeHelper.GetAttribute<SlotTypeAttribute>(fieldInfo);
					if (slotType != null)
					{
						_ConstraintInfo = new ClassConstraintInfo() { slotTypeAttribute = slotType };
					}
				}
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
		public abstract System.Type dataType
		{
			get;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 接続可能な型
		/// </summary>
#else
		/// <summary>
		/// Connectable type
		/// </summary>
#endif
		public System.Type connectableType
		{
			get
			{
				ClassConstraintInfo constraint = GetConstraint();
				if (constraint != null)
				{
					return constraint.GetConstraintBaseType();
				}
				return dataType;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 接続可能な型名
		/// </summary>
#else
		/// <summary>
		/// Connectable type name
		/// </summary>
#endif
		public string connectableTypeName
		{
			get
			{
				ClassConstraintInfo constraint = GetConstraint();
				if (constraint != null)
				{
					return constraint.GetConstraintTypeName();
				}
				
				return TypeUtility.GetSlotTypeName(dataType);
			}
		}

		private bool _EnabledGUI = true;
		private bool _IsVisible = true;
		private bool _IsVisibleNext = false;

#if ARBOR_DOC_JA
		/// <summary>
		/// スロットのGUIが有効であるかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether the GUI for the slot is valid.
		/// </summary>
#endif
		public bool enabledGUI
		{
			get
			{
				return _EnabledGUI;
			}
			set
			{
				_EnabledGUI = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
#endif
		public bool isVisible
		{
			get
			{
				return _IsVisible;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
#endif
		public void SetVisible()
		{
			_IsVisibleNext = true;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Editor用。
		/// </summary>
#else
		/// <summary>
		/// For Editor.
		/// </summary>
#endif
		public void ClearVisible()
		{
			_IsVisible = _IsVisibleNext;
			_IsVisibleNext = false;
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
		public abstract void Disconnect();

#if ARBOR_DOC_JA
		/// <summary>
		/// 接続状態をクリアする。DataBranchは残るため、コピー＆ペーストなどで接続状態のみ不要になった時に呼ぶ。
		/// </summary>
#else
		/// <summary>
		/// Clear the connection status. Since the DataBranch remains, call it when the connection status is no longer needed by copy and paste.
		/// </summary>
#endif
		public abstract void ClearBranch();

		internal void ConnectionChanged(bool isConnect)
		{
			onConnectionChanged?.Invoke(isConnect);
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
		public abstract void DirtyBranchCache();

		internal void ClearDataBranchSlot()
		{
			DirtyBranchCache();

			DataSlot slot = this;
			IInputSlot inputSlot = slot as IInputSlot;
			if (inputSlot != null)
			{
				DataBranch branch = inputSlot.GetBranch();
				if (branch != null)
				{
					branch._InputSlot = null;
				}
			}

			IOutputSlot outputSlot = slot as IOutputSlot;
			if (outputSlot != null)
			{
				for (int i = outputSlot.branchCount - 1; i >= 0; i--)
				{
					DataBranch branch = outputSlot.GetBranch(i);
					if (branch != null)
					{
						branch._OutputSlot = null;
					}
				}
			}
		}

		internal void SetupDataBranchSlot()
		{
			DirtyBranchCache();
			IInputSlot inputSlot = this as IInputSlot;
			if (inputSlot != null)
			{
				DataBranch branch = inputSlot.GetBranch();
				if (branch != null)
				{
					if (branch._InputSlot == null)
					{
						branch._InputSlot = this;
					}
					else if (!object.ReferenceEquals(branch._InputSlot, this))
					{
						inputSlot.RemoveBranch(branch);
					}
				}
			}

			IOutputSlot outputSlot = this as IOutputSlot;
			if (outputSlot != null)
			{
				for (int i = outputSlot.branchCount - 1; i >= 0; i--)
				{
					DataBranch branch = outputSlot.GetBranch(i);
					if (branch != null)
					{
						if (branch._OutputSlot == null)
						{
							branch._OutputSlot = this;
						}
						else if (!object.ReferenceEquals(branch._OutputSlot, this))
						{
							outputSlot.RemoveBranch(branch);
						}
					}
				}
			}
		}

		internal void RefreshDataBranchSlot()
		{
			DirtyBranchCache();

			IInputSlot inputSlot = this as IInputSlot;
			if (inputSlot != null)
			{
				DataBranch branch = inputSlot.GetBranch();
				if (branch == null)
				{
					inputSlot.ResetBranch();
				}
			}

			IOutputSlot outputSlot = this as IOutputSlot;
			if (outputSlot != null)
			{
				for (int i = outputSlot.branchCount - 1; i >= 0; i--)
				{
					DataBranch branch = outputSlot.GetBranch(i);
					if (branch == null)
					{
						outputSlot.RemoveBranchAt(i);
					}
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 接続可能か判定する。
		/// </summary>
		/// <param name="slot">判定するスロット</param>
		/// <returns>接続可能ならtrue、それ以外はfalseを返す。</returns>
#else
		/// <summary>
		/// It is judged whether connection is possible.
		/// </summary>
		/// <param name="slot">Slot to determine</param>
		/// <returns>Returns true if connectable, false otherwise.</returns>
#endif
		public bool IsConnectable(DataSlot slot)
		{
			DataSlot inputSlot = null;
			DataSlot outputSlot = null;

			if (slot == null)
			{
				//UnityEngine.Debug.Log("slotField == null");
				return false;
			}

			switch (slotType)
			{
				case SlotType.Input:
					if (slot.slotType == SlotType.Input)
					{
						return false;
					}

					inputSlot = this;
					outputSlot = slot;
					break;
				case SlotType.Output:
				case SlotType.Reroute:
					if (slot.slotType == SlotType.Output)
					{
						return false;
					}

					inputSlot = slot;
					outputSlot = this;
					break;
			}

			return IsConnectable(inputSlot, outputSlot);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlot同士が接続可能か判定する。
		/// </summary>
		/// <param name="inputSlot">入力スロット</param>
		/// <param name="outputSlot">出力スロット</param>
		/// <returns>接続可能であればtrueを返す。</returns>
#else
		/// <summary>
		/// Determine if DataSlots can connect to each other.
		/// </summary>
		/// <param name="inputSlot">Input slot</param>
		/// <param name="outputSlot">Output slot</param>
		/// <returns>Returns true if the connection is possible.</returns>
#endif
		public static bool IsConnectable(DataSlot inputSlot, DataSlot outputSlot)
		{
			if (inputSlot == null || outputSlot == null)
			{
				return false;
			}

			System.Type inputSlotType = inputSlot.dataType;
			System.Type outputConnectableType = outputSlot.connectableType;

			bool isAnyInput = inputSlotType == null || inputSlotType == typeof(object);

			if (!isAnyInput && !TypeUtility.IsAssignableFrom(inputSlotType, outputConnectableType))
			{
				return false;
			}

			ClassConstraintInfo constraint = inputSlot.GetConstraint();
			if (constraint != null)
			{
				return constraint.IsConstraintSatisfied(outputConnectableType);
			}

			return true;
		}
	}
}
