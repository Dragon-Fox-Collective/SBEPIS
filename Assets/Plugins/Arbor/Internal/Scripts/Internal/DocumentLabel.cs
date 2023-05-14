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
	public class DocumentLabel : Attribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// ラベル名
		/// </summary>
#else
		/// <summary>
		/// Label name
		/// </summary>
#endif
		public string name
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">ラベル名</param>
#else
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="name">Label name</param>
#endif
		public DocumentLabel(string name)
		{
			this.name = name;
		}
	}
}