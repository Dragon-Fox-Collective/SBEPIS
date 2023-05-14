//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor.ValueFlow
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 値が取得できることを示すインターフェイス
	/// </summary>
	/// <typeparam name="T">値の型</typeparam>
#else
	/// <summary>
	/// Interface to indicate that the value can get
	/// </summary>
	/// <typeparam name="T">Value type</typeparam>
#endif
	public interface IValueGetter<T>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 値を取得する
		/// </summary>
		/// <returns>格納されている値</returns>
#else
		/// <summary>
		/// Get value
		/// </summary>
		/// <returns>Stored value</returns>
#endif
		T GetValue();
	}
}