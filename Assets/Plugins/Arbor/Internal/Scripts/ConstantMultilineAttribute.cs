//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// <see cref="Arbor.FlexibleString"/>のタイプがConstantの時に複数行表示にする
	/// </summary>
#else
	/// <summary>
	/// Display multiple lines when the type of <see cref="Arbor.FlexibleString"/> is Constant.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public sealed class ConstantMultilineAttribute : Attribute
	{
	}
}