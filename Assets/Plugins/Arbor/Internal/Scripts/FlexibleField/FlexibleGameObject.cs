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
	/// 参照方法が複数ある柔軟なGameObject型を扱うクラス。
	/// </summary>
#else
	/// <summary>
	/// Class to handle a flexible GameObject type reference method there is more than one.
	/// </summary>
#endif
	[System.Serializable]
	public sealed class FlexibleGameObject : FlexibleSceneObjectBase, IValueGetter<GameObject>
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
		private GameObject _Value = default(GameObject);

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
		private GameObjectParameterReference _Parameter = new GameObjectParameterReference();

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
		private InputSlotGameObject _Slot = new InputSlotGameObject();

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
		public GameObject value
		{
			get
			{
				GameObject value = default(GameObject);
				switch (_Type)
				{
					case FlexibleSceneObjectType.Constant:
						value = _Value;
						break;
					case FlexibleSceneObjectType.Parameter:
						value = _Parameter.value;
						break;
					case FlexibleSceneObjectType.DataSlot:
						_Slot.GetValue(ref value);
						break;
					case FlexibleSceneObjectType.Hierarchy:
						value = targetGameObject;
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
				case FlexibleSceneObjectType.Parameter:
					_Parameter.Disconnect();
					break;
				case FlexibleSceneObjectType.DataSlot:
					_Slot.Disconnect();
					break;
			}
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
		/// FlexibleGameObjectデフォルトコンストラクタ
		/// </summary>
#else
		/// <summary>
		/// FlexibleGameObject default constructor
		/// </summary>
#endif
		public FlexibleGameObject() : base()
		{
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleGameObjectコンストラクタ。
		/// </summary>
		/// <param name="gameObject">GameObject</param>
#else
		/// <summary>
		/// FlexibleGameObject constructor.
		/// </summary>
		/// <param name="gameObject">GameObject</param>
#endif
		public FlexibleGameObject(GameObject gameObject)
		{
			_Type = FlexibleSceneObjectType.Constant;
			_Value = gameObject;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleGameObjectコンストラクタ。
		/// </summary>
		/// <param name="parameter">Parameter</param>
#else
		/// <summary>
		/// FlexibleGameObject constructor.
		/// </summary>
		/// <param name="parameter">Parameter</param>
#endif
		public FlexibleGameObject(GameObjectParameterReference parameter)
		{
			_Type = FlexibleSceneObjectType.Parameter;
			_Parameter = parameter;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleGameObjectコンストラクタ。
		/// </summary>
		/// <param name="slot">スロット</param>
#else
		/// <summary>
		/// FlexibleGameObject constructor.
		/// </summary>
		/// <param name="slot">Slot</param>
#endif
		public FlexibleGameObject(InputSlotGameObject slot)
		{
			_Type = FlexibleSceneObjectType.DataSlot;
			_Slot = slot;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleGameObjectコンストラクタ。
		/// </summary>
		/// <param name="hierarchyType">参照するオブジェクトのヒエラルキータイプ</param>
#else
		/// <summary>
		/// FlexibleGameObject constructor.
		/// </summary>
		/// <param name="hierarchyType">Hierarchy type of referenced object</param>
#endif
		public FlexibleGameObject(FlexibleHierarchyType hierarchyType)
		{
			_Type = FlexibleSceneObjectType.Hierarchy;
			_HierarchyType = hierarchyType;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// FlexibleGameObjectをGameObjectにキャスト。
		/// </summary>
		/// <param name="flexible">FlexibleGameObject</param>
		/// <returns>GameObjectにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast FlexibleGameObject to GameObject.
		/// </summary>
		/// <param name="flexible">FlexibleGameObject</param>
		/// <returns>Returns the result of casting to GameObject.</returns>
#endif
		public static explicit operator GameObject(FlexibleGameObject flexible)
		{
			return flexible.value;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// GameObjectをFlexibleGameObjectにキャスト。
		/// </summary>
		/// <param name="value">GameObject</param>
		/// <returns>FlexibleGameObjectにキャストした結果を返す。</returns>
#else
		/// <summary>
		/// Cast GameObject to FlexibleGameObject.
		/// </summary>
		/// <param name="value">GameObject</param>
		/// <returns>Returns the result of casting to FlexibleGameObject.</returns>
#endif
		public static explicit operator FlexibleGameObject(GameObject value)
		{
			return new FlexibleGameObject(value);
		}

		internal void SetSlot(InputSlotBase slot)
		{
			_Type = FlexibleSceneObjectType.DataSlot;
			_Slot.Copy(slot);
		}

		GameObject IValueGetter<GameObject>.GetValue()
		{
			return value;
		}
	}
}
