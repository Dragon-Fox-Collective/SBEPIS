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
	/// Rigidbody2Dの向きを徐々に変化させる。
	/// </summary>
#else
	/// <summary>
	/// Gradually change rotaion of Rigidbody2D.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenRigidbody2DRotation")]
	[BuiltInBehaviour]
	public sealed class TweenRigidbody2DRotation : TweenBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象となるRigidbody2D。
		/// </summary>
#else
		/// <summary>
		/// Rigidbody2D of interest.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleRigidbody2D _Target = new FlexibleRigidbody2D(FlexibleHierarchyType.Self);

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始した状態からの相対的な変化かどうか。
		/// </summary>
#else
		/// <summary>
		/// Whether the relative change from the start state.
		/// </summary>
#endif
		[SerializeField]
		[Internal.DocumentType(typeof(TweenMoveType))]
		private FlexibleTweenMoveType _TweenMoveType = new FlexibleTweenMoveType(TweenMoveType.Absolute);

#if ARBOR_DOC_JA
		/// <summary>
		/// 開始向き。
		/// </summary>
#else
		/// <summary>
		/// Start orientation.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _From = new FlexibleFloat();

#if ARBOR_DOC_JA
		/// <summary>
		/// 目標向き。
		/// </summary>
#else
		/// <summary>
		/// Goal orientation.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleFloat _To = new FlexibleFloat();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion;

		#region old

		[SerializeField, FormerlySerializedAs("_Target")]
		[HideInInspector]
		private Rigidbody2D _OldTarget = null;

		[SerializeField, FormerlySerializedAs("_From")]
		[HideInInspector]
		private float _OldFrom = 0f;

		[SerializeField, FormerlySerializedAs("_To")]
		[HideInInspector]
		private float _OldTo = 0f;

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_Relative")]
		[FormerlySerializedAs("_TweenMoveType")]
		private TweenMoveType _OldTweenMoveType = TweenMoveType.Absolute;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 3;

		protected override void Reset()
		{
			base.Reset();

			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Target = (FlexibleRigidbody2D)_OldTarget;
			_From = (FlexibleFloat)_OldFrom;
			_To = (FlexibleFloat)_OldTo;
		}

		void SerializeVer2()
		{
			_Target.SetHierarchyIfConstantNull();
		}

		void SerializeVer3()
		{
			_TweenMoveType = (FlexibleTweenMoveType)_OldTweenMoveType;
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
					case 2:
						SerializeVer3();
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

		Rigidbody2D _CachedTarget;

		private float _CachedFromValue;
		private float _CachedToValue;

		public override bool fixedUpdate
		{
			get
			{
				return true;
			}
		}

		protected override void OnTweenBegin()
		{
			_CachedTarget = _Target.value;

			if (_CachedTarget == null)
			{
				return;
			}

			_CachedFromValue = _From.value;
			_CachedToValue = _To.value;

			float startRotation = _CachedTarget.rotation;

			switch (_TweenMoveType.value)
			{
				case TweenMoveType.Absolute:
					break;
				case TweenMoveType.Relative:
					_CachedFromValue += startRotation;
					_CachedToValue += startRotation;
					break;
				case TweenMoveType.ToAbsolute:
					_CachedFromValue = startRotation;
					break;
			}
		}

		protected override void OnTweenUpdate(float factor)
		{
			if (_CachedTarget != null)
			{
				_CachedTarget.MoveRotation(Mathf.Lerp(_CachedFromValue, _CachedToValue, factor));
			}
		}
	}
}
