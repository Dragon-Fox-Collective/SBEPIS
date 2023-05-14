//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// 指定したTransformの位置でサウンドを再生する。
	/// </summary>
#else
	/// <summary>
	/// Play the sound at the specified Transform position.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Audio/PlaySoundAtTransform")]
	[BuiltInBehaviour]
	public sealed class PlaySoundAtTransform : PlaySoundAtPointBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 再生する位置。
		/// </summary>
#else
		/// <summary>
		/// Position to play.
		/// </summary>
#endif
		[SerializeField] private FlexibleTransform _Target = new FlexibleTransform(FlexibleHierarchyType.Self);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		public Transform cachedTarget
		{
			get
			{
				return _Target.value;
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			Transform target = cachedTarget;

			if (target != null)
			{
				PlayClipAtPoint(target.position);
			}
		}

		protected override void Reset()
		{
			base.Reset();

			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Target.SetHierarchyIfConstantNull();
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

		public override void OnBeforeSerialize()
		{
			base.OnBeforeSerialize();

			Serialize();
		}

		public override void OnAfterDeserialize()
		{
			base.OnAfterDeserialize();

			Serialize();
		}
	}
}
