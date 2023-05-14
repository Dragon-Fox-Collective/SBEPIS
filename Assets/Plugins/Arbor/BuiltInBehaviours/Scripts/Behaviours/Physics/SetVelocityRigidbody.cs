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
	/// Rigidbodyの速度を設定する。
	/// </summary>
#else
	/// <summary>
	/// Set the velocity of Rigidbody.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Physics/SetVelocityRigidbody")]
	[BuiltInBehaviour]
	public sealed class SetVelocityRigidbody : RigidbodyBehaviourBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 実行するメソッド
		/// </summary>
#else
		/// <summary>
		/// The method to execute
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(ExecuteMethodFlags))]
		private FlexibleExecuteMethodFlags _ExecuteMethodFlags = new FlexibleExecuteMethodFlags(ExecuteMethodFlags.Enter);

#if ARBOR_DOC_JA
		/// <summary>
		/// 方向タイプ
		/// </summary>
#else
		/// <summary>
		/// Direction type
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(DirectionType))]
		private FlexibleDirectionType _DirectionType = new FlexibleDirectionType(DirectionType.Vector);

#if ARBOR_DOC_JA
		/// <summary>
		/// 方向。DirectionTypeがEulerAngleの場合はオイラー角で指定。
		/// </summary>
#else
		/// <summary>
		/// Direction. If DirectionType is EulerAngle, specify Euler angle.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _Direction = new FlexibleVector3(Vector3.zero);

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動する速さ。
		/// </summary>
#else
		/// <summary>
		/// Speed to move
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Speed = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// 速度を設定する空間。
		/// </summary>
#else
		/// <summary>
		/// Space to set velocity.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleSpace _Space = new FlexibleSpace(Space.Self);

		[SerializeField]
		[HideInInspector]
		private int _SetVelocityRigidbody_SerializeVersion = 0;

		#region old

		[FormerlySerializedAs("_Angle")]
		[SerializeField]
		[HideInInspector]
		private Vector3 _OldAngle = Vector3.zero;

		[FormerlySerializedAs("_Speed")]
		[SerializeField]
		[HideInInspector]
		private float _OldSpeed = 0f;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_Space")]
		private Space _OldSpace = Space.Self;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		private Vector3 direction
		{
			get
			{
				Vector3 direction = _Direction.value;

				switch (_DirectionType.value)
				{
					case DirectionType.EulerAngle:
						return Quaternion.Euler(direction) * Vector3.forward;
					case DirectionType.Vector:
						return direction.normalized;
				}

				return Vector3.forward;
			}
		}

		void DoSetVelocity(ExecuteMethodFlags executeMethodFlags)
		{
			if ((_ExecuteMethodFlags.value & executeMethodFlags) != executeMethodFlags)
			{
				return;
			}

			Rigidbody target = cachedTarget;
			if (target == null)
			{
				return;
			}

			Vector3 direction = this.direction;

			switch (_Space.value)
			{
				case Space.Self:
					direction = target.transform.rotation * direction;
					break;
			}

			target.velocity = direction.normalized * _Speed.value;
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			DoSetVelocity(ExecuteMethodFlags.Enter);
		}

		public override void OnStateUpdate()
		{
			DoSetVelocity(ExecuteMethodFlags.Update);
		}

		public override void OnStateLateUpdate()
		{
			DoSetVelocity(ExecuteMethodFlags.LateUpdate);
		}

		public override void OnStateEnd()
		{
			DoSetVelocity(ExecuteMethodFlags.Leave);
		}

		public override void OnStateFixedUpdate()
		{
			DoSetVelocity(ExecuteMethodFlags.FixedUpdate);
		}

		protected override void Reset()
		{
			base.Reset();

			_SetVelocityRigidbody_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_DirectionType = new FlexibleDirectionType(DirectionType.EulerAngle);
			_Direction = (FlexibleVector3)_OldAngle;
			_Speed = (FlexibleFloat)_OldSpeed;
			_Space = (FlexibleSpace)_OldSpace;
		}

		void Serialize()
		{
			while (_SetVelocityRigidbody_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_SetVelocityRigidbody_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_SetVelocityRigidbody_SerializeVersion++;
						break;
					default:
						_SetVelocityRigidbody_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		public override void OnBeforeSerialize()
		{
			Serialize();
		}

		public override void OnAfterDeserialize()
		{
			Serialize();
		}
	}
}
