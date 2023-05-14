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
		/// Quaternion型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Quaternion type.
		/// </summary>
#endif
		public Quaternion quaternionValue
		{
			get
			{
				Quaternion value;
				if (TryGetQuaternion(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetQuaternion(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Quaternion

#if ARBOR_DOC_JA
		/// <summary>
		/// Quaternion型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Quaternion type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetQuaternion(Quaternion value)
		{
			if (type == Type.Quaternion)
			{
				if (container._QuaternionParameters[_ParameterIndex] != value)
				{
					container._QuaternionParameters[_ParameterIndex] = value;
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Quaternion型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Quaternion type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetQuaternion(out Quaternion value)
		{
			if (type == Type.Quaternion)
			{
				value = container._QuaternionParameters[_ParameterIndex];

				return true;
			}

			value = Quaternion.identity;
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// Quaternion型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Quaternion type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Quaternion GetQuaternion(Quaternion defaultValue)
		{
			Quaternion value;
			if (TryGetQuaternion(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Quaternion型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はQuaternion.identityを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Quaternion type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns Quaternion.identity.</returns>
#endif
		public Quaternion GetQuaternion()
		{
			return GetQuaternion(Quaternion.identity);
		}

		#endregion //Quaternion
	}
}