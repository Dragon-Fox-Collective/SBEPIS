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
	/// Transformの回転を設定する。
	/// </summary>
#else
	/// <summary>
	/// Set the rotation of Transform.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/TransformSetRotation")]
	[BuiltInBehaviour]
	public sealed class TransformSetRotation : TransformBehaviourBase
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
		/// 回転の座標空間
		/// </summary>
#else
		/// <summary>
		/// Coordinate space of rotation
		/// </summary>
#endif
		[SerializeField]
		private FlexibleSpace _Space = new FlexibleSpace(Space.World);

#if ARBOR_DOC_JA
		/// <summary>
		/// 回転
		/// </summary>
#else
		/// <summary>
		/// Rotation
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _Rotation = new FlexibleVector3();

		[SerializeField]
		[HideInInspector]
		private int _TransformSetRotation_SerializeVersion = 0;

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

			Quaternion rotation = Quaternion.Euler(_Rotation.value);
			Transform target = cachedTransform;
			if (target != null)
			{
				switch (_Space.value)
				{
					case Space.World:
						target.rotation = rotation;
						break;
					case Space.Self:
						target.localRotation = rotation;
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

			_TransformSetRotation_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_UpdateTiming = (FlexibleExecuteMethodFlags)_OldUpdateTiming;
			_Space = (FlexibleSpace)_OldSpace;
		}

		void Serialize()
		{
			while (_TransformSetRotation_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_TransformSetRotation_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_TransformSetRotation_SerializeVersion++;
						break;
					default:
						_TransformSetRotation_SerializeVersion = kCurrentSerializeVersion;
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