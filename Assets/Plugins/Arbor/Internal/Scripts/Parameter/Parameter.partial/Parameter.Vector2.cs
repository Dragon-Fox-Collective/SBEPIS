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
		/// Vector2型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Vector2 type.
		/// </summary>
#endif
		public Vector2 vector2Value
		{
			get
			{
				Vector2 value;
				if (TryGetVector2(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetVector2(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Vector2

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector2 type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetVector2(Vector2 value)
		{
			if (type == Type.Vector2)
			{
				if (container._Vector2Parameters[_ParameterIndex] != value)
				{
					container._Vector2Parameters[_ParameterIndex] = value;
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2 type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetVector2(out Vector2 value)
		{
			if (type == Type.Vector2)
			{
				value = container._Vector2Parameters[_ParameterIndex];

				return true;
			}

			value = Vector2.zero;
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2 type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public Vector2 GetVector2(Vector2 defaultValue)
		{
			Vector2 value;
			if (TryGetVector2(out value))
			{
				return value;
			}
			return defaultValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はVector2.zeroを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2 type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns Vector2.zero.</returns>
#endif
		public Vector2 GetVector2()
		{
			return GetVector2(Vector2.zero);
		}

		#endregion //Vector2
	}
}