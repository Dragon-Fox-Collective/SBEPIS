//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	public sealed partial class Parameter
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// Float型の値。
		/// </summary>
#else
		/// <summary>
		/// Value of Float type.
		/// </summary>
#endif
		public float floatValue
		{
			get
			{
				float value;
				if (TryGetFloat(out value))
				{
					return value;
				}

				throw new ParameterTypeMismatchException();
			}
			set
			{
				if (!SetFloat(value))
				{
					throw new ParameterTypeMismatchException();
				}
			}
		}

		#region Float

#if ARBOR_DOC_JA
		/// <summary>
		/// Float型の値を設定する。
		/// </summary>
		/// <param name="value">値。</param>
		/// <returns>値を設定できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// It wants to set the value of the Float type.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <returns>Return true if the value could be set.</returns>
#endif
		public bool SetFloat(float value)
		{
			if (type == Type.Float)
			{
				if (container._FloatParameters[_ParameterIndex] != value)
				{
					container._FloatParameters[_ParameterIndex] = value;
					DoChanged();
				}
				return true;
			}

			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Float型の値を取得する。
		/// </summary>
		/// <param name="value">取得する値。</param>
		/// <returns>値を取得できた場合にtrueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Float type.
		/// </summary>
		/// <param name="value">The value you get.</param>
		///  <returns>Return true if the value can be obtained.</returns>
#endif
		public bool TryGetFloat(out float value)
		{
			if (type == Type.Float)
			{
				value = container._FloatParameters[_ParameterIndex];
				return true;
			}

			value = 0.0f;
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Float型の値を取得する。
		/// </summary>
		/// <param name="defaultValue">デフォルトの値。パラメータがない場合に返される。</param>
		/// <returns>パラメータの値。パラメータがない場合はdefaultValueを返す。</returns>
#else
		/// <summary>
		/// Get the value of the Float type.
		/// </summary>
		/// <param name="defaultValue">Default value. It is returned when there is no parameter.</param>
		/// <returns>The value of the parameter. If there is no parameter, it returns defaultValue.</returns>
#endif
		public float GetFloat(float defaultValue = default(float))
		{
			float value;
			if (TryGetFloat(out value))
			{
				return value;
			}
			return defaultValue;
		}

		#endregion //Float
	}
}