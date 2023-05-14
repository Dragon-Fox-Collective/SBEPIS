//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor.Events
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ArborEventの選択ポップアップにメソッドを表示しないようにする属性。
	/// </summary>
#else
	/// <summary>
	/// Attribute that hides the method in the selection popup of ArborEvent.
	/// </summary>
#endif
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public sealed class HideEventAttribute : Attribute
	{
	}
}