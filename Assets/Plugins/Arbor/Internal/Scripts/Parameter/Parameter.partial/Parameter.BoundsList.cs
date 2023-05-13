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
		/// BoundsList型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of BoundsList type.
		/// </summary>
#endif
		public IList<Bounds> boundsListValue
		{
			get
			{
				IList<Bounds> value;
				if (TryGetBoundsList(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetBoundsList(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region BoundsList

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsList型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the BoundsList type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetBoundsList(IList<Bounds> value)
		{
			if (type == Type.BoundsList)
			{
				if (container._BoundsListParameters[_ParameterIndex].SetList(value))
				{
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsList型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsList type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetBoundsList(out IList<Bounds> value)
		{
			if (type == Type.BoundsList)
			{
				value = container._BoundsListParameters[_ParameterIndex].list;

				return true;
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsList型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsList type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<Bounds> GetBoundsList()
		{
			IList<Bounds> value;
			if (TryGetBoundsList(out value))
			{
				return value;
			}
			return null;
		}

		#endregion //BoundsList
	}
}