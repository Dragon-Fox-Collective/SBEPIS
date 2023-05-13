//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Arbor.Internal
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 内部用
	/// </summary>
#else
	/// <summary>
	/// For internal
	/// </summary>
#endif
	[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Interface, AllowMultiple =false, Inherited = false)]
	public class DocumentManual : System.Attribute
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 内部用
		/// </summary>
#else
		/// <summary>
		/// For internal
		/// </summary>
#endif
		public readonly string link;

#if ARBOR_DOC_JA
		/// <summary>
		/// 内部用
		/// </summary>
		/// <param name="link">リンク</param>
#else
		/// <summary>
		/// For internal
		/// </summary>
		/// <param name="link">Link</param>
#endif
		public DocumentManual(string link)
		{
			this.link = link;
		}
	}
}