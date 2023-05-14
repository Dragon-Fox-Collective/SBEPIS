//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
using UnityEngine;
using UnityEngine.Serialization;

namespace Arbor.StateMachine.StateBehaviours
{
	using Arbor.Utilities;

#if ARBOR_DOC_JA
	/// <summary>
	/// Animatorのステートを参照して遷移する。
	/// </summary>
#else
	/// <summary>
	/// Transit by referring to the state of Animator.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Transition/AnimatorStateTransition")]
	[BuiltInBehaviour]
	public sealed class AnimatorStateTransition : AnimatorBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// レイヤー名。
		/// </summary>
#else
		/// <summary>
		/// Layer Name
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _LayerName = new FlexibleString(string.Empty);

#if ARBOR_DOC_JA
		/// <summary>
		/// ステート名。
		/// </summary>
#else
		/// <summary>
		/// State Name
		/// </summary>
#endif
		[SerializeField]
		private FlexibleString _StateName = new FlexibleString(string.Empty);

#if ARBOR_DOC_JA
		/// <summary>
		/// 遷移先ステート。<br />
		/// 遷移メソッド : OnStateBegin, OnStateUpdate
		/// </summary>
#else
		/// <summary>
		/// Transition destination state.<br />
		/// Transition Method : OnStateBegin, OnStateUpdate
		/// </summary>
#endif
		[SerializeField]
		[FormerlySerializedAs("nextState")]
		private StateLink _NextState = new StateLink();

		[SerializeField]
		[HideInInspector]
		private int _AnimatorStateTransition_SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("layerName")]
		[FormerlySerializedAs("_LayerName")]
		[HideInInspector]
		private string _OldLayerName = string.Empty;

		[SerializeField]
		[FormerlySerializedAs("stateName")]
		[FormerlySerializedAs("_StateName")]
		[HideInInspector]
		private string _OldStateName = string.Empty;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		void CheckTransition()
		{
			Animator animator = cachedAnimator;
			if (animator != null)
			{
				string layerName = _LayerName.value;
				string stateName = _StateName.value;

				int layerIndex = AnimatorUtility.GetLayerIndex(animator, layerName);
				if (layerIndex >= 0)
				{
					AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(layerIndex);
					if (stateInfo.IsName(layerName + "." + stateName))
					{
						Transition(_NextState);
					}
				}
			}
		}

		// Use this for enter state
		public override void OnStateBegin()
		{
			CheckTransition();
		}

		// Update is called once per frame
		public override void OnStateUpdate()
		{
			CheckTransition();
		}

		protected override void Reset()
		{
			base.Reset();

			_AnimatorStateTransition_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_LayerName = (FlexibleString)_OldLayerName;
			_StateName = (FlexibleString)_OldStateName;
		}

		void Serialize()
		{
			while (_AnimatorStateTransition_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_AnimatorStateTransition_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_AnimatorStateTransition_SerializeVersion++;
						break;
					default:
						_AnimatorStateTransition_SerializeVersion = kCurrentSerializeVersion;
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
