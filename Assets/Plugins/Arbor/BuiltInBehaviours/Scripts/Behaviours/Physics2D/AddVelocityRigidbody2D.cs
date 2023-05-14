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
	/// Rigidbody2Dの速度を加算する。
	/// </summary>
#else
	/// <summary>
	/// It will add the velocity of Rigidbody2D.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Physics2D/AddVelocityRigidbody2D")]
	[BuiltInBehaviour]
	public sealed class AddVelocityRigidbody2D : Rigidbody2DBehaviourBase
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
		/// 上方向を基準とした角度。
		/// </summary>
#else
		/// <summary>
		/// Angle with reference to the upward direction.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Angle = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// 方向。
		/// </summary>
#else
		/// <summary>
		/// Direction.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector2 _Direction = new FlexibleVector2(Vector2.zero);

#if ARBOR_DOC_JA
		/// <summary>
		/// 加える速さ。
		/// </summary>
#else
		/// <summary>
		/// Speed added.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Speed = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// 速度を加える空間。
		/// </summary>
#else
		/// <summary>
		/// Space to add velocity.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleSpace _Space = new FlexibleSpace(Space.Self);

		[SerializeField]
		[HideInInspector]
		private int _AddVelocityRigidbody2D_SerializeVersion = 0;

		#region old

		[FormerlySerializedAs("_Angle")]
		[SerializeField]
		[HideInInspector]
		private float _OldAngle = 0f;

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

		private Vector2 direction
		{
			get
			{
				switch (_DirectionType.value)
				{
					case DirectionType.EulerAngle:
						return Quaternion.Euler(0.0f, 0.0f, _Angle.value) * Vector2.up;
					case DirectionType.Vector:
						return _Direction.value.normalized;
				}

				return Vector2.up;
			}
		}

		void DoAddVelocity(ExecuteMethodFlags executeMethodFlags)
		{
			if ((_ExecuteMethodFlags.value & executeMethodFlags) != executeMethodFlags)
			{
				return;
			}

			Rigidbody2D target = cachedTarget;
			if (target == null)
			{
				return;
			}

			Vector2 direction = this.direction;

			switch (_Space.value)
			{
				case Space.Self:
					direction = target.transform.rotation * direction;
					break;
			}
			target.velocity += direction.normalized * _Speed.value;
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			DoAddVelocity(ExecuteMethodFlags.Enter);
		}

		public override void OnStateUpdate()
		{
			DoAddVelocity(ExecuteMethodFlags.Update);
		}

		public override void OnStateLateUpdate()
		{
			DoAddVelocity(ExecuteMethodFlags.LateUpdate);
		}

		public override void OnStateEnd()
		{
			DoAddVelocity(ExecuteMethodFlags.Leave);
		}

		public override void OnStateFixedUpdate()
		{
			DoAddVelocity(ExecuteMethodFlags.FixedUpdate);
		}

		protected override void Reset()
		{
			base.Reset();

			_AddVelocityRigidbody2D_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_DirectionType = (FlexibleDirectionType)DirectionType.EulerAngle;

			_Angle = (FlexibleFloat)_OldAngle;
			_Speed = (FlexibleFloat)_OldSpeed;
			_Space = (FlexibleSpace)_OldSpace;
		}

		void Serialize()
		{
			while (_AddVelocityRigidbody2D_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_AddVelocityRigidbody2D_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_AddVelocityRigidbody2D_SerializeVersion++;
						break;
					default:
						_AddVelocityRigidbody2D_SerializeVersion = kCurrentSerializeVersion;
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
