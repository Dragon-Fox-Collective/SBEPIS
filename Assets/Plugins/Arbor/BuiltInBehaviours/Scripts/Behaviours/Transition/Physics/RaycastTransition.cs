//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// レイキャストによって遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition by Ray cast.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Physics/RaycastTransition")]
	[BuiltInBehaviour]
	public sealed class RaycastTransition : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// レイの始点
		/// </summary>
#else
		/// <summary>
		/// The starting point of the Ray
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Origin = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// レイの方向
		/// </summary>
#else
		/// <summary>
		/// Direction of Ray
		/// </summary>
#endif
		[SerializeField] private FlexibleVector3 _Direction = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// レイの距離
		/// </summary>
#else
		/// <summary>
		/// Distance of Ray
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _Distance = new FlexibleFloat(Mathf.Infinity);

#if ARBOR_DOC_JA
		/// <summary>
		/// 判定対象のレイヤー
		/// </summary>
#else
		/// <summary>
		/// Determination target layer
		/// </summary>
#endif
		[SerializeField]
		private FlexibleLayerMask _LayerMask = new FlexibleLayerMask(Physics.DefaultRaycastLayers);

#if ARBOR_DOC_JA
		/// <summary>
		/// Update時に判定するかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether or not to make an update decision.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _CheckUpdate = new FlexibleBool(false);

#if ARBOR_DOC_JA
		/// <summary>
		/// タグをチェックするかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether to check the tag.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _IsCheckTag = new FlexibleBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// チェックするタグ。
		/// </summary>
#else
		/// <summary>
		/// Tag to be checked.
		/// </summary>
#endif
		[SerializeField]
		[TagSelector]
		private FlexibleString _Tag = new FlexibleString("Untagged");

#if ARBOR_DOC_JA
		/// <summary>
		/// レイキャストによるヒット情報を出力。
		/// </summary>
#else
		/// <summary>
		/// Output hit information by raycast.
		/// </summary>
#endif
		[SerializeField] private OutputSlotRaycastHit _RaycastHit = new OutputSlotRaycastHit();

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート。<br />
		/// 遷移メソッド : OnStateBegin, OnStateUpdate
		/// </summary>
#else
		/// <summary>
		/// Transition destination state.<br />
		/// Transition Method : OnStateBegin, OnStateUpdate
		/// </summary>
#endif
		[SerializeField] private StateLink _NextState = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_CheckUpdate")]
		[HideInInspector]
		private bool _OldCheckUpdate = false;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_LayerMask")]
		private LayerMask _OldLayerMask = Physics.DefaultRaycastLayers;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		void CheckRaycast()
		{
			RaycastHit hit;
			if (Physics.Raycast(_Origin.value, _Direction.value, out hit, _Distance.value, _LayerMask.value.value))
			{
				if (!_IsCheckTag.value || hit.collider.CompareTag(_Tag.value))
				{
					_RaycastHit.SetValue(hit);
					Transition(_NextState);
				}
			}
		}

		public override void OnStateBegin()
		{
			CheckRaycast();
		}

		// Update is called once per frame
		public override void OnStateUpdate()
		{
			if (_CheckUpdate.value)
			{
				CheckRaycast();
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_CheckUpdate = (FlexibleBool)_OldCheckUpdate;
			_LayerMask = (FlexibleLayerMask)_OldLayerMask;
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
