//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.AI;

namespace Arbor
{
	using Arbor.Extensions;
	using Arbor.Utilities;

#if ARBOR_DOC_JA
	/// <summary>
	/// <see cref="UnityEngine.AI.NavMeshAgent"/>をラップしたAI用移動コンポーネント。<br />
	/// 主に組み込みBehaviourのAgentを介して使用する。
	/// </summary>
#else
	/// <summary>
	/// AI for the movement component that wraps the <see cref="UnityEngine.AI.NavMeshAgent"/>.<br />
	/// Used mainly through built-in Behavior's Agent.
	/// </summary>
#endif
	[AddComponentMenu("Arbor/Navigation/AgentController")]
	[BuiltInComponent]
	[HelpURL(ArborReferenceUtility.componentUrl + "Arbor/Navigation/agentcontroller.html")]
	[Internal.DocumentManual("/manual/builtin/agentcontroller/_index.md")]
	public sealed class AgentController : MovingEntity, ISerializationCallbackReceiver
	{
		enum Status
		{
			Stopping,
			Moving,
			Rotating,
			RotatingOffMeshLink,
			WatingStartOffMeshLink,
			TraversingOffMeshLink,
			WatingEndOffMeshLink,
		}

		enum MovingMode
		{
			Follow,
			Escape,
			Hide,
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AnimatorのSpeedパラメータに受け渡す値のタイプ。
		/// </summary>
#else
		/// <summary>
		/// The type of value to pass to the animator's Speed parameter.
		/// </summary>
#endif
		[Internal.Documentable]
		public enum SpeedType
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// <see cref="NavMeshAgent.velocity"/>のmagnitudeをそのまま設定する。
			/// </summary>
#else
			/// <summary>
			/// Set the magnitude of <see cref="NavMeshAgent.velocity"/> as it is.
			/// </summary>
#endif
			NotChange,

#if ARBOR_DOC_JA
			/// <summary>
			/// <see cref="NavMeshAgent.velocity"/>のmagnitudeを<see cref="NavMeshAgent.speed"/>で除算する
			/// </summary>
#else
			/// <summary>
			/// Divide the magnitude of <see cref="NavMeshAgent.velocity"/> by <see cref="NavMeshAgent.speed"/>
			/// </summary>
#endif
			DivSpeed,

#if ARBOR_DOC_JA
			/// <summary>
			/// <see cref="NavMeshAgent.velocity"/>のmagnitudeを指定した値で除算する
			/// </summary>
#else
			/// <summary>
			/// Divide the magnitude of <see cref="NavMeshAgent.velocity"/> by the specified value
			/// </summary>
#endif
			DivValue,
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動ベクトルのタイプ
		/// </summary>
#else
		/// <summary>
		/// Type of movement vector
		/// </summary>
#endif
		[Internal.Documentable]
		public enum MovementType
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// ローカル座標系の<see cref="NavMeshAgent.velocity"/>の値をそのまま使用する。
			/// </summary>
#else
			/// <summary>
			/// Use the value of <see cref="NavMeshAgent.velocity"/> in the local coordinate system as it is.
			/// </summary>
#endif
			NotChange,

#if ARBOR_DOC_JA
			/// <summary>
			/// ローカル座標系の<see cref="NavMeshAgent.velocity"/>を正規化した値を使用する。
			/// </summary>
#else
			/// <summary>
			/// Use the normalized value of <see cref="NavMeshAgent.velocity"/> in the local coordinate system.
			/// </summary>
#endif
			Normalize,

#if ARBOR_DOC_JA
			/// <summary>
			/// ローカル座標系の<see cref="NavMeshAgent.velocity"/>を<see cref="NavMeshAgent.speed"/>で割った値を使用する。
			/// </summary>
#else
			/// <summary>
			/// Use the value of <see cref="NavMeshAgent.velocity"/> in the local coordinate system divided by <see cref="NavMeshAgent.speed"/>.
			/// </summary>
#endif
			DivSpeed,

#if ARBOR_DOC_JA
			/// <summary>
			/// ローカル座標系の<see cref="NavMeshAgent.velocity"/>をMovementDivValueで割った値を使用する。
			/// </summary>
#else
			/// <summary>
			/// Use the value of <see cref="NavMeshAgent.velocity"/> in the local coordinate system divided by MovementDivValue.
			/// </summary>
#endif
			DivValue,
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Turnのタイプ
		/// </summary>
#else
		/// <summary>
		/// Type of Turn
		/// </summary>
#endif
		[Internal.Documentable]
		public enum TurnType
		{
#if ARBOR_DOC_JA
			/// <summary>
			/// 正規化したローカル座標系の<see cref="NavMeshAgent.velocity"/>のX値を使う。
			/// </summary>
#else
			/// <summary>
			/// Use the X value of <see cref="NavMeshAgent.velocity"/> in the normalized local coordinate system.
			/// </summary>
#endif
			UseX,

#if ARBOR_DOC_JA
			/// <summary>
			/// 正規化したローカル座標系の<see cref="NavMeshAgent.velocity"/>のXZ値からラジアン角を計算する。
			/// </summary>
#else
			/// <summary>
			/// Calculate the radian angle from the XZ value of <see cref="NavMeshAgent.velocity"/> in the normalized local coordinate system.
			/// </summary>
#endif
			RadianAngle,
		}

		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 制御したい<see cref="NavMeshAgent"/>。
		/// </summary>
#else
		/// <summary>
		/// <see cref="NavMeshAgent"/> you want to control.
		/// </summary>
#endif
		[SerializeField] private NavMeshAgent _Agent;

#if ARBOR_DOC_JA
		/// <summary>
		/// 制御したいAnimator。
		/// </summary>
#else
		/// <summary>
		/// Animator you want to control.
		/// </summary>
#endif
		[SerializeField] private Animator _Animator;

#if ARBOR_DOC_JA
		/// <summary>
		/// Agentが移動中かどうかをAnimatorへ設定するためのboolパラメータを指定する。
		/// </summary>
#else
		/// <summary>
		/// Specify the bool parameter for setting to the Animator whether or not the Agent is moving.
		/// </summary>
#endif
		[SerializeField]
		[AnimatorParameterName(AnimatorControllerParameterType.Bool)]
		private AnimatorName _MovingParameter = new AnimatorName();

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動中と判定する速さの閾値
		/// </summary>
#else
		/// <summary>
		/// Threshold value of the speed of moving
		/// </summary>
#endif
		[SerializeField] private float _MovingSpeedThreshold = 0f;

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動する速さをAnimatorへ設定するためのfloatパラメータを指定する。
		/// </summary>
#else
		/// <summary>
		/// Specify the float parameter to set the moving speed to Animator.
		/// </summary>
#endif
		[SerializeField]
		[AnimatorParameterName(AnimatorControllerParameterType.Float)]
		private AnimatorName _SpeedParameter = new AnimatorName();

#if ARBOR_DOC_JA
		/// <summary>
		/// AnimatorのSpeedパラメータに受け渡す値のタイプ。
		/// </summary>
#else
		/// <summary>
		/// The type of value to pass to the animator's Speed parameter.
		/// </summary>
#endif
		[SerializeField]
		[FormerlySerializedAs("_IsDivAgentSpeed")]
		private SpeedType _SpeedType = SpeedType.NotChange;

#if ARBOR_DOC_JA
		/// <summary>
		/// Speedに対して割る値。(_SpeedTypeがSpeedType.DivValueの時のみ使用)
		/// </summary>
		/// <remarks>0を指定した場合は無効。</remarks>
#else
		/// <summary>
		/// The value to divide for Speed. (Used only when _SpeedType is SpeedType.DivValue)
		/// </summary>
		/// <remarks>Disable If you specify 0.</remarks>
#endif
		[SerializeField]
		private float _SpeedDivValue = 1.0f;

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動する速さのダンプ時間。
		/// </summary>
#else
		/// <summary>
		/// Dump time of moving speed.
		/// </summary>
#endif
		[SerializeField] private float _SpeedDampTime = 0.0f;

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動ベクトルのタイプ。
		/// </summary>
#else
		/// <summary>
		/// Type of movement vector.
		/// </summary>
#endif
		[SerializeField]
		private MovementType _MovementType = MovementType.Normalize;

#if ARBOR_DOC_JA
		/// <summary>
		/// velocityに対して割る値。(_MovementTypeがMovementType.DivValueの時のみ使用)
		/// </summary>
		/// <remarks>0を指定した場合は無効。</remarks>
#else
		/// <summary>
		/// The value to divide for velocity. (Used only when _MovementType is MovementType.DivValue)
		/// </summary>
		/// <remarks>Disable If you specify 0.</remarks>
#endif
		[SerializeField]
		private float _MovementDivValue = 1.0f;

#if ARBOR_DOC_JA
		/// <summary>
		/// Agentのローカル空間での移動ベクトルのX値をAnimatorへ設定ためのfloatパラメータを指定する。
		/// </summary>
#else
		/// <summary>
		/// Specify the float parameter for setting the X value of the moving vector in the Agent's local space to Animator.
		/// </summary>
#endif
		[SerializeField]
		[AnimatorParameterName(AnimatorControllerParameterType.Float)]
		private AnimatorName _MovementXParameter = new AnimatorName();

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動ベクトルのX値のダンプ時間。
		/// </summary>
#else
		/// <summary>
		/// Dump time of X value of moving vector.
		/// </summary>
#endif
		[SerializeField] private float _MovementXDampTime = 0.0f;

#if ARBOR_DOC_JA
		/// <summary>
		/// Agentのローカル空間での移動ベクトルのY値をAnimatorへ設定ためのfloatパラメータを指定する。
		/// </summary>
#else
		/// <summary>
		/// Specify the float parameter for setting the Y value of the moving vector in the Agent's local space to Animator.
		/// </summary>
#endif
		[SerializeField]
		[AnimatorParameterName(AnimatorControllerParameterType.Float)]
		private AnimatorName _MovementYParameter = new AnimatorName();

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動ベクトルのY値のダンプ時間。
		/// </summary>
#else
		/// <summary>
		/// Dump time of Y value of moving vector.
		/// </summary>
#endif
		[SerializeField] private float _MovementYDampTime = 0.0f;

#if ARBOR_DOC_JA
		/// <summary>
		/// Agentのローカル空間での移動ベクトルのZ値をAnimatorへ設定ためのfloatパラメータを指定する。
		/// </summary>
#else
		/// <summary>
		/// Specify the float parameter for setting the Z value of the moving vector in the Agent's local space to Animator.
		/// </summary>
#endif
		[SerializeField]
		[AnimatorParameterName(AnimatorControllerParameterType.Float)]
		private AnimatorName _MovementZParameter = new AnimatorName();

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動ベクトルのZ値のダンプ時間。
		/// </summary>
#else
		/// <summary>
		/// Dump time of Z value of moving vector.
		/// </summary>
#endif
		[SerializeField] private float _MovementZDampTime = 0.0f;

#if ARBOR_DOC_JA
		/// <summary>
		/// ターン方向をAnimatorへ設定するためのfloatパラメータを指定する。
		/// </summary>
#else
		/// <summary>
		/// Specify the float parameter for setting the turn direction to Animator.
		/// </summary>
#endif
		[SerializeField]
		[AnimatorParameterName(AnimatorControllerParameterType.Float)]
		private AnimatorName _TurnParameter = new AnimatorName();

#if ARBOR_DOC_JA
		/// <summary>
		/// Turnのタイプ
		/// </summary>
#else
		/// <summary>
		/// Type of Turn.
		/// </summary>
#endif
		[SerializeField]
		private TurnType _TurnType = TurnType.RadianAngle;

#if ARBOR_DOC_JA
		/// <summary>
		/// ターン方向のダンプ時間。
		/// </summary>
#else
		/// <summary>
		/// Dump time in the turn direction.
		/// </summary>
#endif
		[SerializeField] private float _TurnDampTime = 0.0f;

#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="OffMeshLink"/>を通過する処理を有効にする<br/>有効にする場合、<see cref="NavMeshAgent.autoTraverseOffMeshLink"/>がfalseでなければならない。
		/// </summary>
#else
		/// <summary>
		/// Enable processing to pass <see cref="OffMeshLink"/><br/>If enabled, <see cref="NavMeshAgent.autoTraverseOffMeshLink"/> must be false.
		/// </summary>		
#endif
		[SerializeField]
		bool _EnableTraverseOffMeshLink = true;

#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="OffMeshLink"/>をジャンプして飛び越える場合の通過方法を指定する。<br/><see cref="OffMeshLink"/>がOffMeshLinkType.LinkTypeJumpAcrossかOffMeshLinkType.LinkTypeManual(OffMeshLinkSettingsがない場合)だった場合に使用される。
		/// </summary>
#else
		/// <summary>
		/// Specify the traverse method when jumping over <see cref="OffMeshLink"/>.<br/>Used when <see cref="OffMeshLink"/> is OffMeshLinkType.LinkTypeJumpAcross or OffMeshLinkType.LinkTypeManual(without OffMeshLinkSettings).
		/// </summary>
#endif
		[SerializeField]
		OffMeshLinkTraverseData _JumpAcross = new OffMeshLinkTraverseData();

#if ARBOR_DOC_JA
		/// <summary>
		/// <see cref="OffMeshLink"/>を飛び降りる場合の通過方法を指定する。<br/>OffMeshLinkがOffMeshLinkType.LinkTypeDropDownだった場合に使用される。
		/// </summary>
#else
		/// <summary>
		/// Off Specifies the traverse method when jumping off MeshLink.<br/>Used when <see cref="OffMeshLink"/> is OffMeshLinkType.LinkTypeDropDown.
		/// </summary>
#endif
		[SerializeField]
		OffMeshLinkTraverseData _DropDown = new OffMeshLinkTraverseData();

