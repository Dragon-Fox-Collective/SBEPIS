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
	/// Rigidbody2Dに力を加える。
	/// </summary>
#else
	/// <summary>
	/// We will apply a force to Rigidbody2D.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Physics2D/AddForceRigidbody2D")]
	[BuiltInBehaviour]
	public sealed class AddForceRigidbody2D : Rigidbody2DBehaviourBase
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
		/// 加える力。
		/// </summary>
#else
		/// <summary>
		/// Force applied.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Power = new FlexibleFloat(0f);

#if ARBOR_DOC_JA
		/// <summary>
		/// 力を適用する方法のためのオプション。<br/>詳細は<see cref="ForceMode2D"/>を参照して下さい。
		/// </summary>
#else
		/// <summary>
		/// Option for how to apply a force.<br/>See <see cref="ForceMode2D"/> for details.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleForceMode2D _ForceMode = new FlexibleForceMode2D(ForceMode2D.Force);

#if ARBOR_DOC_JA
		/// <summary>
		/// 力を適用する空間。<br/>Space.Selfを指定した場合、<see cref="Rigidbody2D.AddRelativeForce(Vector2, ForceMode2D)"/>を使用する。
		/// </summary>
#else
		/// <summary>
		/// Space to apply force.<br/>If Space.Self is specified, <see cref="Rigidbody2D.AddRelativeForce(Vector2, ForceMode2D)"/> is used.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleSpace _Space = new FlexibleSpace(Space.Self);

		[SerializeField]
		[HideInInspector]
		private int _AddForceRigidbody2D_SerializeVersion = 0;

		#region old

		[FormerlySerializedAs("_Angle")]
		[SerializeField]
		[HideInInspector]
		private float _OldAngle = 0f;

		[FormerlySerializedAs("_Power")]
		[SerializeField]
		[HideInInspector]
		private float _OldPower = 0f;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_ForceMode")]
		private ForceMode2D _OldForceMode = ForceMode2D.Force;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_Space")]
		private Space _OldSpace = Space.Self;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		protected override void Reset()
		{
			base.Reset();

			_AddForceRigidbody2D_SerializeVersion = kCurrentSerializeVersion;
		}

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


		void DoAddForce(ExecuteMethodFlags executeMethodFlags)
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

			Vector2 force = direction * _Power.value;
			switch (_Space.value)
			{
				case Space.Self:
					target.AddRelativeForce(force, _ForceMode.value);
					break;
				case Space.World:
					target.AddForce(force, _ForceMode.value);
					break;
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			DoAddForce(ExecuteMethodFlags.Enter);
		}

		public override void OnStateUpdate()
		{
			DoAddForce(ExecuteMethodFlags.Update);
		}

		public override void OnStateLateUpdate()
		{
			DoAddForce(ExecuteMethodFlags.LateUpdate);
		}

		public override void OnStateEnd()
		{
			DoAddForce(ExecuteMethodFlags.Leave);
		}

		public override void OnStateFixedUpdate()
		{
			DoAddForce(ExecuteMethodFlags.FixedUpdate);
		}

		void SerializeVer1()
		{
			_DirectionType = (FlexibleDirectionType)DirectionType.EulerAngle;

			_Angle = (FlexibleFloat)_OldAngle;
			_Power = (FlexibleFloat)_OldPower;
			_ForceMode = (FlexibleForceMode2D)_OldForceMode;
			_Space = (FlexibleSpace)_OldSpace;
		}

		void Serialize()
		{
			while (_AddForceRigidbody2D_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_AddForceRigidbody2D_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_AddForceRigidbody2D_SerializeVersion++;
						break;
					default:
						_AddForceRigidbody2D_SerializeVersion = kCurrentSerializeVersion;
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
