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
		/// Bounds型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Bounds type.
		/// </summary>
#endif
		public Bounds boundsValue
		{
			get
			{
				Bounds value;
				if (TryGetBounds(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetBounds(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Bounds

#if ARBOR_DOC_JA
		/// <summary>
		/// Bounds型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Bounds type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetBounds(Bounds value)
		{
			if (type == Type.Bounds)
			{
				if (container._BoundsParameters[_ParameterIndex] != value)
				{
					container._BoundsParameters[_ParameterIndex] = value;
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bounds型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Bounds type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetBounds(out Bounds value)
		{
			if (type == Type.Bounds)
			{
				value = container._BoundsParameters[_ParameterIndex];
				;

				return true;
			}

			value = new Bounds();
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bounds型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Bounds type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Bounds GetBounds(Bounds defaultValue)
		{
			Bounds value;
			if (TryGetBounds(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Bounds型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合は0 Boundsを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Bounds type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns 0 bounds.</returns>
#endif
		public Bounds GetBounds()
		{
			return GetBounds(new Bounds());
		}

		#endregion //Bounds
	}
}