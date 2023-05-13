//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// DataSlotのField情報
	/// </summary>
#else
	/// <summary>
	/// Field information of DataSlot
	/// </summary>
#endif
	[System.Obsolete("use DataSlot")]
	public sealed class DataSlotField
	{
		/// <summary>
		/// DataSlot
		/// </summary>
		public readonly DataSlot slot;

		/// <summary>
		/// FieldInfo
		/// </summary>
		[System.Obsolete("use DataSlot.fieldInfo")]
		public System.Reflection.FieldInfo fieldInfo
		{
			get
			{
				return slot.fieldInfo;
			}
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
		[System.Obsolete("use DataSlot.overrideConstraint")]
		public ClassConstraintInfo overrideConstraint
		{
			get
			{
				return slot.overrideConstraint;
			}
			set
			{
				slot.overrideConstraint = value;
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
		[System.Obsolete("use DataSlot.GetConstraint()")]
		public ClassConstraintInfo GetConstraint()
		{
			return slot.GetConstraint();
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
		[System.Obsolete("use DataSlot.connectableType")]
		public System.Type connectableType
		{
			get
			{
				return slot.connectableType;
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
		[System.Obsolete("use DataSlot.connectableTypeName")]
		public string connectableTypeName
		{
			get
			{
				return slot.connectableTypeName;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// スロットのGUIが有効であるかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether the GUI for the slot is valid.
		/// </summary>
#endif
		[System.Obsolete("use DataSlot.enabledGUI")]
		public bool enabled
		{
			get
			{
				return slot.enabledGUI;
			}
			set
			{
				slot.enabledGUI = value;
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
		[System.Obsolete("use DataSlot.isVisible")]
		public bool isVisible
		{
			get
			{
				return slot.isVisible;
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
		[System.Obsolete("use DataSlot.SetVisible")]
		public void SetVisible()
		{
			slot.SetVisible();
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
		[System.Obsolete("use DataSlot.ClearVisible")]
		public void ClearVisible()
		{
			slot.ClearVisible();
		}

		/// <summary>
		/// DataSlotField constructor
		/// </summary>
		/// <param name="slot">DataSlot</param>
		/// <param name="fieldInfo">FieldInfo</param>
		public DataSlotField(DataSlot slot, System.Reflection.FieldInfo fieldInfo)
		{
			this.slot = slot;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// DataSlotField同士が接続可能か判定する。
		/// </summary>
		/// <param name="inputSlotField">入力スロットフィールド</param>
		/// <param name="outputSlotField">出力スロットフィールド</param>
		/// <returns>接続可能であればtrueを返す。</returns>
#else
		/// <summary>
		/// Determine if DataSlotFields can connect to each other.
		/// </summary>
		/// <param name="inputSlotField">Input slot field</param>
		/// <param name="outputSlotField">Output slot field</param>
		/// <returns>Returns true if the connection is possible.</returns>
#endif
		[System.Obsolete("use DataSlot.IsConnectable")]
		public static bool IsConnectable(DataSlotField inputSlotField, DataSlotField outputSlotField)
		{
			if (inputSlotField == null || outputSlotField == null)
			{
				return false;
			}

			return DataSlot.IsConnectable(inputSlotField.slot, outputSlotField.slot);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 接続可能か判定する。
		/// </summary>
		/// <param name="slotField">判定するスロット</param>
		/// <returns>接続可能ならtrue、それ以外はfalseを返す。</returns>
#else
		/// <summary>
		/// It is judged whether connection is possible.
		/// </summary>
		/// <param name="slotField">Slot to determine</param>
		/// <returns>Returns true if connectable, false otherwise.</returns>
#endif
		[System.Obsolete("use DataSlot.IsConnectable")]
		public bool IsConnectable(DataSlotField slotField)
		{
			if (slotField == null)
			{
				//UnityEngine.Debug.Log("slotField == null");
				return false;
			}

			return slot.IsConnectable(slotField.slot);
		}
	}
}