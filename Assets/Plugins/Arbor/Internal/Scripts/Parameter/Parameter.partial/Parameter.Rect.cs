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
		/// Rect型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Rect type.
		/// </summary>
#endif
		public Rect rectValue
		{
			get
			{
				Rect value;
				if (TryGetRect(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetRect(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Rect

#if ARBOR_DOC_JA
		/// <summary>
		/// Rect型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Rect type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetRect(Rect value)
		{
			if (type == Type.Rect)
			{
				if (container._RectParameters[_ParameterIndex] != value)
				{
					container._RectParameters[_ParameterIndex] = value;
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rect型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Rect type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetRect(out Rect value)
		{
			if (type == Type.Rect)
			{
				value = container._RectParameters[_ParameterIndex];

				return true;
			}

			value = new Rect(0, 0, 0, 0);
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// Rect型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Rect type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Rect GetRect(Rect defaultValue)
		{
			Rect value;
			if (TryGetRect(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rect型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はRect(0, 0, 0, 0)を返す。</returns>
#else
		/// <summary>
		/// Get the value of the Rect type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns Rect(0, 0, 0, 0).</returns>
#endif
		public Rect GetRect()
		{
			return GetRect(new Rect(0, 0, 0, 0));
		}

		#endregion //Rect
	}
}