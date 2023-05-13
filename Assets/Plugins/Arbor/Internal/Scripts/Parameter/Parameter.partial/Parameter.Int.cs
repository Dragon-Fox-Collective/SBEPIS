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
		/// Int型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Int type.
		/// </summary>
#endif
		public int intValue
		{
			get
			{
				int value;
				if (TryGetInt(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetInt(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Int

		void Internal_SetInt(int value)
		{
			if (container._IntParameters[_ParameterIndex] != value)
			{
				container._IntParameters[_ParameterIndex] = value;
				DoChanged();
			}
		}

		int Internal_GetInt()
		{
			return container._IntParameters[_ParameterIndex];
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Int型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Int type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetInt(int value)
		{
			if (type == Type.Int || type == Type.Enum)
			{
				Internal_SetInt(value);
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Int型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Int type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetInt(out int value)
		{
			if (type == Type.Int || type == Type.Enum)
			{
				value = Internal_GetInt();

				return true;
			}

			value = 0;
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// Int型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Int type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public int GetInt(int defaultValue = default(int))
		{
			int value;
			if (TryGetInt(out value))
			{
				return value;
			}
			return defaultValue;
		}

		#endregion //Int
	}
}