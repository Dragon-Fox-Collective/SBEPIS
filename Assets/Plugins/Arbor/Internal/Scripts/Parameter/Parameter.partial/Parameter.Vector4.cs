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
		/// Vector4型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Vector4 type.
		/// </summary>
#endif
		public Vector4 vector4Value
		{
			get
			{
				Vector4 value;
				if (TryGetVector4(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetVector4(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Vector4

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector4 type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetVector4(Vector4 value)
		{
			if (type == Type.Vector4)
			{
				if (container._Vector4Parameters[_ParameterIndex] != value)
				{
					container._Vector4Parameters[_ParameterIndex] = value;
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector4 type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetVector4(out Vector4 value)
		{
			if (type == Type.Vector4)
			{
				value = container._Vector4Parameters[_ParameterIndex];

				return true;
			}

			value = Vector4.zero;
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector4 type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Vector4 GetVector4(Vector4 defaultValue)
		{
			Vector4 value;
			if (TryGetVector4(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はVector4.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector4 type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns Vector4.zero.</returns>
#endif
		public Vector4 GetVector4()
		{
			return GetVector4(Vector4.zero);
		}

		#endregion //Vector4
	}
}