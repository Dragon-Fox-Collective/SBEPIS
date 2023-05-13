//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// <see cref="ComponentParameterReference" />や<see cref="FlexibleComponent" />などに接続可能な型を指定する属性。
	/// </summary>
#else
	/// <summary>
	/// An attribute that specifies a connectable type such as <see cref="ComponentParameterReference" /> or <see cref="FlexibleComponent" />.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public sealed class SlotTypeAttribute : Attribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 接続可能な型
		/// </summary>
#else
		/// <summary>
		/// Connectable type
		/// </summary>
#endif
		public Type connectableType
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 接続可能な型を指定してSlotTypeAttributeを作成する。
		/// </summary>
		/// <param name="connectableType">接続可能な型</param>
#else
		/// <summary>
		/// To create a SlotTypeAttribute by specifying the possible connection types.
		/// </summary>
		/// <param name="connectableType">Connectable type</param>
#endif
		public SlotTypeAttribute(Type connectableType)
		{
			this.connectableType = connectableType;
		}
	}
}