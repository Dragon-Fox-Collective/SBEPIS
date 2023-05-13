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
		/// LongList型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of LongList type.
		/// </summary>
#endif
		public IList<long> longListValue
		{
			get
			{
				IList<long> value;
				if (TryGetLongList(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetLongList(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region LongList

#if ARBOR_DOC_JA
		/// <summary>
		/// LongList型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the LongList type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetLongList(IList<long> value)
		{
			if (type == Type.LongList)
			{
				if (container._LongListParameters[_ParameterIndex].SetList(value))
				{
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// LongList型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the LongList type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetLongList(out IList<long> value)
		{
			if (type == Type.LongList)
			{
				value = container._LongListParameters[_ParameterIndex].list;

				return true;
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// LongList型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the LongList type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<long> GetLongList()
		{
			IList<long> value;
			if (TryGetLongList(out value))
			{
				return value;
			}
			return null;
		}

		#endregion //LongList
	}
}