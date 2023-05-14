//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// 入力スロットのジェネリッククラス
	/// </summary>
	/// <typeparam name="T">データの型</typeparam>
#else
	/// <summary>
	/// Generic class of the input slot
	/// </summary>
	/// <typeparam name="T">Type of data</typeparam>
#endif
	[System.Serializable]
	public class InputSlot<T> : InputSlotBase, IValueGetter<T>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// スロットに格納されるデータの型
		/// </summary>
#else
		/// <summary>
		/// The type of data stored in the slot
		/// </summary>
#endif
		public override System.Type dataType
		{
			get
			{
				return typeof(T);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を取得する
		/// </summary>
		/// <param name="value">取得する値</param>
		/// <returns>値が取得できたらtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value
		/// </summary>
		/// <param name="value">The value you get</param>
		/// <returns>Returns true if the value can be obtained.</returns>
#endif
		public bool GetValue(ref T value)
		{
			return TryGetValue<T>(out value);
		}

		T IValueGetter<T>.GetValue()
		{
			T value = default(T);
			if (TryGetValue<T>(out value))
			{
				return value;
			}
			return default(T);
		}
	}
}