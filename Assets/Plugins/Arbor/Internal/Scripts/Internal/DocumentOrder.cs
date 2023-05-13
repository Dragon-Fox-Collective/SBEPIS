//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor.Internal
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ドキュメント化する際に使用する内部クラス。
	/// </summary>
#else
	/// <summary>
	/// Inner class to use when documenting.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
	public class DocumentOrder : Attribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// ドキュメントに表示する順番
		/// </summary>
#else
		/// <summary>
		/// Display order in document
		/// </summary>
#endif
		public readonly int order;

#if ARBOR_DOC_JA
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="order">ドキュメントに表示する順番</param>
#else
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="order">Display order in document</param>
#endif
		public DocumentOrder(int order)
		{
			this.order = order;
		}
	}
}