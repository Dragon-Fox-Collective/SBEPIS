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
	/// AnimatorにアクセスするためのStateBehaviour基本クラス
	/// </summary>
#else
	/// <summary>
	/// StateBehaviour base class to access Animator
	/// </summary>
#endif
	[AddComponentMenu("")]
	[HideBehaviour()]
	public class AnimatorBase : StateBehaviour, INodeBehaviourSerializationCallbackReceiver
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象のAnimator
		/// </summary>
#else
		/// <summary>
		/// Animator of target
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Animator))]
		private FlexibleComponent _Animator = new FlexibleComponent(FlexibleHierarchyType.Self);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion;

		#region old

		[FormerlySerializedAs("animator")]
		[SerializeField]
		[HideInInspector]
		protected Animator _OldAnimator;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 2;

		public Animator cachedAnimator
		{
			get
			{
				return _Animator.value as Animator;
			}
		}

		protected virtual void Reset()
		{
			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Animator = (FlexibleComponent)_OldAnimator;
		}

		void SerializeVer2()
		{
			_Animator.SetHierarchyIfConstantNull();
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

		public virtual void OnBeforeSerialize()
		{
			Serialize();
		}

		public virtual void OnAfterDeserialize()
		{
			Serialize();
		}
	}
}
