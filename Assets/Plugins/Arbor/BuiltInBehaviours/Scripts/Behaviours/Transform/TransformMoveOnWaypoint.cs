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
	/// Waypointに沿ってTransformを移動させる。
	/// </summary>
#else
	/// <summary>
	/// Move the Transform along the Waypoint.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/TransformMoveOnWaypoint")]
	[BuiltInBehaviour]
	public sealed class TransformMoveOnWaypoint : TransformBehaviourBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動する速さ
		/// </summary>
#else
		/// <summary>
		/// Speed to move
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Speed = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// Transformを移動させる経路
		/// </summary>
#else
		/// <summary>
		/// Route to move Transform
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Waypoint))]
		private FlexibleComponent _Waypoint = new FlexibleComponent();

#if ARBOR_DOC_JA
		/// <summary>
		/// 位置調整オフセット
		/// </summary>
#else
		/// <summary>
		/// Position adjustment offset
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _Offset = new FlexibleVector3();

#if ARBOR_DOC_JA
		/// <summary>
		/// 再開時にWaypointの最初の位置からやり直すフラグ
		/// </summary>
#else
		/// <summary>
		/// Flag to restart from the first position of Waypoint when restarting
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _ClearDestPoint = new FlexibleBool();

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生タイプ。
		/// </summary>
#else
		/// <summary>
		/// Play type.
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(MoveWaypointType))]
		private FlexibleMoveWayppintType _Type = new FlexibleMoveWayppintType(MoveWaypointType.Once);

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動計算を行う時間タイプ
		/// </summary>
#else
		/// <summary>
		/// Time type for moving calculation
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(WaypointTimeType))]
		private FlexibleWaypointTimeType _TimeType = new FlexibleWaypointTimeType(WaypointTimeType.Normal);

#if ARBOR_DOC_JA
		/// <summary>
		/// 移動完了した時のステート遷移(Onceのみ)<br />
		/// 遷移メソッド : OnStateUpdate
		/// </summary>
#else
		/// <summary>
		/// State transition at the time of movement completion(only Once)<br />
		/// Transition Method : OnStateUpdate
		/// </summary>
#endif
		[SerializeField] private StateLink _Done = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _TransformMoveOnWaypoint_SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_Type")]
		private MoveWaypointType _OldType = MoveWaypointType.Once;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_TimeType")]
		private WaypointTimeType _OldTimeType = WaypointTimeType.Normal;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		private MoveOnWaypointProcessor _Processor = new MoveOnWaypointProcessor();

		// Use this for enter state
		public override void OnStateBegin()
		{
			Transform target = cachedTransform;
			if (target != null)
			{
				_Processor.Play(_Waypoint.value as Waypoint, target.position, _Offset.value, _Type.value, WaypointTimeUtility.ToTimeType(_TimeType.value), _Speed.value, _ClearDestPoint.value);
			}
		}

		// Update is called once per frame
		public override void OnStateUpdate()
		{
			Transform target = cachedTransform;
			if (target != null && _Processor.isPlaying)
			{
				_Processor.Update();

				target.position = _Processor.currentPosition;

				if (_Processor.isDone)
				{
					Transition(_Done);
				}
			}
		}

		protected override void OnGraphPause()
		{
			base.OnGraphPause();

			_Processor.Pause();
		}

		protected override void OnGraphResume()
		{
			base.OnGraphResume();

			_Processor.Resume();
		}

		public override void OnStateEnd()
		{
			base.OnStateEnd();

			_Processor.Stop();
		}

		protected override void Reset()
		{
			base.Reset();

			_TransformMoveOnWaypoint_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Type = (FlexibleMoveWayppintType)_OldType;
			_TimeType = (FlexibleWaypointTimeType)_OldTimeType;
		}

		void Serialize()
		{
			while (_TransformMoveOnWaypoint_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_TransformMoveOnWaypoint_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_TransformMoveOnWaypoint_SerializeVersion++;
						break;
					default:
						_TransformMoveOnWaypoint_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		public override void OnAfterDeserialize()
		{
			base.OnAfterDeserialize();

			Serialize();
		}

		public override void OnBeforeSerialize()
		{
			base.OnBeforeSerialize();

			Serialize();
		}
	}
}