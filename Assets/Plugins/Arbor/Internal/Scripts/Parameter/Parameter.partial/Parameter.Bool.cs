﻿//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	public sealed partial class Parameter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Bool型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Bool type.
		/// </summary>
#endif
		public bool boolValue
		{
			get
			{
				bool value;
				if (TryGetBool(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetBool(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Bool

#if ARBOR_DOC_JA
		/// <summary>
		/// Bool型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Bool type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetBool(bool value)
		{
			if (type == Type.Bool)
			{
				if (container._BoolParameters[_ParameterIndex] != value)
				{
					container._BoolParameters[_ParameterIndex] = value;
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bool型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Bool type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetBool(out bool value)
		{
			if (type == Type.Bool)
			{
				value = container._BoolParameters[_ParameterIndex];

				return true;
			}

			value = false;
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// Bool型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Bool type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public bool GetBool(bool defaultValue = default(bool))
		{
			bool value;
			if (TryGetBool(out value))
			{
				return value;
			}
			return defaultValue;
		}

		#endregion //Bool
	}
}