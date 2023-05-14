//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor
{
	using Arbor.Extensions;
	using Arbor.ValueFlow;

#if ARBOR_DOC_JA
	/// <summary>
	/// <see cref="FlexibleComponent{T}"/>の基本クラス。<br/>
	/// PropertyDrawerへの橋渡しのために定義。
	/// </summary>
#else
	/// <summary>
	/// Base class for <see cref="FlexibleComponent{T}"/>.<br/>
	/// Defined for bridging to PropertyDrawer.
	/// </summary>
#endif
	public abstract class FlexibleComponentBase : FlexibleSceneObjectBase
	{
	}

#if ARBOR_DOC_JA
	/// <summary>
	/// 参照方法が複数ある柔軟なComponent型を扱うクラス(ジェネリック)。
	/// </summary>
	/// <typeparam name="T">Componentの型を指定</typeparam>
#else
	/// <summary>
	/// Class to handle a flexible Component type reference method there is more than one(Generic).
	/// </summary>
	/// <typeparam name="T">Specify Component Type</typeparam>
#endif
	public class FlexibleComponent<T> : FlexibleComponentBase, IValueGetter<T> where T : Component
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
		protected ComponentParameterReference _Parameter = new ComponentParameterReference();

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
		protected InputSlotComponent _Slot = new InputSlotComponent();

		private NodeGraph _CachedTargetGraph = null;
		private T _CachedComponent = null;

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
				if (_Type == FlexibleSceneObjectType.Parameter)
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
					case FlexibleSceneObjectType.Constant:
						value = _Value;
						break;
					case FlexibleSceneObjectType.Parameter:
						try
						{
							value = _Parameter.value as T;
						}
						catch (System.InvalidCastException ex)
						{
							Debug.LogException(ex);
						}
						break;
					case FlexibleSceneObjectType.DataSlot:
						value = _Slot.GetValue<T>();
						break;
					case FlexibleSceneObjectType.Hierarchy:
						{
							if (_CachedTargetGraph == null || _CachedComponent == null)
							{
								_CachedComponent = null;
								_CachedTargetGraph = this.targetGraph;
								if (_CachedTargetGraph != null)
								{
									_CachedComponent = _CachedTargetGraph as T;
									if (_CachedComponent == null)
									{
										_CachedComponent = _CachedTargetGraph.parameterContainer as T;
									}
									if (_CachedComponent == null)
									{
										_CachedTargetGraph.TryGetComponent<T>(out _CachedComponent);
									}
								}
							}
							value = _CachedComponent;
						}
						break;
				}

				return value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="FlexibleComponent{T}"/>デフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// <see cref="FlexibleComponent{T}"/> default constructor
		/// </summary>
#endif
		public FlexibleComponent()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="FlexibleComponent{T}"/>コンストラクタ
		/// </summary>
		/// <param name="value">値</param>
#else
		/// <summary>
		/// <see cref="FlexibleComponent{T}"/> constructor
		/// </summary>
		/// <param name="value">Value</param>
#endif
		public FlexibleComponent(T value)
		{
			_Type = FlexibleSceneObjectType.Constant;
			_Value = value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="FlexibleComponent{T}"/>コンストラクタ
		/// </summary>
		/// <param name="parameter">パラメータ</param>
#else
		/// <summary>
		/// <see cref="FlexibleComponent{T}"/> constructor
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleComponent(ComponentParameterReference parameter)
		{
			_Type = FlexibleSceneObjectType.Parameter;
			_Parameter = parameter;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="FlexibleComponent{T}"/>コンストラクタ。
		/// </summary>
		/// <param name="hierarchyType">参照するオブジェクトのヒエラルキータイプ</param>
#else
		/// <summary>
		/// <see cref="FlexibleComponent{T}"/> constructor.
		/// </summary>
		/// <param name="hierarchyType">Hierarchy type of referenced object</param>
#endif
		public FlexibleComponent(FlexibleHierarchyType hierarchyType)
		{
			_Type = FlexibleSceneObjectType.Hierarchy;
			_HierarchyType = hierarchyType;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="FlexibleComponent{T}"/>コンストラクタ
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// <see cref="FlexibleComponent{T}"/> constructor
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleComponent(InputSlotComponent slot)
		{
			_Type = FlexibleSceneObjectType.DataSlot;
			_Slot = slot;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="FlexibleComponent{T}"/>をTにキャスト。
		/// </summary>
		/// <param name="flexible"><see cref="FlexibleComponent{T}"/></param>
		/// <returns>Tにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast <see cref="FlexibleComponent{T}"/> to T.
		/// </summary>
		/// <param name="flexible"><see cref="FlexibleComponent{T}"/></param>
		/// <returns>Returns the result of casting to T.</returns>
#endif
		public static explicit operator T(FlexibleComponent<T> flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Tを<see cref="FlexibleComponent{T}"/>にキャスト。
		/// </summary>
		/// <param name="value">T</param>
		/// <returns>FlexibleComponent&lt;T&gt;にキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast T to <see cref="FlexibleComponent{T}"/>.
		/// </summary>
		/// <param name="value">T</param>
		/// <returns>Returns the result of casting to FlexibleComponent&lt;T&gt;.</returns>
#endif
		public static explicit operator FlexibleComponent<T>(T value)
		{
			return new FlexibleComponent<T>(value);
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
		/// FlexibleSceneObjectType.ConstantのObjectを返す。
		/// </summary>
		/// <returns>Constantの時のObject値</returns>
#else
		/// <summary>
		/// Returns an Object of FlexibleSceneObjectType.Constant.
		/// </summary>
		/// <returns>Object value at Constant</returns>
#endif
		public override Object GetConstantObject()
		{
			return _Value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleComponentへ変換して返す。
		/// </summary>
		/// <returns>FlexibleComponent</returns>
#else
		/// <summary>
		/// Convert to FlexibleComponent and return it.
		/// </summary>
		/// <returns>FlexibleComponent</returns>
#endif
		public FlexibleComponent ToFlexibleComponent()
		{
			switch (_Type)
			{
				case FlexibleSceneObjectType.Constant:
					return new FlexibleComponent(_Value);
				case FlexibleSceneObjectType.Parameter:
					ComponentParameterReference paremeterReference = new ComponentParameterReference();
					paremeterReference.Copy(_Parameter);
					return new FlexibleComponent(paremeterReference);
				case FlexibleSceneObjectType.DataSlot:
					InputSlotComponent slot = new InputSlotComponent();
					slot.Copy(_Slot);
					return new FlexibleComponent(slot);
				case FlexibleSceneObjectType.Hierarchy:
					return new FlexibleComponent(_HierarchyType);
			}

			return null;
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
				case FlexibleSceneObjectType.Parameter:
					_Parameter.Disconnect();
					break;
				case FlexibleSceneObjectType.DataSlot:
					_Slot.Disconnect();
					break;
			}
		}

		T IValueGetter<T>.GetValue()
		{
			return value;
		}
	}
}