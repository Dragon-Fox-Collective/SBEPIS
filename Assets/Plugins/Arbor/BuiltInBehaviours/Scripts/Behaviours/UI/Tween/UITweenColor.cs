//-----------------------------------------------------
//            Arbor 3: FSM & BT Graph Editor
//		  Copyright(c) 2014-2021 caitsithware
//-----------------------------------------------------
#if ARBOR_SUPPORT_UGUI
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Serialization;

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
	[AddBehaviourMenu("UI/Tween/UITweenColor")]
	[BuiltInBehaviour]
	public sealed class UITweenColor : TweenBase
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
		/// 色の変化の指定。
		/// </summary>
#else
		/// <summary>
		/// Specifying the color change.
		/// </summary>
#endif
		[SerializeField] private FlexibleGradient _Gradient = new FlexibleGradient(new Gradient());

		[SerializeField]
		[HideInInspector]
		private int _SerializeVersion = 0;

		#region old

		[SerializeField]
		[FormerlySerializedAs("_Target")]
		[HideInInspector]
		private Graphic _OldTarget = null;

		[SerializeField]
		[FormerlySerializedAs("_Gradient")]
		[HideInInspector]
		private Gradient _OldGradient = new Gradient();

		#endregion // old

		#endregion // Serialize fields

		private const int kCurrentSerializeVersion = 3;

		private Graphic _CachedTarget;

		private Gradient _CachedGradient;

		protected override void Reset()
		{
			base.Reset();

			_SerializeVersion = kCurrentSerializeVersion;
		}

		void SerializeVer1()
		{
			_Target = (FlexibleComponent)_OldTarget;
		}

		void SerializeVer2()
		{
			_Gradient = (FlexibleGradient)_OldGradient;
		}

		void SerializeVer3()
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

		protected override void OnTweenBegin()
		{
			base.OnTweenBegin();

			_CachedTarget = _Target.value as Graphic;

			if (_CachedTarget == null)
			{
				return;
			}

			_CachedGradient = _Gradient.value;
			if (_CachedGradient == null)
			{
				_CachedGradient = new Gradient();
			}
		}

		protected override void OnTweenUpdate(float factor)
		{
			if (_CachedTarget != null)
			{
				_CachedTarget.color = _CachedGradient.Evaluate(factor);
			}
		}
	}
}
#endif
