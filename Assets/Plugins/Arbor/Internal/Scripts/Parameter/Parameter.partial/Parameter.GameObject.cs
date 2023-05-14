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
		/// GameObject型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of GameObject type.
		/// </summary>
#endif
		public GameObject gameObjectValue
		{
			get
			{
				GameObject value;
				if (TryGetGameObject(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetGameObject(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region GameObject

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObject型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the GameObject type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetGameObject(GameObject value)
		{
			if (type == Type.GameObject)
			{
				Internal_SetObject(value);
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObject型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the GameObject type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetGameObject(out GameObject value)
		{
			if (type == Type.GameObject)
			{
				value = Internal_GetObject() as GameObject;

				return true;
			}

			value = null;
			return false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// GameObject型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the GameObject type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public GameObject GetGameObject(GameObject defaultValue = null)
		{
			GameObject value;
			if (TryGetGameObject(out value))
			{
				return value;
			}
			return defaultValue;
		}

		#endregion //GameObject
	}
}