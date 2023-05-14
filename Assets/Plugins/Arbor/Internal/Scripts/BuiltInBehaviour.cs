//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 組み込みBehaviourを定義する属性。Arbor内部で使用する。
	/// </summary>
#else
	/// <summary>
	/// Attributes that define the built-in Behaviour. Arbor used internally.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
	public sealed class BuiltInBehaviour : Attribute
	{
	}
}
