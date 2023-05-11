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
	/// 2Dのレイキャストによって遷移する。
	/// </summary>
#else
	/// <summary>
	/// It will transition by 2D of Ray cast.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/Physics2D/Raycast2DTransition")]
	[BuiltInBehaviour]
	public sealed class Raycast2DTransition : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
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
		[SerializeField] private FlexibleVector2 _Origin = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// レイの方向
		/// </summary>
#else
		/// <summary>
		/// Direction of Ray
		/// </summary>
#endif
		[SerializeField] private FlexibleVector2 _Direction = new FlexibleVector2();

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
		/// 最小の深度値
		/// </summary>
#else
		/// <summary>
		/// Minimum depth value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _MinDepth = new FlexibleFloat(-Mathf.Infinity);

#if ARBOR_DOC_JA
		/// <summary>
		/// 最大の深度値
		/// </summary>
#else
		/// <summary>
		/// Maximum depth value
		/// </summary>
#endif
		[SerializeField] private FlexibleFloat _MaxDepth = new FlexibleFloat(Mathf.Infinity);

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
		/// タグチェック
		/// </summary>
#else
		/// <summary>
		/// tag check
		/// </summary>
#endif
		[SerializeField]
		private TagChecker _TagChecker = new TagChecker();

#if ARBOR_DOC_JA
		/// <summary>
		/// レイキャストによるヒット情報を出力。
		/// </summary>
#else
		/// <summary>
		/// Output hit information by raycast.
		/// </summary>
#endif
		[SerializeField] private OutputSlotRaycastHit2D _RaycastHit2D = new OutputSlotRaycastHit2D();

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
			RaycastHit2D hit = Physics2D.Raycast(_Origin.value, _Direction.value, _Distance.value, _LayerMask.value.value, _MinDepth.value, _MaxDepth.value);
			if (hit.collider != null && _TagChecker.CheckTag(hit.collider))
			{
				_RaycastHit2D.SetValue(hit);
				Transition(_NextState);
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
