//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Component型の入力スロット(ジェネリック)
	/// </summary>
	/// <typeparam name="T">入力するComponentの型</typeparam>
#else
	/// <summary>
	/// Component type of input slot(Generic)
	/// </summary>
	/// <typeparam name="T">The type of the Component to input</typeparam>
#endif
	public class InputSlotComponent<T> : InputSlot<T> where T : Component
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="InputSlotComponent{T}"/>を<see cref="InputSlotComponent"/>にキャスト。
		/// </summary>
		/// <param name="inputSlot"><see cref="InputSlotComponent{T}"/></param>
		/// <returns>InputSlotComponentにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast <see cref="InputSlotComponent{T}"/> to <see cref="InputSlotComponent"/>.
		/// </summary>
		/// <param name="inputSlot"><see cref="InputSlotComponent{T}"/></param>
		/// <returns>Returns the result of casting to InputSlotComponent.</returns>
#endif
		public static explicit operator InputSlotComponent(InputSlotComponent<T> inputSlot)
		{
			InputSlotComponent inputSlotComponent = new InputSlotComponent();
			inputSlotComponent.Copy(inputSlot);
			return inputSlotComponent;
		}
	}
}