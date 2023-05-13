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
		/// RectTransform型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of RectTransform type.
		/// </summary>
#endif
		public RectTransform rectTransformValue
		{
			get
			{
				RectTransform value;
				if (TryGetRectTransform(out value))
				{
					return value;
				}
				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetRectTransform(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region RectTransform

#if ARBOR_DOC_JA
		/// <summary>
		/// RectTransform型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the RectTransform type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetRectTransform(RectTransform value)
		{
			return SetComponent<RectTransform>(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// RectTransform型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the RectTransform type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetRectTransform(out RectTransform value)
		{
			return TryGetComponent<RectTransform>(out value);
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// RectTransform型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the RectTransform type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public RectTransform GetRectTransform(RectTransform defaultValue = null)
		{
			RectTransform value;
			if (TryGetRectTransform(out value))
			{
				return value;
			}
			return defaultValue;
		}

		#endregion //RectTransform
	}
}