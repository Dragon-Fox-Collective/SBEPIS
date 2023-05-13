//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Collections.Generic;

namespace Arbor
{
	public sealed partial class Parameter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2List型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Vector2List type.
		/// </summary>
#endif
		public IList<Vector2> vector2ListValue
		{
			get
			{
				IList<Vector2> value;
				if (TryGetVector2List(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetVector2List(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Vector2List

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2List型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector2List type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetVector2List(IList<Vector2> value)
		{
			if (type == Type.Vector2List)
			{
				if (container._Vector2ListParameters[_ParameterIndex].SetList(value))
				{
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2List型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2List type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetVector2List(out IList<Vector2> value)
		{
			if (type == Type.Vector2List)
			{
				value = container._Vector2ListParameters[_ParameterIndex].list;

				return true;
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector2List型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector2List type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Vector2> GetVector2List()
		{
			IList<Vector2> value;
			if (TryGetVector2List(out value))
			{
				return value;
			}
			return null;
		}

		#endregion //Vector2List
	}
}