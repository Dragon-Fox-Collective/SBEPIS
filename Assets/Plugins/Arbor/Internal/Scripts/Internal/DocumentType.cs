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
	public class DocumentType : Attribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// ドキュメントに表示する型
		/// </summary>
#else
		/// <summary>
		/// The type to display in the document
		/// </summary>
#endif
		public readonly Type type;

#if ARBOR_DOC_JA
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="type">ドキュメントに表示する型</param>
#else
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="type">The type to display in the document</param>
#endif
		public DocumentType(Type type)
		{
			this.type = type;
		}
	}
}