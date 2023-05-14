//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor.Internal
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ドキュメントから隠す属性
	/// </summary>
#else
	/// <summary>
	/// Attributes to hide in documents
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum | AttributeTargets.Interface | AttributeTargets.Delegate | AttributeTargets.Constructor | AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event)]
	public sealed class HideInDocument : Attribute
	{
	}
}