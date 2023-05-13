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
		/// Vector3型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Vector3 type.
		/// </summary>
#endif
		public Vector3 vector3Value
		{
			get
			{
				Vector3 value;
				if (TryGetVector3(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetVector3(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Vector3

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector3 type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetVector3(Vector3 value)
		{
			if (type == Type.Vector3)
			{
				if (container._Vector3Parameters[_ParameterIndex] != value)
				{
					container._Vector3Parameters[_ParameterIndex] = value;
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3 type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetVector3(out Vector3 value)
		{
			if (type == Type.Vector3)
			{
				value = container._Vector3Parameters[_ParameterIndex];

				return true;
			}

			value = Vector3.zero;
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3 type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Vector3 GetVector3(Vector3 defaultValue)
		{
			Vector3 value;
			if (TryGetVector3(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はVector3.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3 type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns Vector3.zero.</returns>
#endif
		public Vector3 GetVector3()
		{
			return GetVector3(Vector3.zero);
		}

		#endregion //Vector3
	}
}