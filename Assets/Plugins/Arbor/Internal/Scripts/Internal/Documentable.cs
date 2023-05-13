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
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public sealed class DocumentableAttribute : Attribute
	{
	}
}