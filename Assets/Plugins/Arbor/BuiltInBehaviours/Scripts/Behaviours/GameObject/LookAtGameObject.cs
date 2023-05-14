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
	/// 指定したTransformを注視する。
	/// </summary>
#else
	/// <summary>
	/// We will watch the specified Transform.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("GameObject/LookAtGameObject")]
	[BuiltInBehaviour]
	public sealed class LookAtGameObject : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 注視するTransform。
		/// </summary>
#else
		/// <summary>
		/// The gaze to Transform.
		/// </summary>
#endif
		[SerializeField] private FlexibleTransform _Transform = new FlexibleTransform(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 注視先のTransform。
		/// </summary>
#else
		/// <summary>
		/// Gaze destination of Transform.
		/// </summary>
#endif
		[SerializeField] private FlexibleTransform _Target = new FlexibleTransform();

#if ARBOR_DOC_JA
		/// <summary>
		/// 注視先TransformのX座標を使用するかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether to use the X position of the target Transform.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _UsePositionX = new FlexibleBool(true);

#if ARBOR_DOC_JA
		/// <summary>
		/// 注視先TransformのY座標を使用するかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether to use the Y position of the target Transform.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _UsePositionY = new FlexibleBool(true);

#if ARBOR_DOC_JA
		/// <summary>
		/// 注視先TransformのZ座標を使用するかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether to use the Z position of the target Transform.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _UsePositionZ = new FlexibleBool(true);

#if ARBOR_DOC_JA
		/// <summary>
		/// LateUpdateの時に適用するかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether to apply at the time of the LateUpdate.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _ApplyLateUpdate = new FlexibleBool(false);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_UsePositionX")]
		private bool _OldUsePositionX = true;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_UsePositionY")]
		private bool _OldUsePositionY = true;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_UsePositionZ")]
		private bool _OldUsePositionZ = true;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_ApplyLateUpdate")]
		private bool _OldApplyLateUpdate = false;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 2;

		public Transform target
		{
			get
			{
				return _Target.value;
			}
		}

		public Transform cachedTransform
		{
			get
			{
				return _Transform.value;
			}
		}

		void LookAt()
		{
			Transform t = cachedTransform;
			if (t != null && target != null)
			{
				Vector3 position = target.position;
				if (!_UsePositionX.value)
				{
					position.x = t.position.x;
				}
				if (!_UsePositionY.value)
				{
					position.y = t.position.y;
				}
				if (!_UsePositionZ.value)
				{
					position.z = t.position.z;
				}
				t.LookAt(position);
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			LookAt();
		}

		[Internal.ExcludeTest]
		void LateUpdate()
		{
			using (CalculateScope.OpenScope())
			{
				if (_ApplyLateUpdate.value)
				{
					LookAt();
				}
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Transform.SetHierarchyIfConstantNull();
		}

		void SerializeVer2()
		{
			_UsePositionX = (FlexibleBool)_OldUsePositionX;
			_UsePositionY = (FlexibleBool)_OldUsePositionY;
			_UsePositionZ = (FlexibleBool)_OldUsePositionZ;
			_ApplyLateUpdate = (FlexibleBool)_OldApplyLateUpdate;
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
					case 1:
						SerializeVer2();
						_SerializeVersion++;
						break;
					default:
						_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();
		}

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			Serialize();
		}
	}
}
