//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
namespace Arbor
{
	using Arbor.Internal;
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// 出力スロットのジェネリッククラス
	/// </summary>
	/// <typeparam name="T">データの型</typeparam>
#else
	/// <summary>
	/// Generic class of the output slot
	/// </summary>
	/// <typeparam name="T">Type of data</typeparam>
#endif
	[System.Serializable]
	public class OutputSlot<T> : OutputSlotBase, IValueSetter, IValueSetter<T>
	{
		private static readonly bool s_IsValueType;

		static OutputSlot()
		{
			s_IsValueType = TypeUtility.IsValueType(typeof(T));
		}

		internal T value
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// スロットに格納されるデータの型
		/// </summary>
#else
		/// <summary>
		/// The type of data stored in the slot
		/// </summary>
#endif
		public override System.Type dataType
		{
			get
			{
				return typeof(T);
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を設定する
		/// </summary>
		/// <param name="value">設定する値</param>
#else
		/// <summary>
		/// Set the value
		/// </summary>
		/// <param name="value">The value to be set</param>
#endif
		public void SetValue(T value)
		{
			bool updated = !EqualityComparerEx<T>.Default.Equals(this.value, value);

			this.value = value;
			if (updated || !s_IsValueType)
			{
				_DirtyInternalValue = true;
			}
			Used(updated);
		}

		private bool _DirtyInternalValue = true;
		private object _InternalValue;

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を返す。
		/// </summary>
		/// <returns>値を返す。</returns>
#else
		/// <summary>
		/// Returns the value.
		/// </summary>
		/// <returns>Returns the value.</returns>
#endif
		public override object GetValue()
		{
			if (_DirtyInternalValue)
			{
#if ARBOR_PROFILER && (DEVELOPMENT_BUILD || UNITY_EDITOR)
				using (new ProfilerScope("Boxing"))
#endif
				{
					_InternalValue = this.value;
				}
				_DirtyInternalValue = false;
			}
			return _InternalValue;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値の型を返す。
		/// </summary>
		/// <returns>値の型を返す。</returns>
#else
		/// <summary>
		/// Returns the type of the value.
		/// </summary>
		/// <returns>Returns the type of the value.</returns>
#endif
		public override System.Type GetValueType()
		{
			return typeof(T);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を文字列に変換して返す。
		/// </summary>
		/// <returns>変換した文字列。</returns>
#else
		/// <summary>
		/// Converts a value to a string and returns it.
		/// </summary>
		/// <returns>Converted string.</returns>
#endif
		public override string GetValueString()
		{
			if (value == null)
			{
				return "null";
			}
			return value.ToString();
		}

		void IValueSetter.SetValueObject(object value)
		{
			if (value is T)
			{
				SetValue((T)value);
			}
		}
	}
}