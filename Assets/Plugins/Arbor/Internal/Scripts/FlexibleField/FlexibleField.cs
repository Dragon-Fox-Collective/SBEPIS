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
	/// 参照方法が複数ある柔軟な型を扱うための基本クラス。
	/// 使用するには<see cref="FlexibleField{T}"/>を参照してください。
	/// </summary>
#else
	/// <summary>
	/// A base class for dealing with flexible types with multiple reference methods.
	/// See <see cref="FlexibleField{T}"/> for use.
	/// </summary>
#endif
	[System.Serializable]
	public abstract class FlexibleFieldBase : IFlexibleField
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 値の指定タイプ
		/// </summary>
#else
		/// <summary>
		/// Specified type of value
		/// </summary>
#endif
		[SerializeField]
		protected FlexibleType _Type = FlexibleType.Constant;

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
		/// フィールドの型を返す。
		/// </summary>
#else
		/// <summary>
		/// It returns a field type.
		/// </summary>
#endif
		public abstract System.Type fieldType
		{
			get;
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
		public abstract object GetValueObject();

#if ARBOR_DOC_JA
		/// <summary>
		/// データスロットの接続を切断する。
		/// </summary>
#else
		/// <summary>
		/// Disconnect the data slot.
		/// </summary>
#endif
		public abstract void Disconnect();
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟な型を扱うクラス。
	/// 使用する場合は、Tにユーザー定義クラスを指定して継承してください。
	/// </summary>
	/// <typeparam name="T">シリアライズ可能な型</typeparam>
#else
	/// <summary>
	/// A base class for dealing with flexible types with multiple reference methods.
	/// To use it, inherit T by specifying a user-defined class.
	/// </summary>
	/// <typeparam name="T">Serializable type</typeparam>
#endif
	[System.Serializable]
	[Internal.DocumentManual("/manual/scripting/parameterreference/flexiblefield.md")]
	public class FlexibleField<T> : FlexibleFieldBase, IValueGetter<T>
	{
#if ARBOR_DOC_JA
		/// <summary>
		/// 固定値
		/// </summary>
#else
		/// <summary>
		/// Constant value
		/// </summary>
#endif
		[SerializeField]
		protected T _Value = default(T);

#if ARBOR_DOC_JA
		/// <summary>
		/// パラメータ参照
		/// </summary>
#else
		/// <summary>
		/// Parameter reference
		/// </summary>
#endif
		[SerializeField]
		[ClassGenericArgument(0)]
		protected AnyParameterReference _Parameter = new AnyParameterReference();

#if ARBOR_DOC_JA
		/// <summary>
		/// データ入力スロット
		/// </summary>
#else
		/// <summary>
		/// Data input slot
		/// </summary>
#endif
		[SerializeField]
		[ClassGenericArgument(0)]
		protected InputSlotAny _Slot = new InputSlotAny();

#if ARBOR_DOC_JA
		/// <summary>
		/// フィールドの型を返す。
		/// </summary>
#else
		/// <summary>
		/// It returns a field type.
		/// </summary>
#endif
		public override System.Type fieldType
		{
			get
			{
				return typeof(T);
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
		public T value
		{
			get
			{
				T value = default(T);
				switch (_Type)
				{
					case FlexibleType.Constant:
						value = _Value;
						break;
					case FlexibleType.Parameter:
						try
						{
							if (_Parameter != null)
							{
								value = _Parameter.GetValue<T>();
							}
						}
						catch (System.InvalidCastException ex)
						{
							Debug.LogException(ex);
						}
						break;
					case FlexibleType.DataSlot:
						_Slot.TryGetValue<T>(out value);
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
		/// FlexibleFieldデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleField default constructor
		/// </summary>
#endif
		public FlexibleField()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleFieldコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleField constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleField(T value)
		{
			_Type = FlexibleType.Constant;
			_Value = value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleFieldコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleField constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleField(AnyParameterReference parameter)
		{
			_Type = FlexibleType.Parameter;
			_Parameter = parameter;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleFieldコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleField constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleField(InputSlotAny slot)
		{
			_Type = FlexibleType.DataSlot;
			_Slot = slot;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleFieldをTにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleField</param>
		/// <returns>Tにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleField to T.
		/// </summary>
		/// <param name="flexible">FlexibleField</param>
		/// <returns>Returns the result of casting to T.</returns>
#endif
		public static explicit operator T(FlexibleField<T> flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// TをFlexibleFieldにキャスト。
		/// </summary>
		/// <param name="value">T</param>
		/// <returns>FlexibleField&lt;T&gt;にキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast T to FlexibleField.
		/// </summary>
		/// <param name="value">T</param>
		/// <returns>Returns the result of casting to FlexibleField&lt;T&gt;.</returns>
#endif
		public static explicit operator FlexibleField<T>(T value)
		{
			return new FlexibleField<T>(value);
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

		T IValueGetter<T>.GetValue()
		{
			return value;
		}
	}
}