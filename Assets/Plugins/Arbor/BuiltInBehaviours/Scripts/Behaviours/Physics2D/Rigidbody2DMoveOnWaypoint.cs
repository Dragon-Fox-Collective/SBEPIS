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
	/// Waypointに沿ってRigidbody2Dを移動させる。
	/// </summary>
#else
	/// <summary>
	/// Move the Rigidbody2D along the Waypoint.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Physics2D/Rigidbody2DMoveOnWaypoint")]
	[BuiltInBehaviour]
	public sealed class Rigidbody2DMoveOnWaypoint : Rigidbody2DBehaviourBase
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
		/// Rigidbody2Dを移動させる経路
		/// </summary>
#else
		/// <summary>
		/// Route to move Rigidbody2D
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
		private FlexibleVector2 _Offset = new FlexibleVector2();

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
		private int _Rigidbody2DMoveOnWaypoint_SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_Type")]
		private MoveWaypointType _OldType = MoveWaypointType.Once;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		private MoveOnWaypointProcessor _Processor = new MoveOnWaypointProcessor();
		private bool _IsDone = false;

		// Use this for enter state
		public override void OnStateBegin()
		{
			_IsDone = false;
			Rigidbody2D target = cachedTarget;
			if (target != null)
			{
				_Processor.Play(_Waypoint.value as Waypoint, target.position, _Offset.value, _Type.value, TimeType.FixedTime, _Speed.value, _ClearDestPoint.value);
			}
		}

		// OnStateUpdate is called once per frame
		public override void OnStateUpdate()
		{
			if (_IsDone)
			{
				Transition(_Done);
				_IsDone = false;
			}
		}

		public override void OnStateFixedUpdate()
		{
			Rigidbody2D target = cachedTarget;
			if (target != null && _Processor.isPlaying)
			{
				_Processor.Update();

				target.MovePosition(_Processor.currentPosition);

				_IsDone = _Processor.isDone;
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

			_Rigidbody2DMoveOnWaypoint_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Type = (FlexibleMoveWayppintType)_OldType;
		}

		void Serialize()
		{
			while (_Rigidbody2DMoveOnWaypoint_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_Rigidbody2DMoveOnWaypoint_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_Rigidbody2DMoveOnWaypoint_SerializeVersion++;
						break;
					default:
						_Rigidbody2DMoveOnWaypoint_SerializeVersion = kCurrentSerializeVersion;
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