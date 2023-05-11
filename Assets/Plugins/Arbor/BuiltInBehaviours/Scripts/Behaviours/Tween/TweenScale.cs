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
	/// スケールを徐々に変化させる。
	/// </summary>
#else
	/// <summary>
	/// Gradually change scale.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("Tween/TweenScale")]
	[BuiltInBehaviour]
	public sealed class TweenScale : TweenBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象となるTransform。
		/// </summary>
#else
		/// <summary>
		/// Transform of interest.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleTransform _Target = new FlexibleTransform(FlexibleHierarchyType.Self);

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
		/// 開始スケール。
		/// </summary>
#else
		/// <summary>
		/// Start scale.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _From = new FlexibleVector3(Vector3.one);

#if ARBOR_DOC_JA
		/// <summary>
		/// 目標スケール。
		/// </summary>
#else
		/// <summary>
		/// Goal scale.
		/// </summary>
#endif
		[SerializeField]
		private FlexibleVector3 _To = new FlexibleVector3(Vector3.one);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion;

		#region old

		[SerializeField, FormerlySerializedAs("_Target")]
		[HideInInspector]
		private Transform _OldTarget = null;

		[SerializeField, FormerlySerializedAs("_From")]
		[HideInInspector]
		private Vector3 _OldFrom = Vector3.one;

		[SerializeField, FormerlySerializedAs("_To")]
		[HideInInspector]
		private Vector3 _OldTo = Vector3.one;

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
			_Target = (FlexibleTransform)_OldTarget;
			_From = (FlexibleVector3)_OldFrom;
			_To = (FlexibleVector3)_OldTo;
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

		Transform _CachedTarget;

		private Vector3 _CachedFromValue;
		private Vector3 _CachedToValue;

		protected override void OnTweenBegin()
		{
			_CachedTarget = _Target.value;

			if (_CachedTarget == null)
			{
				return;
			}

			_CachedFromValue = _From.value;
			_CachedToValue = _To.value;

			Vector3 startScale = _CachedTarget.localScale;

			switch (_TweenMoveType.value)
			{
				case TweenMoveType.Absolute:
					break;
				case TweenMoveType.Relative:
					_CachedFromValue += startScale;
					_CachedToValue += startScale;
					break;
				case TweenMoveType.ToAbsolute:
					_CachedFromValue = startScale;
					break;
			}
		}

		protected override void OnTweenUpdate(float factor)
		{
			if (_CachedTarget != null)
			{
				_CachedTarget.localScale = Vector3.Lerp(_CachedFromValue, _CachedToValue, factor);
			}
		}
	}
}
