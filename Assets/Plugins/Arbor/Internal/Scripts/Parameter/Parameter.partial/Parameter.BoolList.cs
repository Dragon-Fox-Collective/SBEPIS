//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using System.Collections.Generic;

namespace Arbor
{
	public sealed partial class Parameter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// BoolList型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of BoolList type.
		/// </summary>
#endif
		public IList<bool> boolListValue
		{
			get
			{
				IList<bool> value;
				if (TryGetBoolList(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetBoolList(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region BoolList

#if ARBOR_DOC_JA
		/// <summary>
		/// BoolList型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the BoolList type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetBoolList(IList<bool> value)
		{
			if (type == Type.BoolList)
			{
				if (container._BoolListParameters[_ParameterIndex].SetList(value))
				{
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoolList型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the BoolList type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetBoolList(out IList<bool> value)
		{
			if (type == Type.BoolList)
			{
				value = container._BoolListParameters[_ParameterIndex].list;

				return true;
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// BoolList型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the BoolList type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<bool> GetBoolList()
		{
			IList<bool> value;
			if (TryGetBoolList(out value))
			{
				return value;
			}
			return null;
		}

		#endregion //BoolList
	}
}