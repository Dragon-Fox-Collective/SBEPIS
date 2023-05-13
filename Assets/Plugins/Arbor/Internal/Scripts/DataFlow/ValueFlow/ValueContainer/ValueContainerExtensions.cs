//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------

namespace Arbor.ValueFlow
{
#if ARBOR_DOC_JA
	/// <summary>
	/// ValueContainerの拡張クラス
	/// </summary>
#else
	/// <summary>
	/// ValueContainer extension class
	/// </summary>
#endif
	public static class ValueContainerExtensions
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 指定した型での値の取得を試みる。
		/// </summary>
		/// <typeparam name="T">取得する値の型</typeparam>
		/// <param name="container">値が格納されているIValueGetter</param>
		/// <param name="value">値の出力引数</param>
		/// <returns>値の取得に成功した場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Try to get a value of the specified type.
		/// </summary>
		/// <typeparam name="T">Value type</typeparam>
		/// <param name="container">IValueGetter where the value is stored</param>
		/// <param name="value">Value Output Arguments</param>
		/// <returns>Returns true if the value is successfully retrieved.</returns>
#endif
		public static bool TryGetValue<T>(this IValueGetter container, out T value)
		{
			IValueGetter<T> c = container as IValueGetter<T>;
			if (c != null)
			{
				value = c.GetValue();
				return true;
			}

			IValueTryGetter genericGetter = container as IValueTryGetter;
			if (genericGetter != null)
			{
				return genericGetter.TryGetValue<T>(out value);
			}

			try
			{
				value = (T)container.GetValueObject();
				return true;
			}
			catch (System.InvalidCastException)
			{
				value = default(T);
				return true;
			}
		}
	}
}
