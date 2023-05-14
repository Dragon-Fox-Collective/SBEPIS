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
		/// StringList型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of StringList type.
		/// </summary>
#endif
		public IList<string> stringListValue
		{
			get
			{
				IList<string> value;
				if (TryGetStringList(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetStringList(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region StringList

#if ARBOR_DOC_JA
		/// <summary>
		/// StringList型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the StringList type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetStringList(IList<string> value)
		{
			if (type == Type.StringList)
			{
				if (container._StringListParameters[_ParameterIndex].SetList(value))
				{
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StringList型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the StringList type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetStringList(out IList<string> value)
		{
			if (type == Type.StringList)
			{
				value = container._StringListParameters[_ParameterIndex].list;

				return true;
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// StringList型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the StringList type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<string> GetStringList()
		{
			IList<string> value;
			if (TryGetStringList(out value))
			{
				return value;
			}
			return null;
		}

		#endregion //StringList
	}
}