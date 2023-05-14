//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なlong型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible long type reference method there is more than one.
	/// </summary>
#endif

	[System.Serializable]
	public sealed class FlexibleLong : FlexiblePrimitiveBase, IValueGetter<long>
	{
		[SerializeField] private long _Value = 0;
		[SerializeField] private LongParameterReference _Parameter = new LongParameterReference();
		[SerializeField] private long _MinRange = 0;
		[SerializeField] private long _MaxRange = 0;
		[SerializeField] private InputSlotLong _Slot = new InputSlotLong();

#if ARBOR_DOC_JA
		/// <summary>
		/// Parameterを返す。TypeがParameter以外の場合はnull。
		/// </summary>
#else
		/// <summary>
		/// It return a Paramter. It is null if Type is other than Parameter.
		/// </summary>
#endif
		public Parameter parameter
		{
			get
			{
				if (_Type == FlexiblePrimitiveType.Parameter)
				{
					return _Parameter.parameter;
				}
				return null;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値を返す
		/// </summary>
#else
		/// <summary>
		/// It returns a value
		/// </summary>
#endif
		public long value
		{
			get
			{
				long value = 0;
				switch (_Type)
				{
					case FlexiblePrimitiveType.Constant:
						value = _Value;
						break;
					case FlexiblePrimitiveType.Parameter:
						value = _Parameter.value;
						break;
					case FlexiblePrimitiveType.Random:
						value = (long)((ulong)(_MaxRange - _MinRange - 1) * Random.value + _MinRange);
						break;
					case FlexiblePrimitiveType.DataSlot:
						_Slot.GetValue(ref value);
						break;
				}

				return value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// スロットを返す
		/// </summary>
#else
		/// <summary>
		/// It returns a slot
		/// </summary>
#endif
		public InputSlotLong slot
		{
			get
			{
				return _Slot;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 値をobjectで返す。
		/// </summary>
		/// <returns>値のobject</returns>
#else
		/// <summary>
		/// Return the value as object.
		/// </summary>
		/// <returns>The value object</returns>
#endif
		public override object GetValueObject()
		{
			return value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// データスロットの接続を切断する。
		/// </summary>
#else
		/// <summary>
		/// Disconnect the data slot.
		/// </summary>
#endif
		public override void Disconnect()
		{
			switch (_Type)
			{
				case FlexiblePrimitiveType.Parameter:
					_Parameter.Disconnect();
					break;
				case FlexiblePrimitiveType.DataSlot:
					_Slot.Disconnect();
					break;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleLongデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleLong default constructor
		/// </summary>
#endif
		public FlexibleLong()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleLongコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleLong constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleLong(long value)
		{
			_Type = FlexiblePrimitiveType.Constant;
			_Value = value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleLongコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleLong constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleLong(LongParameterReference parameter)
		{
			_Type = FlexiblePrimitiveType.Parameter;
			_Parameter = parameter;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleLongコンストラクタ
		/// </summary>
		/// <param name="minRange">最小範囲。</param>
		/// <param name="maxRange">最大範囲。</param>
#else
		/// <summary>
		/// FlexibleLong constructor
		/// </summary>
		/// <param name="minRange">Minimum range.</param>
		/// <param name="maxRange">Maximum range.</param>
#endif
		public FlexibleLong(long minRange, long maxRange)
		{
			_Type = FlexiblePrimitiveType.Random;
			_MinRange = minRange;
			_MaxRange = maxRange;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleLongコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleLong constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleLong(InputSlotLong slot)
		{
			_Type = FlexiblePrimitiveType.DataSlot;
			_Slot = slot;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleLongをlongにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleLong</param>
		/// <returns>longにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleLong to long.
		/// </summary>
		/// <param name="flexible">FlexiblFlexibleLongeInt</param>
		/// <returns>Returns the result of casting to long.</returns>
#endif
		public static explicit operator long(FlexibleLong flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// longをFlexibleLongにキャスト。
		/// </summary>
		/// <param name="value">long</param>
		/// <returns>FlexibleLongにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast long to FlexibleLong.
		/// </summary>
		/// <param name="value">long</param>
		/// <returns>Returns the result of casting to FlexibleLong.</returns>
#endif
		public static explicit operator FlexibleLong(long value)
		{
			return new FlexibleLong(value);
		}

		internal void SetSlot(InputSlotBase slot)
		{
			_Type = FlexiblePrimitiveType.DataSlot;
			_Slot.Copy(slot);
		}

		long IValueGetter<long>.GetValue()
		{
			return value;
		}
	}
}
