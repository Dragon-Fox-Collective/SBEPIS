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
		/// Vector4List型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Vector4List type.
		/// </summary>
#endif
		public IList<Vector4> vector4ListValue
		{
			get
			{
				IList<Vector4> value;
				if (TryGetVector4List(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetVector4List(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Vector4List

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4List型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Vector4List type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetVector4List(IList<Vector4> value)
		{
			if (type == Type.Vector4List)
			{
				if (container._Vector4ListParameters[_ParameterIndex].SetList(value))
				{
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4List型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector4List type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetVector4List(out IList<Vector4> value)
		{
			if (type == Type.Vector4List)
			{
				value = container._Vector4ListParameters[_ParameterIndex].list;

				return true;
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Vector4List型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Vector4List type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Vector4> GetVector4List()
		{
			IList<Vector4> value;
			if (TryGetVector4List(out value))
			{
				return value;
			}
			return null;
		}

		#endregion //Vector4List
	}
}