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
		/// String型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of String type.
		/// </summary>
#endif
		public string stringValue
		{
			get
			{
				string value;
				if (TryGetString(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetString(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region String

#if ARBOR_DOC_JA
		/// <summary>
		/// String型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the String type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetString(string value)
		{
			if (type == Type.String)
			{
				if (container._StringParameters[_ParameterIndex] != value)
				{
					container._StringParameters[_ParameterIndex] = value;
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// String型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the String type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetString(out string value)
		{
			if (type == Type.String)
			{
				value = container._StringParameters[_ParameterIndex];
				;

				return true;
			}

			value = "";
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// String型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the String type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public string GetString(string defaultValue = "")
		{
			string value;
			if (TryGetString(out value))
			{
				return value;
			}
			return defaultValue;
		}

		#endregion //String
	}
}