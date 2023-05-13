//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	public sealed partial class Parameter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Long型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Long type.
		/// </summary>
#endif
		public long longValue
		{
			get
			{
				long value;
				if (TryGetLong(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetLong(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Long

#if ARBOR_DOC_JA
		/// <summary>
		/// Long型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Long type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetLong(long value)
		{
			if (type == Type.Long)
			{
				if (container._LongParameters[_ParameterIndex] != value)
				{
					container._LongParameters[_ParameterIndex] = value;
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Long型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Long type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetLong(out long value)
		{
			if (type == Type.Long)
			{
				value = container._LongParameters[_ParameterIndex];

				return true;
			}

			value = 0;
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// Long型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Long type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public long GetLong(long defaultValue = default(long))
		{
			long value;
			if (TryGetLong(out value))
			{
				return value;
			}
			return defaultValue;
		}

		#endregion //Long
	}
}