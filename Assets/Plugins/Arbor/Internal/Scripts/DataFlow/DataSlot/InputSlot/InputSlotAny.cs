//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 型を指定する入力スロットクラス
	/// </summary>
	/// <remarks>
	/// 使用可能な属性 : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// Input slot class specifying type
	/// </summary>
	/// <remarks>
	/// Available Attributes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// </remarks>
#endif
	[System.Serializable]
	public sealed class InputSlotAny : InputSlotBase
	{
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
				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// InputSlotAnyデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// InputSlotAny default constructor
		/// </summary>
#endif
		public InputSlotAny()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// InputSlotAnyコンストラクタ
		/// </summary>
		/// <param name="inputSlot">コピー元の入力スロット</param>
#else
		/// <summary>
		/// InputSlotAny constructor
		/// </summary>
		/// <param name="inputSlot">Copy source input slot</param>
#endif
		internal InputSlotAny(InputSlotBase inputSlot)
		{
			Copy(inputSlot);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// InputSlotAnyコンストラクタ
		/// </summary>
		/// <param name="targetType">入力の型</param>
#else
		/// <summary>
		/// InputSlotAny constructor
		/// </summary>
		/// <param name="targetType">Input type</param>
#endif
		[System.Obsolete("use ClassExtendsAttribute or SlotTypeAttribute in the field.", true)]
		public InputSlotAny(System.Type targetType)
		{
		}
	}
}