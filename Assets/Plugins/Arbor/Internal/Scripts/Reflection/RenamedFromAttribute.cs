//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 名前の変更を設定する属性。
	/// </summary>
#else
	/// <summary>
	/// The attribute that sets the rename.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface | AttributeTargets.Enum | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
	public class RenamedFromAttribute : Attribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 以前の名前
		/// </summary>
#else
		/// <summary>
		/// Old name
		/// </summary>
#endif
		public string oldName
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 名前の変更を設定する
		/// </summary>
		/// <param name="oldName">以前の名前</param>
#else
		/// <summary>
		/// Sets the rename
		/// </summary>
		/// <param name="oldName">Old name</param>
#endif
		public RenamedFromAttribute(string oldName)
		{
			this.oldName = oldName;
		}
	}
}