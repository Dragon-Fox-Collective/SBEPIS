//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
#if ARBOR_DOC_JA
	/// <summary>
	/// Component型の出力スロット(ジェネリック)
	/// </summary>
	/// <typeparam name="T">出力するComponentの型</typeparam>
#else
	/// <summary>
	/// Component type of output slot(Generic)
	/// </summary>
	/// <typeparam name="T">The type of the Component to output</typeparam>
#endif
	public class OutputSlotComponent<T> : OutputSlot<T> where T : Component
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="OutputSlotComponent{T}"/>を<see cref="OutputSlotComponent"/>にキャスト。
		/// </summary>
		/// <param name="outputSlot"><see cref="OutputSlotComponent{T}"/></param>
		/// <returns>OutputSlotComponentにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast <see cref="OutputSlotComponent{T}"/> to <see cref="OutputSlotComponent"/>.
		/// </summary>
		/// <param name="outputSlot"><see cref="OutputSlotComponent{T}"/></param>
		/// <returns>Returns the result of casting to OutputSlotComponent.</returns>
#endif
		public static explicit operator OutputSlotComponent(OutputSlotComponent<T> outputSlot)
		{
			OutputSlotComponent outputSlotComponent = new OutputSlotComponent();
			outputSlotComponent.Copy(outputSlot);
			return outputSlotComponent;
		}
	}
}