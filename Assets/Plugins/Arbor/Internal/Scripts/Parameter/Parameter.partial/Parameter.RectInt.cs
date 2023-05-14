//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	using Arbor.Extensions;

	public sealed partial class Parameter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// RectInt型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of RectInt type.
		/// </summary>
#endif
		public RectInt rectIntValue
		{
			get
			{
				RectInt value;
				if (TryGetRectInt(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetRectInt(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region RectInt

#if ARBOR_DOC_JA
		/// <summary>
		/// RectInt型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the RectInt type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetRectInt(RectInt value)
		{
			if (type == Type.RectInt)
			{
				if (!container._RectIntParameters[_ParameterIndex].Equals(value))
				{
					container._RectIntParameters[_ParameterIndex] = value;
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectInt型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the RectInt type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetRectInt(out RectInt value)
		{
			if (type == Type.RectInt)
			{
				value = container._RectIntParameters[_ParameterIndex];

				return true;
			}

			value = RectIntExtensions.zero;
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// RectInt型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the RectInt type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public RectInt GetRectInt(RectInt defaultValue)
		{
			RectInt value;
			if (TryGetRectInt(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectInt型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はRectInt(0, 0, 0, 0)を返す。</returns>
#else
		/// <summary>
		/// Get the value of the RectInt type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns RectInt(0, 0, 0, 0).</returns>
#endif
		public RectInt GetRectInt()
		{
			return GetRectInt(RectIntExtensions.zero);
		}

#endregion //RectInt
	}
}