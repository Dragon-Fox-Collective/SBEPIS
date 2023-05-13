//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;

namespace Arbor.Events
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ArborEventに使用できない引数のメソッドであっても選択ポップアップに表示するようにする属性
	/// </summary>
	/// <remarks>使用可能な引数の型は <see cref="ParameterType" /> を参照。</remarks>
#else
	/// <summary>
	/// Attributes to be displayed in selection popup even argument methods that can not be used for ArborEvent
	/// </summary>
	/// <remarks>See <see cref="ParameterType" /> for available argument types.</remarks>
#endif
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false)]
	public sealed class ShowEventAttribute : Attribute
	{
	}
}