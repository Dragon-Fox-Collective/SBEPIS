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
	/// Rigidbodyに力を加える。
	/// </summary>
#else
	/// <summary>
	/// We will apply a force to Rigidbody.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Physics/AddForceRigidbody")]
	[BuiltInBehaviour]
	public sealed class AddForceRigidbody : RigidbodyBehaviourBase
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
		/// 力を適用する方法のためのオプション。<br/>詳細は<see cref="ForceMode"/>を参照して下さい。
		/// </summary>
#else
		/// <summary>
		/// Option for how to apply a force.<br/>See <see cref="ForceMode"/> for details.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleForceMode _ForceMode = new FlexibleForceMode(ForceMode.Force);

#if ARBOR_DOC_JA
		/// <summary>
		/// 力を適用する空間。<br/>Space.Selfを指定した場合、<see cref="Rigidbody.AddRelativeForce(Vector3, ForceMode)"/>を使用する。
		/// </summary>
#else
		/// <summary>
		/// Space to apply force.<br/>If Space.Self is specified, <see cref="Rigidbody.AddRelativeForce(Vector3, ForceMode)"/> is used.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleSpace _Space = new FlexibleSpace(Space.Self);

		[SerializeField]
		[HideInInspector]
		private int _AddForceRigidbody_SerializeVersion = 0;

		#region old

		#endregion // old

		[FormerlySerializedAs("_Angle")]
		[SerializeField]
		[HideInInspector]
		private Vector3 _OldAngle = Vector3.zero;

		[FormerlySerializedAs("_Power")]
		[SerializeField]
		[HideInInspector]
		private float _OldPower = 0f;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_ForceMode")]
		private ForceMode _OldForceMode = ForceMode.Force;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_Space")]
		private Space _OldSpace = Space.Self;

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

		void DoAddForce(ExecuteMethodFlags executeMethodFlags)
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

			Vector3 force = direction * _Power.value;
			switch (_Space.value)
			{
				case Space.World:
					target.AddForce(force, _ForceMode.value);
					break;
				case Space.Self:
					target.AddRelativeForce(force, _ForceMode.value);
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

		protected override void Reset()
		{
			base.Reset();

			_AddForceRigidbody_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_DirectionType = (FlexibleDirectionType)DirectionType.EulerAngle;

			_Direction = (FlexibleVector3)_OldAngle;
			_Power = (FlexibleFloat)_OldPower;
			_ForceMode = (FlexibleForceMode)_OldForceMode;
			_Space = (FlexibleSpace)_OldSpace;
		}

		void Serialize()
		{
			while (_AddForceRigidbody_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_AddForceRigidbody_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_AddForceRigidbody_SerializeVersion++;
						break;
					default:
						_AddForceRigidbody_SerializeVersion = kCurrentSerializeVersion;
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
