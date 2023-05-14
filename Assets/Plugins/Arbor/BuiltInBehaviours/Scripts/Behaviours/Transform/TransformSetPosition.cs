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
	/// Transformの位置を設定する。
	/// </summary>
#else
	/// <summary>
	/// Set the position of Transform.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/TransformSetPosition")]
	[BuiltInBehaviour]
	public sealed class TransformSetPosition : TransformBehaviourBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 更新タイミング
		/// </summary>
#else
		/// <summary>
		/// Update timing
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(ExecuteMethodFlags))]
		private FlexibleExecuteMethodFlags _UpdateTiming = new FlexibleExecuteMethodFlags(ExecuteMethodFlags.Enter);

#if ARBOR_DOC_JA
		/// <summary>
		/// 位置の座標空間
		/// </summary>
#else
		/// <summary>
		/// Coordinate space of position
		/// </summary>
#endif
		[SerializeField]
		private FlexibleSpace _Space = new FlexibleSpace(Space.World);

#if ARBOR_DOC_JA
		/// <summary>
		/// 位置
		/// </summary>
#else
		/// <summary>
		/// Position
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _Position = new FlexibleVector3();

		[SerializeField]
		[HideInInspector]
		private int _TransformSetPosition_SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_UpdateTiming")]
		private ExecuteMethodFlags _OldUpdateTiming = ExecuteMethodFlags.Enter;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_Space")]
		private Space _OldSpace = Space.World;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		void UpdateTransform(ExecuteMethodFlags updateTiming)
		{
			if ((_UpdateTiming.value & updateTiming) == 0)
			{
				return;
			}

			Vector3 position = _Position.value;
			Transform target = cachedTransform;
			if (target != null)
			{
				switch (_Space.value)
				{
					case Space.World:
						target.position = position;
						break;
					case Space.Self:
						target.localPosition = position;
						break;
				}
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			UpdateTransform(ExecuteMethodFlags.Enter);
		}

		public override void OnStateUpdate()
		{
			UpdateTransform(ExecuteMethodFlags.Update);
		}

		public override void OnStateLateUpdate()
		{
			UpdateTransform(ExecuteMethodFlags.LateUpdate);
		}

		public override void OnStateEnd()
		{
			UpdateTransform(ExecuteMethodFlags.Leave);
		}

		public override void OnStateFixedUpdate()
		{
			UpdateTransform(ExecuteMethodFlags.FixedUpdate);
		}

		protected override void Reset()
		{
			base.Reset();

			_TransformSetPosition_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_UpdateTiming = (FlexibleExecuteMethodFlags)_OldUpdateTiming;
			_Space = (FlexibleSpace)_OldSpace;
		}

		void Serialize()
		{
			while (_TransformSetPosition_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_TransformSetPosition_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_TransformSetPosition_SerializeVersion++;
						break;
					default:
						_TransformSetPosition_SerializeVersion = kCurrentSerializeVersion;
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