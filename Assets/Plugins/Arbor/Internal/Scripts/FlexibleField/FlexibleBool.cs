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
	/// 参照方法が複数ある柔軟なbool型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible bool type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleBool : FlexiblePrimitiveBase, IValueGetter<bool>
	{
		[SerializeField] private bool _Value = false;

		[SerializeField] private BoolParameterReference _Parameter = new BoolParameterReference();

		[Range(0.0f, 1.0f), SerializeField] private float _Probability = 0f;

		[SerializeField] private InputSlotBool _Slot = new InputSlotBool();

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
		public bool value
		{
			get
			{
				bool value = false;
				switch (_Type)
				{
					case FlexiblePrimitiveType.Constant:
						value = _Value;
						break;
					case FlexiblePrimitiveType.Parameter:
						value = _Parameter.value;
						break;
					case FlexiblePrimitiveType.Random:
						value = Random.Range(0.0f, 1.0f) <= _Probability;
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
		/// FlexibleBoolのデフォルトコンストラクタ。
		/// </summary>
#else
		/// <summary>
		/// FlexibleBool default constructor.
		/// </summary>
#endif
		public FlexibleBool()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleBoolのコンストラクタ。
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleBool default constructor.
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleBool(bool value)
		{
			_Type = FlexiblePrimitiveType.Constant;
			_Value = value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleBoolのコンストラクタ。
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleBool default constructor.
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleBool(BoolParameterReference parameter)
		{
			_Type = FlexiblePrimitiveType.Parameter;
			_Parameter = parameter;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleBoolのコンストラクタ。
		/// </summary>
		/// <param name="probability">ランダムの確率</param>
#else
		/// <summary>
		/// FlexibleBool default constructor.
		/// </summary>
		/// <param name="probability">Probability</param>
#endif
		public FlexibleBool(float probability)
		{
			_Type = FlexiblePrimitiveType.Random;
			_Probability = probability;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleBoolのコンストラクタ。
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleBool default constructor.
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleBool(InputSlotBool slot)
		{
			_Type = FlexiblePrimitiveType.DataSlot;
			_Slot = slot;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleBoolをboolにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleBool</param>
		/// <returns>boolにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleBool to bool.
		/// </summary>
		/// <param name="flexible">FlexibleBool</param>
		/// <returns>Returns the result of casting to bool.</returns>
#endif
		public static explicit operator bool(FlexibleBool flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// boolをFlexibleBoolにキャスト。
		/// </summary>
		/// <param name="value">bool</param>
		/// <returns>FlexibleBoolにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast bool to FlexibleBool.
		/// </summary>
		/// <param name="value">bool</param>
		/// <returns>Returns the result of casting to FlexibleBool.</returns>
#endif
		public static explicit operator FlexibleBool(bool value)
		{
			return new FlexibleBool(value);
		}

		internal void SetSlot(InputSlotBase slot)
		{
			_Type = FlexiblePrimitiveType.DataSlot;
			_Slot.Copy(slot);
		}

		bool IValueGetter<bool>.GetValue()
		{
			return value;
		}
	}
}