		[HideInInspector]
		[SerializeField]
		private int _SerializeVersion = 0;

		#region old

		// version 0
		[FormerlySerializedAs("_SpeedParameter")]
		[HideInInspector]
		[SerializeField]
		private AnimatorFloatParameterReference _SpeedParameterOld = new AnimatorFloatParameterReference();

		// version 1
		[SerializeField, HideInInspector, FormerlySerializedAs("_MovingParameter")]
		private string _OldMovingParameterVer1 = string.Empty;

		[SerializeField, HideInInspector, FormerlySerializedAs("_SpeedParameter")]
		private string _OldSpeedParameterVer1 = string.Empty;

		[SerializeField, HideInInspector, FormerlySerializedAs("_MovementXParameter")]
		private string _OldMovementXParameterVer1 = string.Empty;

		[SerializeField, HideInInspector, FormerlySerializedAs("_MovementYParameter")]
		private string _OldMovementYParameterVer1 = string.Empty;

		[SerializeField, HideInInspector, FormerlySerializedAs("_MovementZParameter")] 
		private string _OldMovementZParameterVer1 = string.Empty;

		[SerializeField, HideInInspector, FormerlySerializedAs("_TurnParameter")]
		private string _OldTurnParameterVer1 = string.Empty;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 2;

#if ARBOR_DOC_JA
		/// <summary>
		/// 制御したい<see cref="NavMeshAgent"/>。
		/// </summary>
#else
		/// <summary>
		/// <see cref="NavMeshAgent"/> you want to control.
		/// </summary>
#endif
		public NavMeshAgent agent
		{
			get
			{
				return _Agent;
			}
			set
			{
				_Agent = value;
				if (_Agent != null)
				{
					agentTransform = _Agent.transform;
				}
				else
				{
					agentTransform = null;
				}
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 制御したいAnimator。
		/// </summary>
#else
		/// <summary>
		/// Animator you want to control.
		/// </summary>
#endif
		public Animator animator
		{
			get
			{
				return _Animator;
			}
			set
			{
				_Animator = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Agentが移動中かどうかをAnimatorへ設定するためのboolパラメータを指定する。
		/// </summary>
#else
		/// <summary>
		/// Specify the bool parameter for setting to the Animator whether or not the Agent is moving.
		/// </summary>
#endif
		public string movingParameter
		{
			get
			{
				return _MovingParameter.name;
			}
			set
			{
				_MovingParameter.name = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動中と判定する速さの閾値
		/// </summary>
#else
		/// <summary>
		/// Threshold for the speed at which it is determined to be moving
		/// </summary>
#endif
		public float movingSpeedThreshold
		{
			get
			{
				return _MovingSpeedThreshold;
			}
			set
			{
				_MovingSpeedThreshold = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動する速さをAnimatorへ設定するためのfloatパラメータを指定する。
		/// </summary>
#else
		/// <summary>
		/// Specify the float parameter to set the moving speed to Animator.
		/// </summary>
#endif
		public string speedParameter
		{
			get
			{
				return _SpeedParameter.name;
			}
			set
			{
				_SpeedParameter.name = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AnimatorのSpeedパラメータに受け渡す値のタイプ。
		/// </summary>
#else
		/// <summary>
		/// The type of value to pass to the animator's Speed parameter.
		/// </summary>
#endif
		public SpeedType speedType
		{
			get
			{
				return _SpeedType;
			}
			set
			{
				_SpeedType = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Agentに設定しているspeedで割るかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether or not to divide by the speed set for Agent.
		/// </summary>
#endif
		[System.Obsolete("use speedType")]
		public bool isDivAgentSpeed
		{
			get
			{
				return _SpeedType == SpeedType.DivSpeed;
			}
			set
			{
				_SpeedType = value ? SpeedType.DivSpeed : SpeedType.NotChange;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動する速さのダンプ時間。
		/// </summary>
#else
		/// <summary>
		/// Dump time of moving speed.
		/// </summary>
#endif
		public float speedDampTime
		{
			get
			{
				return _SpeedDampTime;
			}
			set
			{
				_SpeedDampTime = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動ベクトルのタイプ。
		/// </summary>
#else
		/// <summary>
		/// Type of movement vector.
		/// </summary>
#endif
		public MovementType movementType
		{
			get
			{
				return _MovementType;
			}
			set
			{
				_MovementType = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// velocityに対して割る値。(_MovementTypeがMovementType.DivValueの時のみ使用)
		/// </summary>
		/// <remarks>0を指定した場合は無効。</remarks>
#else
		/// <summary>
		/// The value to divide for velocity. (Used only when _MovementType is MovementType.DivValue)
		/// </summary>
		/// <remarks>Disable If you specify 0.</remarks>
#endif
		public float movementDivValue
		{
			get
			{
				return _MovementDivValue;
			}
			set
			{
				_MovementDivValue = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Agentのローカル空間での移動ベクトルのX値をAnimatorへ設定ためのfloatパラメータを指定する。
		/// </summary>
#else
		/// <summary>
		/// Specify the float parameter for setting the X value of the moving vector in the Agent's local space to Animator.
		/// </summary>
#endif
		public string movementXParameter
		{
			get
			{
				return _MovementXParameter.name;
			}
			set
			{
				_MovementXParameter.name = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動方向ベクトルのX値のダンプ時間。
		/// </summary>
#else
		/// <summary>
		/// Dump time of X value of moving direction vector.
		/// </summary>
#endif
		public float movementXDampTime
		{
			get
			{
				return _MovementXDampTime;
			}
			set
			{
				_MovementXDampTime = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Agentのローカル空間での移動方向ベクトルのY値をAnimatorへ設定ためのfloatパラメータを指定する。
		/// </summary>
#else
		/// <summary>
		/// Specify the float parameter for setting the Y value of the moving direction vector in the Agent's local space to Animator.
		/// </summary>
#endif
		public string movementYParameter
		{
			get
			{
				return _MovementYParameter.name;
			}
			set
			{
				_MovementYParameter.name = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動方向ベクトルのY値のダンプ時間。
		/// </summary>
#else
		/// <summary>
		/// Dump time of Y value of moving direction vector.
		/// </summary>
#endif
		public float movementYDampTime
		{
			get
			{
				return _MovementYDampTime;
			}
			set
			{
				_MovementYDampTime = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Agentのローカル空間での移動方向ベクトルのZ値をAnimatorへ設定ためのfloatパラメータを指定する。
		/// </summary>
#else
		/// <summary>
		/// Specify the float parameter for setting the Z value of the moving direction vector in the Agent's local space to Animator.
		/// </summary>
#endif
		public string movementZParameter
		{
			get
			{
				return _MovementZParameter.name;
			}
			set
			{
				_MovementZParameter.name = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動方向ベクトルのZ値のダンプ時間。
		/// </summary>
#else
		/// <summary>
		/// Dump time of Z value of moving direction vector.
		/// </summary>
#endif
		public float movementZDampTime
		{
			get
			{
				return _MovementZDampTime;
			}
			set
			{
				_MovementZDampTime = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ターン方向をAnimatorへ設定するためのfloatパラメータを指定する。
		/// </summary>
#else
		/// <summary>
		/// Specify the float parameter for setting the turn direction to Animator.
		/// </summary>
#endif
		public string turnParameter
		{
			get
			{
				return _TurnParameter.name;
			}
			set
			{
				_TurnParameter.name = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Turnのタイプ
		/// </summary>
#else
		/// <summary>
		/// Type of Turn.
		/// </summary>
#endif
		public TurnType turnType
		{
			get
			{
				return _TurnType;
			}
			set
			{
				_TurnType = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// ターン方向のダンプ時間。
		/// </summary>
#else
		/// <summary>
		/// Dump time in the turn direction.
		/// </summary>
#endif
		public float turnDampTime
		{
			get
			{
				return _TurnDampTime;
			}
			set
			{
				_TurnDampTime = value;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// AgentのTransform
		/// </summary>
#else
		/// <summary>
		/// Agent Transform
		/// </summary>
#endif
		public Transform agentTransform
		{
			get;
			private set;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動速度
		/// </summary>
#else
		/// <summary>
		/// Movement velocity
		/// </summary>
#endif
		public override Vector3 velocity
		{
			get
			{
				return _Agent.velocity;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 自身のTransform
		/// </summary>
#else
		/// <summary>
		/// Own Transform
		/// </summary>
#endif
		public override Transform selfTransform
		{
			get
			{
				return agentTransform;
			}
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動完了したかどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether the move is complete or not.
		/// </summary>
#endif
		public bool isDone
		{
			get
			{
				switch (_Status)
				{
					case Status.Stopping:
						return true;
					case Status.Moving:
						return IsOnDestination() || (_MovingMode == MovingMode.Escape && (agentTransform.position - _TargetPosition).magnitude >= _EscapeDistance);
					case Status.RotatingOffMeshLink:
						return false;
					case Status.WatingStartOffMeshLink:
						return false;
					case Status.TraversingOffMeshLink:
						return false;
					case Status.WatingEndOffMeshLink:
						return false;
					case Status.Rotating:
						{
							float angle = Quaternion.Angle(_LookRotation, agentTransform.rotation);
							return angle <= 0.1f;
						}
				}
				return true;
			}
		}

		bool IsOnDestination()
		{
			return (!_Agent.pathPending && (_Agent.remainingDistance <= _Agent.stoppingDistance || Mathf.Approximately(_Agent.remainingDistance, _Agent.stoppingDistance))) && !(_Agent.hasPath && isMoving);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動中かどうか
		/// </summary>
#else
		/// <summary>
		/// Whether it is moving
		/// </summary>
#endif
		public bool isMoving
		{
			get;
			private set;
		}

		private Status _Status = Status.Stopping;
		private MovingMode _MovingMode = MovingMode.Follow;

		private Vector3 _StartPosition;

		private Vector3 _Direction = Vector3.zero;
		private Quaternion _LookRotation = Quaternion.identity;
		private float _AngularSpeed = 0.0f;

		private Vector3 _TargetPosition;
		private float _EscapeDistance;

		OffMeshLinkData _OffMeshLinkData;
		AnimatorName _CurrentTraversingParameter;

		bool CheckExistsParameter(AnimatorName name)
		{
			return _Animator != null && name != null && name.hash.HasValue && AnimatorUtility.CheckExistsParameter(_Animator, name.name);
		}

		private float GetFloat(AnimatorName name)
		{
			if (CheckExistsParameter(name))
			{
				return _Animator.GetFloat(name.hash.Value);
			}
			return 0.0f;
		}

		private void SetFloat(AnimatorName name, float value)
		{
			if (CheckExistsParameter(name))
			{
				_Animator.SetFloat(name.hash.Value, value);
			}
		}

		private void SetBool(AnimatorName name, bool value)
		{
			if (CheckExistsParameter(name))
			{
				_Animator.SetBool(name.hash.Value, value);
			}
		}

		private void SetFloat(AnimatorName name, float value, float dampTime, float deltaTime)
		{
			if (CheckExistsParameter(name))
			{
				_Animator.SetFloat(name.hash.Value, value, dampTime, deltaTime);
			}
		}

		private bool IsMovingAnimator()
		{
			return Mathf.Abs(GetFloat(_SpeedParameter)) >= 0.01f ||
				Mathf.Abs(GetFloat(_MovementXParameter)) >= 0.01f ||
				Mathf.Abs(GetFloat(_MovementYParameter)) >= 0.01f ||
				Mathf.Abs(GetFloat(_MovementZParameter)) >= 0.01f ||
				Mathf.Abs(GetFloat(_TurnParameter)) >= 0.01f;
		}

		private bool IsMoving()
		{
			return _Status == Status.Moving && (!Mathf.Approximately(_Agent.velocity.magnitude, 0.0f) || IsMovingAnimator());
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// この関数はスクリプトのインスタンスがロードされたときに呼び出される。
		/// </summary>
#else
		/// <summary>
		/// This function is called when the script instance is being loaded.
		/// </summary>
#endif
		protected override void Awake()
		{
			base.Awake();
			if (_Agent == null)
			{
				this.TryGetComponent<NavMeshAgent>(out _Agent);
			}

			agentTransform = _Agent.transform;
			_StartPosition = agentTransform.position;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定半径内のランダムな位置に向かって移動する
		/// </summary>
		/// <param name="center">中心</param>
		/// <param name="speed">移動する速さ</param>
		/// <param name="radius">開始位置からの半径</param>
		/// <param name="stoppingDistance">停止距離</param>
		/// <param name="checkRaycast">レイキャストによる補正を行うかどうか。<br/>trueの場合壁を迂回せずに壁に向かって移動する。</param>
#else
		/// <summary>
		/// Move towards a random position within a specified radius
		/// </summary>
		/// <param name="center">Center</param>
		/// <param name="speed">Speed to move</param>
		/// <param name="radius">Radius from the starting position</param>
		/// <param name="stoppingDistance">Stopping distance</param>
		/// <param name="checkRaycast">Whether to make corrections by raycasting. <br/> If true, move toward the wall without detouring the wall.</param>
#endif
		public void MoveToRandomPosition(Vector3 center, float speed, float radius, float stoppingDistance, bool checkRaycast)
		{
			Vector2 circle = Random.insideUnitCircle;
			Vector3 dir = new Vector3(circle.x, 0f,circle.y) * Random.Range(0.0f, radius);

			Vector3 toPos = center + dir;

			if (checkRaycast)
			{
				if (NavMesh.Raycast(this.position, toPos, out var hit, _Agent.areaMask))
				{
					toPos = hit.position;
				}
			}

			MoveTo(speed, stoppingDistance, toPos);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始位置から指定半径内のランダムな位置に向かって移動する
		/// </summary>
		/// <param name="speed">移動する速さ</param>
		/// <param name="radius">開始位置からの半径</param>
		/// <param name="stoppingDistance">停止距離</param>
		/// <param name="checkRaycast">レイキャストによる補正を行うかどうか。<br/>trueの場合壁を迂回せずに壁に向かって移動する。</param>
#else
		/// <summary>
		/// Move from the start position to a random position within the specified radius
		/// </summary>
		/// <param name="speed">Speed to move</param>
		/// <param name="radius">Radius from the starting position</param>
		/// <param name="stoppingDistance">Stopping distance</param>
		/// <param name="checkRaycast">Whether to make corrections by raycasting. <br/> If true, move toward the wall without detouring the wall.</param>
#endif
		public void MoveToRandomPosition(float speed, float radius, float stoppingDistance, bool checkRaycast)
		{
			MoveToRandomPosition(_StartPosition, speed, radius, stoppingDistance, checkRaycast);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定半径内をうろつく
		/// </summary>
		/// <param name="center">中心</param>
		/// <param name="speed">移動する速さ</param>
		/// <param name="radius">開始位置からの半径</param>
#else
		/// <summary>
		/// Wander within specified radius
		/// </summary>
		/// <param name="center">Center</param>
		/// <param name="speed">Speed to move</param>
		/// <param name="radius">Radius from the starting position</param>
#endif
		[System.Obsolete("use MoveToRandomPosition()")]
		public void Patrol(Vector3 center, float speed, float radius)
		{
			MoveToRandomPosition(center, speed, radius, 0f, false);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始位置から指定半径内をうろつく
		/// </summary>
		/// <param name="speed">移動する速さ</param>
		/// <param name="radius">開始位置からの半径</param>
#else
		/// <summary>
		/// Wander within the specified radius from the start position
		/// </summary>
		/// <param name="speed">Speed to move</param>
		/// <param name="radius">Radius from the starting position</param>
#endif
		[System.Obsolete("use MoveToRandomPosition()")]
		public void Patrol(float speed, float radius)
		{
			MoveToRandomPosition(speed, radius, 0f, false);
		}

		private Vector3 _WanderVec = Vector3.forward;

#if ARBOR_DOC_JA
		/// <summary>
		/// Agentを徘徊させる。
		/// </summary>
		/// <param name="speed">移動する速さ</param>
		/// <param name="radius">徘徊する半径。</param>
		/// <param name="distance">徘徊する距離。</param>
		/// <param name="jitter">移動先を決めるランダムな変位の最大値。</param>
		/// <param name="stoppingDistance">停止距離</param>
		/// <returns>移動先の設定が成功した場合にtrueを返す。それ以外はfalseを返す。</returns>
		/// <remarks>
		/// 徘徊の移動先は、Agentの前方方向Distanceを中心としたRadius円上の点<br/>
		/// 円上のどこになるかはJitterによってランダムに変位する。
		/// </remarks>
#else
		/// <summary>
		/// Wander the Agent
		/// </summary>
		/// <param name="speed">Speed to move</param>
		/// <param name="radius">The wandering radius.</param>
		/// <param name="distance">The wandering distance.</param>
		/// <param name="jitter">The maximum value of random displacement that determines the destination.</param>
		/// <param name="stoppingDistance">Stopping distance</param>
		/// <returns>Returns true if the destination setting is successful. Otherwise false.</returns>
		/// <remarks>
		/// The destination of the wandering is the point on the Radius circle centered on the agent's forward distance <br />
		/// Where on the circle is randomly displaced by Jitter.
		/// </remarks>
#endif
		public bool Wander(float speed, float radius, float distance, float jitter,float stoppingDistance)
		{
			_WanderVec *= radius;
			_WanderVec += new Vector3(Random.Range(-1f, 1f) * jitter, Random.Range(-1f, 1f) * jitter);
			_WanderVec.Normalize();
			
			Vector3 targetWorld = position + forward * distance + selfTransform.TransformDirection(_WanderVec) * radius;

			return MoveTo(speed, stoppingDistance, targetWorld);
		}

		bool BeginMoveTo(Vector3 toPos)
		{
			if (_Agent.isOnOffMeshLink && !_Agent.autoTraverseOffMeshLink && !_EnableTraverseOffMeshLink || _Status == Status.RotatingOffMeshLink)
			{
				_Agent.Warp(position); // Cancel off mesh link
				_Agent.updateRotation = true;
			}

			bool result = _Agent.SetDestination(toPos);
			if (result)
			{
				if (_Status != Status.TraversingOffMeshLink && _Status != Status.WatingStartOffMeshLink && _Status != Status.WatingEndOffMeshLink)
				{
					_Status = Status.Moving;
				}
				Resume();
			}
			return result;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定した位置へ近づく
		/// </summary>
		/// <param name="speed">移動する速さ</param>
		/// <param name="stoppingDistance">停止距離</param>
		/// <param name="targetPosition">目標地点</param>
		/// <returns>移動先の設定が成功した場合にtrueを返す。それ以外はfalseを返す。</returns>
#else
		/// <summary>
		/// It approaches the specified position
		/// </summary>
		/// <param name="speed">Speed to move</param>
		/// <param name="stoppingDistance">Stopping distance</param>
		/// <param name="targetPosition">Objective point</param>
		/// <returns>Returns true if the destination setting is successful. Otherwise false.</returns>
#endif
		public bool MoveTo(float speed, float stoppingDistance, Vector3 targetPosition)
		{
			_MovingMode = MovingMode.Follow;
			_TargetPosition = targetPosition;
			_Agent.speed = speed;
			_Agent.stoppingDistance = stoppingDistance;

			return BeginMoveTo(targetPosition);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定したTransformの位置へ近づく
		/// </summary>
		/// <param name="speed">移動する速さ</param>
		/// <param name="stoppingDistance">停止距離</param>
		/// <param name="target">目標地点</param>
		/// <returns>移動先の設定が成功した場合にtrueを返す。それ以外はfalseを返す。</returns>
#else
		/// <summary>
		/// Approach to the position of the specified Transform
		/// </summary>
		/// <param name="speed">Speed to move</param>
		/// <param name="stoppingDistance">Stopping distance</param>
		/// <param name="target">Objective point</param>
		/// <returns>Returns true if the destination setting is successful. Otherwise false.</returns>
#endif
		public bool MoveTo(float speed, float stoppingDistance, Transform target)
		{
			if (target == null)
			{
				return false;
			}
			return MoveTo(speed, stoppingDistance, target.position);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定した位置へ近づく
		/// </summary>
		/// <param name="speed">移動する速さ</param>
		/// <param name="stoppingDistance">停止距離</param>
		/// <param name="targetPosition">目標地点</param>
		/// <returns>移動先の設定が成功した場合にtrueを返す。それ以外はfalseを返す。</returns>
#else
		/// <summary>
		/// It approaches the specified position
		/// </summary>
		/// <param name="speed">Speed to move</param>
		/// <param name="stoppingDistance">Stopping distance</param>
		/// <param name="targetPosition">Objective point</param>
		/// <returns>Returns true if the destination setting is successful. Otherwise false.</returns>
#endif
		[System.Obsolete("use MoveTo")]
		public bool Follow(float speed, float stoppingDistance, Vector3 targetPosition)
		{
			return MoveTo(speed, stoppingDistance, targetPosition);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定したTransformの位置へ近づく
		/// </summary>
		/// <param name="speed">移動する速さ</param>
		/// <param name="stoppingDistance">停止距離</param>
		/// <param name="target">目標地点</param>
		/// <returns>移動先の設定が成功した場合にtrueを返す。それ以外はfalseを返す。</returns>
#else
		/// <summary>
		/// Approach to the position of the specified Transform
		/// </summary>
		/// <param name="speed">Speed to move</param>
		/// <param name="stoppingDistance">Stopping distance</param>
		/// <param name="target">Objective point</param>
		/// <returns>Returns true if the destination setting is successful. Otherwise false.</returns>
#endif
		[System.Obsolete("use MoveTo")]
		public bool Follow(float speed, float stoppingDistance, Transform target)
		{
			return MoveTo(speed, stoppingDistance, target);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象の移動速度を考慮して追跡するように移動する。
		/// </summary>
		/// <param name="speed">移動する速さ</param>
		/// <param name="stoppingDistance">停止距離</param>
		/// <param name="facingAngle">対面しているかを判定する角度<br/>TargetがAgent側を向いて対面している時はTargetに向かって直接移動する</param>
		/// <param name="target">対象となるMovingEntity</param>
		/// <param name="offset">Targetのローカル座標系でのオフセット</param>
		/// <returns>移動先の設定が成功した場合にtrueを返す。それ以外はfalseを返す。</returns>
#else
		/// <summary>
		/// Move to track in consideration of the movement velocity of the target.
		/// </summary>
		/// <param name="speed">Speed to move</param>
		/// <param name="stoppingDistance">Stopping distance</param>
		/// <param name="facingAngle">Angle to judge whether they are facing each other <br/>When Target faces the Agent side and faces, move directly toward Target</param>
		/// <param name="target">Target MovingEntity</param>
		/// <param name="offset">Target's offset in the local coordinate system</param>
		/// <returns>Returns true if the destination setting is successful. Otherwise false.</returns>
#endif
		public bool Pursuit(float speed, float stoppingDistance, float facingAngle, MovingEntity target, Vector3 offset)
		{
			if (target == null)
			{
				return false;
			}

			Transform targetTransform = target.selfTransform;
			Vector3 targetPosition = target.position + targetTransform.TransformVector(offset);
			Vector3 targetVelocity = target.velocity;
			Vector3 targetForward = target.forward;

			Vector3 agentPosition = this.position;
			Vector3 agentForward = this.forward;
			Vector3 agentToTarget = target.position - agentPosition;

			float relativeForward = Vector3.Dot(agentForward, targetForward);

			if(Vector3.Dot(agentToTarget, agentForward) > 0f &&
				relativeForward < -Mathf.Cos(facingAngle * Mathf.Deg2Rad))
			{
				return MoveTo(speed, stoppingDistance, targetPosition);
			}

			float totalSpeed = speed + targetVelocity.magnitude;

			float lookAheadTime = totalSpeed > 0f ? agentToTarget.magnitude / totalSpeed : 0f;

			return MoveTo(speed, stoppingDistance, targetPosition + targetVelocity * lookAheadTime);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Agentを２つのTargetの間に向かって近づくように移動させる。
		/// </summary>
		/// <param name="speed">移動する速さ</param>
		/// <param name="targetA">近づきたい対象の一つ目のMovingEntity</param>
		/// <param name="targetB">近づきたい対象の二つ目のMovingEntity</param>
		/// <param name="stoppingDistance">停止距離</param>
		/// <returns>移動先の設定が成功した場合にtrueを返す。それ以外はfalseを返す。</returns>
#else
		/// <summary>
		/// Move the Agent closer to between the two Targets.
		/// </summary>
		/// <param name="speed">Speed to move</param>
		/// <param name="targetA">The first Moving Entity of the object you want to approach</param>
		/// <param name="targetB">The second Moving Entity of the object you want to approach</param>
		/// <param name="stoppingDistance">Stopping distance</param>
		/// <returns>Returns true if the destination setting is successful. Otherwise false.</returns>
#endif
		public bool Interpose(float speed, MovingEntity targetA, MovingEntity targetB, float stoppingDistance)
		{
			if (targetA == null || targetB == null)
			{
				return false;
			}

			Vector3 midPoint = (targetA.position + targetB.position) * 0.5f;

			if (speed > 0f)
			{
				float timeToReachMidPoint = Vector3.Distance(position, midPoint) / speed;

				Vector3 posA = targetA.position + targetA.velocity * timeToReachMidPoint;
				Vector3 posB = targetB.position + targetB.velocity * timeToReachMidPoint;

				midPoint = (posA + posB) * 0.5f;
			}

			return MoveTo(speed, stoppingDistance, midPoint);
		}

		private bool _CornerStucking;
		private Vector3 _CornerStuckingDirection;

		Vector3 GetEscapePosition(Vector3 agentPosition, Vector3 toPos, float distance, float distanceToCorner, Vector3 agentToTargetDir, ref bool cornerStuking, ref Vector3 cornerStuckDirection)
		{
			NavMeshHit hit;
			Vector3 currentPos = agentPosition;
			float currentDistance = distance;
			if (NavMesh.Raycast(agentPosition, toPos, out hit, _Agent.areaMask))
			{
				currentPos = hit.position;
				currentDistance -= hit.distance;

				Vector3 hitNormal = hit.normal;
				Vector3 sideDir = new Vector3(-hitNormal.z, hitNormal.y, hitNormal.x);

				if (Vector3.Dot(sideDir, agentToTargetDir) > Vector3.Dot(-sideDir, agentToTargetDir))
				{
					sideDir = -sideDir;
				}

				toPos = currentPos + sideDir * currentDistance;

				if (hit.distance < distanceToCorner || cornerStuking)
				{
					if (NavMesh.Raycast(agentPosition, toPos, out hit, _Agent.areaMask))
					{
						currentPos = hit.position;
						toPos = hit.position;
						currentDistance -= hit.distance;

						if (!cornerStuking && hit.distance < distanceToCorner)
						{
							if (Vector3.Dot(hitNormal, agentToTargetDir) > Vector3.Dot(-sideDir, agentToTargetDir))
							{
								hitNormal = -sideDir;
							}
							toPos = currentPos + hitNormal * currentDistance;

							cornerStuking = true;
							cornerStuckDirection = hitNormal;
						}
					}
					else
					{
						cornerStuking = false;
					}
				}
			}
			else
			{
				cornerStuking = false;
			}

			return toPos;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定した位置から遠ざかる
		/// </summary>
		/// <param name="speed">移動する速さ</param>
		/// <param name="distance">遠ざかる距離</param>
		/// <param name="targetPosition">対象</param>
		/// <param name="distanceToCorner">コーナー付近を判別する距離。<br/>壁際に追い詰められないようにコーナーを迂回するのに使用する</param>
		/// <returns>移動先の設定が成功した場合にtrueを返す。それ以外はfalseを返す。</returns>
#else
		/// <summary>
		/// Keep away from specified position
		/// </summary>
		/// <param name="speed">Speed to move</param>
		/// <param name="distance">Distance away</param>
		/// <param name="targetPosition">Target</param>
		/// <param name="distanceToCorner">The distance to determine the vicinity of the corner.<br/>Used to detour corners so that you can't be cornered by the wall</param>
		/// <returns>Returns true if the destination setting is successful. Otherwise false.</returns>
#endif
		public bool Escape(float speed, float distance, Vector3 targetPosition, float distanceToCorner)
		{
			_MovingMode = MovingMode.Escape;
			_TargetPosition = targetPosition;
			_EscapeDistance = distance;

			Vector3 agentPosition = agentTransform.position;
			Vector3 agentToTarget = targetPosition - agentPosition;

			float distanceToTarget = agentToTarget.magnitude;

			if (distanceToTarget >= distance)
			{
				return false;
			}

			Vector3 agentToTargetDir = agentToTarget.normalized;

			Vector3 toPos = -agentToTargetDir * distance + agentPosition;

			bool oldCornerStucking = _CornerStucking;
			Vector3 oldCornerStuckingDir = _CornerStuckingDirection;

			Vector3 cornerStuckingDir = Vector3.zero;
			toPos = GetEscapePosition(agentPosition, toPos, distance, distanceToCorner, agentToTarget, ref _CornerStucking, ref _CornerStuckingDirection);

			if (oldCornerStucking && _CornerStucking)
			{
				toPos = _CornerStuckingDirection * distance + agentPosition;
			}

			NavMeshHit hit;
			if (NavMesh.Raycast(agentPosition, toPos, out hit, _Agent.areaMask))
			{
				if (NavMesh.FindClosestEdge(toPos, out hit, _Agent.areaMask))
				{
					toPos = hit.position;
				}
			}

			_Agent.speed = speed;
			_Agent.stoppingDistance = 0.0f;

			return BeginMoveTo(toPos);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定したTransformから遠ざかる
		/// </summary>
		/// <param name="speed">移動する速さ</param>
		/// <param name="distance">遠ざかる距離</param>
		/// <param name="target">対象</param>
		/// <param name="distanceToCorner">コーナー付近を判別する距離。<br/>壁際に追い詰められないようにコーナーを迂回するのに使用する</param>
		/// <returns>移動先の設定が成功した場合にtrueを返す。それ以外はfalseを返す。</returns>
#else
		/// <summary>
		/// Away from the specified Transform
		/// </summary>
		/// <param name="speed">Speed to move</param>
		/// <param name="distance">Distance away</param>
		/// <param name="target">Target</param>
		/// <param name="distanceToCorner">The distance to determine the vicinity of the corner.<br/>Used to detour corners so that you can't be cornered by the wall</param>
		/// <returns>Returns true if the destination setting is successful. Otherwise false.</returns>
#endif
		public bool Escape(float speed, float distance, Transform target, float distanceToCorner)
		{
			if (target == null)
			{
				return false;
			}

			return Escape(speed, distance, target.position, distanceToCorner);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Targetの移動速度を考慮して逃げるように移動する。
		/// </summary>
		/// <param name="speed">移動する速さ</param>
		/// <param name="distance">遠ざかる距離</param>
		/// <param name="target">対象のMovingEntity</param>
		/// <param name="distanceToCorner">コーナー付近を判別する距離。<br/>壁際に追い詰められないようにコーナーを迂回するのに使用する</param>
		/// <returns>移動先の設定が成功した場合にtrueを返す。それ以外はfalseを返す。</returns>
#else
		/// <summary>
		/// Move to escape in consideration of the movement velocity of Target.
		/// </summary>
		/// <param name="speed">Speed to move</param>
		/// <param name="distance">Distance away</param>
		/// <param name="target">Target MovingEntity</param>
		/// <param name="distanceToCorner">The distance to determine the vicinity of the corner.<br/>Used to detour corners so that you can't be cornered by the wall</param>
		/// <returns>Returns true if the destination setting is successful. Otherwise false.</returns>
#endif
		public bool Evade(float speed, float distance, MovingEntity target, float distanceToCorner)
		{
			if (target == null)
			{
				return false;
			}

			Vector3 targetPosition = target.position;
			Vector3 targetVelocity = target.velocity;

			Vector3 agentPosition = this.position;
			Vector3 agentVelocity = this.velocity;

			Vector3 agentToTarget = targetPosition - agentPosition;

			float totalSpeed = speed + targetVelocity.magnitude;
			float lookAheadTime = totalSpeed > 0f ? agentToTarget.magnitude / totalSpeed : 0f;

			return Escape(speed, distance, targetPosition + targetVelocity * lookAheadTime, distanceToCorner);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Targetから隠れるように移動する。
		/// </summary>
		/// <param name="speed">移動する速さ</param>
		/// <param name="targetPosition">対象の位置</param>
		/// <param name="minDistanceToTarget">ターゲットとの最小距離。この距離よりも近い位置にある障害物は隠れ先の候補から除外される。</param>
		/// <param name="obstacleTargetFlags">どの障害物のタイプに隠れるかを設定する</param>
		/// <param name="obstacleSearchFlags">障害物の検索方法を設定する</param>
		/// <param name="obstacleLayer">障害物のレイヤー</param>
		/// <returns>移動先の設定が成功した場合にtrueを返す。それ以外はfalseを返す。</returns>
#else
		/// <summary>
		/// Move to hide from Target.
		/// </summary>
		/// <param name="speed">Speed to move</param>
		/// <param name="targetPosition">Target position</param>
		/// <param name="minDistanceToTarget">Minimum distance to the target. Obstacles closer than this distance are excluded from potential hiding places.</param>
		/// <param name="obstacleTargetFlags">Set which obstacle type to hide</param>
		/// <param name="obstacleSearchFlags">Set the obstacle search method</param>
		/// <param name="obstacleLayer">Obstacle layer</param>
		/// <returns>Returns true if the destination setting is successful. Otherwise false.</returns>
#endif
		public bool Hide(float speed, Vector3 targetPosition, float minDistanceToTarget, ObstacleTargetFlags obstacleTargetFlags, ObstacleSearchFlags obstacleSearchFlags, int obstacleLayer)
		{
			if (ObstacleManager.TryGetHidingPosition(this, targetPosition, minDistanceToTarget, obstacleTargetFlags, obstacleSearchFlags, obstacleLayer, out var hidingPos))
			{
				bool result = MoveTo(speed, 0f, hidingPos);
				if (result)
				{
					_MovingMode = MovingMode.Hide;
					_TargetPosition = targetPosition;
					return true;
				}
			}
			return false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// Targetから隠れるように移動する。
		/// </summary>
		/// <param name="speed">移動する速さ</param>
		/// <param name="target">対象のTransform</param>
		/// <param name="minDistanceToTarget">ターゲットとの最小距離。この距離よりも近い位置にある障害物は隠れ先の候補から除外される。</param>
		/// <param name="obstacleTargetFlags">どの障害物のタイプに隠れるかを設定する</param>
		/// <param name="obstacleSearchFlags">障害物の検索方法を設定する</param>
		/// <param name="obstacleLayer">障害物のレイヤー</param>
		/// <returns>移動先の設定が成功した場合にtrueを返す。それ以外はfalseを返す。</returns>
#else
		/// <summary>
		/// Move to hide from Target.
		/// </summary>
		/// <param name="speed">Speed to move</param>
		/// <param name="target">Target Transform</param>
		/// <param name="minDistanceToTarget">Minimum distance to the target. Obstacles closer than this distance are excluded from potential hiding places.</param>
		/// <param name="obstacleTargetFlags">Set which obstacle type to hide</param>
		/// <param name="obstacleSearchFlags">Set the obstacle search method</param>
		/// <param name="obstacleLayer">Obstacle layer</param>
		/// <returns>Returns true if the destination setting is successful. Otherwise false.</returns>
#endif
		public bool Hide(float speed, Transform target, float minDistanceToTarget, ObstacleTargetFlags obstacleTargetFlags, ObstacleSearchFlags obstacleSearchFlags, int obstacleLayer)
		{
			if (target == null)
			{
				return false;
			}

			return Hide(speed, target.position, minDistanceToTarget, obstacleTargetFlags, obstacleSearchFlags, obstacleLayer);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定した位置の方向へ回転する。
		/// </summary>
		/// <param name="angularSpeed">回転する速さ</param>
		/// <param name="targetPosition">対象</param>
#else
		/// <summary>
		/// Rotate in the direction of the specified position.
		/// </summary>
		/// <param name="angularSpeed">Speed of rotation</param>
		/// <param name="targetPosition">Target</param>
#endif
		public void LookAt(float angularSpeed, Vector3 targetPosition)
		{
			Stop();

			_AngularSpeed = angularSpeed;

			Vector3 direction = (targetPosition - agentTransform.position);
			if (Mathf.Approximately(direction.sqrMagnitude, 0.0f))
			{
				_Direction = agentTransform.forward;
			}
			else
			{
				_Direction = direction.normalized;
			}

			_Direction.y = 0f;
			_LookRotation = Quaternion.LookRotation(_Direction);

			_Status = Status.Rotating;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定したTransformの方向へ回転する。
		/// </summary>
		/// <param name="angularSpeed">回転する速さ</param>
		/// <param name="target">対象</param>
#else
		/// <summary>
		/// Rotates in the direction of the specified Transform.
		/// </summary>
		/// <param name="angularSpeed">Speed of rotation</param>
		/// <param name="target">Target</param>
#endif
		public void LookAt(float angularSpeed, Transform target)
		{
			if (target == null)
			{
				return;
			}

			LookAt(angularSpeed, target.position);
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動を再開する。
		/// </summary>
#else
		/// <summary>
		/// Resume movement.
		/// </summary>
#endif
		public void Resume()
		{
			_Agent.isStopped = false;
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 停止する。
		/// </summary>
#else
		/// <summary>
		/// Stop.
		/// </summary>
#endif
		public void Stop()
		{
			Stop(false);
		}

		void InternalStop()
		{
			_Status = Status.Stopping;

			_MovingMode = MovingMode.Follow;
			_CornerStucking = false;
		}


#if ARBOR_DOC_JA
		/// <summary>
		/// 停止する。
		/// </summary>
		/// <param name="clearVelocity">速度をクリアするかどうか</param>
#else
		/// <summary>
		/// Stop.
		/// </summary>
		/// <param name="clearVelocity">Whether to clear velocity</param>
#endif

		public void Stop(bool clearVelocity)
		{
			_Agent.isStopped = true;

			if (clearVelocity)
			{
				_Agent.velocity = Vector3.zero;
			}

			InternalStop();
		}

#if ARBOR_DOC_JA
		/// <summary>
		/// 指定された位置にエージェントをワープします。
		/// </summary>
		/// <param name="newPosition">エージェントをワープさせる位置</param>
		/// <returns>経路の割り当てに成功した場合 true</returns>
#else
		/// <summary>
		/// Warps agent to the provided position.
		/// </summary>
		/// <param name="newPosition">New position to warp the agent to.</param>
		/// <returns>True if agent is successfully warped, otherwise false.</returns>
#endif
		public bool Warp(Vector3 newPosition)
		{
			return _Agent.Warp(newPosition);
		}

		private OffMeshLinkTraverseData _TraverseData;
		private float _TraversingDuration;
		private float _TraversingTime;
		private string _TraversingTriggerName;
		private Vector3 _TraversingStartPos;
		private Vector3 _TraversingEndPos;
		private float _TraversingGravity;
		private Vector3 _TraversingVelocity = Vector3.zero;
		private AnimationTriggerEventReceiver _TriggerEventReceiver = null;
		private OffMeshLinkTraverseWaitData _TraverseWaitData;

		float GetTurnValue(Vector3 direction)
		{
			float turn = 0.0f;
			switch (_TurnType)
			{
				case TurnType.UseX:
					turn = direction.x;
					break;
				case TurnType.RadianAngle:
					turn = Mathf.Atan2(direction.x, direction.z);
					break;
			}
			return turn;
		}

		void OnTrigger(string triggerName)
		{
			_TraversingTriggerName = triggerName;
		}

		void Update()
		{
			bool currentMoving = IsMoving();
			if (isMoving != currentMoving)
			{
				isMoving = currentMoving;

				if (!isMoving)
				{
					if (_Animator != null)
					{
						SetBool(_MovingParameter, false);
						SetFloat(_SpeedParameter, 0.0f);
						SetFloat(_MovementXParameter, 0.0f);
						SetFloat(_MovementYParameter, 0.0f);
						SetFloat(_MovementZParameter, 0.0f);
						SetFloat(_TurnParameter, 0.0f);
					}
				}
			}

			float deltaTime = Time.deltaTime;

			if (isMoving)
			{
				if (_Animator != null)
				{
					Vector3 velocity = _Agent.velocity;
					velocity = agentTransform.InverseTransformDirection(velocity);

					float speed = velocity.magnitude;
					float agentSpeed = _Agent.speed;

					if (_MovingParameter.hash.HasValue)
					{
						bool moving = speed > _MovingSpeedThreshold;
						SetBool(_MovingParameter, moving);
					}

					if (_SpeedParameter.hash.HasValue)
					{
						switch (_SpeedType)
						{
							case SpeedType.NotChange:
								break;
							case SpeedType.DivSpeed:
								if (agentSpeed != 0.0f)
								{
									speed /= agentSpeed;
								}
								break;
							case SpeedType.DivValue:
								if (_SpeedDivValue != 0.0f)
								{
									speed /= _SpeedDivValue;
								}
								break;
						}
						SetFloat(_SpeedParameter, speed, _SpeedDampTime, deltaTime);
					}

					if (_MovementXParameter.hash.HasValue ||
						_MovementYParameter.hash.HasValue ||
						_MovementZParameter.hash.HasValue)
					{
						Vector3 movement = Vector3.zero;

						switch (_MovementType)
						{
							case MovementType.NotChange:
								movement = velocity;
								break;
							case MovementType.Normalize:
								movement = velocity.normalized;
								break;
							case MovementType.DivSpeed:
								if (agentSpeed != 0.0f)
								{
									movement = velocity / agentSpeed;
								}
								break;
							case MovementType.DivValue:
								if (_MovementDivValue != 0.0f)
								{
									movement = velocity / _MovementDivValue;
								}
								break;
						}

						SetFloat(_MovementXParameter, movement.x, _MovementXDampTime, deltaTime);
						SetFloat(_MovementYParameter, movement.y, _MovementYDampTime, deltaTime);
						SetFloat(_MovementZParameter, movement.z, _MovementZDampTime, deltaTime);
					}

					if (_TurnParameter.hash.HasValue)
					{
						velocity.Normalize();

						float turn = GetTurnValue(velocity);
						SetFloat(_TurnParameter, turn, _TurnDampTime, deltaTime);
					}
				}
			}

			bool isOnOffMeshLink = _Agent.isOnOffMeshLink;
			switch (_Status)
			{
				case Status.Moving:
					{
						_OffMeshLinkData = _Agent.currentOffMeshLinkData;
						if (isOnOffMeshLink && !_Agent.autoTraverseOffMeshLink && _EnableTraverseOffMeshLink && _OffMeshLinkData.valid && _OffMeshLinkData.activated)
						{
							_TraverseData = null;

							switch (_OffMeshLinkData.linkType)
							{
								case OffMeshLinkType.LinkTypeJumpAcross:
									_TraverseData = _JumpAcross;
									break;
								case OffMeshLinkType.LinkTypeDropDown:
									_TraverseData = _DropDown;
									break;
								case OffMeshLinkType.LinkTypeManual:
									if (_OffMeshLinkData.offMeshLink.TryGetComponent<OffMeshLinkSettings>(out var offMeshLinkController))
									{
										_TraverseData = offMeshLinkController.traverseData;
									}
									else
									{
										_TraverseData = _JumpAcross;
									}
									break;
							}

							_AngularSpeed = _TraverseData.angularSpeed;

							Vector3 direction = (_OffMeshLinkData.endPos - position);
							if (Mathf.Approximately(direction.sqrMagnitude, 0.0f))
							{
								_Direction = agentTransform.forward;
							}
							else
							{
								_Direction = direction.normalized;
							}

							_Direction.y = 0f;
							_LookRotation = Quaternion.LookRotation(_Direction);

							_Agent.velocity = Vector3.zero;
							_Agent.updateRotation = false;

							_Status = Status.RotatingOffMeshLink;
							break;
						}

						if (IsOnDestination())
						{
							InternalStop();
						}
					}
					break;
				case Status.RotatingOffMeshLink:
					{
						if (!isOnOffMeshLink || _Agent.autoTraverseOffMeshLink || !_EnableTraverseOffMeshLink || !_OffMeshLinkData.valid || !_OffMeshLinkData.activated)
						{
							_Agent.updateRotation = true;

							_Status = Status.Moving;
							break;
						}
						else
						{
							agentTransform.rotation = Quaternion.RotateTowards(agentTransform.rotation, _LookRotation, Time.deltaTime * _AngularSpeed);

							if (_TurnParameter.hash.HasValue)
							{
								Vector3 direction = agentTransform.InverseTransformDirection(_Direction);

								float turn = GetTurnValue(direction);
								SetFloat(_TurnParameter, turn, _TurnDampTime, deltaTime);
							}
							
							float angle = Quaternion.Angle(_LookRotation, agentTransform.rotation);
							if (angle <= 0.1f)
							{
								_CurrentTraversingParameter = _TraverseData.parameter;
								SetBool(_CurrentTraversingParameter, true);

								_TraverseWaitData = _TraverseData.startWait;
								switch (_TraverseWaitData.type)
								{
									case OffMeshLinkTraverseWaitData.WaitType.None:
										{
										}
										break;
									case OffMeshLinkTraverseWaitData.WaitType.Time:
										{
											_TraversingTime = 0f;
											_TraversingDuration = _TraverseWaitData.time;
										}
										break;
									case OffMeshLinkTraverseWaitData.WaitType.AnimationEvent:
										{
											_TraversingTriggerName = null;
											if (!_Animator.gameObject.TryGetComponent(out _TriggerEventReceiver))
											{
												_TriggerEventReceiver = _Animator.gameObject.AddComponent<AnimationTriggerEventReceiver>();
											}
											_TriggerEventReceiver.AddListener(OnTrigger);
										}
										break;
								}								

								_Status = Status.WatingStartOffMeshLink;
								break;
							}
						}
					}
					break;
				case Status.WatingStartOffMeshLink:
					{
						if (!isOnOffMeshLink || _Agent.autoTraverseOffMeshLink || !_EnableTraverseOffMeshLink || !_OffMeshLinkData.valid || !_OffMeshLinkData.activated)
						{
							_Status = Status.Moving;
							_Agent.updateRotation = true;
							if (_TriggerEventReceiver != null)
							{
								_TriggerEventReceiver.RemoveListener(OnTrigger);
							}
							break;
						}
						else
						{
							bool transition = false;
							switch (_TraverseWaitData.type)
							{
								case OffMeshLinkTraverseWaitData.WaitType.None:
									{
										transition = true;
									}
									break;
								case OffMeshLinkTraverseWaitData.WaitType.Time:
									{
										_TraversingTime += Time.deltaTime;
										transition = (_TraversingTime >= _TraversingDuration);
									}
									break;
								case OffMeshLinkTraverseWaitData.WaitType.AnimationEvent:
									{
										transition = (_TraversingTriggerName == _TraverseWaitData.eventName);
									}
									break;
							}

							if (transition)
							{
								if (_TriggerEventReceiver != null)
								{
									_TriggerEventReceiver.RemoveListener(OnTrigger);
								}

								_TraversingStartPos = position;
								_TraversingEndPos = _OffMeshLinkData.endPos + Vector3.up * _Agent.baseOffset;

								Vector3 startToEnd = (_TraversingEndPos - _TraversingStartPos);
								float moveY = startToEnd.y;
								startToEnd.y = 0f;

								float jumpSpeed = Mathf.Max(_TraverseData.minSpeed, _Agent.speed);
								float duration = startToEnd.magnitude / jumpSpeed;

								float height = _TraverseData.jumpHeight;
								if (moveY > 0f)
								{
									height += moveY;
								}
								height = Mathf.Max(height, 0f);

								float jumpTime = 0f;
								if (moveY > height || Mathf.Abs(moveY) <= 1.192093E-7f)
								{
									jumpTime = duration / 2f;
								}
								else
								{
									jumpTime = duration * (height - Mathf.Sqrt(height * (height - moveY))) / moveY;
								}

								_TraversingDuration = duration;
								_TraversingTime = 0f;

								_TraversingVelocity = startToEnd / duration;

								if (!Mathf.Approximately(jumpTime,0f))
								{
									_TraversingVelocity.y = 2f * height / jumpTime;
									_TraversingGravity = -2f * height / (jumpTime * jumpTime);
								}
								else
								{
									_TraversingGravity = 0f;
								}

								_Status = Status.TraversingOffMeshLink;
								break;
							}
						}
					}
					break;
				case Status.TraversingOffMeshLink:
					{
						_TraversingTime += Time.deltaTime;
						float t = _TraversingTime / _TraversingDuration;
						if (t < 1f)
						{
							float ySpeed = _TraversingGravity * _TraversingTime * 0.5f;
							agentTransform.position = _TraversingStartPos + (_TraversingVelocity + ySpeed * Vector3.up) * _TraversingTime;
						}
						else
						{
							agentTransform.position = _TraversingEndPos;

							SetBool(_CurrentTraversingParameter, false);

							_TraverseWaitData = _TraverseData.endWait;
							switch (_TraverseWaitData.type)
							{
								case OffMeshLinkTraverseWaitData.WaitType.None:
									{
									}
									break;
								case OffMeshLinkTraverseWaitData.WaitType.Time:
									{
										_TraversingTime = 0f;
										_TraversingDuration = _TraverseWaitData.time;
									}
									break;
								case OffMeshLinkTraverseWaitData.WaitType.AnimationEvent:
									{
										_TraversingTriggerName = null;
										if (!_Animator.gameObject.TryGetComponent<AnimationTriggerEventReceiver>(out _TriggerEventReceiver) || _TriggerEventReceiver == null)
										{
											_TriggerEventReceiver = _Animator.gameObject.AddComponent<AnimationTriggerEventReceiver>();
										}
										_TriggerEventReceiver.AddListener(OnTrigger);
									}
									break;
							}

							_Status = Status.WatingEndOffMeshLink;
							break;
						}
					}
					break;
				case Status.WatingEndOffMeshLink:
					{
						bool transition = false;
						switch (_TraverseWaitData.type)
						{
							case OffMeshLinkTraverseWaitData.WaitType.None:
								{
									transition = true;
								}
								break;
							case OffMeshLinkTraverseWaitData.WaitType.Time:
								{
									_TraversingTime += Time.deltaTime;
									transition = (_TraversingTime >= _TraversingDuration);
								}
								break;
							case OffMeshLinkTraverseWaitData.WaitType.AnimationEvent:
								{
									transition = (_TraversingTriggerName == _TraverseWaitData.eventName);
								}
								break;
						}

						if (transition)
						{
							if (_TriggerEventReceiver != null)
							{
								_TriggerEventReceiver.RemoveListener(OnTrigger);
							}

							_Agent.CompleteOffMeshLink();
							_Agent.updateRotation = true;
							_Status = Status.Moving;
							break;
						}
					}
					break;
				case Status.Rotating:
					{
						agentTransform.rotation = Quaternion.RotateTowards(agentTransform.rotation, _LookRotation, Time.deltaTime * _AngularSpeed);

						if (_TurnParameter.hash.HasValue)
						{
							Vector3 direction = agentTransform.InverseTransformDirection(_Direction);

							float turn = GetTurnValue(direction);
							SetFloat(_TurnParameter, turn, _TurnDampTime, deltaTime);
						}

						if (isDone)
						{
							InternalStop();
						}
					}
					break;
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Animator = _SpeedParameterOld.animator;
			_OldSpeedParameterVer1 = _SpeedParameterOld.name;
		}

		void SerializeVer2()
		{
			_MovingParameter.name = _OldMovingParameterVer1;
			_SpeedParameter.name = _OldSpeedParameterVer1;
			_MovementXParameter.name = _OldMovementXParameterVer1;
			_MovementYParameter.name = _OldMovementYParameterVer1;
			_MovementZParameter.name = _OldMovementZParameterVer1;
			_TurnParameter.name = _OldTurnParameterVer1;
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

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			Serialize();
		}
	}
}
