//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 型選択ポップアップで非表示にする属性
	/// </summary>
#else
	/// <summary>
	/// Attributes to hide in the type selection popup
	/// </summary>
#endif
	[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Struct, AllowMultiple = false)]
	public sealed class HideTypeAttribute : System.Attribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 継承したクラスも非表示にするかどうか
		/// </summary>
#else
		/// <summary>
		/// Whether to also hide inherited classes
		/// </summary>
#endif
		public bool forChildren
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 型選択ポップアップで非表示にする
		/// </summary>
#else
		/// <summary>
		/// Hide in type selection popup
		/// </summary>
#endif
		public HideTypeAttribute()
		{
			forChildren = false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 型選択ポップアップで非表示にする
		/// </summary>
		/// <param name="forChildren">継承したクラスも非表示にするかどうか</param>
#else
		/// <summary>
		/// Hide in type selection popup
		/// </summary>
		/// <param name="forChildren">Whether to also hide inherited classes</param>
#endif
		public HideTypeAttribute(bool forChildren)
		{
			this.forChildren = forChildren;
		}
	}
}