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
	/// 参照方法が複数ある柔軟なstring型を扱うクラス。
	/// </summary>
	/// <remarks>
	/// FlexibleType.Constantの時、<see cref="Arbor.ConstantMultilineAttribute"/>属性をつけることでエディタのフィールドを複数行表示できる。
	/// </remarks>
#else
	/// <summary>
	/// Class to handle a flexible string type reference method there is more than one.
	/// </summary>
	/// <remarks>
	/// When FlexibleType.Constant, you can display multiple fields of editor fields by attaching the <see cref="Arbor.ConstantMultilineAttribute"/> attribute.
	/// </remarks>
#endif
	[System.Serializable]
	public sealed class FlexibleString : IFlexibleField, IValueGetter<string>
	{
		[SerializeField] private FlexibleType _Type = FlexibleType.Constant;

		[SerializeField] private string _Value = string.Empty;

		[SerializeField] private StringParameterReference _Parameter = new StringParameterReference();

		[SerializeField] private InputSlotString _Slot = new InputSlotString();

#if ARBOR_DOC_JA
		/// <summary>
		/// Typeを返す
		/// </summary>
#else
		/// <summary>
		/// It returns a type
		/// </summary>
#endif
		public FlexibleType type
		{
			get
			{
				return _Type;
			}
		}

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
				if (_Type == FlexibleType.Parameter)
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
		public string value
		{
			get
			{
				string value = string.Empty;
				switch (_Type)
				{
					case FlexibleType.Constant:
						value = _Value;
						break;
					case FlexibleType.Parameter:
						value = _Parameter.value;
						break;
					case FlexibleType.DataSlot:
						_Slot.GetValue(ref value);
						break;
				}

				return value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleStringデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleString default constructor
		/// </summary>
#endif
		public FlexibleString()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleStringコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleString constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleString(string value)
		{
			_Type = FlexibleType.Constant;
			_Value = value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleStringコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleString constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleString(StringParameterReference parameter)
		{
			_Type = FlexibleType.Parameter;
			_Parameter = parameter;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleStringコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleString constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleString(InputSlotString slot)
		{
			_Type = FlexibleType.DataSlot;
			_Slot = slot;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleStringをstringにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleString</param>
		/// <returns>stringにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleString to string.
		/// </summary>
		/// <param name="flexible">FlexibleString</param>
		/// <returns>Returns the result of casting to string.</returns>
#endif
		public static explicit operator string(FlexibleString flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// stringをFlexibleStringにキャスト。
		/// </summary>
		/// <param name="value">string</param>
		/// <returns>FlexibleStringにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast string to FlexibleString.
		/// </summary>
		/// <param name="value">string</param>
		/// <returns>Returns the result of casting to FlexibleString.</returns>
#endif
		public static explicit operator FlexibleString(string value)
		{
			return new FlexibleString(value);
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
		public object GetValueObject()
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
		public void Disconnect()
		{
			switch (_Type)
			{
				case FlexibleType.Parameter:
					_Parameter.Disconnect();
					break;
				case FlexibleType.DataSlot:
					_Slot.Disconnect();
					break;
			}
		}

		internal void SetSlot(InputSlotBase slot)
		{
			_Type = FlexibleType.DataSlot;
			_Slot.Copy(slot);
		}

		string IValueGetter<string>.GetValue()
		{
			return value;
		}
	}
}
