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
		/// FloatList型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of FloatList type.
		/// </summary>
#endif
		public IList<float> floatListValue
		{
			get
			{
				IList<float> value;
				if (TryGetFloatList(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetFloatList(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region FloatList

#if ARBOR_DOC_JA
		/// <summary>
		/// FloatList型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the FloatList type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetFloatList(IList<float> value)
		{
			if (type == Type.FloatList)
			{
				if (container._FloatListParameters[_ParameterIndex].SetList(value))
				{
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FloatList型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		///  <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the FloatList type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetFloatList(out IList<float> value)
		{
			if (type == Type.FloatList)
			{
				value = container._FloatListParameters[_ParameterIndex].list;

				return true;
			}

			value = null;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FloatList型の値を取得する。
		/// </summary>
		/// <returns>パラメータの値。パラメータがない場合はnullを返す。</returns>
#else
		/// <summary>
		/// Get the value of the FloatList type.
		/// </summary>
		/// <returns>The value of the parameter. If there is no parameter, it returns null.</returns>
#endif
		public IList<float> GetFloatList()
		{
			IList<float> value;
			if (TryGetFloatList(out value))
			{
				return value;
			}
			return null;
		}

		#endregion //FloatList
	}
}