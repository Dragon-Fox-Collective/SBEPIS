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
		/// Rigidbody型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Rigidbody type.
		/// </summary>
#endif
		public Rigidbody rigidbodyValue
		{
			get
			{
				Rigidbody value;
				if (TryGetRigidbody(out value))
				{
					return value;
				}
				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetRigidbody(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Rigidbody

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Rigidbody type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetRigidbody(Rigidbody value)
		{
			return SetComponent<Rigidbody>(value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Rigidbody type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetRigidbody(out Rigidbody value)
		{
			return TryGetComponent<Rigidbody>(out value);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Rigidbody型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Rigidbody type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Rigidbody GetRigidbody(Rigidbody defaultValue = null)
		{
			Rigidbody value;
			if (TryGetRigidbody(out value))
			{
				return value;
			}
			return defaultValue;
		}

		#endregion //Rigidbody
	}
}