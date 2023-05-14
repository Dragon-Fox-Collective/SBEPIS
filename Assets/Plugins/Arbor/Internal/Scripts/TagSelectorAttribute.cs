//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// <see cref="Arbor.FlexibleString"/>のタイプがConstantの時にタグ選択ポップアップを表示する。
	/// </summary>
#else
	/// <summary>
	/// Display the tag selection popup when the type of <see cref="Arbor.FlexibleString"/> is Constant.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
	public sealed class TagSelectorAttribute : Attribute
	{
	}
}