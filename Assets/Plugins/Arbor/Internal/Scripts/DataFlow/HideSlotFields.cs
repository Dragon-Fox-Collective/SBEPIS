//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System;
namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// DataSlotの追加フィールドを隠す属性。
	/// </summary>
	/// <remarks>
	/// 利用可能なクラス : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="OutputSlotComponent"/></description></item>
	/// <item><description><see cref="OutputSlotUnityObject"/></description></item>
	/// <item><description><see cref="OutputSlotTypable"/></description></item>
	/// <item><description><see cref="InputSlotTypable"/></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// Attribute to hide additional fields of DataSlot
	/// </summary>
	/// <remarks>
	/// Available classes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="OutputSlotComponent"/></description></item>
	/// <item><description><see cref="OutputSlotUnityObject"/></description></item>
	/// <item><description><see cref="OutputSlotTypable"/></description></item>
	/// <item><description><see cref="InputSlotTypable"/></description></item>
	/// </list>
	/// </remarks>
#endif
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public sealed class HideSlotFields : Attribute
	{
	}
}