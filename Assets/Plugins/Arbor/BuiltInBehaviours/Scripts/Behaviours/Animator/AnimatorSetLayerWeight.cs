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
	/// Animatorのレイヤーのウェイトを設定する。
	/// </summary>
#else
	/// <summary>
	/// Set weight of Animator's layer.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Animator/AnimatorSetLayerWeight")]
	[BuiltInBehaviour]
	public sealed class AnimatorSetLayerWeight : AnimatorBase
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
		/// ウェイト
		/// </summary>
#else
		/// <summary>
		/// Weight
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _Weight = new FlexibleFloat(0f);

		[SerializeField]
		[HideInInspector]
		private int _AnimatorSetLayerWeight_SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("layerName")]
		[FormerlySerializedAs("_LayerName")]
		[HideInInspector]
		private string _OldLayerName = string.Empty;

		[SerializeField]
		[FormerlySerializedAs("weight")]
		[FormerlySerializedAs("_Weight")]
		[HideInInspector]
		private float _OldWeight = 0f;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 1;

		// Use this for enter state
		public override void OnStateBegin()
		{
			Animator animator = cachedAnimator;
			if (animator != null)
			{
				animator.SetLayerWeight(AnimatorUtility.GetLayerIndex(animator, _LayerName.value), _Weight.value);
			}
		}

		protected override void Reset()
		{
			base.Reset();

			_AnimatorSetLayerWeight_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_LayerName = (FlexibleString)_OldLayerName;
			_Weight = (FlexibleFloat)_OldWeight;
		}

		void Serialize()
		{
			while (_AnimatorSetLayerWeight_SerializeVersion != kCurrentSerializeVersion)
			{
				switch (_AnimatorSetLayerWeight_SerializeVersion)
				{
					case 0:
						SerializeVer1();
						_AnimatorSetLayerWeight_SerializeVersion++;
						break;
					default:
						_AnimatorSetLayerWeight_SerializeVersion = kCurrentSerializeVersion;
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