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
	/// Rigidbody2Dの位置を徐々に変化させる。
	/// </summary>
#else
	/// <summary>
	/// Gradually change position of Rigidbody2D.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenRigidbody2DPosition")]
	[BuiltInBehaviour]
	public sealed class TweenRigidbody2DPosition : TweenBase
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
		/// 開始位置。
		/// </summary>
#else
		/// <summary>
		/// Starting position.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector2 _From = new FlexibleVector2();

#if ARBOR_DOC_JA
		/// <summary>
		/// 目標位置。
		/// </summary>
#else
		/// <summary>
		/// Target position.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector2 _To = new FlexibleVector2();

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion;

		#region old

		[SerializeField, FormerlySerializedAs("_Target")]
		[HideInInspector]
		private Rigidbody2D _OldTarget = null;

		[SerializeField, FormerlySerializedAs("_From")]
		[HideInInspector]
		private Vector2 _OldFrom = Vector2.zero;

		[SerializeField, FormerlySerializedAs("_To")]
		[HideInInspector]
		private Vector2 _OldTo = Vector2.zero;

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
			_From = (FlexibleVector2)_OldFrom;
			_To = (FlexibleVector2)_OldTo;
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

		private Vector2 _CachedFromValue;
		private Vector2 _CachedToValue;

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

			Vector2 startPosition = _CachedTarget.position;

			switch (_TweenMoveType.value)
			{
				case TweenMoveType.Absolute:
					break;
				case TweenMoveType.Relative:
					_CachedFromValue += startPosition;
					_CachedToValue += startPosition;
					break;
				case TweenMoveType.ToAbsolute:
					_CachedFromValue = startPosition;
					break;
			}
		}

		protected override void OnTweenUpdate(float factor)
		{
			if (_CachedTarget != null)
			{
				_CachedTarget.MovePosition(Vector2.Lerp(_CachedFromValue, _CachedToValue, factor));
			}
		}
	}
}
