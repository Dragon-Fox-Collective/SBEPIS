//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Component型の入力スロット
	/// </summary>
	/// <remarks>
	/// 使用可能な属性 : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// </remarks>
#else
	/// <summary>
	/// Component type of input slot
	/// </summary>
	/// <remarks>
	/// Available Attributes : <br/>
	/// <list type="bullet">
	/// <item><description><see cref="ClassTypeConstraintAttribute" /></description></item>
	/// <item><description><see cref="SlotTypeAttribute" /></description></item>
	/// </list>
	/// </remarks>
#endif
	[System.Serializable]
	public sealed class InputSlotComponent : InputSlot<Component>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 値を取得する。
		/// </summary>
		/// <typeparam name="T">取得する値の型</typeparam>
		/// <returns>取得した値。ない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get value.
		/// </summary>
		/// <typeparam name="T">Type of value to get.</typeparam>
		/// <returns>The value you got. If there is none, it returns null.</returns>
#endif
		public T GetValue<T>() where T : Component
		{
			Component obj = null;
			if (GetValue(ref obj))
			{
				return obj as T;
			}
			return null;
		}
	}
}