//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using System.Reflection;

namespace Arbor
{
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なアセットObject型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible Asset Object type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleAssetObject : IFlexibleField, IValueGetter<Object>, IAssignFieldReceiver, IOverrideConstraint
	{
		[SerializeField]
		FlexibleType _Type = FlexibleType.Constant;

		[SerializeField]
		private Object _Value = null;

		[SerializeField]
		private AssetObjectParameterReference _Parameter = new AssetObjectParameterReference();

		[SerializeField]
		[ClassAssetObject]
		private InputSlotUnityObject _Slot = new InputSlotUnityObject();

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
		public Object value
		{
			get
			{
				Object value = null;
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
		/// FlexibleAssetObjectデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleAssetObject default constructor
		/// </summary>
#endif
		public FlexibleAssetObject()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleAssetObjectコンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// FlexibleAssetObject constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleAssetObject(Object value)
		{
			_Type = FlexibleType.Constant;
			_Value = value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleAssetObjectコンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// FlexibleAssetObject constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleAssetObject(AssetObjectParameterReference parameter)
		{
			_Type = FlexibleType.Parameter;
			_Parameter = parameter;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleAssetObjectコンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleAssetObject constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleAssetObject(InputSlotUnityObject slot)
		{
			_Type = FlexibleType.DataSlot;
			_Slot = slot;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleAssetObjectをObjectにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleAssetObject</param>
		/// <returns>Objectにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleAssetObject to Object.
		/// </summary>
		/// <param name="flexible">FlexibleAssetObject</param>
		/// <returns>Returns the result of casting to Object.</returns>
#endif
		public static explicit operator Object(FlexibleAssetObject flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ObjectをFlexibleAssetObjectにキャスト。
		/// </summary>
		/// <param name="value">Object</param>
		/// <returns>FlexibleAssetObjectにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast Object to FlexibleAssetObject.
		/// </summary>
		/// <param name="value">Object</param>
		/// <returns>Returns the result of casting to FlexibleAssetObject.</returns>
#endif
		public static explicit operator FlexibleAssetObject(Object value)
		{
			return new FlexibleAssetObject(value);
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

		Object IValueGetter<Object>.GetValue()
		{
			return value;
		}

		private ClassConstraintInfo _FieldConstraintInfo;
		private ClassConstraintInfo _OverrideConstraint;

#if ARBOR_DOC_JA
		/// <summary>
		/// 上書きする型制約の情報
		/// </summary>
#else
		/// <summary>
		/// override ClassConstraintInfo
		/// </summary>
#endif
		public ClassConstraintInfo overrideConstraint
		{
			get
			{
				return _OverrideConstraint;
			}
			set
			{
				if (_OverrideConstraint != value)
				{
					_OverrideConstraint = value;

					SetInternalConstraint(GetConstraint());
				}
			}
		}

		static ClassConstraintInfo CreateFieldConstraint(FieldInfo fieldInfo)
		{
			ClassTypeConstraintAttribute constraint = AttributeHelper.GetAttribute<ClassTypeConstraintAttribute>(fieldInfo);
			if (constraint != null && typeof(Object).IsAssignableFrom(constraint.GetBaseType(fieldInfo)))
			{
				return new ClassConstraintInfo() { constraintAttribute = constraint, constraintFieldInfo = fieldInfo };
			}

			SlotTypeAttribute slotTypeAttribute = AttributeHelper.GetAttribute<SlotTypeAttribute>(fieldInfo);
			if (slotTypeAttribute != null && typeof(Object).IsAssignableFrom(slotTypeAttribute.connectableType))
			{
				return new ClassConstraintInfo() { slotTypeAttribute = slotTypeAttribute };
			}

			return null;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 型制約の情報を返す。
		/// </summary>
		/// <returns>型制約の情報</returns>
#else
		/// <summary>
		/// Return information on type constraints.
		/// </summary>
		/// <returns>Type constraint information</returns>
#endif
		public ClassConstraintInfo GetConstraint()
		{
			return _OverrideConstraint ?? _FieldConstraintInfo;
		}

		void SetInternalConstraint(ClassConstraintInfo constraint)
		{
			if (_Parameter != null)
			{
				_Parameter.overrideConstraint = constraint;
			}
			if (_Slot != null)
			{
				_Slot.overrideConstraint = constraint;
			}
		}

		void IAssignFieldReceiver.OnAssignField(Object ownerObject, FieldInfo fieldInfo)
		{
			_FieldConstraintInfo = CreateFieldConstraint(fieldInfo);
			SetInternalConstraint(GetConstraint());
		}
	}
}