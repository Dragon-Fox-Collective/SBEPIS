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
	/// Transformのスケールを設定する。
	/// </summary>
#else
	/// <summary>
	/// Set the scale of Transform.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transform/TransformSetScale")]
	[BuiltInBehaviour]
	public sealed class TransformSetScale : TransformBehaviourBase
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
		/// スケール
		/// </summary>
#else
		/// <summary>
		/// Scale
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _Scale = new FlexibleVector3(Vector3.one);

		[SerializeField]
		[HideInInspector]
		private int _TransfromSetScale_SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_UpdateTiming")]
		private ExecuteMethodFlags _OldUpdateTiming = ExecuteMethodFlags.Enter;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		void UpdateTransform(ExecuteMethodFlags updateTiming)
		{
			if ((_UpdateTiming.value & updateTiming) == 0)
			{
				return;
			}

			Vector3 scale = _Scale.value;
			Transform target = cachedTransform;
			if (target != null)
			{
				target.localScale = scale;
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

			_TransfromSetScale_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_UpdateTiming = (FlexibleExecuteMethodFlags)_OldUpdateTiming;
		}

		void Serialize()
		{
			while (_TransfromSetScale_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_TransfromSetScale_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_TransfromSetScale_SerializeVersion++;
						break;
					default:
						_TransfromSetScale_SerializeVersion = kCurrentSerializeVersion;
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