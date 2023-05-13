//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 型選択ポップアップで使用するフィルタを制限する属性。
	/// <see cref="ClassTypeReference"/>型のフィールドへ付けることで特定のフィルタのみ使用できるようになる。
	/// </summary>
#else
	/// <summary>
	/// An attribute that limits the filters used in the type selection popup.
	/// Only specified filter can be used by attaching to the field of <see cref="ClassTypeReference"/> type.
	/// </summary>
#endif
	[System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
	public sealed class TypeFilterAttribute : System.Attribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// フィルタのフラグ
		/// </summary>
#else
		/// <summary>
		/// Filter flags
		/// </summary>
#endif
		public TypeFilterFlags flags
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 型選択ポップアップで使用するフィルタを制限する。
		/// </summary>
		/// <param name="flags">制限するフィルタ</param>
#else
		/// <summary>
		/// Limit the filters used in the type selection popup.
		/// </summary>
		/// <param name="flags">Filter to restrict</param>
#endif
		public TypeFilterAttribute(TypeFilterFlags flags)
		{
			this.flags = flags;
		}
	}
}