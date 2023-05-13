//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	public sealed partial class Parameter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Color型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Color type.
		/// </summary>
#endif
		public Color colorValue
		{
			get
			{
				Color value;
				if (TryGetColor(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetColor(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Color

#if ARBOR_DOC_JA
		/// <summary>
		/// Color型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Color type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetColor(Color value)
		{
			if (type == Type.Color)
			{
				if (container._ColorParameters[_ParameterIndex] != value)
				{
					container._ColorParameters[_ParameterIndex] = value;
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Color型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Color type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetColor(out Color value)
		{
			if (type == Type.Color)
			{
				value = container._ColorParameters[_ParameterIndex];

				return true;
			}

			value = Color.white;
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// Color型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Color type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Color GetColor(Color defaultValue)
		{
			Color value;
			if (TryGetColor(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Color型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はColor.whiteを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Color type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns Color.white.</returns>
#endif
		public Color GetColor()
		{
			return GetColor(Color.white);
		}

		#endregion //Color
	}
}