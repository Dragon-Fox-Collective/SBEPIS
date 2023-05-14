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
		/// Vector3Int型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Vector3Int type.
		/// </summary>
#endif
		public Vector3Int vector3IntValue
		{
			get
			{
				Vector3Int value;
				if (TryGetVector3Int(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetVector3Int(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Vector3Int

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3Int型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector3Int type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetVector3Int(Vector3Int value)
		{
			if (type == Type.Vector3Int)
			{
				if (container._Vector3IntParameters[_ParameterIndex] != value)
				{
					container._Vector3IntParameters[_ParameterIndex] = value;
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3Int型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3Int type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetVector3Int(out Vector3Int value)
		{
			if (type == Type.Vector3Int)
			{
				value = container._Vector3IntParameters[_ParameterIndex];

				return true;
			}

			value = Vector3Int.zero;
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3Int型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3Int type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Vector3Int GetVector3Int(Vector3Int defaultValue)
		{
			Vector3Int value;
			if (TryGetVector3Int(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector3Int型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はVector3Int.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector3Int type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns Vector3Int.zero.</returns>
#endif
		public Vector3Int GetVector3Int()
		{
			return GetVector3Int(Vector3Int.zero);
		}

		#endregion //Vector3Int
	}
}