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
	/// Rendererのアクティブを切り替える。
	/// </summary>
#else
	/// <summary>
	/// Activate Renderer.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Renderer/ActivateRenderer")]
	[BuiltInBehaviour]
	public sealed class ActivateRenderer : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// アクティブを切り替えるRenderer
		/// </summary>
#else
		/// <summary>
		/// Renderer to switch the active.
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Renderer))]
		private FlexibleComponent _Target = new FlexibleComponent(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// ステート開始時のアクティブ切り替え。
		/// </summary>
#else
		/// <summary>
		/// Active switching at the state start.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _BeginActive = new FlexibleBool(false);

#if ARBOR_DOC_JA
		/// <summary>
		/// ステート終了時のアクティブ切り替え。
		/// </summary>
#else
		/// <summary>
		/// Active switching at the state end.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleBool _EndActive = new FlexibleBool(false);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_BeginActive")]
		[HideInInspector]
		private bool _OldBeginActive = false;

		[SerializeField]
		[FormerlySerializedAs("_EndActive")]
		[HideInInspector]
		private bool _OldEndActive = false;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		public Renderer target
		{
			get
			{
				return _Target.value as Renderer;
			}
		}

		public override void OnStateBegin()
		{
			if (target != null)
			{
				target.enabled = _BeginActive.value;
			}
		}

		public override void OnStateEnd()
		{
			if (target != null)
			{
				target.enabled = _EndActive.value;
			}
		}

		void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_BeginActive = (FlexibleBool)_OldBeginActive;
			_EndActive = (FlexibleBool)_OldEndActive;
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
					default:
						_SerializeVersion = kCurrentSerializeVersion;
						break;
				}
			}
		}

		void INodeBehaviourSerializationCallbackReceiver.OnBeforeSerialize()
		{
			Serialize();
		}

		void INodeBehaviourSerializationCallbackReceiver.OnAfterDeserialize()
		{
			Serialize();
		}
	}
}