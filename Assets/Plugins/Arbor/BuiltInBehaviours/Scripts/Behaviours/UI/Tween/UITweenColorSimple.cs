//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ARBOR_SUPPORT_UGUI
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Arbor.StateMachine.StateBehaviours
{
#if ARBOR_DOC_JA
	/// <summary>
	/// UIの色を徐々に変化させる。
	/// </summary>
#else
	/// <summary>
	/// Gradually change color of UI.
	/// </summary>
#endif
	[AddComponentMenu("")]
	[AddBehaviourMenu("UI/Tween/UITweenColorSimple")]
	[BuiltInBehaviour]
	public sealed class UITweenColorSimple : TweenBase
	{
		#region Serialize fields

#if ARBOR_DOC_JA
		/// <summary>
		/// 対象となるGraphic。
		/// </summary>
#else
		/// <summary>
		/// Graphic of interest.
		/// </summary>
#endif
		[SerializeField]
		[SlotType(typeof(Graphic))]
		private FlexibleComponent _Target = new FlexibleComponent(FlexibleHierarchyType.Self);

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
		/// 開始色。
		/// </summary>
#else
		/// <summary>
		/// Start color.
		/// </summary>
#endif
		[SerializeField] private FlexibleColor _From = new FlexibleColor(Color.white);

#if ARBOR_DOC_JA
		/// <summary>
		/// 目標色。
		/// </summary>
#else
		/// <summary>
		/// Target color.
		/// </summary>
#endif
		[SerializeField] private FlexibleColor _To = new FlexibleColor(Color.white);

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[HideInInspector]
		[FormerlySerializedAs("_TweenMoveType")]
		private TweenMoveType _OldTweenMoveType = TweenMoveType.Absolute;

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 2;

		private Graphic _CachedTarget;

		private Color _CachedFromValue;
		private Color _CachedToValue;

		protected override void Reset()
		{
			base.Reset();

			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Target.SetHierarchyIfConstantNull();
		}

		void SerializeVer2()
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

		protected override void OnTweenBegin()
		{
			_CachedTarget = _Target.value as Graphic;

			if (_CachedTarget == null)
			{
				return;
			}

			_CachedFromValue = _From.value;
			_CachedToValue = _To.value;

			Color startColor = _CachedTarget.color;

			switch (_TweenMoveType.value)
			{
				case TweenMoveType.Absolute:
					break;
				case TweenMoveType.Relative:
					_CachedFromValue += startColor;
					_CachedToValue += startColor;
					break;
				case TweenMoveType.ToAbsolute:
					_CachedFromValue = startColor;
					break;
			}
		}

		protected override void OnTweenUpdate(float factor)
		{
			if (_CachedTarget != null)
			{
				_CachedTarget.color = Color.Lerp(_CachedFromValue, _CachedToValue, factor);
			}
		}
	}
}
#endif
