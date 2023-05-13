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
		/// Vector2Int型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Vector2Int type.
		/// </summary>
#endif
		public Vector2Int vector2IntValue
		{
			get
			{
				Vector2Int value;
				if (TryGetVector2Int(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetVector2Int(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Vector2Int

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2Int型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector2Int type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetVector2Int(Vector2Int value)
		{
			if (type == Type.Vector2Int)
			{
				if (container._Vector2IntParameters[_ParameterIndex] != value)
				{
					container._Vector2IntParameters[_ParameterIndex] = value;
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2Int型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2Int type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetVector2Int(out Vector2Int value)
		{
			if (type == Type.Vector2Int)
			{
				value = container._Vector2IntParameters[_ParameterIndex];

				return true;
			}

			value = Vector2Int.zero;
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2Int型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2Int type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Vector2Int GetVector2Int(Vector2Int defaultValue)
		{
			Vector2Int value;
			if (TryGetVector2Int(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2Int型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はVector2Int.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2Int type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns Vector2Int.zero.</returns>
#endif
		public Vector2Int GetVector2Int()
		{
			return GetVector2Int(Vector2Int.zero);
		}

		#endregion //Vector2Int
	}
}