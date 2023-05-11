//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.Calculators
{
#if ARBOR_DOC_JA
	/// <summary>
	/// GameObjectからComponentを出力する。
	/// </summary>
#else
	/// <summary>
	/// Output Component from GameObject.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Component/GameObject.GetComponent")]
	[BehaviourTitle("GameObject.GetComponent")]
	[BuiltInBehaviour]
	public sealed class GameObjectGetComponentCalculator : Calculator, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

		/// <summary>
		/// GameObject
		/// </summary>
		[SerializeField] private FlexibleGameObject _GameObject = new FlexibleGameObject(FlexibleHierarchyType.Self);

		/// <summary>
		/// Component
		/// </summary>
		[SerializeField]
		private OutputSlotComponent _Component = new OutputSlotComponent();

#if ARBOR_DOC_JA
		/// <summary>
		/// 常にGetComponentするかどうか。<br/>
		/// falseの場合、GameObjectが更新されるまで一度GetComponentした内容がキャッシュされます。
		/// </summary>
#else
		/// <summary>
		/// Whether to always GetComponent.<br/>
		/// If false, GetComponent contents once cached until the GameObject is updated.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _AlwaysGetComponent = new FlexibleBool(true);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_AlwaysGetComponent")]
		[HideInInspector]
		private bool _OldAlwaysGetComponent = true;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		// Use this for calculate
		public override void OnCalculate()
		{
			GameObject gameObject = _GameObject.value;
			if (gameObject != null)
			{
				_Component.SetValue(gameObject.GetComponent(_Component.dataType));
			}
		}

		public override bool OnCheckDirty()
		{
			return _AlwaysGetComponent.value;
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_AlwaysGetComponent = (FlexibleBool)_OldAlwaysGetComponent;
		}

		void Serialize()
		{
			while (_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_SerializeVersion++;
						break;
					default:
						_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			Serialize();
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();
		}
	}
}