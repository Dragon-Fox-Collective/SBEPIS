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
		/// BoundsIntList型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of BoundsIntList type.
		/// </summary>
#endif
		public IList<BoundsInt> boundsIntListValue
		{
			get
			{
				IList<BoundsInt> value;
				if (TryGetBoundsIntList(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetBoundsIntList(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region BoundsIntList

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntList型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the BoundsIntList type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetBoundsIntList(IList<BoundsInt> value)
		{
			if (type == Type.BoundsIntList)
			{
				if (container._BoundsIntListParameters[_ParameterIndex].SetList(value))
				{
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntList型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsIntList type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetBoundsIntList(out IList<BoundsInt> value)
		{
			if (type == Type.BoundsIntList)
			{
				value = container._BoundsIntListParameters[_ParameterIndex].list;

				return true;
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoundsIntList型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the BoundsIntList type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<BoundsInt> GetBoundsIntList()
		{
			IList<BoundsInt> value;
			if (TryGetBoundsIntList(out value))
			{
				return value;
			}
			return null;
		}

		#endregion //BoundsIntList
	}
}