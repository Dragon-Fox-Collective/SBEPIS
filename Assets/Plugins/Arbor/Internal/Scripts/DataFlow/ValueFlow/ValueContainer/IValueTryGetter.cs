//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor.ValueFlow
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ジェネリックメソッドで値の取得を試みることを示すインターフェイス
	/// </summary>
#else
	/// <summary>
	/// An interface that indicates trying to get a value with a generic method
	/// </summary>
#endif
	public interface IValueTryGetter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 指定した型での値の取得を試みる。
		/// </summary>
		/// <typeparam name="T">取得する値の型</typeparam>
		/// <param name="value">値の出力引数</param>
		/// <returns>値の取得に成功した場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Try to get a value of the specified type.
		/// </summary>
		/// <typeparam name="T">Value type</typeparam>
		/// <param name="value">Value Output Arguments</param>
		/// <returns>Returns true if the value is successfully retrieved.</returns>
#endif
		bool TryGetValue<T>(out T value);
	}
}